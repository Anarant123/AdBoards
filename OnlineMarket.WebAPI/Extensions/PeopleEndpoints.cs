using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AdBoards.Domain.Enums;
using AdBoardsWebAPI.Auth;
using AdBoardsWebAPI.Contracts.Requests.Models;
using AdBoardsWebAPI.Contracts.Responses;
using AdBoardsWebAPI.Data;
using AdBoardsWebAPI.Data.Models;
using AdBoardsWebAPI.Options;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MimeKit;
using MimeKit.Text;

namespace AdBoardsWebAPI.Extensions;

public static class PeopleEndpoints
{
    private static Error? ValidateBirthday(DateOnly birthday)
    {
        var minDate = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-150));
        var maxDate = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-14));

        if (birthday < minDate || birthday > maxDate)
        {
            return new Error
            {
                Reason = "birthday_min_max",
                Message = "Возраст должен быть от 14 до 150 лет"
            };
        }

        return null;
    }

    private static async Task<Error?> ValidateEmail(string email, OnlineMarketContext context)
    {
        if (await context.People.AnyAsync(x => x.Email == email))
        {
            return new Error
            {
                Reason = "email_unique",
                Message = "Указанный email уже занят"
            };
        }

        return null;
    }

    public static IEndpointRouteBuilder MapPeopleEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("People").WithTags("People");

        group.MapGet("GetPeople", async (OnlineMarketContext context) =>
        {
            var people = await context.People.Include(x => x.Role).ToListAsync();
            return people.Count == 0 ? Results.NotFound() : Results.Ok(people);
        }).RequireAuthorization(Policies.Admin);

        group.MapGet("GetMe", async (OnlineMarketContext context, ClaimsPrincipal user) =>
        {
            var id = int.Parse(user.Claims.First(x => x.Type == "id").Value);
            return await context.People.Include(x => x.Role).FirstOrDefaultAsync(x => x.Id == id);
        });

        group.MapGet("GetCountOfClient", async (OnlineMarketContext context) =>
        {
            var count = await context.People.CountAsync();
            return count == 0 ? Results.NotFound() : Results.Ok(count);
        }).RequireAuthorization(Policies.Admin);

        group.MapGet("Authorization", async (string login, string password, OnlineMarketContext context,
            IOptions<JwtOptions> jwtOptions) =>
        {
            var person = await context.People
                .Include(x => x.Role)
                .FirstOrDefaultAsync(x => x.Login == login && x.Password == password);
            if (person is null) return Results.BadRequest();

            var key = Encoding.ASCII.GetBytes(jwtOptions.Value.Key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, person.Id.ToString()),
                    new Claim("id", person.Id.ToString()),
                    new Claim("email", person.Email),
                    new Claim("login", person.Login),
                    new Claim("role", person.RoleId.ToString())
                }, "jwt"),
                Expires = DateTime.UtcNow.AddDays(7),
                Issuer = jwtOptions.Value.Issuer,
                Audience = jwtOptions.Value.Audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha512Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var stringToken = tokenHandler.WriteToken(token);

            return Results.Ok(new { person, Token = stringToken });
        }).AllowAnonymous();

        group.MapPost("Registration", async (RegisterModel model, OnlineMarketContext context, FileManager fileManager) =>
        {
            var p = new Person
            {
                Login = model.Login.Trim(),
                Password = model.Password,
                Name = model.Name?.Trim(),
                City = model.City?.Trim(),
                Birthday = DateOnly.FromDateTime(model.Birthday),
                Phone = model.Phone.Trim(),
                Email = model.Email.Trim(),
                RoleId = RoleType.Normal,
                PhotoName = await fileManager.SaveUserPhoto(null)
            };

            var errors = new List<Error>(3);

            if (await context.People.AnyAsync(x => x.Login == p.Login))
            {
                errors.Add(new Error
                {
                    Reason = "login_unique",
                    Message = "Указанный логин уже занят"
                });
            }

            var emailValidationResult = await ValidateEmail(p.Email, context);
            if (emailValidationResult is not null) errors.Add(emailValidationResult);

            var birthdayValidationResult = ValidateBirthday(p.Birthday);
            if (birthdayValidationResult is not null) errors.Add(birthdayValidationResult);

            if (errors.Count != 0) return Results.BadRequest(errors);

            context.People.Add(p);

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                Console.WriteLine(e);
                return Results.BadRequest();
            }

            return Results.Ok(p);
        }).AllowAnonymous();

        group.MapPost("RecoveryPassword", async (OnlineMarketContext dbContext, IOptions<SmtpOptions> smtpOptions,
            string login) =>
        {
            var p = await dbContext.People.FirstOrDefaultAsync(x => x.Login == login);
            if (p is null) return Results.NotFound();

            var smtp = smtpOptions.Value;
            using var emailMessage = new MimeMessage
            {
                Subject = "Восстановление пароля",
                Body = new TextPart(TextFormat.Html)
                {
                    Text = "Ваш пароль от AdBoards: " + p.Password
                }
            };
            emailMessage.From.Add(new MailboxAddress("Администрация сайта", smtp.Address));
            emailMessage.To.Add(new MailboxAddress("", p.Email));

            using var client = new SmtpClient();

            try
            {
                await client.ConnectAsync(smtp.Host, smtp.Port, false);
                await client.AuthenticateAsync(smtp.Username, smtp.Password);
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);

                return Results.Ok();
            }
            catch
            {
                return Results.BadRequest();
            }
        }).AllowAnonymous();

        group.MapPut("Update", async (UpdatePersonModel model, OnlineMarketContext context, ClaimsPrincipal user) =>
        {
            var id = int.Parse(user.Claims.First(x => x.Type == "id").Value);

            var person = await context.People.Include(x => x.Role).FirstOrDefaultAsync(x => x.Id == id);
            if (person is null) return Results.NotFound();

            var errors = new List<Error>(3);
            var emailChanged = person.Email != model.Email?.Trim();

            person.Name = model.Name?.Trim();
            person.City = model.City?.Trim();
            if (model.Birthday is not null) person.Birthday = DateOnly.FromDateTime(model.Birthday.Value);
            if (model.Phone is not null) person.Phone = model.Phone.Trim();
            if (model.Email is not null) person.Email = model.Email.Trim();

            var emailValidationResult = await ValidateEmail(person.Email, context);
            if (emailValidationResult is not null && emailChanged) errors.Add(emailValidationResult);

            var birthdayValidationResult = ValidateBirthday(person.Birthday);
            if (birthdayValidationResult is not null) errors.Add(birthdayValidationResult);

            if (errors.Count != 0) return Results.BadRequest(errors);

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                Console.WriteLine(e);
                return Results.BadRequest();
            }

            return Results.Ok(person);
        });

        group.MapPut("Photo", async (IFormFile? photo, OnlineMarketContext context, FileManager fileManager,
            ClaimsPrincipal user) =>
        {
            var id = int.Parse(user.Claims.First(x => x.Type == "id").Value);

            var person = await context.People.Include(x => x.Role).FirstOrDefaultAsync(x => x.Id == id);
            if (person is null) return Results.NotFound();

            person.PhotoName = await fileManager.SaveUserPhoto(photo);

            await context.SaveChangesAsync();

            return Results.Ok(person);
        });

        group.MapDelete("Delete", async (string login, OnlineMarketContext context) =>
        {
            var p = await context.People.FirstOrDefaultAsync(x => x.Login == login);
            if (p is null) return Results.NotFound();

            context.People.Remove(p);
            await context.SaveChangesAsync();

            return Results.Ok();
        }).RequireAuthorization(Policies.Admin);

        return app;
    }
}
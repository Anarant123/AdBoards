using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.EntityFrameworkCore;
using AdBoardsWebAPI.Models.db;
using AdBoardsWebAPI.DTO;
using System.Net;

namespace AdBoardsWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PeopleController : ControllerBase
    {
        private readonly AdBoardsContext _context;

        public PeopleController(AdBoardsContext context)
        {
            _context = context;
        }

        [HttpGet("GetPeople")]
        public async Task<ActionResult<Person>> GetPeople()
        {
            var people = _context.People.ToList();

            if (people.Count < 0)
            {
                return NotFound();
            }
            else
            {
                return Ok(people);
            }

        }

        [HttpGet("GetCountOfClient")]
        public async Task<ActionResult<Person>> GetCountOfClient()
        {
            int count = _context.People.Count();

            if (count == 0)
            {
                return NotFound();
            }
            else
            {
                return Ok(count);
            }

        }

        [HttpPost("Authorization")]
        public async Task<ActionResult<Person>> GetPerson(string login, string password)
        {
            var person = await _context.People.FirstOrDefaultAsync(x => x.Login == login);

            if (person == null)
            {
                return NotFound();
            }
            else
            {
                if (password == person.Password)
                    return Ok(person);
                else
                    return BadRequest();
            }

        }

        [HttpPost("Registration")]
        public async Task<ActionResult<Person>> AddPerson(PersonDTO person)
        {
            Person p = new Person();
            p.Login = person.Login;
            p.Password = person.Password;
            p.Name = person.Name;
            p.City = person.City;
            p.Birthday = person.Birthday;
            p.Phone = person.Phone;
            p.Email = person.Email;
            p.RightId = person.RightId;
            p.Photo = System.IO.File.ReadAllBytes("C:\\Учеба\\РПМ\\Проект\\AdBoardsWeb\\AdBoardsWeb\\wwwroot\\drawable\\icon_image_person.png");

            _context.People.Add(p);
            await _context.SaveChangesAsync();

            return Ok(p);
            //return CreatedAtAction(nameof(GetPerson), new { id = p.Id }, p);
        }

		[HttpPost("RecoveryPassword")]
		public async Task<ActionResult<Person>> RecoveryPassword(string Login)
		{
			Person p = _context.People.FirstOrDefault(x => x.Login == Login);
            if(p != null)
            {
				try
				{
					using var emailMessage = new MimeMessage();

					emailMessage.From.Add(new MailboxAddress("Администрация сайта", "safronov-laba11@mail.ru"));
					emailMessage.To.Add(new MailboxAddress("", p.Email));
					emailMessage.Subject = "Восстановление пароля";
                    emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                    {
                        Text = "Ваш пароль от AdBoards: " + p.Password
					};

					using (var client = new SmtpClient())
					{
						await client.ConnectAsync("smtp.mail.ru", 587, false);
						await client.AuthenticateAsync("safronov-laba11@mail.ru", "aG5qd0fNX8RSxKaiiad7");
						await client.SendAsync(emailMessage);

						await client.DisconnectAsync(true);
					}

					return Ok();
				}
				catch(Exception ex)
				{
                    Console.WriteLine("fffffffffffffffffffffffff  " + ex.ToString());
					return BadRequest();
				}
			}
            else
                return BadRequest();

			
		}

		[HttpPut("Update")]
        public async Task<ActionResult<Person>> UpdatePerson(PersonDTO person)
        {
            Person p = await _context.People.FirstAsync(x => x.Login == person.Login);

            p.Name = person.Name;
            p.City = person.City;
            p.Birthday = person.Birthday;
            p.Phone = person.Phone;
            p.Email = person.Email;
            if (person.Photo != null)
                p.Photo = person.Photo;

            await _context.SaveChangesAsync();

            return Ok(p);
        }

        [HttpDelete("Delete")]
        public async Task<ActionResult> DeletePerson(string Login)
        {
            Person p = await _context.People.FirstOrDefaultAsync(x => x.Login == Login);

            if (p != null)
            {
                var favorites = _context.Favorites.Where(x => x.PersonId == p.Id).ToList();
                foreach (var f in favorites)
                    _context.Favorites.Remove(f);
                var ads = _context.Ads.Where(x => x.PersonId == p.Id).ToList();
                foreach (var a in ads)
                    _context.Ads.Remove(a);

                _context.People.Remove(p);
                await _context.SaveChangesAsync();
                return Ok();
            }
            else
            {
                return NotFound();
            }

        }
    }
}

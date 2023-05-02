using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AdBoardsWebAPI.Models.db;
using AdBoardsWebAPI.DTO;

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

            _context.People.Add(p);
            await _context.SaveChangesAsync();

            return Ok(p);
            //return CreatedAtAction(nameof(GetPerson), new { id = p.Id }, p);
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
            p.Photo = person.Photo;

            await _context.SaveChangesAsync();

            return Ok(p);
        }
    }
}

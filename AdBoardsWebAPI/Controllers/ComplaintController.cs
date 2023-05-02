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
    public class ComplaintController : ControllerBase
    {
        private readonly AdBoardsContext _context;

        public ComplaintController(AdBoardsContext context)
        {
            _context = context;
        }


        [HttpPost("Addition")]
        public async Task<ActionResult> AddComplaint(int AdId, int PersonId)
        {
            Complaint c = new Complaint();
            c.AdId = AdId;
            c.PersonId = PersonId;

            _context.Complaints.Add(c);
            await _context.SaveChangesAsync();

            return Ok();
            //return CreatedAtAction(nameof(GetPerson), new { id = p.Id }, p);
        }

        [HttpDelete("Delete")]
        public async Task<ActionResult> DeleteComplaint(int AdId, int PersonId)
        {
            Complaint c = await _context.Complaints.FirstAsync(x => x.AdId == AdId && x.PersonId == PersonId);

            _context.Complaints.Remove(c);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}

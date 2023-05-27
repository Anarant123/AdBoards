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
            if (_context.Complaints.FirstOrDefault(x => x.AdId == AdId && x.PersonId == PersonId) != null)
                return BadRequest();

            Complaint c = new Complaint();
            c.AdId = AdId;
            c.PersonId = PersonId;

            _context.Complaints.Add(c);
            await _context.SaveChangesAsync();

            return Ok();
            //return CreatedAtAction(nameof(GetPerson), new { id = p.Id }, p);
        }

        [HttpDelete("Delete")]
        public async Task<ActionResult> DeleteComplaint(int AdId)
        {
            List<Complaint> complaints = _context.Complaints.Where(x => x.AdId == AdId).ToList();

            foreach (var c in complaints)
                _context.Complaints.Remove(c);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}

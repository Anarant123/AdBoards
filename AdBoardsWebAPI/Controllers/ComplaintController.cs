using Microsoft.AspNetCore.Mvc;
using AdBoardsWebAPI.Models.db;

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

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AdBoardsWebAPI.Models.db;

namespace AdBoardsWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FavoritesController : ControllerBase
    {
        private readonly AdBoardsContext _context;

        public FavoritesController(AdBoardsContext context)
        {
            _context = context;
        }

        [HttpGet("IsFavorite")]
        public ActionResult IsFavorite(int AdId, int PersonId)
        {
            if (_context.Favorites.FirstOrDefault(x => x.AdId == AdId && x.PersonId == PersonId) != null)
                return Ok();
            else
                return BadRequest();
        }

        [HttpPost("Addition")]
        public async Task<ActionResult> AddFavorite(int AdId, int PersonId)
        {
            if (_context.Favorites.FirstOrDefault(x => x.AdId == AdId && x.PersonId == PersonId) != null)
                return BadRequest();

            Favorite f = new Favorite();
            f.AdId = AdId;
            f.PersonId = PersonId;

            _context.Favorites.Add(f);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("Delete")]
        public async Task<ActionResult> DeleteFavorite(int AdId, int PersonId)
        {
            Favorite f = await _context.Favorites.FirstAsync(x => x.AdId == AdId && x.PersonId == PersonId);

            _context.Favorites.Remove(f);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}

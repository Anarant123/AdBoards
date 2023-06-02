using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AdBoardsWebAPI.Models.db;
using AdBoardsWebAPI.DTO;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace AdBoardsWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdsController : ControllerBase
    {
        private readonly AdBoardsContext _context;

        public AdsController(AdBoardsContext context)
        {
            _context = context;
        }

        [HttpGet("GetAd")]
        public ActionResult GetAd(int id)
        {
            var ads = _context.Ads.FirstOrDefault(x => x.Id == id);
            if (ads != null)
            {
                return Ok(ads);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet("GetAds")]
        public ActionResult GetAds()
        {
            var ads = _context.Ads.Include(x => x.Person).Include(x => x.Complaints).ToList();
            if (ads.Any())
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase, // Используйте это, если нужно преобразование в camelCase
                    ReferenceHandler = ReferenceHandler.Preserve
                };

                string json = JsonSerializer.Serialize(ads, options);

                return Ok(json);
            }
            else
            {
                return BadRequest();
            }    
        }

        [HttpGet("GetMyAds")]
        public ActionResult GetMyAds(int id)
        {
            
            var ads = _context.Ads.Where(x => x.PersonId == id).ToList();
            if (ads.Any())
            {
                return Ok(ads);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet("GetFavoritesAds")]
        public ActionResult GetFavoritesAds(int id)
        {
            List<Favorite> f = _context.Favorites.Include(e => e.Ad).Where(x => x.PersonId == id).ToList();
            List<Ad> ads = new List<Ad>();
            foreach (var fitem in f)
            {
                ads.Add(_context.Ads.Include(e => e.Person).First(x => x.Id == fitem.Ad.Id));
            }

            if (ads.Any())
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase, // Используйте это, если нужно преобразование в camelCase
                    ReferenceHandler = ReferenceHandler.Preserve
                };

                string json = JsonSerializer.Serialize(ads, options);

                return Ok(json);
            }
            else
            {
                return BadRequest();
            }
        }


        [HttpPost("Addition")]
        public async Task<ActionResult<Ad>> AddAd(AdDTO ad)
        {
            Ad a = new Ad();
            a.Price = ad.Price;
            a.Name = ad.Name;
            a.Description = ad.Description;
            a.City = ad.City;
            a.Photo = ad.Photo;
            a.Date = ad.Date;
            a.CotegorysId = ad.CotegorysId;
            a.PersonId = ad.PersonId;
            a.TypeOfAdId = ad.TypeOfAdId;

            _context.Ads.Add(a);
            await _context.SaveChangesAsync();

            return Ok(a);
        }

        [HttpPut("Update")]
        public async Task<ActionResult<Ad>> UpdateAd(AdDTO ad)
        {
            Ad a = await _context.Ads.FirstAsync(x => x.Id == ad.Id);

            a.Price = ad.Price;
            a.Name = ad.Name;
            a.Description = ad.Description;
            a.City = ad.City;
            a.Photo = ad.Photo;
            a.CotegorysId = ad.CotegorysId;
            a.TypeOfAdId = ad.TypeOfAdId;

            await _context.SaveChangesAsync();

            return Ok(a);
        }

        [HttpDelete("Delete")]
        public async Task<ActionResult<Ad>> DeleteAd(int id)
        {
            Ad a = await _context.Ads.FirstAsync(x => x.Id == id);
            List<Favorite> favorites = _context.Favorites.Where(x => x.AdId == id).ToList();
            List<Complaint> complaints = _context.Complaints.Where(x => x.AdId == id).ToList();

            foreach (Favorite favorite in favorites)
                _context.Favorites.Remove(favorite);

            foreach (Complaint complaint in complaints)
                _context.Complaints.Remove(complaint);

            _context.Ads.Remove(a);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}

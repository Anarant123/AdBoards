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
    public class AdsController : ControllerBase
    {
        private readonly AdBoardsContext _context;

        public AdsController(AdBoardsContext context)
        {
            _context = context;
        }

        [HttpGet("GetAds")]
        public ActionResult GetAds()
        {
            var ads = _context.Ads.ToList();
            if (ads.Any())
            {
                return Ok(ads);
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

        [HttpGet("GetMyAds")]
        public ActionResult GetFavoritesAds(int id)
        {
            Person person = _context.People.First(x => x.Id == id);
            List<Favorite> ads = person.Favorites.ToList();

            if (ads.Any())
            {
                return Ok(ads);
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
            //return CreatedAtAction(nameof(GetPerson), new { id = p.Id }, p);
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

        [HttpDelete("Update")]
        public async Task<ActionResult<Ad>> DeleteAd(int id)
        {
            Ad a = await _context.Ads.FirstAsync(x => x.Id == id);

            _context.Ads.Remove(a);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}

using Infrastructure.Context;
using Infrastructure.Dto;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace web_api_silicon.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SubscribController(DataContext dataContext) : ControllerBase
{
    private readonly DataContext _dataContext = dataContext;

    [HttpPost]
    public async Task<IActionResult> CreateSunscribAsync(SubscribDto dto)
    {
        if (ModelState.IsValid)
        {
            if (!await _dataContext.Subscribes.AnyAsync(x => x.Email == dto.Email))
            {
                var Sunscrib = new SubscribEntity
                {
                    Email = dto.Email,
                    DailyNewsletter = dto.DailyNewsletter,
                    AdvertisingUpdates = dto.AdvertisingUpdates,
                    WeekinReview = dto.WeekinReview,
                    EventUpdates = dto.EventUpdates,
                    StartupsWeekly = dto.StartupsWeekly,
                    Podcasts = dto.Podcasts
                };
                _dataContext.Subscribes.Add(Sunscrib);
                await _dataContext.SaveChangesAsync();
                return Created();
            }
            return Conflict();
        }
        return BadRequest();
    }

    [HttpGet]

    public async Task<IActionResult> GetAllSunscribersAsync()
    {
        var Sunscribers = await _dataContext.Subscribes.ToListAsync();
        if (Sunscribers != null)
        {
            return Ok(Sunscribers);
        }
        return NotFound();
    }


    [HttpGet("{email}")]
    public async Task<IActionResult> GetOneSunscriberAsync(string email)
    {
        var Sunscriber = await _dataContext.Subscribes.FirstOrDefaultAsync(x => x.Email == email);
        if (Sunscriber != null)
        {
            return Ok(Sunscriber);
        }
        return NotFound();
    }

    [HttpDelete("{email}")]
    public async Task<IActionResult> DeleteOneSunscriberAsync(string email)
    {
        var Sunscriber = await _dataContext.Subscribes.FirstOrDefaultAsync(x => x.Email == email);
        if (Sunscriber != null)
        {
            _dataContext.Subscribes.Remove(Sunscriber);
            await _dataContext.SaveChangesAsync();
            return Ok(Sunscriber);
        }
        return NotFound(email);
    }
}

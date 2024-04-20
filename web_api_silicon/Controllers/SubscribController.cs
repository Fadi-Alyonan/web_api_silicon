using Infrastructure.Context;
using Infrastructure.Dto;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

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
            try
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
            }
            catch (Exception ex) { Debug.WriteLine(ex); }
            return Conflict();
        }
        return BadRequest();
    }

    [HttpGet]

    public async Task<IActionResult> GetAllSunscribersAsync()
    {
        try
        {
            var Sunscribers = await _dataContext.Subscribes.ToListAsync();
            if (Sunscribers != null)
            {
                return Ok(Sunscribers);
            }
        }
        catch (Exception ex) { Debug.WriteLine(ex); }
       
        return NotFound();
    }


    [HttpGet("{email}")]
    public async Task<IActionResult> GetOneSunscriberAsync(string email)
    {
        try
        {
            var Sunscriber = await _dataContext.Subscribes.FirstOrDefaultAsync(x => x.Email == email);
            if (Sunscriber != null)
            {
                return Ok(Sunscriber);
            }
        }
        catch (Exception ex) { Debug.WriteLine(ex); }
        return NotFound();
    }

    [HttpDelete("{email}")]
    public async Task<IActionResult> DeleteOneSunscriberAsync(string email)
    {
        try
        {
            var Sunscriber = await _dataContext.Subscribes.FirstOrDefaultAsync(x => x.Email == email);
            if (Sunscriber != null)
            {
                _dataContext.Subscribes.Remove(Sunscriber);
                await _dataContext.SaveChangesAsync();
                return Ok(Sunscriber);
            }
        }
        catch (Exception ex) { Debug.WriteLine(ex); }
        return NotFound(email);
    }
}

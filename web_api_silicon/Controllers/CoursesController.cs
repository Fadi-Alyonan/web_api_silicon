using Infrastructure.Context;
using Infrastructure.Dto;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace web_api_silicon.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CoursesController(DataContext dataContext) : ControllerBase
{
    private readonly DataContext _dataContext = dataContext;
    [HttpPost]
    public async Task<IActionResult> CreateCourssAsync(CourseDto dto)
    {
        if (ModelState.IsValid)
        {
            try
            {
                if (!await _dataContext.Courses.AnyAsync(x => x.Title == dto.Title))
                {
                    var course = new Course
                    {
                        Image = dto.Image,
                        Title = dto.Title,
                        Author = dto.Author,
                        OriginalPrice = dto.OriginalPrice,
                        DiscountPrice = dto.DiscountPrice,
                        Hours = dto.Hours,
                        LikesInProcent = dto.LikesInProcent,
                        NumberOfLikes = dto.NumberOfLikes,
                        IsDigital = dto.IsDigital,
                        IsBestseller = dto.IsBestseller,
                    };
                    _dataContext.Courses.Add(course);
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

    public async Task<IActionResult> GetAllCoursesAsync()
    {
        try
        {
            var courses = await _dataContext.Courses.ToListAsync();
            if (courses != null)
            {
                return Ok(courses);
            }
        }
        catch (Exception ex) { Debug.WriteLine(ex); }
        
        return NotFound();
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetOneCourseAsync(string id)
    {
        try
        {
            var course = await _dataContext.Courses.FirstOrDefaultAsync(x => x.Id == id);
            if (course != null)
            {
                return Ok(course);
            }
        }
        catch (Exception ex) { Debug.WriteLine(ex); }
        
        return NotFound();
    }


}

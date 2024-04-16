using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Contexts;
using WebApi.Dtos;
using WebApi.Entities;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CoursesController : ControllerBase
{
    private readonly DataContext _context;

    public CoursesController(DataContext context)
    {
        _context = context;


    }

    [HttpPost]
    public async Task<IActionResult> Create(CourseDto dto)
    {


        if (ModelState.IsValid)
        {
            if (! await _context.Courses.AnyAsync(x => x.Title == dto.Title))
            {
                var courseEntity = new CourseEntity
                {
                    Title = dto.Title,
                    Author = dto.Author,
                    IsBestseller = dto.IsBestseller,
                    DiscountPrice = dto.DiscountPrice,
                    OriginalPrice = dto.OriginalPrice,
                    LikesInNumbers = dto.LikesInNumbers,
                    LikesInProcent = dto.LikesInProcent,
                    Hours = dto.Hours,
                    ImageName = dto.ImageName,
                };
                _context.Courses.Add(courseEntity);
                await _context.SaveChangesAsync();

                return Created("", null);
            }

            return Conflict();
        }

        return BadRequest();
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var courses = await _context.Courses.ToListAsync();
        return Ok(courses);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOne(string id)
    {
        var courseEntity = await _context.Courses.FirstOrDefaultAsync(x => x.Id == id);
        if (courseEntity != null)
        {
            return Ok(courseEntity);
        }

        return NotFound();
    }

}

#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lms.Core;
using Lms.Core.Entities;
using Lms.Data.Data;
using Lms.Core.Dto;
using AutoMapper;

namespace Lms.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly LmsApiContext _context;
        private readonly IMapper mapper;

        public CoursesController(LmsApiContext context, IMapper mapper)
        {
            _context = context;
            this.mapper = mapper;
        }

        // GET: api/Courses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CourseDto>>> GetCourse()
        {
            //return await _context.Course.ToListAsync();
            var corseDto = mapper.ProjectTo<CourseDto>(_context.Course);
            //var corseDto =  mapper.ProjectTo<CourseDto>( _context.Course.Include(m => m.Modules));

            return Ok(corseDto);
            //return Ok(await corseDto.ToListAsync());
            //return await _context.Course.Include(m => m.Modules).ToListAsync();
        }

        // GET: api/Courses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CourseDto>> GetCourse(int id)
        {
            var course = mapper.Map<CourseDto>(await _context.Course.FindAsync(id));

            if (course == null)
            {
                return NotFound();
            }

            return Ok(course);
        }

        // PUT: api/Courses/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCourse(int id, CourseForUpdateDto course)
        {
            //if (id != course.Id)
            //{
            //    return BadRequest();
            //}
            var courseFromRepo = await _context.Course.FindAsync(id);

            if (courseFromRepo == null)
            {
                return NotFound();
                // Skapa om den inte finns (Upserting)
                //Fick det inte att funka!
                //var courseToAdd = mapper.Map<Course>(course);
                //courseToAdd.Id = id;

                //_context.Course.Add(courseToAdd);
                //await _context.SaveChangesAsync();

                //var courseToReturn = mapper.Map<CourseDto>(courseToAdd);

                //return CreatedAtAction("GetCourse", new { id = courseToReturn.Id }, courseToReturn);
                
                //return NotFound();
            }

            mapper.Map(course, courseFromRepo);
            _context.Entry(courseFromRepo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Courses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CourseDto>> PostCourse(CourseForUpdateDto course)
        {
            var courseEntity = mapper.Map<Course>(course);

            _context.Course.Add(courseEntity);
            await _context.SaveChangesAsync();

            var courseToReturn = mapper.Map<CourseDto>(courseEntity);
            return CreatedAtAction("GetCourse", new { id = courseToReturn.Id }, courseToReturn);
            
        }

        // DELETE: api/Courses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var course = await _context.Course.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            _context.Course.Remove(course);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CourseExists(int id)
        {
            return _context.Course.Any(e => e.Id == id);
        }
    }
}

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
using Microsoft.AspNetCore.JsonPatch;
using Lms.Core.IDAL;
using Lms.Core.ResourceParameters;

namespace Lms.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        //private readonly LmsApiContext _context;
        private readonly IMapper mapper;
        private readonly ICourseLibraryRepository _repository;
        private readonly ILogger<CoursesController> _logger;

        
        public CoursesController(LmsApiContext context, IMapper mapper, ICourseLibraryRepository repository, ILogger<CoursesController> logger)
        {
            //_context = context;
            this.mapper = mapper;
            _repository = repository;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // GET: api/Courses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CourseDto>>> GetCourses(
            [FromQuery] CourseResourceParameters courseResourseParameters)
        {
            _logger.LogInformation("Before GetAllCourses");
            var courses = await _repository.GetAllCourses(courseResourseParameters);
            //ToDo GetCourses: courses har inga moduler
            _logger.LogInformation("After GetAllCourses");

            _logger.LogInformation("Before Mapping");
            //var corseDto = mapper.ProjectTo<CourseDto>((IQueryable)courses);
            var corseDto = mapper.Map<IEnumerable<CourseDto>>(courses);
            //ToDo GetCourses: efter Map så har den moduler?!
            //ToDo GetCourses: Det är mappningen som skiter sig när jag har tolist i dalen
            //HACK ändrat till att mappa till en IEnumerable så funkar det och då har jag toList i dalen.
            // Nu funkar även den där parameter att inkluderar moduler eller ej antar att det var
            // så add jag hade hela contextet med förut så då mappades det in av den...

            _logger.LogInformation("After Mapping");

            return Ok(corseDto);
            //HACK när jag inte har tolist så verkar det som SQL anropet sker här vid return...
        }

        // GET: api/Courses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CourseDto>> GetCourse(int id)
        {
            //var course = mapper.Map<CourseDto>(await _context.Course.FindAsync(id));
            var course = mapper.Map<CourseDto>(await _repository.GetCourseByIdAsync(id));

            if (course == null)
            {
                _logger.LogInformation($"Course with id {id} wasn't found.");
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
            //var courseFromRepo = await _context.Course.FindAsync(id);
            var courseFromRepo = await _repository.GetCourseByIdAsync(id);

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

            /*
            mapper.Map(course, courseFromRepo);
            _context.Entry(courseFromRepo).State = EntityState.Modified;

            try
            {
                //await _context.SaveChangesAsync();
                _repository.Save();
                //throw new DbUpdateConcurrencyException();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseExists(id))
                {
                    return NotFound();
                }
                else
                {
                    //throw;
                    return StatusCode(500);
                }
            }

            return NoContent();
            */
            var courseToUpdate = mapper.Map<Course>(course);
            courseToUpdate.Id = id;
            var updatedCourse = await _repository.UpdateCourseAsync(courseToUpdate);
            var courseDto = mapper.Map<CourseDto>(updatedCourse);
            return Ok(courseDto);
            //return NoContent();
        }

        // POST: api/Courses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CourseDto>> PostCourse(CourseForUpdateDto course)
        {
            if (ModelState.IsValid)
            {
                var courseEntity = mapper.Map<Course>(course);

                //_context.Course.Add(courseEntity);
                //await _context.SaveChangesAsync();

                await _repository.InsertCourseAsync(courseEntity);


                var courseToReturn = mapper.Map<CourseDto>(courseEntity);
                return CreatedAtAction("GetCourse", new { id = courseToReturn.Id }, courseToReturn);
            }
            return BadRequest(ModelState);
        }

        // DELETE: api/Courses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            //var course = await _context.Course.FindAsync(id);
            //if (course == null)
            //{
            //    return NotFound();
            //}

            //_context.Course.Remove(course);
            //await _context.SaveChangesAsync();

            if (await _repository.DeleteCourseByIdAsync(id))
            {
                return NoContent();
            }
            return NotFound();
        }


        [HttpPatch("{courseId}")]
        public async Task<ActionResult<CourseDto>> PatchCourse(int courseId,
            JsonPatchDocument<CourseForUpdateDto> patchDocument)
        {
            //if (!CourseExists(courseId)) //ToDo Är inte denna koll onödig? Blir väl samma som nedan, två databasslagningar.
            //{
            //    return NotFound();
            //}

            //var courseFromRepo = await _context.Course.FindAsync(courseId);
            var courseFromRepo = await _repository.GetCourseByIdAsync(courseId);
            if (courseFromRepo == null)
            {
                return NotFound();
            }

            var courseToPatch = mapper.Map<CourseForUpdateDto>(courseFromRepo);

            patchDocument.ApplyTo(courseToPatch, ModelState);

            if (!TryValidateModel(courseToPatch))
            {
                return ValidationProblem(ModelState);
            }

            mapper.Map(courseToPatch, courseFromRepo);
            //_context.Update(courseFromRepo);
            //await _context.SaveChangesAsync();
            
            //return NoContent();

            var updatdedCoourse = await _repository.UpdateCourseAsync(courseFromRepo);
            return Ok(updatdedCoourse);
        }

        private async Task<bool> CourseExistsAsync(int id)
        {
            //return _context.Course.Any(e => e.Id == id);

            return await _repository.CourseExistsAsync(id);
        }
    }
}

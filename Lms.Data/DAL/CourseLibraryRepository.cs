using Lms.Core.Entities;
using Lms.Core.IDAL;
using Lms.Core.ResourceParameters;
using Lms.Data.Data;
using Microsoft.EntityFrameworkCore;

namespace Lms.Data.DAL
{
    public class CourseLibraryRepository : IDisposable, ICourseLibraryRepository
    {
        private LmsApiContext _context;

        public CourseLibraryRepository(LmsApiContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Course> InsertCourseAsync(Course course)
        {
            if (course == null)
            {
                throw new ArgumentNullException(nameof(course));
            }
            var courseToBeSaved = _context.Course.Add(course);
            await _context.SaveChangesAsync();

            return courseToBeSaved.Entity;
        }

        //Todo GetAllCourses: Async???
        public async Task<IEnumerable<Course>> GetAllCourses()
        {
            return await _context.Course.ToListAsync();
        }
        public async Task<IEnumerable<Course>> GetAllCourses(CourseResourceParameters courseResourseParameters)
        {
            if (courseResourseParameters.IncludeModules == null
                && string.IsNullOrWhiteSpace(courseResourseParameters.Sort)
                && string.IsNullOrWhiteSpace(courseResourseParameters.SearchQuery)
                && string.IsNullOrWhiteSpace(courseResourseParameters.Filter))
            {
                return await GetAllCourses();
            }

            var collection = _context.Course as IQueryable<Course>;

            if (!string.IsNullOrWhiteSpace(courseResourseParameters.Filter))
            {
                var filter = courseResourseParameters.Filter.Trim();
                collection = collection.Where(s => s.Title == filter);
            }

            if (courseResourseParameters.IncludeModules == true)
            {
                //return await _context.Course.Include(m => m.Modules).ToListAsync();
                collection = collection.Include(m => m.Modules);
            }

            if (!string.IsNullOrWhiteSpace(courseResourseParameters.SearchQuery))
            {
                var searchQuery = courseResourseParameters.SearchQuery.Trim();
                collection = collection.Where(s => s.Title.Contains(searchQuery));
            }

            return  collection;
            //ToDO GetAllCourses: Sakll det inte vara .ToListAsync()??
            //ToDO GetAllCourses: Hur funkar det att läagg till sortering mm..
        }


        public async Task<Course?> GetCourseByIdAsync(int courseId)
        {
            return await _context.Course.FindAsync(courseId);
        }

        public async Task<bool> DeleteCourseByIdAsync(int courseId)
        {
            var courseToRemove = _context.Course.Find(courseId);

            if (courseToRemove == null)
            {
                return false;
            }

            _context.Course.Remove(courseToRemove);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<Course> UpdateCourseAsync(Course course)
        {
            if (course == null)
            {
                throw new ArgumentNullException(nameof(course));
            }

            var courseToUpdate = await _context.Course.FindAsync(course.Id);
            if (courseToUpdate == null)
            {
                throw new KeyNotFoundException($"Not found course with id {course.Id}.");
            }
            courseToUpdate.Title = course.Title;
            courseToUpdate.StartDate = course.StartDate;
            //ToDo Repo-UpdateCourse: Skall jag mappa även här med automapper eller manuellt? 
            //ToDo Repo-UpdateCourse: EntityState.Modified hur funkar det?

            var updatedCourse = _context.Course.Update(courseToUpdate);
            await _context.SaveChangesAsync();

            return updatedCourse.Entity;
        }

        public async Task<bool> CourseExistsAsync(int courseId)
        {
            return _context.Course.Any(e => e.Id == courseId);
        }

        public bool Save()
        {
            return _context.SaveChanges() >= 0;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose resources when needed
            }
        }


    }
}

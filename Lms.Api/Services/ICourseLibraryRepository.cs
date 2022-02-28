using Lms.Api.ResourceParameters;
using Lms.Core.Entities;

namespace Lms.Api.Services
{
    public interface ICourseLibraryRepository
    {
        void Dispose();
        Task<IEnumerable<Course>> GetAllCourses();
        Task<IEnumerable<Course>> GetAllCourses(CourseResourceParameters courseResourseParameters);
        Task<Course?> GetCourseByIdAsync(int courseId);
        Task<Course> InsertCourseAsync(Course course);
        Task<Course> UpdateCourseAsync(Course course);
        Task<bool> DeleteCourseByIdAsync(int courseId);

        Task<bool> CourseExistsAsync(int courseId);
        bool Save();
    }
}
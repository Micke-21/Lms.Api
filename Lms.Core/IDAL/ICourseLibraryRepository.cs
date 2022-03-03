using Lms.Core.Entities;
using Lms.Core.ResourceParameters;
using Lms.Data.DAL;

namespace Lms.Core.IDAL
{
    public interface ICourseLibraryRepository
    {
        void Dispose();
        Task<IEnumerable<Course>> GetAllCourses();
        Task<(IEnumerable<Course>, PaginationMetadata)> GetAllCourses(CourseResourceParameters courseResourseParameters, int pageNumber, int pageSize);
        Task<Course?> GetCourseByIdAsync(int courseId);
        Task<Course> InsertCourseAsync(Course course);
        Task<Course> UpdateCourseAsync(Course course);
        Task<bool> DeleteCourseByIdAsync(int courseId);

        Task<bool> CourseExistsAsync(int courseId);
        Task<bool> SaveAsync();
    }
}
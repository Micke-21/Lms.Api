namespace Lms.Core.ResourceParameters
{
    public class CourseResourceParameters
    {
        public bool? IncludeModules { get; set; } = false;

        public string? Sort { get; set; } = string.Empty;

        public string? SearchQuery { get; set; } = string.Empty;
    }
}

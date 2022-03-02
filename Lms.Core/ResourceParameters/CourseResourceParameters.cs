namespace Lms.Core.ResourceParameters
{
    /// <summary>
    /// Course Resource Paramters 
    /// Putting teh parameter together to easer use in code and scalability
    /// </summary>
    public class CourseResourceParameters
    {
        /// <summary>
        /// Indicates if the modules should be included or not
        /// </summary>
        public bool? IncludeModules { get; set; } = false;

        /// <summary>
        /// Sort the result?
        /// </summary>
        public string? Sort { get; set; } = string.Empty;

        /// <summary>
        /// SearchQuery string to seach for
        /// </summary>
        public string? SearchQuery { get; set; } = string.Empty;

        /// <summary>
        /// Filter to used for finding exact match
        /// </summary>
        public string? Filter { get; set; } = string.Empty;
    
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.Data.DAL
{
    /// <summary>
    /// Paginationmetadata
    /// </summary>
    public class PaginationMetadata
    {
        /// <summary>
        /// The Total Item count
        /// </summary>
        public int TotalItemCount { get; set; }

        /// <summary>
        /// Total Pages
        /// </summary>
        public int TotalPageCount { get; set; }

        /// <summary>
        /// PageSize
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// CurrentPage
        /// </summary>
        public int CurrentPage { get; set; }
        
        /// <summary>
        /// Constructor for setten up the Pagination metadata
        /// </summary>
        /// <param name="totalItemCount">Total number of</param>
        /// <param name="pageSize">Page Size</param>
        /// <param name="currentPage">Current page</param>
        public PaginationMetadata(int totalItemCount, int pageSize, int currentPage)
        {
            TotalItemCount = totalItemCount;
            PageSize = pageSize;
            CurrentPage = currentPage;
            TotalPageCount = (int)Math.Ceiling(TotalItemCount / (double)pageSize);
        }
    }
}

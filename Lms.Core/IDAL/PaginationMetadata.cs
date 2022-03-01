using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.Data.DAL
{
    public class PaginationMetadata
    {
        public int TotalItemCount { get; set; }

        public int TotalPageCount { get; set; }

        public int PageSize { get; set; }

        public int CurrentPage { get; set; }
        public int TatalItemCount { get; }

        public PaginationMetadata(int totalItemCount, int pageSize, int currentPage)
        {
            TatalItemCount = totalItemCount;
            PageSize = pageSize;
            CurrentPage = currentPage;
            TotalPageCount = (int)Math.Ceiling(TotalItemCount / (double)pageSize);
        }
    }
}

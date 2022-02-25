using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.Core.Dto
{
    public class ModuleDto
    {
        public string Title { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime SlutDate
        {
            get
            {
                return StartDate.AddMonths(1);
            }
        }
    }
}

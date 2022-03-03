using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.Core.Dto
{
    public class CourseForUpdateDto
    {
        [Required]
        [MaxLength(20, ErrorMessage = "Title får var max 20 tecken")]
        public string Title { get; set; } = String.Empty; //ToDo CourseForUpdateDto: Title = String.Empty är detta ett bra sätt att släcka null varning???
        public DateTime StartDate { get; set; }

    }
}

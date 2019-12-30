using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    public class Employee
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage ="Invalid Email Format")]
       
        [Display(Name="Office Email")]
        public string Email { get; set; }
        [Required]
        public Dept Department { get; set; }

        public string PhotoPath { get; set; }
    }
}

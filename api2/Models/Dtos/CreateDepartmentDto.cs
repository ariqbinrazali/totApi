using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api2.Models.Dtos
{
    public class CreateDepartmentDto
    {
        [Required]
        public String Name { get; set; }
    }
}

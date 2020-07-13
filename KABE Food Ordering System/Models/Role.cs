using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KABE_Food_Ordering_System.Models
{
    public class Role
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(30)]
        [MinLength(3)]
        public string Name { get; set; }
    }
}
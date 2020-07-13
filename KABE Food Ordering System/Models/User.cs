using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KABE_Food_Ordering_System.Models
{
    public enum Status
    {
         Active, ProfileNotCreated, InActive, ChangePassword, Deactivated
    }
    public class User
    {

        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        [MinLength(5)]
        [Display(Name="Full Name")]
        public string Name { get; set; }

        [Required]
        [MinLength(9)]
        [StringLength(225)]
        public string Password { get; set; }

        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress, ErrorMessage = "Please enter a valid email address")]
        [StringLength(225)]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [StringLength(225)]
        [Display(Name = "Secret Questions")]
        public string SecretQuestions { get; set; }

        public Role Role { get; set; }
        
        [Required]
        [Display(Name ="Role")]
        public int RoleId { get; set; }

        public IEnumerable<Role> Roles { get; set; }

        [Display(Name = "Date Created")]
        public DateTime DateCreated { get; set; }

        [Display(Name = "Last Logged In")]
        public DateTime LastLoggedIn { get; set; }

        [Display(Name = "Last Logged Out")]
        public DateTime LastLoggedOut { get; set; }

        [Required]
        public Status Status { get; set; }

        
    }
}
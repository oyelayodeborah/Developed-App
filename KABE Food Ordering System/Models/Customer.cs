using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KABE_Food_Ordering_System.Models
{
    
    public class Customer
    {
        [Required]
        public int Id { get; set; }

        public User Users { get; set; }

        //The id of the customer in the user table
        //[Required]
        public int UsersId { get; set; }

        //[Required]
        [MaxLength(225)]
        [RegularExpression(@"^[ a-zA-Z]+$", ErrorMessage = "Name should only contain characters and white spaces")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Date Of Birth")]

        public DateTime DateOfBirth { get; set; }

        //[Required]
        [EmailAddress]
        [StringLength(225)]
        [Display(Name="Email Address")]
        public string Email { get; set; }

        //[Required]
        public string Gender { get; set; }

        //[Required]
        [StringLength(65)]
        public string BestFood { get; set; }

        [Display(Name = "Residential Address")]
        [MaxLength(60)]
        //[Required]
        public string ResidentialAddress { get; set; }

        public Location Location { get; set; }

        [Display(Name = "Location")]
        //[Required]
        public int LocationId { get; set; }

        public IEnumerable<Location> Locations { get; set; }


        [Required]
        [StringLength(11)]
        //[MaxLength(11)]
        //[MinLength(11)]
        [Display(Name = "Telephone Number")]
        public string PhoneNumber { get; set; }

        //[Required]
        [Display(Name="List of Foods you are allergic to ?")]
        public string FoodAllergies { get; set; }

        //[Required]
        [Display(Name ="Recommended Food")]
        public string RecommendedFood { get; set; }
        //[Required]
        [Display(Name ="Recommended Food")]
        public string RecommendedFoodOne { get; set; }
        //[Required]
        [Display(Name ="Recommended Food")]
        public string RecommendedFoodTwo { get; set; }
        //Required
        [Display(Name ="Recommended Food")]
        public string RecommendedFoodThree { get; set; }

        //[Required]
        [Display(Name="Job/Occupation")]
        public string Occupation { get; set; }




    }
}
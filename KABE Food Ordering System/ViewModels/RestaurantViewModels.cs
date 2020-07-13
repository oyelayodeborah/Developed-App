using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using KABE_Food_Ordering_System.Models;

namespace KABE_Food_Ordering_System.ViewModels
{
    public class RestaurantViewModels
    {
        [Required]
        public int Id { get; set; }

        public User Restaurants { get; set; }

        //The id of the restaurant in the user table
        [Required]
        public int RestaurantsId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [StringLength(60)]
        [Display(Name="Residential Address")]
        public string ResidentialAddress { get; set; }

        public Location Location { get; set; }

        [Required]
        [Display(Name="Location")]
        public int LocationId { get; set; }

        public IEnumerable<Location> Locations { get; set; }


        [Required]
        [StringLength(11)]
        [MinLength(11)]
        [Display(Name = "Telephone Number")]
        public string PhoneNumber { get; set; }

        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress, ErrorMessage = "Please enter a valid email address")]
        [StringLength(225)]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [Required]
        [Display(Name="Establishment Date")]
        public DateTime EstablishmentDate { get; set; }
    }
}
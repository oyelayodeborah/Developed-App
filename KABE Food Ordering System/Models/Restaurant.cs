using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KABE_Food_Ordering_System.Models
{
    public class Restaurant
    {
        [Required]
        public int Id { get; set; }

        public User Restaurants { get; set; }

        //The id of the restaurant in the user table
        //[Required]
        public int RestaurantsId { get; set; }

        [Required]
        public string Name { get; set; }

        //[Required]
        [StringLength(60)]
        [Display(Name="Residential Address")]
        public string ResidentialAddress { get; set; }

        public Location Location { get; set; }

        //[Required]
        [Display(Name="Location")]
        public int LocationId { get; set; }

        public IEnumerable<Location> Locations { get; set; }


        //[Required]
        [StringLength(11)]
        //[MaxLength(11)]
        //[MinLength(11)]
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

        public string About { get; set; }
        public string SmallThumbnailjpg { get; set; }
        public string SmallThumbnailpng { get; set; }
        //public string Food { get; set; }
        //public Dictionary<int, string> Foods { get; set; } //int FoodId, string Price
        //[Required]
        public string FoodId { get; set; }
        public string Price { get; set; }
        //public Dictionary<string, string> Foods { get; set; } = new Dictionary<string, string>();//string FoodId, string Price

    }
}
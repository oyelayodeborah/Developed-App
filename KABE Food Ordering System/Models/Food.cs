using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KABE_Food_Ordering_System.Models
{
    //public enum FoodType
    //{
    //    Traditional
    //}
    public class Food
    {
        [Required]
        public int Id { get; set; }


        //public List<Restaurant> Restaurants { get; set; }

        //public Dictionary<int,string> Restaurants { get; set; }

        [Required]
        public string Name { get; set; }

        //[Required]
        //public FoodType FoodType { get; set; }

        [Display (Name= "Image")]
        public string Image { get; set; }

        public string ContentType { get; set; }


    }
}
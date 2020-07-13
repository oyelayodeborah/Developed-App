using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KABE_Food_Ordering_System.ViewModels
{
    //public enum FoodType
    //{
    //    Traditional
    //}
    public class FoodViewModels
    {
        public int Id { get; set; }


        ////public List<Restaurant> Restaurants { get; set; }

        //public Dictionary<int,string> Restaurants { get; set; }

        //public string Name { get; set; }

        ////[Required]
        ////public FoodType FoodType { get; set; }

        //[Display (Name= "Image")]
        //public string Image { get; set; }

        //public List<string> Results { get; set; }

        public string Image { get; set; }//food image
        public string Name { get; set; }//food name
        public string RestaurantName { get; set; }//restaurant name
        public string ContentType { get; set; }//food contentType
        public string Location { get; set; }//restaurant location
        public string About { get; set; }//about restaurant
        public decimal Price { get; set; }//food price

    }
}
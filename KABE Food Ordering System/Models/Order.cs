using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KABE_Food_Ordering_System.Models
{
    public enum OrderStatus {
        Delivered, Pending, NotPaid,Cancelled
    }
    public class Order
    {
        [Required]
        public int Id { get; set; }

        public User Customer { get; set; }

        [Required]
        [Display(Name ="Customer")]
        public int CustomerId { get; set; }

        public User Restaurant { get; set; }
        public IEnumerable<User> Restaurants { get; set; }

        [Required]
        [Display(Name ="Restaurant")]
        public int RestaurantId { get; set; }

        [Required]
        public string Amount { get; set; }

        [Required]
        public string ResidentialAddress { get; set; }

        [Required]
        public string VerificationCode { get; set; }

        [Required]
        [Display(Name="Quantity")]
        public int FoodQuantity { get; set; }

        public Location Location { get; set; }
        public IEnumerable<Location> Locations { get; set; }

        [Required]
        [Display(Name = "Location")]
        public int LocationId { get; set; }

        [Required]
        [Display(Name = "Transaction Reference")]
        public string TransactionReference { get; set; }

        [Required]
        [Display(Name ="Card Number")]
        public string CardNumber { get; set; }

        public Food Food { get; set; }

        [Required]
        [Display(Name ="Food")]
        public int FoodId { get; set; }

        [Required]
        [Display(Name = "Date and Time of Order")]
        public DateTime OrderDateTime { get; set; }

        [Required]
        [Display(Name = "Date and Time of Delivery")]
        public DateTime DeliveryDateTime { get; set; }

        [Required]
        public OrderStatus Status { get; set; }






    }
}
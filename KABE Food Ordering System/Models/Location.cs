using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KABE_Food_Ordering_System.Models
{
    public enum State
    {
        Abia,Adamawa,Akwa_Ibom,Anambra,Bayelsa,Benue,Borno,Cross_River,Delta,Ebonyi,Edo,Ekiti,Enugu,FCT_Abuja,Gombe,Imo,Jigawa,
        Kaduna,Kano,Katsina,Kebbi,Kogi,Kwara,Lagos,Nasarawa,Niger,Ogun,Ondo,Osun,Oyo,Plateau,Rivers,Sokoto,Taraba,Yobe,Zamfara
    }
    public class Location
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public State State { get; set; }

    }
}
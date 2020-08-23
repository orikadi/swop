using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace swop.Models
{
    public class User
    {
        //add rating
        [Key]
        public int UserId { get; set; }

        [DataType(DataType.EmailAddress)]
        [Required]
        public string Email { get; set; }

        [Display(Name = "First Name")]
        [Required]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required]
        public string LastName { get; set; }

        [Display(Name = "Date Of Birth")]
        [Required]
        public DateTime DateOfBirth { get; set; }

        [DataType(DataType.Currency)]
        public double Balance { get; set; }

        [Required]
        public string Password { get; set; }

        [DataType(DataType.ImageUrl)]
        [Display(Name = "User Picture")]
        public string UserPicture { get; set; }

        public int UserType { get; set; } // 0 - user, 1 - admin

        //Apartment Info
        [Required]
        public string Country { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Address { get; set; }

        [Display(Name = "Apartment Picture")]
        [DataType(DataType.ImageUrl)]
        //[Required]
        public string ApartmentPicture { get; set; } // no longer [Required]

       
        [Display(Name = "Apartment Description")]
        [Required]
        public string ApartmentDescription { get; set; }

        [Display(Name = "Apartment Price Per Night")]
        [Required]
        public double ApartmentPrice { get; set; } //price per night
        

        //Requests and cycles
        public ICollection<Request> Requests { get; set; } //virtual??
        public virtual ICollection<UserCycle> UserCycles { get; set; } //many to many
        //ADDED
        public ICollection<History> Histories { get; set; }
    }
}
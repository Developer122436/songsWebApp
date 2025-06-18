using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SongsProject.Models
{
    public class Order
    {
        [BindNever]
        public int OrderID { get; set; }
        [BindNever]
        public ICollection<CartLine> Lines { get; set; }

        [BindNever]
        public bool Sended { get; set; }

        [Required(ErrorMessage = "Please enter a name")]
        [MaxLength(50, ErrorMessage = "Name cannot exceed more than 50 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter the address line")]
        [MaxLength(50, ErrorMessage = "Name cannot exceed more than 50 characters")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Please enter a city name")]
        [MaxLength(50, ErrorMessage = "Name cannot exceed more than 50 characters")]
        public string City { get; set; }

        [MaxLength(50, ErrorMessage = "Name cannot exceed more than 50 characters")]
        public string State { get; set; }

        [Required(ErrorMessage = "The email address is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [Display(Name = "User Email Order")]
        public string Mail { get; set; }

        [Required(ErrorMessage = "Please enter a country name")]
        [MaxLength(50, ErrorMessage = "Name cannot exceed more than 50 characters")]
        public string Country { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace SongsProject.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please insert a user name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please insert a password")]
        [UIHint("password")]
        public string Password { get; set; }

    }
}

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace SongsProject.Models
{
    // To create new properties write "prop"
    // and press TAB button twice on keyboard
    public class Song
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Please enter a song name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter a artist name")]
        public string Artist { get; set; }

        [Required(ErrorMessage = "Please enter a description")]
        public string Description { get; set; }

        [Required]
        [Range(0.01, double.MaxValue,
            ErrorMessage = "Please enter a positive price")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Please specify a country")]
        public string Country { get; set; }

        public string ImagePath { get; set; }

        [Required(ErrorMessage = "Please specify a musicstyle")]
        public string MusicStyle { get; set; }

        public int Rating { get; set; }

    }
}
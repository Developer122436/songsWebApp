using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace SongsProject.Models.ViewModels
{
    public class SongsCreateListViewModel
    {
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

        public IFormFile Photo { get; set; }

        public IFormFile Audio { get; set; }

        [Required(ErrorMessage = "Please specify a musicstyle")]
        public string MusicStyle { get; set; }
    }
}

using Microsoft.AspNetCore.Mvc;
using SongsProject.Models;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using SongsProject.Infrastructure;
using SongsProject.Models.ViewModels;
using System.Web;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

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

        [Required(ErrorMessage = "Please specify a musicstyle")]
        public string MusicStyle { get; set; }

    }
}

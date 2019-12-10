using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SongsProject.Models
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Song>().HasData(
                         new Song
                         {
                             Id = 1,
                             Name = "Toy",
                             Artist = "Neta",
                             Description = "#1 Eurovision",
                             Price = 18,
                             Country = "Israel",
                             MusicStyle = "Pop",
                             ImagePath = "~/images/NoImage.png",
                             AudioPath = " ~/audios/Toy.mp3",        
                             Rating = 0
                         },
                         new Song
                         {
                             Id = 2,
                             Name = "On the floor",
                             Artist = "Jennifer Lopez",
                             Description = "#1 USA",
                             Price = 21,
                             Country = "USA",
                             MusicStyle = "Pop",
                             ImagePath = "~/images/NoImage.png",
                             AudioPath = " ~/audios/On The Floor.mp3",
                             Rating = 0
                         }, new Song
                         {
                             Id = 3,
                             Name = "7 Rings",
                             Artist = "Ariana Grande",
                             Description = "#1 USA",
                             Price = 22,
                             Country = "USA",
                             MusicStyle = "Pop",
                             ImagePath = "~/images/NoImage.png",
                             AudioPath = " ~/audios/7 rings.mp3",
                             Rating = 0
                         }, new Song
                         {
                             Id = 4,
                             Name = "Home",
                             Artist = "Kobi Marimi",
                             Description = "#23 Eurovision",
                             Price = 11,
                             Country = "Israel",
                             MusicStyle = "Pop",
                             ImagePath = "~/images/NoImage.png",
                             AudioPath = " ~/audios/60971056_Home.mp3",
                             Rating = 0
                         }
                     );        
        }
    }
}

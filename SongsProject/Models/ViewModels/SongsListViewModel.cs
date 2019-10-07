using System.Collections.Generic;
using SongsProject.Models;
using SongsProject.Models.ViewModels;

namespace SongsProject.Infrastructure
{
    public class SongsListViewModel
    {
        public IEnumerable<Song> Songs { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public string CurrentCountry { get; set; }
        public string CurrentName { get; set; }
    }

}
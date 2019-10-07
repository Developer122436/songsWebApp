using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SongsProject.Models.ViewModels
{
    public class SongEditViewModel : SongsCreateListViewModel
    {
          public int Id { get; set; }
          public string ExistingPhotoPath { get; set; }
    }
}

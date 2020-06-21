namespace SongsProject.Models.ViewModels
{
    public class SongEditViewModel : SongsCreateListViewModel
    {
        public int Id { get; set; }
        public string ExistingPhotoPath { get; set; }
        public string ExistingAudioPath { get; set; }
    }
}

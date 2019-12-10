using SongsProject.Models.ViewModels;
using System.Linq;

namespace SongsProject.Models
{
    //This class implements dependency injection:
    //easier to change details and for unit testing.
    //A class that depends on the ISongRepository interface can obtain Song objects without need
    //to know the details of how they are stored or how the implementation class will deliver them.
    public interface ISongRepository
    {
        IQueryable<Song> Songs { get; }

        Song DeleteSong(int Id);

        void SaveSong(Song song);

        void AddRating(int Id);
    }
}
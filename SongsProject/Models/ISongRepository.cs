using System.Linq;

namespace SongsProject.Models
{
    public interface ISongRepository
    {
        IQueryable<Song> Songs { get; }

        Song DeleteSong(int Id);

        void SaveSong(Song song);

        void AddRating(int Id);
    }
}
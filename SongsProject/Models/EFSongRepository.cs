using Microsoft.Extensions.Logging;
using System.Linq;

namespace SongsProject.Models
{
    public class EFSongRepository : ISongRepository
    {
        private readonly ApplicationDbContext context;
        private readonly ILogger<EFSongRepository> _logger;

        public EFSongRepository(ApplicationDbContext ctx, ILogger<EFSongRepository> logger)
        {
            context = ctx;
            _logger = logger;
        }

        // Method that show songs from the database
        public IQueryable<Song> Songs => context.Songs;

        // Method that save song in database
        public void SaveSong(Song song)
        {
            if (song.Id == 0)
            {
                song.Rating = 0;
                context.Songs.Add(song);
            }
            else
            {
                Song dbEntry = context.Songs
                .FirstOrDefault(p => p.Id == song.Id);
                if (dbEntry != null)
                {
                    dbEntry.Name = song.Name;
                    dbEntry.Artist = song.Artist;
                    dbEntry.Description = song.Description;
                    dbEntry.Price = song.Price;
                    dbEntry.Country = song.Country;
                    dbEntry.MusicStyle = song.MusicStyle;
                    dbEntry.ImagePath = song.ImagePath;
                    dbEntry.AudioPath = song.AudioPath;
                }
            }
            context.SaveChanges();
        }

        // Method that delete song from database
        public Song DeleteSong(int Id)
        {
            Song dbEntry = context.Songs
            .FirstOrDefault(p => p.Id == Id);
            if (dbEntry != null)
            {
                context.Songs.Remove(dbEntry);
                context.SaveChanges();
            }
            return dbEntry;
        }

        // Method that add rating for specific song in database
        public void AddRating(int Id)
        {
            GetLogger();

            Song dbEntry = context.Songs
                .FirstOrDefault(p => p.Id == Id);
            if (dbEntry != null)
            {
                dbEntry.Rating = dbEntry.Rating + 1;
                context.SaveChanges();
            }
        }

        // Method that will show logging details of the specified method
        public void GetLogger()
        {
            _logger.LogTrace("Trace Log");
            _logger.LogDebug("Debug Log");
            _logger.LogInformation("Information Log");
            _logger.LogWarning("Warning Log");
            _logger.LogError("Error Log");
            _logger.LogCritical("Critical Log");
        }

    }
}
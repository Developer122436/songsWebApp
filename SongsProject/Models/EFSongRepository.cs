using Microsoft.Extensions.Logging;
using SongsProject.Models.ViewModels;
using System.Linq;

namespace SongsProject.Models
{
    public class EFSongRepository : ISongRepository
    {
        private readonly ApplicationDbContext context;
        private readonly ILogger<EFSongRepository> _logger;

        public EFSongRepository(ApplicationDbContext ctx, ILogger <EFSongRepository> logger)
        {
            context = ctx;
            _logger = logger;
        }

        public IQueryable<Song> Songs => context.Songs;
        
        public void AddRating(int Id)
        {
            _logger.LogTrace("Trace Log");
            _logger.LogDebug("Debug Log");
            _logger.LogInformation("Information Log");
            _logger.LogWarning("Warning Log");
            _logger.LogError("Error Log");
            _logger.LogCritical("Critical Log");

            Song dbEntry = context.Songs
                .FirstOrDefault(p => p.Id == Id);
            dbEntry.Rating = dbEntry.Rating + 1;
            context.SaveChanges();
        }

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
                }
            }
            context.SaveChanges();
        }

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
        
    }
}
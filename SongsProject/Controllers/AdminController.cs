using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using SongsProject.Models;
using SongsProject.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SongsProject.Controllers
{
    // Authorize without any parameters it only checks if the user is authenticated.
    [Authorize(Policy = "AdminRolePolicy")]
    public class AdminController : Controller
    {
        private readonly ISongRepository _repositorySongs;
        private readonly IOrderRepository _repositoryOrders;
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _hostingEnvironment;

        public AdminController(IHostingEnvironment hostingEnvironment, ISongRepository repoSongs, IOrderRepository repoOrders, ApplicationDbContext context)
        {
            _hostingEnvironment = hostingEnvironment;
            _repositorySongs = repoSongs;
            _repositoryOrders = repoOrders;
            _context = context;
        }

        public async Task<IActionResult> Index() => View(await _repositorySongs.Songs.ToListAsync());

        // HttpGet UI - UI for edit song
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (ModelState.IsValid)
            {
                Song song = await _repositorySongs.Songs
                .FirstOrDefaultAsync(p => p.Id == id);
                SongEditViewModel songEditViewModel = new SongEditViewModel
                {
                    Id = song.Id,
                    Name = song.Name,
                    Artist = song.Artist,
                    Description = song.Description,
                    Price = song.Price,
                    Country = song.Country,
                    MusicStyle = song.MusicStyle,
                    ExistingPhotoPath = song.ImagePath,
                    ExistingAudioPath = song.AudioPath
                };

                return View(songEditViewModel);
            }
            return View();
        }

        // HttpPost UI - User edit song and the song will inserted to the database
        [HttpPost]
        public IActionResult Edit(SongEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                string stringImagePath = "~/images/";
                string stringAudioPath = "~/audios/";
                string filePhotoName = model.ExistingPhotoPath;
                string fileAudioName = model.ExistingAudioPath;

                Song song = _repositorySongs.Songs
                .FirstOrDefault(p => p.Id == model.Id);
                song.Name = model.Name;
                song.Artist = model.Artist;
                song.Description = model.Description;
                song.Price = model.Price;
                song.Country = model.Country;
                song.MusicStyle = model.MusicStyle;
                song.ImagePath = model.ExistingPhotoPath;
                song.AudioPath = model.ExistingAudioPath;

                if (model.Photo != null)
                {
                    FileInfo fi = new FileInfo(model.Photo.FileName);
                    if (fi.Extension == ".JPG" || fi.Extension == ".PNG" || fi.Extension == ".GIF" || fi.Extension == ".jpg" ||
                        fi.Extension == ".png" || fi.Extension == ".gif")
                    {
                        string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images");
                        filePhotoName = String.Format("{0:d}",
                              (DateTime.Now.Ticks / 10) % 100000000) + "_" + model.Photo.FileName;

                        string filePath = Path.Combine(uploadsFolder, filePhotoName);
                        model.Photo.CopyTo(new FileStream(filePath, FileMode.Create));

                    }
                    song.ImagePath = stringImagePath + filePhotoName;
                }

                if (model.Audio != null)
                {
                    FileInfo fi = new FileInfo(model.Audio.FileName);
                    if (fi.Extension == ".MP3" || fi.Extension == ".WAV" || fi.Extension == ".mp3" ||
                        fi.Extension == ".wav")
                    {

                        string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "audios");
                        fileAudioName = String.Format("{0:d}",
                              (DateTime.Now.Ticks / 10) % 100000000) + "_" + model.Audio.FileName;

                        string filePath = Path.Combine(uploadsFolder, fileAudioName);

                        model.Audio.CopyTo(new FileStream(filePath, FileMode.Create));

                    }
                    song.AudioPath = stringAudioPath + fileAudioName;
                }

                _repositorySongs.SaveSong(song);
                TempData["message"] = $"{song.Name} has been edited";
                return RedirectToAction("Index");

            }

            return View(model);

        }

        // HttpGet UI - UI for create song
        [HttpGet]
        public IActionResult Create() => View("Create", new SongsCreateListViewModel());

        // HttpPost UI - User create song and the song will inserted to the database
        [HttpPost]
        public IActionResult Create(SongsCreateListViewModel model)
        {
            if (ModelState.IsValid)
            {
                string filePhotoName = "NoImage.png";
                string fileAudioName = "";
                string stringImagesPath = "~/images/";
                string stringAudioPath = "~/audios/";

                if (model.Photo != null)
                {
                    FileInfo fi = new FileInfo(model.Photo.FileName);
                    if (fi.Extension == ".JPG" || fi.Extension == ".PNG" || fi.Extension == ".GIF" || fi.Extension == ".jpg" ||
                        fi.Extension == ".png" || fi.Extension == ".gif")
                    {
                        string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images");
                        filePhotoName = String.Format("{0:d}",
                              (DateTime.Now.Ticks / 10) % 100000000) + "_" + model.Photo.FileName;

                        string filePath = Path.Combine(uploadsFolder, filePhotoName);
                        model.Photo.CopyTo(new FileStream(filePath, FileMode.Create));
                    }
                }

                if (model.Audio != null)
                {
                    FileInfo fi = new FileInfo(model.Audio.FileName);
                    if (fi.Extension == ".MP3" || fi.Extension == ".WAV" || fi.Extension == ".mp3" ||
                        fi.Extension == ".wav")
                    {
                        string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "audios");
                        fileAudioName = String.Format("{0:d}",
                              (DateTime.Now.Ticks / 10) % 100000000) + "_" + model.Audio.FileName;

                        string filePath = Path.Combine(uploadsFolder, fileAudioName);
                        model.Audio.CopyTo(new FileStream(filePath, FileMode.Create));

                    }
                }
                Song newSong = new Song
                {
                    Name = model.Name,
                    Artist = model.Artist,
                    Description = model.Description,
                    Price = model.Price,
                    Country = model.Country,
                    MusicStyle = model.MusicStyle,
                    ImagePath = stringImagesPath + filePhotoName,
                    AudioPath = stringAudioPath + fileAudioName
                };

                _repositorySongs.SaveSong(newSong);

                TempData["message"] = $"{newSong.Name} has been created";
                return RedirectToAction("Index", new { id = newSong.Id });

            }
            else
            {
                return View();
            }
        }

        // Method for button that will delete song from the database
        [HttpPost]
        public IActionResult Delete(int Id)
        {
            Song deletedSong = _repositorySongs.DeleteSong(Id);
            if (deletedSong != null)
            {
                TempData["message"] = $"{deletedSong.Name} was deleted";
            }
            return RedirectToAction("Index");
        }

        // Method for button that will export songs from the database to excel file
        [HttpGet("ExportToExcelSongs")]
        public async Task<IActionResult> ExportToExcelSongs()
        {
            byte[] fileContents;
            int rowStart = 2;
            List<SongsProject.Models.Song> songs = await _repositorySongs.Songs.ToListAsync();
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Songs");

                worksheet.Cells["A1"].Value = "SongID";
                worksheet.Cells["B1"].Value = "Name";
                worksheet.Cells["C1"].Value = "Artist";
                worksheet.Cells["D1"].Value = "Description";
                worksheet.Cells["E1"].Value = "Price";
                worksheet.Cells["F1"].Value = "Country";
                worksheet.Cells["G1"].Value = "ImagePath";
                worksheet.Cells["H1"].Value = "MusicStyle";
                worksheet.Cells["I1"].Value = "Rating";

                foreach (var item in songs)
                {
                    worksheet.Cells[string.Format("A{0}", rowStart)].Value = item.Id;
                    worksheet.Cells[string.Format("B{0}", rowStart)].Value = item.Name;
                    worksheet.Cells[string.Format("C{0}", rowStart)].Value = item.Artist;
                    worksheet.Cells[string.Format("D{0}", rowStart)].Value = item.Description;
                    worksheet.Cells[string.Format("E{0}", rowStart)].Value = item.Price;
                    worksheet.Cells[string.Format("F{0}", rowStart)].Value = item.Country;
                    worksheet.Cells[string.Format("G{0}", rowStart)].Value = item.ImagePath;
                    worksheet.Cells[string.Format("H{0}", rowStart)].Value = item.MusicStyle;
                    worksheet.Cells[string.Format("I{0}", rowStart)].Value = item.Rating;
                    rowStart++;
                }

                fileContents = package.GetAsByteArray();
            }

            if (fileContents == null || fileContents.Length == 0)
            {
                return NotFound();
            }

            return File(
                fileContents: fileContents,
                contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileDownloadName: "Songs.xlxs"
            );
        }

        // Method for button that will export orders from the database to excel file
        [HttpGet("ExportToExcelOrders")]
        public async Task<IActionResult> ExportToExcelOrders()
        {
            byte[] fileContents;
            int rowStart = 2;
            List<SongsProject.Models.Order> orders = await _repositoryOrders.Orders.ToListAsync();
            //List<SongsProject.Models.Cart> cart = await _repositoryOrders.Orders.ToListAsync();

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Orders");

                worksheet.Cells["A1"].Value = "OrderID";
                worksheet.Cells["B1"].Value = "Name";
                worksheet.Cells["C1"].Value = "Address";
                worksheet.Cells["D1"].Value = "State";
                worksheet.Cells["E1"].Value = "City";
                worksheet.Cells["F1"].Value = "Country";
                worksheet.Cells["G1"].Value = "Mail";
                worksheet.Cells["H1"].Value = "Is sended";

                foreach (var item in orders)
                {
                    worksheet.Cells[string.Format("A{0}", rowStart)].Value = item.OrderID;
                    worksheet.Cells[string.Format("B{0}", rowStart)].Value = item.Name;
                    worksheet.Cells[string.Format("C{0}", rowStart)].Value = item.Address;
                    worksheet.Cells[string.Format("D{0}", rowStart)].Value = item.State;
                    worksheet.Cells[string.Format("E{0}", rowStart)].Value = item.City;
                    worksheet.Cells[string.Format("F{0}", rowStart)].Value = item.Country;
                    worksheet.Cells[string.Format("G{0}", rowStart)].Value = item.Mail;
                    worksheet.Cells[string.Format("H{0}", rowStart)].Value = item.Sended;

                    rowStart++;
                }

                fileContents = package.GetAsByteArray();
            }

            if (fileContents == null || fileContents.Length == 0)
            {
                return NotFound();
            }

            return File(
                fileContents: fileContents,
                contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileDownloadName: "Orders.xlxs"
            );
        }

        // Method for button that will return user back to home page
        [HttpGet]
        public IActionResult Cancel()
        {
            return RedirectToAction("ListCountry", "Home");
        }

    }
}
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SongsProject.Models;
using SongsProject.Models.ViewModels;
using System.Linq;
using System.Threading.Tasks;

namespace SongsProject.Controllers
{
    [Authorize(Policy = "UserRolePolicy")]
    public class CartController : Controller
    {
        private readonly Cart cart;
        private readonly ISongRepository repository;

        public CartController(ISongRepository repo, Cart cartService)
        {
            repository = repo;
            cart = cartService;
        }

        public ViewResult Index(string returnUrl)
        {
            return View(new CartIndexViewModel
            {
                Cart = cart,
                ReturnUrl = returnUrl
            });
        }

        public async Task<RedirectToActionResult> AddToCart(int id, string returnUrl)
        {
            //HttpContext.Session.GetString("Id");
            var song = await repository.Songs
                .FirstOrDefaultAsync(p => p.Id == id);
            if (song != null) cart.AddItem(song, 1);
            return RedirectToAction("Index", new {returnUrl});
        }

        public async Task<RedirectToActionResult> RemoveFromCart(int id,
            string returnUrl)
        {
            var song = await repository.Songs
                .FirstOrDefaultAsync(p => p.Id == id);
            if (song != null) cart.RemoveLine(song);
            return RedirectToAction("Index", new {returnUrl});
        }

    }
}
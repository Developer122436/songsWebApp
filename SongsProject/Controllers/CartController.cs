using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SongsProject.Models;
using SongsProject.Models.ViewModels;
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

        // Show UI of all songs that user want to buy in his cart
        public ViewResult Index(string returnUrl)
        {
            return View(new CartIndexViewModel
            {
                Cart = cart,
                ReturnUrl = returnUrl
            });
        }

        // Method for button that will add song to cart
        public async Task<RedirectToActionResult> AddToCart(int id, string returnUrl)
        {
            var song = await repository.Songs
                .FirstOrDefaultAsync(p => p.Id == id);
            if (song != null) cart.AddItem(song, 1);
            return RedirectToAction("Index", new { returnUrl });
        }

        // Method for button that will remove song from cart
        public async Task<RedirectToActionResult> RemoveFromCart(int id,
            string returnUrl)
        {
            var song = await repository.Songs
                .FirstOrDefaultAsync(p => p.Id == id);
            if (song != null) cart.RemoveLine(song);
            return RedirectToAction("Index", new { returnUrl });
        }

    }
}
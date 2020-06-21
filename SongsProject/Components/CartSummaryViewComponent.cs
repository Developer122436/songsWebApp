using Microsoft.AspNetCore.Mvc;
using SongsProject.Models;

namespace SongsProject.Components
{
    public class CartSummaryViewComponent : ViewComponent
    {
        private readonly Cart cart;

        public CartSummaryViewComponent(Cart cartService)
        {
            cart = cartService;
        }

        //Renders the cart summary partial view
        public IViewComponentResult Invoke()
        {
            return View(cart);
        }
    }
}
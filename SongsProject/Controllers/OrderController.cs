using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SongsProject.Models;
using System.Linq;

namespace SongsProject.Controllers
{
    [Authorize(Policy = "UserRolePolicy")]
    public class OrderController : Controller
    {
        private readonly Cart _cart;
        private readonly IOrderRepository _repository;

        public OrderController(IOrderRepository repoService, Cart cartService)
        {
            _repository = repoService;
            _cart = cartService;
        }

        // UI that will show all orders that was not sended to the users
        public ViewResult List() =>
            View(_repository.Orders.Where(o => !o.Sended));

        // Method that let admin user to mark the orders that was sended to users
        [HttpPost]
        public IActionResult MarkSended(int orderId)
        {
            Order order = _repository.Orders
                .FirstOrDefault(o => o.OrderID == orderId);

            if (order != null)
            {
                order.Sended = true;
                _repository.SaveOrder(order);
            }

            return RedirectToAction(nameof(List));

        }

        // HttpGet UI - UI that show all the details user need to add for the order
        [HttpGet]
        public IActionResult Checkout() => View(new Order());

        // HttpPost UI - User add his details and the order inserted to the database
        [HttpPost]
        public IActionResult Checkout(Order order)
        {
            if (!_cart.Lines.Any()) ModelState.AddModelError("", "Sorry, your cart is empty!");

            if (ModelState.IsValid)
            {
                order.Lines = _cart.Lines.ToArray();
                _repository.SaveOrder(order);
                return RedirectToAction(nameof(Completed), order);
            }

            return View(order);
        }

        // UI that let the user know if his order is completed or there is a problem with his order
        public IActionResult Completed(Order order)
        {
            if (!_cart.Lines.Any()) ModelState.AddModelError("", "Sorry, your cart is empty!");

            if (ModelState.IsValid)
            {
                _cart.Clear();
                return View(order);
            }
            return RedirectToAction("AccessDenied", "Order");
        }

        // Method for button that will return user back to home page
        [HttpGet]
        public IActionResult Cancel()
        {
            return RedirectToAction("ListCountry", "Home");
        }

        // HttpGet UI - UI of access denied, will show if user don't have credentials to access the UI
        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
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

        public ViewResult List() =>
            View(_repository.Orders.Where(o => !o.Sended));


        [HttpGet]
        public IActionResult Checkout() => View(new Order());

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

        [HttpPost]
        public IActionResult Checkout(Order order)
        {
            if (!_cart.Lines.Any()) ModelState.AddModelError("", "Sorry, your cart is empty!");
            //any validation problems are passed to the action method through the ModelState property
            if (ModelState.IsValid)
            {
                order.Lines = _cart.Lines.ToArray();
                _repository.SaveOrder(order);
                return RedirectToAction(nameof(Completed), order);
            }

            return View(order);
        }

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

        [HttpGet]
        public IActionResult Cancel()
        {
            return RedirectToAction("ListCountry", "Home");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
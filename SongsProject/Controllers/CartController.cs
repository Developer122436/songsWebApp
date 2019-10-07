﻿using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SongsProject.Models;
using SongsProject.Models.ViewModels;

namespace SongsProject.Controllers
{
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

        public RedirectToActionResult AddToCart(int id, string returnUrl)
        {
            //HttpContext.Session.GetString("Id");
            var song = repository.Songs
                .FirstOrDefault(p => p.Id == id);
            if (song != null) cart.AddItem(song, 1);
            return RedirectToAction("Index", new {returnUrl});
        }

        public RedirectToActionResult RemoveFromCart(int id,
            string returnUrl)
        {
            //HttpContext.Session.GetString("Id");
            var song = repository.Songs
                .FirstOrDefault(p => p.Id == id);
            if (song != null) cart.RemoveLine(song);
            return RedirectToAction("Index", new {returnUrl});
        }


    }
}
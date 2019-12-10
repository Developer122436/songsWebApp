using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using SongsProject.Infrastructure;

namespace SongsProject.Models
{
    public class SessionCart : Cart
    {
        //The static GetCart method is a factory
        //for creating SessionCart objects and providing them with an 
        //ISession object so they can store themselves.
        public static Cart GetCart(IServiceProvider services)
        {
            //Getting hold of the ISession object is a little complicated. I have to obtain an instance of the
            //IHttpContextAccessor service, which provides me with access to an HttpContext object that, in turn,
            //provides me with the ISession.
            //This indirect approach is required because the session isn’t provided as a
            //regular service.
            ISession session = services.GetService<IHttpContextAccessor>()?
            .HttpContext.Session;
            SessionCart cart = session?.GetJson<SessionCart>("Cart")
            ?? new SessionCart();
            cart.Session = session;
            return cart;
        }

        [JsonIgnore]
        public ISession Session { get; set; }
        public override void AddItem(Song song, int quantity)
        {
            base.AddItem(song, quantity);
            Session.SetJson("Cart", this);
        }
        public override void RemoveLine(Song song)
        {
            base.RemoveLine(song);
            Session.SetJson("Cart", this);
        }
        public override void Clear()
        {
            base.Clear();
            Session.Remove("Cart");
        }
    }
}
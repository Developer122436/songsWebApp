using Microsoft.AspNetCore.Mvc;
using SongsProject.Models;
using System.Linq;

namespace SongsProject.Components
{
    public class NavigationSearchViewComponent : ViewComponent
    {
        private readonly ISongRepository repository;

        public NavigationSearchViewComponent(ISongRepository repo)
        {
            repository = repo;
        }

        //Renders the navigation search partial view
        public IViewComponentResult Invoke()
        {
            ViewBag.SelectedName = RouteData?.Values["Name"];
            return View(repository.Songs
                .Select(x => x.Name)
                .Distinct()
                .OrderBy(x => x));
        }
    }
}
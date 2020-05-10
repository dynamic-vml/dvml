using DynamicVML;
using DynamicVML.Extensions;

using Microsoft.AspNetCore.Mvc;

namespace Tests
{
    public class HomeController : Controller
    {
        public HomeController()
        {
        }


        public IActionResult AddBook(AddNewDynamicItem parameters)
        {
            var newBookViewModel = new BookViewModel()
            {
                Title = "New book 1"
            };

            return this.PartialView(newBookViewModel, parameters);
        }


    }
}

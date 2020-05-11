using System;
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

        public IActionResult GetSimple()
        {
            var vm = new SimpleList(4);

            try
            {
                return PartialView(vm);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public IActionResult GetNested()
        {
            var vm = new NestedRoot(4, 3, 5);

            try
            {
                return PartialView(vm);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public IActionResult GetNestedRecursive()
        {
            var vm = new NestedRecursive(5);

            try
            {
                return PartialView(vm);
            }
            catch (Exception ex)
            {
                throw;
            }
        }






        public IActionResult EditSimple()
        {
            var vm = new SimpleList(4);
            
            try
            {
                return PartialView(vm);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public IActionResult EditNested()
        {
            var vm = new NestedRoot(4, 3, 5);

            try
            {
                return PartialView(vm);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public IActionResult EditNestedRecursive()
        {
            var vm = new NestedRecursive(5);

            try
            {
                return PartialView(vm);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}

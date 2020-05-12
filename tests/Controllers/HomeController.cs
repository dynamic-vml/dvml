using System;
using DynamicVML;
using DynamicVML.Extensions;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Tests
{
    public class HomeController : Controller
    {
        ILogger<HomeController> logger;

        public HomeController(ILogger<HomeController> logger)
        {
            this.logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddBook(AddNewDynamicItem parameters)
        {
            var newBookViewModel = new BookViewModel()
            {
                Title = "New book 1"
            };

            return this.PartialView(newBookViewModel, parameters);
        }



        #region Basic display tests
        public IActionResult GetSimple()
        {
            var vm = new SimpleList(4);

            try
            {
                return PartialView(vm);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
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
                logger.LogError(ex, ex.Message);
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
                logger.LogError(ex, ex.Message);
                throw;
            }
        }
        #endregion



        #region basic editor tests

        public IActionResult EditSimple()
        {
            var vm = new SimpleList(4);

            try
            {
                return PartialView(vm);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
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
                logger.LogError(ex, ex.Message);
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
                logger.LogError(ex, ex.Message);
                throw;
            }
        }
        #endregion




        #region add new item tests
        public IActionResult AddSimpleItem(AddNewDynamicItem parameters)
        {
            var vm = new SimpleItem(99);

            return this.PartialView(vm, parameters, options =>
            {
                options.Index = "N";
            });
        }

        public IActionResult EditSimpleWithLayout()
        {
            var vm = new SimpleList(1);

            try
            {
                return View("EditSimple", vm);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                throw;
            }
        }

        #endregion
    }
}

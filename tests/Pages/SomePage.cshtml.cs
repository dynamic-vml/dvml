using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using DynamicVML;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.Extensions.Logging;
using DynamicVML.Extensions;
using Tests.ViewModels;

namespace Tests.Pages
{
    public class SomePageModel : PageModel
    {
        private ILogger<SomePageModel> logger;
        private ICompositeViewEngine engine;

        public SomePageModel(ILogger<SomePageModel> logger, ICompositeViewEngine engine)
        {
            this.logger = logger;
            this.engine = engine;
        }

        public IActionResult OnGetAddBook(AddNewDynamicItem parameters)
        {
            var newBookViewModel = new BookViewModel()
            {
                Title = "New book 1"
            };

            return this.Partial(newBookViewModel, parameters);
        }


        public IActionResult OnGetAddBookWithParameterByGet(AddNewDynamicItem parameters, int integerParameter, string stringParameter)
        {
            var newBookViewModel = new BookViewModel()
            {
                Title = $"New book via GET with parameters: {integerParameter} {stringParameter}"
            };

            return this.Partial(newBookViewModel, parameters);
        }

        public IActionResult OnGetAddBookWithOptionsAndParameterByGet(AddNewDynamicItem parameters, int integerParameter, string stringParameter)
        {
            var newBookViewModel = new BookViewModel()
            {
                Title = $"New book via GET with parameters: {integerParameter} {stringParameter}"
            };

            return this.Partial(newBookViewModel, parameters, new TestOptions<BookViewModel>()
            {
                TestText = "OptionsTestText"
            });
        }
    }
}
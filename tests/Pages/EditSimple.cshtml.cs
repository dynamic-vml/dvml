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
    public class EditSimpleModel : PageModel
    {
        private ILogger<EditSimpleModel> logger;
        private ICompositeViewEngine engine;

        public EditSimpleModel(ILogger<EditSimpleModel> logger, ICompositeViewEngine engine)
        {
            this.logger = logger;
            this.engine = engine;
        }

        [BindProperty]
        public SimpleList Input { get; set; }

        public IActionResult OnGet()
        {
            this.Input = new SimpleList(4);

            return Page();
        }



        public IActionResult OnGetAddSimpleItem(AddNewDynamicItem parameters)
        {
            var vm = new SimpleItem(99);

            return this.Partial(vm, parameters, options =>
            {
                options.Index = "N";
            });
        }

     
        public async Task<IActionResult> OnPostAddSimpleItemByPost([FromBody] AddNewDynamicItem parameters)
        {
            var vm = new SimpleItem(99);

            try
            {
                return await this.PartialAsync(engine, vm, parameters, options =>
                {
                    options.Index = "N";
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                throw;
            }
        }

    }
}
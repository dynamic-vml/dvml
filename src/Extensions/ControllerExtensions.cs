// Dynamic View Model (lists)
// Copyright (c) 2020 César Roberto de Souza. Licensed under the MIT license
// cesarsouza@gmail.com - http://crsouza.com

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using DynamicVML.Internals;
using DynamicVML.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace DynamicVML.Extensions
{
    /// <summary>
    ///   Extension methods to help sending back list items from the controllers.
    /// </summary>
    /// 
    /// <remarks>
    ///     Please see the sample applications for examples on how they are used.
    /// </remarks>
    /// 
    public static class ControllerExtensions
    {
        /// <summary>
        ///    Renders a partial view containing the item to be in plain HTML,
        ///    and converts it to JSON so it can be sent back to the client. You should
        ///    only use this method when using <see cref="NewItemMethod.Post"/>.
        /// </summary>
        /// 
        /// <typeparam name="TViewModel">The type of the view model whose view should be rendered.</typeparam>
        /// <typeparam name="TOptions">The options associated with the view model.</typeparam>
        /// 
        /// <param name="controller">The controller handling the action for creating the new item.</param>
        /// <param name="engine">An instance of the <see cref="ICompositeViewEngine"/> class (which 
        ///   can be obtained using dependency injection by asking it as a new parameter in your 
        ///   controllers' constructor)</param>
        /// <param name="viewModel">The view model whose view should be rendered.</param>
        /// <param name="parameters">The <see cref="AddNewDynamicItem"/> received by the controller.</param>
        /// <param name="options">Any additional options you might want to specify for this item.</param>
        /// 
        /// <returns>An <see cref="ActionResult"/> to be sent back to the client.</returns>
        /// 
        public async static Task<ActionResult> PartialViewAsync<TViewModel, TOptions>(this Controller controller,
            ICompositeViewEngine engine, TViewModel viewModel, AddNewDynamicItem parameters, TOptions options)
            where TViewModel : class
            where TOptions : DynamicListItem<TViewModel>, new()
        {
            var item = viewModel.ToDynamicList(parameters.ContainerId, options);
            return await PartialViewAsync(controller, engine, (IDynamicList)item, parameters);
        }

        /// <summary>
        ///    Renders a partial view containing the item to be in plain HTML,
        ///    and converts it to JSON so it can be sent back to the client. You should
        ///    only use this method when using <see cref="NewItemMethod.Post"/>.
        /// </summary>
        /// 
        /// <typeparam name="TViewModel">The type of the view model whose view should be rendered.</typeparam>
        /// <typeparam name="TOptions">The options associated with the view model.</typeparam>
        /// 
        /// <param name="controller">The controller handling the action for creating the new item.</param>
        /// <param name="viewModel">The view model whose view should be rendered.</param>
        /// <param name="parameters">The <see cref="AddNewDynamicItem"/> received by the controller.</param>
        /// <param name="options">Any additional options you might want to specify for this item.</param>
        /// 
        /// <returns>An <see cref="ActionResult"/> to be sent back to the client.</returns>
        /// 
        public static ActionResult PartialView<TViewModel, TOptions>(this Controller controller,
            TViewModel viewModel, AddNewDynamicItem parameters, TOptions options)
            where TViewModel : class
            where TOptions : DynamicListItem<TViewModel>, new()
        {
            var item = viewModel.ToDynamicList(parameters.ContainerId, options);
            return PartialView(controller, (IDynamicList)item, parameters);
        }

        /// <summary>
        ///    Creates the partial view for the item that should be sent back to the client.
        /// </summary>
        /// 
        /// <typeparam name="TViewModel">The type of the view model whose view should be rendered.</typeparam>
        /// 
        /// <param name="controller">The controller handling the action for creating the new item.</param>
        /// <param name="viewModel">The view model whose view should be rendered.</param>
        /// <param name="parameters">The <see cref="AddNewDynamicItem"/> received by the controller.</param>
        /// 
        /// <returns>An <see cref="ActionResult"/> to be sent back to the client.</returns>
        /// 
        public static ActionResult PartialView<TViewModel>(this Controller controller,
            TViewModel viewModel, AddNewDynamicItem parameters)
            where TViewModel : class
        {
            var item = viewModel.ToDynamicList(parameters.ContainerId);
            return PartialView(controller, (IDynamicList)item, parameters);
        }

        /// <summary>
        ///    Creates the partial view for the item that should be sent back to the client.
        /// </summary>
        /// 
        /// <param name="controller">The controller handling the action for creating the new item.</param>
        /// <param name="parameters">The <see cref="AddNewDynamicItem"/> received by the controller.</param>
        /// <param name="item">The dynamic list item to be rendered.</param>
        /// 
        /// <returns>An <see cref="ActionResult"/> to be sent back to the client.</returns>
        /// 
        public static ActionResult PartialView(this Controller controller,
           IDynamicList item, AddNewDynamicItem parameters)
        {
            PartialViewResult partialView = controller.PartialView(parameters.ListTemplate, item);
            partialView.ViewData.SetEditorItemParameters(item.Index, parameters, NewItemMethod.Get);
            return partialView;
        }

        /// <summary>
        ///    Renders a partial view containing the item to be in plain HTML,
        ///    and converts it to JSON so it can be sent back to the client. You should
        ///    only use this method when using <see cref="NewItemMethod.Post"/>.
        /// </summary>
        /// 
        /// <param name="controller">The controller handling the action for creating the new item.</param>
        /// <param name="engine">An instance of the <see cref="ICompositeViewEngine"/> class (which 
        ///   can be obtained using dependency injection by asking it as a new parameter in your 
        ///   controllers' constructor)</param>
        /// <param name="item">The dynamic list item to be rendered.</param>
        /// <param name="parameters">The <see cref="AddNewDynamicItem"/> received by the controller.</param>
        /// 
        /// <returns>An <see cref="ActionResult"/> to be sent back to the client.</returns>
        /// 
        internal static async Task<ActionResult> PartialViewAsync(this Controller controller,
            ICompositeViewEngine engine, IDynamicList item, AddNewDynamicItem parameters)
        {
            // Notes: based on https://stackoverflow.com/a/31906578/262032

            ViewEngineResult viewResult = engine
                .FindView(controller.ControllerContext, parameters.ListTemplate, false);

            using (var writer = new StringWriter())
            {
                ViewContext viewContext = new ViewContext(
                    controller.ControllerContext,
                    viewResult.View,
                    controller.ViewData,
                    controller.TempData,
                    writer,
                    new HtmlHelperOptions()
                );

                viewContext.ViewData.Model = item;
                viewContext.ViewData.SetEditorItemParameters(item.Index, parameters, NewItemMethod.Post);

                await viewResult.View.RenderAsync(viewContext);
                string html = writer.GetStringBuilder().ToString();

                return controller.Json(new
                {
                    success = true,
                    html = html
                });
            }
        }



    }
}

// Dynamic View Model (lists)
// Copyright (c) 2020 César Roberto de Souza. Licensed under the MIT license
// cesarsouza@gmail.com - http://crsouza.com

using System;
using System.IO;
using System.Threading.Tasks;

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
        /// 
        /// <param name="controller">The controller handling the action for creating the new item.</param>
        /// <param name="engine">An instance of the <see cref="ICompositeViewEngine"/> class (which 
        ///   can be obtained using dependency injection by asking it as a new parameter in your 
        ///   controllers' constructor)</param>
        /// <param name="viewModel">The view model whose view should be rendered.</param>
        /// <param name="parameters">The <see cref="AddNewDynamicItem"/> received by the controller.</param>
        /// 
        /// <returns>An <see cref="ActionResult"/> to be sent back to the client.</returns>
        /// 
        public async static Task<ActionResult> PartialViewAsync<TViewModel>(this Controller controller,
            ICompositeViewEngine engine, TViewModel viewModel, AddNewDynamicItem parameters)
            where TViewModel : class
        {
            CheckPostParametersAndThrow(parameters, controller);
            IDynamicList item = viewModel.ToDynamicList(parameters.ContainerId!);
            return await PartialViewAsync(controller, engine, item: item, parameters);
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
            CheckPostParametersAndThrow(parameters, controller);
            IDynamicList item = viewModel.ToDynamicList(parameters.ContainerId!, options);
            return await PartialViewAsync(controller, engine, item: item, parameters);
        }

        /// <summary>
        ///    Renders a partial view containing the item to be in plain HTML,
        ///    and converts it to JSON so it can be sent back to the client. You should
        ///    only use this method when using <see cref="NewItemMethod.Post"/>.
        /// </summary>
        /// 
        /// <typeparam name="TViewModel">The type of the view model whose view should be rendered.</typeparam>
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
        public async static Task<ActionResult> PartialViewAsync<TViewModel>(this Controller controller,
            ICompositeViewEngine engine, TViewModel viewModel, AddNewDynamicItem parameters,
            Action<DynamicListItem<TViewModel>> options)
            where TViewModel : class
        {
            return await PartialViewAsync<TViewModel, DynamicListItem<TViewModel>>(
                controller, engine, viewModel, parameters, options);
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
            ICompositeViewEngine engine, TViewModel viewModel, AddNewDynamicItem parameters, Action<TOptions> options)
            where TViewModel : class
            where TOptions : DynamicListItem<TViewModel>, new()
        {
            CheckPostParametersAndThrow(parameters, controller);
            IDynamicList item = CreateItem(viewModel, parameters, options);
            return await PartialViewAsync(controller, engine, item: item, parameters);
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
            CheckGetParametersAndThrow(parameters, controller);
            IDynamicList item = viewModel.ToDynamicList(parameters.ContainerId!, options);
            return PartialView(controller, item: item, parameters);
        }

        /// <summary>
        ///    Renders a partial view containing the item to be in plain HTML,
        ///    and converts it to JSON so it can be sent back to the client. You should
        ///    only use this method when using <see cref="NewItemMethod.Post"/>.
        /// </summary>
        /// 
        /// <typeparam name="TViewModel">The type of the view model whose view should be rendered.</typeparam>
        /// 
        /// <param name="controller">The controller handling the action for creating the new item.</param>
        /// <param name="viewModel">The view model whose view should be rendered.</param>
        /// <param name="parameters">The <see cref="AddNewDynamicItem"/> received by the controller.</param>
        /// <param name="options">An action that allows for configuring the options to be associated 
        ///     with the new item before it gets inserted to the list.</param>
        /// 
        /// <returns>An <see cref="ActionResult"/> to be sent back to the client.</returns>
        /// 
        public static ActionResult PartialView<TViewModel>(this Controller controller,
            TViewModel viewModel, AddNewDynamicItem parameters, Action<DynamicListItem<TViewModel>> options)
            where TViewModel : class
        {
            return PartialView<TViewModel, DynamicListItem<TViewModel>>(controller, viewModel, parameters, options);
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
        /// <param name="options">An action that allows for configuring the options to be associated 
        ///     with the new item before it gets inserted to the list.</param>
        /// 
        /// <returns>An <see cref="ActionResult"/> to be sent back to the client.</returns>
        /// 
        public static ActionResult PartialView<TViewModel, TOptions>(this Controller controller,
            TViewModel viewModel, AddNewDynamicItem parameters, Action<TOptions> options)
            where TViewModel : class
            where TOptions : DynamicListItem<TViewModel>, new()
        {
            CheckGetParametersAndThrow(parameters, controller);
            IDynamicList item = CreateItem(viewModel, parameters, options);
            return PartialView(controller, item: item, parameters: parameters);
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
            CheckGetParametersAndThrow(parameters, controller);
            IDynamicList item = viewModel.ToDynamicList(parameters.ContainerId!);
            return PartialView(controller, item: item, parameters);
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
            CheckGetParametersAndThrow(parameters, controller);
            PartialViewResult partialView = controller.PartialView(parameters.ListTemplate, item);
            partialView.ViewData.GetItemEditorParameters(item.Index, parameters, NewItemMethod.Get);
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
        public static async Task<ActionResult> PartialViewAsync(this Controller controller,
            ICompositeViewEngine engine, IDynamicList item, AddNewDynamicItem parameters)
        {
            // Notes: This method is based on https://stackoverflow.com/a/31906578/262032

            CheckPostParametersAndThrow(parameters, controller);

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
                viewContext.ViewData.GetItemEditorParameters(item.Index, parameters, NewItemMethod.Post);

                try
                {
                    await viewResult.View.RenderAsync(viewContext);
                    string html = writer.GetStringBuilder().ToString();

                    return controller.Json(new
                    {
                        success = true,
                        html = html
                    });
                }
                catch (Exception ex) when (ex is InvalidOperationException || ex is DynamicListException)
                {
                    string error = $"Failed to render the dynamic list item. If you are using custom templates with " +
                        $"options, make sure you have passed the options object when calling {nameof(PartialViewAsync)}.";

                    return controller.Json(new
                    {
                        success = false,
                        html = error
                    });
                }
            }
        }



        private static IDynamicList CreateItem<TViewModel, TOptions>(TViewModel viewModel,
            AddNewDynamicItem parameters, Action<TOptions> options)
            where TViewModel : class
            where TOptions : DynamicListItem<TViewModel>, new()
        {
            TOptions o = new TOptions();
            options(o);
            var item = viewModel.ToDynamicList(parameters.ContainerId!, o);
            return item;
        }

        private static void CheckPostParametersAndThrow(AddNewDynamicItem parameters, Controller controller)
        {
            if (parameters.ContainerId == null)
            {
                throw new ArgumentException("The received new item parameters did not contain a valid " +
                    "containerId. Have you forgotten to add the \"[FromBody]\" attribute to the 'AddNewDynamicItem' " +
                    "parameter in your [HttpPost] controller action?\"", nameof(parameters));
            }

            if (controller.Request != null && controller.Request.Method != "POST")
            {
                throw new DynamicListException($"The item has been received via GET, but the action method is calling " +
                    $"{nameof(PartialViewAsync)} instead of {nameof(PartialView)}. Please switch it to use {nameof(PartialView)}" +
                    $"otherwise the item will not render correctly.")
                {
                    AddNewItemParameters = parameters
                };
            }
        }

        private static void CheckGetParametersAndThrow(AddNewDynamicItem parameters, Controller controller)
        {
            if (parameters.ContainerId == null)
            {
                throw new ArgumentException("The received new item parameters did not contain a valid containerId.", nameof(parameters));
            }

            if (controller.Request != null && controller.Request.Method != "GET")
            {
                throw new DynamicListException($"The item has been received via POST, but the action method is calling " +
                    $"{nameof(PartialView)} instead of {nameof(PartialViewAsync)}. Please switch it to use {nameof(PartialViewAsync)}" +
                    $"otherwise the item will not render correctly.")
                {
                    AddNewItemParameters = parameters
                };
            }

            if (parameters.AdditionalViewData != null && parameters.AdditionalViewData.Length > 0)
            {
                throw new ArgumentException($"The received item parameters contain additional view data, but the" +
                    $"item has been received via GET or the controller action method is calling {nameof(PartialView)} instead " +
                    $"of {nameof(PartialViewAsync)}. Please switch to POST and call {nameof(PartialViewAsync)} in your action " +
                    $"method instead.", nameof(parameters));
            }
        }
    }
}

// Dynamic View Model (lists)
// Copyright (c) 2020 César Roberto de Souza. Licensed under the MIT license
// cesarsouza@gmail.com - http://crsouza.com

using System;
using System.IO;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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
    public static class PageExtensions
    {
        /// <summary>
        ///    Renders a partial view containing the item to be in plain HTML,
        ///    and converts it to JSON so it can be sent back to the client. You should
        ///    only use this method when using <see cref="NewItemMethod.Post"/>.
        /// </summary>
        /// 
        /// <typeparam name="TViewModel">The type of the view model whose view should be rendered.</typeparam>
        /// 
        /// <param name="pageModel">The page model handling the action for creating the new item.</param>
        /// <param name="engine">An instance of the <see cref="ICompositeViewEngine"/> class (which 
        ///   can be obtained using dependency injection by asking it as a new parameter in your 
        ///   controllers' constructor)</param>
        /// <param name="viewModel">The view model whose view should be rendered.</param>
        /// <param name="parameters">The <see cref="AddNewDynamicItem"/> received by the controller.</param>
        /// 
        /// <returns>An <see cref="ActionResult"/> to be sent back to the client.</returns>
        /// 
        public async static Task<ActionResult> PartialAsync<TViewModel>(this PageModel pageModel,
            ICompositeViewEngine engine, TViewModel viewModel, AddNewDynamicItem parameters)
            where TViewModel : class
        {
            CheckPostParametersAndThrow(parameters, pageModel);
            IDynamicList item = viewModel.ToDynamicList(parameters.ContainerId!);
            return await PartialAsync(pageModel, engine, item: item, parameters);
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
        /// <param name="pageModel">The page model handling the action for creating the new item.</param>
        /// <param name="engine">An instance of the <see cref="ICompositeViewEngine"/> class (which 
        ///   can be obtained using dependency injection by asking it as a new parameter in your 
        ///   controllers' constructor)</param>
        /// <param name="viewModel">The view model whose view should be rendered.</param>
        /// <param name="parameters">The <see cref="AddNewDynamicItem"/> received by the controller.</param>
        /// <param name="options">Any additional options you might want to specify for this item.</param>
        /// 
        /// <returns>An <see cref="ActionResult"/> to be sent back to the client.</returns>
        /// 
        public async static Task<ActionResult> PartialAsync<TViewModel, TOptions>(this PageModel pageModel,
            ICompositeViewEngine engine, TViewModel viewModel, AddNewDynamicItem parameters, TOptions options)
            where TViewModel : class
            where TOptions : DynamicListItem<TViewModel>, new()
        {
            CheckPostParametersAndThrow(parameters, pageModel);
            IDynamicList item = viewModel.ToDynamicList(parameters.ContainerId!, options);
            return await PartialAsync(pageModel, engine, item: item, parameters);
        }

        /// <summary>
        ///    Renders a partial view containing the item to be in plain HTML,
        ///    and converts it to JSON so it can be sent back to the client. You should
        ///    only use this method when using <see cref="NewItemMethod.Post"/>.
        /// </summary>
        /// 
        /// <typeparam name="TViewModel">The type of the view model whose view should be rendered.</typeparam>
        /// 
        /// <param name="pageModel">The page model handling the action for creating the new item.</param>
        /// <param name="engine">An instance of the <see cref="ICompositeViewEngine"/> class (which 
        ///   can be obtained using dependency injection by asking it as a new parameter in your 
        ///   controllers' constructor)</param>
        /// <param name="viewModel">The view model whose view should be rendered.</param>
        /// <param name="parameters">The <see cref="AddNewDynamicItem"/> received by the controller.</param>
        /// <param name="options">Any additional options you might want to specify for this item.</param>
        /// 
        /// <returns>An <see cref="ActionResult"/> to be sent back to the client.</returns>
        /// 
        public async static Task<ActionResult> PartialAsync<TViewModel>(this PageModel pageModel,
            ICompositeViewEngine engine, TViewModel viewModel, AddNewDynamicItem parameters,
            Action<DynamicListItem<TViewModel>> options)
            where TViewModel : class
        {
            return await PartialAsync<TViewModel, DynamicListItem<TViewModel>>(
                pageModel, engine, viewModel, parameters, options);
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
        /// <param name="pageModel">The page model handling the action for creating the new item.</param>
        /// <param name="engine">An instance of the <see cref="ICompositeViewEngine"/> class (which 
        ///   can be obtained using dependency injection by asking it as a new parameter in your 
        ///   controllers' constructor)</param>
        /// <param name="viewModel">The view model whose view should be rendered.</param>
        /// <param name="parameters">The <see cref="AddNewDynamicItem"/> received by the controller.</param>
        /// <param name="options">Any additional options you might want to specify for this item.</param>
        /// 
        /// <returns>An <see cref="ActionResult"/> to be sent back to the client.</returns>
        /// 
        public async static Task<ActionResult> PartialAsync<TViewModel, TOptions>(this PageModel pageModel,
            ICompositeViewEngine engine, TViewModel viewModel, AddNewDynamicItem parameters, Action<TOptions> options)
            where TViewModel : class
            where TOptions : DynamicListItem<TViewModel>, new()
        {
            CheckPostParametersAndThrow(parameters, pageModel);
            IDynamicList item = CreateItem(viewModel, parameters, options);
            return await PartialAsync(pageModel, engine, item: item, parameters);
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
        /// <param name="pageModel">The page model handling the action for creating the new item.</param>
        /// <param name="viewModel">The view model whose view should be rendered.</param>
        /// <param name="parameters">The <see cref="AddNewDynamicItem"/> received by the controller.</param>
        /// <param name="options">Any additional options you might want to specify for this item.</param>
        /// 
        /// <returns>An <see cref="ActionResult"/> to be sent back to the client.</returns>
        /// 
        public static ActionResult Partial<TViewModel, TOptions>(this PageModel pageModel,
            TViewModel viewModel, AddNewDynamicItem parameters, TOptions options)
            where TViewModel : class
            where TOptions : DynamicListItem<TViewModel>, new()
        {
            CheckGetParametersAndThrow(parameters, pageModel);
            IDynamicList item = viewModel.ToDynamicList(parameters.ContainerId!, options);
            return Partial(pageModel, item: item, parameters);
        }

        /// <summary>
        ///    Renders a partial view containing the item to be in plain HTML,
        ///    and converts it to JSON so it can be sent back to the client. You should
        ///    only use this method when using <see cref="NewItemMethod.Post"/>.
        /// </summary>
        /// 
        /// <typeparam name="TViewModel">The type of the view model whose view should be rendered.</typeparam>
        /// 
        /// <param name="pageModel">The page model handling the action for creating the new item.</param>
        /// <param name="viewModel">The view model whose view should be rendered.</param>
        /// <param name="parameters">The <see cref="AddNewDynamicItem"/> received by the controller.</param>
        /// <param name="options">An action that allows for configuring the options to be associated 
        ///     with the new item before it gets inserted to the list.</param>
        /// 
        /// <returns>An <see cref="ActionResult"/> to be sent back to the client.</returns>
        /// 
        public static ActionResult Partial<TViewModel>(this PageModel pageModel,
            TViewModel viewModel, AddNewDynamicItem parameters, Action<DynamicListItem<TViewModel>> options)
            where TViewModel : class
        {
            return Partial<TViewModel, DynamicListItem<TViewModel>>(pageModel, viewModel, parameters, options);
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
        /// <param name="pageModel">The page model handling the action for creating the new item.</param>
        /// <param name="viewModel">The view model whose view should be rendered.</param>
        /// <param name="parameters">The <see cref="AddNewDynamicItem"/> received by the controller.</param>
        /// <param name="options">An action that allows for configuring the options to be associated 
        ///     with the new item before it gets inserted to the list.</param>
        /// 
        /// <returns>An <see cref="ActionResult"/> to be sent back to the client.</returns>
        /// 
        public static ActionResult Partial<TViewModel, TOptions>(this PageModel pageModel,
            TViewModel viewModel, AddNewDynamicItem parameters, Action<TOptions> options)
            where TViewModel : class
            where TOptions : DynamicListItem<TViewModel>, new()
        {
            CheckGetParametersAndThrow(parameters, pageModel);
            IDynamicList item = CreateItem(viewModel, parameters, options);
            return Partial(pageModel, item: item, parameters: parameters);
        }



        /// <summary>
        ///    Creates the partial view for the item that should be sent back to the client.
        /// </summary>
        /// 
        /// <typeparam name="TViewModel">The type of the view model whose view should be rendered.</typeparam>
        /// 
        /// <param name="pageModel">The page model handling the action for creating the new item.</param>
        /// <param name="viewModel">The view model whose view should be rendered.</param>
        /// <param name="parameters">The <see cref="AddNewDynamicItem"/> received by the controller.</param>
        /// 
        /// <returns>An <see cref="ActionResult"/> to be sent back to the client.</returns>
        /// 
        public static ActionResult Partial<TViewModel>(this PageModel pageModel,
            TViewModel viewModel, AddNewDynamicItem parameters)
            where TViewModel : class
        {
            CheckGetParametersAndThrow(parameters, pageModel);
            IDynamicList item = viewModel.ToDynamicList(parameters.ContainerId!);
            return Partial(pageModel, item: item, parameters);
        }

        /// <summary>
        ///    Creates the partial view for the item that should be sent back to the client.
        /// </summary>
        /// 
        /// <param name="pageModel">The page model handling the action for creating the new item.</param>
        /// <param name="parameters">The <see cref="AddNewDynamicItem"/> received by the controller.</param>
        /// <param name="item">The dynamic list item to be rendered.</param>
        /// 
        /// <returns>An <see cref="ActionResult"/> to be sent back to the client.</returns>
        /// 
        public static ActionResult Partial(this PageModel pageModel,
           IDynamicList item, AddNewDynamicItem parameters)
        {
            CheckGetParametersAndThrow(parameters, pageModel);
            PartialViewResult partialView = pageModel.Partial(parameters.ListTemplate, item);
            partialView.ViewData.GetItemEditorParameters(item.Index, parameters, NewItemMethod.Get);
            return partialView;
        }

        /// <summary>
        ///    Renders a partial view containing the item to be in plain HTML,
        ///    and converts it to JSON so it can be sent back to the client. You should
        ///    only use this method when using <see cref="NewItemMethod.Post"/>.
        /// </summary>
        /// 
        /// <param name="pageModel">The page model handling the action for creating the new item.</param>
        /// <param name="engine">An instance of the <see cref="ICompositeViewEngine"/> class (which 
        ///   can be obtained using dependency injection by asking it as a new parameter in your 
        ///   controllers' constructor)</param>
        /// <param name="item">The dynamic list item to be rendered.</param>
        /// <param name="parameters">The <see cref="AddNewDynamicItem"/> received by the controller.</param>
        /// 
        /// <returns>An <see cref="ActionResult"/> to be sent back to the client.</returns>
        /// 
        public static async Task<ActionResult> PartialAsync(this PageModel pageModel,
            ICompositeViewEngine engine, IDynamicList item, AddNewDynamicItem parameters)
        {
            // Notes: This method is based on https://stackoverflow.com/a/31906578/262032

            CheckPostParametersAndThrow(parameters, pageModel);

            ViewEngineResult viewResult = engine
                .FindView(pageModel.PageContext, parameters.ListTemplate, false);

            using (var writer = new StringWriter())
            {
                ViewDataDictionary viewData = new ViewDataDictionary(
                    new Microsoft.AspNetCore.Mvc.ModelBinding.EmptyModelMetadataProvider(),
                    new Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary());

                ViewContext viewContext = new ViewContext(
                    pageModel.PageContext,
                    viewResult.View,
                    viewData,
                    pageModel.TempData,
                    writer,
                    new HtmlHelperOptions()
                );

                viewContext.ViewData.Model = item;
                viewContext.ViewData.GetItemEditorParameters(item.Index, parameters, NewItemMethod.Post);

                try
                {
                    await viewResult.View.RenderAsync(viewContext);
                    string html = writer.GetStringBuilder().ToString();

                    return new JsonResult(new
                    {
                        success = true,
                        html = html
                    });
                }
                catch (Exception ex) when (ex is InvalidOperationException || ex is DynamicListException)
                {
                    string error = $"Failed to render the dynamic list item. If you are using custom templates with " +
                        $"options, make sure you have passed the options object when calling {nameof(PartialAsync)}.";

                    return new JsonResult(new
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

        private static void CheckPostParametersAndThrow(AddNewDynamicItem parameters, PageModel pageModel)
        {
            if (parameters.ContainerId == null)
            {
                throw new ArgumentException("The received new item parameters did not contain a valid " +
                    "containerId. Have you forgotten to add the \"[FromBody]\" attribute to the 'AddNewDynamicItem' " +
                    "parameter in your [HttpPost] controller action?\"", nameof(parameters));
            }

            if (pageModel.Request != null && pageModel.Request.Method != "POST")
            {
                throw new DynamicListException($"The item has been received via GET, but the action method is calling " +
                    $"{nameof(PartialAsync)} instead of {nameof(Partial)}. Please switch it to use {nameof(Partial)}" +
                    $"otherwise the item will not render correctly.")
                {
                    AddNewItemParameters = parameters
                };
            }
        }

        private static void CheckGetParametersAndThrow(AddNewDynamicItem parameters, PageModel pageModel)
        {
            if (parameters.ContainerId == null)
            {
                throw new ArgumentException("The received new item parameters did not contain a valid containerId.", nameof(parameters));
            }

            if (pageModel.Request != null && pageModel.Request.Method != "GET")
            {
                throw new DynamicListException($"The item has been received via POST, but the action method is calling " +
                    $"{nameof(Partial)} instead of {nameof(PartialAsync)}. Please switch it to use {nameof(PartialAsync)}" +
                    $"otherwise the item will not render correctly.")
                {
                    AddNewItemParameters = parameters
                };
            }

            if (parameters.AdditionalViewData != null && parameters.AdditionalViewData.Length > 0)
            {
                throw new ArgumentException($"The received item parameters contain additional view data, but the" +
                    $"item has been received via GET or the controller action method is calling {nameof(Partial)} instead " +
                    $"of {nameof(PartialAsync)}. Please switch to POST and call {nameof(PartialAsync)} in your action " +
                    $"method instead.", nameof(parameters));
            }
        }
    }
}

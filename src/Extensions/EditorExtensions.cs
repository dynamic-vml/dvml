// Dynamic View Model (lists)
// Copyright (c) 2020 César Roberto de Souza. Licensed under the MIT license
// cesarsouza@gmail.com - http://crsouza.com

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

using DynamicVML.Internals;
using DynamicVML.Options;

using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace DynamicVML.Extensions
{
    /// <summary>
    ///   Contains extension methods for rendering different parts of the list from .cshtml view files. 
    /// </summary>
    /// 
    /// <remarks>
    /// <para>
    ///   The methods in this class can be used to render different regions of a dynamic list 
    ///   from within a view. A quick reference for the regions can be seen in the figure below:
    /// </para>
    /// 
    /// <img src="~\images\templates.png"/>
    /// </remarks>
    /// 
    public static partial class EditorExtensions
    {
        #region Display
        /// <summary>
        ///   Renders a dynamic list for display. Please see the image in the remarks section for 
        ///   a quick understanding of all the different templates that can be specified.
        /// </summary>
        /// 
        /// <remarks>
        /// <img src="~\images\templates.png"/>
        /// </remarks>
        /// 
        /// <typeparam name="TModel">The type of the model object in your current view.</typeparam>
        /// <typeparam name="TValue">The list to be displayed.</typeparam>
        /// 
        /// <param name="html">The <see cref="IHtmlHelper{TModel}"/> from your current view.</param>
        /// <param name="propertyExpression">An expresion to be evaluated against the current model.</param>
        /// <param name="itemTemplate">The view name for the view models to be displayed.</param>
        /// <param name="itemContainerTemplate">The template for the "DynamicItemContainer" part of the list.</param>
        /// <param name="mode">The <see cref="ListRenderMode"/> to be used when rendering the template
        ///   for your view model. Please see the <see cref="ListRenderMode"/> for more details.</param>
        /// <param name="listTemplate">The template for the "DynamicList" part of the list.</param>
        /// <param name="listContainerTemplate">The template for the "DynamicListContainer" part of the list.</param>
        /// <param name="additionalViewData">An anonymous object or <see cref="Dictionary{String, String}"/> that can
        ///     contain additional view data that will be merged into the <see cref="ViewDataDictionary"/>
        ///     instance created for the template.</param>
        /// <param name="displayTemplates">The path for where display templates are stored. By default, 
        ///   this is any folder named "DisplayTemplates" in your controller folder or in the Shared folder
        ///   under your Views director.</param>
        /// 
        /// <returns>The rendered HTML for the list.</returns>
        /// 
        public static IHtmlContent DisplayListFor<TModel, TValue>(this IHtmlHelper<TModel> html, Expression<Func<TModel, TValue>> propertyExpression,
            string? itemTemplate = null,
            string? itemContainerTemplate = null,
            string? listTemplate = null,
            string listContainerTemplate = Constants.DefaultListContainerTemplate,
            object? additionalViewData = null,
            string displayTemplates = Constants.DefaultDisplayTemplatesPath,
            ListRenderMode mode = Constants.DefaultRenderMode)
        {
            return html.DisplayFor(propertyExpression, listContainerTemplate, new ListViewDataObject // e.g. listContainerTemplate: DynamicListContainer
            {
                DynamicListDisplayOptions = new DynamicListDisplayOptions()
                {
                    ItemTemplate = itemTemplate,
                    ItemContainerTemplate = itemContainerTemplate,
                    ListTemplate = listTemplate,
                    DisplayTemplates = displayTemplates,
                    Mode = mode
                },
                DynamicListAdditionalViewData = additionalViewData
            });
        }

        /// <summary>
        ///   Renders the "DynamicList" part of the list to the view. Please see the image in
        ///   <see cref="EditorExtensions"/> remarks section to get a quick understanding of 
        ///   which part this refers to.
        /// </summary>
        /// 
        /// <typeparam name="TModel">The type of the model object in your current view.</typeparam>
        /// 
        /// <param name="html">The <see cref="IHtmlHelper{TModel}"/> from your current view.</param>
        /// 
        /// <returns>An awaitable async task.</returns>
        /// 
        public static async Task RenderDynamicListDisplayAsync<TModel>(this IHtmlHelper<TModel> html)
            where TModel : IDynamicList
        {
            DisplayParams param = html.ViewData.GetDisplayParameters();
            await html.RenderPartialAsync(param.ListTemplate, html.ViewData.Model, html.ViewData); // e.g. ListTemplate: DisplayTemplates/DynamicList
        }

        /// <summary>
        ///   Renders the "DynamicItemContainer" part of the list to the view. Please see the image in
        ///   <see cref="EditorExtensions"/> remarks section to get a quick understanding of 
        ///   which part this refers to.
        /// </summary>
        /// 
        /// <typeparam name="TModel">The type of the model object in your current view.</typeparam>
        /// 
        /// <param name="html">The <see cref="IHtmlHelper{TModel}"/> from your current view.</param>
        /// <param name="itemId">The div id (key of the item in the <see cref="DynamicList{TViewModel, TOptions}"/>)
        ///   for the current item being rendered.</param>
        /// 
        /// <returns>An awaitable async task.</returns>
        /// 
        public static IHtmlContent RenderDynamicItemContainerDisplay<TModel>(this IHtmlHelper<TModel> html, string itemId)
            where TModel : IDynamicList
        {
            DisplayParams param = html.ViewData.GetDisplayParameters();
            var newViewData = new ItemViewDataObject
            {
                DynamicListDisplayParams = param,
                DynamicListAdditionalViewData = html.ViewData[Constants.AdditionalViewData],
                DynamicListCurrentIndex = itemId
            };
#if DEBUG
            if (param.ItemContainerTemplate.StartsWith(Constants.DefaultDisplayTemplatesPath))
                throw new Exception("If we call 'EditorFor', we should not include the template paths");
#endif
            return html.DisplayFor(x => x[itemId], param.ItemContainerTemplate,  // e.g. ItemContainerTemplate: DynamicListItemContainer
                additionalViewData: newViewData);
        }

        /// <summary>
        ///   Renders the "Item" part of the list to the view. This corresponds to your actual
        ///   user-provided view for your current viewmodel. Please see the image in
        ///   <see cref="EditorExtensions"/> remarks section to get a quick understanding of 
        ///   which part this refers to.
        /// </summary>
        /// 
        /// <typeparam name="TModel">The type of the model object in your current view.</typeparam>
        /// 
        /// <param name="html">The <see cref="IHtmlHelper{TModel}"/> from your current view.</param>
        /// 
        /// <returns>An awaitable async task.</returns>
        /// 
        public static IHtmlContent RenderDynamicItemDisplay<TModel>(this IHtmlHelper<TModel> html)
            where TModel : IDynamicListItem
        {
            DisplayParams param = html.ViewData.GetDisplayParameters();
            var newViewData = new ItemViewDataObject
            {
                DynamicListDisplayParams = param,
                DynamicListAdditionalViewData = html.ViewData[Constants.AdditionalViewData],
                DynamicListCurrentIndex = html.ViewData.Model.Index
            };
#if DEBUG
            if (param.ItemTemplate.StartsWith(Constants.DefaultDisplayTemplatesPath))
                throw new Exception("If we call 'EditorFor', we should not include the template paths");
#endif
            if (param.Mode == ListRenderMode.ViewModelOnly)
                // Note: even though VisualStudio shows the <TModel, object> part below shadowed, it is actually needed
                return html.DisplayFor<TModel, object>(x => x.ViewModel!, param.ItemTemplate, // e.g. ItemTemplate: "Book"
                    additionalViewData: newViewData);
            if (param.Mode == ListRenderMode.ViewModelWithOptions)
                return html.DisplayForModel(param.ItemTemplate,  // e.g. ItemTemplate: "Book"
                    additionalViewData: newViewData);
            throw new ApplicationException("Invalid DynamicList render mode.");
        }

        #endregion

        #region Editor
        /// <summary>
        ///   Renders a dynamic list for display. Please see the image in 
        ///   <see cref="EditorExtensions"/> remarks section to get a quick understanding of 
        ///   which part this refers to.
        /// </summary>
        /// 
        /// <remarks>
        /// >[!WARNING]
        ///   The <paramref name="additionalViewData"/>
        ///   object will not be sent back to the server when the user clicks the "Add new item" button if 
        ///   <paramref name="method"/> is also set to <see cref="NewItemMethod.Get"/>. However, it will 
        ///   still be passed to the templates during the first rendering of the list. In order to be able 
        ///   to receive the additional data from the controller, please also specify 
        ///   <paramref name="method"/>: <c>NewItemMethod.Post</c>.
        /// </remarks>
        /// 
        /// <typeparam name="TModel">The type of the model object in your current view.</typeparam>
        /// <typeparam name="TValue">The list to be displayed.</typeparam>
        /// 
        /// <param name="actionUrl">The path to the controller action responsible for generating the view for a new item.</param>
        /// <param name="addNewItemText">The text to be displayed in the button that the user can click to add a new item to the list.</param>
        /// <param name="html">The <see cref="IHtmlHelper{TModel}"/> from your current view.</param>
        /// <param name="propertyExpression">An expresion to be evaluated against the current model.</param>
        /// <param name="itemTemplate">The view name for the view models to be displayed.</param>
        /// <param name="itemContainerTemplate">The template for the "DynamicItemContainer" part of the list.</param>
        /// <param name="mode">The <see cref="ListRenderMode"/> to be used when rendering the template
        ///   for your view model. Please see the <see cref="ListRenderMode"/> for more details.</param>
        /// <param name="listTemplate">The template for the "DynamicList" part of the list.</param>
        /// <param name="method">The <see cref="NewItemMethod"/> to use, whether 
        ///     <see cref="NewItemMethod.Get"/> or <see cref="NewItemMethod.Post"/>.</param>
        /// <param name="listContainerTemplate">The template for the "DynamicListContainer" part of the list.</param>
        /// <param name="additionalViewData">An anonymous object or <see cref="Dictionary{String, String}"/> that can
        ///     contain additional view data that will be merged into the <see cref="ViewDataDictionary"/>
        ///     instance created for the template.</param>
        /// <param name="editorTemplates">The path for where editor templates are stored. By default, 
        ///   this is any folder named "EditorTemplates" in your controller folder or in the Shared folder
        ///   under your Views director.</param>
        /// 
        /// <returns>The rendered HTML for the list.</returns>
        /// 
        public static IHtmlContent ListEditorFor<TModel, TValue>(this IHtmlHelper<TModel> html, Expression<Func<TModel, TValue>> propertyExpression,
            string actionUrl, string addNewItemText,
            string? itemTemplate = null,
            string? itemContainerTemplate = null,
            string? listTemplate = null,
            string listContainerTemplate = Constants.DefaultListContainerTemplate,
            object? additionalViewData = null,
            string editorTemplates = Constants.DefaultEditorTemplatesPath,
            ListRenderMode mode = Constants.DefaultRenderMode,
            NewItemMethod method = NewItemMethod.Get)
        {
            return html.EditorFor(propertyExpression, listContainerTemplate, new ListViewDataObject // e.g. listContainerTemplate: DynamicListContainer
            {
                DynamicListEditorOptions = new DynamicListEditorOptions()
                {
                    ItemTemplate = itemTemplate,
                    ItemContainerTemplate = itemContainerTemplate,
                    ListTemplate = listTemplate,

                    ActionUrl = actionUrl,
                    AddNewItemText = addNewItemText,
                    EditorTemplates = editorTemplates,
                    Mode = mode,
                    Method = method
                },

                DynamicListAdditionalViewData = additionalViewData
            });
        }

        /// <summary>
        ///   Renders the "DynamicList" part of the list to the view. Please see the image in
        ///   <see cref="EditorExtensions"/> remarks section to get a quick understanding of 
        ///   which part this refers to.
        /// </summary>
        /// 
        /// <typeparam name="TModel">The type of the model object in your current view.</typeparam>
        /// 
        /// <param name="html">The <see cref="IHtmlHelper{TModel}"/> from your current view.</param>
        /// 
        /// <returns>An awaitable async task.</returns>
        /// 
        public static async Task RenderDynamicListEditorAsync<TModel>(this IHtmlHelper<TModel> html)
            where TModel : IDynamicList
        {
            EditorParams param = html.ViewData.GetEditorParameters();
            html.ViewData[Constants.NewItemParams] = param.CreateNewItemParams(html.ViewData.Model.ContainerId);
            await html.RenderPartialAsync(param.ListTemplate, html.ViewData.Model, html.ViewData); // e.g. ListTemplate: EditorTemplates/DynamicList
        }

        /// <summary>
        ///   Renders the "DynamicItemContainer" part of the list to the view. Please see the image in
        ///   <see cref="EditorExtensions"/> remarks section to get a quick understanding of 
        ///   which part this refers to.
        /// </summary>
        /// 
        /// <typeparam name="TModel">The type of the model object in your current view.</typeparam>
        /// 
        /// <param name="html">The <see cref="IHtmlHelper{TModel}"/> from your current view.</param>
        /// <param name="itemId">The div id (key of the item in the <see cref="DynamicList{TViewModel, TOptions}"/>)
        ///   for the current item being rendered.</param>
        /// 
        /// <returns>An awaitable async task.</returns>
        /// 
        public static IHtmlContent RenderDynamicItemContainerEditor<TModel>(this IHtmlHelper<TModel> html, string itemId)
            where TModel : IDynamicList
        {
            AddNewDynamicItem param = html.ViewData.GetNewItemParameters();
            var newViewData = new ItemViewDataObject
            {
                DynamicListNewItemParams = param,
                DynamicListAdditionalViewData = html.ViewData[Constants.AdditionalViewData],
                DynamicListCurrentIndex = itemId
            };
#if DEBUG
            if (param.ItemContainerTemplate.StartsWith(Constants.DefaultEditorTemplatesPath))
                throw new Exception("If we call 'EditorFor', we should not include the template paths");
#endif
            return html.EditorFor(x => x[itemId], param.ItemContainerTemplate, // e.g. ItemContainerTemplate: DynamicListItemContainer
                additionalViewData: newViewData);
        }

        /// <summary>
        ///   Renders the "Item" part of the list to the view. This corresponds to your actual
        ///   user-provided view for your current viewmodel. Please see the image in
        ///   <see cref="EditorExtensions"/> remarks section to get a quick understanding of 
        ///   which part this refers to.
        /// </summary>
        /// 
        /// <typeparam name="TModel">The type of the model object in your current view.</typeparam>
        /// 
        /// <param name="html">The <see cref="IHtmlHelper{TModel}"/> from your current view.</param>
        /// 
        /// <returns>An awaitable async task.</returns>
        /// 
        public static IHtmlContent RenderDynamicItemEditor<TModel>(this IHtmlHelper<TModel> html)
            where TModel : IDynamicListItem
        {
            AddNewDynamicItem param = html.ViewData.GetNewItemParameters();
            var newViewData = new ItemViewDataObject
            {
                DynamicListNewItemParams = param,
                DynamicListAdditionalViewData = html.ViewData[Constants.AdditionalViewData],
                DynamicListCurrentIndex = html.ViewData.Model.Index
            };
#if DEBUG
            if (param.ItemTemplate.StartsWith("EditorTemplates"))
                throw new Exception("If we call 'EditorFor', we should not include the template paths");
#endif
            if (param.Mode == ListRenderMode.ViewModelOnly)
                // Note: even though VisualStudio shows the <TModel, object> part below shadowed, it is actually needed
                return html.EditorFor<TModel, object>(x => x.ViewModel!, param.ItemTemplate, // e.g. ItemTemplate: "Book"
                    additionalViewData: newViewData);
            if (param.Mode == ListRenderMode.ViewModelWithOptions)
                return html.EditorForModel(param.ItemTemplate,  // e.g. ItemTemplate: "Book"
                    additionalViewData: newViewData);
            throw new ApplicationException("Invalid DynamicList render mode.");
        }


        /// <summary>
        ///   Renders the "Add new item" text that have been defined in the
        ///   <see cref="DynamicListEditorOptions.AddNewItemText"/> to the view.
        /// </summary>
        /// 
        /// <param name="html">The <see cref="IHtmlHelper{TModel}"/> associated with the current view.</param>
        /// 
        /// <returns>A string containing the specified text, e.g., "Add new book".</returns>
        /// 
        public static string RenderDynamicListAddNewItemText(this IHtmlHelper<IDynamicList> html)
        {
            // TODO: replace string with IHtmlContent to allow more flexibility
            return html.ViewData.GetEditorParameters().AddNewItemText;
        }

        /// <summary>
        ///   Gets a string with information about how request a new item from the server.
        ///   Please see <see cref="EditorParams.GetActionContent(string)"/> for more information.
        /// </summary>
        /// 
        /// <param name="html">The <see cref="IHtmlHelper{TModel}"/> associated with the current view.</param>
        /// 
        /// <returns>A string containing information to be passed to the JavaScript scripts.</returns>
        /// 
        public static string GetDynamicListActionUrl(this IHtmlHelper<IDynamicList> html)
        {
            return html.ViewData.GetEditorParameters().GetActionContent(html.ViewData.Model.ContainerId);
        }
        #endregion
    }
}

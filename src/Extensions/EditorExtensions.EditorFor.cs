// Dynamic View Model (lists)
// Copyright (c) 2020 César Roberto de Souza. Licensed under the MIT license
// cesarsouza@gmail.com - http://crsouza.com

using System;
using System.Collections.Generic;
using System.Diagnostics;
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
      
        /// <summary>
        ///   Renders a dynamic list for display. Please see the image in the
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
        /// 
        /// <returns>The rendered HTML for the list.</returns>
        /// 
        public static IHtmlContent ListEditorFor<TModel, TValue>(this IHtmlHelper<TModel> html, Expression<Func<TModel, TValue>> propertyExpression,
            string? actionUrl = null, string? addNewItemText = null,
            string? itemTemplate = null,
            string? itemContainerTemplate = null,
            string? listTemplate = null,
            string listContainerTemplate = Constants.DefaultListContainerTemplate,
            object? additionalViewData = null,
            ListRenderMode mode = Constants.DefaultRenderMode,
            NewItemMethod method = NewItemMethod.Get)
            where TValue : IDynamicList
        {
            PackViewData(html, propertyExpression, new ViewDataObject
            {
                EditorOptions = new DynamicListEditorOptions()
                {
                    ItemTemplate = itemTemplate,
                    ItemContainerTemplate = itemContainerTemplate,
                    ListTemplate = listTemplate,

                    ActionUrl = actionUrl,
                    AddNewItemText = addNewItemText,
                    Mode = mode,
                    Method = method
                },

                AdditionalViewData = additionalViewData
            });

            try
            {
                IHtmlContent output = html.EditorFor(
                    expression: propertyExpression,
                    templateName: listContainerTemplate); // e.g. listContainerTemplate: DynamicListContainer
                    
                return output;
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
                throw ex;
            }
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
        public static void RenderDynamicListEditor<TModel>(this IHtmlHelper<TModel> html)
            where TModel : IDynamicList
        {
            ListEditorParameters param = html.ViewData.GetListEditorParameters(html.ViewData.Model.ContainerId);

            try
            {
                html.RenderPartial(
                    partialViewName: param.List.ListTemplate, // e.g. ListTemplate: EditorTemplates/DynamicListContainer
                    model: html.ViewData.Model,
                    viewData: html.ViewData);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
                throw;
            }
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
            ItemEditorParameters param = html.ViewData.GetItemEditorParameters(itemId, html.ViewData.Model.ContainerId);

            try
            {
                IHtmlContent output = html.EditorFor(expression: x => x[itemId],
                    templateName: param.Editor.List.ItemContainerTemplate); // e.g. ItemContainerTemplate: DynamicListItemContainer
                    
                return output;
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
                throw;
            }
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
            ItemEditorParameters param = html.ViewData.GetItemEditorParameters(html.ViewData.Model.Index);

            try
            {
                if (param.Editor.List.Mode == ListRenderMode.ViewModelOnly)
                {
                    return html.EditorFor(expression: x => x.ViewModel,
                        templateName: param.Editor.List.ItemTemplate); // e.g. ItemTemplate: "Book"
                }

                if (param.Editor.List.Mode == ListRenderMode.ViewModelWithOptions)
                {
                    html.RenderPartial(
                        partialViewName: Constants.EditorTemplates + param.Editor.List.ItemTemplate, // e.g. ItemTemplate: "Book"
                        model: html.ViewData.Model,
                        viewData: html.ViewData);
                }

                throw new ApplicationException("Invalid DynamicList render mode.");
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
                throw;
            }
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
            ListEditorParameters param = html.ViewData.GetListEditorParameters(html.ViewData.Model.ContainerId);
            return param.AddNewItemText;
        }

        /// <summary>
        ///   Gets a string with information about how request a new item from the server.
        ///   Please see <see cref="ListEditorParameters.GetActionContent()"/> for more information.
        /// </summary>
        /// 
        /// <param name="html">The <see cref="IHtmlHelper{TModel}"/> associated with the current view.</param>
        /// 
        /// <returns>A string containing information to be passed to the JavaScript scripts.</returns>
        /// 
        public static string GetDynamicListActionUrl(this IHtmlHelper<IDynamicList> html)
        {
            ListEditorParameters param = html.ViewData.GetListEditorParameters(html.ViewData.Model.ContainerId);
            return param.GetActionContent();
        }

    }
}

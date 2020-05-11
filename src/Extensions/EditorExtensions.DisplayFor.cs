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
        /// 
        /// <returns>The rendered HTML for the list.</returns>
        /// 
        public static IHtmlContent DisplayListFor<TModel, TValue>(this IHtmlHelper<TModel> html, Expression<Func<TModel, TValue>> propertyExpression,
            string? itemTemplate = null,
            string? itemContainerTemplate = null,
            string? listTemplate = null,
            string listContainerTemplate = Constants.DefaultListContainerTemplate,
            object? additionalViewData = null,
            ListRenderMode mode = Constants.DefaultRenderMode)
            where TValue : IDynamicList
        {
            var obj = PackViewData(html, propertyExpression, new ListViewDataObject
            {
                DynamicListDisplayOptions = new DynamicListDisplayOptions()
                {
                    ItemTemplate = itemTemplate,
                    ItemContainerTemplate = itemContainerTemplate,
                    ListTemplate = listTemplate,
                    Mode = mode
                },
                DynamicListAdditionalViewData = additionalViewData
            });

            try
            {
                IHtmlContent output = html.DisplayFor(propertyExpression,
                    listContainerTemplate, // e.g. listContainerTemplate: DynamicListContainer
                    additionalViewData: obj);
                return output;
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
                throw;
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
        public static void RenderDynamicListDisplay<TModel>(this IHtmlHelper<TModel> html)
            where TModel : IDynamicList
        {
            DisplayParams param = html.ViewData.GetDisplayParameters(html.ViewData.Model.ContainerId);

            // Note: MVC prevents us from calling Display/DisplayFor for the same object multiple times. This
            // happens due the check at TemplateBuilder next to the comment:
            //
            //      Normally this shouldn't happen, unless someone writes their own custom Object templates which
            //      don't check to make sure that the object hasn't already been displayed
            //
            // So instead of calling DisplayFor again, we will use RenderPartial, which should not have the same issue

            try
            {
                html.RenderPartial(param.ListTemplate, // e.g. ListTemplate: DynamicListContainer
                    html.ViewData.Model,
                    html.ViewData);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
                throw;
            }
        }

        /// <summary>
        ///   Renders the "DynamicItemContainer" part of the list to the view. Please see the image
        ///   in the <see cref="EditorExtensions"/> remarks section to get a quick understanding of 
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
            DisplayItemParams param = html.ViewData.GetDisplayItemParameters(itemId, html.ViewData.Model.ContainerId);

            try
            {
                IHtmlContent output = html.DisplayFor(x => x[itemId],
                    param.DisplayParams.ItemContainerTemplate,  // e.g. ItemContainerTemplate: DynamicItemContainer
                    additionalViewData: param);
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
        public static IHtmlContent RenderDynamicItemDisplay<TModel>(this IHtmlHelper<TModel> html)
            where TModel : IDynamicListItem
        {
            DisplayItemParams param = html.ViewData.GetDisplayItemParameters(html.ViewData.Model.Index);

            try
            {
                if (param.DisplayParams.Mode == ListRenderMode.ViewModelOnly)
                {
                    return html.DisplayFor(x => x.ViewModel,
                        param.DisplayParams.ItemTemplate, // e.g. ItemTemplate: "Book"
                        additionalViewData: param);
                }

                if (param.DisplayParams.Mode == ListRenderMode.ViewModelWithOptions)
                {
                    html.RenderPartial("DisplayTemplates/" + param.DisplayParams.ItemTemplate, // e.g. ItemTemplate: "Book"
                        html.ViewData.Model,
                        html.ViewData);
                }

                throw new ApplicationException("Invalid DynamicList render mode.");
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
                throw;
            }
        }

    }
}

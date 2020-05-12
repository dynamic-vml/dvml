// Dynamic View Model (lists)
// Copyright (c) 2020 César Roberto de Souza. Licensed under the MIT license
// cesarsouza@gmail.com - http://crsouza.com

using System;

using DynamicVML.Internals;
using DynamicVML.Options;

using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace DynamicVML.Extensions
{

    /// <summary>
    ///   Contains extension methods for extracting and storing information
    ///   related to <see cref="DynamicList{TViewModel, TOptions}">DynamicLists</see>
    ///   from <see cref="ViewDataDictionary"/> objects.
    /// </summary>
    /// 
    public static partial class ViewDataExtensions
    {

        /// <summary>
        ///   Builds a new <see cref="ListDisplayParameters"/> object gathering information from different
        ///   sources, including <see cref="DynamicListDisplayOptions"/> objects that may be stored in the
        ///   view data, <see cref="DynamicListAttribute"/> attributes defined in the view model class,
        ///   or, as a last resort, reflection.
        /// </summary>
        /// 
        /// <remarks>
        /// >[!WARNING]
        ///   This method will resort to reflecion in case the <see cref="DynamicListOptions.ItemTemplate"/>,
        ///   <see cref="DynamicListEditorOptions.ActionUrl"/>, or <see cref="DynamicListEditorOptions.AddNewItemText"/>
        ///   have not been specified, which can incur
        ///   a significant performance impact on the server. To avoid the extra performance hit, specify the name 
        ///   of the view you want to use for your view model when calling <see cref="EditorExtensions.DisplayListFor"/>
        /// </remarks>
        /// 
        /// <param name="viewData">The view data object from where information will be extracted.</param>
        /// <param name="containerId">The HTML div element ID for the current list.</param>
        /// 
        /// <returns>
        ///     A new <see cref="ListDisplayParameters"/> object with the actual values to be 
        ///     used when rendering the editor view for your view model.
        /// </returns>
        /// 
        public static ListDisplayParameters GetListDisplayParameters(this ViewDataDictionary viewData, string containerId)
        {
            var viewDataObject = viewData[containerId] as ViewDataObject;
            if (viewDataObject == null)
            {
                throw new ApplicationException($"Could not find a {nameof(ViewDataObject)} for the container {containerId}. " +
                    $"Please make sure to call the {nameof(EditorExtensions.DisplayListFor)} method when attempting to show " +
                    $"a display for an {nameof(IDynamicList)}.");
            }

            if (viewDataObject.DisplayParameters != null)
                return viewDataObject.DisplayParameters;

            ListRenderMode mode = Constants.DefaultRenderMode;
            string? listTemplate = null;
            string? itemContainerTemplate = null;
            string? itemTemplate = null;

            DynamicListAttribute? attribute = GetDynamicListAttribute(viewData);
            if (attribute != null)
            {
                if (attribute.ListTemplate != null)
                    listTemplate = attribute.ListTemplate;
                if (attribute.ItemContainerTemplate != null)
                    itemContainerTemplate = attribute.ItemContainerTemplate;
                if (attribute.ItemTemplate != null)
                    itemTemplate = attribute.ItemTemplate;
            }

            if (listTemplate == null || itemContainerTemplate == null)
            {
                DynamicListDisplayOptions? options = viewDataObject.DisplayOptions;
                if (options == null)
                    throw new ApplicationException("The DynamicList view did not contain the DynamicList display options in its view data.");
                if (options.Mode != null)
                    mode = options.Mode.Value;
                if (options.ItemTemplate != null)
                    itemTemplate = options.ItemTemplate;
                if (options.ListTemplate != null)
                    listTemplate = options.ListTemplate;
                if (options.ItemContainerTemplate != null)
                    itemContainerTemplate = options.ItemContainerTemplate;
            }

            // TODO: add a warning that if the viewmodel name is not specified 
            // anywhere, the code resorts to reflection, which can be quite slow
            if (itemTemplate == null)
                itemTemplate = GetViewModelTypeName(viewData); // user-defined view, e.g. "Book"
            if (itemContainerTemplate == null)
                itemContainerTemplate = Constants.DefaultItemContainerTemplate; // eg. DynamicItemContainer
            if (listTemplate == null)
                listTemplate = Constants.DefaultListTemplate; // eg. DynamicList

            listTemplate = Constants.DisplayTemplates + listTemplate; // e.g. DisplayTemplates/DynamicList

            string prefix = viewData.TemplateInfo.HtmlFieldPrefix;
            object? userData = GetUserDataAndRemoveFromView(viewData);

            var displayParameters = new ListDisplayParameters(
                parameters: new ListParameters(
                            containerId: containerId,
                            itemTemplate: itemTemplate,
                            itemContainerTemplate: itemContainerTemplate,
                            listTemplate: listTemplate,
                            prefix: prefix,
                            mode: mode),
                additionalViewData: userData);

            viewDataObject.DisplayParameters = displayParameters;
            viewData[Constants.ListDisplayParameters] = displayParameters;
            return displayParameters;
        }

        /// <summary>
        ///   Retrieves the <see cref="ItemDisplayParameters"/> object stored in the <see cref="ViewDataDictionary"/>.
        ///   If no <see cref="ItemDisplayParameters"/> is available, a new one will be created from the information 
        ///   stored in the <see cref="ListDisplayParameters"/> object that should have been stored in the ViewData. 
        /// </summary>
        /// 
        /// <param name="viewData">The view data object from where information will be extracted.</param>
        /// <param name="containerId">The HTML div element ID for the current list.</param>
        /// <param name="itemId">The HTML div element ID for the current list item.</param>
        /// 
        public static ItemDisplayParameters GetItemDisplayParameters(this ViewDataDictionary viewData,
            string? itemId, string? containerId = null)
        {
            if (itemId == null)
                throw new ArgumentNullException(nameof(itemId));

            if (viewData.TryGetValue(itemId, out object obj))
            {
                if (obj != null && obj is ItemDisplayParameters d)
                    return d;
            }

            if (containerId == null)
                throw new ArgumentNullException(nameof(containerId));

            ListDisplayParameters listDisplayParameters = viewData.GetListDisplayParameters(containerId);

            var itemDisplayParameters = new ItemDisplayParameters(containerId, itemId, listDisplayParameters);

            viewData[Constants.AdditionalViewData] = listDisplayParameters.AdditionalViewData;
            viewData[Constants.ListDisplayParameters] = listDisplayParameters;
            viewData[Constants.ItemDisplayParameters] = itemDisplayParameters;
            viewData[Constants.CurrentContainerId] = containerId;
            viewData[Constants.CurrentIndex] = itemId;

            viewData[itemId] = itemDisplayParameters;
            return itemDisplayParameters;
        }
    }
}

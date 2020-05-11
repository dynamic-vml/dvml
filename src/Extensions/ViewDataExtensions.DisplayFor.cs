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
                throw new ApplicationException();

            object? additionalViewData = viewData[Constants.AdditionalViewData];
            ListDisplayParameters listParameters = viewData.GetListDisplayParameters(containerId);

            var itemParameters = new ItemDisplayParameters(containerId, itemId, additionalViewData, listParameters);

            viewData[Constants.AdditionalViewData] = additionalViewData;
            viewData[Constants.ListDisplayParameters] = listParameters;
            viewData[Constants.ItemDisplayParameters] = itemParameters;
            viewData[Constants.CurrentContainerId] = containerId;
            viewData[Constants.CurrentIndex] = itemId;

            viewData[itemId] = itemParameters;
            return itemParameters;
        }

        /// <summary>
        ///   Builds a new <see cref="ListDisplayParameters"/> object gathering information from different
        ///   sources, including <see cref="DynamicListDisplayOptions"/> objects that may be stored in the
        ///   view data, <see cref="DynamicListAttribute"/> attributes defined in the view model class,
        ///   or, as a last resort, reflection.
        /// </summary>
        /// 
        /// <remarks>
        /// >[!WARNING]
        ///   This method will resort to reflecion in case the <see cref="DynamicListEditorOptions.ItemTemplate"/>,
        ///   <see cref="DynamicListEditorOptions.ActionUrl"/>, or <see cref="DynamicListEditorOptions.AddNewItemText"/>
        ///   have not been specified, which can incur
        ///   a significant performance impact on the server. To avoid the extra performance hit, specify the name 
        ///   of the view you want to use for your view model when calling <see cref="EditorExtensions.DisplayListFor"/>
        /// </remarks>
        /// 
        /// <param name="viewData">The view data object from where information will be extracted.</param>
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

            listTemplate = Constants.DisplayTemplates + listTemplate;

            string prefix = viewData.TemplateInfo.HtmlFieldPrefix;
            object? userData = GetUserDataAndRemoveFromView(viewData);

            var displayParameters = new ListDisplayParameters(
                parameters: new ListParameters(
                            containerId: containerId,
                            itemTemplate: itemTemplate,
                            itemContainerTemplate: itemContainerTemplate!,
                            listTemplate: listTemplate,
                            prefix: prefix,
                            mode: mode),
                additionalViewData: userData);

            viewDataObject.DisplayParameters = displayParameters;
            viewData[Constants.ListDisplayParameters] = displayParameters;
            return displayParameters;
        }

    }
}

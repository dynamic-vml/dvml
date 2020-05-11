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
        ///   Extracts a <see cref="AddNewDynamicItem"/> object from a <see cref="ViewDataDictionary">ViewData</see>
        ///   dictionary.
        /// </summary>
        /// 
        /// <param name="viewData">
        ///     The view data dictionary that should contain the object. If the object is
        ///     not present, an <see cref="ApplicationException"/> will be thrown.
        /// </param>
        /// 
        /// <returns>The <see cref="AddNewDynamicItem"/> stored in the view data.</returns>
        /// 
        public static AddNewDynamicItem GetNewItemParameters(this ViewDataDictionary viewData, string itemId)
        {
            if (viewData.TryGetValue(itemId, out object obj))
                if (obj is AddNewDynamicItem param)
                    return param;

            throw new ApplicationException($"Could not extract {nameof(AddNewDynamicItem)} from the ViewData. " +
                $"Make sure you are using the {nameof(ControllerExtensions.PartialView)} extension method to " +
                "create the dynamic item view in your controllers.");
        }

        public static void SetItemEditorParameters(this ViewDataDictionary viewData, 
            string? itemId, AddNewDynamicItem newItemParameters, NewItemMethod method)
        {
            if (itemId == null)
                throw new ArgumentNullException(nameof(itemId));

            var additionalViewData = newItemParameters.GetAdditionalViewData();

            if (newItemParameters.ContainerId == null)
                throw new ApplicationException($"The {nameof(AddNewDynamicItem)} object set by the controller does not contain a valid ContainerId.");
            if (newItemParameters.ItemTemplate == null)
                throw new ApplicationException($"The {nameof(AddNewDynamicItem)} object set by the controller does not contain a valid ItemTemplate.");
            if (newItemParameters.ItemContainerTemplate == null)
                throw new ApplicationException($"The {nameof(AddNewDynamicItem)} object set by the controller does not contain a valid ItemContainerTemplate.");
            if (newItemParameters.Prefix == null)
                throw new ApplicationException($"The {nameof(AddNewDynamicItem)} object set by the controller does not contain a valid Prefix.");

            string containerId = newItemParameters.ContainerId;

            var editorParameters = new ListEditorParameters(
                parameters: new ListParameters(
                    containerId: containerId,
                    itemTemplate: newItemParameters.ItemTemplate,
                    itemContainerTemplate: newItemParameters.ItemContainerTemplate,
                    listTemplate: String.Empty,
                    prefix: newItemParameters.Prefix,
                    mode: newItemParameters.Mode),
                    actionUrl: String.Empty,
                    addNewItemText: String.Empty,
                    additionalViewData: additionalViewData,
                    method: method);

            var itemParameters = new ItemEditorParameters(containerId, itemId, additionalViewData, newItemParameters, editorParameters);

            viewData[Constants.AdditionalViewData] = additionalViewData;
            viewData[Constants.ListEditorParameters] = editorParameters;
            viewData[Constants.ItemEditorParameters] = itemParameters;
            viewData[Constants.ItemCreatorParameters] = newItemParameters;
            viewData[Constants.CurrentContainerId] = containerId;
            viewData[Constants.CurrentIndex] = itemId;

            viewData[itemId] = itemParameters;

            foreach (var pair in additionalViewData)
                viewData[pair.Key] = pair.Value;

            viewData.TemplateInfo.HtmlFieldPrefix = newItemParameters.Prefix;
        }


        public static ItemEditorParameters GetItemEditorParameters(this ViewDataDictionary viewData, 
            string? itemId, string? containerId = null)
        {
            if (itemId == null)
                throw new ArgumentNullException(nameof(itemId));

            if (viewData.TryGetValue(itemId, out object obj))
            {
                if (obj != null && obj is ItemEditorParameters e)
                    return e;
            }

            if (containerId == null)
                throw new ApplicationException();

            ListEditorParameters editorParameters = viewData.GetListEditorParameters(containerId);
            AddNewDynamicItem newItemParameters = editorParameters.CreateNewItemParams();
            object? additionalViewData = viewData[Constants.AdditionalViewData];

            var itemParameters = new ItemEditorParameters(containerId, itemId, additionalViewData, newItemParameters, editorParameters);

            viewData[Constants.AdditionalViewData] = additionalViewData;
            viewData[Constants.ListEditorParameters] = editorParameters;
            viewData[Constants.ItemEditorParameters] = itemParameters;
            viewData[Constants.ItemCreatorParameters] = newItemParameters;
            viewData[Constants.CurrentContainerId] = containerId;
            viewData[Constants.CurrentIndex] = itemId;

            viewData[itemId] = itemParameters;
            return itemParameters;
        }

        /// <summary>
        ///   Builds a new <see cref="ListEditorParameters"/> object gathering information from different
        ///   sources, including <see cref="DynamicListEditorOptions"/> objects that may be stored in the
        ///   view data, <see cref="DynamicListAttribute"/> attributes defined in the view model class,
        ///   or, as a last resort, reflection.
        /// </summary>
        /// 
        /// <remarks>
        /// >[!WARNING]
        ///   This method will resort to reflecion in case the <see cref="DynamicListEditorOptions.ItemTemplate"/>
        ///   has not been specified, which can incur a significant performance impact on the server. To avoid the 
        ///   extra performance hit, specify the name of the view you want to use for your view model when calling
        ///   <see cref="EditorExtensions.ListEditorFor"/>
        /// </remarks>
        /// 
        /// <param name="viewData">The view data object from where information will be extracted.</param>
        /// 
        /// <returns>
        ///     A new <see cref="ListEditorParameters"/> object with the actual values to be 
        ///     used when rendering the editor view for your view model.
        /// </returns>
        /// 
        public static ListEditorParameters GetListEditorParameters(this ViewDataDictionary viewData, string containerId)
        {
            var viewDataObject = viewData[containerId] as ViewDataObject;
            if (viewDataObject == null)
            {
                throw new ApplicationException($"Could not find a {nameof(ViewDataObject)} for the container {containerId}. " +
                    $"Please make sure to call the {nameof(EditorExtensions.ListEditorFor)} method when attempting to show " +
                    $"an editor for an {nameof(IDynamicList)}.");
            }

            if (viewDataObject.EditorParameters != null)
                return viewDataObject.EditorParameters;

            ListRenderMode mode = Constants.DefaultRenderMode;
            string? listTemplate = null;
            string? itemContainerTemplate = null;
            string? itemTemplate = null;
            string? addNewItemText = null;
            string? actionUrl = null;
            NewItemMethod method = NewItemMethod.Get;

            DynamicListAttribute? attribute = GetDynamicListAttribute(viewData);
            if (attribute != null)
            {
                if (attribute.ListTemplate != null)
                    listTemplate = attribute.ListTemplate;
                if (attribute.ItemContainerTemplate != null)
                    itemContainerTemplate = attribute.ItemContainerTemplate;
                if (attribute.ItemTemplate != null)
                    itemTemplate = attribute.ItemTemplate;
                method = attribute.Method;
                mode = attribute.Mode;
            }

            if (listTemplate == null || itemContainerTemplate == null)
            {
                DynamicListEditorOptions? options = viewDataObject.EditorOptions;
                if (options == null)
                    throw new ApplicationException("The DynamicList view did not contain the DynamicList editor options in its view data.");
                addNewItemText = options.AddNewItemText;
                actionUrl = options.ActionUrl;
                if (options.ItemTemplate != null)
                    itemTemplate = options.ItemTemplate;
                if (options.ListTemplate != null)
                    listTemplate = options.ListTemplate;
                if (options.ItemContainerTemplate != null)
                    itemContainerTemplate = options.ItemContainerTemplate;
                if (options.Method != null)
                    method = options.Method.Value;
                if (options.Mode != null)
                    mode = options.Mode.Value;
            }

            string? viewModelTypeName = null;
            if (actionUrl == null)
            {
                viewModelTypeName = GetViewModelTypeName(viewData);
                actionUrl = $"Add{viewModelTypeName}"; // user-defined action, e.g. "Authors/AddBook"
            }
            if (itemTemplate == null)
            {
                viewModelTypeName ??= GetViewModelTypeName(viewData);
                itemTemplate = viewModelTypeName; // user-defined view, e.g. "Book"
            }
            if (addNewItemText == null)
            {
                viewModelTypeName ??= GetViewModelTypeName(viewData);
                addNewItemText = $"Add new {viewModelTypeName}";
            }

            if (itemContainerTemplate == null)
                itemContainerTemplate = Constants.DefaultItemContainerTemplate; // eg. DynamicListItem
            if (listTemplate == null)
                listTemplate = Constants.DefaultListTemplate; // eg. DynamicList

            listTemplate = Constants.EditorTemplates + listTemplate;

            string prefix = viewData.TemplateInfo.HtmlFieldPrefix;
            object? userData = GetUserDataAndRemoveFromView(viewData);

            var editorParameters = new ListEditorParameters(
                parameters: new ListParameters(
                                containerId: containerId,
                                itemTemplate: itemTemplate,
                                itemContainerTemplate: itemContainerTemplate!,
                                listTemplate: listTemplate,
                                prefix: prefix,
                                mode: mode),
                actionUrl: actionUrl,
                addNewItemText: addNewItemText,
                additionalViewData: userData,
                method: method);

            viewDataObject.EditorParameters = editorParameters;
            viewData[Constants.ListEditorParameters] = editorParameters;
            return editorParameters;
        }

    }
}

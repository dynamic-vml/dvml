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
        ///   Builds a new <see cref="ListEditorParameters"/> object gathering information from different
        ///   sources, including <see cref="DynamicListEditorOptions"/> objects that may be stored in the
        ///   view data, <see cref="DynamicListAttribute"/> attributes defined in the view model class,
        ///   or, as a last resort, reflection.
        /// </summary>
        /// 
        /// <remarks>
        /// >[!WARNING]
        ///   This method will resort to reflecion in case the <see cref="DynamicListOptions.ItemTemplate"/>
        ///   has not been specified, which can incur a significant performance impact on the server. To avoid the 
        ///   extra performance hit, specify the name of the view you want to use for your view model when calling
        ///   <see cref="EditorExtensions.ListEditorFor"/>
        /// </remarks>
        /// 
        /// <param name="viewData">The view data object from where information will be extracted.</param>
        /// <param name="containerId">The HTML div element ID for the current list.</param>
        /// 
        /// <returns>
        ///     A new <see cref="ListEditorParameters"/> object with the actual values
        ///     to be used when rendering the editor view for your view model.
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

            listTemplate = Constants.EditorTemplates + listTemplate; // e.g. EditorTemplates/DynamicList

            string prefix = viewData.TemplateInfo.HtmlFieldPrefix;
            object? userData = GetUserDataAndRemoveFromView(viewData);

            var editorParameters = new ListEditorParameters(
                parameters: new ListParameters(
                                containerId: containerId,
                                itemTemplate: itemTemplate,
                                itemContainerTemplate: itemContainerTemplate,
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

        /// <summary>
        ///   Retrieves the <see cref="ItemEditorParameters"/> object stored in the <see cref="ViewDataDictionary"/>.
        ///   If no <see cref="ItemEditorParameters"/> is available, a new one will be created from the information 
        ///   stored in the <see cref="ListEditorParameters"/> object that should have been stored in the ViewData. 
        /// </summary>
        /// 
        /// <param name="viewData">The view data object from where information will be extracted.</param>
        /// <param name="containerId">The HTML div element ID for the current list.</param>
        /// <param name="itemId">The HTML div element ID for the current list item.</param>
        /// 
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
                throw new ArgumentNullException(nameof(containerId));

            ListEditorParameters listEditorParameters = viewData.GetListEditorParameters(containerId);
            AddNewDynamicItem itemCreateParameters = listEditorParameters.GetItemCreateParameters();

            var itemEditorParameters = new ItemEditorParameters(containerId, itemId, itemCreateParameters, listEditorParameters);

            viewData[Constants.AdditionalViewData] = listEditorParameters.AdditionalViewData;
            viewData[Constants.ListEditorParameters] = listEditorParameters;
            viewData[Constants.ItemEditorParameters] = itemEditorParameters;
            viewData[Constants.ItemCreateParameters] = itemCreateParameters;
            viewData[Constants.CurrentContainerId] = containerId;
            viewData[Constants.CurrentIndex] = itemId;

            viewData[itemId] = itemEditorParameters;
            return itemEditorParameters;
        }

        /// <summary>
        ///   Re-creates a <see cref="ItemEditorParameters"/> from the partial information about the original list
        ///   contained in a <see cref="AddNewDynamicItem"/> object. This information will be stored in the
        ///   <see cref="ViewDataDictionary"/> in order to make the rest of the code work the same as much as
        ///   possible.
        /// </summary>
        /// 
        /// <param name="viewData">The view data object from where information will be extracted.</param>
        /// <param name="itemId">The HTML div element ID for the current list item.</param>
        /// <param name="method">The HTTP method used when calling the controller to add new items to the list.</param>
        /// <param name="newItemParameters">The <see cref="AddNewDynamicItem"/> parameters containing information
        ///     about how the controller should create new items to be added to the list.</param>
        /// 
        public static ItemEditorParameters GetItemEditorParameters(this ViewDataDictionary viewData,
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

            // Re-create the editor parameters from the information we have available. We do not
            // have all the information needed for a full reconstruction because those informations
            // have not been passed to the controller via the AddNewDynamicItem object. However,
            // the missing information should not be needed anymore as it was required only for
            // creating new items or displaying the list container (we are now at the item level).

            var editorParameters = new ListEditorParameters(
                parameters: new ListParameters(
                                containerId: containerId,
                                itemTemplate: newItemParameters.ItemTemplate,
                                itemContainerTemplate: newItemParameters.ItemContainerTemplate,
                                listTemplate: String.Empty, // we do not have this information at this point
                                prefix: newItemParameters.Prefix,
                                mode: newItemParameters.Mode),
                actionUrl:  String.Empty, // we do not have this information at this point 
                addNewItemText: String.Empty, // we do not have this information at this point
                additionalViewData: additionalViewData,
                method: method);

            var itemParameters = new ItemEditorParameters(containerId, itemId, newItemParameters, editorParameters);

            viewData[Constants.AdditionalViewData] = additionalViewData;
            viewData[Constants.ListEditorParameters] = editorParameters;
            viewData[Constants.ItemEditorParameters] = itemParameters;
            viewData[Constants.ItemCreateParameters] = newItemParameters;
            viewData[Constants.CurrentContainerId] = containerId;
            viewData[Constants.CurrentIndex] = itemId;

            viewData[itemId] = itemParameters;

            foreach (var pair in additionalViewData)
                viewData[pair.Key] = pair.Value;

            viewData.TemplateInfo.HtmlFieldPrefix = newItemParameters.Prefix;

            return itemParameters;
        }

        /// <summary>
        ///   Extracts a <see cref="AddNewDynamicItem"/> object from a <see cref="ViewDataDictionary">ViewData</see>
        ///   dictionary.
        /// </summary>
        /// 
        /// <param name="viewData">
        ///     The view data dictionary that should contain the object. If the object is
        ///     not present, an <see cref="ApplicationException"/> will be thrown.
        /// </param>
        /// <param name="itemId">The HTML div ID for the current list item.</param>
        /// 
        /// <returns>The <see cref="AddNewDynamicItem"/> stored in the view data.</returns>
        /// 
        public static AddNewDynamicItem GetItemCreateParameters(this ViewDataDictionary viewData, string itemId)
        {
            if (viewData.TryGetValue(itemId, out object obj))
                if (obj is AddNewDynamicItem param)
                    return param;

            throw new ApplicationException($"Could not extract {nameof(AddNewDynamicItem)} from the ViewData. " +
                $"Make sure you are using the {nameof(ControllerExtensions.PartialView)} extension method to " +
                "create the dynamic item view in your controllers.");
        }

    }
}

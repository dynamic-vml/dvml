// Dynamic View Model (lists)
// Copyright (c) 2020 César Roberto de Souza. Licensed under the MIT license
// cesarsouza@gmail.com - http://crsouza.com

using System;
using System.Linq;

using DynamicVML.Internals;
using DynamicVML.Options;

using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace DynamicVML.Extensions
{

    /// <summary>
    ///   Contains extension methods for extracting and storing information
    ///   related to <see cref="DynamicList{TViewModel, TOptions}">DynamicLists</see>
    ///   from <see cref="ViewDataDictionary"/> objects.
    /// </summary>
    /// 
    public static class ViewDataExtensions
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

        public static void SetEditorItemParameters(this ViewDataDictionary viewData, string? itemId, AddNewDynamicItem parameters, NewItemMethod method)
        {
            if (itemId == null)
                throw new ArgumentNullException(nameof(itemId));

            var dict = parameters.GetAdditionalViewData();
            var obj = new EditorItemParams
            {
                AdditionalViewData = dict,
                CurrentIndex = itemId,
                EditorParams = new EditorParams(
                    containerId: parameters.ContainerId,
                    itemTemplate: parameters.ItemTemplate,
                    itemContainerTemplate: parameters.ItemContainerTemplate,
                    listTemplate: String.Empty,
                    actionUrl: String.Empty,
                    addNewItemText: String.Empty,
                    prefix: parameters.Prefix,
                    additionalViewData: dict,
                    mode: parameters.Mode,
                    method: method)
            };

            viewData[itemId] = obj;

            foreach (var pair in dict)
                viewData[pair.Key] = pair.Value;

            viewData.TemplateInfo.HtmlFieldPrefix = parameters.Prefix;
        }

        public static DisplayItemParams GetDisplayItemParameters(this ViewDataDictionary viewData, string? itemId, string? containerId = null)
        {
            if (itemId == null)
                throw new ArgumentNullException(nameof(itemId));

            if (viewData.TryGetValue(itemId, out object obj))
            {
                if (obj != null && obj is DisplayItemParams d)
                    return d;
            }

            if (containerId == null)
                throw new ApplicationException();

            DisplayParams param = viewData.GetDisplayParameters(containerId);

            var displayItemParams = new DisplayItemParams
            {
                DisplayParams = param,
                AdditionalViewData = viewData[Constants.AdditionalViewData],
                CurrentIndex = itemId
            };

            viewData[itemId] = displayItemParams;
            return displayItemParams;
        }

        public static EditorItemParams GetEditorItemParams(this ViewDataDictionary viewData, string? itemId, string? containerId = null)
        {
            if (itemId == null)
                throw new ArgumentNullException(nameof(itemId));

            if (viewData.TryGetValue(itemId, out object obj))
            {
                if (obj != null && obj is EditorItemParams e)
                    return e;
            }

            if (containerId == null)
                throw new ApplicationException();

            EditorParams param = viewData.GetEditorParameters(containerId);
            AddNewDynamicItem newItemParams = param.CreateNewItemParams();

            var editItemParams = new EditorItemParams
            {
                NewItemParams = newItemParams,
                EditorParams = param,
                AdditionalViewData = viewData[Constants.AdditionalViewData],
                CurrentIndex = itemId
            };

            viewData[itemId] = editItemParams;
            return editItemParams;
        }

        /// <summary>
        ///   Builds a new <see cref="EditorParams"/> object gathering information from different
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
        ///     A new <see cref="EditorParams"/> object with the actual values to be 
        ///     used when rendering the editor view for your view model.
        /// </returns>
        /// 
        public static EditorParams GetEditorParameters(this ViewDataDictionary viewData, string containerId)
        {
            var viewDataObject = viewData[containerId] as ListViewDataObject;
            if (viewDataObject == null)
            {
                throw new ApplicationException($"Could not find a {nameof(ListViewDataObject)} for the container {containerId}. " +
                    $"Please make sure to call the {nameof(EditorExtensions.ListEditorFor)} method when attempting to show " +
                    $"an editor for an {nameof(IDynamicList)}.");
            }

            if (viewDataObject.DynamicListEditorParams != null)
                return viewDataObject.DynamicListEditorParams;

            ListRenderMode mode = Constants.DefaultRenderMode;
            string? listTemplate = null;
            string? itemContainerTemplate = null;
            string? itemTemplate = null;
            string? addNewItemText = null;
            string? actionUrl = null;
            NewItemMethod method = NewItemMethod.Get;

            DynamicListAttribute? attribute = GetAttributeInfo(viewData);
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
                DynamicListEditorOptions? options = viewDataObject.DynamicListEditorOptions;
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

            listTemplate = "EditorTemplates/" + listTemplate;

            string prefix = viewData.TemplateInfo.HtmlFieldPrefix;
            object? userData = GetUserDataAndRemoveFromView(viewData);

            viewDataObject.DynamicListEditorParams = new EditorParams(
                containerId: containerId,
                itemTemplate: itemTemplate,
                itemContainerTemplate: itemContainerTemplate!,
                listTemplate: listTemplate,
                actionUrl: actionUrl,
                addNewItemText: addNewItemText,
                prefix: prefix,
                additionalViewData: userData,
                mode: mode,
                method: method);

            return viewDataObject.DynamicListEditorParams;
        }

        /// <summary>
        ///   Builds a new <see cref="DisplayParams"/> object gathering information from different
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
        ///     A new <see cref="DisplayParams"/> object with the actual values to be 
        ///     used when rendering the editor view for your view model.
        /// </returns>
        /// 
        public static DisplayParams GetDisplayParameters(this ViewDataDictionary viewData, string containerId)
        {
            var viewDataObject = viewData[containerId] as ListViewDataObject;
            if (viewDataObject == null)
            {
                throw new ApplicationException($"Could not find a {nameof(ListViewDataObject)} for the container {containerId}. " +
                    $"Please make sure to call the {nameof(EditorExtensions.DisplayListFor)} method when attempting to show " +
                    $"a display for an {nameof(IDynamicList)}.");
            }

            if (viewDataObject.DynamicListDisplayParams != null)
                return viewDataObject.DynamicListDisplayParams;

            ListRenderMode mode = Constants.DefaultRenderMode;
            string? listTemplate = null;
            string? itemContainerTemplate = null;
            string? itemTemplate = null;

            DynamicListAttribute? attribute = GetAttributeInfo(viewData);
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
                DynamicListDisplayOptions? options = viewDataObject.DynamicListDisplayOptions;
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

            listTemplate = "DisplayTemplates/" + listTemplate;

            string prefix = viewData.TemplateInfo.HtmlFieldPrefix;
            object? userData = GetUserDataAndRemoveFromView(viewData);

            viewDataObject.DynamicListDisplayParams = new DisplayParams(
                itemTemplate: itemTemplate,
                itemContainerTemplate: itemContainerTemplate!,
                listTemplate: listTemplate,
                prefix: prefix,
                additionalViewData: userData,
                mode: mode);

            return viewDataObject.DynamicListDisplayParams;
        }

        /// <summary>
        ///   Attempts to extract a <see cref="DynamicListAttribute"/> from the model's
        ///   metadata stored in the <see cref="ViewDataDictionary.Model"/> property.
        /// </summary>
        /// 
        /// <param name="viewData">The view data containing the metadata to be inspected.</param>
        /// 
        /// <returns>A <see cref="DynamicListAttribute"/> object if one is set; otherwise null.</returns>
        /// 
        public static DynamicListAttribute? GetAttributeInfo(this ViewDataDictionary viewData)
        {
            DefaultModelMetadata? metadata = viewData.ModelExplorer.Metadata as DefaultModelMetadata;
            if (metadata?.Attributes?.PropertyAttributes == null)
                return null;

            DynamicListAttribute? attr = metadata.Attributes.PropertyAttributes
                .Where(x => x.GetType() == typeof(DynamicListAttribute))
                .FirstOrDefault() as DynamicListAttribute;

            return attr;
        }


        private static object? GetUserDataAndRemoveFromView(ViewDataDictionary viewData)
        {
            object? userData = null;
            if (viewData.ContainsKey(Constants.AdditionalViewData))
            {
                userData = viewData[Constants.AdditionalViewData];
                viewData.Remove(Constants.AdditionalViewData);
                if (userData != null)
                    viewData.Add(userData);
            }
            return userData;
        }

        private static void Add(this ViewDataDictionary dict, object o)
        {
            foreach (var propertyInfo in o.GetType().GetProperties())
            {
                if (propertyInfo.GetIndexParameters().Length == 0)
                    dict.Add(propertyInfo.Name, propertyInfo.GetValue(o, null));
            }
        }

        private static string GetViewModelTypeName(ViewDataDictionary viewData)
        {
            var modelType = viewData.Model.GetType();
            var interfaces = modelType.GetInterfaces();
            var listType = interfaces.Where(x =>
                x.GenericTypeArguments.Length == 1 && x.Name.StartsWith(nameof(IDynamicList)))
                .FirstOrDefault();
            var optionsType = listType.GenericTypeArguments.First();
            var viewModelType = optionsType.GenericTypeArguments.First();

            string viewModelTypeName = viewModelType.Name;
            return viewModelTypeName;
        }
    }
}

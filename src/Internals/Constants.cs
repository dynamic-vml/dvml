// Dynamic View Model (lists)
// Copyright (c) 2020 César Roberto de Souza. Licensed under the MIT license
// cesarsouza@gmail.com - http://crsouza.com

using DynamicVML.Options;

using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace DynamicVML.Internals
{
    /// <summary>
    ///   Constants that are used throughout the Dynamic View Model library. You can use the value of 
    ///   those constants to access internal objects created by the library which may be present in 
    ///   your view's <see cref="ViewDataDictionary">ViewData</see>.
    /// </summary>
    /// 
    public static class Constants
    {
        /// <summary>
        ///   Gets the default file name for list containers. The default is "DynamicListContainer".
        /// </summary>
        /// 
        public const string DefaultListContainerTemplate = "DynamicListContainer";

        /// <summary>
        ///   Gets the default file name for lists. The default is "DynamicList".
        /// </summary>
        /// 
        public const string DefaultListTemplate = "DynamicList";

        /// <summary>
        ///   Gets the default file name for list item containers. The default is "DynamicItemContainer".
        /// </summary>
        /// 
        public const string DefaultItemContainerTemplate = "DynamicItemContainer";




        /// <summary>
        ///   Gets the string value that is used as a key in ViewData 
        ///   dictionaries when storing additional user data objects.
        /// </summary>
        /// 
        public const string AdditionalViewData = "DynamicList_AdditionalViewData";

        /// <summary>
        ///   Gets the string value that is used as a key in ViewData 
        ///   dictionaries when storing the <see cref="IDynamicListItem.Index"/>
        ///   for the current item.
        /// </summary>
        /// 
        public const string CurrentIndex = "DynamicList_CurrentIndex";

        /// <summary>
        ///   Gets the string value that is used as a key in ViewData 
        ///   dictionaries when storing the <see cref="IDynamicList.ContainerId"/>
        ///   for the current list.
        /// </summary>
        /// 
        public const string CurrentContainerId = "DynamicList_CurrentContainerId";




        /// <summary>
        ///   Gets the string value that is used as a key in ViewData 
        ///   dictionaries when storing <see cref="EditorOptions"/>
        ///   objects for the current view.
        /// </summary>
        /// 
        public const string EditorOptions = "DynamicList_EditorOptions";

        /// <summary>
        ///   Gets the string value that is used as a key in ViewData 
        ///   dictionaries when storing <see cref="DisplayOptions"/>
        ///   objects for the current view.
        /// </summary>
        /// 
        public const string DisplayOptions = "DynamicList_DisplayOptions";

        /// <summary>
        ///   Gets the string value that is used as a key in ViewData 
        ///   dictionaries when storing <see cref="ListDisplayParameters"/>
        ///   objects for the current view.
        /// </summary>
        /// 
        public const string ListDisplayParameters = "DynamicList_ListDisplayParameters";

        /// <summary>
        ///   Gets the string value that is used as a key in ViewData 
        ///   dictionaries when storing <see cref="ListEditorParameters"/>
        ///   objects for the current view.
        /// </summary>
        /// 
        public const string ListEditorParameters = "DynamicList_ListEditorParameters";

        /// <summary>
        ///   Gets the string value that is used as a key in ViewData 
        ///   dictionaries when storing <see cref="ItemDisplayParameters"/>
        ///   objects for the current view.
        /// </summary>
        /// 
        public const string ItemDisplayParameters = "DynamicList_ItemDisplayParameters";

        /// <summary>
        ///   Gets the string value that is used as a key in ViewData 
        ///   dictionaries when storing <see cref="ItemEditorParameters"/>
        ///   objects for the current view.
        /// </summary>
        /// 
        public const string ItemEditorParameters = "DynamicList_ItemEditorParameters";

        /// <summary>
        ///   Gets the string value that is used as a key in ViewData 
        ///   dictionaries when storing <see cref="AddNewDynamicItem"/>
        ///   objects for the current view.
        /// </summary>
        /// 
        public const string ItemCreatorParameters = "DynamicList_ItemCreatorParameters";

        internal const string EditorTemplates = "EditorTemplates/";
        internal const string DisplayTemplates = "DisplayTemplates/";

        internal const ListRenderMode DefaultRenderMode = ListRenderMode.ViewModelOnly;
    }


    // The classes below are used to pass additional data to methods like EditorFor or DisplayFor

    internal class ViewDataObject
    {
        public ListEditorParameters? EditorParameters { get; set; }
        public ListDisplayParameters? DisplayParameters { get; set; }

        public DynamicListEditorOptions? EditorOptions { get; set; }
        public DynamicListDisplayOptions? DisplayOptions { get; set; }

        public object? AdditionalViewData { get; set; }

    }

}

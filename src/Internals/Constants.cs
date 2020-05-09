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
        public const string AdditionalViewData = nameof(ViewDataObject.DynamicListAdditionalViewData);

        /// <summary>
        ///   Gets the string value that is used as a key in ViewData 
        ///   dictionaries when storing the <see cref="IDynamicListItem.Index"/>
        ///   for the current item.
        /// </summary>
        /// 
        public const string CurrentIndex = nameof(ItemViewDataObject.DynamicListCurrentIndex);



        /// <summary>
        ///   Gets the string value that is used as a key in ViewData 
        ///   dictionaries when storing <see cref="EditorOptions"/>
        ///   objects for the current view.
        /// </summary>
        /// 
        public const string EditorOptions = nameof(ListViewDataObject.DynamicListEditorOptions);

        /// <summary>
        ///   Gets the string value that is used as a key in ViewData 
        ///   dictionaries when storing <see cref="DisplayOptions"/>
        ///   objects for the current view.
        /// </summary>
        /// 
        public const string DisplayOptions = nameof(ListViewDataObject.DynamicListDisplayOptions);

        /// <summary>
        ///   Gets the string value that is used as a key in ViewData 
        ///   dictionaries when storing <see cref="DisplayParams"/>
        ///   objects for the current view.
        /// </summary>
        /// 
        public const string DisplayParams = nameof(ItemViewDataObject.DynamicListDisplayParams);

        /// <summary>
        ///   Gets the string value that is used as a key in ViewData 
        ///   dictionaries when storing <see cref="EditorParams"/>
        ///   objects for the current view.
        /// </summary>
        /// 
        public const string EditorParams = nameof(ItemViewDataObject.DynamicListEditorParams);

        /// <summary>
        ///   Gets the string value that is used as a key in ViewData 
        ///   dictionaries when storing <see cref="NewItemParams"/>
        ///   objects for the current view.
        /// </summary>
        /// 
        public const string NewItemParams = nameof(ItemViewDataObject.DynamicListNewItemParams);


        internal const string DefaultEditorTemplatesPath = "EditorTemplates";
        internal const string DefaultDisplayTemplatesPath = "DisplayTemplates";
        internal const ListRenderMode DefaultRenderMode = ListRenderMode.ViewModelOnly;
    }


    // The classes below are used to pass additional data to methods like EditorFor or DisplayFor. The
    // data could be passed in a more simpler way using anonymous types, but using real concrete classes
    // allows us to get the nameof() of those methods to be registered on the Constants defined above.

    // The names are long on purpose, to avoid collision with any other ViewData objects you might
    // already have been using in your views. Unfortunately it makes those classes look a bit ugly.

    internal class ViewDataObject
    {
        public object? DynamicListAdditionalViewData { get; set; }
    }

    internal class ListViewDataObject : ViewDataObject
    {
        public DynamicListEditorOptions? DynamicListEditorOptions { get; set; }
        public DynamicListDisplayOptions? DynamicListDisplayOptions { get; set; }
    }

    internal class ItemViewDataObject : ViewDataObject
    {
        public EditorParams? DynamicListEditorParams { get; set; }
        public DisplayParams? DynamicListDisplayParams { get; set; }
        public AddNewDynamicItem? DynamicListNewItemParams { get; set; }
        public string? DynamicListCurrentIndex { get; set; }
    }

}

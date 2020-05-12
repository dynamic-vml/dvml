// Dynamic View Model (lists)
// Copyright (c) 2020 César Roberto de Souza. Licensed under the MIT license
// cesarsouza@gmail.com - http://crsouza.com

using DynamicVML.Extensions;
using DynamicVML.Internals;

namespace DynamicVML.Options
{
    /// <summary>
    ///   Represents different options that can be used when displaying a <see cref="DynamicList{TViewModel, TOptions}"/>
    ///   for edigint. Instances of this class are normally created internally by the library when calling the 
    ///   <see cref="EditorExtensions.ListEditorFor">
    ///   Html.EditorFor</see> extension method provided by this library.
    /// </summary>
    /// 
    /// <remarks>
    ///   An instance of this class may be present in the ViewData dictionaries for your view. If you would like 
    ///   to access this object, use the value of <see cref="Constants.ListEditorParameters"/> as the ViewData key.
    /// </remarks>
    /// 
    /// <seealso cref="DynamicListDisplayOptions"/>
    /// 
    public class DynamicListEditorOptions : DynamicListOptions
    {

        /// <summary>
        ///   Gets or sets the text that gets displayed on the "Add New Item" link that will
        ///   call the server to dynamically add a new item to the list. The default is to
        ///   use "Add new {name of your view model}".
        /// </summary>
        /// 
        public string? AddNewItemText { get; set; }

        /// <summary>
        ///   Gets or sets the path to your controller's action which will be responsible for
        ///   rendering a new item to be added to the list. Please see the sample applications
        ///   for an example on how they are set up.
        /// </summary>
        /// 
        public string? ActionUrl { get; set; }

        /// <summary>
        ///   Gets or sets whether to use <see cref="NewItemMethod.Get">GET</see> or 
        ///   <see cref="NewItemMethod.Post">POST</see> when requesting new list items
        ///   from the server. Default is to use <see cref="NewItemMethod.Get">GET</see>.
        /// </summary>
        /// 
        public NewItemMethod? Method { get; set; }

    }
}

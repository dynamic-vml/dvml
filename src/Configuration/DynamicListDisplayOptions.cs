// Dynamic View Model (lists)
// Copyright (c) 2020 César Roberto de Souza. Licensed under the MIT license
// cesarsouza@gmail.com - http://crsouza.com

using DynamicVML.Extensions;
using DynamicVML.Internals;

namespace DynamicVML.Options
{
    /// <summary>
    ///   Represents different options that can be used when displaying a <see cref="DynamicList{TViewModel, TOptions}"/>.
    ///   Instances of this class are normally created internally by the library when calling the 
    ///   <see cref="EditorExtensions.DisplayListFor{TModel, TValue}(Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper{TModel}, System.Linq.Expressions.Expression{System.Func{TModel, TValue}}, string?, string?, string?, string, object?, string, ListRenderMode)">
    ///   Html.DisplayFor</see> extension method provided by this library.
    /// </summary>
    /// 
    /// <remarks>
    ///   An instance of this class may be present in the ViewData dictionaries for your view. If you would like 
    ///   to access this object, use the value of <see cref="Constants.DisplayParams"/> as the ViewData key.
    /// </remarks>
    /// 
    public class DynamicListDisplayOptions
    {
        /// <summary>
        ///   Gets or sets the item template to be used when displaying a list for this attribute. 
        ///   This should normally be your view for the view models you are using. If you do not specify
        ///   a view name, the library will attempt to find one based on your view model's class name. For 
        ///   more details about the different regions associated with a <see cref="DynamicList{TViewModel, TOptions}"/>,
        ///   plase <see cref = "EditorExtensions" />.
        /// </summary>
        /// 
        public string? ItemTemplate { get; set; }

        /// <summary>
        ///   Gets or sets the item container template to be used when displaying a list
        ///   for this attribute. For more details about the different regions associated 
        ///   with a <see cref="DynamicList{TViewModel, TOptions}"/>, please 
        ///   <see cref = "EditorExtensions" />.
        /// </summary>
        /// 
        public string? ItemContainerTemplate { get; set; }

        /// <summary>
        ///   Gets or sets the list template to be used when displaying a list
        ///   for this attribute. For more details about the different regions associated 
        ///   with a <see cref="DynamicList{TViewModel, TOptions}"/>, please 
        ///   <see cref = "EditorExtensions" />.
        /// </summary>
        /// 
        public string? ListTemplate { get; set; }

        /// <summary>
        ///   Gets or sets the location where display templates are normally found. In ASP.NET Core 3.0,
        ///   display templates are normally located under any of the "DisplayTemplates" sub-folders in 
        ///   either your controller's directory or your Shared directory under "Views".
        /// </summary>
        /// 
        public string? DisplayTemplates { get; set; }

        /// <summary>
        ///   Gets or sets whether the view for your viewmodel should receive a @model of type
        ///   <c>YourOptions{YourViewModel}</c> or simply <c>YourViewModel</c>. Default is to
        ///   use <see cref="ListRenderMode.ViewModelOnly"/> (so your view will receive just
        ///   your view model, without its associated options object.
        /// </summary>
        /// 
        public ListRenderMode? Mode { get; set; } = ListRenderMode.ViewModelOnly;

    }
}

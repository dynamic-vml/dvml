// Dynamic View Model (lists)
// Copyright (c) 2020 César Roberto de Souza. Licensed under the MIT license
// cesarsouza@gmail.com - http://crsouza.com

using System.ComponentModel.DataAnnotations;

using DynamicVML.Extensions;
using DynamicVML.Internals;

namespace DynamicVML.Options
{
    /// <summary>
    ///   Represents an attribute which you can include in your view model's
    ///   properties to indicate the property should be rendered as a Dynamic 
    ///   List. This class inherits from <see cref="UIHintAttribute" />.
    /// </summary>
    /// 
    /// <remarks>
    /// <para>
    /// Instead of specifying each of your views to add the extra parameters to `EditorFor()`,
    /// you can alternatively configure the DynamicList properties of your view models directly
    /// in your view model class definition using this attribute:
    /// </para>
    /// 
    /// <code language="csharp">
    /// public class AuthorViewModel
    /// {
    ///     public string FirstName { get; set; }
    ///     public string LastName { get; set; }
    /// 
    ///     [Display(Name = "Authored books")]
    ///     [DynamicList("BookView", 
    ///         ItemContainerTemplate = "MyCustomItemContainer", 
    ///         ListTemplate = "MyCustomList",
    ///         Method = RequestNewItemMethod.Post)]
    ///     public virtual DynamicList&lt;BookViewModel&gt; Books { get; set; } = 
    ///         new DynamicList&lt;BookViewModel&gt;();
    /// }
    /// </code>
    /// 
    /// >[!NOTE]
    /// While this is possible, this is not exactly recommended as one could argue that specifying view 
    /// parameters in your view models may add unnecessary coupling between your code and the presentation
    /// layer. However, if you **really** want to specify parameters this way, the library will let you do so.
    /// </remarks>
    /// 
    /// <seealso cref="System.ComponentModel.DataAnnotations.UIHintAttribute" />
    /// 
    public class DynamicListAttribute : UIHintAttribute
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref="DynamicListAttribute"/> class.
        /// </summary>
        /// 
        /// <param name="listContainerTemplate">The template for the list container. For more details
        ///   about the different regions associated with a <see cref="DynamicList{TViewModel, TOptions}"/>,
        ///   please <see cref="EditorExtensions"/>.</param>
        /// 
        public DynamicListAttribute(string listContainerTemplate = Constants.DefaultListContainerTemplate) :
            base(uiHint: listContainerTemplate, presentationLayer: "HTML")
        {
            ListContainerTemplate = listContainerTemplate;
        }

        /// <summary>
        ///   Gets or sets whether the view for your viewmodel should receive a @model of type
        ///   <c>YourOptions&lt;YourViewModel&gt;</c> or simply <c>YourViewModel</c>. Default is to
        ///   use <see cref="ListRenderMode.ViewModelOnly"/> (so your view will receive just
        ///   your view model, without its associated options object.
        /// </summary>
        /// 
        public ListRenderMode Mode { get; set; }

        /// <summary>
        ///   Gets or sets the list container template to be used when displaying a list
        ///   for this attribute. For more details about the different regions associated 
        ///   with a <see cref="DynamicList{TViewModel, TOptions}"/>, please 
        ///   <see cref = "EditorExtensions" />.
        /// </summary>
        /// 
        public string ListContainerTemplate { get; }

        /// <summary>
        ///   Gets or sets the list template to be used when displaying a list
        ///   for this attribute. For more details about the different regions associated 
        ///   with a <see cref="DynamicList{TViewModel, TOptions}"/>, please 
        ///   <see cref = "EditorExtensions" />.
        /// </summary>
        /// 
        public string? ListTemplate { get; set; }

        /// <summary>
        ///   Gets or sets the item container template to be used when displaying a list
        ///   for this attribute. For more details about the different regions associated 
        ///   with a <see cref="DynamicList{TViewModel, TOptions}"/>, please 
        ///   <see cref = "EditorExtensions" />.
        /// </summary>
        /// 
        public string? ItemContainerTemplate { get; set; }

        /// <summary>
        ///   Gets or sets the item template to be used when displaying a list for this attribute. 
        ///   This should normally be your view for the view models you are using. If you do not specify
        ///   a view name, the library will attempt to find one based on your view model's class name. For 
        ///   more details about the different regions associated with a <see cref="DynamicList{TViewModel, TOptions}"/>,
        ///   please <see cref = "EditorExtensions" />.
        /// </summary>
        /// 
        public string? ItemTemplate { get; set; }

        /// <summary>
        ///   Gets or sets the location where editor templates are normally found. In ASP.NET Core 3.0,
        ///   editor templates are normally located under any of the "EditorTemplates" sub-folders in 
        ///   either your controller's directory or your Shared directory under "Views".
        /// </summary>
        /// 
        public string? EditorTemplates { get; set; }

        /// <summary>
        ///   Gets or sets the location where display templates are normally found. In ASP.NET Core 3.0,
        ///   display templates are normally located under any of the "DisplayTemplates" sub-folders in 
        ///   either your controller's directory or your Shared directory under "Views".
        /// </summary>
        /// 
        public string? DisplayTemplates { get; set; }

        /// <summary>
        ///   Gets or sets whether to use <see cref="NewItemMethod.Get">GET</see> or 
        ///   <see cref="NewItemMethod.Post">POST</see> when requesting new list items
        ///   from the server. Default is to use <see cref="NewItemMethod.Get">GET</see>.
        /// </summary>
        /// 
        public NewItemMethod Method { get; set; }
    }
}

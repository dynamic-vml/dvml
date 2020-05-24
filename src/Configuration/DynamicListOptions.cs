// Dynamic View Model (lists)
// Copyright (c) 2020 César Roberto de Souza. Licensed under the MIT license
// cesarsouza@gmail.com - http://crsouza.com

using System;

using DynamicVML.Extensions;

namespace DynamicVML.Options
{
    /// <summary>
    ///   Base class for <see cref="DynamicListDisplayOptions"/> and <see cref="DynamicListEditorOptions"/>.
    /// </summary>
    /// 
    /// <remarks>
    /// <para>
    ///   The properties of this class specify templates for different regions of the dynamic list when
    ///   it gets rendered into a view. A quick reference for the regions can be seen in the figure below:
    /// </para>
    /// 
    /// <img src="~\images\templates.png"/>
    /// </remarks>
    /// 
    /// <seealso cref="DynamicListEditorOptions"/>
    /// <seealso cref="DynamicListDisplayOptions"/>
    /// 
    [Serializable]
    public abstract class DynamicListOptions
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
        ///   Gets or sets whether the view for your viewmodel should receive a @model of type
        ///   <c>YourOptions{YourViewModel}</c> or simply <c>YourViewModel</c>. Default is to
        ///   use <see cref="ListRenderMode.ViewModelOnly"/> (so your view will receive just
        ///   your view model, without its associated options object.
        /// </summary>
        /// 
        public ListRenderMode? Mode { get; set; } = ListRenderMode.ViewModelOnly;

    }
}

// Dynamic View Model (lists)
// Copyright (c) 2020 César Roberto de Souza. Licensed under the MIT license
// cesarsouza@gmail.com - http://crsouza.com

using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DynamicVML
{
    /// <summary>
    ///   Represents an item stored in a <see cref="DynamicList{TViewModel, TOptions}"/> when
    ///   the type of the view model does not need to be known. This is normally the case when
    ///   handling the list from inside a list template view.
    /// </summary>
    /// 
    public interface IDynamicListItem
    {
        /// <summary>
        ///   Gets the view model associated with this item as an object.
        /// </summary>
        /// 
        /// <remarks>
        ///   This property is only visible when handling this item through the non-generic
        ///   <see cref="IDynamicListItem"/> interface, which normally should be
        ///   the case only when acessing this item from a view (e.g. list templates).
        /// </remarks>
        /// 
        /// <value>The view model.</value>
        /// 
        object? ViewModel { get; }

        /// <summary>
        ///   This property is needed by the <see cref="IModelBinder"/> in order
        ///   to bind this item correctly to the HTML form. It does not have to
        ///   be set manually and will likely be overwritten by the rest of the
        ///   library.
        /// </summary>
        /// 
        /// <value>The index of the item, represented as a GUID value formatted as a HTML-friendly string.</value>
        /// 
        string? Index { get; set; }
    }

    /// <summary>
    ///   Represents an item stored in a <see cref="DynamicList{TViewModel, TOptions}"/> 
    ///   when the type of the view model needs to be known.
    /// </summary>
    /// 
    public interface IDynamicListItem<out TViewModel> : IDynamicListItem
        where TViewModel : class
    {
        /// <summary>
        ///   Gets or sets the view model stored in this item.
        /// </summary>
        /// 
        /// <value>The view model.</value>
        /// 
        new TViewModel? ViewModel { get; }
    }
}

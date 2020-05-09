// Dynamic View Model (lists)
// Copyright (c) 2020 César Roberto de Souza. Licensed under the MIT license
// cesarsouza@gmail.com - http://crsouza.com

using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DynamicVML
{
    /// <summary>
    ///   Represents a list item containing a <typeparamref name="TViewModel"/> for
    ///   the item and its associated options.
    /// </summary>
    /// 
    /// <typeparam name="TViewModel">The type of view model to be stored in this item.</typeparam>
    /// 
    /// <seealso cref="DynamicVML.IDynamicListItem{TViewModel}" />
    /// 
    public class DynamicListItem<TViewModel> : IDynamicListItem<TViewModel>
        where TViewModel : class
    {
        /// <summary>
        ///   Gets or sets the view model stored in this item.
        /// </summary>
        /// 
        /// <value>The view model.</value>
        /// 
        public TViewModel? ViewModel { get; set; }

        /// <summary>
        ///   This property is needed by the <see cref="IModelBinder"/> in order
        ///   to bind this item correctly to the HTML form. It does not have to
        ///   be set manually and will likely be overwritten by the rest of the
        ///   library.
        /// </summary>
        /// 
        /// <value>The index of the item, represented as a GUID value formatted as a HTML-friendly string.</value>
        /// 
        public string? Index { get; set; }

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
        object? IDynamicListItem.ViewModel => ViewModel;
    }
}

// Dynamic View Model (lists)
// Copyright (c) 2020 César Roberto de Souza. Licensed under the MIT license
// cesarsouza@gmail.com - http://crsouza.com

using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DynamicVML
{
    /// <summary>
    ///   Represents a list whose items are tracked by a string key associated with
    ///   the id of div elements in an HTML form, and whose items can be succesfully
    ///   binded to object properties by ASP.NET's default <see cref="IModelBinder"/>. 
    /// </summary>
    /// 
    /// <remarks>
    ///     This is the non-generic version of <see cref="IDynamicList{TListItem}"/> which
    ///     can be used whenever list item types do not need to be known. This will
    ///     usually be the case when acessing the dynamic list from a template view.
    /// </remarks>
    /// 
    /// <seealso cref="IDynamicListItem{TViewModel}"/>
    /// 
    public interface IDynamicList
    {
        /// <summary>
        ///   Gets the number of elements contained in this list.
        /// </summary>
        /// 
        /// <value>The number of elements in this list.</value>
        /// 
        int Count { get; }

        /// <summary>
        ///   The HTML div ID associated with this list. This ID is generated
        ///   automatically using the <see cref="DynamicList{TViewModel, TOptions}.CreateId"/> 
        ///   method and is guaranteed to be unique.
        /// </summary>
        /// 
        /// <value>
        ///   The identifier for the HTML div element that contains
        ///   the representation of this list in an HTML form.
        /// </value>
        /// 
        string ContainerId { get; set; }

        /// <summary>
        ///   This property is required to help <see cref="IModelBinder"/> during runtime
        ///   and does not have to be set to anything when creating the list.
        /// </summary>
        /// 
        string? Index { get; set; }

        /// <summary>
        ///   Gets an enumerable that can be used to iterate through the identifiers
        ///   of the div HTML elements used by your view models when rendered in a form.
        /// </summary>
        /// 
        /// <value>The keys, represented as an <see cref="IEnumerable{String}"/>.</value>
        /// 
        IEnumerable<string> Keys { get; }


        /// <summary>
        ///   Gets the <see typeparamref="TOptions"/> with the specified 
        ///   identifier as an <see cref="IDynamicListItem"/> object.
        /// </summary>
        /// 
        /// <remarks>
        ///     This is an explicit interface implementation which is only available when
        ///     interacting with this list through the <see cref="IDynamicList"/>
        ///     interface. Normally, this should only be the case when accessing the list
        ///     from a view.
        /// </remarks>
        /// 
        IDynamicListItem this[string id] { get; }
    }

    /// <summary>
    ///   Represents a list whose items are tracked by a string key associated with
    ///   the id of div elements in an HTML form, and whose items can be succesfully
    ///   binded to object properties by ASP.NET's default <see cref="IModelBinder"/>. 
    /// </summary>
    /// 
    /// <remarks>
    ///     This is the generic version of <see cref="IDynamicList"/> which
    ///     can be used whenever list item types need to be known. 
    /// </remarks>
    /// 
    /// <seealso cref="IDynamicListItem"/>
    /// 
    public interface IDynamicList<out TValue> : IDynamicList
        where TValue : IDynamicListItem
    {

        /// <summary>
        ///   Gets the <see typeparamref="TValue"/> with the specified identifier.
        /// </summary>
        /// 
        /// <remarks>
        ///     This is an explicit interface implementation which is only available when
        ///     interacting with this list through the <see cref="IDynamicList{TOptions}"/>
        ///     interface. Normally, this should only be the case when accessing the list
        ///     from a view.
        /// </remarks>
        /// 
        new TValue this[string id] { get; }
    }
}

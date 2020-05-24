// Dynamic View Model (lists)
// Copyright (c) 2020 César Roberto de Souza. Licensed under the MIT license
// cesarsouza@gmail.com - http://crsouza.com

using System;

using DynamicVML.Extensions;
using DynamicVML.Internals;

namespace DynamicVML.Options
{
    /// <summary>
    ///   Represents different options that can be used when displaying a <see cref="DynamicList{TViewModel, TOptions}"/>.
    ///   Instances of this class are normally created internally by the library when calling the 
    ///   <see cref="EditorExtensions.DisplayListFor">
    ///   Html.DisplayFor</see> extension method provided by this library.
    /// </summary>
    /// 
    /// <remarks>
    ///   An instance of this class may be present in the ViewData dictionaries for your view. If you would like 
    ///   to access this object, use the value of <see cref="Constants.ItemDisplayParameters"/> as the ViewData key.
    /// </remarks>
    /// 
    /// <seealso cref="DynamicListEditorOptions"/>
    /// 
    [Serializable]
    public class DynamicListDisplayOptions : DynamicListOptions
    {
       

    }
}

// Dynamic View Model (lists)
// Copyright (c) 2020 César Roberto de Souza. Licensed under the MIT license
// cesarsouza@gmail.com - http://crsouza.com

using DynamicVML.Extensions;
using DynamicVML.Options;

namespace DynamicVML.Internals
{
    /// <summary>
    ///   Represents the actual parameters being used to render the list. An instance of this class
    ///   will be stored in the ViewData object of your view under the key <see cref="Constants.ListDisplayParameters"/>.
    ///   You should never need to instantiate this class directly as it is part of the inner workings
    ///   of the library.
    /// </summary>
    /// 
    public class ListDisplayParameters
    {
        /// <summary>
        ///   Gets the parameters used to create this list.
        /// </summary>
        /// 
        public ListParameters List { get; }


        /// <summary>
        ///   Gets any additional view data which may have been passed by the user
        ///   when calling the <see cref="EditorExtensions.DisplayListFor">Html.DisplayListFor</see>
        ///   or <see cref="EditorExtensions.ListEditorFor">ListEditorFor</see> extension methods.
        /// </summary>
        /// 
        public object? AdditionalViewData { get; }


        /// <summary>
        ///   Creates a new instance of <see cref="ListEditorParameters"/>.
        /// </summary>
        /// 
        public ListDisplayParameters(ListParameters parameters, object? additionalViewData)
        {
            this.List = parameters;
            this.AdditionalViewData = additionalViewData;
        }

    }
}

// Dynamic View Model (lists)
// Copyright (c) 2020 César Roberto de Souza. Licensed under the MIT license
// cesarsouza@gmail.com - http://crsouza.com

using DynamicVML.Options;

namespace DynamicVML.Internals
{
    /// <summary>
    ///   Represents the actual parameters being used to render the list. An instance of this class
    ///   will be stored in the ViewData object of your view under the key <see cref="Constants.DisplayParams"/>.
    ///   You should never need to instantiate this class directly as it is part of the inner workings
    ///   of the library.
    /// </summary>
    /// 
    public abstract class Parameters
    {
        /// <summary>
        ///   Gets the actual <see cref="DynamicListDisplayOptions.ItemTemplate"/> being used.
        /// </summary>
        /// 
        public string ItemTemplate { get;  set; } = string.Empty;

        /// <summary>
        ///   Gets the actual <see cref="DynamicListDisplayOptions.ItemContainerTemplate"/> being used.
        /// </summary>
        /// 
        public string ItemContainerTemplate { get;  set; } = string.Empty;

        /// <summary>
        ///   Gets the actual <see cref="DynamicListDisplayOptions.ListTemplate"/> being used.
        /// </summary>
        /// 
        public string ListTemplate { get;  set; } = string.Empty;

        /// <summary>
        ///   Gets the actual HTML prefix being used for the forms.
        /// </summary>
        /// 
        public string Prefix { get;  set; } = string.Empty;

        /// <summary>
        ///   Gets the actual <see cref="ListRenderMode"/> being used.
        /// </summary>
        /// 
        public ListRenderMode Mode { get;  set; }

    }
}

// Dynamic View Model (lists)
// Copyright (c) 2020 César Roberto de Souza. Licensed under the MIT license
// cesarsouza@gmail.com - http://crsouza.com

using System;

namespace DynamicVML.Internals
{
    /// <summary>
    ///   Base class for <see cref="ItemDisplayParameters"/> and <see cref="ItemEditorParameters"/>.
    /// </summary>
    /// 
    [Serializable]
    public abstract class ItemParameters : Parameters
    {

        /// <summary>
        ///   Gets the HTML div ID associted with this list item.
        /// </summary>
        /// 
        public string Index { get; }

        /// <summary>
        ///   Creates a new instance of the <see cref="Parameters"/> class.
        /// </summary>
        /// 
        /// <param name="containerId">The HTML div element ID for the current list.</param>
        /// <param name="itemId">The HTML div element ID for the current list item.</param>
        /// 
        public ItemParameters(string containerId, string itemId)
            : base(containerId)
        {
            this.Index = itemId;
        }
    }
}
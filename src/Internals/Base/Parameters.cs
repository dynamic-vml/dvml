// Dynamic View Model (lists)
// Copyright (c) 2020 César Roberto de Souza. Licensed under the MIT license
// cesarsouza@gmail.com - http://crsouza.com

using System;

namespace DynamicVML.Internals
{
    /// <summary>
    ///   Base class for <see cref="ListParameters"/> and <see cref="ItemParameters"/>.
    /// </summary>
    /// 
    [Serializable]
    public abstract class Parameters
    {
        /// <summary>
        ///   Gets the HTML div element ID for the current list.
        /// </summary>
        /// 
        public string ContainerId { get; }

        /// <summary>
        ///   Creates a new instance of the <see cref="Parameters"/> class.
        /// </summary>
        /// 
        /// <param name="containerId">The HTML div element ID for the current list.</param>
        /// 
        public Parameters(string containerId)
        {
            this.ContainerId = containerId;
        }
    }
}

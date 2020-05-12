// Dynamic View Model (lists)
// Copyright (c) 2020 César Roberto de Souza. Licensed under the MIT license
// cesarsouza@gmail.com - http://crsouza.com

namespace DynamicVML.Internals
{
    /// <summary>
    ///   Represents the actual parameters being used to render an item of the list for display. An instance of this class
    ///   will be stored in the ViewData object of your view under the key <see cref="Constants.ItemDisplayParameters"/>.
    /// </summary>
    /// 
    /// <seealso cref="ListDisplayParameters"/>
    /// <seealso cref="ItemEditorParameters"/>
    /// 
    public class ItemDisplayParameters : ItemParameters
    {
        /// <summary>
        ///   Gets the parameters used to render this list for display.
        /// </summary>
        /// 
        public ListDisplayParameters Display { get; }

        /// <summary>
        ///   Creates a new instance of <see cref="ItemDisplayParameters"/>.
        /// </summary>
        /// 
        public ItemDisplayParameters(string containerId, string itemId,
            ListDisplayParameters parameters)
            : base(containerId, itemId)
        {
            this.Display = parameters;
        }
    }
}
// Dynamic View Model (lists)
// Copyright (c) 2020 César Roberto de Souza. Licensed under the MIT license
// cesarsouza@gmail.com - http://crsouza.com

using System;

namespace DynamicVML.Internals
{
    /// <summary>
    ///   Represents the actual parameters being used to render an item of the list for edit. An instance of this class
    ///   will be stored in the ViewData object of your view under the key <see cref="Constants.ItemEditorParameters"/>.
    /// </summary>
    /// 
    /// <seealso cref="ListEditorParameters"/>
    /// <seealso cref="ItemDisplayParameters"/>
    /// 
    [Serializable]
    public class ItemEditorParameters : ItemParameters
    {
        /// <summary>
        ///   Gets the parameters used to render this list for edit.
        /// </summary>
        /// 
        public ListEditorParameters Editor { get; }

        /// <summary>
        ///   Gets the parameters that should be sent to the server with instructions 
        ///   on how to create new items to be dynamically added to the list.
        /// </summary>
        /// 
        public AddNewDynamicItem AddNewItem { get; }

        /// <summary>
        ///   Creates a new instance of <see cref="ItemEditorParameters"/>.
        /// </summary>
        /// 
        public ItemEditorParameters(string containerId, string itemId,
            AddNewDynamicItem newItemParameters, ListEditorParameters editorParameters)
            : base(containerId, itemId)
        {
            this.AddNewItem = newItemParameters;
            this.Editor = editorParameters;
        }
    }
}
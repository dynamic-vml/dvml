// Dynamic View Model (lists)
// Copyright (c) 2020 César Roberto de Souza. Licensed under the MIT license
// cesarsouza@gmail.com - http://crsouza.com

using System;
using DynamicVML.Extensions;
using DynamicVML.Internals;
using DynamicVML.Options;

namespace DynamicVML
{
    /// <summary>
    ///   Represents a common type for exceptions thrown by the library.
    /// </summary>
    /// 
    public class DynamicListException : Exception
    {
        /// <summary>
        ///   Gets the <see cref="AddNewDynamicItem"/> object that was 
        ///   available in the context that this exception was thrown.
        /// </summary>
        /// 
        public AddNewDynamicItem? AddNewItemParameters { get; internal set; }

        /// <summary>
        ///   Gets values passed as parameters when the method <see cref="EditorExtensions.ListEditorFor"/>, 
        ///   was called, if they were available in the context that this exception was thrown.
        /// </summary>
        /// 
        public DynamicListEditorOptions? EditorOptions { get; internal set; }

        /// <summary>
        ///   Gets values passed as parameters when the method <see cref="EditorExtensions.DisplayListFor"/>, 
        ///   was called, if they were available in the context that this exception was thrown.
        /// </summary>
        /// 
        public DynamicListDisplayOptions? DisplayOptions { get; internal set; }

        /// <summary>
        ///   Gets values passed additional view data to either <see cref="EditorExtensions.DisplayListFor"/>
        ///   or <see cref="EditorExtensions.ListEditorFor"/> were called, if they were available in the context 
        ///   that this exception was thrown.
        /// </summary>
        /// 
        public object? AdditionalViewData { get; internal set; }

        /// <summary>
        ///   Gets the <see cref="ListDisplayParameters"/> object that was present in the view data when this exception was thrown.
        /// </summary>
        /// 
        public ListDisplayParameters? DisplayParameters { get; internal set; }

        /// <summary>
        ///   Gets the <see cref="ListEditorParameters"/> object that was present in the view data when this exception was thrown.
        /// </summary>
        /// 
        public ListEditorParameters? EditorParameters { get; internal set; }

        /// <summary>
        ///   Gets the <see cref="ItemDisplayParameters"/> object that was present in the view data when this exception was thrown.
        /// </summary>
        /// 
        public ItemDisplayParameters? ItemDisplayParameters { get; internal set; }

        /// <summary>
        ///   Gets the <see cref="ItemEditorParameters"/> object that was present in the view data when this exception was thrown.
        /// </summary>
        /// 
        public ItemEditorParameters? ItemEditorParameters { get; internal set; }



        internal DynamicListException(string message)
            : base(message)
        {
        }

        internal DynamicListException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
        
    }
}

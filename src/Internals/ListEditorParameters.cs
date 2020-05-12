// Dynamic View Model (lists)
// Copyright (c) 2020 César Roberto de Souza. Licensed under the MIT license
// cesarsouza@gmail.com - http://crsouza.com

using System;

using DynamicVML.Extensions;
using DynamicVML.Options;

namespace DynamicVML.Internals
{
    /// <summary>
    ///   Represents the actual parameters being used to render an editor for the list. An instance of this class
    ///   will be stored in the ViewData object of your view under the key <see cref="Constants.ListEditorParameters"/>.
    /// </summary>
    /// 
    public class ListEditorParameters : ListDisplayParameters
    {
        /// <summary>
        ///   Gets the actual <see cref="DynamicListEditorOptions.ActionUrl"/> being used.
        /// </summary>
        /// 
        public string ActionUrl { get; }

        /// <summary>
        ///   Gets the actual <see cref="DynamicListEditorOptions.AddNewItemText"/> being used.
        /// </summary>
        /// 
        public string AddNewItemText { get; }

        /// <summary>
        ///   Gets the actual <see cref="DynamicListEditorOptions.Method"/> being used.
        /// </summary>
        /// 
        public NewItemMethod Method { get; }

        /// <summary>
        ///   Creates a new <see cref="AddNewDynamicItem"/> object 
        ///   based on the information stored in this instance.
        /// </summary>
        /// 
        /// <returns>A new <see cref="AddNewDynamicItem"/> object.</returns>
        /// 
        public AddNewDynamicItem GetItemCreateParameters()
        {
            return new AddNewDynamicItem(List.ContainerId,
                listTemplate: List.ListTemplate,
                itemContainerTemplate: List.ItemContainerTemplate,
                itemTemplate: List.ItemTemplate,
                prefix: List.Prefix,
                mode: List.Mode,
                additionalViewData: AdditionalViewData);
        }


        /// <summary>
        ///   Gets a string containing information about the item to be rendered
        ///   and the controller action to be called, either as a query string
        ///   (for when using <see cref="NewItemMethod.Get"/> or JSON 
        ///   (for when using <see cref="NewItemMethod.Post"/>. Those
        ///   instructions will be passed to the JavaScript scripts in the
        ///   view to call the controller using Ajax.
        /// </summary>
        /// 
        /// <returns>A string containing instructions on how to call the controller using Ajax.</returns>
        /// 
        public string GetActionInfo()
        {
            if (ActionUrl.Length == 0)
                return String.Empty;

            var p = GetItemCreateParameters();

            if (Method == NewItemMethod.Get)
            {
                string queryString = p.ToQueryString();
                return $"{ActionUrl}/{queryString}";
            }

            if (Method == NewItemMethod.Post)
                return $"POST|{ActionUrl}|{p.ToJSON()}";

            throw new ApplicationException($"Unsupported {nameof(NewItemMethod)}.");
        }

        /// <summary>
        ///   Creates a new instance of <see cref="ListEditorParameters"/>.
        /// </summary>
        /// 
        public ListEditorParameters(ListParameters parameters, object? additionalViewData,
            string actionUrl, string addNewItemText, NewItemMethod method)
            : base(parameters, additionalViewData)
        {
            this.ActionUrl = actionUrl;
            this.AddNewItemText = addNewItemText;
            this.Method = method;
        }

    }
}

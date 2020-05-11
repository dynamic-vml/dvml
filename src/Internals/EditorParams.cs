// Dynamic View Model (lists)
// Copyright (c) 2020 César Roberto de Souza. Licensed under the MIT license
// cesarsouza@gmail.com - http://crsouza.com

using System;
using System.Linq.Expressions;

using DynamicVML.Extensions;
using DynamicVML.Options;

using Microsoft.AspNetCore.Mvc.Rendering;

namespace DynamicVML.Internals
{
    /// <summary>
    ///   Represents the actual parameters being used to render an editor for the list. An instance of this class
    ///   will be stored in the ViewData object of your view under the key <see cref="Constants.EditorParams"/>.
    /// </summary>
    /// 
    public class EditorParams : Parameters
    {
        public string ContainerId { get; }

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
        ///   Gets any additional view data which may have been passed by the user
        ///   when calling the <see cref="EditorExtensions.ListEditorFor">
        ///   Html.EditorFor</see> extension method.
        /// </summary>
        /// 
        public object? AdditionalViewData { get; }


        /// <summary>
        ///   Creates a new <see cref="AddNewDynamicItem"/> object based on
        ///   the information stored in this instance and the provided
        ///   <see paramref="containerId"/>.
        /// </summary>
        /// 
        /// <param name="containerId">The id of the div that should receive the new item.</param>
        /// 
        /// <returns>A new <see cref="AddNewDynamicItem"/> object.</returns>
        /// 
        public AddNewDynamicItem CreateNewItemParams()
        {
            return new AddNewDynamicItem(ContainerId,
                listTemplate: ListTemplate,
                itemContainerTemplate: ItemContainerTemplate,
                itemTemplate: ItemTemplate,
                prefix: Prefix,
                mode: Mode,
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
        /// <param name="containerId">The id of the div that should receive the new item.</param>
        /// 
        /// <returns>A string containing instructions on how to call the controller using Ajax.</returns>
        /// 
        public string GetActionContent()
        {
            if (ActionUrl.Length == 0)
                return String.Empty;

            var p = CreateNewItemParams();

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
        ///   Creates a new instance of <see cref="EditorParams"/>.
        /// </summary>
        /// 
        public EditorParams(string containerId, string itemTemplate, string itemContainerTemplate, string listTemplate,
            string actionUrl, string addNewItemText, string prefix, object? additionalViewData,
            ListRenderMode mode, NewItemMethod method)
        {
            this.ContainerId = containerId;
            this.ActionUrl = actionUrl;
            this.ItemTemplate = itemTemplate;
            this.ItemContainerTemplate = itemContainerTemplate;
            this.ListTemplate = listTemplate;
            this.AddNewItemText = addNewItemText;
            this.Prefix = prefix;
            this.AdditionalViewData = additionalViewData;
            this.Mode = mode;
            this.Method = method;
        }

    }
}

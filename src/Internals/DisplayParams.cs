// Dynamic View Model (lists)
// Copyright (c) 2020 César Roberto de Souza. Licensed under the MIT license
// cesarsouza@gmail.com - http://crsouza.com

using System;
using System.Linq.Expressions;

using DynamicVML.Extensions;

using Microsoft.AspNetCore.Mvc.Rendering;

namespace DynamicVML.Internals
{
    /// <summary>
    ///   Represents the actual parameters being used to render the list. An instance of this class
    ///   will be stored in the ViewData object of your view under the key <see cref="Constants.DisplayParams"/>.
    /// </summary>
    /// 
    public class DisplayParams : Parameters
    {
        /// <summary>
        ///   Gets any additional view data which may have been passed by the user
        ///   when calling the <see cref="EditorExtensions.DisplayListFor{TModel, TValue}(IHtmlHelper{TModel}, Expression{Func{TModel, TValue}}, string?, string?, string?, string, object?, string, ListRenderMode)">
        ///   Html.DisplayFor</see> extension method.
        /// </summary>
        /// 
        public object? AdditionalViewData { get; }


        internal DisplayParams(
            string itemTemplate, string itemContainerTemplate, string listTemplate, 
            string prefix, object? additionalViewData, ListRenderMode mode)
        {
            this.ItemTemplate = itemTemplate;
            this.ItemContainerTemplate = itemContainerTemplate;
            this.ListTemplate = listTemplate;
            this.Prefix = prefix;
            this.AdditionalViewData = additionalViewData;
            this.Mode = mode;
        }
    }
}

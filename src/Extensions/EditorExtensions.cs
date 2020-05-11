// Dynamic View Model (lists)
// Copyright (c) 2020 César Roberto de Souza. Licensed under the MIT license
// cesarsouza@gmail.com - http://crsouza.com

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Threading.Tasks;

using DynamicVML.Internals;
using DynamicVML.Options;

using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace DynamicVML.Extensions
{
    /// <summary>
    ///   Contains extension methods for rendering different parts of the list from .cshtml view files. 
    /// </summary>
    /// 
    /// <remarks>
    /// <para>
    ///   The methods in this class can be used to render different regions of a dynamic list 
    ///   from within a view. A quick reference for the regions can be seen in the figure below:
    /// </para>
    /// 
    /// <img src="~\images\templates.png"/>
    /// </remarks>
    /// 
    public static partial class EditorExtensions
    {
      
        private static ListViewDataObject PackViewData<TModel, TValue>(IHtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> propertyExpression, ListViewDataObject viewDataObject)
            where TValue : IDynamicList
        {
            TValue list = GetList(html, propertyExpression);

            viewDataObject.DynamicListContainerId = list.ContainerId;

            // register options for the current container
            html.ViewData[list.ContainerId] = viewDataObject;

            // if we do not do like this, nesting containers with different types will not work 
            // because this object would get overwritten when we try to display children containers
            return viewDataObject;
        }

        private static TValue GetList<TModel, TValue>(IHtmlHelper<TModel> html, Expression<Func<TModel, TValue>> propertyExpression) where TValue : IDynamicList
        {
            TValue list = propertyExpression.Compile()(html.ViewData.Model);
            if (list == null)
            {
                throw new ArgumentException($"The {nameof(IDynamicList)} at Model.{propertyExpression} cannot be null.",
                    nameof(propertyExpression));
            }

            return list;
        }
    }
}

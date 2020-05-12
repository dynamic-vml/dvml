// Dynamic View Model (lists)
// Copyright (c) 2020 César Roberto de Souza. Licensed under the MIT license
// cesarsouza@gmail.com - http://crsouza.com

using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using DynamicVML.Internals;

using Microsoft.AspNetCore.Mvc.Rendering;

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
      
        private static void PackViewData<TModel, TValue>(IHtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> propertyExpression, ViewDataObject viewDataObject)
            where TValue : IDynamicList
        {
            TValue list = GetDynamicListFromModel(html, propertyExpression);

            html.ViewData[Constants.CurrentContainerId] = list.ContainerId;
            html.ViewData[Constants.AdditionalViewData] = viewDataObject.AdditionalViewData;
            html.ViewData[Constants.DisplayOptions] = viewDataObject.DisplayOptions;
            html.ViewData[Constants.EditorOptions] = viewDataObject.EditorOptions;

            // Register the display/editor options _specifically_ for the current container. If we do not 
            // do like this, nesting containers with different types will not work because this object would 
            // get overwritten when we try to display children containers.
            html.ViewData[list.ContainerId] = viewDataObject;
        }

        private static TValue GetDynamicListFromModel<TModel, TValue>(this IHtmlHelper<TModel> html, 
            Expression<Func<TModel, TValue>> propertyExpression) 
            where TValue : IDynamicList
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

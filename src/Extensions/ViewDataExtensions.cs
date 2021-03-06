﻿// Dynamic View Model (lists)
// Copyright (c) 2020 César Roberto de Souza. Licensed under the MIT license
// cesarsouza@gmail.com - http://crsouza.com

using System;
using System.Linq;
using System.Reflection;

using DynamicVML.Internals;
using DynamicVML.Options;

using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace DynamicVML.Extensions
{
    /// <summary>
    ///   Contains extension methods for extracting and storing information
    ///   related to <see cref="DynamicList{TViewModel, TOptions}">DynamicLists</see>
    ///   from <see cref="ViewDataDictionary"/> objects.
    /// </summary>
    ///
    public static partial class ViewDataExtensions
    {
        /// <summary>
        ///   Attempts to extract a <see cref="DynamicListAttribute"/> from the model's
        ///   metadata stored in the <see cref="ViewDataDictionary.Model"/> property.
        /// </summary>
        ///
        /// <param name="viewData">The view data containing the metadata to be inspected.</param>
        ///
        /// <returns>A <see cref="DynamicListAttribute"/> object if one is set; otherwise null.</returns>
        ///
        public static DynamicListAttribute? GetDynamicListAttribute(this ViewDataDictionary viewData)
        {
            DefaultModelMetadata? metadata = viewData.ModelExplorer.Metadata as DefaultModelMetadata;
            if (metadata?.Attributes?.PropertyAttributes == null)
                return null;

            DynamicListAttribute? attr = metadata.Attributes.PropertyAttributes
                .Where(x => x.GetType() == typeof(DynamicListAttribute))
                .FirstOrDefault() as DynamicListAttribute;

            return attr;
        }

        private static object? GetUserDataAndRemoveFromView(ViewDataDictionary viewData)
        {
            object? userData = null;
            if (viewData.ContainsKey(Constants.AdditionalViewData))
            {
                userData = viewData[Constants.AdditionalViewData];
                viewData.Remove(Constants.AdditionalViewData);
                if (userData != null)
                    viewData.Append(userData);
            }
            return userData;
        }

        private static void Append(this ViewDataDictionary viewData, object obj)
        {
            foreach (PropertyInfo propertyInfo in obj.GetType().GetProperties())
            {
                if (propertyInfo.GetIndexParameters().Length == 0)
                    viewData.Add(propertyInfo.Name, propertyInfo.GetValue(obj, null));
            }
        }

        private static string GetViewModelTypeName(this ViewDataDictionary viewData)
        {
            Type modelType = viewData.Model.GetType();
            Type[] interfaces = modelType.GetInterfaces();
            Type? listType = interfaces.Where(x =>
                x.GenericTypeArguments.Length == 1 && x.Name.StartsWith(nameof(IDynamicList)))
                .FirstOrDefault();

            if (listType == null)
                throw new Exception($"Could not find {nameof(IDynamicList)} among the interfaces implemented by the model.");

            Type optionsType = listType.GenericTypeArguments.First();
            Type viewModelType = optionsType.GenericTypeArguments.First();

            string viewModelTypeName = viewModelType.Name;
            return viewModelTypeName;
        }
    }
}

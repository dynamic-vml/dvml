// Dynamic View Model (lists)
// Copyright (c) 2020 César Roberto de Souza. Licensed under the MIT license
// cesarsouza@gmail.com - http://crsouza.com

using System;
using System.Linq;

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
        public static DynamicListAttribute? GetAttributeInfo(this ViewDataDictionary viewData)
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
                    viewData.Add(userData);
            }
            return userData;
        }

        private static void Add(this ViewDataDictionary dict, object o)
        {
            foreach (var propertyInfo in o.GetType().GetProperties())
            {
                if (propertyInfo.GetIndexParameters().Length == 0)
                    dict.Add(propertyInfo.Name, propertyInfo.GetValue(o, null));
            }
        }

        private static string GetViewModelTypeName(ViewDataDictionary viewData)
        {
            var modelType = viewData.Model.GetType();
            var interfaces = modelType.GetInterfaces();
            var listType = interfaces.Where(x =>
                x.GenericTypeArguments.Length == 1 && x.Name.StartsWith(nameof(IDynamicList)))
                .FirstOrDefault();
            var optionsType = listType.GenericTypeArguments.First();
            var viewModelType = optionsType.GenericTypeArguments.First();

            string viewModelTypeName = viewModelType.Name;
            return viewModelTypeName;
        }
    }
}

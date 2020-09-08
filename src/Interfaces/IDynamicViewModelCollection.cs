// Dynamic View Model (lists)
// Copyright (c) 2020 César Roberto de Souza. Licensed under the MIT license
// cesarsouza@gmail.com - http://crsouza.com

using System;
using System.Collections.Generic;

namespace DynamicVML
{
    /// <summary>
    ///   Represents an entity that contains a list of view model objects.
    /// </summary>
    /// 
    /// <typeparam name="TViewModel">The type of the view model.</typeparam>
    /// 
    /// <seealso cref="DynamicList{TViewModel}" />
    /// <seealso cref="DynamicList{TViewModel, TOptions}" />
    /// 
    public interface IDynamicViewModelCollection<TViewModel> 
        where TViewModel : class
    {
        /// <summary>
        ///   Gets an enumerable that can be used to iterate through your ViewModel objects
        ///   that may be contained in this list.
        /// </summary>
        /// 
        /// <value>The view models, represented as an <see cref="IEnumerable{TViewModel}"/>.</value>
        /// 
        IEnumerable<TViewModel> ViewModels { get; }

        /// <summary>
        ///   Gets the number of elements contained in this list.
        /// </summary>
        /// 
        /// <value>The number of elements in this list.</value>
        /// 
        public int Count { get; }

        /// <summary>
        ///   Removes all items from this list.
        /// </summary>
        /// 
        void Clear();

        /// <summary>
        ///   Converts all view models stored inside this instance to their database model 
        ///   counterparts using the specified <paramref name="func">lambda function</paramref>.
        /// </summary>
        /// 
        /// <typeparam name="TModel">The type for the models to convert to.</typeparam>
        /// 
        /// <param name="func">
        ///     A function that takes a <see typeparamref="TViewModel"/> 
        ///     and converts it to a <see typeparamref="TModel"/>.
        /// </param>
        /// 
        /// <returns>
        ///     A <see cref="IEnumerable{TModel}"/> that can be materialized to an actual 
        ///     list/collection or just iterated over (e.g. using <see cref="System.Linq"/>.
        /// </returns>
        /// 
        IEnumerable<TModel> ToModel<TModel>(Func<TViewModel, TModel> func);

    }
}
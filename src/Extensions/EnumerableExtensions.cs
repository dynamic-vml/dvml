// Dynamic View Model (lists)
// Copyright (c) 2020 César Roberto de Souza. Licensed under the MIT license
// cesarsouza@gmail.com - http://crsouza.com

using System;
using System.Collections.Generic;
using System.Linq;

namespace DynamicVML.Extensions
{
    /// <summary>
    ///   Contains extension methods to convert view models and option objects to 
    ///   <see cref="DynamicList{TViewModel, TOptions}"/>.
    /// </summary>
    /// 
    public static partial class EnumerableExtensions
    {

        #region From single
        # region -- TOptions
        /// <summary>
        ///   Wraps a single view model into a partial dynamic list that can be sent back to the client.
        /// </summary>
        /// 
        /// <typeparam name="TViewModel">The type of your view model.</typeparam>
        /// <typeparam name="TOptions">The type of the options to be associated with your view model.</typeparam>
        /// 
        /// <param name="viewModel">The view model to be wrapped in a partial dynamic list.</param>
        /// <param name="containerId">The div ID of the already existing dynamic list container.</param>
        /// <param name="options">The options to be associated with your view model. Note that this
        ///   method will set the <see cref="DynamicListItem{TOptions}.ViewModel"/> property
        ///   of the given <paramref name="options"/> object to the given <paramref name="viewModel"/>.</param>
        /// 
        /// <returns>A partial dynamic list with a single item that can be passed to the client.</returns>
        /// 
        public static DynamicList<TViewModel, TOptions> ToDynamicList<TViewModel, TOptions>(this TViewModel viewModel, string containerId, TOptions options)
            where TViewModel : class
            where TOptions : DynamicListItem<TViewModel>, new()
        {
            options.ViewModel = viewModel;
            return new DynamicList<TViewModel, TOptions>(containerId) { options };
        }
        #endregion

        # region -- TViewModel
        /// <summary>
        ///   Wraps a single view model into a partial dynamic list that can be sent back to the client.
        /// </summary>
        /// 
        /// <typeparam name="TViewModel">The type of your view model.</typeparam>
        /// 
        /// <param name="viewModel">The view model to be wrapped in a partial dynamic list.</param>
        /// <param name="containerId">The div ID of the already existing dynamic list container.</param>
        /// 
        /// <returns>A partial dynamic list with a single item that can be passed to the client.</returns>
        /// 
        public static DynamicList<TViewModel> ToDynamicList<TViewModel>(this TViewModel viewModel, string containerId)
            where TViewModel : class
        {
            return new DynamicList<TViewModel>(containerId) { viewModel };
        }

        /// <summary>
        ///   Wraps a single view model into a partial dynamic list that can be sent back to the client.
        /// </summary>
        /// 
        /// <typeparam name="TViewModel">The type of your view model.</typeparam>
        /// <typeparam name="TOptions">The type of the options to be associated with your view model.</typeparam>
        /// 
        /// <param name="viewModel">The view model to be wrapped in a partial dynamic list.</param>
        /// <param name="containerId">The div ID of the already existing dynamic list container.</param>
        /// <param name="optionsSelector">A function that can be used to define how your view model
        ///   should be wrapped in a <typeparamref name="TOptions"/> object.</param>
        /// 
        /// <returns>A partial dynamic list with a single item that can be passed to the client.</returns>
        /// 
        public static DynamicList<TViewModel, TOptions> ToDynamicList<TViewModel, TOptions>(this TViewModel viewModel,
            string containerId, Func<TViewModel, TOptions> optionsSelector)
            where TViewModel : class
            where TOptions : DynamicListItem<TViewModel>, new()
        {
            return new DynamicList<TViewModel, TOptions>(containerId) { optionsSelector(viewModel) };
        }
        #endregion

        # region -- TModel
        /// <summary>
        ///   Wraps a single view model into a partial dynamic list that can be sent back to the client.
        /// </summary>
        /// 
        /// <typeparam name="TViewModel">The type of your view model.</typeparam>
        /// <typeparam name="TModel">The type of your (database) model.</typeparam>
        /// <typeparam name="TOptions">The type of the options to be associated with your view model.</typeparam>
        /// 
        /// <param name="model">The database model to be converted in a view model and wrapped in a partial dynamic list.</param>
        /// <param name="containerId">The div ID of the already existing dynamic list container.</param>
        /// <param name="optionsSelector">A function that can be used to define how your view model
        ///   should be wrapped in a <typeparamref name="TOptions"/> object.</param>
        /// <param name="viewModelSelector">A function that can be used to define how your view model
        ///   should be created from your model.</param>
        /// 
        /// <returns>A partial dynamic list with a single item that can be passed to the client.</returns>
        /// 
        public static DynamicList<TViewModel, TOptions> ToDynamicList<TViewModel, TOptions, TModel>(this TModel model,
            string containerId, Func<TModel, TViewModel> viewModelSelector, Func<TModel, TOptions> optionsSelector)
            where TViewModel : class
            where TOptions : DynamicListItem<TViewModel>, new()
        {
            var list = new DynamicList<TViewModel, TOptions>(containerId);
            var options = optionsSelector(model);
            options.ViewModel = viewModelSelector(model);
            list.Add(options);
            return list;
        }

        /// <summary>
        ///   Wraps a single view model into a partial dynamic list that can be sent back to the client.
        /// </summary>
        /// 
        /// <typeparam name="TViewModel">The type of your view model.</typeparam>
        /// <typeparam name="TModel">The type of your (database) model.</typeparam>
        /// 
        /// <param name="model">The database model to be converted in a view model and wrapped in a partial dynamic list.</param>
        /// <param name="containerId">The div ID of the already existing dynamic list container.</param>
        /// <param name="viewModelSelector">A function that can be used to define how your view model
        ///   should be created from your model.</param>
        /// 
        /// <returns>A partial dynamic list with a single item that can be passed to the client.</returns>
        /// 
        public static DynamicList<TViewModel> ToDynamicList<TViewModel, TModel>(this TModel model,
            string containerId, Func<TModel, TViewModel> viewModelSelector)
            where TViewModel : class
        {
            return new DynamicList<TViewModel>(containerId) { viewModelSelector(model) };
        }

        /// <summary>
        ///   Wraps a single view model into a partial dynamic list that can be sent back to the client.
        /// </summary>
        /// 
        /// <typeparam name="TViewModel">The type of your view model.</typeparam>
        /// <typeparam name="TModel">The type of your (database) model.</typeparam>
        /// <typeparam name="TOptions">The type of the options to be associated with your view model.</typeparam>
        /// 
        /// <param name="model">The database model to be converted in a view model and wrapped in a partial dynamic list.</param>
        /// <param name="containerId">The div ID of the already existing dynamic list container.</param>
        /// <param name="optionsSelector">A function that can be used to define how your view model
        ///   should be wrapped in a <typeparamref name="TOptions"/> object.</param>
        /// 
        /// <returns>A partial dynamic list with a single item that can be passed to the client.</returns>
        /// 
        public static DynamicList<TViewModel, TOptions> ToDynamicList<TViewModel, TOptions, TModel>(this TModel model,
            string containerId, Func<TModel, TOptions> optionsSelector)
            where TViewModel : class
            where TOptions : DynamicListItem<TViewModel>, new()
        {
            return new DynamicList<TViewModel, TOptions>(containerId) { optionsSelector(model) };
        }
        #endregion
        #endregion


        #region From enumerable
        #region -- IEnumerable<TOptions>
        /// <summary>
        ///   Converts a list of <typeparamref name="TOptions"/> objects into
        ///   a <see cref="DynamicList{TViewModel, TOptions}"/>.
        /// </summary>
        /// 
        /// <typeparam name="TViewModel">The type of your view model.</typeparam>
        /// <typeparam name="TOptions">The type of the options to be associated with your view model.</typeparam>
        /// 
        /// <param name="options">The list of options objects to be added to the list.</param>
        /// 
        /// <returns>A <see cref="DynamicList{TViewModel, TOptions}"/> containing the given <paramref name="options"/>.</returns>
        /// 
        public static DynamicList<TViewModel, TOptions> ToDynamicList<TViewModel, TOptions>(this IEnumerable<TOptions> options)
            where TViewModel : class
            where TOptions : DynamicListItem<TViewModel>, new()
        {
            var list = new DynamicList<TViewModel, TOptions>();
            list.AddRange(options);
            return list;
        }
        #endregion

        #region -- IEnumerable<TViewModel>
        /// <summary>
        ///   Converts a list of <typeparamref name="TViewModel"/> objects into
        ///   a <see cref="DynamicList{TViewModel, TOptions}"/>.
        /// </summary>
        /// 
        /// <typeparam name="TViewModel">The type of your view model.</typeparam>
        /// <typeparam name="TOptions">The type of the options to be associated with your view model.</typeparam>
        /// 
        /// <param name="viewModels">The list of viewmodel objects to be wrapped in option objects and then added to the list.</param>
        /// 
        /// <returns>A <see cref="DynamicList{TViewModel, TOptions}"/> containing the given <paramref name="viewModels"/>.</returns>
        /// 
        public static DynamicList<TViewModel> ToDynamicList<TViewModel, TOptions>(this IEnumerable<TViewModel> viewModels)
          where TViewModel : class
        {
            var list = new DynamicList<TViewModel>();
            list.AddRange(viewModels);
            return list;
        }

        /// <summary>
        ///   Converts a list of <typeparamref name="TViewModel"/> objects into
        ///   a <see cref="DynamicList{TViewModel, TOptions}"/>.
        /// </summary>
        /// 
        /// <typeparam name="TViewModel">The type of your view model.</typeparam>
        /// <typeparam name="TOptions">The type of the options to be associated with your view model.</typeparam>
        /// 
        /// <param name="viewModels">The list of viewmodel objects to be wrapped in option objects and then added to the list.</param>
        /// <param name="optionsSelector">A function that can be used to configure the <typeparamref name="TOptions"/>
        ///     associated with each of your <paramref name="viewModels"/>.</param>
        /// 
        /// <returns>A <see cref="DynamicList{TViewModel, TOptions}"/> containing the given <paramref name="viewModels"/>.</returns>
        /// 
        public static DynamicList<TViewModel, TOptions> ToDynamicList<TViewModel, TOptions>(this IEnumerable<TViewModel> viewModels,
            Func<TViewModel, TOptions> optionsSelector)
            where TViewModel : class
            where TOptions : DynamicListItem<TViewModel>, new()
        {
            var list = new DynamicList<TViewModel, TOptions>();
            list.AddRange(viewModels.Select(optionsSelector));
            return list;
        }
        #endregion

        #region IEnumerable<TModel>
        /// <summary>
        ///   Converts a list of <typeparamref name="TViewModel"/> objects into
        ///   a <see cref="DynamicList{TViewModel, TOptions}"/>.
        /// </summary>
        /// 
        /// <typeparam name="TViewModel">The type of your view models.</typeparam>
        /// <typeparam name="TModel">The type of your (database) models.</typeparam>
        /// <typeparam name="TOptions">The type of the options to be associated with your view models.</typeparam>
        /// 
        /// <param name="models">A list of database models to be converted into view models, wrapped in
        ///     <typeparamref name="TOptions"/> objects, and then inserted in a dynamic list.</param>
        /// <param name="optionsSelector">A function that can be used to define how your view model
        ///   should be wrapped in a <typeparamref name="TOptions"/> object.</param>
        /// <param name="viewModelSelector">A function that can be used to define how your view model
        ///   should be created from your model.</param>
        ///   
        public static DynamicList<TViewModel, TOptions> ToDynamicList<TViewModel, TOptions, TModel>(this IEnumerable<TModel> models,
            Func<TModel, TViewModel> viewModelSelector, Func<TModel, TOptions> optionsSelector)
            where TViewModel : class
            where TOptions : DynamicListItem<TViewModel>, new()
        {
            var list = new DynamicList<TViewModel, TOptions>();
            var options = models.Select(viewModelSelector).Zip(models.Select(optionsSelector), (vm, o) => { o.ViewModel = vm; return o; });
            list.AddRange(options);
            return list;
        }

        /// <summary>
        ///   Converts a list of <typeparamref name="TViewModel"/> objects into
        ///   a <see cref="DynamicList{TViewModel, TOptions}"/>.
        /// </summary>
        /// 
        /// <typeparam name="TViewModel">The type of your view models.</typeparam>
        /// <typeparam name="TModel">The type of your (database) models.</typeparam>
        /// 
        /// <param name="models">A list of database models to be converted into view models, and then 
        /// inserted in a dynamic list.</param>
        /// <param name="viewModelSelector">A function that can be used to define how your view model
        ///   should be created from your model.</param>
        ///   
        public static DynamicList<TViewModel> ToDynamicList<TViewModel, TModel>(this IEnumerable<TModel> models,
            Func<TModel, TViewModel> viewModelSelector)
            where TViewModel : class
        {
            var list = new DynamicList<TViewModel>();
            list.AddRange(models.Select(viewModelSelector));
            return list;
        }

        /// <summary>
        ///   Converts a list of <typeparamref name="TViewModel"/> objects into
        ///   a <see cref="DynamicList{TViewModel, TOptions}"/>.
        /// </summary>
        /// 
        /// <typeparam name="TViewModel">The type of your view models.</typeparam>
        /// <typeparam name="TModel">The type of your (database) models.</typeparam>
        /// <typeparam name="TOptions">The type of the options to be associated with your view models.</typeparam>
        /// 
        /// <param name="models">A list of database models to be converted into view models, wrapped in
        ///     <typeparamref name="TOptions"/> objects, and then inserted in a dynamic list.</param>
        /// <param name="optionsSelector">A function that can be used to define how your view model
        ///   should be wrapped in a <typeparamref name="TOptions"/> object.</param>
        ///   
        public static DynamicList<TViewModel, TOptions> ToDynamicList<TViewModel, TOptions, TModel>(this IEnumerable<TModel> models,
            Func<TModel, TOptions> optionsSelector)
            where TViewModel : class
            where TOptions : DynamicListItem<TViewModel>, new()
        {
            var list = new DynamicList<TViewModel, TOptions>();
            list.AddRange(models.Select(optionsSelector));
            return list;
        }
        #endregion
        #endregion

    }
}

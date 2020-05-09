// Dynamic View Model (lists)
// Copyright (c) 2020 César Roberto de Souza. Licensed under the MIT license
// cesarsouza@gmail.com - http://crsouza.com

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DynamicVML
{
    /// <summary>
    ///   Represents a list of view model objects that can be added and removed from a form through Ajax.
    ///   This class can be used to create lists of view models that contain custom options for your items.
    /// </summary>
    /// 
    /// <typeparam name="TViewModel">The type of the view model.</typeparam>
    /// <typeparam name="TOptions">The options class type containing the additional options
    ///     you might want to specify, e.g., "Title", or "Subtitle".</typeparam>
    /// 
    /// <seealso cref="DynamicList{TViewModel}" />
    /// 
    /// <remarks>
    /// <para>
    ///   This class implements <see cref="ICollection{TOptions}" />, 
    ///   <see cref="ICollection{TOptions}" />, <see cref="IDynamicList{TOptions}" />,
    ///   and <see cref="IDynamicList" />. 
    /// </para>
    /// <para>
    ///   This class represents a list of HTML elements, but is actually implemented 
    ///   as a <see cref="Dictionary{TKey, TValue}"/> of unique div HTML element IDs and 
    ///   their associated <see typeparamref="TOptions"/> objects and the <see cref="IDynamicListItem{TViewModel}.ViewModel">
    ///   view models</see> contained inside them.
    /// </para>
    /// 
    /// >[!NOTE]
    ///   If you see yourself implementing many properties in your view model using this class,
    ///   you might notice that your properties' type names will start to look a bit <i>too</i> long. In
    ///   that case, you can subclass <see cref="DynamicList{TViewModel, TOptions}"/> and create a type
    ///   that either always implements your custom options, or always use your desired view model. You 
    ///   can then use this subclass in as the property type of your view model collections instead of
    ///   <see cref="DynamicList{TViewModel, TOptions}"/>. This should reduce the length of your property 
    ///   names significantly.
    /// </remarks>
    /// 
    /// <seealso cref="System.Collections.Generic.ICollection{TOptions}" />
    /// <seealso cref="DynamicVML.IDynamicList{TOptions}" />
    /// <seealso cref="DynamicVML.IDynamicList" />
    /// 
    public class DynamicList<TViewModel, TOptions> : ICollection<TOptions>, IDynamicList<TOptions>, IDynamicList
        where TViewModel : class
        where TOptions : DynamicListItem<TViewModel>, new()
    {
        /// <summary>
        ///   Gets or sets the internal dictionary that is used to store 
        ///   <see typeparamref="TOptions"/> by their HTML div id key.
        /// </summary>
        /// 
        protected Dictionary<string, TOptions> Dictionary { get; set; } = new Dictionary<string, TOptions>();

        /// <summary>
        ///   Initializes a new instance of the <see cref="DynamicList{TViewModel, TOptions}"/> class. This
        ///   constructor overload is only used when creating a new item to be added to an existing form in
        ///   an HTML page.
        /// </summary>
        /// 
        /// <param name="containerId">
        ///     The ID of the HTML div element to which the 
        ///     contents of this list should be appended to.
        /// </param>
        /// 
        public DynamicList(string containerId)
        {
            this.ContainerId = containerId;
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="DynamicList{TViewModel, TOptions}"/> class.
        /// </summary>
        /// 
        public DynamicList()
        {
        }

        /// <summary>
        ///   This method creates an unique identified that can be used to identify
        ///   HTML div elements in your form. Those IDs are needed in order to help
        ///   ASP.NET's ASP.NET's <see cref="IModelBinder"/> bind the dynamic view 
        ///   models to your forms.
        /// </summary>
        /// 
        /// <returns>A string containing a GUID value in a HTML-friendly format.</returns>
        /// 
        protected static string CreateId()
        {
            return Regex.Replace(Convert.ToBase64String(Guid.NewGuid().ToByteArray()), "[/+=]", "");
        }

        /// <summary>
        ///   Gets an enumerable that can be used to iterate through the identifiers
        ///   of the div HTML elements used by your view models when rendered in a form.
        /// </summary>
        /// 
        /// <value>The keys, represented as an <see cref="IEnumerable{String}"/>.</value>
        /// 
        public IEnumerable<string> Keys => Dictionary.Keys;

        /// <summary>
        ///   Gets an enumerable that can be used to iterate through your ViewModel objects
        ///   that may be contained in this list.
        /// </summary>
        /// 
        /// <value>The view models, represented as an <see cref="IEnumerable{TViewModel}"/>.</value>
        /// 
        public IEnumerable<TViewModel> ViewModels => Dictionary.Values
                    .Where(x => x.ViewModel != null)
                    .Select(x => x.ViewModel!);

        /// <summary>
        ///   Gets an enumerable that can be used to iterate through the Option objects
        ///   associated with each of your ViewModel objects contained in this list. An
        ///   Options object contains at least a property called "Index" which is necessary
        ///   for ASP.NET's <see cref="IModelBinder"/> to work.
        /// </summary>
        /// 
        /// <value>The view models, represented as an <see cref="IEnumerable{TViewModel}"/>.</value>
        /// 
        public IEnumerable<TOptions> Options => Dictionary.Values;

        /// <summary>
        ///   The HTML div ID associated with this list. This ID is generated
        ///   automatically using <see cref="CreateId"/> and is guaranteed to
        ///   be unique.
        /// </summary>
        /// 
        /// <value>
        ///   The identifier for the HTML div element that contains the representation of 
        ///   this list in an HTML form.
        /// </value>
        /// 
        public string ContainerId { get; set; } = CreateId();

        /// <summary>
        ///   This property is required to help <see cref="IModelBinder"/> during runtime
        ///   and does not have to be set to anything when creating the list.
        /// </summary>
        /// 
        public string? Index { get; set; }

        /// <summary>
        ///   Gets the number of elements contained in this list.
        /// </summary>
        /// 
        /// <value>The number of elements in this list.</value>
        /// 
        public int Count => Dictionary.Count;

        /// <summary>
        ///   Gets a value indicating whether this <see cref="T:System.Collections.Generic.ICollection`1" /> 
        ///   is read-only. In the case of a <see cref="DynamicList{TViewModel, TOptions}"/>, it should always
        ///   return false.
        /// </summary>
        /// 
        /// <value>Always false.</value>
        /// 
        public bool IsReadOnly => false;

        /// <summary>
        ///   Gets the <see typeparamref="TOptions"/> object associated with the specified HTML div 
        ///   ID. Your view models are contained inside the <see typeparamref="TOptions"/> object.
        /// </summary>
        /// 
        /// <param name="id">The HTML div id for the element you want to retrieve.</param>
        /// 
        /// <returns>
        ///     The <see typeparamref="TOptions"/> object containing your ViewModel 
        ///     plus any additional options associated to it.
        /// </returns>
        /// 
        public TOptions this[string id]
        {
            get { return Dictionary[id]; }
        }

        /// <summary>
        ///   Adds multiple elements to this list. Note that, when added, the list
        ///   may change properties of the <see typeparamref="TOptions"/> object (but not
        ///   of your view models).
        /// </summary>
        /// 
        /// <param name="options">The elements to be added to this list.</param>
        /// 
        public void AddRange(IEnumerable<TOptions> options)
        {
            foreach (var v in options)
                this.Add(v);
        }

        /// <summary>
        ///   Adds multiple elements to this list. Note that, when added, the list
        ///   may change properties of the <see typeparamref="TOptions"/> object (but not
        ///   of your view models).
        /// </summary>
        /// 
        /// <param name="viewModels">The view models to be added to this list.</param>
        /// <param name="options">
        ///     A default <see typeparamref="TOptions"/> creation function that can be used to specify
        ///     options associated with each of your view models. This is optional.
        /// </param>
        /// 
        public void AddRange(IEnumerable<TViewModel> viewModels, Func<TViewModel, TOptions>? options = null)
        {
            if (options == null)
            {
                foreach (var v in viewModels)
                    this.Add(v);
            }
            else
            {
                foreach (var v in viewModels)
                {
                    var item = options(v);
                    item.ViewModel = v;
                    this.Add(item);
                }
            }
        }

        /// <summary>
        ///   Adds the specified <see typeparamref="TOptions"/> object to the list. The
        ///   <see cref="IDynamicListItem.Index"/> property of your options object
        ///   may be changed by this method upon insertion.
        /// </summary>
        /// 
        /// <remarks>
        ///   If an item with a duplicate key gets inserted to this list, it will
        ///   replace the old one with this new instance. This is not the same behavior
        ///   of a standard <see cref="Dictionary{TKey, TValue}"/> but is needed to avoid
        ///   model binding errors since <see cref="IModelBinder"/> will call this method
        ///   to reconstruct the <see cref="DynamicList{TViewModel, TOptions}"/> object
        ///   from the user's HTTP request.
        /// </remarks>
        /// 
        /// <param name="options">The options object to be added to this list.</param>
        /// 
        public void Add(TOptions options)
        {
            if (options.ViewModel == null)
            {
                throw new ArgumentNullException(nameof(options),
                    "Tried to add an option object with no associated ViewModel to the list. If trying to" +
                    "customize the list, please make sure that any options objects have their ViewModel property" +
                    "non-null.");
            }

            if (String.IsNullOrEmpty(options.Index))
                options.Index = CreateId();

            if (this.Dictionary.ContainsKey(options.Index))
            {
                // we will update the item
                this.Dictionary[options.Index] = options;
            }
            else
            {
                // add new item
                this.Index = options.Index;
                this.Dictionary.Add(options.Index, options);
            }
        }

        /// <summary>
        ///   Adds the specified <see typeparamref="TViewModel"/> object to the list. The list
        ///   will automatically create a <see typeparamref="TOptions"/> object to wrap it and
        ///   provide it with an unique ID.
        /// </summary>
        /// 
        /// <param name="viewModel">The view model object to be added to this list.</param>
        /// 
        public void Add(TViewModel viewModel)
        {
            this.Add(new TOptions()
            {
                ViewModel = viewModel
            });
        }

        /// <summary>
        ///   Removes all items from this list.
        /// </summary>
        /// 
        public void Clear()
        {
            Dictionary.Clear();
        }

        /// <summary>
        ///   Determines whether the this list contains a specific value. Note that only
        ///   the <see cref="IDynamicListItem.Index"/> property of the provided <see typeparamref="TOptions"/>
        ///   object will be used to determine whether the list contains a similar element or not.
        ///   This method will not look into the actual contents of your options or view model.
        /// </summary>
        /// 
        /// <param name="item">The object to locate in the list.</param>
        /// 
        /// <returns>
        ///   <see langword="true" /> if <paramref name="item" /> is found 
        ///   in the list; otherwise, <see langword="false" />.
        /// </returns>
        /// 
        public bool Contains(TOptions item)
        {
            if (item.Index == null)
                return false;
            return this.Dictionary.ContainsKey(item.Index);
        }

        /// <summary>
        ///   Copies the elements of this list to an <see cref="T:System.Array" />, starting at
        ///   a particular <see cref="T:System.Array" /> index.
        /// </summary>
        /// 
        /// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.ICollection`1" />. The <see cref="T:System.Array" /> must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in <paramref name="array" /> at which copying begins.</param>
        /// 
        public void CopyTo(TOptions[] array, int arrayIndex)
        {
            this.Options.ToArray().CopyTo(array, arrayIndex);
        }

        /// <summary>
        ///   Removes the item with the same key specified in the <see cref="IDynamicListItem.Index"/> 
        ///   property of the provided <paramref name="item"/>.
        /// </summary>
        /// 
        /// <param name="item">The object to remove from the list.</param>
        /// 
        /// <returns>
        ///   <see langword="true" /> if <paramref name="item" /> was successfully
        ///   removed from the list; otherwise, <see langword="false" />. This method also 
        ///   returns <see langword="false" /> if <paramref name="item" /> is not found in
        ///   the original <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </returns>
        /// 
        public bool Remove(TOptions item)
        {
            if (item.Index == null)
                return false;
            return this.Dictionary.Remove(item.Index);
        }

        /// <summary>
        ///   Returns an enumerator that iterates through the collection.
        /// </summary>
        /// 
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        /// 
        public IEnumerator<TOptions> GetEnumerator()
        {
            return this.Dictionary.Values.GetEnumerator();
        }

        /// <summary>
        ///   Returns an enumerator that iterates through a collection.
        /// </summary>
        /// 
        /// <returns>
        ///   An <see cref="T:System.Collections.IEnumerator" /> object 
        ///   that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

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
        public IEnumerable<TModel> ToModel<TModel>(Func<TViewModel, TModel> func)
        {
            return ViewModels.Select(func);
        }

        /// <summary>
        ///   Converts all view models stored inside this instance to their database model 
        ///   counterparts using the specified <paramref name="func">lambda function</paramref>.
        ///   This overload can be used to take the options associated with each view model
        ///   object into consideration during the conversion.
        /// </summary>
        /// 
        /// <typeparam name="TModel">The type for the models to convert to.</typeparam>
        /// 
        /// <param name="func">
        ///     A function that takes a <see typeparamref="TOptions"/> (which contains a 
        ///     <see typeparamref="TViewModel"/>) and converts both to a <see typeparamref="TModel"/>.
        /// </param>
        /// 
        /// <returns>
        ///     A <see cref="IEnumerable{TModel}"/> that can be materialized to an actual 
        ///     list/collection or just iterated over (e.g. using <see cref="System.Linq"/>.
        /// </returns>
        /// 
        public IEnumerable<TModel> ToModel<TModel>(Func<TOptions, TModel> func)
        {
            return Options.Select(func);
        }

        /// <summary>
        ///   Gets the <see typeparamref="TOptions"/> with the specified identifier.
        /// </summary>
        /// 
        /// <remarks>
        ///     This is an explicit interface implementation which is only available when
        ///     interacting with this list through the <see typeparamref="IDynamicList{TOptions}"/>
        ///     interface. Normally, this should only be the case when accessing the list
        ///     from a view.
        /// </remarks>
        /// 
        TOptions IDynamicList<TOptions>.this[string id] => this[id];

        /// <summary>
        ///   Gets the <see typeparamref="TOptions"/> with the specified 
        ///   identifier as an <see cref="IDynamicListItem"/> object.
        /// </summary>
        /// 
        /// <remarks>
        ///     This is an explicit interface implementation which is only available when
        ///     interacting with this list through the <see cref="IDynamicList"/>
        ///     interface. Normally, this should only be the case when accessing the list
        ///     from a view.
        /// </remarks>
        /// 
        IDynamicListItem IDynamicList.this[string id] => this[id];
    }
}

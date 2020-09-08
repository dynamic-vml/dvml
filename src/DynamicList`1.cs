// Dynamic View Model (lists)
// Copyright (c) 2020 César Roberto de Souza. Licensed under the MIT license
// cesarsouza@gmail.com - http://crsouza.com

using System.Collections.Generic;
using System.Linq;

namespace DynamicVML
{
    /// <summary>
    ///   Represents a list of view model objects that can be added and removed from a form through Ajax.
    ///   This class can be used to create lists of view models that contain no custom options. To specify
    ///   custom options for your objects, see <see cref="DynamicList{TViewModel, TOptions}"/>.
    /// </summary>
    /// 
    /// <typeparam name="TViewModel">The type of the view model.</typeparam>
    /// 
    /// <seealso cref="DynamicList{TViewModel, TOptions}" />
    /// 
    public class DynamicList<TViewModel> : DynamicList<TViewModel, DynamicListItem<TViewModel>>, ICollection<TViewModel>
        where TViewModel : class
    {
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
            : base(containerId)
        {
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="DynamicList{TViewModel, TOptions}"/> class.
        /// </summary>
        /// 
        public DynamicList()
            : base()
        {
        }

        /// <summary>
        ///   Determines whether the this list contains a specific value by iterating over all elements of the list.
        /// </summary>
        /// 
        /// <param name="item">The object to locate in the list.</param>
        /// 
        /// <returns>
        ///   <see langword="true" /> if <paramref name="item" /> is found 
        ///   in the list; otherwise, <see langword="false" />.
        /// </returns>
        /// 
        public bool Contains(TViewModel item)
        {
            return base.ViewModels.Contains(item);
        }

        /// <summary>
        ///   Copies the elements of this list to an <see cref="T:System.Array" />, starting at
        ///   a particular <see cref="T:System.Array" /> index.
        /// </summary>
        /// 
        /// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.ICollection`1" />. The <see cref="T:System.Array" /> must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in <paramref name="array" /> at which copying begins.</param>
        /// 
        public void CopyTo(TViewModel[] array, int arrayIndex)
        {
            base.ViewModels.ToArray().CopyTo(array, arrayIndex);
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
        public bool Remove(TViewModel item)
        {
            DynamicListItem<TViewModel>? options = base.Options
                .Where(x => x.ViewModel == item)
                .SingleOrDefault();

            if (options == null)
                return false;

            return base.Remove(options);
        }

        void ICollection<TViewModel>.Add(TViewModel item)
        {
            base.Add(item);
        }

        IEnumerator<TViewModel> IEnumerable<TViewModel>.GetEnumerator()
        {
            return base.ViewModels.GetEnumerator();
        }
    }
}

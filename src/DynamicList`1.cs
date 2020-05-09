// Dynamic View Model (lists)
// Copyright (c) 2020 César Roberto de Souza. Licensed under the MIT license
// cesarsouza@gmail.com - http://crsouza.com

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
    public class DynamicList<TViewModel> : DynamicList<TViewModel, DynamicListItem<TViewModel>>
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
    }
}

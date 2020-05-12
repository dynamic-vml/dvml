// Dynamic View Model (lists)
// Copyright (c) 2020 César Roberto de Souza. Licensed under the MIT license
// cesarsouza@gmail.com - http://crsouza.com

using DynamicVML.Options;

namespace DynamicVML.Internals
{
    /// <summary>
    ///   Represents a collection of read-only parameter values that are being used to
    ///   currently render the list. Those cannot be changed after rendering has started.
    /// </summary>
    /// 
    public class ListParameters : Parameters
    {
        /// <summary>
        ///   Gets the actual <see cref="DynamicListOptions.ItemTemplate"/> being used.
        /// </summary>
        /// 
        public string ItemTemplate { get; }

        /// <summary>
        ///   Gets the actual <see cref="DynamicListOptions.ItemContainerTemplate"/> being used.
        /// </summary>
        /// 
        public string ItemContainerTemplate { get; }

        /// <summary>
        ///   Gets the actual <see cref="DynamicListOptions.ListTemplate"/> being used.
        /// </summary>
        /// 
        public string ListTemplate { get; }

        /// <summary>
        ///   Gets the actual HTML prefix being used for the forms.
        /// </summary>
        /// 
        public string Prefix { get; }

        /// <summary>
        ///   Gets the actual <see cref="ListRenderMode"/> being used.
        /// </summary>
        /// 
        public ListRenderMode Mode { get; }




        /// <summary>
        ///   Creates a new instance of <see cref="ListParameters"/>.
        /// </summary>
        /// 
        public ListParameters(string containerId,
            string itemTemplate, string itemContainerTemplate, string listTemplate,
            string prefix, ListRenderMode mode)
            : base(containerId)
        {
            this.ItemTemplate = itemTemplate;
            this.ItemContainerTemplate = itemContainerTemplate;
            this.ListTemplate = listTemplate;
            this.Prefix = prefix;
            this.Mode = mode;
        }

    }
}

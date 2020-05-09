// Dynamic View Model (lists)
// Copyright (c) 2020 César Roberto de Souza. Licensed under the MIT license
// cesarsouza@gmail.com - http://crsouza.com

namespace DynamicVML
{
    /// <summary>
    ///   Represents different HTTP methods which can be used to retrieve new
    ///   list items from the server. Default is to use <see cref="NewItemMethod.Get"/>.
    /// </summary>
    /// 
    public enum NewItemMethod
    {
        /// <summary>
        ///   New items should be requested via GET.
        /// </summary>
        /// 
        /// <remarks>
        ///   Note: Additional user view data will not be sent to/from the server when using GET
        ///   since the additional data may be too long to be included in a GET request. In this
        ///   case, please switch over to <see cref="NewItemMethod.Post"/>.
        /// </remarks>
        /// 
        Get,

        /// <summary>
        ///   New items should be requested via POST.
        /// </summary>
        /// 
        Post
    }
}

// Dynamic View Model (lists)
// Copyright (c) 2020 César Roberto de Souza. Licensed under the MIT license
// cesarsouza@gmail.com - http://crsouza.com

namespace DynamicVML
{
    /// <summary>
    ///    Represents different rendering modes for the final (user-provided) item template.
    /// </summary>
    /// 
    public enum ListRenderMode
    {
        /// <summary>
        ///   The final user-provided item template expects an instance of their 
        ///   view model as the MVC <c>@model</c> class (e.g., "BookViewModel").
        /// </summary>
        /// 
        ViewModelOnly,

        /// <summary>
        ///   The final user-provided item template expects an instance of their 
        ///   custom options for their view model as the MVC <c>@model</c> class
        ///   (e.g., "TOptions&lt;BookViewModel&gt;").
        /// </summary>
        /// 
        ViewModelWithOptions
    }
}

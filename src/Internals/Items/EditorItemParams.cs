// Dynamic View Model (lists)
// Copyright (c) 2020 César Roberto de Souza. Licensed under the MIT license
// cesarsouza@gmail.com - http://crsouza.com

using DynamicVML.Internals;

namespace DynamicVML.Extensions
{
    public class EditorItemParams
    {
        public AddNewDynamicItem NewItemParams { get; set; }
        public EditorParams EditorParams { get; set; }
        public object AdditionalViewData { get; set; }
        public string CurrentIndex { get; set; }
    }
}
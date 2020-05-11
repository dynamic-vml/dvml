// Dynamic View Model (lists)
// Copyright (c) 2020 César Roberto de Souza. Licensed under the MIT license
// cesarsouza@gmail.com - http://crsouza.com

namespace DynamicVML.Internals
{
    public class ItemEditorParameters : ItemParameters
    {
        public ListEditorParameters Editor { get; }

        public AddNewDynamicItem AddNewItem { get; }

        public ItemEditorParameters(string containerId, string itemId, object? additionalViewData,
            AddNewDynamicItem newItemParameters, ListEditorParameters editorParameters)
            : base(containerId, itemId, additionalViewData)
        {
            this.AddNewItem = newItemParameters;
            this.Editor = editorParameters;
        }
    }
}
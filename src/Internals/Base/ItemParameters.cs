// Dynamic View Model (lists)
// Copyright (c) 2020 César Roberto de Souza. Licensed under the MIT license
// cesarsouza@gmail.com - http://crsouza.com

namespace DynamicVML.Internals
{
    public abstract class ItemParameters : Parameters
    {
        public object? AdditionalViewData { get; }

        public string Index { get; }


        public ItemParameters(string containerId, string itemId, object? additionalViewData)
            : base(containerId)
        {
            this.Index = itemId;
            this.AdditionalViewData = additionalViewData;
        }
    }
}
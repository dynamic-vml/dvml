// Dynamic View Model (lists)
// Copyright (c) 2020 César Roberto de Souza. Licensed under the MIT license
// cesarsouza@gmail.com - http://crsouza.com

namespace DynamicVML.Internals
{
    public class ItemDisplayParameters : ItemParameters
    {
        public ListDisplayParameters Display { get; }

        public ItemDisplayParameters(string containerId, string itemId, object? additionalViewData,
            ListDisplayParameters parameters)
            : base(containerId, itemId, additionalViewData)
        {
            this.Display = parameters;
        }
    }
}
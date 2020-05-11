using System.ComponentModel.DataAnnotations;
using DynamicVML;

namespace Tests
{
    public class SimpleItem
    {
      
        public string ItemProperty { get; set; }

        public SimpleItem(int i)
        {
            this.ItemProperty = "P" + i;
        }
    }
}

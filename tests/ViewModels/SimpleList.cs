using System.ComponentModel.DataAnnotations;
using DynamicVML;

namespace Tests
{
    public class SimpleList
    {

        public string ParentProperty { get; set; }

        public virtual DynamicList<SimpleItem> Items { get; set; } = new DynamicList<SimpleItem>("I");

        public SimpleList()
        {

        }

        public SimpleList(int children)
        {
            for (int i = 0; i < children; i++)
                Items.Add(new SimpleItem(i), m => m.Index = "R" + i);
        }
    }
}

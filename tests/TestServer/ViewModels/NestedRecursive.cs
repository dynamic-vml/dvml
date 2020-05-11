using System.ComponentModel.DataAnnotations;
using DynamicVML;

namespace Tests
{
    public class NestedRecursive
    {
        public int Id { get; set; }

        public string SomeProperty { get; set; }

        public virtual DynamicList<NestedRecursive> Children { get; set; }

        public NestedRecursive(int children, string level = "R")
        {
            Children = new DynamicList<NestedRecursive>(level);

            SomeProperty = $"[{level}:{children}]";

            for (int i = 0; i < children; i++)
            {
                string newLevel = level + i;
                var child = new NestedRecursive(children - 1, newLevel);
                Children.Add(child, o => o.Index = newLevel);
            }
        }

        public override string ToString()
        {
            return SomeProperty;
        }
    }
}

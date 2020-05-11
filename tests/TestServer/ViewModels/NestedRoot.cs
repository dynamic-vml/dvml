using System.ComponentModel.DataAnnotations;
using DynamicVML;

namespace Tests
{
    public class NestedRoot
    {

        public int Id { get; set; }

        public string SomePropertyR { get; set; }

        public virtual DynamicList<NestedA> Children { get; set; }

        public NestedRoot(int nA, int nB, int nC)
        {
            this.Children = new DynamicList<NestedA>("R");

            for (int i = 0; i < nA; i++)
            {
                var a = new NestedA();
                a.Children = new DynamicList<NestedB>("A");
                a.SomePropertyA = $"a: {i}";
                for (int j = 0; j < nB; j++)
                {
                    var b = new NestedB();
                    b.Children = new DynamicList<NestedC>("B");
                    b.SomePropertyB = $"a: {i} b: {j}";
                    for (int k = 0; k < nC; k++)
                    {
                        var c = new NestedC();
                        c.SomePropertyC = $"a: {i} b: {j} c: {k}";
                        b.Children.Add(c, m => m.Index = "B" + k);
                    }
                    a.Children.Add(b, m => m.Index = "A" + j);
                }
                this.Children.Add(a, m => m.Index = "R" + i);
            }

        }
    }

    public class NestedA
    {
        public string SomePropertyA { get; set; }

        public virtual DynamicList<NestedB> Children { get; set; } = new DynamicList<NestedB>();
    }

    public class NestedB
    {
        public string SomePropertyB { get; set; }

        public virtual DynamicList<NestedC> Children { get; set; } = new DynamicList<NestedC>();
    }

    public class NestedC
    {
        public string SomePropertyC { get; set; }
    }


}

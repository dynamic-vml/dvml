using DynamicVML;

namespace Tests.ViewModels
{
    public interface ITestOptions : IDynamicListItem
    {
        string TestText { get; set; }
    }

    public class TestOptions<T> : DynamicListItem<T>, ITestOptions
        where T : class
    {
        public string TestText { get; set; }
    }

    public interface IWrongOptions : IDynamicListItem
    {
        string WrongText { get; set; }
    }

    public class WrongOptions<T> : DynamicListItem<T>, IWrongOptions
        where T : class
    {
        public string WrongText { get; set; }
    }
}

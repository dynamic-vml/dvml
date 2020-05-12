using System.ComponentModel.DataAnnotations;

using DynamicVML;

namespace Tests
{
    public class AuthorViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Display Name")]
        public string Name { get; set; }

        [Display(Name = "Authored books")]
        public virtual DynamicList<BookViewModel> Books { get; set; } = new DynamicList<BookViewModel>();
    }
}

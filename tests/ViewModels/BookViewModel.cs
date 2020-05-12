using System.ComponentModel.DataAnnotations;

namespace Tests
{
    public class BookViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Title")]
        public string Title { get; set; }

        [Display(Name = "Publication year")]
        public int PublicationYear { get; set; }
    }
}

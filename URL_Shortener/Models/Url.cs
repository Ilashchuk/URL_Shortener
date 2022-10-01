using System.ComponentModel.DataAnnotations;

namespace URL_Shortener.Models
{
    public class Url
    {
        public int Id { get; set; }
        public string? Link { get; set; }
        public string? ShortLink { get; set; }
        public int UserId { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date")]
        public DateTime Date { get; set; }

        public User? User { get; set; }
    }
}

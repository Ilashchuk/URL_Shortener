using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace URL_Shortener.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string? FullName { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
        public int? RoleId { get; set; }
        public Role? Role { get; set; }
        public ICollection<Url> Urls { get; set; } = new List<Url>();
    }
}

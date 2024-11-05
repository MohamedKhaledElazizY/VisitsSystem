using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VisitsSystem.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string HashedPassword { get; set; }
        public string? Hash { get; set; }
        public bool Degree { get; set; }
        public int OfficeId { get; set; }

        [ForeignKey("OfficeId")]
        public Office? Office { get; set; }

    }
}

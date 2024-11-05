using System.ComponentModel.DataAnnotations;

namespace VisitsSystem.Models
{
    public class Office
    {
        [Key]
        public int OfficeId { get; set; }
        [Required]
        public string OfficeName { get; set; }

        public List<User> UsersList { get; set; }
    }
}

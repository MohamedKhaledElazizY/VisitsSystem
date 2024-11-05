using System.ComponentModel.DataAnnotations;

namespace VisitsSystem.Models
{
    public class Visitor
    {
        [Key]
        public int VisitorId { get; set; }
        [Required]
        public string Rank { get; set; }
        [Required]
        public string VisitorName { get; set; }
        [Required]
        public string JobTitle { get; set; }
    }
}
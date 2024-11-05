using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VisitsSystem.Models
{
    public class PostponedVisit
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public DateTime PostponedDate { get; set; }
        public int VisitId { get; set; }

        [ForeignKey("VisitId")]
        public Visit Visit { get; set; }
    }
}

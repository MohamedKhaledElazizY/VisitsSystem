using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VisitsSystem.Models
{
    public class Visit
    {
        [Key]
        public int VisitId { get; set; }
        [Required]
        public DateTime ArrivalDate { get; set; }
        public DateTime? EntryDate { get; set; }
        public DateTime? LeavingDate { get; set; }
        public string? Notes { get; set; }
        public int State { get; set; }
        public int VisitorId { get; set; }
        public int? OfficeId { get; set; }

        [ForeignKey("VisitorId")]
        public Visitor? Visitor { get; set; }
        [ForeignKey("OfficeId")]
        public Office? office { get; set; }



    }
}

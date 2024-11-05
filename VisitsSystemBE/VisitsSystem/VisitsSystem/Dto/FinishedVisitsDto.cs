using VisitsSystem.Models;

namespace VisitsSystem.Dto
{
    public class FinishedVisitsDto
    {
        public int VisitId { get; set; }
        public DateTime ArrivalDate { get; set; }
        public DateTime? EntryDate { get; set; }
        public DateTime? LeavingDate { get; set; }
        public string? Notes { get; set; }
        public int State { get; set; }
        public string? StateName { get; set; }
        public int VisitorId { get; set; }
        public int? OfficeId { get; set; }
        public Visitor? Visitor { get; set; }
    }
}

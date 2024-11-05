namespace VisitsSystem.Dto
{
    public class VisitDto
    {
        public DateTime? ArrivalDate { get; set; }
        public DateTime? EntryDate { get; set; }
        public DateTime? LeavingDate { get; set; }
        public string? Notes { get; set; }
        public int? State { get; set; }
        public int VisitorId { get; set; }

    }
}

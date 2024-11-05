namespace VisitsSystem.Dto
{
    public class UserDto
    {
        public int? UserId { get; set; }
        public string UserName { get; set; }
        public string HashedPassword { get; set; }
        public bool? Degree { get; set; }
        public int? OfficeId { get; set; }
    }
}

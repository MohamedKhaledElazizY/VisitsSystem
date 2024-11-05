using System.ComponentModel.DataAnnotations;

namespace VisitsSystem.Dto
{
    public class VisitorsTblDto
    {

        public string Rank { get; set; }
       // [Required]
        public string VisitorName { get; set; }
      //  [Required]
        public string JobTitle { get; set; }
    }
}

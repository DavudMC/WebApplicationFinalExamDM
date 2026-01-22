using WebApplicationFinalExamDM.Models.Common;

namespace WebApplicationFinalExamDM.Models
{
    public class Member : BaseEntity
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string ImagePath { get; set; } = string.Empty;
        public int PositionId { get; set; }
        public Position Position { get; set; }
    }
}

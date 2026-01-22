using WebApplicationFinalExamDM.Models.Common;

namespace WebApplicationFinalExamDM.Models
{
    public class Position : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public ICollection<Member> Members { get; set; } = [];
    }
}

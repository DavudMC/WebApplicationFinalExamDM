namespace WebApplicationFinalExamDM.ViewModels.MemberViewModels
{
    public class MemberGetVM
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string ImagePath { get; set; } = string.Empty;
        public int PositionId { get; set; }
        public string PositionName { get; set; } = string.Empty;
    }
}

using System.ComponentModel.DataAnnotations;

namespace WebApplicationFinalExamDM.ViewModels.MemberViewModels
{
    public class MemberCreateVM
    {
        [Required,MaxLength(256),MinLength(3)]
        public string FirstName { get; set; } = string.Empty;
        [Required, MaxLength(256), MinLength(3)]
        public string LastName { get; set; } = string.Empty;
        [Required]
        public IFormFile Image { get; set; }
        [Required]
        public int PositionId { get; set; }
    }
}

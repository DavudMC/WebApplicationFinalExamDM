using System.ComponentModel.DataAnnotations;

namespace WebApplicationFinalExamDM.ViewModels.PositionViewModels
{
    public class PositionCreateVM
    {
        [Required,MaxLength(256),MinLength(3)]
        public string Name { get; set; } = string.Empty;
    }
}

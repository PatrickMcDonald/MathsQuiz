using System.ComponentModel.DataAnnotations;

namespace MathsQuizApp.Models
{
    public record MultiplicationViewModel(int Operand1, int Operand2)
    {
        [Display(Name = "Your answer")]
        [Required]
        public int? Answer { get; set; }
    }
}

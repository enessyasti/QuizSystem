using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace BeFit.Models
{
    public class TrainingSession
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Start Time")]
        public DateTime StartTime { get; set; }

        [Required]
        [Display(Name = "End Time")]
        public DateTime EndTime { get; set; }

        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }

        public bool IsValid => StartTime <= EndTime;

        public ICollection<ExercisePerformed> Exercises { get; set; }
    }
}

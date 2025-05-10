using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenGamedev.Models
{
    public class SolutionVote
    {
        [Key]
        public long Id { get; set; }

        [Required]
        [Display(Name = "ID решения")]
        [ForeignKey(nameof(Solution))]
        public long SolutionId { get; set; }

        [Required]
        [Display(Name = "Решение")]
        public required Solution Solution { get; set; }

        [Required]
        [Display(Name = "ID пользователя")]
        [ForeignKey(nameof(User))]
        public required string UserId { get; set; }

        [Required]
        [Display(Name = "Пользователь")]
        public required ApplicationUser User { get; set; }

        [Display(Name = "Тип голоса")]
        public VoteType VoteType { get; set; }

        [Display(Name = "Время голосования")]
        [DataType(DataType.DateTime)]
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}

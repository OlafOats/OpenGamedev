using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenGamedev.Models
{
    public enum VoteType
    {
        Upvote,
        Downvote
    }

    public class FeatureRequestVote
    {
        [Key]
        public long Id { get; set; }

        [Required]
        [Display(Name = "ID запроса")]
        public long FeatureRequestId { get; set; }

        [Required]
        [ForeignKey(nameof(FeatureRequestId))]
        [Display(Name = "Запрос")]
        public virtual FeatureRequest FeatureRequest { get; set; } = default!;

        [Required]
        [Display(Name = "ID пользователя")]
        public string UserId { get; set; } = default!;

        [Required]
        [ForeignKey(nameof(UserId))]
        [Display(Name = "Пользователь")]
        public virtual ApplicationUser User { get; set; } = default!;

        [Display(Name = "Тип голоса")]
        public VoteType VoteType { get; set; }

        [Display(Name = "Время голосования")]
        [DataType(DataType.DateTime)]
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        [Display(Name = "Предложенная длительность голосования за решение")]
        public TimeSpan? SuggestedSolutionVotingDuration { get; set; }
    }
}

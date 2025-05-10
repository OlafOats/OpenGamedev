using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenGamedev.Models
{
    public enum SolutionStatus
    {
        Proposed,
        SolutionVoting,
        ApprovedForBuild,
        BuildInProgress,
        BuildSuccess,
        BuildFailed,
        Accepted,
        Rejected
    }

    public class Solution
    {
        [Key]
        public long Id { get; set; }

        [Required]
        [Display(Name = "ID запроса")]
        [ForeignKey(nameof(FeatureRequest))]
        public long FeatureRequestId { get; set; }

        [Required]
        [Display(Name = "Запрос")]
        public required FeatureRequest FeatureRequest { get; set; }

        [Required]
        [Display(Name = "ID автора")]
        [ForeignKey(nameof(Author))]
        public required string AuthorId { get; set; }

        [Required]
        [Display(Name = "Автор")]
        public required ApplicationUser Author { get; set; }

        [Required]
        [Display(Name = "Описание решения")]
        [DataType(DataType.MultilineText)]
        public required string Description { get; set; }

        [Display(Name = "Дата создания")]
        [DataType(DataType.DateTime)]
        public DateTime CreationDate { get; set; } = DateTime.UtcNow;

        [Display(Name = "Статус решения")]
        public SolutionStatus Status { get; set; } = SolutionStatus.Proposed;

        [Display(Name = "Ссылка на исходный код")]
        [Url(ErrorMessage = "Неверный формат URL.")]
        public string? SourceCodeUrl { get; set; }

        [Display(Name = "Ссылка на артефакт сборки")]
        [Url(ErrorMessage = "Неверный формат URL.")]
        public string? BuildArtifactUrl { get; set; }

        [Display(Name = "Лог сборки")]
        [DataType(DataType.MultilineText)]
        public string? BuildLog { get; set; }

        [Display(Name = "Дата сборки")]
        [DataType(DataType.DateTime)]
        public DateTime? BuildDate { get; set; }

        [Display(Name = "Голоса за решение")]
        [InverseProperty(nameof(SolutionVote.Solution))]
        public ICollection<SolutionVote> Votes { get; set; } = new HashSet<SolutionVote>();
    }
}

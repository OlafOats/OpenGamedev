using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenGamedev.Models
{
    public enum FeatureRequestStatus
    {
        Proposed,
        WaitingDependency,
        TaskVoting,
        ReadyForImplementation,
        InProgress,
        SolutionVotingExpired,
        Implemented,
        Superseded,
        Closed,
        Cancelled
    }

    public class FeatureRequest
    {
        [Key]
        public long Id { get; set; }

        [Required]
        [ForeignKey(nameof(Project))]
        [Display(Name = "ID проекта")]
        public long ProjectId { get; set; }

        [Required]
        [Display(Name = "Проект")]
        public required Project Project { get; set; }

        [Required]
        [ForeignKey(nameof(Category))]
        [Display(Name = "ID категории")]
        public long CategoryId { get; set; }

        [Required]
        [Display(Name = "Категория")]
        public required FeatureCategory Category { get; set; }

        [Required]
        [MaxLength(255)]
        [Display(Name = "Заголовок запроса")]
        public required string Title { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Описание запроса")]
        public required string Description { get; set; }

        [Display(Name = "Дата создания")]
        [DataType(DataType.DateTime)]
        public DateTime CreationDate { get; set; } = DateTime.UtcNow;

        [Display(Name = "Статус запроса")]
        public FeatureRequestStatus Status { get; set; } = FeatureRequestStatus.Proposed;

        [Required]
        [ForeignKey(nameof(Author))]
        [Display(Name = "ID автора")]
        public required string AuthorId { get; set; }

        [Required]
        [Display(Name = "Автор")]
        public required ApplicationUser Author { get; set; }

        [Display(Name = "Средняя продолжительность голосования за решение")]
        public TimeSpan? AverageSolutionVotingDuration { get; set; }

        [Display(Name = "Время начала голосования за решение")]
        [DataType(DataType.DateTime)]
        public DateTime? SolutionVotingStartTime { get; set; }

        [Display(Name = "Решения")]
        public virtual ICollection<Solution> Solutions { get; set; } = new HashSet<Solution>();

        [Display(Name = "Голоса за запрос")]
        public virtual ICollection<FeatureRequestVote> Votes { get; set; } = new HashSet<FeatureRequestVote>();

        [Display(Name = "Запросы, от которых зависит данный запрос")]
        [InverseProperty(nameof(FeatureRequestDependency.FeatureRequest))]
        public virtual ICollection<FeatureRequestDependency> DependentOnTasks { get; set; } = new HashSet<FeatureRequestDependency>();

        [Display(Name = "Запросы, зависящие от данного запроса")]
        [InverseProperty(nameof(FeatureRequestDependency.DependsOnFeatureRequest))]
        public virtual ICollection<FeatureRequestDependency> TasksDependingOnThis { get; set; } = new HashSet<FeatureRequestDependency>();

        [Display(Name = "Рабочие Области")]
        [InverseProperty(nameof(FeatureRequestWorkArea.FeatureRequest))]
        public virtual ICollection<FeatureRequestWorkArea> FeatureRequestWorkAreas { get; set; } = new HashSet<FeatureRequestWorkArea>();

        [Display(Name = "Устарела из-за запроса ID")]
        [ForeignKey(nameof(SupersededBy))]
        public long? SupersededByFeatureRequestId { get; set; }

        [Display(Name = "Устарела из-за запроса")]
        public virtual FeatureRequest? SupersededBy { get; set; } 

        [InverseProperty(nameof(SupersededBy))]
        public virtual ICollection<FeatureRequest> TasksSupersededByThis { get; set; } = new HashSet<FeatureRequest>();
        public int Priority { get; internal set; }
    }
}

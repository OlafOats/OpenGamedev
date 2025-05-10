using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenGamedev.Models
{
    public class Project
    {
        [Key]
        public long Id { get; set; }

        [Required]
        [MaxLength(255)]
        [Display(Name = "Название проекта")]
        public required string Title { get; set; }

        [Display(Name = "Описание проекта")]
        [DataType(DataType.MultilineText)]
        public string? Description { get; set; }

        [Display(Name = "Ссылка на репозиторий")]
        [Url(ErrorMessage = "Неверный формат URL.")]
        public string? RepositoryLink { get; set; }

        [Display(Name = "Дата создания")]
        [DataType(DataType.DateTime)]
        public DateTime CreationDate { get; set; } = DateTime.UtcNow;

        [InverseProperty(nameof(FeatureRequest.Project))]
        [Display(Name = "Запросы фич")]
        public ICollection<FeatureRequest> FeatureRequests { get; set; } = new HashSet<FeatureRequest>();
    }
}

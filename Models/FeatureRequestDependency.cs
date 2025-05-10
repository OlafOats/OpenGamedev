using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenGamedev.Models
{
    public class FeatureRequestDependency
    {
        [Key]
        public long Id { get; set; }

        [Required]
        [Display(Name = "Запрос, который зависит")]
        public long FeatureRequestId { get; set; }

        [Required]
        [ForeignKey(nameof(FeatureRequestId))]
        [InverseProperty(nameof(FeatureRequest.DependentOnTasks))]
        public virtual FeatureRequest FeatureRequest { get; set; } = default!;

        [Required]
        [Display(Name = "Запрос, от которого зависит")]
        public long DependsOnFeatureRequestId { get; set; }

        [Required]
        [ForeignKey(nameof(DependsOnFeatureRequestId))]
        [InverseProperty(nameof(FeatureRequest.TasksDependingOnThis))]
        public virtual FeatureRequest DependsOnFeatureRequest { get; set; } = default!;
    }
}

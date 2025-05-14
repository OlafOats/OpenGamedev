using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenGamedev.Models
{
    public class WorkArea
    {
        [Key]
        public long Id { get; set; }

        [Required]
        [MaxLength(100)]
        [Display(Name = "Название Рабочей Области")]
        public required string Name { get; set; }

        [Display(Name = "Описание Рабочей Области")]
        [DataType(DataType.MultilineText)]
        public string? Description { get; set; }

        [InverseProperty(nameof(FeatureRequestWorkArea.WorkArea))]
        public virtual ICollection<FeatureRequestWorkArea> FeatureRequestWorkAreas { get; set; } = new HashSet<FeatureRequestWorkArea>();
    }
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenGamedev.Models
{
    public class FeatureCategory
    {
        [Key]
        public long Id { get; set; }

        [Required]
        [MaxLength(100)]
        [Display(Name = "Название категории")]
        public required string Name { get; set; }

        [Display(Name = "Описание категории")]
        public string? Description { get; set; }

        [InverseProperty("Category")]
        public virtual ICollection<FeatureRequest> FeatureRequests { get; set; } = new HashSet<FeatureRequest>();
    }
}

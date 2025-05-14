using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenGamedev.Models
{
    public class FeatureRequestWorkArea
    {
        [Key]
        [Column(Order = 1)] 
        [Display(Name = "ID запроса")]
        public long FeatureRequestId { get; set; }

        [Key]
        [Column(Order = 2)] 
        [Display(Name = "ID Рабочей Области")]
        public long WorkAreaId { get; set; }

        [ForeignKey(nameof(FeatureRequestId))]
        public virtual FeatureRequest FeatureRequest { get; set; } = default!;

        [ForeignKey(nameof(WorkAreaId))]
        public virtual WorkArea WorkArea { get; set; } = default!;
    }
}
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OpenGamedev.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Дата регистрации")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;

        [InverseProperty("Author")]
        public virtual ICollection<FeatureRequest> CreatedFeatureRequests { get; set; } = new HashSet<FeatureRequest>();

        [InverseProperty("Author")]
        public virtual ICollection<Solution> CreatedSolutions { get; set; } = new HashSet<Solution>();

        [InverseProperty("User")]
        public virtual ICollection<FeatureRequestVote> FeatureRequestVotes { get; set; } = new HashSet<FeatureRequestVote>();

        [InverseProperty("User")]
        public virtual ICollection<SolutionVote> SolutionVotes { get; set; } = new HashSet<SolutionVote>();
    }
}

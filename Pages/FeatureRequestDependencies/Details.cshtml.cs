using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OpenGamedev.Data;
using OpenGamedev.Models;

namespace OpenGamedev.Pages.FeatureRequestDependencies
{
    public class DetailsModel(OpenGamedev.Data.OpenGamedevContext context) : PageModel
    {
        private readonly OpenGamedev.Data.OpenGamedevContext _context = context;

        public FeatureRequestDependency FeatureRequestDependency { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var featurerequestdependency = await _context.FeatureRequestDependencies.FirstOrDefaultAsync(m => m.Id == id);

            if (featurerequestdependency is not null)
            {
                FeatureRequestDependency = featurerequestdependency;

                return Page();
            }

            return NotFound();
        }
    }
}

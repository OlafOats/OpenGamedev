using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OpenGamedev.Data;
using OpenGamedev.Models;

namespace OpenGamedev.Pages.FeatureCategories
{
    public class DetailsModel : PageModel
    {
        private readonly OpenGamedev.Data.OpenGamedevContext _context;

        public DetailsModel(OpenGamedev.Data.OpenGamedevContext context)
        {
            _context = context;
        }

        public FeatureCategory FeatureCategory { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var featurecategory = await _context.FeatureCategories.FirstOrDefaultAsync(m => m.Id == id);

            if (featurecategory is not null)
            {
                FeatureCategory = featurecategory;

                return Page();
            }

            return NotFound();
        }
    }
}

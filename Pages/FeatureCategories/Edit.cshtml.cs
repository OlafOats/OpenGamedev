using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OpenGamedev.Data;
using OpenGamedev.Models;

namespace OpenGamedev.Pages.FeatureCategories
{
    public class EditModel : PageModel
    {
        private readonly OpenGamedev.Data.OpenGamedevContext _context;

        public EditModel(OpenGamedev.Data.OpenGamedevContext context)
        {
            _context = context;
        }

        [BindProperty]
        public FeatureCategory FeatureCategory { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var featurecategory =  await _context.FeatureCategories.FirstOrDefaultAsync(m => m.Id == id);
            if (featurecategory == null)
            {
                return NotFound();
            }
            FeatureCategory = featurecategory;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(FeatureCategory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FeatureCategoryExists(FeatureCategory.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool FeatureCategoryExists(long id)
        {
            return _context.FeatureCategories.Any(e => e.Id == id);
        }
    }
}

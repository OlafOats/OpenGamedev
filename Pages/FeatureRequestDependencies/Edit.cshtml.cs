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

namespace OpenGamedev.Pages.FeatureRequestDependencies
{
    public class EditModel(OpenGamedev.Data.OpenGamedevContext context) : PageModel
    {
        private readonly OpenGamedev.Data.OpenGamedevContext _context = context;

        [BindProperty]
        public FeatureRequestDependency FeatureRequestDependency { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var featurerequestdependency =  await _context.FeatureRequestDependencies.FirstOrDefaultAsync(m => m.Id == id);
            if (featurerequestdependency == null)
            {
                return NotFound();
            }
            FeatureRequestDependency = featurerequestdependency;
           ViewData["DependsOnFeatureRequestId"] = new SelectList(_context.FeatureRequests, "Id", "Description");
           ViewData["FeatureRequestId"] = new SelectList(_context.FeatureRequests, "Id", "Description");
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

            _context.Attach(FeatureRequestDependency).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FeatureRequestDependencyExists(FeatureRequestDependency.Id))
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

        private bool FeatureRequestDependencyExists(long id)
        {
            return _context.FeatureRequestDependencies.Any(e => e.Id == id);
        }
    }
}

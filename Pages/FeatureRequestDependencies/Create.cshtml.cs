using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using OpenGamedev.Data;
using OpenGamedev.Models;

namespace OpenGamedev.Pages.FeatureRequestDependencies
{
    public class CreateModel(OpenGamedev.Data.OpenGamedevContext context) : PageModel
    {
        private readonly OpenGamedev.Data.OpenGamedevContext _context = context;

        public IActionResult OnGet()
        {
        ViewData["DependsOnFeatureRequestId"] = new SelectList(_context.FeatureRequests, "Id", "Description");
        ViewData["FeatureRequestId"] = new SelectList(_context.FeatureRequests, "Id", "Description");
            return Page();
        }

        [BindProperty]
        public FeatureRequestDependency FeatureRequestDependency { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.FeatureRequestDependencies.Add(FeatureRequestDependency);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}

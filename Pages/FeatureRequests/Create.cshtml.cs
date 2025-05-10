using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using OpenGamedev.Data;
using OpenGamedev.Models;

namespace OpenGamedev.Pages.FeatureRequests
{
    public class CreateModel(OpenGamedev.Data.OpenGamedevContext context) : PageModel
    {
        private readonly OpenGamedev.Data.OpenGamedevContext _context = context;

        public IActionResult OnGet()
        {
        ViewData["AuthorId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id");
        ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "Title");
            return Page();
        }

        [BindProperty]
        public FeatureRequest FeatureRequest { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.FeatureRequests.Add(FeatureRequest);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}

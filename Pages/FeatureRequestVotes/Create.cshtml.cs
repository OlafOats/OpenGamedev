using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using OpenGamedev.Data;
using OpenGamedev.Models;

namespace OpenGamedev.Pages.FeatureRequestVotes
{
    public class CreateModel(OpenGamedev.Data.OpenGamedevContext context) : PageModel
    {
        private readonly OpenGamedev.Data.OpenGamedevContext _context = context;

        public IActionResult OnGet()
        {
        ViewData["FeatureRequestId"] = new SelectList(_context.FeatureRequests, "Id", "Description");
        ViewData["UserId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id");
            return Page();
        }

        [BindProperty]
        public FeatureRequestVote FeatureRequestVote { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.FeatureRequestVotes.Add(FeatureRequestVote);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}

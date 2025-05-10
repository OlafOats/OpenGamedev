using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using OpenGamedev.Data;
using OpenGamedev.Models;

namespace OpenGamedev.Pages.SolutionVotes
{
    public class CreateModel : PageModel
    {
        private readonly OpenGamedev.Data.OpenGamedevContext _context;

        public CreateModel(OpenGamedev.Data.OpenGamedevContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["SolutionId"] = new SelectList(_context.Solutions, "Id", "Description");
        ViewData["UserId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id");
            return Page();
        }

        [BindProperty]
        public SolutionVote SolutionVote { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.SolutionVotes.Add(SolutionVote);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OpenGamedev.Data;
using OpenGamedev.Models;

namespace OpenGamedev.Pages.SolutionVotes
{
    public class DeleteModel : PageModel
    {
        private readonly OpenGamedev.Data.OpenGamedevContext _context;

        public DeleteModel(OpenGamedev.Data.OpenGamedevContext context)
        {
            _context = context;
        }

        [BindProperty]
        public SolutionVote SolutionVote { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var solutionvote = await _context.SolutionVotes.FirstOrDefaultAsync(m => m.Id == id);

            if (solutionvote is not null)
            {
                SolutionVote = solutionvote;

                return Page();
            }

            return NotFound();
        }

        public async Task<IActionResult> OnPostAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var solutionvote = await _context.SolutionVotes.FindAsync(id);
            if (solutionvote != null)
            {
                SolutionVote = solutionvote;
                _context.SolutionVotes.Remove(SolutionVote);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}

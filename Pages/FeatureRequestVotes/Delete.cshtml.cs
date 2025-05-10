using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OpenGamedev.Data;
using OpenGamedev.Models;

namespace OpenGamedev.Pages.FeatureRequestVotes
{
    public class DeleteModel : PageModel
    {
        private readonly OpenGamedev.Data.OpenGamedevContext _context;

        public DeleteModel(OpenGamedev.Data.OpenGamedevContext context)
        {
            _context = context;
        }

        [BindProperty]
        public FeatureRequestVote FeatureRequestVote { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var featurerequestvote = await _context.FeatureRequestVotes.FirstOrDefaultAsync(m => m.Id == id);

            if (featurerequestvote is not null)
            {
                FeatureRequestVote = featurerequestvote;

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

            var featurerequestvote = await _context.FeatureRequestVotes.FindAsync(id);
            if (featurerequestvote != null)
            {
                FeatureRequestVote = featurerequestvote;
                _context.FeatureRequestVotes.Remove(FeatureRequestVote);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OpenGamedev.Data;
using OpenGamedev.Models;

namespace OpenGamedev.Pages.Solutions
{
    public class DeleteModel : PageModel
    {
        private readonly OpenGamedev.Data.OpenGamedevContext _context;

        public DeleteModel(OpenGamedev.Data.OpenGamedevContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Solution Solution { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var solution = await _context.Solutions.FirstOrDefaultAsync(m => m.Id == id);

            if (solution is not null)
            {
                Solution = solution;

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

            var solution = await _context.Solutions.FindAsync(id);
            if (solution != null)
            {
                Solution = solution;
                _context.Solutions.Remove(Solution);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}

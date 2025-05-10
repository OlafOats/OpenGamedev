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

namespace OpenGamedev.Pages.Solutions
{
    public class EditModel : PageModel
    {
        private readonly OpenGamedev.Data.OpenGamedevContext _context;

        public EditModel(OpenGamedev.Data.OpenGamedevContext context)
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

            var solution =  await _context.Solutions.FirstOrDefaultAsync(m => m.Id == id);
            if (solution == null)
            {
                return NotFound();
            }
            Solution = solution;
           ViewData["AuthorId"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id");
           ViewData["FeatureRequestId"] = new SelectList(_context.Set<Task>(), "Id", "Description");
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

            _context.Attach(Solution).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SolutionExists(Solution.Id))
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

        private bool SolutionExists(long id)
        {
            return _context.Solutions.Any(e => e.Id == id);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using OpenGamedev.Data;
using OpenGamedev.Models;

namespace OpenGamedev.Pages.FeatureCategories
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
            return Page();
        }

        [BindProperty]
        public FeatureCategory FeatureCategory { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.FeatureCategories.Add(FeatureCategory);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}

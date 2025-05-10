using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OpenGamedev.Data;
using OpenGamedev.Models;

namespace OpenGamedev.Pages.FeatureCategories
{
    public class IndexModel(OpenGamedev.Data.OpenGamedevContext context) : PageModel
    {
        private readonly OpenGamedev.Data.OpenGamedevContext _context = context;

        public IList<FeatureCategory> FeatureCategory { get;set; } = default!;

        public async Task OnGetAsync()
        {
            FeatureCategory = await _context.FeatureCategories.ToListAsync();
        }
    }
}

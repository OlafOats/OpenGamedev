using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OpenGamedev.Data;
using OpenGamedev.Models;

namespace OpenGamedev.Pages.FeatureRequestDependencies
{
    public class IndexModel(OpenGamedev.Data.OpenGamedevContext context) : PageModel
    {
        private readonly OpenGamedev.Data.OpenGamedevContext _context = context;

        public IList<FeatureRequestDependency> FeatureRequestDependency { get;set; } = default!;

        public async Task OnGetAsync()
        {
            FeatureRequestDependency = await _context.FeatureRequestDependencies
                .Include(f => f.DependsOnFeatureRequest)
                .Include(f => f.FeatureRequest).ToListAsync();
        }
    }
}

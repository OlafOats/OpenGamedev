using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OpenGamedev.Data;
using OpenGamedev.Models;

namespace OpenGamedev.Pages.FeatureRequests
{
    public class IndexModel(OpenGamedev.Data.OpenGamedevContext context) : PageModel
    {
        private readonly OpenGamedev.Data.OpenGamedevContext _context = context;

        public IList<FeatureRequest> FeatureRequest { get;set; } = default!;

        public async Task OnGetAsync()
        {
            FeatureRequest = await _context.FeatureRequests
                .Include(f => f.Author)
                .Include(f => f.Project).ToListAsync();
        }
    }
}

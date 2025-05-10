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
    public class IndexModel : PageModel
    {
        private readonly OpenGamedev.Data.OpenGamedevContext _context;

        public IndexModel(OpenGamedev.Data.OpenGamedevContext context)
        {
            _context = context;
        }

        public IList<Solution> Solution { get;set; } = default!;

        public async System.Threading.Tasks.Task OnGetAsync()
        {
            Solution = await _context.Solutions
                .Include(s => s.Author)
                .Include(s => s.FeatureRequest).ToListAsync();
        }
    }
}

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
    public class IndexModel(OpenGamedev.Data.OpenGamedevContext context) : PageModel
    {
        private readonly OpenGamedev.Data.OpenGamedevContext _context = context;

        public IList<FeatureRequestVote> FeatureRequestVote { get;set; } = default!;

        public async Task OnGetAsync()
        {
            FeatureRequestVote = await _context.FeatureRequestVotes
                .Include(f => f.FeatureRequest)
                .Include(f => f.User).ToListAsync();
        }
    }
}

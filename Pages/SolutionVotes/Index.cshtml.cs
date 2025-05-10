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
    public class IndexModel(OpenGamedev.Data.OpenGamedevContext context) : PageModel
    {
        private readonly OpenGamedev.Data.OpenGamedevContext _context = context;

        public IList<SolutionVote> SolutionVote { get;set; } = default!;

        public async Task OnGetAsync()
        {
            SolutionVote = await _context.SolutionVotes
                .Include(s => s.Solution)
                .Include(s => s.User).ToListAsync();
        }
    }
}

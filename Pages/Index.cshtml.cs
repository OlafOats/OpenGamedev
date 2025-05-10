using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OpenGamedev.Data;
using OpenGamedev.Models;

namespace OpenGamedev.Pages
{
    public class IndexModel(OpenGamedevContext context) : PageModel
    {
        private readonly OpenGamedevContext _context = context;

        public IList<Project> Projects { get; set; } = default!;

        public async Task OnGetAsync()
        {
            Projects = await _context.Projects.ToListAsync();
        }
    }
}

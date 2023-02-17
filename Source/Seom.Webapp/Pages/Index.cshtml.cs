using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Seom.Application.Infrastructure;

namespace Seom.Webapp.Pages;

public class IndexModel : PageModel
{
    private readonly SeomContext _db;

    public IndexModel(SeomContext db)
    {
        _db = db;
    }

    public void OnGet()
    {

    }
}

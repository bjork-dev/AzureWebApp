using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using AzureWebApp.Data;
using AzureWebApp.Models;

namespace AzureWebApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IDataAccess _client;

        public Todo[] Todo { get; set; }
        [BindProperty]
        public Todo AddTodo { get; set; }
        public IndexModel(ILogger<IndexModel> logger, IDataAccess client)
        {
            _logger = logger;
            _client = client;
        }

        public async Task<ActionResult> OnGetAsync()
        {
            Todo = await _client.GetAsync();
            return Page();
        }

        public async Task<RedirectToPageResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return RedirectToPage("Index");
            await _client.PostAsync(AddTodo);
            return RedirectToPage("Index");
        }

        public async Task<RedirectToPageResult> OnPostDelete(string id)
        {
            await _client.DeleteAsync(id);

            return RedirectToPage("Index");
        }
    }
}

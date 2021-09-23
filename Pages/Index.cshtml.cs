using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using AzureWebApp.Data;
using AzureWebApp.Models;

namespace AzureWebApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IDataAccess _client;
        public INotyfService _notifyService { get; }

        public Todo[] Todo { get; set; }
        [BindProperty]
        public Todo AddTodo { get; set; }
        public IndexModel(ILogger<IndexModel> logger, IDataAccess client, INotyfService notifyService)
        {
            _logger = logger;
            _client = client;
            _notifyService = notifyService;
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
            _notifyService.Success("This is a Success Notification");
            return RedirectToPage("Index");
        }

        public async Task<RedirectToPageResult> OnPostDelete(string id)
        {
            var response = await _client.DeleteAsync(id);

            if(response.IsSuccessStatusCode) return RedirectToPage("Index");
            _notifyService.Success("This is a Success Notification");

            return RedirectToPage("Index");
        }
    }
}

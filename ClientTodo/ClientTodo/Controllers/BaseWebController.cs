using ClientTodo.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClientTodo.Controllers
{
    public class BaseWebController : Controller
    {
        protected readonly Options _options;
        protected readonly HttpClient _httpClient;
        public BaseWebController(Options options, HttpClient httpClient) {
            _options = options;
            _httpClient = httpClient;
        }
    }
}

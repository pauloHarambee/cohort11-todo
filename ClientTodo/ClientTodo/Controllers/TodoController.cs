using ClientTodo.Models;
using ClientTodo.Models.ViewModel;
using ClientTodo.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;


namespace ClientTodo.Controllers
{
    [Authorize]
    public class TodoController : BaseWebController
    {
        public TodoController(Options options, HttpClient httpClient) : base(options, httpClient)
        {
        }
        public async Task<IActionResult> Index(string Message)
        {
            if (!string.IsNullOrWhiteSpace(Message))
                ViewBag.Message = Message;

            var apiUrl = _options.ApiUrl + "/Todo";

            var response = await  _httpClient.GetStringAsync(apiUrl);

            var todoList = JsonConvert.DeserializeObject<IEnumerable<TodoTask>>(response);

            return View(todoList);
        }
        public async Task<IActionResult> Details(int? id)
        {
            var apiUrl = _options.ApiUrl + "/Todo/" +id;
            var res = await _httpClient.GetStringAsync(apiUrl);
            var todo = JsonConvert.DeserializeObject<TodoTask>(res);

            return View(todo);
        }
        [HttpPost]
        public async Task<IActionResult> Create(TodoViewModel todoViewModel)
        {
            var apiUrl = _options.ApiUrl + "/Todo";

            //  var todoJson = JsonConvert.SerializeObject(todoViewModel);

            var res = await _httpClient.PostAsJsonAsync(apiUrl, todoViewModel);

            if(res.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index), new { Message = "New record created successful" });
            }

            ModelState.AddModelError("", "Failed to add new task!");
            PopulateDDL().Wait();
            return View(todoViewModel);

        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var apiUrl = _options.ApiUrl + "/Account/login";
            var res = await _httpClient.GetStringAsync(apiUrl);
            var user = JsonConvert.DeserializeObject<AppUser>(res);

            PopulateDDL().Wait();
            return View(new TodoViewModel { 
                AppUserId = user.Id,
            });
        }
        private async Task PopulateDDL()
        {
            var apiUrl = _options.ApiUrl + "/Type";
            var res = await _httpClient.GetStringAsync(apiUrl);
            var types = JsonConvert.DeserializeObject<IEnumerable<TaskType>>(res);

            ViewBag.TypeID = new SelectList(types.ToList(), "Id", "Name");
        }
    }
}

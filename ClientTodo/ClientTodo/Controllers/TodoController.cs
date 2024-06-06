using ClientTodo.Models;
using ClientTodo.Models.ViewModel;
using ClientTodo.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;


namespace ClientTodo.Controllers
{
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

           // var todoJson = JsonConvert.SerializeObject(todoViewModel);

            var res = await _httpClient.PostAsJsonAsync(apiUrl, todoViewModel);

            if(res.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index), new { Message = "New record created successful" });
            }

            ModelState.AddModelError("", "Failed to add new task!");
            return View(todoViewModel);

        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
    }
}

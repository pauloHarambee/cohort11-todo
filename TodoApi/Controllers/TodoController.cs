using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TodoApi.Data;
using TodoApi.Models;
using TodoApi.Models.DTOs;

namespace TodoApi.Controllers
{
   // [Authorize]
    public class TodoController : BaseController
    {
        private readonly UserManager<AppUser> userManager;
        public TodoController(AppDbContext appDbContext, UserManager<AppUser> userManager): base(appDbContext){
            this.userManager = userManager;
        }
        [HttpGet]
        /*
         * SELECT [t].[Id], [t].[AppUserId], [t].[Description], [t].[DueDate], [t].[IsComplete], [t].[TaskName], [a].[Id], [a].[AccessFailedCount], [a].[ConcurrencyStamp], [a].[Email], [a].[EmailConfirmed], [a].[LockoutEnabled], [a].[LockoutEnd], [a].[NormalizedEmail], [a].[NormalizedUserName], [a].[PasswordHash], [a].[PhoneNumber], [a].[PhoneNumberConfirmed], [a].[SecurityStamp], [a].[TwoFactorEnabled], [a].[UserName]
           FROM [Tasks] AS [t]
           INNER JOIN [AspNetUsers] AS [a] ON [t].[AppUserId] = [a].[Id]
         */
        public IActionResult Get(){
            var todos = _appDbContext.TodoTasks.Include(user=>user.AppUser)
                .Include(type=>type.Type)
                .ToList();
            if(todos.Count() == 0)
                return BadRequest("No Records found!");
            return Ok(todos);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult> Get(int? id)
        {
            if(id == null)
                return BadRequest("The 'id' value is Required");
            
            

            TodoTask todo = await _appDbContext.TodoTasks.Include(user => user.AppUser)
                .Include(s=>s.Type)
                .FirstOrDefaultAsync(s => s.Id == id);

            if(todo == null)
               return NotFound();

            return Ok(todo);
        }
        [HttpPost]
        public async Task<ActionResult> Post(TodoDTO todoTask){
            if(!ModelState.IsValid)
                return BadRequest(new {todoTask, errorMsg = "Some of your values were invalid"});

            var user = await userManager.FindByIdAsync(todoTask.AppUserId);
            if (user == null)
                return BadRequest(new { todoTask, errorMsg = $"User with Id; {todoTask.AppUserId} does not exist"});

            var type = _appDbContext.Types.Find(todoTask.TaskTypeId);

            var task = todoTask.ToTodoTask();
            task.AppUser = user;
            task.Type = type;

             await _appDbContext.TodoTasks.AddAsync(task);
             if(await _appDbContext.SaveChangesAsync() > 0)
                return CreatedAtAction(nameof(Post),  new {todoTask, message = $"Task with id {todoTask.TaskName} was created successful"});
            
            return BadRequest("Failed to created a new task");

        }
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int? id, TodoDTO todoDTO){
            if(!ModelState.IsValid)
                return BadRequest(new {todoDTO, errorMsg = "Some of your values were invalid"});

             if(id == null)
                return BadRequest("The 'id' value is Required");

            var todo = await _appDbContext.TodoTasks.FindAsync(id);

            var type = _appDbContext.Types.Find(todoDTO.TaskTypeId);


            if (todo == null)
               return BadRequest($"The provide id {id} does not match any record");
            
            todo.TaskName = todoDTO.TaskName;
            todo.Description = todoDTO.Description;
            todo.DueDate = todoDTO.DueDate;
            todo.IsComplete = todoDTO.IsComplete;
            todo.Type = type;

            _appDbContext.Update(todo);
            if(await _appDbContext.SaveChangesAsync() > 0)
              return NoContent();
            
            return BadRequest($"Failed to update record with Id {id}");

        }
        [HttpDelete("{id}")]
          public async Task<ActionResult> Delete(int? id){

            if(id == null)
                return BadRequest("The 'id' value is Required");

            var todo = await _appDbContext.TodoTasks.FindAsync(id);

             if(todo == null)
               return BadRequest($"The provide id {id} does not match any record");

             _appDbContext.TodoTasks.Remove(todo);
              if(await _appDbContext.SaveChangesAsync() > 0)
              return NoContent();
            
            return BadRequest($"Failed to delete record with Id {id}");
          }
    }
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    public class TypeController : BaseController
    {
        public TypeController(AppDbContext appDbContext) : base(appDbContext)
        {
        }
        [HttpGet]

        public IActionResult Get()
        {
            var types = _appDbContext.Types.ToList();
            if (types.Count() == 0)
                return BadRequest("No Records found!");
            return Ok(types);
        }
        [HttpGet("{id}")]
        public  ActionResult Get(int? id)
        {
            if (id == null)
                return BadRequest("The 'id' value is Required");



            TaskType type =  _appDbContext.Types.Find(id);

            if (type == null)
                return NotFound();

            return Ok(type);
        }
    }
}

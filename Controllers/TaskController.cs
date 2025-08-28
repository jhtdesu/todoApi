using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using todoApi.Data;
using todoApi.Models;
using Microsoft.EntityFrameworkCore;

namespace todoApi.Controllers
{
    [ApiController]
    [Route("api/todo")]
    public class TaskController : ControllerBase
    {
        private readonly AppDBContext _context;
        public TaskController(AppDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var todos = await _context.Todos.ToListAsync();
            return Ok(todos);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Todo todo)
        {
            await _context.Todos.AddAsync(todo);
            await _context.SaveChangesAsync();
            return Ok(todo);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] TodoDto todotask)
        {
            var todo = await _context.Todos.FindAsync(id);
            if (todo == null)
            {
                return NotFound();
            }
            todo.Title = todotask.Title;
            todo.IsDone = todotask.IsDone;
            await _context.SaveChangesAsync();
            return Ok(todo);
        }
        public class TodoDto {
            public string Title { get; set; } = string.Empty;
            public bool IsDone { get; set; } = false;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var todo = await _context.Todos.FindAsync(id);
            if (todo == null)
            {
                return NotFound();
            }
            _context.Todos.Remove(todo);
            await _context.SaveChangesAsync();
            return Ok(todo);
        }
    }
}
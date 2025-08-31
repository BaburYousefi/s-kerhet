using Ganss.Xss;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.Models;
using TodoApi.ViewModels;

namespace TodoApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]

public class TodosController(DataContext context) : ControllerBase
{
    private readonly DataContext _context = context;
    private readonly HtmlSanitizer _htmlSanitizer = new();

    [HttpPost("create")]
    public async Task<ActionResult> AddToDos(RolesPostViewModel model)
    {
        if (!ModelState.IsValid) return ValidationProblem();
        // if (!ModelState.IsValid) return BadRequest();

        // Sanera datat...
        model.FirstName = _htmlSanitizer.Sanitize(model.FirstName);
        model.LastName = _htmlSanitizer.Sanitize(model.LastName);
        model.Email = _htmlSanitizer.Sanitize(model.Email);
        model.Password = _htmlSanitizer.Sanitize(model.Password);
        model.RoleName = _htmlSanitizer.Sanitize(model.RoleName);

        // Omvalidera modellen...
        ModelState.Clear();
        TryValidateModel(model);

        if (!ModelState.IsValid) return ValidationProblem();

        // Mappa modellen till vår entity...
        var toDo = new ToDo
        {
            Title = $"{model.FirstName} {model.LastName}"
        };

        _context.ToDos.Add(toDo);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(FindToDo), new { id = toDo.Id });

    }


    [Authorize]
    [HttpGet]
    public async Task<ActionResult> ListAllToDo()
    {
        var ToDos = await _context.ToDos.ToListAsync();
        return Ok(new { success = true, data = ToDos });
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> FindToDo(string id)
    {
        var toDo = await _context.ToDos.FindAsync(id);

        if (toDo is null) return NotFound();

        return Ok(new { success = true, data = toDo });
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> RemoveProduct(string id)
    {
        var product = await _context.ToDos.FindAsync(id);

        if (product is null) return NotFound();

        _context.ToDos.Remove(product);

        await _context.SaveChangesAsync();

        return NoContent();
    }
}

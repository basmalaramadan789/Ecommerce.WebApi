using Ecommerce.Infrastructure.Data;
using Ecommerce.Web.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Web.Controllers;
[Route("api/[controller]")]
[ApiController]
public class BuggyController : BaseApiController
{
    private readonly ApplicationDbContext _context;

    public BuggyController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("notFound")]
    public IActionResult GetNotFoundRequest()
    {
        var thing = _context.Products.Find(new ApiResponse(404));
        if (thing == null)
        {
            return NotFound();

        }
        return Ok();
    }
    [HttpGet("serverError")]
    public IActionResult GetServerError()
    {
        var thing = _context.Products.Find(50);
        var thingToreturn = thing.ToString();
        return Ok();
    }

    [HttpGet("badRequest")]
    public IActionResult GetBadRequest()
    {
        return BadRequest(new ApiResponse(400));
    }

    [HttpGet("badRequest/{id}")]
    public IActionResult GetNotFoundRequest(int id)
    {
        return Ok();
    }
}

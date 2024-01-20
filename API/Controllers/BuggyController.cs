using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class BuggyController : BaseApiController
{
    private readonly DataContext _context;

    public BuggyController(DataContext context)
    {
        _context = context;
    }

    [Authorize]
    [HttpGet("auth")] // route : api/buggy/auth
    public ActionResult<string> GetSecret()
    {
        return "secret text";
    }

    [HttpGet("not-found")] // route : api/buggy/not-found
    public ActionResult<AppUser> GetNotFound()
    {
        var thing = _context.Users.Find(-1);

        if(thing == null) return NotFound();
        return thing;
    }

    [HttpGet("server-error")] // route : api/buggy/server-error
    public ActionResult<string> GetServerError()
    {
        var thing = _context.Users.Find(-1);

        var thingToReturn = thing.ToString();

        return thingToReturn;
    }

    [HttpGet("bad-request")] // route : api/buggy/bad-request
    public ActionResult<string> GetBadRequest()
    {
        return BadRequest("This was not a good request");
    }
}

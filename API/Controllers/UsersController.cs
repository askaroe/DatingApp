using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")] // /api/users
public class UsersController : ControllerBase
{
    private readonly DataContext _context;
    public UsersController(DataContext context)
    {
        _context = context;
    }

    [HttpGet]
    public ActionResult<IEnumerable<AppUser>> GetUsers(){
        var users = _context.Users.ToList();

        return Ok(users);
    }

    [HttpGet("{id}")] //api/users/2
    public ActionResult<AppUser> GetUser(int id){
        return _context.Users.Find(id);
    }
}

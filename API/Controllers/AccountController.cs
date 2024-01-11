using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class AccountController : BaseApiController
{
    private readonly DataContext _context;

    public AccountController(DataContext context)
    {
        _context = context;
    }

    [HttpPost("register")] // POST route: api/account/register
    public async Task<ActionResult<AppUser>> Register(RegisterDTO registerDTO)
    {

        if(await UserExists(registerDTO.Username)) return BadRequest("Username is already taken");

        using var hmac = new HMACSHA512();

        var user = new AppUser
        {
            UserName = registerDTO.Username.ToLower(),
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDTO.Password)),
            PasswordSalt = hmac.Key
        };

        _context.Users.Add(user);

        await _context.SaveChangesAsync();

        return user;
    }

    [HttpPost("login")] // route: api/account/register
    public async Task<ActionResult<AppUser>> Login(LoginDTO loginDTO)
    {
        var user = await _context.Users.SingleOrDefaultAsync(u => 
        u.UserName == loginDTO.Username.ToLower());

        if(user == null) return Unauthorized("Invalid username");

        using var hmac = new HMACSHA512(user.PasswordSalt);

        var computedHashOfTheLogin = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDTO.Password));

        for(int i = 0; i < computedHashOfTheLogin.Length; i++)
        {
            if(computedHashOfTheLogin[i] != user.PasswordHash[i]) return Unauthorized("Invalid password");
        }

        return user;
    }

    private async Task<bool> UserExists(string username)
    {
        return await _context.Users.AnyAsync(u => u.UserName == username.ToLower());    
    }

}

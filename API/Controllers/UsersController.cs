using System.Security.Claims;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[Authorize]
public class UsersController : BaseApiController
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UsersController(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MemberDTO>>> GetUsers()
    {
        var users = await _userRepository.GetMembersAsync();

        return Ok(users);
    }

    [HttpGet("{username}")] //api/users/jon
    public async Task<ActionResult<MemberDTO>> GetUser(string username)
    {
        var user = await _userRepository.GetUserByUsernameAsync(username);

        return await _userRepository.GetMemberAsync(username);
    }

    [HttpPut]
    public async Task<ActionResult<MemberDTO>> UpdateUser(MemberUpdateDTO memberUpdateDTO)
    {
        var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var user = await _userRepository.GetUserByUsernameAsync(username);

        if(user == null) return NotFound();

        _mapper.Map(memberUpdateDTO, user); 
        
        if(await _userRepository.SaveAllAsync()) return NoContent();

        return BadRequest("Failed to Update user");
    }

}

﻿using System.Security.Claims;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class UsersController : BaseApiController
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IPhotoService _photoService;

    public UsersController(IUserRepository userRepository, IMapper mapper, IPhotoService photoService)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _photoService = photoService;
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

    [HttpPut] // api/users
    public async Task<ActionResult<MemberDTO>> UpdateUser(MemberUpdateDTO memberUpdateDTO)
    {
        var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());

        if(user == null) return NotFound();

        _mapper.Map(memberUpdateDTO, user); 
        
        if(await _userRepository.SaveAllAsync()) return NoContent();

        return BadRequest("Failed to Update user");
    }

    [HttpPut("add-photo")]
    public async Task<IActionResult<PhotoDTO>> AddPhoto(IFormFile file)
    {
        var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());

        if (user == null) return NotFound();

        var result = await _photoService.AddPhotoAsync(file);

        if(result.Error != null) return BadRequest(result.Error.Message);
        
        var photo = new Photo
        {
            Url = result.SecureUrl.AbsoluteUri,
            PublicId = result.PublicId
        };

        if(user.Photos.Count == 0)
        {
            photo.IsMain = true;
        }

        user.Photos.Add(photo);

        if(await _userRepository.SaveAllAsync()) return _mapper.Map<PhotoDTO>(photo);

        return BadRequest("Problem adding photo"); 
    }

}

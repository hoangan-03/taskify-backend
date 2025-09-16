using MediatR;
using Microsoft.AspNetCore.Mvc;
using Taskify.Application.DTOs;
using Taskify.Application.Features.Users.Queries;

namespace Taskify.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<List<UserDto>>> GetAllUsers()
    {
        var result = await _mediator.Send(new GetAllUsersQuery());
        return Ok(result);
    }
}
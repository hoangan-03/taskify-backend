using MediatR;
using Microsoft.EntityFrameworkCore;
using Taskify.Application.DTOs;
using Taskify.Application.Interfaces;

namespace Taskify.Application.Features.Users.Queries;

public record GetAllUsersQuery : IRequest<List<UserDto>>;

public class GetAllUsersHandler : IRequestHandler<GetAllUsersQuery, List<UserDto>>
{
    private readonly IApplicationDbContext _context;

    public GetAllUsersHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<UserDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        return await _context.Users
            .Select(u => new UserDto
            {
                Id = u.Id,
                FullName = u.FullName,
                Email = u.Email,
                CreatedAt = u.CreatedAt
            })
            .ToListAsync(cancellationToken);
    }
}
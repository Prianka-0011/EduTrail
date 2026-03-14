using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace EduTrail.Application.Auths
{
    public class IsLoginQuery : IRequest<bool>
    {
        public class Handler : IRequestHandler<IsLoginQuery, bool>
        {
            private readonly IHttpContextAccessor _httpContextAccessor;

            public Handler(IHttpContextAccessor httpContextAccessor)
            {
                _httpContextAccessor = httpContextAccessor;
            }

            public Task<bool> Handle(IsLoginQuery request, CancellationToken cancellationToken)
            {
                var user = _httpContextAccessor.HttpContext?.User;

                if (user == null || !user.Identity.IsAuthenticated)
                {
                    return Task.FromResult(false);
                }

                var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
                var email = user.FindFirstValue(ClaimTypes.Email);

                return Task.FromResult(true);
            }
        }
    }
}

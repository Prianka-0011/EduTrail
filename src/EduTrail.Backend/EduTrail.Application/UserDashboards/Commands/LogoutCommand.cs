using System.Windows.Input;
using EduTrail.Application.Shared;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace EduTrail.Application.UserDashboards
{
    public class LogoutCommand : IRequest<bool>
    {
        public class Handler : IRequestHandler<LogoutCommand, bool>
        {
            private readonly ICommonService _commonService;

            public Handler(IHttpContextAccessor httpContextAccessor, ICommonService commonService)
            {
                _commonService = commonService;
            }

            public Task<bool> Handle(LogoutCommand request, CancellationToken cancellationToken)
            {
               _commonService._HttpContextAccessor.HttpContext?.Response.Cookies.Delete(_commonService._AuthTokenCookieName);
                return Task.FromResult(true);
            }
        }
    }
}

using System.Globalization;
using AutoMapper;
using EduTrail.Application.Auths;
using EduTrail.Application.LabRequests;
using EduTrail.Application.Shared;
using EduTrail.Domain.Entities;
using EduTrail.Shared;
using MediatR;

namespace EduTrail.Application.Auths
{
 public class SignInCommand : IRequest<string>
    {
        public SignDto Login { get; set; }

        public class Handler : IRequestHandler<SignInCommand, string>
        {
            private readonly IAuthRepository _repository;
            private readonly IJwtTokenGenerator _jwtTokenGenerator;

            public Handler(IAuthRepository repository, IJwtTokenGenerator jwtTokenGenerator)
            {
                _repository = repository;
                _jwtTokenGenerator = jwtTokenGenerator;
            }

            public async Task<string> Handle(SignInCommand request, CancellationToken cancellationToken)
            {
                var user = await _repository.GetUserByEmail(request.Login.Email);

                if (user == null || !user.IsActive)
                    throw new Exception("Invalid credentials");

                var isPasswordValid = PasswordHasher.VerifyPassword(
                    request.Login.Password,
                    user.PasswordHash,
                    user.PasswordSalt
                );

                if (!isPasswordValid)
                    throw new Exception("Invalid credentials");

                var token = _jwtTokenGenerator.GenerateToken(user);

                return token;
            }
        }
    }
}
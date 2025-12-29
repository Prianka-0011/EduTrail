using MediatR;
using EduTrail.Domain.Entities;
namespace EduTrail.Application.Tests
{
    public class CreateTestCommand : IRequest<string>
    {
        public TestDto testDto { get; set; } = null!;
        public class Handler : IRequestHandler<CreateTestCommand, string>
        {
            private readonly ITestRepository _repository;
            public Handler(ITestRepository repository)
            {
                _repository = repository;
            }
            public async Task<string> Handle(CreateTestCommand request, CancellationToken cancellationToken)
            {
                var test = new Test();

                test.Title = request.testDto.Title;

                test.Description = request.testDto.Description;


                var result = await _repository.Create(test);
                return result;
            }
        }
    }
}
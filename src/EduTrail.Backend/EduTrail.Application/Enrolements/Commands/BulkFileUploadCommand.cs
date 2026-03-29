

using System.Globalization;
using CsvHelper;
using EduTrail.Application.Shared.Dtos;
using EduTrail.Application.Users;
using EduTrail.Domain.Entities;
using EduTrail.Shared;
using MediatR;

namespace EduTrail.Application.Enrolements
{
    public class BulkEnrollmentUploadCommand : IRequest<bool>
    {
        public EnrollmentBulkCsvDto DetailDto { get; set; }

        public class Handler : IRequestHandler<BulkEnrollmentUploadCommand, bool>
        {
            private readonly IEnrolementRepository _repository;
            private readonly IUserRepository _userRepository;

            private readonly ITempFilesService _tempFilesService;

            public Handler(IEnrolementRepository repository, ITempFilesService tempFilesService, IUserRepository userRepository)
            {
                _repository = repository;
                _tempFilesService = tempFilesService;
                _userRepository = userRepository;
            }

            public async Task<bool> Handle(BulkEnrollmentUploadCommand request, CancellationToken cancellationToken)
            {
                if (request.DetailDto.File == null || request.DetailDto.File.Length == 0)
                    throw new Exception("No file provided");

                var tempFilePath = await _tempFilesService.SaveTempFileAsync(request.DetailDto.File);

                List<EnrollmentCsvDto> records;
                using (var reader = new StreamReader(tempFilePath))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    csv.Context.RegisterClassMap<EnrollmentCsvMap>();
                    records = csv.GetRecords<EnrollmentCsvDto>().ToList();
                }

                var enrollments = new List<Enrollment>();
                var users = (await _userRepository.GetAllAsync()).ToList();

                var role = await _userRepository.GetRolesByIdsAsync(new List<DropdownItemDto>
                    {
                        new DropdownItemDto { Id = CustomCategory.RoleType.Student, Name = "Student" }
                    });
                foreach (var row in records)
                {
                    var user = users.FirstOrDefault(u => u.Email.Equals(row.Email, StringComparison.OrdinalIgnoreCase));


                    if (user == null)
                    {
                        user = new User
                        {
                            Id = Guid.NewGuid(),
                            FirstName = row.FullName.Split(' ')[0],
                            LastName = row.FullName.Split(' ').Last(),
                            Email = row.Email,
                            SISId = row.SISId,
                            CanvasUserId = row.CanvasUserId,
                            IsActive = true,
                            CreatedDate = DateTimeOffset.UtcNow
                        };
                        await _userRepository.CreateNotSaveChangeAsync(user);
                        user.Roles ??= new List<Role>();

                        var firstRole = role.FirstOrDefault();
                        if (firstRole != null)
                        {
                            user.Roles.Add(firstRole);
                        }
                        users.Add(user);
                        user.Roles.Add(role.FirstOrDefault());
                    }

                    var existing = await _repository.GetByCourseOfferingIdAndStudentIdAsync(request.DetailDto.CourseOfferingId, user.Id);
                    if (existing != null) continue;

                    enrollments.Add(new Enrollment
                    {
                        Id = Guid.NewGuid(),
                        CourseOfferingId = request.DetailDto.CourseOfferingId,
                        UserId = user.Id,
                        EnrolledDate = DateTimeOffset.UtcNow,
                        IsActive = true
                    });
                }
                await _repository.BulkInsertAsync(enrollments);

                _tempFilesService.DeleteTempFile(tempFilePath);

                return true;
            }
        }
    }

}

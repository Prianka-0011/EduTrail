using EduTrail.Application.Shared.Dtos;

namespace EduTrail.Application.Users
{
    public class UserDto
    {
        public UserDetailDto DetailDto { get; set; } = new UserDetailDto();
        public List<UserDetailDto> DetailDtoList { get; set; } = new List<UserDetailDto>();
    }
    public class UserDetailDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
    }


}
using EduTrail.Application.Shared.Dtos;

namespace EduTrail.Application.Users
{
    public class UserDto
    {
        public UserDetailDto DetailDto { get; set; } = new UserDetailDto();
        public List<UserDetailDto> DetailDtoList { get; set; } = new List<UserDetailDto>();
        public List<DropdownItemDto> DropdownRoleList { get; set; }
    }
    public class UserDetailDto
    {
        public Guid Id { get; set; } = Guid.Empty;
        public string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string? Password { get; set; }
        public List<DropdownItemDto>? SelectedRoleList { get; set; } = new List<DropdownItemDto>();
        public bool IsActive { get; set; }
    }


}
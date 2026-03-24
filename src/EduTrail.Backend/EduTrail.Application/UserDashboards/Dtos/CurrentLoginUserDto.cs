using EduTrail.Application.Shared.Dtos;

namespace EduTrail.Application.UserDashboards
{
    public class CurrentLoginUserDto
    {
       public Guid Id { get; set; } = Guid.Empty;
        public string FullName { get; set;}
        public string Email { get; set; }
        public string? Image { get; set; }
        public List<DropdownItemDto>? Roles { get; set; } = new List<DropdownItemDto>();
    }
}
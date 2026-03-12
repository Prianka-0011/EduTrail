// using AutoMapper;
// using EduTrail.Domain.Entities;

// namespace EduTrail.Application.Auths
// {
//     public class AuthMappingProfile : Profile
//     {
//         public AuthMappingProfile()
//         {
//             CreateMap<LabRequest, HelpRequestDetailDto>()
//                 .ForMember(
//                     d => d.StudentName,
//                     o => o.MapFrom(s =>
//                         s.Student != null
//                             ? s.Student.User.FirstName + " " + s.Student.User.LastName
//                             : null
//                     )
//                 )
//                 .ForMember(
//                     d => d.AssignedTeacherName,
//                     o => o.MapFrom(s =>
//                         s.AssignedTeacher != null
//                             ? s.AssignedTeacher.User.FirstName + " " + s.AssignedTeacher.User.LastName
//                             : null
//                     )
//                 )
//                 .ForMember(
//                     d => d.CourseOfferingName,
//                     o => o.MapFrom(s =>
//                         s.CourseOffering != null
//                             ? s.CourseOffering.Course.CourseName
//                             : null
//                     )
//                 )
//                 .ForMember(
//                     d => d.StatusName,
//                     o => o.MapFrom(s =>
//                         s.Status != null
//                             ? s.Status.Name
//                             : null
//                     )
//                 );

//             CreateMap<HelpRequestDetailDto, LabRequest>();
//         }
//     }
// }

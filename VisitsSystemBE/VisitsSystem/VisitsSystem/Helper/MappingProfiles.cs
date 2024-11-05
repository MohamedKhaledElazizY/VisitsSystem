using AutoMapper;
using VisitsSystem.Dto;
using VisitsSystem.Models;

namespace VisitsSystem.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles() {

            CreateMap<VisitorsTblDto, Visitor>();
            CreateMap<Visitor, VisitorsTblDto>();
            CreateMap<VisitDto, Visit>();
            CreateMap<Visit, VisitDto>();
            CreateMap<FinishedVisitsDto, Visit>();
            CreateMap<Visit, FinishedVisitsDto>();
            CreateMap<Office, OfficeDto>();
            CreateMap<OfficeDto, Office>();
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();
           

        }
    }
}

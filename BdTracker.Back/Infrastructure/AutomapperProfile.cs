using AutoMapper;
using BdTracker.Shared.Entities;
using BdTracker.Shared.Models.Request;
using BdTracker.Shared.Models.Response;
using FluentValidation.Results;

namespace BirthdayTracker.Backend.Infrastructure
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            MapRequest();
            MapResponse();
        }

        private void MapRequest()
        {
            CreateMap<AddEmployeeRequest, AppUser>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid().ToString()))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => string.Join('_', src.Name, src.Surname)))
                .ForMember(dest => dest.NormalizedUserName, opt => opt.MapFrom(src => string.Join('_', src.Name, src.Surname).ToUpper()))
                .ForMember(dest => dest.NormalizedEmail, opt => opt.MapFrom(src => src.Email.ToUpper()));
            CreateMap<UpdateEmployeeRequest, AppUser>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid().ToString()))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => string.Join('_', src.Name, src.Surname)))
                .ForMember(dest => dest.NormalizedUserName, opt => opt.MapFrom(src => string.Join('_', src.Name, src.Surname).ToUpper()))
                .ForMember(dest => dest.NormalizedEmail, opt => opt.MapFrom(src => src.Email.ToUpper()));
            CreateMap<CompanyOwnerRequest, AppUser>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid().ToString()))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => string.Join('_', src.Name, src.Surname)))
                .ForMember(dest => dest.NormalizedUserName, opt => opt.MapFrom(src => string.Join('_', src.Name, src.Surname).ToUpper()))
                .ForMember(dest => dest.NormalizedEmail, opt => opt.MapFrom(src => src.Email.ToUpper()));

            CreateMap<RegisterOwnerRequest, AppUser>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid().ToString()))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => string.Join('_', src.Name, src.Surname)))
                .ForMember(dest => dest.NormalizedUserName, opt => opt.MapFrom(src => string.Join('_', src.Name, src.Surname).ToUpper()))
                .ForMember(dest => dest.NormalizedEmail, opt => opt.MapFrom(src => src.Email.ToUpper())); ;
        }

        private void MapResponse()
        {
            CreateMap<AppUser, EmployeeResponse>();
        }
    }
}

using System;
using AutoMapper;
using BdTracker.Shared.Constants;
using BdTracker.Shared.Entities;
using BdTracker.Shared.Models.Request;
using BdTracker.Shared.Models.Response;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;

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
            CreateMap<AddUserRequest, AppUser>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid().ToString()))
                // TODO: need to disable providing Username or delete it from Identity User 
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => Guid.NewGuid().ToString("N")))
                // TODO: need to disable providing Username or delete it from Identity User 
                .ForMember(dest => dest.NormalizedUserName, opt => opt.MapFrom(src => Guid.NewGuid().ToString("N")))
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
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => string.Join('_', src.Name.Replace(' ', '_'), src.Surname.Replace(' ', '_'))))
                .ForMember(dest => dest.NormalizedUserName, opt => opt.MapFrom(src => string.Join('_', src.Name.Replace(' ', '_'), src.Surname.Replace(' ', '_')).ToUpper()))
                .ForMember(dest => dest.NormalizedEmail, opt => opt.MapFrom(src => src.Email.ToUpper()))
                .ForMember(dest => dest.PositionName, opt => opt.MapFrom(src => AppConstants.OwnerRoleName));
        }

        private void MapResponse()
        {
            CreateMap<AppUser, UserResponse>();
            CreateMap<AppUser, LoginResponse>();

            // mapping error message into response
            CreateMap<ValidationFailure, ErrorResponse>()
                .ForMember(dest => dest.PropertyName, src => src.MapFrom(opt => opt.PropertyName))
                .ForMember(dest => dest.Message, src => src.MapFrom(opt => opt.ErrorMessage));

            // CreateMap<Error, ErrorResponse>()
            //     .ForMember(dest => dest.Message, src => src.MapFrom(opt => opt.Message));

            CreateMap<IdentityError, ErrorResponse>()
                .ForMember(dest => dest.Message, opt => opt.MapFrom(src => src.Description));
        }
    }
}

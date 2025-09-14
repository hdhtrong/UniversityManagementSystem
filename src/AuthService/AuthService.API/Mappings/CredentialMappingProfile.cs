using AuthService.API.Models;
using AuthService.Domain.Entities;
using AutoMapper;

namespace AuthService.API.Mappings
{
    public class CredentialMappingProfile : Profile
    {
        public CredentialMappingProfile()
        {
            CreateMap<CredentialDto, AppCredential>();

            CreateMap<AppCredential, CredentialDto>();
        }
    }
}

using AutoMapper;
using SmartRecruitmentMatchingPlatform.API.Models.Entities;
using SmartRecruitmentMatchingPlatform.API.Models.Entities.Employers;

namespace SmartRecruitmentMatchingPlatform.API.Mappings.Employers
{
    public class EmployerMappingProfile : Profile
    {
        public EmployerMappingProfile()
        {
            // Employer mappings will be added here
            // when Employer DTO classes are finalized.

            CreateMap<Employer, Employer>();
        }
    }
}
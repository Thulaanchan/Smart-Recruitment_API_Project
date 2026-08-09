using AutoMapper;
using SmartRecruitmentMatchingPlatform.API.Models.DTOs.JobSeekers;
using SmartRecruitmentMatchingPlatform.API.Models.Entities.JobSeekers;

namespace SmartRecruitmentMatchingPlatform.API.Mappings.JobSeekers
{
    public class JobSeekerMappingProfile : Profile
    {
        public JobSeekerMappingProfile()
        {
            CreateMap<
                JobSeekerProfile,
                JobSeekerProfileResponseDto>();

            CreateMap<
                CreateJobSeekerProfileDto,
                JobSeekerProfile>();

            CreateMap<
                UpdateJobSeekerProfileDto,
                JobSeekerProfile>();

            CreateMap<
                AddEducationDto,
                Education>();

            CreateMap<
                AddExperienceDto,
                Experience>();

            CreateMap<
                AddSkillDto,
                JobSeekerSkill>();

            CreateMap<
                CV,
                CVResponseDto>();
        }
    }
}
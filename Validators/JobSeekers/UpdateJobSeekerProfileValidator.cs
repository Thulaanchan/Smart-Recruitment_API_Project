using FluentValidation;
using SmartRecruitmentMatchingPlatform.API.Models.DTOs.JobSeekers;

namespace SmartRecruitmentMatchingPlatform.API.Validators.JobSeekers
{
    public class UpdateJobSeekerProfileValidator
        : AbstractValidator<UpdateJobSeekerProfileDto>
    {
        public UpdateJobSeekerProfileValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty()
                .WithMessage("Full name is required.")
                .MaximumLength(100);

            RuleFor(x => x.PhoneNumber)
                .MaximumLength(20);

            RuleFor(x => x.Location)
                .MaximumLength(100);

            RuleFor(x => x.Summary)
                .MaximumLength(1000);
        }
    }
}
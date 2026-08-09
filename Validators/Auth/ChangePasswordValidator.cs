using FluentValidation;
using SmartRecruitmentMatchingPlatform.Models.DTOs.Auth;

namespace SmartRecruitmentMatchingPlatform.Validators.Auth
{
    public class ChangePasswordValidator
        : AbstractValidator<ChangePasswordDto>
    {
        public ChangePasswordValidator()
        {
            RuleFor(x => x.CurrentPassword)
                .NotEmpty()
                .WithMessage("Current password is required.");

            RuleFor(x => x.NewPassword)
                .NotEmpty()
                .WithMessage("New password is required.")
                .MinimumLength(8)
                .WithMessage(
                    "New password must be at least 8 characters long.")
                .NotEqual(x => x.CurrentPassword)
                .WithMessage(
                    "New password must be different from current password.");
        }
    }
}
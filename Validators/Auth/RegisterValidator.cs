using FluentValidation;
using SmartRecruitmentMatchingPlatform.Models.DTOs.Auth;
using SmartRecruitmentMatchingPlatform.Models.Enums.Users;

namespace SmartRecruitmentMatchingPlatform.Validators.Auth
{
    public class RegisterValidator : AbstractValidator<RegisterRequestDto>
    {
        public RegisterValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty()
                .WithMessage("Full name is required.")
                .MaximumLength(100)
                .WithMessage("Full name cannot exceed 100 characters.");

            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email is required.")
                .EmailAddress()
                .WithMessage("Enter a valid email address.")
                .MaximumLength(150)
                .WithMessage("Email cannot exceed 150 characters.");

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Password is required.")
                .MinimumLength(8)
                .WithMessage("Password must be at least 8 characters long.");

            RuleFor(x => x.Role)
                .IsInEnum()
                .WithMessage("Invalid user role.")
                .Must(role =>
                    role == UserRole.JobSeeker ||
                    role == UserRole.Employer)
                .WithMessage(
                    "Only JobSeeker or Employer can register.");
        }
    }
}
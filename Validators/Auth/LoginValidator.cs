using FluentValidation;
using SmartRecruitmentMatchingPlatform.Models.DTOs.Auth;

namespace SmartRecruitmentMatchingPlatform.Validators.Auth
{
    public class LoginValidator : AbstractValidator<LoginRequestDto>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email is required.")
                .EmailAddress()
                .WithMessage("Enter a valid email address.");

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Password is required.");
        }
    }
}
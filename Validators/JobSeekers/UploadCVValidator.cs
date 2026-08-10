using FluentValidation;
using SmartRecruitmentMatchingPlatform.API.Models.DTOs.JobSeekers;

namespace SmartRecruitmentMatchingPlatform.API.Validators.JobSeekers
{
    public class UploadCVValidator
        : AbstractValidator<UploadCVDto>
    {
        private static readonly string[] AllowedExtensions =
        {
            ".pdf",
            ".doc",
            ".docx"
        };

        private const long MaximumFileSize =
            5 * 1024 * 1024;

        public UploadCVValidator()
        {
            RuleFor(x => x.File)
                .NotNull()
                .WithMessage("CV file is required.");

            RuleFor(x => x.File)
                .Must(file =>
                    file != null &&
                    file.Length > 0)
                .WithMessage(
                    "CV file cannot be empty.");

            RuleFor(x => x.File)
                .Must(file =>
                    file == null ||
                    file.Length <= MaximumFileSize)
                .WithMessage(
                    "CV file size cannot exceed 5 MB.");

            RuleFor(x => x.File)
                .Must(file =>
                {
                    if (file == null)
                        return true;

                    var extension =
                        Path.GetExtension(
                            file.FileName)
                        .ToLowerInvariant();

                    return AllowedExtensions
                        .Contains(extension);
                })
                .WithMessage(
                    "Only PDF, DOC and DOCX files are allowed.");
        }
    }
}
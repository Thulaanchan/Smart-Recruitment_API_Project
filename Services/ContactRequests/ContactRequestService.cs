using SmartRecruitmentMatchingPlatform.API.Interfaces.Repositories.ContactRequests;
using SmartRecruitmentMatchingPlatform.API.Interfaces.Repositories.Employers;
using SmartRecruitmentMatchingPlatform.API.Interfaces.Repositories.JobSeekers;
using SmartRecruitmentMatchingPlatform.API.Interfaces.Repositories.Vacancies;
using SmartRecruitmentMatchingPlatform.API.Interfaces.Services.ContactRequests;
using SmartRecruitmentMatchingPlatform.API.Services.Interfaces;
using SmartRecruitmentMatchingPlatform.API.Models.DTOs.ContactRequests;
using SmartRecruitmentMatchingPlatform.API.Models.Entities.ContactRequests;
using SmartRecruitmentMatchingPlatform.API.Models.Enums.ContactRequests;

namespace SmartRecruitmentMatchingPlatform.API.Services.ContactRequests
{
    public class ContactRequestService : IContactRequestService
    {
        private readonly IContactRequestRepository _contactRequestRepository;
        private readonly IEmployerRepository _employerRepository;
        private readonly IJobSeekerRepository _jobSeekerRepository;
        private readonly IVacancyRepository _vacancyRepository;
        private readonly INotificationService _notificationService;

        public ContactRequestService(
            IContactRequestRepository contactRequestRepository,
            IEmployerRepository employerRepository,
            IJobSeekerRepository jobSeekerRepository,
            IVacancyRepository vacancyRepository,
            INotificationService notificationService)
        {
            _contactRequestRepository = contactRequestRepository;
            _employerRepository = employerRepository;
            _jobSeekerRepository = jobSeekerRepository;
            _vacancyRepository = vacancyRepository;
            _notificationService = notificationService;
        }

        public async Task<(bool Success, string Message, ContactRequestDto? Result)> SendContactRequestAsync(
            int employerId,
            CreateContactRequestDto dto)
        {
            if (dto == null || dto.JobSeekerId <= 0)
            {
                return (false, "Invalid contact request details.", null);
            }

            var employer = await _employerRepository.GetByIdAsync(employerId);
            if (employer == null)
            {
                return (false, "Employer not found.", null);
            }

            var jobSeeker = await _jobSeekerRepository.GetByIdAsync(dto.JobSeekerId);
            if (jobSeeker == null)
            {
                return (false, "Job seeker not found.", null);
            }

            if (dto.VacancyId.HasValue && dto.VacancyId.Value > 0)
            {
                var belongs = await _vacancyRepository.BelongsToEmployerAsync(dto.VacancyId.Value, employerId);
                if (!belongs)
                {
                    return (false, "Specified vacancy does not belong to employer.", null);
                }
            }

            var entity = new ContactRequest
            {
                EmployerId = employerId,
                JobSeekerId = dto.JobSeekerId,
                VacancyId = dto.VacancyId,
                Message = dto.Message,
                Status = ContactRequestStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };

            await _contactRequestRepository.AddAsync(entity);
            await _contactRequestRepository.SaveChangesAsync();

            // Send database notification to job seeker user
            await _notificationService.CreateNotificationAsync(
                jobSeeker.UserId,
                "New Contact Request",
                $"Employer '{employer.CompanyName}' sent you a contact request."
            );

            var fullRequest = await _contactRequestRepository.GetByIdAsync(entity.ContactRequestId);
            return (true, "Contact request sent successfully.", MapToDto(fullRequest!));
        }

        public async Task<IEnumerable<ContactRequestDto>> GetEmployerRequestsAsync(int employerId)
        {
            var requests = await _contactRequestRepository.GetByEmployerIdAsync(employerId);
            return requests.Select(MapToDto);
        }

        public async Task<IEnumerable<ContactRequestDto>> GetJobSeekerRequestsAsync(int jobSeekerId)
        {
            var requests = await _contactRequestRepository.GetByJobSeekerIdAsync(jobSeekerId);
            return requests.Select(MapToDto);
        }

        public async Task<(bool Success, string Message, ContactRequestDto? Result)> RespondToContactRequestAsync(
            int contactRequestId,
            int jobSeekerId,
            ContactRequestStatus status)
        {
            if (status != ContactRequestStatus.Accepted && status != ContactRequestStatus.Declined)
            {
                return (false, "Response status must be Accepted or Declined.", null);
            }

            var request = await _contactRequestRepository.GetByIdAsync(contactRequestId);
            if (request == null)
            {
                return (false, "Contact request not found.", null);
            }

            if (request.JobSeekerId != jobSeekerId)
            {
                return (false, "Unauthorized to respond to this contact request.", null);
            }

            if (request.Status != ContactRequestStatus.Pending)
            {
                return (false, $"Contact request has already been {request.Status}.", null);
            }

            request.Status = status;
            request.RespondedAt = DateTime.UtcNow;

            await _contactRequestRepository.UpdateAsync(request);
            await _contactRequestRepository.SaveChangesAsync();

            // Notify employer
            var employer = await _employerRepository.GetByIdAsync(request.EmployerId);
            if (employer != null)
            {
                await _notificationService.CreateNotificationAsync(
                    employer.UserId,
                    "Contact Request Response",
                    $"A candidate has {status.ToString().ToLower()} your contact request."
                );
            }

            return (true, $"Contact request {status.ToString().ToLower()} successfully.", MapToDto(request));
        }

        private ContactRequestDto MapToDto(ContactRequest request)
        {
            return new ContactRequestDto
            {
                ContactRequestId = request.ContactRequestId,
                EmployerId = request.EmployerId,
                EmployerName = request.Employer?.CompanyName ?? string.Empty,
                JobSeekerId = request.JobSeekerId,
                JobSeekerName = request.JobSeeker?.Profile?.FullName ?? string.Empty,
                VacancyId = request.VacancyId,
                VacancyTitle = request.Vacancy?.Title,
                Message = request.Message,
                Status = request.Status,
                CreatedAt = request.CreatedAt,
                RespondedAt = request.RespondedAt
            };
        }
    }
}

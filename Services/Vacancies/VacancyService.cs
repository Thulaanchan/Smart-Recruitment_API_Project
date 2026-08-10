using SmartRecruitmentMatchingPlatform.API.Interfaces.Repositories.Vacancies;
using SmartRecruitmentMatchingPlatform.API.Interfaces.Services.Vacancies;

namespace SmartRecruitmentMatchingPlatform.API.Services.Vacancies
{
    public class VacancyService : IVacancyService
    {
        private readonly IVacancyRepository _vacancyRepository;

        public VacancyService(IVacancyRepository vacancyRepository)
        {
            _vacancyRepository = vacancyRepository;
        }

        public async Task<bool> CreateVacancyAsync(
            int employerId,
            object createData)
        {
            if (createData == null)
            {
                return false;
            }

            // Actual DTO mapping will be added later
            return false;
        }

        public async Task<object?> GetVacancyByIdAsync(
            int vacancyId)
        {
            if (vacancyId <= 0)
            {
                return null;
            }

            return await _vacancyRepository.GetByIdAsync(vacancyId);
        }

        public async Task<object> GetEmployerVacanciesAsync(
            int employerId)
        {
            return await _vacancyRepository
                .GetByEmployerIdAsync(employerId);
        }

        public async Task<bool> UpdateVacancyAsync(
            int vacancyId,
            int employerId,
            object updateData)
        {
            if (updateData == null)
            {
                return false;
            }

            var belongsToEmployer =
                await _vacancyRepository
                    .BelongsToEmployerAsync(
                        vacancyId,
                        employerId);

            if (!belongsToEmployer)
            {
                return false;
            }

            var vacancy =
                await _vacancyRepository
                    .GetByIdAsync(vacancyId);

            if (vacancy == null)
            {
                return false;
            }

            await _vacancyRepository.UpdateAsync(vacancy);
            await _vacancyRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> CloseVacancyAsync(
            int vacancyId,
            int employerId)
        {
            var belongsToEmployer =
                await _vacancyRepository
                    .BelongsToEmployerAsync(
                        vacancyId,
                        employerId);

            if (!belongsToEmployer)
            {
                return false;
            }

            var vacancy =
                await _vacancyRepository
                    .GetByIdAsync(vacancyId);

            if (vacancy == null)
            {
                return false;
            }

            await _vacancyRepository.UpdateAsync(vacancy);
            await _vacancyRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ReopenVacancyAsync(
            int vacancyId,
            int employerId)
        {
            var belongsToEmployer =
                await _vacancyRepository
                    .BelongsToEmployerAsync(
                        vacancyId,
                        employerId);

            if (!belongsToEmployer)
            {
                return false;
            }

            var vacancy =
                await _vacancyRepository
                    .GetByIdAsync(vacancyId);

            if (vacancy == null)
            {
                return false;
            }

            await _vacancyRepository.UpdateAsync(vacancy);
            await _vacancyRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ExistsAsync(
            int vacancyId)
        {
            return await _vacancyRepository
                .ExistsAsync(vacancyId);
        }
    }
}
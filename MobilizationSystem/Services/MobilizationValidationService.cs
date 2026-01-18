using Microsoft.EntityFrameworkCore;
using MobilizationSystem.Domain;
using MobilizationSystem.Infrastructure;

namespace MobilizationSystem.Services
{
    public class MobilizationValidationService : IMobilizationValidationService
    {
        private readonly MobilizationDbContext _context;

        private static readonly RequestStatus[] BlockingStatuses =
        {
            RequestStatus.Submitted,
            RequestStatus.Approved,
            RequestStatus.Activated
        };

        public MobilizationValidationService(MobilizationDbContext context)
        {
            _context = context;
        }

        public async Task ValidateAvailabilityAsync(
            IEnumerable<int> personIds,
            IEnumerable<int> resourceIds)
        {
            var busyPersons = await _context.MobilizationRequestPersons
                .Where(mrp => personIds.Contains(mrp.PersonId))
                .Where(mrp => BlockingStatuses.Contains(mrp.MobilizationRequest!.Status))
                .Select(mrp => mrp.PersonId)
                .Distinct()
                .ToListAsync();

            if (busyPersons.Any())
                throw new InvalidOperationException(
                    $"Persons already assigned: {string.Join(", ", busyPersons)}");

            var busyResources = await _context.MobilizationRequestResources
                .Where(mrr => resourceIds.Contains(mrr.ResourceId))
                .Where(mrr => BlockingStatuses.Contains(mrr.MobilizationRequest!.Status))
                .Select(mrr => mrr.ResourceId)
                .Distinct()
                .ToListAsync();

            if (busyResources.Any())
                throw new InvalidOperationException(
                    $"Resources already assigned: {string.Join(", ", busyResources)}");
        }
    }
}

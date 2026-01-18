namespace MobilizationSystem.Services
{
    public interface IMobilizationValidationService
    {
        Task ValidateAvailabilityAsync(
            IEnumerable<int> personIds,
            IEnumerable<int> resourceIds);
    }
}

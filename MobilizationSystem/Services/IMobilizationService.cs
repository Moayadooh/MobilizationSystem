namespace MobilizationSystem.Services
{
    public interface IMobilizationService
    {
        Task ApproveAsync(int requestId, string approvedBy, bool isFinalApproval);
        Task CompleteAsync(int requestId);
        Task CancelAsync(int requestId, string cancelledBy);
    }
}

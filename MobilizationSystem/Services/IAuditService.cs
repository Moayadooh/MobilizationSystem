namespace MobilizationSystem.Services
{
    public interface IAuditService
    {
        Task LogAsync(string action, string table, int? recordId);
    }
}

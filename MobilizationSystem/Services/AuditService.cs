using MobilizationSystem.Domain;
using MobilizationSystem.Infrastructure;

namespace MobilizationSystem.Services
{
    public class AuditService : IAuditService
    {
        private readonly MobilizationDbContext _context;
        private readonly IHttpContextAccessor _http;

        public AuditService(
            MobilizationDbContext context,
            IHttpContextAccessor http)
        {
            _context = context;
            _http = http;
        }

        public async Task LogAsync(string action, string table, int? recordId)
        {
            var user = _http.HttpContext?.User?.Identity?.Name ?? "system";

            _context.AuditLogs.Add(new AuditLog
            {
                UserName = user,
                Action = action,
                TableName = table,
                RecordId = recordId,
                CreatedAt = DateTime.UtcNow
            });

            await _context.SaveChangesAsync();
        }
    }
}

namespace MobilizationSystem.Domain
{
    public class AuditLog
    {
        public int Id { get; set; }
        public string UserName { get; set; } = null!;
        public string Action { get; set; } = null!;
        public string TableName { get; set; } = null!;
        public int? RecordId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}

namespace MobilizationSystem.Domain
{
    public class Approval
    {
        public int Id { get; set; }
        public int MobilizationRequestId { get; set; }
        public string ApprovedBy { get; set; } = null!;
        public DateTime ApprovalDate { get; set; }
        public bool IsApproved { get; set; }

        // Navigation
        public MobilizationRequest? MobilizationRequest { get; set; }
    }
}

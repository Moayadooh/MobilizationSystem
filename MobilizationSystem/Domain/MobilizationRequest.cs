namespace MobilizationSystem.Domain
{
    public class MobilizationRequest
    {
        public int Id { get; set; }
        public DateTime RequestDate { get; set; }
        public string Purpose { get; set; } = null!;
        public RequestStatus Status { get; set; }

        public ICollection<MobilizationRequestPerson> MobilizationRequestPersons { get; set; } = new List<MobilizationRequestPerson>();
        public ICollection<MobilizationRequestResource> MobilizationRequestResources { get; set; } = new List<MobilizationRequestResource>();
        public ICollection<Approval> Approvals { get; set; } = new List<Approval>();
    }
}

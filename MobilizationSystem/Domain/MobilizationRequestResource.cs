namespace MobilizationSystem.Domain
{
    public class MobilizationRequestResource
    {
        public int Id { get; set; }
        public int MobilizationRequestId { get; set; }
        public int ResourceId { get; set; }

        public MobilizationRequest? MobilizationRequest { get; set; }
        public Resource? Resource { get; set; }
    }
}

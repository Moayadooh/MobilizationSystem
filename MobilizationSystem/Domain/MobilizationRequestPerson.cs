namespace MobilizationSystem.Domain
{
    public class MobilizationRequestPerson
    {
        public int Id { get; set; }
        public int MobilizationRequestId { get; set; }
        public int PersonId { get; set; }

        public MobilizationRequest? MobilizationRequest { get; set; }
        public Person? Person { get; set; }
    }
}

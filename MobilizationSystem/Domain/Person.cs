namespace MobilizationSystem.Domain
{
    public class Person
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public string NationalId { get; set; } = null!;
        public bool IsAvailable { get; set; }

        public ICollection<MobilizationRequestPerson> MobilizationRequestPersons { get; set; } = new List<MobilizationRequestPerson>();
    }
}

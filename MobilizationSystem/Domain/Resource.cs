namespace MobilizationSystem.Domain
{
    public class Resource
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Type { get; set; } = null!;
        public bool IsAvailable { get; set; }

        public ICollection<MobilizationRequestResource> MobilizationRequestResources { get; set; } = new List<MobilizationRequestResource>();
    }
}

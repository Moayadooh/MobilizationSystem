namespace MobilizationSystem.Models
{
    public class DashboardViewModel
    {
        // Requests
        public int TotalRequests { get; set; }
        public int PendingRequests { get; set; }
        public int ActiveRequests { get; set; }
        public int CompletedRequests { get; set; }
        public int CancelledRequests { get; set; }

        // Persons
        public int TotalPersons { get; set; }
        public int AvailablePersons { get; set; }
        public int AssignedPersons { get; set; }

        // Resources
        public int TotalResources { get; set; }
        public int AvailableResources { get; set; }
        public int AssignedResources { get; set; }
    }
}

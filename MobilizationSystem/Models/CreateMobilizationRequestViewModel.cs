using Microsoft.AspNetCore.Mvc.Rendering;

namespace MobilizationSystem.Models
{
    public class CreateMobilizationRequestViewModel
    {
        public string Purpose { get; set; } = null!;
        public List<int> SelectedPersonIds { get; set; } = new();
        public List<int> SelectedResourceIds { get; set; } = new();

        // For dropdowns
        public IEnumerable<SelectListItem> Persons { get; set; } = Enumerable.Empty<SelectListItem>();
        public IEnumerable<SelectListItem> Resources { get; set; } = Enumerable.Empty<SelectListItem>();
    }
}

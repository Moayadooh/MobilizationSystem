using Microsoft.AspNetCore.Mvc;
using MobilizationSystem.Domain;
using MobilizationSystem.Infrastructure;

namespace MobilizationSystem.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class ResourcesController : Controller
    {
        private readonly MobilizationDbContext _context;

        public ResourcesController(MobilizationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
            => View(_context.Resources.ToList());

        public IActionResult Create()
            => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Resource model)
        {
            if (!ModelState.IsValid)
                return View(model);

            model.IsAvailable = true;
            _context.Resources.Add(model);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var resource = _context.Resources.Find(id);
            if (resource == null) return NotFound();
            return View(resource);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Resource model)
        {
            if (!ModelState.IsValid)
                return View(model);

            _context.Resources.Update(model);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}

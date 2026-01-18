using Microsoft.AspNetCore.Mvc;
using MobilizationSystem.Domain;
using MobilizationSystem.Infrastructure;

namespace MobilizationSystem.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class PersonsController : Controller
    {
        private readonly MobilizationDbContext _context;

        public PersonsController(MobilizationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
            => View(_context.Persons.ToList());

        public IActionResult Create()
            => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Person model)
        {
            if (!ModelState.IsValid)
                return View(model);

            model.IsAvailable = true;
            _context.Persons.Add(model);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var person = _context.Persons.Find(id);
            if (person == null) return NotFound();
            return View(person);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Person model)
        {
            if (!ModelState.IsValid)
                return View(model);

            _context.Persons.Update(model);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}

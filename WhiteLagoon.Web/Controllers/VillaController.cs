using Microsoft.AspNetCore.Mvc;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;

namespace WhiteLagoon.Web.Controllers
{
    public class VillaController : Controller
    {
        private readonly ApplicationDbContext _db;
        public VillaController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            var villas = _db.Villas.ToList();
            return View(villas);
        }

        public IActionResult Create(int id)
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Villa villa)
        {
            if (ModelState.IsValid)
            {
                _db.Villas.Add(villa);
                _db.SaveChanges();
                TempData["success"] = "Villa created successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(villa);
        }

        public IActionResult Update(int id)
        {
            Villa? villa = _db.Villas.Find(id);
            if (villa == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(villa);
        }

        [HttpPost]
        public IActionResult Update(Villa villa)
        {
            if (ModelState.IsValid)
            {
                _db.Villas.Update(villa);
                _db.SaveChanges();
                TempData["success"] = "Villa updated successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(villa);
        }

        public IActionResult Delete(int id)
        {
            Villa? villa = _db.Villas.Find(id);
            if (villa == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(villa);
        }

        [HttpPost]
        public IActionResult Delete(Villa villa)
        {
            Villa? villaInDb = _db.Villas.Find(villa.Id);
            if (villaInDb is not null)
            {
                _db.Villas.Remove(villaInDb);
                _db.SaveChanges();
                TempData["success"] = "Villa deleted successfully";
            }
            TempData["error"] = "Villa could not be deleted";

            return RedirectToAction(nameof(Index));
        }

    }
}

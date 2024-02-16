using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Infrastructure.Data;
using WhiteLagoon.Web.ViewModels;

namespace WhiteLagoon.Web.Controllers
{
    public class VillaNumberController : Controller
    {
        private readonly ApplicationDbContext _db;
        public VillaNumberController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            var villaNumbers = _db.VillaNumbers.Include(u => u.Villa).ToList();
            return View(villaNumbers);
        }

        public IActionResult Create(int id)
        {
            VillaNumberViewModel viewModel = new VillaNumberViewModel
            {
                VillaNumber = new VillaNumber(),
                VillaList = _db.Villas.Select(v => new SelectListItem
                {
                    Text = v.Name,
                    Value = v.Id.ToString()
                })
            };  
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Create(VillaNumberViewModel villaNumberVM)
        {
            bool villaNumberExists = _db.VillaNumbers.Any(vn => vn.Villa_Number == villaNumberVM.VillaNumber.Villa_Number);
            if (ModelState.IsValid && !villaNumberExists)
            { 
                _db.VillaNumbers.Add(villaNumberVM.VillaNumber);
                _db.SaveChanges();
                TempData["success"] = "Villa number created successfully";
                return RedirectToAction(nameof(Index));
            }
            TempData["error"] = "Villa number could not be created";
            villaNumberVM.VillaList = _db.Villas.Select(v => new SelectListItem
            {
                Text = v.Name,
                Value = v.Id.ToString()
            });
            return View(villaNumberVM);
        }

        public IActionResult Update(int id)
        {
            VillaNumberViewModel viewModel = new VillaNumberViewModel
            {
                VillaNumber = new VillaNumber(),
                VillaList = _db.Villas.Select(v => new SelectListItem
                {
                    Text = v.Name,
                    Value = v.Id.ToString()
                })
            };
            VillaNumber? villaNumber = _db.VillaNumbers.FirstOrDefault(vn => vn.Villa_Number == id);
            if (villaNumber is not null)
            {
                viewModel.VillaNumber = villaNumber;
                return View(viewModel);
            }
            return RedirectToAction("Error", "Home");
        }

        [HttpPost]
        public IActionResult Update(VillaNumberViewModel villaNumberVM)
        {
            if (ModelState.IsValid)
            {
                _db.VillaNumbers.Update(villaNumberVM.VillaNumber);
                _db.SaveChanges();
                TempData["success"] = "Villa Number updated successfully";
                return RedirectToAction(nameof(Index));
            }
            TempData["error"] = "Villa number could not be updated";
            villaNumberVM.VillaList = _db.Villas.Select(v => new SelectListItem
            {
                Text = v.Name,
                Value = v.Id.ToString()
            });
            return View(villaNumberVM);
        }

        public IActionResult Delete(int id)
        {
            VillaNumberViewModel viewModel = new VillaNumberViewModel
            {
                VillaNumber = new VillaNumber(),
                VillaList = _db.Villas.Select(v => new SelectListItem
                {
                    Text = v.Name,
                    Value = v.Id.ToString()
                })
            };
            VillaNumber? villaNumber = _db.VillaNumbers.FirstOrDefault(vn => vn.Villa_Number == id);
            if (villaNumber is not null)
            {
                viewModel.VillaNumber = villaNumber;
                return View(viewModel);
            }
            return RedirectToAction("Error", "Home");
        }

        [HttpPost]
        public IActionResult Delete(VillaNumberViewModel villaNumberVM)
        {
            VillaNumber? villaNumberInDb = _db.VillaNumbers.FirstOrDefault(vn => vn.Villa_Number == villaNumberVM.VillaNumber.Villa_Number);
            if (villaNumberInDb is not null)
            {
                _db.VillaNumbers.Remove(villaNumberInDb);
                _db.SaveChanges();
                TempData["success"] = "Villa number deleted successfully";
                return RedirectToAction(nameof(Index));
            }
            TempData["error"] = "Villa number could not be deleted";
            villaNumberVM.VillaList = _db.Villas.Select(v => new SelectListItem
            {
                Text = v.Name,
                Value = v.Id.ToString()
            });
            return View(villaNumberVM);
        }

    }
}

using Hospital.DbContexts;
using Hospital.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hospital.Controllers
{
    public class PatientsController : Controller
    {
        private readonly ApplicationDbContexts _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public PatientsController(ApplicationDbContexts context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public IActionResult GetIndexView(string? search)
        {

            IQueryable<Patient> queryablepatients = _context.Patients.AsQueryable();
            if (string.IsNullOrEmpty(search) == false)
            {
                queryablepatients = queryablepatients.Where(P => P.FullName.Contains(search));
            }

            return View("Index", queryablepatients.ToList());
        }

        public IActionResult GetDetailsView(int id)
        {
            Patient patient = _context.Patients.Include(p => p.Doctor).FirstOrDefault(p => p.Id == id);
            if (patient == null)
            {
                return NotFound();
            }
            else
            {
                return View("Details", patient);
            }
        }

        [HttpGet]
        public IActionResult GetcreateView()
        {
            ViewBag.AllDoctors = _context.Doctors.ToList();
            return View("Create");
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult AddNew(Patient patient)
        {
            
            if (ModelState.IsValid == true)
            {
                _context.Patients.Add(patient);
                _context.SaveChanges();
                return RedirectToAction("GetIndexView");
            }
            else
            {
                ViewBag.AllDoctors = _context.Doctors.ToList();
                return View("Create");
            }
        }

        [HttpGet]
        public IActionResult GetEditView(int id)
        {
            Patient patient = _context.Patients.Find(id);
            if (patient == null)
            {
                return NotFound();
            }
            else
            {
                ViewBag.AllDoctors = _context.Doctors.ToList();
                return View("Edit", patient);
            }
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult EditCurrent(Patient patient)
        {
            if (ModelState.IsValid == true)
            {
                _context.Patients.Update(patient);
                _context.SaveChanges();
                return RedirectToAction("GetIndexView");
            }
            else
            {
                return View("Edit");
            }
        }

        [HttpGet]
        public IActionResult GetDeleteView(int id)
        {
            Patient patient = _context.Patients.Include(p => p.Doctor).FirstOrDefault(p => p.Id == id);
            if (patient == null)
            {
                return NotFound();
            }
            else
            {
                return View("Delete", patient);
            }
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult DeleteCurrent(int id)
        {
            Patient patient = _context.Patients.Find(id);
            if (patient == null)
            {
                return NotFound();
            }
            else
            {
                _context.Remove(patient);
                _context.SaveChanges();
                return RedirectToAction("GetIndexView");
            }
        }
    }
}

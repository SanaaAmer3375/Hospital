using Hospital.DbContexts;
using Hospital.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Numerics;

namespace Hospital.Controllers
{
    public class DoctorsController : Controller
    {
        private readonly ApplicationDbContexts _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public DoctorsController(ApplicationDbContexts context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public IActionResult GetIndexView(string? search)
        {

            IQueryable<Doctor> queryablePatients = _context.Doctors.AsQueryable();
            if (string.IsNullOrEmpty(search) == false)
            {
                queryablePatients = queryablePatients.Where(emp => emp.FullName.Contains(search));
            }

            return View("Index", queryablePatients.ToList());
        }

        public IActionResult GetDetailsView(int id)
        {
            Doctor doctor = _context.Doctors.Include(p => p.Patients).FirstOrDefault(p => p.Id == id);
            if (doctor == null)
            {
                return NotFound();
            }
            else
            {
                return View("Details", doctor);
            }
        }

        [HttpGet]
        public IActionResult GetcreateView()
        {
            ViewBag.AllPatients = _context.Patients.ToList();
            return View("Create");
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult AddNew(Doctor doctor)
        {
            if (ModelState.IsValid == true)
            {
                if (doctor.ImageFile == null)
                {
                    doctor.ImagePath = "\\images\\No_Image.png";
                }
                else
                {
                    Guid imageGuid = Guid.NewGuid();
                    string imageExtention = Path.GetExtension(doctor.ImageFile.FileName);
                    doctor.ImagePath= "\\images\\" + imageGuid + imageExtention;
                    string imageUploadPath = _webHostEnvironment.WebRootPath + doctor.ImagePath;

                    FileStream imageStream = new FileStream(imageUploadPath, FileMode.Create);
                    doctor.ImageFile.CopyTo(imageStream);
                    imageStream.Dispose();
                }


                _context.Doctors.Add(doctor);
                _context.SaveChanges();
                return RedirectToAction("GetIndexView");
            }
            else
            {
                ViewBag.AllPatients = _context.Patients.ToList();
                return View("Create");
            }
        }

        [HttpGet]
        public IActionResult GetEditView(int id)
        {
            Doctor doctor = _context.Doctors.Find(id);
            if (doctor == null)
            {
                return NotFound();
            }
            else
            {
                ViewBag.AllDepartments = _context.Patients.ToList();
                return View("Edit", doctor);
            }
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult EditCurrent(Doctor doctor)
        {
            if (ModelState.IsValid == true)
            {

                if (doctor.ImageFile != null)
                {
                    if (doctor.ImagePath != "\\images\\No_Image.png")
                    {
                        System.IO.File.Delete(_webHostEnvironment.WebRootPath + doctor.ImagePath);
                    }

                    Guid imageGuid = Guid.NewGuid();
                    string imageExtention = Path.GetExtension(doctor.ImageFile.FileName);
                    doctor.ImagePath= "\\images\\" + imageGuid + imageExtention;
                    string imageUploadPath = _webHostEnvironment.WebRootPath + doctor.ImagePath;

                    FileStream imageStream = new FileStream(imageUploadPath, FileMode.Create);
                    doctor.ImageFile.CopyTo(imageStream);
                    imageStream.Dispose();
                }

                _context.Doctors.Update(doctor);
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
            Doctor doctor = _context.Doctors.Include(p => p.Patients).FirstOrDefault(p => p.Id == id);
            if (doctor == null)
            {
                return NotFound();
            }
            else
            {
                return View("Delete", doctor);
            }
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult DeleteCurrent(int id)
        {
            Doctor doctor = _context.Doctors.Find(id);
            if (doctor == null)
            {
                return NotFound();
            }
            else
            {
                if (doctor.ImagePath != "\\images\\No_Image.png")
                {
                    System.IO.File.Delete(_webHostEnvironment.WebRootPath + doctor.ImagePath);
                }
                _context.Remove(doctor);
                _context.SaveChanges();
                return RedirectToAction("GetIndexView");
            }
        }
    }
}


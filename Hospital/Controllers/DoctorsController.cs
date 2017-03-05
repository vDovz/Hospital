using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Hospital.Models;
using System.IO;
using System.Web;
using System.Data.Entity.Validation;

namespace Hospital.Controllers
{
    [Authorize(Roles = "Admin, Doctors")]
    public class DoctorsController : Controller
    {
        private HospitalContext db = new HospitalContext();

        // GET: Doctors
        [HttpGet]
        public ActionResult Index()
        {
            List<Doctor> dc = new List<Doctor>();
            if (TempData["doctors"] == null)
            {
                dc = db.Doctors.ToList();
            }
            else
            {
                dc = (List<Doctor>)TempData["doctors"];
            }
            return View(dc);
        }

        [HttpPost]
        public ActionResult Index(string Name)
        {
            var res = db.Doctors.Where((d) => d.Name == Name).ToList();
            TempData.Add("doctors", res);
            return RedirectToAction("Index");
        }

        // GET: Doctors/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Doctor doctor = db.Doctors.Find(id);
            if (doctor == null)
            {
                return HttpNotFound();
            }
            if (doctor.Photo == null)
            {
                doctor.Photo = new byte[100];
            }
            return View(new DoctorPatients() { Doctor = doctor, Patients = db.Patients.Where((d) => !doctor.Patients.Contains(d)).ToList() });
        }

        public ActionResult Load(int? id, HttpPostedFileBase photo)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Doctor doctor = db.Doctors.Find(id);
            if (doctor == null)
            {
                return HttpNotFound();
            }
            using (var binaryReader = new BinaryReader(photo.InputStream))
            {
                doctor.Photo = binaryReader.ReadBytes(photo.ContentLength);
            }
            if(doctor.Photo.Length > 4000)
            {
                return RedirectToAction("Details", new { id = id });
            }
            db.SaveChanges();
            return RedirectToAction("Details", new { id = id });
        }

        // GET: Doctors/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Doctors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Specialization")] Doctor doctor)
        {
            if (ModelState.IsValid)
            {
                db.Doctors.Add(doctor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(doctor);
        }

        // GET: Doctors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Doctor doctor = db.Doctors.Find(id);
            if (doctor == null)
            {
                return HttpNotFound();
            }
            return View(doctor);
        }

        // POST: Doctors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Specialization")] Doctor doctor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(doctor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(doctor);
        }

        // GET: Doctors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Doctor doctor = db.Doctors.Find(id);
            if (doctor == null)
            {
                return HttpNotFound();
            }
            db.Doctors.Remove(doctor);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [ActionName("Add")]
        public ActionResult AddPatient(int patientid, int doctorid)
        {
            Doctor doctor = db.Doctors.Find(doctorid);
            Patient patient = db.Patients.Find(patientid);
            doctor.Patients.Add(patient);
            patient.Doctors.Add(doctor);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


    }
}

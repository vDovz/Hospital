using Hospital.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Hospital.Controllers
{
    [Authorize(Roles = "Admin, Doctors, Patients")]
    public class PatientsController : Controller
    {
        // GET: Patient
        private HospitalContext db = new HospitalContext();

        // GET: Patients
        [HttpGet]
        public ActionResult Index()
        {
            List<Patient> pt = new List<Patient>();
            if (TempData["patients"] == null)
            {
                pt = db.Patients.ToList();
            }
            else
            {
                pt = (List<Patient>)TempData["patients"];
            }
            return View(pt);
        }

        public ActionResult Index(string name)
        {
            var res = db.Patients.Where((d) => d.Name == name).ToList();
            TempData.Add("patients", res);
            return RedirectToAction("Index");
        }


        public ActionResult Load(int? id, HttpPostedFileBase photo)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient patient = db.Patients.Find(id);
            if (patient == null)
            {
                return HttpNotFound();
            }
            using (var binaryReader = new BinaryReader(photo.InputStream))
            {
                patient.Photo = binaryReader.ReadBytes(photo.ContentLength);
            }
            if(patient.Photo.Length > 4000)
            {
                return RedirectToAction("Details", new { id = id });
            }
            db.SaveChanges();
            return RedirectToAction("Details", new { id = id });
        }

        // GET: Patients/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient patient = db.Patients.Find(id);
            if (patient == null)
            {
                return HttpNotFound();
            }
            return View(new PatientDoctors() { Patient = patient, Doctors = db.Doctors.ToList() });
        }

        // GET: Patients/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Patients/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,DayOfBirth,Status,TaxCode")] Patient patient)
        {
            if (ModelState.IsValid)
            {
                db.Patients.Add(patient);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(patient);
        }

        // GET: Patients/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient patient = db.Patients.Find(id);
            if (patient == null)
            {
                return HttpNotFound();
            }
            return View(patient);
        }

        // POST: Patients/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,DayOfBirth,Status,TaxCode")] Patient patient)
        {
            if (ModelState.IsValid)
            {
                db.Entry(patient).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(patient);
        }

        // GET: Patients/Delete/5
        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient patient = db.Patients.Find(id);
            if (patient == null)
            {
                return HttpNotFound();
            }
            db.Patients.Remove(patient);
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
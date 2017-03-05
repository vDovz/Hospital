using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Hospital.Models;

namespace Hospital.Controllers
{
    [Authorize]
    public class HistoriesController : Controller
    {
        private HospitalContext db = new HospitalContext();

        // GET: Histories
        public ActionResult Index(int id)
        {
            Patient patient = db.Patients.Find(id); 
            return View(patient);
        }
        // GET: Histories/Create
        [Authorize(Roles ="Doctors, Admin")]
        public ActionResult Create(int id)
        {
            ViewBag.Id = id;
            return View();
        }

        // POST: Histories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Doctors, Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Description")] History history, int id)
        {
            if (ModelState.IsValid)
            {
                db.Histories.Add(history);
                Patient patient =  db.Patients.Find(id);
                patient.History.Add(history);
                history.Patient = db.Patients.Find(id);
                db.SaveChanges();
                return RedirectToAction("Index", new { id = id});
            }

            return View(history);
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

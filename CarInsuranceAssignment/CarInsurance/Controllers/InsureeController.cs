﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CarInsurance.Models;

namespace CarInsurance.Controllers
{
    public class InsureeController : Controller
    {
        private InsuranceEntities db = new InsuranceEntities();

        // GET: Insuree
        public ActionResult Index()
        {
            return View(db.Insurees.ToList());
        }

        // GET: Insuree/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Insuree insuree = db.Insurees.Find(id);
            if (insuree == null)
            {
                return HttpNotFound();
            }
            return View(insuree);
        }

        // GET: Insuree/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Insuree/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FirstName,LastName,EmailAddress,DateOfBirth,CarYear,CarMake,CarModel,DUI,SpeedingTickets,CoverageType,Quote")] Insuree insuree)
        {
            if (ModelState.IsValid)
            {
                db.Insurees.Add(insuree);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(insuree);
        }

        // GET: Insuree/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Insuree insuree = db.Insurees.Find(id);
            if (insuree == null)
            {
                return HttpNotFound();
            }
            return View(insuree);
        }

        // POST: Insuree/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,EmailAddress,DateOfBirth,CarYear,CarMake,CarModel,DUI,SpeedingTickets,CoverageType,Quote")] Insuree insuree)
        {
            if (ModelState.IsValid)
            {
                db.Entry(insuree).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(insuree);
        }

        // GET: Insuree/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Insuree insuree = db.Insurees.Find(id);
            if (insuree == null)
            {
                return HttpNotFound();
            }
            return View(insuree);
        }

        // POST: Insuree/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Insuree insuree = db.Insurees.Find(id);
            db.Insurees.Remove(insuree);
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



        // POST: Insuree/CalculateQuote
        [HttpPost]
        public ActionResult CalculateQuote(FormCollection form)
        {
            // Extract data from form fields
            int age = Convert.ToInt32(form["Age"]);
            int carYear = Convert.ToInt32(form["CarYear"]);
            string carMake = form["CarMake"];
            string carModel = form["CarModel"];
            int speedingTickets = Convert.ToInt32(form["SpeedingTickets"]);
            bool hasDUI = Convert.ToBoolean(form["HasDUI"]);
            bool fullCoverage = Convert.ToBoolean(form["FullCoverage"]);

            // Base monthly total
            decimal monthlyTotal = 50;

            // Age calculations
            if (age <= 18)
                monthlyTotal += 100;
            else if (age >= 19 && age <= 25)
                monthlyTotal += 50;
            else
                monthlyTotal += 25;

            // Car year calculations
            if (carYear < 2000)
                monthlyTotal += 25;
            else if (carYear > 2015)
                monthlyTotal += 25;

            // Car make and model calculations
            if (carMake == "Porsche")
            {
                monthlyTotal += 25;

                if (carModel == "911 Carrera")
                    monthlyTotal += 25;
            }

            // Speeding tickets calculation
            monthlyTotal += speedingTickets * 10;

            // DUI calculation
            if (hasDUI)
                monthlyTotal *= 1.25m; // 25% increase

            // Full coverage calculation
            if (fullCoverage)
                monthlyTotal *= 1.5m; // 50% increase

            ViewBag.MonthlyTotal = monthlyTotal;

            return View("QuoteResult");
            }

        // GET: Insuree/Admin
        public ActionResult Admin()
        {
            // Retrieve all Insuree records with their quotes
            var insureesWithQuotes = db.Insurees.ToList();

            return View(insureesWithQuotes);
        }

    }

    
}


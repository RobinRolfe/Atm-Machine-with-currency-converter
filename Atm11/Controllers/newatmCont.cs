using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Atm11;

namespace Atm11.Controllers
{
    public class newatmCont : Controller
    {
        private BankdbEntities db = new BankdbEntities();

        // GET: newatmCont
        public ActionResult Index()
        {
            var atmAccounts = db.AtmAccounts.Include(a => a.AccType);
            return View(atmAccounts.ToList());
        }

        // GET: newatmCont
        public ActionResult getViewAccViewModel()
        {
            var atmAccounts = db.AtmAccounts.Include(a => a.AccType);
            return View(atmAccounts.ToList());
        }

        // GET: newatmCont/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AtmAccount atmAccount = db.AtmAccounts.Find(id);
            if (atmAccount == null)
            {
                return HttpNotFound();
            }
            return View(atmAccount);
        }

        // GET: newatmCont/Create
        public ActionResult Create()
        {
            ViewBag.AccTypeId = new SelectList(db.AccTypes, "acctypeId", "AccountType");
            return View();
        }

        // POST: newatmCont/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "atmaccId,AccountNumber,AccountBalance,AccTypeId,AccUserId")] AtmAccount atmAccount)
        {
            if (ModelState.IsValid)
            {
                db.AtmAccounts.Add(atmAccount);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AccTypeId = new SelectList(db.AccTypes, "acctypeId", "AccountType", atmAccount.AccTypeId);
            return View(atmAccount);
        }

        // GET: newatmCont/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AtmAccount atmAccount = db.AtmAccounts.Find(id);
            if (atmAccount == null)
            {
                return HttpNotFound();
            }
            ViewBag.AccTypeId = new SelectList(db.AccTypes, "acctypeId", "AccountType", atmAccount.AccTypeId);
            return View(atmAccount);
        }

        // POST: newatmCont/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "atmaccId,AccountNumber,AccountBalance,AccTypeId,AccUserId")] AtmAccount atmAccount)
        {
            if (ModelState.IsValid)
            {
                db.Entry(atmAccount).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AccTypeId = new SelectList(db.AccTypes, "acctypeId", "AccountType", atmAccount.AccTypeId);
            return View(atmAccount);
        }

        // GET: newatmCont/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AtmAccount atmAccount = db.AtmAccounts.Find(id);
            if (atmAccount == null)
            {
                return HttpNotFound();
            }
            return View(atmAccount);
        }

        // POST: newatmCont/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AtmAccount atmAccount = db.AtmAccounts.Find(id);
            db.AtmAccounts.Remove(atmAccount);
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

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using JellyBeanTracker.Web.Data;
using JellyBeanTracker.Web.Models;
using JellyBeanTracker.Shared.Calculators;

namespace JellyBeanTracker.Web.Controllers
{
    public class JellyBeansController : Controller
    {
        private JellyBeanDataContext db = new JellyBeanDataContext();

        // GET: JellyBeans
        public ActionResult Index()
        {
            return View(db.JellyBeanValues.ToList());
        }

        // GET: JellyBeans/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JellyBeanValue jellyBeanValue = db.JellyBeanValues.Find(id);
            if (jellyBeanValue == null)
            {
                return HttpNotFound();
            }
            return View(jellyBeanValue);
        }

        public ActionResult JellyBeanGraph()
        {
            return View();
        }

        public ActionResult JellyBeanGraphData()
        {
            var calculator = new JellyBeanGraphCalculator(new DataContentDataSource(this.db));

            return Json(calculator.GetGraphData(), JsonRequestBehavior.AllowGet);
        }

        // GET: JellyBeans/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: JellyBeans/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Jan,Feb,Mar,Apr,May,Jun,Jul,Aug,Sep,Oct,Nov,Dec")] JellyBeanValue jellyBeanValue)
        {
            if (ModelState.IsValid)
            {
                jellyBeanValue.Id = Guid.NewGuid();
                db.JellyBeanValues.Add(jellyBeanValue);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(jellyBeanValue);
        }

        // GET: JellyBeans/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JellyBeanValue jellyBeanValue = db.JellyBeanValues.Find(id);
            if (jellyBeanValue == null)
            {
                return HttpNotFound();
            }
            return View(jellyBeanValue);
        }

        // POST: JellyBeans/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Jan,Feb,Mar,Apr,May,Jun,Jul,Aug,Sep,Oct,Nov,Dec")] JellyBeanValue jellyBeanValue)
        {
            if (ModelState.IsValid)
            {
                db.Entry(jellyBeanValue).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(jellyBeanValue);
        }

        // GET: JellyBeans/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JellyBeanValue jellyBeanValue = db.JellyBeanValues.Find(id);
            if (jellyBeanValue == null)
            {
                return HttpNotFound();
            }
            return View(jellyBeanValue);
        }

        // POST: JellyBeans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            JellyBeanValue jellyBeanValue = db.JellyBeanValues.Find(id);
            db.JellyBeanValues.Remove(jellyBeanValue);
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

		public ActionResult GetAllData()
		{
			return Json (new SyncContainer {
				JellyBeanValues = db.JellyBeanValues.ToList(),
				MyJellyBeans = db.MyJellyBeans.ToList()
			}, JsonRequestBehavior.AllowGet);
		}
    }
}

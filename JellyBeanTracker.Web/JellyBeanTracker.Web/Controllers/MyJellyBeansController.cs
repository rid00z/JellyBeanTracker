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
    public class MyJellyBeansController : Controller
    {
        private JellyBeanDataContext db = new JellyBeanDataContext();

        // GET: MyJellyBeans
        public ActionResult Index()
        {
            return View(db.MyJellyBeans.ToList());
        }

        // GET: MyJellyBeans/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MyJellyBean myJellyBean = db.MyJellyBeans.Find(id);
            if (myJellyBean == null)
            {
                return HttpNotFound();
            }
            return View(myJellyBean);
        }

        // GET: MyJellyBeans/Create
        public ActionResult Create()
        {
            return View();
        }

        public ActionResult ProfitReport()
        {
            var jellyBeanProfitCalculator =
                new JellyBeanProfitCalculator(new DataContentDataSource(db));

            return View(jellyBeanProfitCalculator.CalculateProfit());
        }

        // POST: MyJellyBeans/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,JellyBeanName,TotalBeans")] MyJellyBean myJellyBean)
        {
            if (ModelState.IsValid)
            {
                myJellyBean.Id = Guid.NewGuid();
                db.MyJellyBeans.Add(myJellyBean);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(myJellyBean);
        }

        // GET: MyJellyBeans/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MyJellyBean myJellyBean = db.MyJellyBeans.Find(id);
            if (myJellyBean == null)
            {
                return HttpNotFound();
            }
            return View(myJellyBean);
        }

        // POST: MyJellyBeans/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,JellyBeanName,TotalBeans")] MyJellyBean myJellyBean)
        {
            if (ModelState.IsValid)
            {
                db.Entry(myJellyBean).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(myJellyBean);
        }

        // GET: MyJellyBeans/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MyJellyBean myJellyBean = db.MyJellyBeans.Find(id);
            if (myJellyBean == null)
            {
                return HttpNotFound();
            }
            return View(myJellyBean);
        }

        // POST: MyJellyBeans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            MyJellyBean myJellyBean = db.MyJellyBeans.Find(id);
            db.MyJellyBeans.Remove(myJellyBean);
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

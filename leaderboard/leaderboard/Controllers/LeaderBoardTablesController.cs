using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using leaderboard.Models;

namespace leaderboard.Controllers
{
    public class LeaderBoardTablesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: LeaderBoardTables
        public ActionResult Index()
        {
            Dictionary<string, int> dict = new Dictionary<string, int>();

            // Initialize dictionary with Competitors and their points to 0
            foreach (var item in db.LeaderBoardTables.ToList())
            {
                dict[item.CompeteName] = 0;
            }
            // Calculagte aggregate of points for every user competitor
            foreach (var item in db.LeaderBoardTables.ToList())
            {
                dict[item.CompeteName] += item.Points;
            }

            int win = 0;
            int loss = 0;
            int draw = 0;

            if (dict.Count > 0)
            {
                // Calculate competitor who got maximum points
                win = dict.Values.Max();
                var winKey = dict.FirstOrDefault(x => x.Value == win).Key;
                ViewData["Win Competitor"] = winKey.ToString();
                ViewData["Win Points"] = win;

                // Calculate competitor who got minimum points
                loss = dict.Values.Min();
                var lossKey = dict.FirstOrDefault(x => x.Value == loss).Key;
                ViewData["Loss Competitor"] = lossKey.ToString();
                ViewData["Loss Points"] = loss;

                // Caluclate competetor who got drawn points
                draw = (win + loss) / 2;
                var drawKey = dict.FirstOrDefault(x => x.Value >= draw && x.Value <= win).Key;
                ViewData["Draw Competitor"] = drawKey.ToString();
                ViewData["Draw Points"] = dict[drawKey];
            }

            return View(db.LeaderBoardTables.ToList());
        }

        // GET: LeaderBoardTables/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LeaderBoardTable leaderBoardTable = db.LeaderBoardTables.Find(id);
            if (leaderBoardTable == null)
            {
                return HttpNotFound();
            }
            return View(leaderBoardTable);
        }

        // GET: LeaderBoardTables/Create
        public ActionResult Create()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }

        // POST: LeaderBoardTables/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CompeteName,H2HContestName,Points,Referee")] LeaderBoardTable leaderBoardTable)
        {
            if (ModelState.IsValid && User.Identity.IsAuthenticated)
            {
                // Fill the referee who added this Compettior name
                leaderBoardTable.Referee = User.Identity.Name;

                db.LeaderBoardTables.Add(leaderBoardTable);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(leaderBoardTable);
        }

        // GET: LeaderBoardTables/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LeaderBoardTable leaderBoardTable = db.LeaderBoardTables.Find(id);
            if (leaderBoardTable == null)
            {
                return HttpNotFound();
            }
            return View(leaderBoardTable);
        }

        // POST: LeaderBoardTables/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CompeteName,H2HContestName,Points,Referee")] LeaderBoardTable leaderBoardTable)
        {
            if (ModelState.IsValid && User.Identity.IsAuthenticated)
            {
                db.Entry(leaderBoardTable).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(leaderBoardTable);
        }

        // GET: LeaderBoardTables/Delete/5
        public ActionResult Delete(int? id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LeaderBoardTable leaderBoardTable = db.LeaderBoardTables.Find(id);
            if (leaderBoardTable == null)
            {
                return HttpNotFound();
            }
            return View(leaderBoardTable);
        }

        // POST: LeaderBoardTables/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            LeaderBoardTable leaderBoardTable = db.LeaderBoardTables.Find(id);
            db.LeaderBoardTables.Remove(leaderBoardTable);
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

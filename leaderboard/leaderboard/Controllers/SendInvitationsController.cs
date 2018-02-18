using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.IO;
using leaderboard.Models;

namespace leaderboard.Controllers
{
    public class SendInvitationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: SendInvitations
        public async Task<ActionResult> Index()
        {
            return View(await db.SendInvitations.ToListAsync());
        }

        // GET: SendInvitations/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SendInvitation sendInvitation = await db.SendInvitations.FindAsync(id);
            if (sendInvitation == null)
            {
                return HttpNotFound();
            }
            return View(sendInvitation);
        }

        // GET: SendInvitations/Create
        public ActionResult Create()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }

        // POST: SendInvitations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,EmailId,GuidId,SendDate")] SendInvitation sendInvitation)
        {
            if (ModelState.IsValid && User.Identity.IsAuthenticated)
            {
                var url = System.Configuration.ConfigurationManager.AppSettings.Get("RegisterUrl");

                var model = sendInvitation;

                // Create Guid
                model.GuidId = Guid.NewGuid().ToString();

                // Create Date time stamp
                model.SendDate = DateTime.Now;

                string html = RenderViewToString(ControllerContext,
                        "~/views/SendInvitations/NotificationEmail.cshtml",
                        model, true);

                ViewBag.RenderedHtml = html;

                // Send email invitation to Subscriber through SMTP
                try {
                    System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient(System.Configuration.ConfigurationManager.AppSettings.Get("SmtpServer"));

                    System.Net.Mail.MailAddress from = new System.Net.Mail.MailAddress(User.Identity.Name,
                       User.Identity.Name,
                    System.Text.Encoding.UTF8);
                    // Set destinations for the e-mail message.
                    System.Net.Mail.MailAddress to = new System.Net.Mail.MailAddress(sendInvitation.EmailId.ToString());
                    // Specify the message content.
                    System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage(from, to);

                    // Insert message Body here.

                    message.Body = html;
                    // Include some non-ASCII characters in body and subject.
                    string someArrows = new string(new char[] { '\u2190', '\u2191', '\u2192', '\u2193' });
                    message.Body += Environment.NewLine + someArrows;
                    message.BodyEncoding = System.Text.Encoding.UTF8;
                    message.Subject = "Invitation from ACME" + someArrows;
                    message.SubjectEncoding = System.Text.Encoding.UTF8;
                    // Set the method that is called back when the send operation ends.
                    // client.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);
                    string userState = "Invitation";
                    client.SendAsync(message, userState);
                }
                catch(Exception Ex)
                {

                }


                db.SendInvitations.Add(sendInvitation);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(sendInvitation);
        }

        // GET: SendInvitations/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SendInvitation sendInvitation = await db.SendInvitations.FindAsync(id);
            if (sendInvitation == null)
            {
                return HttpNotFound();
            }
            return View(sendInvitation);
        }

        // POST: SendInvitations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,EmailId,GuidId,SendDate")] SendInvitation sendInvitation)
        {
            if (ModelState.IsValid && User.Identity.IsAuthenticated)
            {
                db.Entry(sendInvitation).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(sendInvitation);
        }

        // GET: SendInvitations/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SendInvitation sendInvitation = await db.SendInvitations.FindAsync(id);
            if (sendInvitation == null)
            {
                return HttpNotFound();
            }
            return View(sendInvitation);
        }

        // POST: SendInvitations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            SendInvitation sendInvitation = await db.SendInvitations.FindAsync(id);
            db.SendInvitations.Remove(sendInvitation);
            await db.SaveChangesAsync();
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

        static string RenderViewToString(ControllerContext context,
                                            string viewPath,
                                            object model = null,
                                            bool partial = false)
        {
            // first find the ViewEngine for this view
            ViewEngineResult viewEngineResult = null;
            if (partial)
                viewEngineResult = ViewEngines.Engines.FindPartialView(context, viewPath);
            else
                viewEngineResult = ViewEngines.Engines.FindView(context, viewPath, null);

            if (viewEngineResult == null)
                throw new FileNotFoundException("View cannot be found.");

            // get the view and attach the model to view data
            var view = viewEngineResult.View;
            context.Controller.ViewData.Model = model;

            string result = null;

            using (var sw = new StringWriter())
            {
                var ctx = new ViewContext(context, view,
                                            context.Controller.ViewData,
                                            context.Controller.TempData,
                                            sw);
                view.Render(ctx, sw);
                result = sw.ToString();
            }

            return result;
        }
    }
}

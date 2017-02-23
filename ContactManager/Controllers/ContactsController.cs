using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using Binbin.Linq;
using ContactManager.Models;
using Microsoft.AspNet.Identity;

namespace ContactManager.Controllers
{
    public class ContactsController : Controller
    {
        private readonly IContactContext _db;

        public ContactsController(IContactContext db)
        {
            _db = db;
        }
        

        // GET: Contacts
        public async Task<ActionResult> Index()
        {
            return View(await _db.SetDb<Contact>().ToListAsync());
        }

        public async Task<ActionResult> FilteredIndex(string firstName, string lastName)
        {
            IQueryable<Contact> query = null;
            if (string.IsNullOrEmpty(firstName))
            {
                query = _db.SetDb<Contact>().Where(c => c.FirstName == firstName);
            }

            if (string.IsNullOrEmpty(lastName))
            {
                if (query != null)
                {
                    query.Where(c => c.LastName == lastName);
                }
                else
                {
                    query = _db.SetDb<Contact>().Where(c => c.LastName == lastName);
                }
            }

            List<Contact> result;
            if (query != null)
            {
                result = await query.ToListAsync();
            }
            else
            {
                result = new List<Contact>();
            }
            

            return View("Index", result);
        }


        // GET: Contacts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Contacts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "FirstName,LastName,EmailAddress,BirthDate,NumberOfComputers")] Contact contact)
        {
            if (ModelState.IsValid)
            {                
                contact.UserId = new Guid(User.Identity.GetUserId());
                _db.SetDb<Contact>().Add(contact);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(contact);
        }

        // GET: Contacts/Edit/5
        public async Task<ActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Contact contact = await _db.SetDb<Contact>().Include(c => c.Addresses).FirstOrDefaultAsync(c=>c.Id == id);

            if (contact == null)
            {
                return HttpNotFound();
            }
            return View(contact);
        }

        [HttpPost]
        public async Task<ActionResult> Edit([Bind(Include = "Id,UserId,FirstName,LastName,EmailAddress,BirthDate,NumberOfComputers")] Contact contact)
        {
            if (ModelState.IsValid)
            {

                _db.Entry(contact).State = EntityState.Modified;

                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(contact);
        }


        
        [ActionName("Delete")]
        public async Task<ActionResult> Delete(long id)
        {            
            var contact = await _db.SetDb<Contact>().FindAsync(id);
            _db.SetDb<Contact>().Remove(contact);
            var addresses = await _db.SetDb<Address>().Where(a => a.ContactId == id).ToListAsync();

            foreach (var address in addresses)
            {
                _db.SetDb<Address>().Remove(address);
            }
           
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

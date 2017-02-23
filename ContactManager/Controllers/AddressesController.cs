using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ContactManager.Models;

namespace ContactManager.Controllers
{
    public class AddressesController : Controller
    {
        private IContactContext _db = new ContactContext();

        // GET: Addresses
        public async Task<ActionResult> Index(int contactId)
        {
            await SetContactInformation(contactId);
            var addresses = _db.SetDb<Address>().Where(a=>a.ContactId == contactId);
            return View(await addresses.ToListAsync());
        }

        // GET: Addresses/Create
        public async Task<ActionResult> Create(long contactid)
        {
            await SetContactInformation(contactid);

            return View();
        }

        // POST: Addresses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,ContactId,AddressType,AddressLine1,AddressLine2,City,StateCode")] Address address)
        {
            if (ModelState.IsValid)
            {
                _db.SetDb<Address>().Add(address);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index", new { contactId = address.ContactId});
            }

            return View(address);
        }

        // GET: Addresses/Edit/5
        public async Task<ActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var address = await _db.SetDb<Address>().FindAsync(id);

            if (address == null)
            {
                return HttpNotFound();
            }
            
            return View(address);
        }

        // POST: Addresses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,ContactId,AddressType,AddressLine1,AddressLine2,City,StateCode")] Address address)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(address).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return RedirectToAction("Index", new {contactId = address.ContactId});
            }

            ViewBag.ContactId = new SelectList(_db.SetDb<Address>(), "Id", "FirstName", address.ContactId);
            return View(address);
        }


        public async Task<ActionResult> Delete(long id)
        {
            Address address = await _db.SetDb<Address>().FindAsync(id);
            var contactId = address.ContactId;
            _db.SetDb<Address>().Remove(address);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index", new {contactId});
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }

        private async Task SetContactInformation(long id)
        {
            var contact = await _db.SetDb<Contact>().Where(c => c.Id == id).FirstOrDefaultAsync();
            if (contact == null)
            {
                throw new Exception($"Invalid ContactID {id}");
            }
            ViewBag.Id = id;
            ViewBag.Name = string.Format($"{contact.FirstName} {contact.LastName}");
        }

    }
}

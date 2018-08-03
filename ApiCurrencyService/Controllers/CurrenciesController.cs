using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using ApiCurrencyService.Models;

namespace ApiCurrencyService.Controllers
{
    [RoutePrefix("api/currencies")]
    public class CurrenciesController : ApiController
    {
        private CurrencyDbEntities db = new CurrencyDbEntities();

        // GET: api/Currencies
        public IQueryable<Currency> GetCurrencies()
        {
            return db.Currencies;
        }

        [ResponseType(typeof(Currency))]
        [Route("country/{country}")]
        public IHttpActionResult GetCurrencyByCountry(string country)
        {
            List<Currency> currency = db.Currencies.Where(t => t.Currency_Country_ == country).ToList();
            if (currency == null)
            {
                return NotFound();
            }

            return Ok(currency);
        }




        // GET: api/Currencies/5
        [ResponseType(typeof(Currency))]
        public IHttpActionResult GetCurrency(int id)
        {
            Currency currency = db.Currencies.Find(id);
            if (currency == null)
            {
                return NotFound();
            }

            return Ok(currency);
        }

        // PUT: api/Currencies/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCurrency(int id, Currency currency)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != currency.Id)
            {
                return BadRequest();
            }

            db.Entry(currency).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CurrencyExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Currencies
        [ResponseType(typeof(Currency))]
        public IHttpActionResult PostCurrency(Currency currency)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Currencies.Add(currency);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = currency.Id }, currency);
        }

        // DELETE: api/Currencies/5
        [ResponseType(typeof(Currency))]
        public IHttpActionResult DeleteCurrency(int id)
        {
            Currency currency = db.Currencies.Find(id);
            if (currency == null)
            {
                return NotFound();
            }

            db.Currencies.Remove(currency);
            db.SaveChanges();

            return Ok(currency);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CurrencyExists(int id)
        {
            return db.Currencies.Count(e => e.Id == id) > 0;
        }
    }
}
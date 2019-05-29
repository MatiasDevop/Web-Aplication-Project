using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using MVClog.Models;

namespace MVClog.Controllers
{
    public class AutomaticoApiController : ApiController
    {
        private Context db = new Context();

        // GET api/AutomaticoApi
        public IEnumerable<AutomaticoPM> GetAutomaticoPMs()
        {
            var automaticopms = db.AutomaticoPMs.Include(a => a.Contaminante).Include(a => a.Estacion);
            return automaticopms.AsEnumerable();
        }

        // GET api/AutomaticoApi/5
        public AutomaticoPM GetAutomaticoPM(int id)
        {
            AutomaticoPM automaticopm = db.AutomaticoPMs.Find(id);
            if (automaticopm == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return automaticopm;
        }

        // PUT api/AutomaticoApi/5
        public HttpResponseMessage PutAutomaticoPM(int id, AutomaticoPM automaticopm)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != automaticopm.AutomaticoId)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            db.Entry(automaticopm).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // POST api/AutomaticoApi
        public HttpResponseMessage PostAutomaticoPM(AutomaticoPM automaticopm)
        {
            if (ModelState.IsValid)
            {
                db.AutomaticoPMs.Add(automaticopm);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, automaticopm);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = automaticopm.AutomaticoId }));
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        // DELETE api/AutomaticoApi/5
        public HttpResponseMessage DeleteAutomaticoPM(int id)
        {
            AutomaticoPM automaticopm = db.AutomaticoPMs.Find(id);
            if (automaticopm == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.AutomaticoPMs.Remove(automaticopm);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, automaticopm);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
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
    public class ActivoPMApiController : ApiController
    {
        private Context db = new Context();

        // GET api/ActivoPMApi
        public IEnumerable<ActivoPM> GetActivoPMs()
        {
            var activopms = db.ActivoPMs.Include(a => a.Contaminante).Include(a => a.Estacion);
            return activopms.AsEnumerable();
        }
        public IEnumerable<ActivoPM> GetActivos(int idEstacion)
        {
            List<ActivoPM> activopm = db.ActivoPMs.Where(x => x.Estacion.EstacionId == idEstacion).ToList();
            if (activopm == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return activopm;
        }

        // GET api/ActivoPMApi/5
        public ActivoPM GetActivoPM(int id)
        {
            ActivoPM activopm = db.ActivoPMs.Find(id);
            if (activopm == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return activopm;
        }

        // PUT api/ActivoPMApi/5
        public HttpResponseMessage PutActivoPM(int id, ActivoPM activopm)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != activopm.Id)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            db.Entry(activopm).State = EntityState.Modified;

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

        // POST api/ActivoPMApi
        public HttpResponseMessage PostActivoPM(ActivoPM activopm)
        {
            if (ModelState.IsValid)
            {
                db.ActivoPMs.Add(activopm);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, activopm);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = activopm.Id }));
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        // DELETE api/ActivoPMApi/5
        public HttpResponseMessage DeleteActivoPM(int id)
        {
            ActivoPM activopm = db.ActivoPMs.Find(id);
            if (activopm == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.ActivoPMs.Remove(activopm);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, activopm);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
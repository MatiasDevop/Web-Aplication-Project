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
    public class PasivoApiController : ApiController
    {
        private Context db = new Context();

        // GET api/PasivoApi
        public IEnumerable<Pasivo> GetPasivoes()
        {
            var pasivoes = db.Pasivoes.Include(p => p.Contaminante).Include(p => p.Estacion);
            return pasivoes.AsEnumerable();
        }

        // GET api/PasivoApi/5
        public Pasivo GetPasivo(int id)
        {
            Pasivo pasivo = db.Pasivoes.Find(id);
            if (pasivo == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return pasivo;
        }

        // PUT api/PasivoApi/5
        public HttpResponseMessage PutPasivo(int id, Pasivo pasivo)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != pasivo.PasivoID)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            db.Entry(pasivo).State = EntityState.Modified;

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

        // POST api/PasivoApi
        public HttpResponseMessage PostPasivo(Pasivo pasivo)
        {
            if (ModelState.IsValid)
            {
                db.Pasivoes.Add(pasivo);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, pasivo);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = pasivo.PasivoID }));
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        // DELETE api/PasivoApi/5
        public HttpResponseMessage DeletePasivo(int id)
        {
            Pasivo pasivo = db.Pasivoes.Find(id);
            if (pasivo == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.Pasivoes.Remove(pasivo);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, pasivo);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
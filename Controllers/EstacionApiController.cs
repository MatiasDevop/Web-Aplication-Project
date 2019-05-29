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
    public class EstacionApiController : ApiController
    {
        private Context db = new Context();

        // GET api/EstacionApi
        public IEnumerable<Estacion> GetEstacions()
        {
            return db.Estacions.AsEnumerable();
        }

        // GET api/EstacionApi/5
        public Estacion GetEstacion(int id)
        {
            Estacion estacion = db.Estacions.Find(id);
            if (estacion == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return estacion;
        }

        // PUT api/EstacionApi/5
        public HttpResponseMessage PutEstacion(int id, Estacion estacion)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != estacion.EstacionId)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            db.Entry(estacion).State = EntityState.Modified;

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

        // POST api/EstacionApi
        public HttpResponseMessage PostEstacion(Estacion estacion)
        {
            if (ModelState.IsValid)
            {
                db.Estacions.Add(estacion);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, estacion);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = estacion.EstacionId }));
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        // DELETE api/EstacionApi/5
        public HttpResponseMessage DeleteEstacion(int id)
        {
            Estacion estacion = db.Estacions.Find(id);
            if (estacion == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.Estacions.Remove(estacion);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, estacion);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
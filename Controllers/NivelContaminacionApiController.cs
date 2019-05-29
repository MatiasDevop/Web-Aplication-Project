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
    public class NivelContaminacionApiController : ApiController
    {
        private Context db = new Context();

        // GET api/NivelContaminacionApi
        public IEnumerable<NivelContaminacion> GetNivelContaminacions()
        {
            var nivelcontaminantes = db.NivelContaminantes.Include(n => n.Contaminante).Include(n => n.Estacion);
            return nivelcontaminantes.AsEnumerable();
        }

        // GET api/NivelContaminacionApi/5
        public NivelContaminacion GetNivelContaminacion(int id)
        {
            NivelContaminacion nivelcontaminacion = db.NivelContaminantes.Find(id);
            if (nivelcontaminacion == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return nivelcontaminacion;
        }

        // PUT api/NivelContaminacionApi/5
        public HttpResponseMessage PutNivelContaminacion(int id, NivelContaminacion nivelcontaminacion)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != nivelcontaminacion.NivelID)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            db.Entry(nivelcontaminacion).State = EntityState.Modified;

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

        // POST api/NivelContaminacionApi
        public HttpResponseMessage PostNivelContaminacion(NivelContaminacion nivelcontaminacion)
        {
            if (ModelState.IsValid)
            {
                db.NivelContaminantes.Add(nivelcontaminacion);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, nivelcontaminacion);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = nivelcontaminacion.NivelID }));
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        // DELETE api/NivelContaminacionApi/5
        public HttpResponseMessage DeleteNivelContaminacion(int id)
        {
            NivelContaminacion nivelcontaminacion = db.NivelContaminantes.Find(id);
            if (nivelcontaminacion == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.NivelContaminantes.Remove(nivelcontaminacion);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, nivelcontaminacion);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
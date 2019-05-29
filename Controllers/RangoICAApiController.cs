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
    public class RangoICAApiController : ApiController
    {
        private Context db = new Context();

        // GET api/RangoICAApi
        public IEnumerable<RangoICA> GetRangoICAs()
        {
            return db.RangoICAs.AsEnumerable();
        }

        // GET api/RangoICAApi/5
        public RangoICA GetRangoICA(int id)
        {
            RangoICA rangoica = db.RangoICAs.Find(id);
            if (rangoica == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return rangoica;
        }

        // PUT api/RangoICAApi/5
        public HttpResponseMessage PutRangoICA(int id, RangoICA rangoica)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != rangoica.ID)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            db.Entry(rangoica).State = EntityState.Modified;

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

        // POST api/RangoICAApi
        public HttpResponseMessage PostRangoICA(RangoICA rangoica)
        {
            if (ModelState.IsValid)
            {
                db.RangoICAs.Add(rangoica);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, rangoica);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = rangoica.ID }));
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        // DELETE api/RangoICAApi/5
        public HttpResponseMessage DeleteRangoICA(int id)
        {
            RangoICA rangoica = db.RangoICAs.Find(id);
            if (rangoica == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.RangoICAs.Remove(rangoica);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, rangoica);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebApplication13.Filters;

namespace WebApplication13.Controllers
{
    public class HomeController : ApiController
    {
        // api/home/getfiles
        [HttpGet]
        [Route("api/home/getfiles")]
        [CustomRateLimit(limit: 5, window: 1)]
        public HttpResponseMessage GetFiles()
        {
            // By Mohammed Ahmed Hussien Babiker
          //  return new HttpResponseMessage(HttpStatusCode.OK);

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);

            var fileName = HttpContext.Current.Server.MapPath("~/MyFiles/books.jpg");


            if (!File.Exists(fileName))
            {
                response.StatusCode = HttpStatusCode.NotFound;
                response.ReasonPhrase = "There is no file has this path or name, check you path or name";
                throw new HttpResponseException(response);
            }

            byte[] bytes = File.ReadAllBytes(fileName);

            response.Content = new ByteArrayContent(bytes);
            // Set the length of the Content
            response.Content.Headers.ContentLength = bytes.LongLength;

            response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");

            response.Content.Headers.ContentDisposition.FileName = "Book.jgp";

            response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(MimeMapping.GetMimeMapping("books.jpg"));
            return response;


        }
    }
}

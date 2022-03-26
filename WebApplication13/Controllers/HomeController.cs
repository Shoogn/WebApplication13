using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace WebApplication13.Controllers
{
    public class HomeController : ApiController
    {
        // GET: api/Home
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Home/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Home
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Home/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Home/5
        public void Delete(int id)
        {
        }

        // api/home/getfiles
        [HttpGet]
        [Route("api/home/getfiles")]
        public HttpResponseMessage GetFiles()
        {
          //  return new HttpResponseMessage(HttpStatusCode.OK);

            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);

            //var filePath = File.OpenRead("E:/TestApp/WebApplication13/WebApplication13/MyFiles/books.jpg");
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

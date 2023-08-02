using BlazorAppFileUpload.Shared.Model;
using Microsoft.AspNetCore.Mvc;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BlazorAppFileUpload.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        // GET: api/<UploadController>

        private readonly IWebHostEnvironment env;
        public UploadController(IWebHostEnvironment ev) { 
            this.env = ev;  
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<UploadController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<UploadController>
        [HttpPost]
        public async Task<ActionResult<List<UploadResult>>> UploadFile([FromForm]IEnumerable<IFormFile> files)
        {
            List<UploadResult> uploadResults = new List<UploadResult>();

            foreach (IFormFile file in files)
            {
                var uploadResult=new UploadResult();

                var trustedFileNameFordisplay = WebUtility.HtmlEncode(file.FileName);

                string trustedFileNameForFileStorage = Path.GetRandomFileName();

                string nf = Guid.NewGuid().ToString();

                string path = Path.Combine(env.ContentRootPath, "Uploads", nf.ToString());

                path += file.ContentType == "image/jpeg" ? ".jpeg" : ".png";

                await using FileStream fileStream = new(path, FileMode.Create);

                await file.CopyToAsync(fileStream);

                uploadResult.StoredFileName = trustedFileNameForFileStorage;
                uploadResults.Add(uploadResult);
                
            }

            return Ok(uploadResults);
        }

        // PUT api/<UploadController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UploadController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

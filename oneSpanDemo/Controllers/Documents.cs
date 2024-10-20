using Microsoft.AspNetCore.Mvc;
using oneSpanDemo.Models;
using oneSpanDemo.Services;

namespace oneSpanDemo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Documents : ControllerBase
    {
        Iedocument edocumentIntegrator;

        public Documents(Iedocument _edocumentIntegrator) 
        {
            edocumentIntegrator = _edocumentIntegrator;
        }


        [HttpGet(Name = "GetDocuments")]
        public async Task<IActionResult> Get(string id)
        {
            
            var documentsZip = await edocumentIntegrator.GetEdocument(id);

            if(documentsZip == null || documentsZip.status == DocumentsResultStatus.UnAbleToLoad || documentsZip.status == DocumentsResultStatus.NoFile)
                return NotFound();

            else if (documentsZip.status == DocumentsResultStatus.UnAuthorized)
                return Unauthorized();



             return File(documentsZip.stream, "application/zip");
        }
    }
}

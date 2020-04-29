using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HelpdeskViewModels;

namespace HelpdeskWebsite.Controllers
{
    public class ProblemController : ApiController
    {
        [Route("api/problems")]
        public IHttpActionResult GetAll()
        {
            try
            {
                ProblemViewModel pVm = new ProblemViewModel();
                List<ProblemViewModel> allVms = pVm.GetAll();
                return Ok(allVms);
            }
            catch (Exception ex)
            {
                return BadRequest("Retrieve failed - " + ex.Message);
            }
        }

    }
}

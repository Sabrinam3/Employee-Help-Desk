using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HelpdeskViewModels;

namespace HelpdeskWebsite.Controllers
{
    public class DepartmentController : ApiController
    {
        [Route("api/departments")]
        public IHttpActionResult GetAll()
        {
            try
            {
                DepartmentViewModel depVm = new DepartmentViewModel();
                List<DepartmentViewModel> allVms = depVm.GetAll();
                return Ok(allVms);
            }
            catch (Exception ex)
            {
                return BadRequest("Retrieve failed - " + ex.Message);
            }
        }
    }
}

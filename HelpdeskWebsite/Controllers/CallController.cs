using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HelpdeskViewModels;

namespace HelpdeskWebsite.Controllers
{
    public class CallController : ApiController
    {
        //Gets all calls from the server
        [Route("api/calls")]
        public IHttpActionResult GetAll()
        {
            try
            {
                CallViewModel cvm = new CallViewModel();
                //Use the CallViewModel's GetAll method to fetch and retrieve a list of calls from the server
                List<CallViewModel> allCalls = cvm.GetAll();
                return Ok(allCalls);
            }
            catch (Exception ex)
            {
                return BadRequest("Retrieve failed- " + ex.Message);
            }
        }

        //Sends a call to the server after updates have been executed
        [Route("api/calls")]
        public IHttpActionResult Put(CallViewModel call)
        {
            try
            {
                //Use the EmployeeViewModel's Update method to update the employee
                int retVal = call.Update();
                switch (retVal)
                {
                    case 1:
                        return Ok("Call  updated!");
                    case -1:
                        return Ok("Call not updated!");
                    case -2:
                        return Ok("Data is stale, call not updated!");
                    default:
                        return Ok("Call not updated!");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Update Failed - " + ex.Message);
            }
        }

        //Send a newly created call back to the server
        [Route("api/calls")]
        public IHttpActionResult Post(CallViewModel call)
        {
            try
            {
                //Use the CallViewModel's add method to add a new call to the database
                call.Add();
                if (call.Id > 0)
                {
                    return Ok("Call added!");
                }
                else
                {
                    return Ok("Call not added!");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Creation failed - Contact Tech Support");
            }
        }
        //Delete a call from the database
        [Route("api/calls/{id}")]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                CallViewModel call = new CallViewModel();
                call.Id = id;

                //Use the EmployeeViewModel's delete method and check the return to see if the delete was successful
                if (call.Delete() == 1)
                {
                    return Ok("Call deleted!");
                }
                else
                {
                    return Ok("Call not deleted!");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Delete failed - Contact Tech Support");
            }
        }
    }
}

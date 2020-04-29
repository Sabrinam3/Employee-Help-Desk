//Employee Controller to send data back from the webpage to the server
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HelpdeskViewModels;

namespace HelpdeskWebsite.Controllers
{
    public class EmployeeController : ApiController
    {
        //Gets an employee from the server based on a name
        [Route("api/employees/{name}")]
        public IHttpActionResult Get(string name)
        {
            try
            {
                EmployeeViewModel emp = new EmployeeViewModel();
                emp.Lastname = name;
                //Use the EmployeeViewModel's GetByLastName to retrieve the employee
                emp.GetByLastName();
                return Ok(emp);
            }catch(Exception ex)
            {
                return BadRequest("Retrieve failed - " + ex.Message);
            }
        }

        //Sends an employee to the server after updates have been executed
        [Route("api/employees")]
        public IHttpActionResult Put(EmployeeViewModel emp)
        {
            try
            {
                //Use the EmployeeViewModel's Update method to update the employee
                int retVal = emp.Update();
                switch(retVal)
                {
                    case 1:
                        return Ok("Employee " + emp.Lastname + " updated!");
                    case -1:
                        return Ok("Employee " + emp.Lastname + "not updated!");
                    case -2:
                        return Ok("Employee is stale for " + emp.Lastname + ", employee not updated!");
                    default:
                        return Ok("Employee " + emp.Lastname + "not updated!");
                }
            }catch(Exception ex)
            {
                return BadRequest("Update Failed - " + ex.Message);
            }
        }

        //Retrieves all employees from the server
        [Route("api/employees")]
        public IHttpActionResult GetAll()
        {
            try
            {
                EmployeeViewModel emp = new EmployeeViewModel();
                //Use the EmployeeViewModel's GetAll method to fetch and retrieve a list of employees from the server
                List<EmployeeViewModel> allEmployees = emp.GetAll();
                return Ok(allEmployees);
            }catch(Exception ex)
            {
                return BadRequest("Retrieve failed- " + ex.Message);
            }
        }

        //Send a newly created employee back to the server
        [Route("api/employees")]
        public IHttpActionResult Post(EmployeeViewModel emp)
        {
            try
            {
                //Use the EmployeeViewModel's add method to add a new employee to the database
                emp.Add();
                if (emp.Id > 0)
                {
                    return Ok("Employee " + emp.Lastname + " added!");
                }
                else
                {
                    return Ok("Employee " + emp.Lastname + " not added!");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Creation failed - Contact Tech Support");
            }
        }

        //Delete an employee from the database
        [Route("api/employees/{id}")]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                EmployeeViewModel emp = new EmployeeViewModel();
                emp.Id = id;

                //Use the EmployeeViewModel's delete method and check the return to see if the delete was successful
                if (emp.Delete() == 1)
                {
                    return Ok("Employee deleted!");
                }
                else
                {
                    return Ok("Employee not deleted!");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Delete failed - Contact Tech Support");
            }
        }

    }
}

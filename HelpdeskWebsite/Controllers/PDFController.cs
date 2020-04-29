using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HelpdeskWebsite.Reports;
namespace HelpdeskWebsite.Controllers
{
    public class PDFController : ApiController
    {
        [Route("api/callreport")]
        public IHttpActionResult GetCallReport()
        {
            try
            {
                CallReport report = new CallReport();
                report.doIt();
                return Ok("report generated");
            }
            catch (Exception ex)
            {
                return BadRequest("Report Generation Failed -" + ex.Message);
            }
        }

        [Route("api/callreport/{id}")]
        public IHttpActionResult GetCallReportForEmployee(int id)
        {
            try
            {
                CallReport report = new CallReport();
                report.generateSpecificEmployeeReport(id);
                return Ok("report generated");
            }
            catch(Exception ex)
            {
                return BadRequest("Report Generation Failed -" + ex.Message);
            }
        }

        [Route("api/employeereport")]
        public IHttpActionResult GetEmployeeReport()
        {
            try
            {
                EmployeeReport report = new EmployeeReport();
                report.doIt();
                return Ok("report generated");
            }
            catch (Exception ex)
            {
                return BadRequest("Report Generation Failed -" + ex.Message);
            }
        }
    }
}

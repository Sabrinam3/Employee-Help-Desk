using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HelpdeskDAL;

namespace CaseStudyTests
{
    [TestClass]
   public class CallModelTests
    {
        [TestMethod]
        public void ComprehensiveModelTestsShouldReturnTrue()
        {
            CallModel cmodel = new CallModel();
            EmployeeModel emodel = new EmployeeModel();
            ProblemModel pmodel = new ProblemModel();
            Call call = new Call();
            call.DateOpened = DateTime.Now;
            call.DateClosed = null;
            call.OpenStatus = true;
            call.EmployeeId = emodel.GetByLastName("Tessier").Id;
            call.TechId = emodel.GetByLastName("Burner").Id;
            call.ProblemId = pmodel.GetByDescription("Hard Drive Failure").Id;
            call.Notes = "Sabrina's drive is shot, Burner to fix it";
            int newCallId = cmodel.Add(call);
            Console.WriteLine("new Call Generated - Id = " + newCallId);
            call = cmodel.GetById(newCallId);
            byte[] oldtimer = call.Timer;
            Console.WriteLine("New Call Retrieved");
            call.Notes += "\n Ordered new RAM!";

            if (cmodel.Update(call) == UpdateStatus.Ok)
            {
                Console.WriteLine("Call was updated " + call.Notes);
            }
            else
            {
                Console.WriteLine("Call was not updated!");
            }
            call.Timer = oldtimer;
            if (cmodel.Update(call) == UpdateStatus.Stale)
            {
                Console.WriteLine("Call was not updated due to stale data");
            }
            cmodel = new CallModel();
            call = cmodel.GetById(newCallId);

            if (cmodel.Delete(newCallId) == 1)
            {
                Console.WriteLine("Call was deleted!");
            }
            else
            {
                Console.WriteLine("Call was not deleted!");
            }

            Assert.IsNull(cmodel.GetById(newCallId));
        }
    }
}

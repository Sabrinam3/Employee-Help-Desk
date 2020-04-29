using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using HelpdeskDAL;

namespace CaseStudyTests
{
    [TestClass]
    public class EmployeeModelTests
    {
        [TestMethod]
        public void EmployeeModelGetAllShouldReturnList()
        {
            EmployeeModel model = new EmployeeModel();
            List<Employee> allEmps = model.GetAllEmployees();
            Assert.IsTrue(allEmps.Count > 0);
        }

        [TestMethod]
        public void EmployeeModelShouldReturnNewId()
        {
            EmployeeModel model = new EmployeeModel();
            Employee newEmp = new Employee();
            newEmp.Title = "Ms. ";
            newEmp.FirstName = "Test";
            newEmp.LastName = "Employee";
            newEmp.Email = "ts@abc.com";
            newEmp.PhoneNo = "(555)-555-5555";
            newEmp.DepartmentId = 100;
            int newId = model.Add(newEmp);
            Assert.IsTrue(newId > 0);
        }

        [TestMethod]
        public void EmployeeModelGetbyIdShouldReturnEmployee()
        {
            EmployeeModel model = new EmployeeModel();
            Employee emp = model.GetByLastName("Employee");
            Employee anotherEmp = model.GetById(emp.Id);
            Assert.IsNotNull(anotherEmp);
        }

        [TestMethod]
        public void EmployeeModelUpdateShouldReturnOkStatus()
        {
            EmployeeModel model = new EmployeeModel();
            Employee updateEmployee = model.GetByLastName("Tessier");
            updateEmployee.Email = (updateEmployee.Email.IndexOf(".ca") > 0) ? "ts@abc.com" : "ts@abc.ca";
            UpdateStatus EmployeesUpdated = model.Update(updateEmployee);
            Assert.IsTrue(EmployeesUpdated == UpdateStatus.Ok);
        }

        [TestMethod]
        public void EmployeeModelUpdateTwiceShouldReturnStaleStatus()
        {
            EmployeeModel model1 = new EmployeeModel();
            EmployeeModel model2 = new EmployeeModel();
            Employee updateEmp1 = model1.GetByLastName("Tessier");
            Employee updateEmp2 = model2.GetByLastName("Tessier");
            updateEmp1.Email = (updateEmp1.Email.IndexOf(".ca") > 0) ? "ts@abc.com" : "ts@abc.ca";
            if (model1.Update(updateEmp1) == UpdateStatus.Ok)
            {
                updateEmp2.Email = (updateEmp2.Email.IndexOf(".ca") > 0) ? "ts@abc.com" : "ts@abc.ca";
                Assert.IsTrue(model2.Update(updateEmp2) == UpdateStatus.Stale);
            }
            else
                Assert.Fail();
        }

        [TestMethod]
        public void EmployeeModelDeleteShouldReturnOne()
        {
            EmployeeModel model = new EmployeeModel();
            Employee deleteEmp = model.GetByLastName("Employee");
            int employeesDeleted = model.Delete(deleteEmp.Id);
            Assert.IsTrue(employeesDeleted == 1);
        }

        [TestMethod]
        public void LoadPicsShouldReturnTrue()
        {
            DALUtil util = new DALUtil();
            Assert.IsTrue(util.AddEmployeePicsToDb());
        }
    }
}

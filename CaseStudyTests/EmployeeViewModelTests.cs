using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HelpdeskViewModels;
using System.Collections.Generic;

namespace CaseStudyTests
{
    [TestClass]
    public class EmployeeViewModelTests
    {
        [TestMethod]
        public void EmployeeViewModelAddShouldReturnId()
        {
            EmployeeViewModel vm = new EmployeeViewModel();
            vm.Title = "Mr.";
            vm.Firstname = "Sabrina";
            vm.Lastname = "Tessier";
            vm.Email = "ts@abc.com";
            vm.Phoneno = "(555)555-5551";
            vm.DepartmentId = 100;
            vm.Add();
            Assert.IsTrue(vm.Id > 0);
        }
        [TestMethod]
        public void EmployeeViewModelGetbyNameShouldPopulatePropertyFirstname()
        {
            EmployeeViewModel vm = new EmployeeViewModel();
            vm.Lastname = "Tessier"; //Look for an existing employee
            vm.GetByLastName();
            Assert.IsNotNull(vm.Firstname);
        }

        [TestMethod]
        public void EmployeeViewModelGetAllShouldReturnAtLeastOneVM()
        {
            EmployeeViewModel vm = new EmployeeViewModel();
            List<EmployeeViewModel> allEmployeeVms = vm.GetAll();
            Assert.IsTrue(allEmployeeVms.Count > 0);
        }

        [TestMethod]
        public void EmployeeViewModelGetbyIdShouldPopulatePropertyFirstname()
        {
            EmployeeViewModel vm = new EmployeeViewModel();
            vm.Lastname = "Tessier";
            vm.GetByLastName(); //Retrieve employee just added should populate Id
            vm.GetById();
            Assert.IsNotNull(vm.Firstname);
        }

        [TestMethod]
        public void EmployeeModelUpdateShouldReturnOkStatus()
        {
            EmployeeViewModel vm = new EmployeeViewModel();
            vm.Lastname = "Tessier";
            vm.GetByLastName(); //Employee just added
            vm.Email = (vm.Email.IndexOf(".ca") > 0) ? "ts@abc.com" : "ts@abc.ca";
            int EmployeesUpdated = vm.Update();
            Assert.IsTrue(EmployeesUpdated > 0);
        }

        [TestMethod]
        public void EmployeeViewModelUpdateTwiceShouldReturnNegativeTwo()
        {
            EmployeeViewModel vm1 = new EmployeeViewModel();
            EmployeeViewModel vm2 = new EmployeeViewModel();
            vm1.Lastname = "Tessier";
            vm2.Lastname = "Tessier";
            vm1.GetByLastName();
            vm2.GetByLastName();
            vm1.Email = (vm1.Email.IndexOf(".ca") > 0) ? "ts@abc.com" : "ts@abc.ca";
            if (vm1.Update() == 1)
            {
                vm2.Email = (vm2.Email.IndexOf(".ca") > 0) ? "ts@abc.com" : "ts@abc.ca";
                Assert.IsTrue(vm2.Update() == -2);
            }
            else
                Assert.Fail();
        }

        [TestMethod]
        public void EmployeeModelDeleteShouldReturnOne()
        {
            EmployeeViewModel vm = new EmployeeViewModel();
            vm.Lastname = "Tessier";
            vm.GetByLastName();
            int employeesDeleted = vm.Delete();
            Assert.IsTrue(employeesDeleted == 1);
        }
    }
}

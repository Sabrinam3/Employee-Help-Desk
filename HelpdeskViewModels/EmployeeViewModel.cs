//Class Name: EmployeeViewModel
//Coder: Sabrina Tessier
//Purpose: Acts as a go-between for the controllers and the Employee Model(data access layer) and
//               allows the webpage to interact with the data in a safe way

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using HelpdeskDAL;

namespace HelpdeskViewModels
{
    public class EmployeeViewModel
    {

        //Properties of the EmployeeViewModel class -> same as the Employee.cs attributes
        private EmployeeModel _model;
        public string Title { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string Phoneno { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public int Id { get; set; }
        public bool? isTech { get; set; }
        public string StaffPicture64 { get; set; }
        public string Timer { get; set; }

        //Constructor- creates an instance of EmployeeModel for every EmployeeViewModel object created
        public EmployeeViewModel()
        {
            _model = new EmployeeModel();
        }

        //find employee using Lastname property by using the EmployeeModel object
        public void GetByLastName()
        {
            try
            {
                Employee emp = _model.GetByLastName(Lastname);
                if (emp == null)
                {
                    this.Lastname = "not found";
                    return;
                }

                //Set the remaining properties of the view model object based on the properties of the employee model object
                Title = emp.Title;
                Firstname = emp.FirstName;
                Lastname = emp.LastName;
                Phoneno = emp.PhoneNo;
                Email = emp.Email;
                Id = emp.Id;
                DepartmentId = emp.DepartmentId;
                if (emp.StaffPicture != null)
                {
                    StaffPicture64 = Convert.ToBase64String(emp.StaffPicture);
                }
                Timer = Convert.ToBase64String(emp.Timer);
            }
            catch (Exception ex)
            {
                Lastname = "not found";
                Console.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
        }

        //Retrieve all the employees in a list
        public List<EmployeeViewModel> GetAll()
        {
            List<EmployeeViewModel> allVms = new List<EmployeeViewModel>();
            try
            {
                //Get a list of all the employees and use each employee in that list to create a view model and then return the list of view model objects
                List<Employee> allEmps = _model.GetAllEmployees();
                foreach (Employee emp in allEmps)
                {
                    EmployeeViewModel empVm = new EmployeeViewModel();
                    empVm.Title = emp.Title;
                    empVm.Firstname = emp.FirstName;
                    empVm.Lastname = emp.LastName;
                    empVm.Phoneno = emp.PhoneNo;
                    empVm.Email = emp.Email;
                    empVm.Id = emp.Id;
                    empVm.DepartmentId = emp.DepartmentId;
                    empVm.DepartmentName = emp.Department.DepartmentName;
                    empVm.Timer = Convert.ToBase64String(emp.Timer);
                    empVm.isTech = emp.IsTech;

                    if(emp.StaffPicture != null)
                    empVm.StaffPicture64 = Convert.ToBase64String(emp.StaffPicture);
                    allVms.Add(empVm);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
            return allVms;
        }

        //Add  an employee
        public void Add()
        {
            Id = -1;
            try
            {
                //Creates an employee object from the attributes of the view model object this method is called on
                Employee emp = new Employee();
                emp.Title = Title;
                emp.FirstName = Firstname;
                emp.LastName = Lastname;
                emp.PhoneNo = Phoneno;
                emp.Email = Email;
                emp.DepartmentId = DepartmentId;
                if (StaffPicture64 != null)
                {
                    emp.StaffPicture = Convert.FromBase64String(StaffPicture64);
                }
                //Use the employee model's add method to add the new employee into the database
                Id = _model.Add(emp);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
        }

        //Update an employee by creating a new employee from the traits of the view model object this method is called on.
        public int Update()
        {
            UpdateStatus opStatus = UpdateStatus.Failed;
            try
            {
                Employee emp = new Employee();
                emp.Title = Title;
                emp.FirstName = Firstname;
                emp.LastName = Lastname;
                emp.PhoneNo = Phoneno;
                emp.Email = Email;
                emp.Id = Id;
                emp.DepartmentId = DepartmentId;
                if (StaffPicture64 != null)
                {
                    emp.StaffPicture = Convert.FromBase64String(StaffPicture64);
                }
                emp.Timer = Convert.FromBase64String(Timer);
                //Use the employee model's update method to send changes back to the database
                opStatus = _model.Update(emp);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
            return Convert.ToInt16(opStatus);
        }

        //Delete an employee
        public int Delete()
        {
            int employeesDeleted = 1;
            try
            {
                employeesDeleted = _model.Delete(Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }

            return employeesDeleted;
        }

        //Retrieve an instance of employee using Id property
        public void GetById()
        {
            try
            {
                //Retrieve the employee that has the same Id as the view model object this method was called on
                Employee emp = _model.GetById(Id);
                //Set the remaining properties of the view model object 
                Title = emp.Title;
                Firstname = emp.FirstName;
                Lastname = emp.LastName;
                Phoneno = emp.PhoneNo;
                Email = emp.Email;
                Id = emp.Id;
                DepartmentId = emp.DepartmentId;
                if (emp.StaffPicture != null)
                {
                    StaffPicture64 = Convert.ToBase64String(emp.StaffPicture);
                }
                Timer = Convert.ToBase64String(emp.Timer);
            }
            catch (NullReferenceException nex)
            {
                Lastname = "not found";
                throw nex;
            }
            catch (Exception ex)
            {
                Lastname = "not found";
                Console.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
        }
    }
}

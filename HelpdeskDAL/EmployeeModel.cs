/*
 * Class Name: EmployeeModel
 * Coder: Sabrina Tessier
 * Purpose: the data access layer for the employee data. This class interacts directly with the data in the database via methods in the repository.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Data.Entity.Infrastructure;

namespace HelpdeskDAL
{
    public class EmployeeModel
    {
        //Create an instance of repository object so that this class can use the repository methods
        IRepository<Employee> repo;

        public EmployeeModel()
        {
            repo = new HelpDeskRepository<Employee>();
        }

        //Retrieves an instance of an employee by the last name
        public Employee GetByLastName(string name)
        {
            List<Employee> empList = null;

            try
            {
                //Uses the repository's method to retrieve the employee with the last name matching the parameter argument
                empList = repo.GetByExpression(emp => emp.LastName == name);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
            //Retrieve the first instance of employee with the last name
            return empList.FirstOrDefault();
        }

        //Retrieves an instance of an employee by the Id
        public Employee GetById(int id)
        {
            List<Employee> empList = null;
            try
            {
                //Uses the repository's method to retrieve the employee with the Id matching the parameter argument
                empList = repo.GetByExpression(emp => emp.Id == id);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
            return empList.FirstOrDefault();
        }
        
        //Retrieves list of all employees
        public List<Employee> GetAllEmployees()
        {
            List<Employee> empList = new List<Employee>();
            try
            {
                //Uses the repository's get all method to retrieve list of all employees
                empList = repo.GetAll();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
            //Return the list of employees
            return empList;
        }

        //Adds a new employee to the database
        public int Add(Employee newEmp)
        {
            try
            {
                //Uses the repository's add method to add the employee passed as a parameter
                repo.Add(newEmp);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
            //Return the Id of the new employee to verify the add was successful
            return newEmp.Id;
        }

        //Updates an employee in the database
        public UpdateStatus Update(Employee updateEmp)
        {
            //Set the enum to failed at first
            UpdateStatus opStatus = UpdateStatus.Failed;
            try
            {
                //Set the enum to the return of the repository's update method. If the update was successful, the enum will be 'Ok'
                opStatus = repo.Update(updateEmp);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
            return opStatus;
        }

        //Delete an employee based on the Id
        public int Delete(int id)
        {
            int employeesDeleted = -1;
            try
            {
                //use the repository's delete method to delete the employee with the Id that matches the parameter
                employeesDeleted = repo.Delete(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
            return employeesDeleted;
        }
    }
}



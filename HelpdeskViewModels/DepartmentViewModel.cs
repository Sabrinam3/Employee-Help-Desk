/*
 * Class Name: Department View Model
 * Coder: Sabrina Tessier
 * Purpose: Allows the webpage to interact with the data access layer for the divisions
 */
 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelpdeskDAL;
using System.Reflection;

namespace HelpdeskViewModels
{
    public class DepartmentViewModel
    {
        private DepartmentModel model_;
        public string Name { get; set; }
        public int Id { get; set; }
        public string Timer { get; set; }

        //Constructs a DepartmentModel object for every instance of DepartmentViewModel created.
        public DepartmentViewModel() { model_ = new DepartmentModel(); }

        //Gets all departments and creates a DepartmentViewModel object with all the same properties. 
        //Returns a list of DepartmentViewModel objects
        public List<DepartmentViewModel> GetAll()
        {
            List<DepartmentViewModel> allVms = new List<DepartmentViewModel>();
            try
            {
                List<Department> allDepartments = model_.GetAll();
                foreach (Department dep in allDepartments)
                {
                    DepartmentViewModel depVm = new DepartmentViewModel();
                    depVm.Name = dep.DepartmentName;
                    depVm.Id = dep.Id;
                    depVm.Timer = Convert.ToBase64String(dep.Timer);
                    allVms.Add(depVm);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
            return allVms;
        }//end GetAll
    }
}

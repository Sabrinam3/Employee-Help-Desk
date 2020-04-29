/*
 * Class Name: DepartmentModel
 * Coder: Sabrina Tessier
 * Purpose: the data access layer for the department data. This class interacts directly with the data in the database via methods in the repository.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace HelpdeskDAL
{
    public class DepartmentModel
    {
        //Create an instance of repository object so this class can use those methods
        IRepository<Department> repo;
        public DepartmentModel() { repo = new HelpDeskRepository<Department>(); }

        //Retrieves a list of all departments from the database
        public List<Department> GetAll()
        {
            List<Department> deps = null;
            try
            {
                deps = repo.GetAll();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
            return deps;
        }
    }
}

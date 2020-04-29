/*
 * Class Name: EmployeeModel
 * Coder: Sabrina Tessier
 * Purpose: the data access layer for the problem data. This class interacts directly with the data in the database via methods in the repository.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace HelpdeskDAL
{
   public class ProblemModel
    {
        //Create an instance of repository object
        IRepository<Problem> repo;
        //Constructor
        public ProblemModel() { repo = new HelpDeskRepository<Problem>(); }

        //Retrieves list of all problems from the database
        public List<Problem> GetAll()
        {
            List<Problem> probs = null;
            try
            {
                probs = repo.GetAll();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
            return probs;
        }

        //Returns an instance of problem based on description 
        public Problem GetByDescription(string description)
        {
            List<Problem> problem = null;

            //Uses the repository's method to retrieve the problem with the description matching the parameter argument
            try
            {
                problem = repo.GetByExpression(prob => prob.Description == description);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
            //Retrieves the first match in the list
            return problem.FirstOrDefault();
        }

    }
}

//Class Name: ProblemViewModel
//Coder: Sabrina Tessier
//Purpose: Acts as a go-between for the controllers and the Problem Model(data access layer) and
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
   public class ProblemViewModel
    {
        private ProblemModel _model;
      //Properties of the ProblemViewModel class -> same as Problem.cs attributes
      public int Id { get; set; }
      public string Description { get; set; }
    
        //Constructor
        public ProblemViewModel() { _model = new ProblemModel(); }

        //Retrieve a list of problem view models constructed from all the problems in the database
        public List<ProblemViewModel> GetAll()
        {
            List<ProblemViewModel> allVms = new List<ProblemViewModel>();
            try
            {
                //Get a list of all problems
                List<Problem> problems = _model.GetAll();
                foreach(Problem prb in problems)
                {
                    ProblemViewModel pvm = new ProblemViewModel();
                    pvm.Id = prb.Id;
                    pvm.Description = prb.Description;
                    allVms.Add(pvm);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
            return allVms;
        }

        //Get By Description 
        public void GetByDescription()
        {
            try
            {
                Problem prb = _model.GetByDescription(Description);
                //Set the remaining properties
                Id = prb.Id;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
        }

    }
}

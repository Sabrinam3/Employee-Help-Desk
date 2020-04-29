/*
 * Class Name: CallModel
 * Coder: Sabrina Tessier
 * Purpose: the data access layer for the Call data. This class interacts directly with the data in the database via methods in the repository.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace HelpdeskDAL
{
   public class CallModel
    {
        //Instance of repository
        IRepository<Call> repo;

        //Constructor
        public CallModel() { repo = new HelpDeskRepository<Call>(); }

        //Retrieves an instance of a call by the Id
        public Call GetById(int id)
        {
            List<Call> calls = null;
            try
            {
                //Uses the repository's method to retrieve the call with the Id matching the parameter argument
                calls = repo.GetByExpression(call => call.Id == id);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
            return calls.FirstOrDefault();
        }

        //Retrieves list of all calls
        public List<Call> GetAll()
        {
            List<Call> callList = new List<Call>();
            try
            {
                //Uses the repository's get all method to retrieve list of all calls
                callList = repo.GetAll();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
            //Return list of calls
            return callList;
        }

        //Adds a new call into the database
        public int Add(Call newCall)
        {
            try
            {
                //Uses the repository's add method to add the call passed as a parameter
                repo.Add(newCall);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
            //Return the Id of the new call to verify the add was successful
            return newCall.Id;
        }

        //Updates a call in the database
        public UpdateStatus Update(Call updateCall)
        {
            //Set the enum to failed at first
            UpdateStatus opStatus = UpdateStatus.Failed;
            try
            {
                //Set the enum to the return of the repository's update method. If the update was successful, the enum will be 'Ok'
                opStatus = repo.Update(updateCall);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
            return opStatus;
        }

        //Delete a call based on the Id
        public int Delete(int id)
        {
            int callsDeleted = -1;
            try
            {
                //use the repository's delete method to delete the call with the Id that matches the parameter
                callsDeleted = repo.Delete(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
            return callsDeleted;
        }
    }
}

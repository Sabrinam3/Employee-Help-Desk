//Class Name: CallViewModel
//Coder: Sabrina Tessier
//Purpose: Acts as a go-between for the controllers and the Call Model(data access layer) and
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
    public class CallViewModel
    {
        private CallModel _model;
        //Properties of CallViewModel class -> same as properties in call.cs
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int ProblemId { get; set; }
        public string EmployeeName { get; set; }
        public string ProblemDescription { get; set; }
        public string TechName { get; set; }
        public int TechId { get; set; }
        public DateTime DateOpened { get; set; }
        public DateTime? DateClosed { get; set; }
        public bool OpenStatus { get; set; }
        public string Notes { get; set; }
        public string Timer { get; set; }

        //Constructor
        public CallViewModel() { _model = new CallModel(); }

        //Retrieves a list of CallViewModels based on a list of all Calls in the database
        public List<CallViewModel> GetAll()
        {
            List<CallViewModel> allVms = new List<CallViewModel>();
            try
            {
                //Get a list of all the calls and use each call in that list to create a view model and then return the list of view model objects
                List<Call> allCalls = _model.GetAll();
                foreach (Call call in allCalls)
                {
                    CallViewModel cvm = new CallViewModel();
                    cvm.Id = call.Id;
                    //Sub properties
                    cvm.EmployeeName = call.Employee.LastName;
                    cvm.ProblemDescription = call.Problem.Description;
                    cvm.TechName = call.Employee1.LastName;

                    cvm.EmployeeId = call.EmployeeId;
                    cvm.ProblemId = call.ProblemId;
                    cvm.TechId = call.TechId;
                    cvm.DateOpened = call.DateOpened;
                    cvm.DateClosed = call.DateClosed;
                    cvm.OpenStatus = call.OpenStatus;
                    cvm.Notes = call.Notes;
                    cvm.Timer = Convert.ToBase64String(call.Timer);
                    allVms.Add(cvm);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
            return allVms;
        }//end getAll

        //Add a call
        public void Add()
        {
            Id = -1;
            try
            {
                //Create a call object from the attributes of the view model object that this methid is called on
                Call call = new Call();
                call.EmployeeId = EmployeeId;
                call.ProblemId = ProblemId;
                call.TechId = TechId;
                call.DateOpened = DateOpened;
                call.DateClosed = DateClosed;
                call.OpenStatus = OpenStatus;
                call.Notes = Notes;

                //use the call model's add method to add the call into the database
                Id = _model.Add(call);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
        }

        //Update a call by creating a new call from the traits of the view model object this method is called on.
        public int Update()
        {
            UpdateStatus opStatus = UpdateStatus.Failed;
            try
            {
                Call call = new Call();
                call.Id = Id;
                call.EmployeeId = EmployeeId;
                call.ProblemId = ProblemId;
                call.TechId = TechId;
                call.DateOpened = DateOpened;
                call.DateClosed = DateClosed;
                call.OpenStatus = OpenStatus;
                call.Notes = Notes;
                call.Timer = Convert.FromBase64String(Timer);
                //Use the employee model's update method to send changes back to the database
                opStatus = _model.Update(call);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
            return Convert.ToInt16(opStatus);
        }

        //Delete a call
        public int Delete()
        {
            int callsDeleted = 1;
            try
            {
                callsDeleted = _model.Delete(Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }

            return callsDeleted;
        }

        //Retrieve an instance of call using Id property
        public void GetById()
        {
            try
            {
                //Retrieve the call that has the same Id as the view model object this method was called on
                Call call = _model.GetById(Id);
                //Set the remaining properties of the view model object 
                EmployeeId = call.EmployeeId;
                ProblemId = call.ProblemId;
                TechId = call.TechId;
                DateOpened = call.DateOpened;
                DateClosed = call.DateClosed;
                OpenStatus = call.OpenStatus;
                Notes = call.Notes;
                Timer = Convert.ToBase64String(call.Timer);         
            }
            catch (NullReferenceException nex)
            {
                Console.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + nex.Message);
                throw nex;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
        }
    }
}

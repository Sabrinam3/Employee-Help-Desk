/*
 * Class Name: HelpDeskRepository
 * Coder: Sabrina Tessier
 * Purpose: Creates the implementation of the IRepository interface. Sets the template parameter to be 'HelpDeskEntity' and implements the CRUD methods
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Infrastructure;
using System.Reflection;

namespace HelpdeskDAL
{
    //HelpDeskRepository implements IRepository and sets 'T' to be HelpDeskEntity
    public class HelpDeskRepository<T> : IRepository<T> where T : HelpDeskEntity
    {
        private HelpdeskContext dbContext = null;

        public HelpDeskRepository(HelpdeskContext context = null)
        {
            //Creates an instance of Helpdesk context to allow these methods to interact directly with the database
            dbContext = context != null ? context : new HelpdeskContext();
        }
        public T Add(T entity)
        {
            //Add the parameter object to the database
            dbContext.Set<T>().Add(entity);
            //Save changes to finalize the add
            dbContext.SaveChanges();
            //Return the newly added entity
            return entity;
        }

        public int Delete(int id)
        {
            //Retrieve the entity with the Id matching the parameter
            T currentEntity = GetByExpression(emp => emp.Id == id).FirstOrDefault();
            //Remove the entity from the database
            dbContext.Set<T>().Remove(currentEntity);
            //Return the result of calling Save Changes
            return dbContext.SaveChanges();
        }

        public List<T> GetAll()
        {
            //Retrieve all of the 'T' entity and convert into List format
            return dbContext.Set<T>().ToList();
        }

        public List<T> GetByExpression(Expression<Func<T, bool>> lambdaExp)
        {
            //Use a lambda expression to retrieve entity(s) from the database that match the expression argument
            return dbContext.Set<T>().Where(lambdaExp).ToList();
        }

        public UpdateStatus Update(T entity)
        {
            UpdateStatus opStatus = UpdateStatus.Failed;
            try
            {
                //Retrieve the entity that matches the Id of the parameter argument
                HelpDeskEntity currentEntity = GetByExpression(emp => emp.Id == entity.Id).FirstOrDefault();
                //Set the timer property to be the same as the parameter entity's timer
                dbContext.Entry(currentEntity).OriginalValues["Timer"] = entity.Timer;
                //Set the currentEntity's properties to be the same as the parameter entity's properties
                dbContext.Entry(currentEntity).CurrentValues.SetValues(entity);
                //If Save Changes is successful, set the enum value to 'Ok'
                if (dbContext.SaveChanges() == 1)
                    opStatus = UpdateStatus.Ok;
            }catch(DbUpdateConcurrencyException dbx)
            {
                //Concurrent updates are happening. Second user has stale data
                opStatus = UpdateStatus.Stale;
                Console.WriteLine("Problem in " + MethodBase.GetCurrentMethod().Name + dbx.Message);
            }
            catch(Exception ex)
            {
                Console.WriteLine("Problem in " + MethodBase.GetCurrentMethod().Name + ex.Message);
            }
            //Return the enum value
            return opStatus;
        }
    }
}

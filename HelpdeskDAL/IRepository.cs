/*
 * Interface Name: IRepository
 * Coder: Sabrina Tessier
 * Purpose: an interface for the CRUD methods that will be implemented. Allows for a generalization of the type. 'T' is a templated parameter
 *              which means any class in this solution could implement these methods
 */
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HelpdeskDAL
{
   public  interface IRepository<T>
    {
        List<T> GetAll();
        List<T> GetByExpression(Expression<Func<T, bool>> lambdaExp);
        T Add(T entity);
        UpdateStatus Update(T entity);
        int Delete(int id);
    }
}

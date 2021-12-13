using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VendingMachineWPF.DataAccess.Abstract;

namespace VendingMachineWPF.DataAccess.concrete
{
    public class GenericRepository<T> : IRepository<T> where T : class
    {
        protected context.DataClassesDataContext data = new context.DataClassesDataContext(); 

        public void Delete(T entity)
        {
            data.GetTable<T>().DeleteOnSubmit(entity);
            SumbitChanges();
        }

        public IQueryable<T> Get(Expression<Func<T, bool>> expression)=> data.GetTable<T>().Where(expression);

        public IQueryable<T> GetAll() => from entity in data.GetTable<T>() select entity;

        public void Insert(T entity)
        {
            data.GetTable<T>().InsertOnSubmit(entity);
            SumbitChanges();
        }

        public void SumbitChanges()
        {
            data.SubmitChanges();
        }

        public void Update(T entity)
        {
            throw new NotImplementedException();
        }
    }
}

using DataAccessLayer.Abstract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Concrete.EfCore
{
    public class EfCoreGenericRepository<T, TContext> : IRepository<T> 
        where T : class
        where TContext : DbContext, new()
    {
        public void Create(T t)
        {
            using (var context = new TContext())
            {
                context.Set<T>().Add(t);
                context.SaveChanges();
            }
        }

        public void Delete(T t)
        {
            using (var context = new TContext())
            {
                context.Set<T>().Remove(t);
                context.SaveChanges();
            }
        }

        public List<T> GetAll()
        {
            using (var context = new TContext())
            {
              return  context.Set<T>().ToList();
               
            }
        }

        public T GetById(int id)
        {
            using (var context = new TContext())
            {
                return context.Set<T>().Find(id);

            }
        }

        public virtual void Update(T t)
        {
            using (var context = new TContext())
            {
                context.Entry(t).State = EntityState.Modified;    
                context.SaveChanges();
            }
        }
    }
}

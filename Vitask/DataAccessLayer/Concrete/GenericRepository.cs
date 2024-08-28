using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Abstract;
using DataAccessLayer.Context;

namespace DataAccessLayer.Concrete
{
    public class GenericRepository<T> : IGenericDal<T> where T : class
    {
        public void Delete(T t)
        {
            using(VitaskContext context = new VitaskContext())
            {
                context.Remove(t);
                context.SaveChanges();
            }
        }

        public List<T> GetAll()
        {
            using (VitaskContext context = new VitaskContext())
            {
                return context.Set<T>().ToList();
            }
        }

        public T GetById(int id)
        {
            using(VitaskContext context = new VitaskContext())
            {
                return context.Set<T>().Find(id);
            }
        }

        public void Insert(T t)
        {
            using(VitaskContext context = new VitaskContext())
            {
                context.Add(t);
                context.SaveChanges();
            }
        }

        public void Update(T t)
        {
            using (VitaskContext context = new VitaskContext())
            {
                context.Update(t);
                context.SaveChanges();
            }
        }
    }
}

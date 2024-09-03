using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Abstract
{
    public interface IGenericDal<T> where T : class
    {

        T GetById(int id);

        T Insert(T t);

        void Update(T t);

        void Delete(T t);

        List<T> GetAll();



    }
}

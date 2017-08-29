using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OurFirstApi.DataAccess
{
    public interface IRepository<T>
    {
        List<T> GetAll();
        T Get(int id);
        void Delete(int id);
        void Update(T entityToUpdate);
    }
}
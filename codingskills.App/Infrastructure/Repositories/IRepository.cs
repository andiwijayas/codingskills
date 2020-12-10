using System.Collections.Generic;
using System.Linq;

namespace codingskills.App.Infrastructure.Repositories
{
    public interface IRepository<T> 
        where T:class 
    {
        void InitLocation(string location);
        IEnumerable<T> GetAll();
        void Delete(T entity);
        void Add(T entity);
        void SaveChanges();
        void RemoveAll();
    }
}
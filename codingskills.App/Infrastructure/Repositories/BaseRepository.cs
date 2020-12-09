using CsvHelper.Configuration;
using System.Collections.Generic;
using System.Linq;
using DeepEqual.Syntax;
using codingskills.App.Infrastructure.CsvSets;

namespace codingskills.App.Infrastructure.Repositories
{
    public abstract class BaseRepository<T, TMap> : IRepository<T>
        where T : class
        where TMap : ClassMap<T>
    {
        private IList<T> _items;
        private readonly ICsvSet<T, TMap> csvSet;

        public BaseRepository(ICsvSet<T,TMap> csvSet)
        {
            this.csvSet = csvSet;
        }

        private IList<T> Items 
        {
            get => _items ?? (_items = this.csvSet.ReadCsv());  
        }
        public BaseRepository() {}
                
        public void Add(T entity)
        {
            Items.Add(entity);
        }

        public void Delete(T entity)
        {
            _items = Items.Where(x => !x.IsDeepEqual(entity)).ToList();
        }

        public IEnumerable<T> GetAll()
        {
            return Items.AsEnumerable();
        }

        public void SaveChanges()
        {
            this.csvSet.SaveCsv(Items);
        }

        public void RemoveAll()
        {
            _items = new List<T>();
        }
    }
}
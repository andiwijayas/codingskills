using codingskills.App.Domain.Models;
using codingskills.App.Infrastructure.Repositories;

namespace codingskills.App.Infrastructure.UnitOfWorks
{
    public class MergedCatalogUnitOfWork : IMergedCatalogUnitOfWork
    {
        public MergedCatalogUnitOfWork(IRepository<MergedCatalog> mergedCatalogRepository)
        {
            MergedCatalogs = mergedCatalogRepository;
        }
        public IRepository<MergedCatalog> MergedCatalogs { get; private set; }

        public void Commit()
        {
            MergedCatalogs.SaveChanges();
        }
    }
}
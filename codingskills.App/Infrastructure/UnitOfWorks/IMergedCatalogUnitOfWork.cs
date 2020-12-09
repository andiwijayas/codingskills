using codingskills.App.Domain.Models;
using codingskills.App.Infrastructure.Repositories;

namespace codingskills.App.Infrastructure.UnitOfWorks
{
    public interface IMergedCatalogUnitOfWork : IUnitOfWork
    {
        IRepository<MergedCatalog> MergedCatalogs { get; }
    }
}
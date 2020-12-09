using codingskills.App.Domain.Models;
using codingskills.App.Infrastructure.CsvSets;
using codingskills.App.Infrastructure.Mappers;

namespace codingskills.App.Infrastructure.Repositories
{
  public class SupplierRepository
      : BaseRepository<Supplier, SupplierMap>,
        IRepository<Supplier>
  {
    public SupplierRepository(ICsvSet<Supplier, SupplierMap> csvSet) 
      : base(csvSet)
    {
    }
  }
}
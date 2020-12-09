using System.Collections.Generic;
using codingskills.App.Domain.Models;
using codingskills.App.Infrastructure.CsvSets;
using codingskills.App.Infrastructure.Mappers;

namespace codingskills.App.Infrastructure.Repositories
{
  public class CatalogRepository
                  : BaseRepository<Catalog, CatalogMap>,
                    IRepository<Catalog>
  {
    public CatalogRepository(ICsvSet<Catalog, CatalogMap> csvSet) 
      : base(csvSet)
    {
    }
  }
}
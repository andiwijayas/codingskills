using System.Collections.Generic;
using codingskills.App.Domain.Models;
using codingskills.App.Infrastructure.CsvSets;
using codingskills.App.Infrastructure.Mappers;

namespace codingskills.App.Infrastructure.Repositories
{
    public class MergedCatalogRepository
        : BaseRepository<MergedCatalog, MergedCatalogMap>,
          IRepository<MergedCatalog>
    {
        public MergedCatalogRepository(ICsvSet<MergedCatalog, MergedCatalogMap> csvSet) : base(csvSet)
        {
        }
    }
}
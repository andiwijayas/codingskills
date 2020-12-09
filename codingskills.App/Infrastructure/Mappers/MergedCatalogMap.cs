using codingskills.App.Domain.Models;
using CsvHelper.Configuration;

namespace codingskills.App.Infrastructure.Mappers
{
    public class MergedCatalogMap : ClassMap<MergedCatalog>
    {
        public MergedCatalogMap()
        {
            Map(x => x.SKU).Name("SKU");
            Map(x => x.Description).Name("Description");
            Map(x => x.Source).Name("Source");
            Map(x => x.Barcodes).Ignore();
        }
    }
}
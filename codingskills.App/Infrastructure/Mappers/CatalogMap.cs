using codingskills.App.Domain.Models;
using CsvHelper.Configuration;

namespace codingskills.App.Infrastructure.Mappers
{
    public class CatalogMap : ClassMap<Catalog>
    {
        public CatalogMap()
        {
            Map(x => x.SKU).Name("SKU");
            Map(x => x.Description).Name("Description");
        }
    }
}
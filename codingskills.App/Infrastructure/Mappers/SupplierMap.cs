using codingskills.App.Domain.Models;
using CsvHelper.Configuration;

namespace codingskills.App.Infrastructure.Mappers
{
    public class SupplierMap : ClassMap<Supplier>
    {
        public SupplierMap()
        {
            Map(x => x.Id).Name("ID");
            Map(x => x.Name).Name("Name");
        }
    }
}
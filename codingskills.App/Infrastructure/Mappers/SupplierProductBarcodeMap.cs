using codingskills.App.Domain.Models;
using CsvHelper.Configuration;

namespace codingskills.App.Infrastructure.Mappers
{
    public class SupplierProductBarcodeMap : ClassMap<SupplierProductBarcode>
    {
        public SupplierProductBarcodeMap()
        {
            Map(x => x.SupplierId).Name("SupplierID");
            Map(x => x.SKU).Name("SKU");
            Map(x => x.Barcode).Name("Barcode");
        }
    }
}
using codingskills.App.Domain.Models;
using codingskills.App.Infrastructure.CsvSets;
using codingskills.App.Infrastructure.Mappers;

namespace codingskills.App.Infrastructure.Repositories
{
    public class SupplierProductBarcodeRepository
        : BaseRepository<SupplierProductBarcode, SupplierProductBarcodeMap>,
          IRepository<SupplierProductBarcode>
    {

        public SupplierProductBarcodeRepository(ICsvSet<SupplierProductBarcode, SupplierProductBarcodeMap> csvSet)
            : base(csvSet)
        {
           
        }
    }
}
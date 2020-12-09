using codingskills.App.Domain.Models;
using codingskills.App.Infrastructure.Repositories;

namespace codingskills.App.Infrastructure.UnitOfWorks
{
    public class CompanyUnitOfWork: ICompanyUnitOfWork
    {
        public CompanyUnitOfWork(
            CatalogRepository catalogARepository, 
            SupplierRepository supplierARepository, 
            SupplierProductBarcodeRepository barcodeARepository)
        {
            Catalogs = catalogARepository;
            Suppliers = supplierARepository;
            Barcodes = barcodeARepository;
        }

        public IRepository<Supplier> Suppliers { get; private set; }
        public IRepository<Catalog> Catalogs { get; private set; }
        public IRepository<SupplierProductBarcode> Barcodes { get; private set; }

        public void Commit() 
        {
            this.Suppliers.SaveChanges();
            this.Catalogs.SaveChanges();
            this.Barcodes.SaveChanges();
        }
    }
}
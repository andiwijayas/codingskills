using codingskills.App.Domain.Models;
using codingskills.App.Infrastructure.UnitOfWorks;
using System.Collections.Generic;
using System.Linq;

namespace codingskills.App.Application.Services
{
    public class BauService : IBauService
    {
        public BauService(
            ICompanyUnitOfWork companyA,
            ICompanyUnitOfWork companyB
        )
        {
            CompanyA = companyA;
            CompanyB = companyB;
        }

        public ICompanyUnitOfWork CompanyA { get; }
        public ICompanyUnitOfWork CompanyB { get; }

        private ICompanyUnitOfWork GetCompany(string name)
        {
            return (name.Equals("A"))
                    ? CompanyA : CompanyB;
        }

        public void AddNewCatalogItemToCompany(Catalog catalog, string name)
        {
            var catalogs = GetCompany(name).Catalogs;

            if (catalogs.GetAll().Any(x => x.SKU == catalog.SKU))
                throw new DuplicateValueException($"SKU {catalog.SKU} already exists");

            catalogs.Add(catalog);
            catalogs.SaveChanges();
        }

        public void RemoveCatalogItemFromCompany(Catalog catalog, string name)
        {
            var company = GetCompany(name);
            var catalogs = company.Catalogs;

            if (!catalogs.GetAll().Any(x => x.SKU == catalog.SKU))
                throw new NotFoundException($"SKU {catalog.SKU} not found");

            catalogs.Delete(catalog);
            catalogs.SaveChanges();

            var barcodes = company.Barcodes;
            foreach (SupplierProductBarcode b in barcodes.GetAll())
            {
                if (b.SKU == catalog.SKU)
                    b.SKU = null;
            }
            barcodes.SaveChanges();
        }

        public void AddNewSupplierAndASetOfBarcodesToCompany(
                Catalog catalog,
                Supplier supplier,
                IEnumerable<string> newBarcodes,
                string name
            )
        {
            var company = GetCompany(name);
            var catalogs = company.Catalogs;

            if (!catalogs.GetAll().Any(x => x.SKU == catalog.SKU))
                throw new NotFoundException($"SKU {catalog.SKU} not found");
            
            //add supplier
            var suppliers = company.Suppliers;
            if (suppliers.GetAll().Any(x => x.Id == supplier.Id))
                throw new DuplicateValueException($"Supplier {supplier.Id} is already exists");
            suppliers.Add(supplier);
            suppliers.SaveChanges();
            
            //add barcodes
            var barcodes = company.Barcodes;
            foreach (var b in newBarcodes)
            {
                var newBarcode = new SupplierProductBarcode
                {
                    SupplierId = supplier.Id,
                    Barcode = b,
                    SKU = catalog.SKU,
                };
                barcodes.Add(newBarcode);
            }
            barcodes.SaveChanges();
        }
    }
}
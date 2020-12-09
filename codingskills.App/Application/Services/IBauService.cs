using System.Collections.Generic;
using codingskills.App.Domain.Models;
using codingskills.App.Infrastructure.UnitOfWorks;

namespace codingskills.App.Application.Services
{
    public interface IBauService
    {
        ICompanyUnitOfWork CompanyA { get; }
        ICompanyUnitOfWork CompanyB { get; }

        void AddNewCatalogItemToCompany(Catalog catalog, string name);
        void AddNewSupplierAndASetOfBarcodesToCompany(Catalog catalog, Supplier supplier, IEnumerable<string> newBarcodes, string name);
        void RemoveCatalogItemFromCompany(Catalog catalog, string name);
    }
}
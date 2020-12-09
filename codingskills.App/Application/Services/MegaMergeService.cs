using codingskills.App.Domain.Models;
using codingskills.App.Infrastructure.UnitOfWorks;
using ConsoleDump;
using System.Collections.Generic;
using System.Linq;

namespace codingskills.App.Application.Services
{
    public class MegaMergeService : IMegaMergeService
    {
        public MegaMergeService(
            ICompanyUnitOfWork companyA,
            ICompanyUnitOfWork companyB,
            IMergedCatalogUnitOfWork mergedCatalog
        )
        {
            CompanyA = companyA;
            CompanyB = companyB;
            MergedCatalog = mergedCatalog;
        }

        public ICompanyUnitOfWork CompanyA { get; }
        public ICompanyUnitOfWork CompanyB { get; }
        public IMergedCatalogUnitOfWork MergedCatalog { get; }

        public IEnumerable<MergedCatalog> MergeCatalogs()
        {
            var catalogsA = GetMergedCatalogs(CompanyA, "A");
            var catalogsB = GetMergedCatalogs(CompanyB, "B");
            var filteredCatalogsB = new List<MergedCatalog>();
            foreach (var itemB in catalogsB)
            {
                var matchedA = catalogsA.FirstOrDefault(itemA =>
                    itemA.Description.Equals(itemB.Description, System.StringComparison.InvariantCultureIgnoreCase) ||
                    itemB.Description.StartsWith(itemA.Description, System.StringComparison.InvariantCultureIgnoreCase)
                );

                if (matchedA != null &&
                    matchedA.Barcodes.Intersect(itemB.Barcodes).Count() > 0)
                    continue;

                filteredCatalogsB.Add(itemB);
            }
            var mergedCatalogs = catalogsA
                                    .Union(filteredCatalogsB)
                                    .OrderBy(x => x.Description)
                                    .ToList();

            MergedCatalog.MergedCatalogs.RemoveAll();
            
            foreach (var c in mergedCatalogs)
                MergedCatalog.MergedCatalogs.Add(c);
            
            MergedCatalog.MergedCatalogs.SaveChanges();

            return mergedCatalogs;
        }

        private IEnumerable<MergedCatalog> GetMergedCatalogs(ICompanyUnitOfWork company, string sourceName)
        {
            var mergedCatalogs =
                company
                    .Catalogs
                    .GetAll()
                    .Select(x =>
                        new MergedCatalog
                        {
                            SKU = x.SKU,
                            Description = x.Description,
                            Source = sourceName
                        })
                    .Distinct()
                    .ToList();
            var barcodes = company
                                .Barcodes
                                .GetAll()
                                .ToList();
            mergedCatalogs.ForEach(x =>
            {
                x.Barcodes =  barcodes
                                .Where(y => y.SKU == x.SKU)
                                .Select(y => y.Barcode)
                                .ToList();
            });

            return mergedCatalogs;
        }
    }
}
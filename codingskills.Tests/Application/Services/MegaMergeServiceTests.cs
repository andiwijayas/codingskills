using codingskills.App.Application.Services;
using codingskills.App.Domain.Models;
using codingskills.App.Infrastructure.UnitOfWorks;
using DeepEqual.Syntax;
using NSubstitute;
using Shouldly;
using System.Collections.Generic;
using System.Linq;
using TestStack.BDDfy;
using Xunit;

namespace codingskills.Tests.Application.Services
{
    public class MegaMergeServiceTests
    {
        private ICompanyUnitOfWork _companyA;
        private ICompanyUnitOfWork _companyB;
        private IMergedCatalogUnitOfWork _mergedCatalog;
        private MegaMergeService _megaMergeService;
        private IList<MergedCatalog> _result;

        public MegaMergeServiceTests()
        {
            _companyA = Substitute.For<ICompanyUnitOfWork>();
            _companyB = Substitute.For<ICompanyUnitOfWork>();
            _mergedCatalog = Substitute.For<IMergedCatalogUnitOfWork>();
            _megaMergeService = new MegaMergeService(_companyA, _companyB, _mergedCatalog);
        }

        [Theory]
        [ClassData(typeof(MegaMergeServiceTestDataGenerator))]
        public void ItShouldMergeCatalogAAndCatalogB(
            IList<Catalog> catalogsA,
            IList<Supplier> suppliersA,
            IList<SupplierProductBarcode> barcodesA,
            IList<Catalog> catalogsB,
            IList<Supplier> suppliersB,
            IList<SupplierProductBarcode> barcodesB,
            IList<MergedCatalog> expectedResult
        )
        {
            this.Given(x => x.GivenCatalogAWithValue(catalogsA))
                    .And(x => x.GivenSupplierAWithValue(suppliersA))
                    .And(x => x.GivenBarcodeAWithValue(barcodesA))
                    .And(x => x.GivenCatalogBWithValue(catalogsB))
                    .And(x => x.GivenSupplierBWithValue(suppliersB))
                    .And(x => x.GivenBarcodeBWithValue(barcodesB))
                .When(x => x.WhenMergeCatalogsIsCalled())
                .Then(x => x.ThenTheMergedCatalogShouldBe(expectedResult))
                .BDDfy();
        }

        private void GivenCatalogAWithValue(IList<Catalog> catalogs)
        {
            _companyA.Catalogs.GetAll().Returns(catalogs);
        }

        private void GivenSupplierAWithValue(IList<Supplier> suppliers)
        {
            _companyA.Suppliers.GetAll().Returns(suppliers);
        }

        private void GivenBarcodeAWithValue(IList<SupplierProductBarcode> barcodes)
        {
            _companyA.Barcodes.GetAll().Returns(barcodes);
        }

        private void GivenCatalogBWithValue(IList<Catalog> catalogs)
        {
            _companyB.Catalogs.GetAll().Returns(catalogs);
        }

        private void GivenSupplierBWithValue(IList<Supplier> suppliers)
        {
            _companyB.Suppliers.GetAll().Returns(suppliers);
        }

        private void GivenBarcodeBWithValue(IList<SupplierProductBarcode> barcodes)
        {
            _companyB.Barcodes.GetAll().Returns(barcodes);
        }

        private void WhenMergeCatalogsIsCalled()
        {
            _result = _megaMergeService.MergeCatalogs().ToList();
        }

        private void ThenTheMergedCatalogShouldBe(IList<MergedCatalog> expected)
        {
            expected.ShouldDeepEqual(_result);
        }

    }
}
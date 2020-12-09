using codingskills.App.Application.Services;
using codingskills.App.Domain.Models;
using codingskills.App.Infrastructure.UnitOfWorks;
using DeepEqual.Syntax;
using NSubstitute;
using Shouldly;
using System.Collections.Generic;
using TestStack.BDDfy;
using Xunit;

namespace codingskills.Tests.Application.Services
{
    public class BauServiceTests
    {
        private ICompanyUnitOfWork _companyA;
        private ICompanyUnitOfWork _companyB;
        private BauService _bauService;
        private DuplicateValueException _duplicateException;
        private NotFoundException _notFoundException;

        public BauServiceTests()
        {
            _companyA = Substitute.For<ICompanyUnitOfWork>();
            _companyB = Substitute.For<ICompanyUnitOfWork>();
            _bauService = new BauService(_companyA, _companyB);
        }

        [Fact]
        public void ItShouldBeAbleToAddSupplierToCompanyA()
        {
            var catalogA = new List<Catalog>
                {
                    new Catalog { SKU = "SKUA", Description="DescriptionA" }
                };
            var catalogToAdd = new Catalog { SKU = "SKUB", Description = "DescriptionB" };

            this.Given(x => x.GivenIntialCatalogA(catalogA))
                .When(x => x.WhenANewCatalogIsAddedIntoCompanyA(catalogToAdd))
                .Then(x => x.ThenCompanyAAddMethodShouldBeCalledWIthParam(catalogToAdd))
                    .And(x => x.ThenCompanyASaveChangesShouldBeCalled())
                .BDDfy();
        }

        [Fact]
        public void ItShouldThrowDuplicateValueExceptionWhenExistingSKUExistsInCompanyA()
        {
            var catalogA = new List<Catalog>
                {
                    new Catalog { SKU = "SKUA", Description="DescriptionA" }
                };
            var catalogToAdd = new Catalog { SKU = "SKUA", Description = "DescriptionB" };

            this.Given(x => x.GivenIntialCatalogA(catalogA))
                .When(x => x.WhenANewCatalogIsAddedIntoCompanyA(catalogToAdd))
                .Then(x => x.ThenItThrowDuplicateValueException())
                .BDDfy();
        }

        [Fact]
        public void ItShouldBeAbleToRemoveSupplierFromCompanyA()
        {
            var catalogA = new List<Catalog>
                {
                    new Catalog { SKU = "SKUA", Description="DescriptionA" }
                };
            var catalogToRemove = new Catalog { SKU = "SKUA", Description = "DescriptionA" };

            this.Given(x => x.GivenIntialCatalogA(catalogA))
                .When(x => x.WhenACatalogIsRemovedFromCompanyA(catalogToRemove))
                .Then(x => x.ThenCompanyADeleteMethodShouldBeCalledWIthParam(catalogToRemove))
                    .And(x => x.ThenCompanyASaveChangesShouldBeCalled())
                .BDDfy();
        }

        [Fact]
        public void ItShouldThrowNotFoundExceptionWhenRemoveSupplierFromCompanyA()
        {
            var catalogA = new List<Catalog>
                {
                    new Catalog { SKU = "SKUA", Description="DescriptionA" }
                };
            var catalogToRemove = new Catalog { SKU = "SKUB", Description = "DescriptionA" };

            this.Given(x => x.GivenIntialCatalogA(catalogA))
                .When(x => x.WhenACatalogIsRemovedFromCompanyA(catalogToRemove))
                .Then(x => x.ThenItShouldThrowNotFoundException())
                .BDDfy();
        }

        [Fact]
        public void ItShouldBeAbleToAddSupplierAndBarcodeOnCompanyBProduct()
        {
            var catalogB = new List<Catalog>
                {
                    new Catalog { SKU = "SKUA", Description="DescriptionA" }
                };
            var supplierToAdd = new Supplier
            {
                Id = "SupplierId",
                Name = "SupplierName"
            };

            var barcodesToAdd = new List<string>
            {
                "111", "222"
            };

            this.Given(x => x.GivenIntialCatalogB(catalogB))
                .When(x => x.WhenSupplierAndBarcodesIsAddedToACatalogOnCompanyB(catalogB[0], supplierToAdd, barcodesToAdd))
                .Then(x => x.ThenItShouldCallAddSupplierOnCompanyB(supplierToAdd))
                    .And(x => x.ThenItShouldCallAddBarcodeOnCompanyB(barcodesToAdd))
                .BDDfy();

        }

        [Fact]
        public void ItShouldThrowNotFoundExceptionWhenAddingSupplierAndBarcodeOnCompanyBProduct()
        {
            var catalogB = new List<Catalog>
                {
                    new Catalog { SKU = "SKUA", Description="DescriptionA" }
                };
            var supplierToAdd = new Supplier
            {
                Id = "SupplierId",
                Name = "SupplierName"
            };

            var barcodesToAdd = new List<string>
            {
                "111", "222"
            };

            var catalogToFind = new Catalog { SKU = "SKUZ", Description = "Desc" };
            this.Given(x => x.GivenIntialCatalogB(catalogB))
                .When(x => x.WhenSupplierAndBarcodesIsAddedToACatalogOnCompanyB(catalogToFind, supplierToAdd, barcodesToAdd))
                .Then(x => x.ThenItShouldThrowNotFoundException())
                .BDDfy();

        }

        private void GivenIntialCatalogA(IList<Catalog> catalogs)
        {
            _companyA.Catalogs.GetAll().Returns(catalogs);
        }

        private void GivenIntialCatalogB(IList<Catalog> catalogs)
        {
            _companyB.Catalogs.GetAll().Returns(catalogs);
        }

        private void WhenANewCatalogIsAddedIntoCompanyA(Catalog catalog)
        {
            try
            {
                _bauService.AddNewCatalogItemToCompany(catalog, "A");
            } 
            catch (DuplicateValueException ex)
            {
                _duplicateException = ex;
            }
        }

        private void WhenACatalogIsRemovedFromCompanyA(Catalog catalog)
        {
            try
            {
                _bauService.RemoveCatalogItemFromCompany(catalog, "A");
            }
            catch (NotFoundException ex)
            {
                _notFoundException = ex;
            }
        }

        private void WhenSupplierAndBarcodesIsAddedToACatalogOnCompanyB(
            Catalog catalog, Supplier supplier, IList<string> barcodes)
        {
            try
            {
                _bauService.AddNewSupplierAndASetOfBarcodesToCompany(catalog, supplier, barcodes, "B");
            }
            catch (NotFoundException ex)
            {
                _notFoundException = ex;
            }
        }

        private void ThenItShouldThrowNotFoundException()
        {
            _notFoundException.ShouldNotBeNull();
        }

        private void ThenItThrowDuplicateValueException()
        {
            _duplicateException.ShouldNotBeNull();
        }

        private void ThenCompanyADeleteMethodShouldBeCalledWIthParam(Catalog catalog)
        {
            _companyA.Catalogs.Received(1).Delete(Arg.Is<Catalog>(x => x.IsDeepEqual(catalog)));
        }

        private void ThenCompanyAAddMethodShouldBeCalledWIthParam(Catalog catalog)
        {
            _companyA.Catalogs.Received(1).Add(Arg.Is<Catalog>(x => x.IsDeepEqual(catalog)));
        }

        private void ThenCompanyASaveChangesShouldBeCalled()
        {
            _companyA.Catalogs.Received(1).SaveChanges();
        }

        private void ThenItShouldCallAddSupplierOnCompanyB(Supplier supplierToAdd)
        {
            _companyB.Suppliers.Received(1).Add(Arg.Is<Supplier>(x => x.IsDeepEqual(supplierToAdd)));
            _companyB.Suppliers.Received(1).SaveChanges();
        }

        private void ThenItShouldCallAddBarcodeOnCompanyB(IList<string> barcodes)
        {
            _companyB.Barcodes.Received(barcodes.Count).Add(Arg.Any<SupplierProductBarcode>());
        }
    }
}
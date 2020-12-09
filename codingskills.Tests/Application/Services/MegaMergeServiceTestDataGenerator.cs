using codingskills.App.Domain.Models;
using System.Collections;
using System.Collections.Generic;

namespace codingskills.Tests.Application.Services
{
    public class MegaMergeServiceTestDataGenerator : IEnumerable<object[]>
    {
        private readonly List<object[]> _data = new List<object[]>
        {
            new object[] 
            {
                //company A
                new List<Catalog>
                {
                    new Catalog { SKU = "SKUA", Description="DescriptionA" }
                },
                new List<Supplier>(),
                new List<SupplierProductBarcode>(),
                //company B
                new List<Catalog>
                {
                    new Catalog { SKU = "SKUB", Description="DescriptionB" }
                },
                new List<Supplier>(),
                new List<SupplierProductBarcode>(),
                //result
                new List<MergedCatalog>
                {
                    new MergedCatalog { SKU = "SKUA", Description="DescriptionA", Source = "A", Barcodes = new List<string>()},
                    new MergedCatalog { SKU = "SKUB", Description="DescriptionB", Source = "B", Barcodes = new List<string>()},
                }
            },
            //same catalog's description, different barcodes
            new object[]
            {
                //company A
                new List<Catalog>
                {
                    new Catalog { SKU = "SKUA", Description="DescriptionA" }
                },
                new List<Supplier>
                {
                    new Supplier { Id = "SupplierIdA", Name = "SupplierNameA"}
                },
                new List<SupplierProductBarcode>
                {
                    new SupplierProductBarcode { SupplierId ="SupplierIdA", SKU = "SKUA", Barcode ="1234"}
                },
                //company B
                new List<Catalog>
                {
                    new Catalog { SKU = "SKUB", Description="DescriptionA" }
                },
                new List<Supplier>()
                {
                    new Supplier { Id = "SupplierIdB", Name = "SupplierNameB"}
                },
                new List<SupplierProductBarcode>()
                {
                    new SupplierProductBarcode { SupplierId ="SupplierIdB", SKU = "SKUB", Barcode ="4567"}
                },
                //result
                new List<MergedCatalog>
                {
                    new MergedCatalog { SKU = "SKUA", Description="DescriptionA", Source = "A", Barcodes = new List<string>{ "1234" } },
                    new MergedCatalog { SKU = "SKUB", Description="DescriptionA", Source = "B", Barcodes = new List<string>{ "4567" } }
                }
            },
            //same catalog's description, overlapped barcode(s)
            new object[]
            {
                //company A
                new List<Catalog>
                {
                    new Catalog { SKU = "SKUA", Description="DescriptionA" }
                },
                new List<Supplier>
                {
                    new Supplier { Id = "SupplierIdA", Name = "SupplierNameA"}
                },
                new List<SupplierProductBarcode>
                {
                    new SupplierProductBarcode { SupplierId ="SupplierIdA", SKU = "SKUA", Barcode ="1234"}
                },
                //company B
                new List<Catalog>
                {
                    new Catalog { SKU = "SKUB", Description="DescriptionA" }
                },
                new List<Supplier>()
                {
                    new Supplier { Id = "SupplierIdB", Name = "SupplierNameB"}
                },
                new List<SupplierProductBarcode>()
                {
                    new SupplierProductBarcode { SupplierId ="SupplierIdB", SKU = "SKUB", Barcode ="1234"}
                },
                //result
                new List<MergedCatalog>
                {
                    new MergedCatalog { SKU = "SKUA", Description="DescriptionA", Source = "A", Barcodes = new List<string>{ "1234" } },
                }
            },

        };

        public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
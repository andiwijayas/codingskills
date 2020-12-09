using System.Collections.Generic;
using System;
namespace codingskills.App.Domain.Models
{
    public class Company
    {
        private Company() {}
        public Company(
            string name,
            IList<SupplierProductBarcode> supplierProductBarcodes,
            IList<Supplier> suppliers,
            IList<Catalog> catalogs
        ) 
        {
            Suppliers = suppliers;
            Catalogs = catalogs;
            SupplierProductBarcodes = supplierProductBarcodes;
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }
        public string Name { get; set; }
        public IList<Supplier> Suppliers { get; set; }
        public IList<Catalog> Catalogs { get; set; }
        public IList<SupplierProductBarcode> SupplierProductBarcodes { get; private set; }
    }
}
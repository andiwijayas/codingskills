using System;

namespace codingskills.App.Domain.Models
{
    public class SupplierProductBarcode 
    {
        public string SupplierId { get; set; }
        public string SKU { get; set; }
        public string Barcode { get; set; }
        public Supplier Supplier { get; set; }
        public Catalog Catalog { get; set; }
        

    }
}
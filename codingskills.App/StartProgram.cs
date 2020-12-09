using codingskills.App.Application.Services;
using codingskills.App.Domain.Models;
using codingskills.App.Infrastructure.UnitOfWorks;
using ConsoleDump;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace codingskills
{
    class StartProgram
    {
        private MegaMergeService megaMergeService;
        private BauService bauService;

        public StartProgram(MegaMergeService megaMergeService, BauService bauService)
        {
            this.megaMergeService = megaMergeService;
            this.bauService = bauService;
        }

        private void DoMerge()
        {
            try
            {
                this.megaMergeService.MergeCatalogs();
                this.megaMergeService
                    .MergedCatalog                    
                    .MergedCatalogs
                    .GetAll()
                    .Select(
                        x => new
                            {
                                x.SKU,
                                x.Description,
                                x.Source
                            })
                    .Dump();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Environment.Exit(0);
            }
        }

        private char PrintMenu() 
        {
            Console.WriteLine("Please select:");
            Console.WriteLine("1. Add Product in Catalog A");
            Console.WriteLine("2. Remove Product From Catalog A");
            Console.WriteLine("3. Add New Supplier & New Barcode for Existing Product in Catalog B");
            Console.WriteLine("4. Exit");            
            Console.Write("Please press '1','2','3', or '4' followed by 'Enter' button:");
            var input = Console.ReadLine();
            return input[0];
        } 

        public void Start(string[] args) 
        {
            char selectedMenu = '0';
            do 
            {
                try {
                    Console.Clear();
                    DoMerge();
                    selectedMenu = PrintMenu();
                    switch (selectedMenu)
                    {
                        case '1': AddNewProductInCatalogA(); break;
                        case '2': RemoveAnExistingProductFromCatalogA();  break;
                        case '3': AssignNewSupplierAndBarcodesToAProductFromCatalogB();  break;
                        case '4': Environment.Exit(0);  break;
                    }
                } 
                catch (Exception ex) {
                    Console.WriteLine(ex.Message);
                    Console.ReadLine();
                }
            } while (selectedMenu != '4');
        }

        private void AssignNewSupplierAndBarcodesToAProductFromCatalogB()
        {
            var catalog = SelectCatalogFromCompany(this.bauService.CompanyB);
            if (catalog == null)
            {
                Console.WriteLine("Cannot find SKU");
                Console.ReadLine();
                return;
            } 
            var supplier = InputNewSupplier(this.bauService.CompanyB);

            var barcodes = InputNewBarcodes();
            this.bauService.AddNewSupplierAndASetOfBarcodesToCompany(catalog, supplier, barcodes, "B");
        }

        private IEnumerable<string> InputNewBarcodes()
        {
            Console.WriteLine("Input new barcodes (Enter twice to end):");
            var barcodes = new List<string>();
            string input;
            do 
            {
                input = Console.ReadLine();
                Regex.Replace(input, @"\s+", string.Empty);
                if (!string.IsNullOrWhiteSpace(input)) barcodes.Add(input);
            } while (!string.IsNullOrWhiteSpace(input));
            return barcodes;
        }

        private Catalog SelectCatalogFromCompany(ICompanyUnitOfWork company)
        {
            Console.WriteLine("These are the Catalog B content:");
            var catalogs = company.Catalogs.GetAll();
            catalogs.Dump();
            Console.Write("Please type SKU no:");
            var sku = Console.ReadLine();
            return catalogs.FirstOrDefault( x=> x.SKU == sku);
        }

        private Supplier InputNewSupplier(ICompanyUnitOfWork company)
        {
            Console.Write("Supplier Name: ");
            var name = Console.ReadLine();
            var maxSupplierId = Convert.ToInt32(company.Suppliers.GetAll().Max(x=>x.Id));
            var supplierId = (maxSupplierId + 1).ToString("00000");
            var supplier = new Supplier{
                    Id = supplierId, 
                    Name = name
            };

            return supplier;
        }

        private void RemoveAnExistingProductFromCatalogA()
        {
            var catalog = SelectCatalogFromCompany(this.bauService.CompanyA);
            if (catalog == null)
            {
                Console.WriteLine("Cannot find SKU");
                return;
            } 
            this.bauService.RemoveCatalogItemFromCompany(catalog, "A");
        }

        private void AddNewProductInCatalogA()
        {
            Console.WriteLine("Add new product");
            Console.Write("Enter SKU: ");
            var sku = Console.ReadLine();
            Console.Write("Enter Description: ");
            var description = Console.ReadLine();
            var catalog = new Catalog {
                SKU = sku,
                Description =description
            };
            this.bauService.AddNewCatalogItemToCompany(catalog, "A");
        }
    }
}

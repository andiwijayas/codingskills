using codingskills.App.Application.Services;
using codingskills.App.Domain.Models;
using codingskills.App.Infrastructure.CsvSets;
using codingskills.App.Infrastructure.Mappers;
using codingskills.App.Infrastructure.Repositories;
using codingskills.App.Infrastructure.UnitOfWorks;
using System;

namespace codingskills
{

    class Program
    {
        static ICompanyUnitOfWork CreateCompanyUnitOfWork(string path, string name)
        {
            var catalogRepository = new CatalogRepository(
                new CsvSet<Catalog, CatalogMap>($"{path}\\catalog{name}.csv")
            );
            var supplierRepository = new SupplierRepository(
                new CsvSet<Supplier, SupplierMap>($"{path}\\suppliers{name}.csv")
            );
            var barcodeRepository = new SupplierProductBarcodeRepository(
                new CsvSet<SupplierProductBarcode, SupplierProductBarcodeMap>($"{path}\\barcodes{name}.csv")
            );
            return new CompanyUnitOfWork(
                catalogRepository,
                supplierRepository,
                barcodeRepository
            );

        }

        static MergedCatalogUnitOfWork CreateMergedCatalogUnitOfWork(string path)
        {
            var mergedCatalogRepository = new MergedCatalogRepository(
                new CsvSet<MergedCatalog, MergedCatalogMap>($"{path}\\result_output2.csv")
            );

            return new MergedCatalogUnitOfWork(
                mergedCatalogRepository
            );
        }

        static void Main(string[] args)
        {
            if (args.Length != 2) 
            {
                var exeName = AppDomain.CurrentDomain.FriendlyName;
                Console.WriteLine($"to use: {exeName} [inputPath] [outputPath]");
                Environment.Exit(0);                
            }
            var inputPath = args[0];
            var outputPath = args[1];

            var companyAUnitOfWork = CreateCompanyUnitOfWork(inputPath, "A");
            var companyBUnitOfWork = CreateCompanyUnitOfWork(inputPath, "B");

            var mergedCatalogUnitOfWork = CreateMergedCatalogUnitOfWork(outputPath);


            var megaMergeService = new MegaMergeService(
                companyAUnitOfWork,
                companyBUnitOfWork,
                mergedCatalogUnitOfWork
            );

            var bauService = new BauService(
                companyAUnitOfWork,
                companyBUnitOfWork
            );

            new StartProgram(megaMergeService, bauService).Start(args);
        }

    }
}

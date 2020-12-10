using Autofac;
using codingskills.App.Application.Services;
using codingskills.App.Domain.Models;
using codingskills.App.Infrastructure.CsvSets;
using codingskills.App.Infrastructure.Repositories;
using codingskills.App.Infrastructure.UnitOfWorks;
using System;

namespace codingskills
{

    class Program
    {
        static IContainer BuildContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<CatalogRepository>().As<IRepository<Catalog>>();
            builder.RegisterType<SupplierRepository>().As<IRepository<Supplier>>();
            builder.RegisterType<SupplierProductBarcodeRepository>().As<IRepository<SupplierProductBarcode>>();
            builder.RegisterType<MergedCatalogRepository>().As<IRepository<MergedCatalog>>();
            builder.RegisterGeneric(typeof(CsvSet<,>)).As(typeof(ICsvSet<,>));
            builder.RegisterType<CompanyUnitOfWork>().AsImplementedInterfaces();
            builder.RegisterType<MergedCatalogUnitOfWork>().AsImplementedInterfaces();

            return builder.Build();
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
            var container = BuildContainer();

            var companyAUnitOfWork = container.Resolve<ICompanyUnitOfWork>();
            companyAUnitOfWork.Catalogs.InitLocation($"{inputPath}\\catalogA.csv");
            companyAUnitOfWork.Suppliers.InitLocation($"{inputPath}\\suppliersA.csv");
            companyAUnitOfWork.Barcodes.InitLocation($"{inputPath}\\barcodesA.csv");

            var companyBUnitOfWork = container.Resolve<ICompanyUnitOfWork>();
            companyBUnitOfWork.Catalogs.InitLocation($"{inputPath}\\catalogB.csv");
            companyBUnitOfWork.Suppliers.InitLocation($"{inputPath}\\suppliersB.csv");
            companyBUnitOfWork.Barcodes.InitLocation($"{inputPath}\\barcodesB.csv");

            var mergedCatalogUnitOfWork = container.Resolve<IMergedCatalogUnitOfWork>();
            mergedCatalogUnitOfWork.MergedCatalogs.InitLocation($"{inputPath}\\result_output2.csv");

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

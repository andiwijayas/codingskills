using codingskills.App.Domain.Models;
using codingskills.App.Infrastructure.Repositories;

namespace codingskills.App.Infrastructure.UnitOfWorks
{
    public interface ICompanyUnitOfWork : IUnitOfWork 
    {
        IRepository<Supplier> Suppliers { get;  }
        IRepository<Catalog> Catalogs { get;  }
        IRepository<SupplierProductBarcode> Barcodes { get;  }
    }
}
using System.Collections.Generic;
using codingskills.App.Domain.Models;
using codingskills.App.Infrastructure.UnitOfWorks;

namespace codingskills.App.Application.Services
{
    public interface IMegaMergeService
    {
        ICompanyUnitOfWork CompanyA { get; }
        ICompanyUnitOfWork CompanyB { get; }
        IMergedCatalogUnitOfWork MergedCatalog { get; }

        IEnumerable<MergedCatalog> MergeCatalogs();
    }
}
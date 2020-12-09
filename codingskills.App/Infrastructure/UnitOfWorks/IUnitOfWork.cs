using System.Collections.Generic;

namespace codingskills.App.Infrastructure.UnitOfWorks
{
    public interface IUnitOfWork 
    {
        void Commit();
    }
}
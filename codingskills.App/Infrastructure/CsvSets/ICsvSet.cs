using System.Collections.Generic;
using CsvHelper.Configuration;

namespace codingskills.App.Infrastructure.CsvSets
{
    public interface ICsvSet<T, TMap>
        where T : class
        where TMap : ClassMap<T>
    {
        IList<T> ReadCsv();
        void SaveCsv(IList<T> objects);
    }
}
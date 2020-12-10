using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;

namespace codingskills.App.Infrastructure.CsvSets
{
    public class CsvSet<T, TMap> : ICsvSet<T, TMap>
        where T : class
        where TMap : ClassMap<T>
    {
        private string location;

        public CsvSet()
        {
        }
        
        public void InitLocation(string location)
        {
            this.location = location;
        }

        public IList<T> ReadCsv()
        {
            try 
            {
                using (var reader = new StreamReader(this.location))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    csv.Configuration.RegisterClassMap<TMap>();
                    return csv.GetRecords<T>().ToList();
                }
            } 
            catch 
            {
                return new List<T>();
            }
        }

        public void SaveCsv(IList<T> objects)
        {
            using (StreamWriter writer = new StreamWriter(this.location, false, new UTF8Encoding(true)))
            using (CsvWriter csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.Configuration.RegisterClassMap<TMap>();

                csv.WriteHeader<T>();
                csv.NextRecord();
                foreach (T o in objects)
                {
                    csv.WriteRecord(o);
                    csv.NextRecord();
                }
            }
        }

    }
}
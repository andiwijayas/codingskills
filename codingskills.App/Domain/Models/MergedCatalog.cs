using System;
using System.Collections.Generic;

namespace codingskills.App.Domain.Models
{
    public class MergedCatalog : IEquatable<MergedCatalog>
    {
        public string SKU { get; set; }
        public string Description { get; set; }
        public IList<string> Barcodes { get; set; }
        public string Source { get; set; }

        public override bool Equals(object obj)
        {
           return this.Equals(obj as MergedCatalog);
        }

        public bool Equals(MergedCatalog o)
        {
           if (Object.ReferenceEquals(o, null))
               return false;
            
           if (Object.ReferenceEquals(this, o))
               return false;

           return this.Description.Equals(o.Description, StringComparison.InvariantCultureIgnoreCase)
               && this.SKU.Equals(o.SKU, StringComparison.InvariantCultureIgnoreCase);
            
        }

        public override int GetHashCode()
        {
           var hashCode = 352033288;
           hashCode = hashCode * -1521134295 + Description.GetHashCode();
           hashCode = hashCode * -1521134295 + SKU.GetHashCode();
           return hashCode;
        }

        public static bool operator == (MergedCatalog a, MergedCatalog b)
        {
           if (Object.ReferenceEquals(a, null))
               return (Object.ReferenceEquals(b, null));
           return a.Equals(b);
        }
        public static bool operator != (MergedCatalog a, MergedCatalog b)
        {
           return !(a == b);
        }

        public override string ToString()
        {
           return base.ToString();
        }
    }
}
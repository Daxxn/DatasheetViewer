using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatasheetViewer.Models
{
   public class TagComparer : IEqualityComparer<Tag>
   {
      public bool Equals(Tag x, Tag y)
      {
         return x.Name == y.Name;
      }

      public int GetHashCode([DisallowNull] Tag obj)
      {
         return obj.GetHashCode();
      }
   }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatasheetViewer.Events
{
   public class UpdateDatasheetEvent : EventArgs
   {
      public string FilePath { get; init; }

      public UpdateDatasheetEvent(string filePath)
      {
         FilePath = filePath;
      }
   }
}

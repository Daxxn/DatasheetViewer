using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatasheetViewer.Models
{
   public interface ITag
   {
      ObservableCollection<Tag> Tags { get; set; }
      void RemoveTagEvent(object sender, Tag tag);
      void CreateTag(string name);
   }
}

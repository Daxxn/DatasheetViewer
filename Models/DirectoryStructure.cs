using MVVMLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatasheetViewer.Models
{
   public class DirectoryStructure : Model
   {
      #region - Fields & Properties
      private ObservableCollection<Folder> _folders = new();
      #endregion

      #region - Constructors
      public DirectoryStructure() { }
      #endregion

      #region - Methods

      #endregion

      #region - Full Properties
      public ObservableCollection<Folder> Folders
      {
         get { return _folders; }
         set
         {
            _folders = value;
            OnPropertyChanged();
         }
      }
      #endregion
   }
}

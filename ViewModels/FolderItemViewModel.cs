using DatasheetViewer.Models;
using MVVMLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatasheetViewer.ViewModels
{
   public class FolderItemViewModel : ViewModel
   {
      #region - Fields & Properties
      private Folder _folder;
      #endregion

      #region - Constructors
      public FolderItemViewModel() { }
      #endregion

      #region - Methods

      #endregion

      #region - Full Properties
      public Folder Folder
      {
         get { return _folder; }
         set
         {
            _folder = value;
            OnPropertyChanged();
         }
      }
      #endregion
   }
}

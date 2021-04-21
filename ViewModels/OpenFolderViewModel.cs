using MVVMLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using DatasheetViewer.Models;
using System.Windows;

namespace DatasheetViewer.ViewModels
{
   public class OpenFolderViewModel : ViewModel
   {
      #region - Fields & Properties
      private DirectoryStructure _drives = new();
      #endregion

      #region - Constructors
      public OpenFolderViewModel() { }
      #endregion

      #region - Methods
      public void OnLoad()
      {
         try
         {
            string[] drives = Directory.GetLogicalDrives();

            foreach (var drive in drives)
            {
               Drives.Folders.Add(new(drive));
            }
         }
         catch (Exception)
         {
            MessageBox.Show("Could not load Drives.");
         }
      }
      #endregion

      #region - Full Properties

      public DirectoryStructure Drives
      {
         get { return _drives; }
         set
         {
            _drives = value;
            OnPropertyChanged();
         }
      }
      #endregion
   }
}

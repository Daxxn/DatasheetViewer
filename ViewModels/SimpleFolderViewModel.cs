using DatasheetViewer.Models;
using MVVMLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DatasheetViewer.ViewModels
{
   public class SimpleFolderViewModel : ViewModel
   {
      #region - Fields & Properties
      public static event EventHandler<string> SelectFolderEvent;
      public event EventHandler CloseDialogEvent;
      private ObservableCollection<string> _previousPaths = new();
      private Settings _appSettings = Settings.AppSettings;

      public Command SaveCmd { get; init; }
      #endregion

      #region - Constructors
      public SimpleFolderViewModel()
      {
         SaveCmd = new Command((o) => SaveFolder());
      }
      #endregion

      #region - Methods
      public void SaveFolder()
      {
         if (Directory.Exists(AppSettings.LastUsedPath))
         {
            SendPath();
         }
         else
         {
            var result = MessageBox.Show($"Could not find folder.\n{AppSettings.LastUsedPath}", "Oops", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
               CloseDialogEvent?.Invoke(this, null);
            }
         }
      }

      private void SendPath()
      {
         if (AppSettings.LastUsedPath is not null) PreviousPaths.Add(AppSettings.LastUsedPath);
         SelectFolderEvent?.Invoke(this, AppSettings.LastUsedPath);
         CloseDialogEvent?.Invoke(this, null);
      }
      #endregion

      #region - Full Properties
      public Settings AppSettings
      {
         get { return _appSettings; }
         set
         {
            _appSettings = value;
            OnPropertyChanged();
         }
      }

      public ObservableCollection<string> PreviousPaths
      {
         get { return _previousPaths; }
         set
         {
            _previousPaths = value;
            if (_previousPaths.Count > 10)
            {
               _previousPaths.RemoveAt(0);
            }
            OnPropertyChanged();
         }
      }
      #endregion
   }
}

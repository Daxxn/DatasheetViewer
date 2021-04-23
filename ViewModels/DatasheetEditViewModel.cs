using DatasheetViewer.Models;
using MVVMLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace DatasheetViewer.ViewModels
{
   public class DatasheetEditViewModel : ViewModel
   {
      #region - Fields & Properties
      private ObservableCollection<DatasheetEditModel> _datasheets;
      public static event EventHandler<List<Datasheet>> EndEditEvent;

      private string _rootDir;
      private string _savepath;

      public Command SaveCmd { get; init; }
      #endregion

      #region - Constructors
      public DatasheetEditViewModel()
      {
         SaveCmd = new(o => EndEdit());
         MainViewModel.StartEditEvent += MainViewModel_StartEditEvent;
      }
      #endregion

      #region - Methods
      private void MainViewModel_StartEditEvent(object sender, DatasheetMetaFile e)
      {
         RootDir = e.RootDirectory;
         Datasheets = new(e.Datasheets.Select(ds => new DatasheetEditModel(ds)));
      }

      public void EndEdit()
      {
         EndEditEvent?.Invoke(this, Datasheets.Select(ds => new Datasheet(ds)).ToList());
      }
      #endregion

      #region - Full Properties
      public ObservableCollection<DatasheetEditModel> Datasheets
      {
         get { return _datasheets; }
         set
         {
            _datasheets = value;
            OnPropertyChanged();
         }
      }

      public string RootDir
      {
         get { return _rootDir; }
         set
         {
            _rootDir = value;
            OnPropertyChanged();
         }
      }

      public string SavePath
      {
         get { return _savepath; }
         set
         {
            _savepath = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(SaveFileName));
         }
      }

      public string SaveFileName
      {
         get => Path.GetFileName(SavePath);
      }
      #endregion
   }
}

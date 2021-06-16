using DatasheetViewer.Models;
using MVVMLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;

namespace DatasheetViewer.ViewModels
{
   public class DatasheetEditViewModel : ViewModel
   {
      #region - Fields & Properties
      private ObservableCollection<DatasheetEditModel> _datasheets;
      public static event EventHandler<List<Datasheet>> EndEditEvent;

      private string _rootDir;
      private string _savepath;

      private DatasheetEditModel _selectedDatasheet;

      public Command SaveCmd { get; init; }
      public Command DeleteCmd { get; init; }
      #endregion

      #region - Constructors
      public DatasheetEditViewModel()
      {
         SaveCmd = new(o => SaveEdits());
         DeleteCmd = new(o => DeleteDatasheet());
         MainViewModel.StartEditEvent += MainViewModel_StartEditEvent;
      }
      #endregion

      #region - Methods
      private void MainViewModel_StartEditEvent(object sender, DatasheetMetaFile e)
      {
         RootDir = e.RootDirectory;
         Datasheets = new(e.Datasheets.Select(ds => new DatasheetEditModel(ds)));
      }

      public void DeleteDatasheet()
      {
         MessageBoxResult result = MessageBox.Show(
            "Are u sure man?\nThis removes all data from the metafile.\n\nNote: this also removes the original file.",
            "Wait!...",
            MessageBoxButton.OKCancel
            );

         if (result == MessageBoxResult.OK)
         {
            try
            {
               File.Delete(SelectedDatasheet.FileName);
               Datasheets.Remove(SelectedDatasheet);
            }
            catch (Exception e)
            {
               MessageBox.Show(e.Message, "Error");
            }
         }
         SaveEdits();
      }

      public void SaveEdits()
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

      public DatasheetEditModel SelectedDatasheet
      {
         get { return _selectedDatasheet; }
         set
         {
            _selectedDatasheet = value;
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

using DatasheetViewer.Events;
using DatasheetViewer.Models;
using JsonReaderLibrary;
using Microsoft.Win32;
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
   public class MainViewModel : ViewModel
   {
      #region - Fields & Properties
      public static readonly List<FileDialogCustomPlace> Custom = new()
      {
         new FileDialogCustomPlace(@"B:\Electrical\Datasheets"),
      };
      public static SimpleFolderViewModel FolderDialogVM { get; private set; } = new SimpleFolderViewModel();

      private Stream _doc;

      private DatasheetMetaFile _datasheetFile = new();
      private ObservableCollection<Datasheet> _searchResults;
      private Datasheet _selectedDatasheet;

      private string _searchText;

      #region Command Bindings
      public Command OpenFolderCmd { get; init; }
      public Command SaveDatasheetMetafileCmd { get; init; }
      public Command SearchCmd { get; init; }
      #endregion
      #endregion

      #region - Constructors
      public MainViewModel()
      {
         SaveDatasheetMetafileCmd = new(o => SaveDatasheetMetafile());
         SearchCmd = new(o => Search());

         SimpleFolderViewModel.SelectFolderEvent += SimpleFolderViewModel_SelectFolderEvent;
      }
      #endregion

      #region - Methods
      public void Search()
      {
         SearchResults = new(
            DatasheetFile.Datasheets.Where(
               (d) => d.MatchSearch(SearchText)
            )
         );
      }

      public void OpenDatasheetMetafile()
      {
         try
         {
            if (File.Exists(DatasheetFile.SavePath))
            {
               DatasheetFile = JsonReader.OpenJsonFile<DatasheetMetaFile>(DatasheetFile.SavePath);
            }
            else
            {
               DatasheetFile = DatasheetMetaFile.NewMetafile(DatasheetFile.RootDirectory);
            }
         }
         catch (Exception e)
         {
            MessageBox.Show(e.Message, "Error");
         }
      }

      public void SaveDatasheetMetafile()
      {
         try
         {
            JsonReader.SaveJsonFile(DatasheetFile.SavePath, DatasheetFile, true);
         }
         catch (Exception e)
         {
            MessageBox.Show(e.Message, "Error");
         }
      }

      private void SimpleFolderViewModel_SelectFolderEvent(object sender, string e)
      {
         DatasheetFile.RootDirectory = e;
         OpenDatasheetMetafile();
      }
      #endregion

      #region - Full Properties
      public Stream Document
      {
         get { return _doc; }
         set
         {
            _doc = value;
            OnPropertyChanged();
         }
      }

      public DatasheetMetaFile DatasheetFile
      {
         get { return _datasheetFile; }
         set
         {
            _datasheetFile = value;
            OnPropertyChanged();
         }
      }

      public Datasheet SelectedDatasheet
      {
         get { return _selectedDatasheet; }
         set
         {
            _selectedDatasheet = value;
            if (value is not null)
            {
               if (File.Exists(value.FilePath))
               {
                  Document = new FileStream(value.FilePath, FileMode.Open);
               }
            }
            OnPropertyChanged();
         }
      }

      #region Search Props
      public string SearchText
      {
         get { return _searchText; }
         set
         {
            _searchText = value;
            OnPropertyChanged();
         }
      }

      public ObservableCollection<Datasheet> SearchResults
      {
         get { return _searchResults; }
         set
         {
            _searchResults = value;
            OnPropertyChanged();
            OnPropertyChanged(
               new string[]{ nameof(SearchActive), nameof(SearchListVisible), nameof(SearchListNotVisible)}
            );
         }
      }

      public bool SearchActive
      {
         get => SearchResults is not null && SearchResults.Count > 0;
      }

      public Visibility SearchListVisible
      {
         get => SearchActive ? Visibility.Visible : Visibility.Collapsed;
      }

      public Visibility SearchListNotVisible
      {
         get => !SearchActive ? Visibility.Visible : Visibility.Collapsed;
      }
      #endregion
      #endregion
   }
}

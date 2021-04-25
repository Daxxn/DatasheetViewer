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
using System.Timers;
using System.Threading.Tasks;
using System.Windows;

namespace DatasheetViewer.ViewModels
{
   public class MainViewModel : ViewModel
   {
      #region - Fields & Properties
      public static event EventHandler<DatasheetMetaFile> StartEditEvent;
      public static readonly List<FileDialogCustomPlace> Custom = new()
      {
         new FileDialogCustomPlace(@"B:\Electrical\Datasheets"),
      };
      public static SimpleFolderViewModel FolderDialogVM { get; private set; } = new();
      public static DatasheetEditViewModel EditVM { get; private set; } = new();

      private Stream _doc;

      private DatasheetMetaFile _datasheetFile = new();
      private ObservableCollection<Datasheet> _searchResults;
      private Datasheet _selectedDatasheet;

      private string _searchText;

      private Tag _selectedTag;
      private ObservableCollection<Tag> _allTags;
      private bool _isFilterTagsAscending;

      private Settings _appSettings = Settings.AppSettings;

      private Timer ScanTimer;

      #region Command Bindings
      public Command EditDatasheetsCmd { get; init; }
      public Command OpenFolderCmd { get; init; }
      public Command SaveDatasheetMetafileCmd { get; init; }
      public Command SearchCmd { get; init; }
      public Command ClearSearchCmd { get; init; }
      public Command ClearSelectedTagCmd { get; init; }
      public Command FilterAscendingCmd { get; init; }

      public Command TestCmd { get; init; }
      #endregion
      #endregion

      #region - Constructors
      public MainViewModel()
      {
         if (AppSettings.AutoScan)
         {
            ScanTimer = new()
            {
               Interval = AppSettings.AutoScanInterval > 0 ? AppSettings.AutoScanInterval * 1000 : 10 * 1000,
               AutoReset = true,
               Enabled = AppSettings.AutoScan
            };
            ScanTimer.Elapsed += ScanTimerCallback;
         }
         AllTags = Tag.AllTags;
         SaveDatasheetMetafileCmd = new(o => SaveDatasheetMetafile());
         SearchCmd = new(o => Search());
         ClearSearchCmd = new(o => ClearSearch());
         ClearSelectedTagCmd = new(o => SelectedTag = null);
         FilterAscendingCmd = new(o => SwitchFilter());

         EditDatasheetsCmd = new(o => EditDatasheets());

         TestCmd = new(o => Test());

         SimpleFolderViewModel.SelectFolderEvent += SimpleFolderViewModel_SelectFolderEvent;
         DatasheetEditViewModel.EndEditEvent += DatasheetEditViewModel_EndEditEvent;
      }
      #endregion

      #region - Methods
      public void Test()
      {
         DatasheetFile.ScanDatasheetDir();
      }
      private void EditDatasheets()
      {
         StartEditEvent?.Invoke(this, DatasheetFile);
      }

      private void DatasheetEditViewModel_EndEditEvent(object sender, List<Datasheet> e)
      {
         DatasheetFile.Datasheets = new(e);
      }

      public void Search()
      {
         if (String.IsNullOrEmpty(SearchText)) return;
         SearchResults = new(
            DatasheetFile.Datasheets.Where(
               (d) => d.MatchSearch(SearchText)
            )
         );
      }

      public void ClearSearch()
      {
         SearchResults = null;
         SearchText = null;
      }

      public void ClearSelectedTag()
      {
         SearchResults = null;
         SelectedTag = null;
      }

      public void FilterByTag()
      {
         if (SelectedTag is null) return;

         SearchResults = new(
            DatasheetFile.Datasheets.Where(
               (d) => d.Tags is not null ? d.Tags.Contains(SelectedTag) : false
            )
         );
      }

      public void SwitchFilter()
      {
         IsFilterTagsAscending = !IsFilterTagsAscending;
         if (IsFilterTagsAscending)
         {
            if (SearchResults is not null)
            {
               SearchResults.OrderBy((d) => d.PartName);
            }
            else
            {
               SearchResults = new(DatasheetFile.Datasheets.OrderBy((d) => d.PartName).ToList());
            }
         }
         else
         {
            if (SearchResults is not null)
            {
               SearchResults.OrderByDescending((d) => d.PartName);
            }
            else
            {
               SearchResults = new(DatasheetFile.Datasheets.OrderByDescending((d) => d.PartName).ToList());
            }
         }
      }

      public void OpenDatasheetMetafile()
      {
         try
         {
            if (File.Exists(DatasheetFile.SavePath))
            {
               var saveFile = JsonReader.OpenJsonFile<DatasheetSaveModel>(DatasheetFile.SavePath);
               DatasheetFile = saveFile.MetaFile;
               Tag.AllTags = new(saveFile.Tags);
            }
            else
            {
               DatasheetFile = DatasheetMetaFile.NewMetafile(DatasheetFile.RootDirectory);
            }
            AppSettings.LastUsedPath = DatasheetFile.RootDirectory;
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
            JsonReader.SaveJsonFile(
               DatasheetFile.SavePath,
               new DatasheetSaveModel { MetaFile = DatasheetFile, Tags = Tag.AllTags.ToList() },
               true
            );
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

      public void WindowLoadedEvent(object sender, EventArgs e)
      {
         DatasheetFile.RootDirectory = Settings.AppSettings.LastUsedPath;

         if (Settings.AppSettings.OpenOnStartup)
         {
            OpenDatasheetMetafile();
         }
      }

      public void AutoScanChanged(object sender, EventArgs e)
      {
         if (AppSettings.AutoScan)
         {
            ScanTimer.Start();
         }
         else
         {
            ScanTimer.Stop();
         }
      }

      public void ScanTimerCallback(object sender, ElapsedEventArgs e)
      {
         try
         {
            DatasheetFile.ScanDatasheetDir();
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message, "Error");
         }
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

      public ObservableCollection<Tag> AllTags
      {
         get { return _allTags; }
         set
         {
            _allTags = value;
            OnPropertyChanged();
         }
      }

      public Settings AppSettings
      {
         get { return _appSettings; }
         set
         {
            _appSettings = value;
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
            OnPropertyChanged(nameof(ClearSearchVisible));
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
               nameof(SearchActive), nameof(SearchListVisible), nameof(SearchListNotVisible)
            );
         }
      }

      public Tag SelectedTag
      {
         get { return _selectedTag; }
         set
         {
            _selectedTag = value;
            FilterByTag();
            OnPropertyChanged();
            OnPropertyChanged(nameof(SearchActive), nameof(SearchResults));
         }
      }

      public bool IsFilterTagsAscending
      {
         get { return _isFilterTagsAscending; }
         set
         {
            _isFilterTagsAscending = value;
            OnPropertyChanged();
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

      public Visibility ClearSearchVisible
      {
         get => !String.IsNullOrEmpty(SearchText) ? Visibility.Visible : Visibility.Collapsed;
      }
      #endregion
      #endregion
   }
}

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
using System.Linq.Expressions;
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
      private bool _searchTagsEnable = true;

      private Tag _selectedTag;
      private ObservableCollection<Tag> _allTags;
      private bool _isFilterTagsAscending;

      private Settings _appSettings = Settings.AppSettings;

      private Timer ScanTimer;
      private Timer MessageTimer;

      private Message _message;
      private Visibility _MessageVisible = Visibility.Collapsed;

      #region Command Bindings
      public Command EditDatasheetsCmd { get; init; }
      public Command OpenFolderCmd { get; init; }
      public Command SaveDatasheetMetafileCmd { get; init; }
      public Command UpdateDatasheetsCmd { get; init; }
      public Command SearchCmd { get; init; }
      public Command ClearSelectedCmd { get; init; }
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
         MessageTimer = new()
         {
            Interval = AppSettings.MessageInterval > 0 ? AppSettings.MessageInterval * 1000 : 10 * 1000,
            AutoReset = false,
         };
         MessageTimer.Elapsed += MessageTimer_Elapsed;
         AllTags = Tag.AllTags;
         SaveDatasheetMetafileCmd = new(o => SaveDatasheetMetafile());
         UpdateDatasheetsCmd = new(o => UpdateDatasheets());
         SearchCmd = new(o => Search());
         ClearSelectedCmd = new(o => ClearSelected());
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
         InitMessage("Test Message", MessageType.Error);
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
         if (SearchTagsEnable)
         {
            SearchResults = new(
               DatasheetFile.Datasheets.Where(
                  d =>
                     (
                        d.Tags is not null
                        ? d.Tags.Any( t => t.Name == SearchText)
                        : false
                     ) 
                     ||
                     d.MatchSearch(SearchText)
               ));
         }
         else
         {
            SearchResults = new(
                  DatasheetFile.Datasheets.Where(
                     (d) => d.MatchSearch(SearchText)
                  )
               );
         }
         InitMessage("Search Done...");
      }

      public void ClearSelected()
      {
         SearchResults = null;
         SearchText = null;
         InitMessage("Search Results Cleared...");
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
            InitMessage("Open Completed", MessageType.Finished);
         }
         catch (Exception e)
         {
            InitMessage(e.Message, MessageType.Error);
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
            InitMessage("Save Completed", MessageType.Finished);
         }
         catch (Exception e)
         {
            InitMessage(e.Message, MessageType.Error);
         }
      }

      public void UpdateDatasheets()
      {
         try
         {
            if (DatasheetFile.UpdateDatasheets())
            {
               SaveDatasheetMetafile();
            }
         }
         catch (Exception e)
         {
            InitMessage(e.Message, MessageType.Error);
         }
      }

      public void SelectedTagChanged(object sender, EventArgs e)
      {
         if (DatasheetFile is not null)
         {
            if (SelectedTag is not null)
            {
               SearchResults = new(DatasheetFile.Datasheets.Where((d) => d.Tags != null ? d.Tags.Any(t => SelectedTag.ID == t.ID) : false));
            }
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

      #region Auto Update Methods
      // Not working...
      // And it might not be necessary.
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

      #region Message Methods
      private void MessageTimer_Elapsed(object sender, ElapsedEventArgs e)
      {
         MessageVisible = Visibility.Collapsed;
      }

      private void InitMessage(string message, MessageType type = MessageType.Info)
      {
         Message = new(message, type);
         MessageVisible = Visibility.Visible;
         if (AppSettings.HideMessage)
         {
            MessageTimer.Start();
         }
      }
      #endregion
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

      #region Message Props
      public Message Message
      {
         get { return _message; }
         set
         {
            _message = value;
            OnPropertyChanged();
         }
      }

      public Visibility MessageVisible
      {
         get { return _MessageVisible; }
         set
         {
            _MessageVisible = value;
            OnPropertyChanged();
         }
      }
      #endregion

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

      public bool SearchTagsEnable
      {
         get { return _searchTagsEnable; }
         set
         {
            _searchTagsEnable = value;
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

using MVVMLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatasheetViewer.Models
{
   public class DatasheetMetaFile : Model
   {
      #region - Fields & Properties
      public static readonly string MetadataName = ".metaFile.json";
      private string _rootDir;
      private ObservableCollection<Datasheet> _datasheets = new();
      #endregion

      #region - Constructors
      public DatasheetMetaFile() { }
      #endregion

      #region - Methods
      public static DatasheetMetaFile NewMetafile(string rootDir, SearchOption searchDepth = SearchOption.TopDirectoryOnly)
      {
         DatasheetMetaFile newFile = new();
         newFile.RootDirectory = rootDir;
         string[] files = Directory.GetFiles(rootDir, "*.pdf", searchDepth);

         foreach (var file in files)
         {
            Datasheet newDatasheet = new()
            {
               FilePath = file,
            };
            newDatasheet.ParseFileName();
            newFile.Datasheets.Add(newDatasheet);
         }

         return newFile;
      }

      public void ScanDatasheetDir()
      {
         try
         {
            if (!Directory.Exists(RootDirectory)) return;
            Console.WriteLine("Checking Dir");
            string[] datasheetFiles = Directory.GetFiles(RootDirectory, "*.pdf");
            if (datasheetFiles.Length > Datasheets.Count)
            {
               ObservableCollection<Datasheet> newDatasheets = new(Datasheets);
               var tempDatasheets = datasheetFiles.Where((ds) => !Datasheets.Any((d) => d.FilePath == ds));
               foreach (var dsFile in tempDatasheets)
               {
                  newDatasheets.Add(new(dsFile));
               }
               Datasheets = newDatasheets;
            }
            else if (datasheetFiles.Length < Datasheets.Count)
            {
               var removedDatasheets = Datasheets.Where((ds) => datasheetFiles.Contains(ds.FilePath));
               if (removedDatasheets.Count() < Datasheets.Count)
               {
                  Datasheets = new(removedDatasheets);
               }
            }
         }
         catch (Exception)
         {
            throw;
         }
      }

      public bool UpdateDatasheets()
      {
         bool isChanged = false;
         string[] files = Directory.GetFiles(RootDirectory, "*.pdf", SearchOption.TopDirectoryOnly);
         foreach (var file in files)
         {
            bool isFound = false;
            foreach (var ds in Datasheets)
            {
               if (ds.Equals(file))
               {
                  isFound = true;
                  isChanged = true;
                  break;
               }
            }
            if (!isFound)
            {
               Datasheets.Add(new(file));
            }
         }
         return isChanged;
      }

      public Datasheet SearchDatasheets(string path)
      {
         foreach (var ds in Datasheets)
         {
            if (ds.FilePath == path)
            {
               return ds;
            }
         }
         return null;
      }
      #endregion

      #region - Full Properties
      public string RootDirectory
      {
         get { return _rootDir; }
         set
         {
            _rootDir = value;
            OnPropertyChanged();
         }
      }

      public ObservableCollection<Datasheet> Datasheets
      {
         get { return _datasheets; }
         set
         {
            _datasheets = value;
            OnPropertyChanged();
         }
      }

      public string SavePath
      {
         get
         {
            return Path.Combine(RootDirectory, MetadataName);
         }
      }
      #endregion
   }
}

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

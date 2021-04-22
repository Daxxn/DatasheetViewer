using MVVMLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatasheetViewer.Models
{
   public class DatasheetEditModel : Model
   {
      #region - Fields & Properties
      private string _partName;
      private string _desc;
      private string _tagNames;
      private string _fileName;

      private Datasheet _datasheet;
      #endregion

      #region - Constructors
      public DatasheetEditModel(Datasheet ds)
      {
         _datasheet = ds;
         PartName = ds.PartName;
         Description = ds.Description;
         TagNames = CreateTagString(ds.Tags);
         FileName = ds.FilePath;
      }
      #endregion

      #region - Methods
      public void SaveEdits()
      {
         _datasheet.RenameFile();
      }

      private string CreateTagString(IEnumerable<Tag> tags)
      {
         StringBuilder sb = new();
         foreach (var tag in tags)
         {
            sb.Append($"{tag} ");
         }
         sb.Remove(sb.Length - 1, 1);
         return sb.ToString();
      }
      #endregion

      #region - Full Properties
      public string PartName
      {
         get { return _partName; }
         set
         {
            _partName = value;
            OnPropertyChanged();
         }
      }

      public string Description
      {
         get { return _desc; }
         set
         {
            _desc = value;
            OnPropertyChanged();
         }
      }

      public string FileName
      {
         get { return _fileName; }
         set
         {
            _fileName = value;
            OnPropertyChanged();
         }
      }

      public string TagNames
      {
         get { return _tagNames; }
         set
         {
            _tagNames = value;
            OnPropertyChanged();
         }
      }
      #endregion
   }
}

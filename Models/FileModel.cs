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
   public class FileModel : Model, ITag, IDisposable
   {
      #region - Fields & Properties
      private string _fullPath;
      private ObservableCollection<Tag> _tags;
      #endregion

      #region - Constructors
      public FileModel()
      {
         Tag.RemoveTagEvent += RemoveTagEvent;
      }
      #endregion

      #region - Methods
      public void CreateTag(string name)
      {
         Tag.CreateTag(name);
      }

      public void RemoveTagEvent(object sender, Tag tag)
      {
         Tags.Remove(tag);
      }

      public void Dispose()
      {
         Tag.RemoveTagEvent -= RemoveTagEvent;
         //GC.SuppressFinalize(this);
      }
      #endregion

      #region - Full Properties
      public string Name
      {
         get => File.Exists(FilePath) ? Path.GetFileNameWithoutExtension(FilePath) : null;
      }

      public string FilePath
      {
         get => _fullPath;
         set
         {
            _fullPath = value;
            OnPropertyChanged();
         }
      }

      public ObservableCollection<Tag> Tags
      {
         get => _tags;
         set
         {
            _tags = value;
            OnPropertyChanged();
         }
      }

      public string FileType
      {
         get => File.Exists(FilePath) ? Path.GetExtension(FilePath) : null;
      }
      #endregion
   }
}

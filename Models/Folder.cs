using MVVMLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DatasheetViewer.Models
{
   public class Folder : Model
   {
      #region - Fields & Properties
      private string _name;
      private string _fullPath;
      private ObservableCollection<Folder> _children = new();
      #endregion

      #region - Constructors
      public Folder(string name, string path)
      {
         try
         {
            FullPath = name;
            Name = Path.GetDirectoryName(path);
            Children = new();
            string[] dirs = Directory.GetDirectories(path);
            foreach (var dir in dirs)
            {
               Children.Add(new(Path.GetDirectoryName(path), path));
            }
         }
         catch (Exception)
         { }
      }
      public Folder(string path)
      {
         try
         {
            FullPath = path;
            Name = Path.GetDirectoryName(path);
            Children = new();
            string[] dirs = Directory.GetDirectories(path);
            foreach (var dir in dirs)
            {
               Children.Add(new(dir));
            }
         }
         catch (Exception)
         { }
      }
      #endregion

      #region - Methods

      #endregion

      #region - Full Properties
      public string Name
      {
         get { return _name; }
         set
         {
            _name = value;
            OnPropertyChanged();
         }
      }

      public string FullPath
      {
         get { return _fullPath; }
         set
         {
            _fullPath = value;
            OnPropertyChanged();
         }
      }

      public ObservableCollection<Folder> Children
      {
         get { return _children; }
         set
         {
            _children = value;
            OnPropertyChanged();
         }
      }
      #endregion
   }
}

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
   public class Datasheet : FileModel, ITag, ISearchable
   {
      #region - Fields & Properties
      private static readonly string[] _nameDelimiter = new string[] { " - " };
      private static readonly string[] _tagDelimiters = new string[] { " " };
      private string _partName;
      private string _desc;
      #endregion

      #region - Constructors
      public Datasheet() { }
      public Datasheet(DatasheetEditModel ds)
      {
         Description = ds.Description;
         PartName = ds.PartName;
         FilePath = ds.FileName;
         CreateTags(ds.TagNames);
      }
      #endregion

      #region - Methods
      public void ParseFileName()
      {
         if (String.IsNullOrEmpty(Name)) return;

         string[] split = Name.Split(_nameDelimiter, StringSplitOptions.RemoveEmptyEntries);
         if (split.Length > 0)
         {
            PartName = split[0];
            if (split.Length == 2)
            {
               CreateTags(split[1]);
            }
            else if (split.Length > 2)
            {
               for (int i = 1; i < split.Length; i++)
               {
                  CreateTags(split[i]);
               }
            }
         }
      }

      private void CreateTags(string tagList)
      {
         if (tagList is null) return;

         string[] split = tagList.Split(_tagDelimiters, StringSplitOptions.RemoveEmptyEntries);
         if (split.Length > 1)
         {
            Tags = new();
            foreach (var t in split)
            {
               Tags.Add(Tag.CreateTag(t));
            }
         }
      }

      public bool MatchSearch(string searchText)
      {
         if (String.IsNullOrEmpty(Name)) return false;

         return Name.Contains(searchText) || searchText.Contains(Name) || PartName.Contains(searchText);
      }

      public void RenameFile()
      {
         if (!Name.Contains(PartName))
         {
            StringBuilder sb = new(Name);
            if (Tags is not null && Tags.Count > 0)
            {
               sb.Append(_nameDelimiter[0]);
               for (int i = 0; i < Tags.Count; i++)
               {
                  sb.Append(Tags[i].Name);
                  if (i != Tags.Count - 1)
                  {
                     sb.Append(_tagDelimiters[0]);
                  }
               }
            }

            var dir = Path.GetRelativePath(FilePath, Name);
            File.Move(FilePath, Path.Combine(dir, sb.ToString()));
         }
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
      #endregion
   }
}

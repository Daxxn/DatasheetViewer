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

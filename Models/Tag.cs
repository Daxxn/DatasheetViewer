using MVVMLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatasheetViewer.Models
{
   public class Tag : Model
   {
      #region - Fields & Properties
      public static event EventHandler<Tag> RemoveTagEvent;
      public static ObservableCollection<Tag> AllTags { get; set; } = new();
      private static readonly TagComparer _TagComparer = new();
      private string _name;
      private Guid _id;
      #endregion

      #region - Constructors
      public Tag()
      {
         ID = Guid.NewGuid();
      }
      private Tag(string name)
      {
         Name = name;
         ID = Guid.NewGuid();
      }
      #endregion

      #region - Methods
      public static void OpenTags(IEnumerable<Tag> tags)
      {
         AllTags = new(tags);
      }
      public static Tag CreateTag(string name)
      {
         Tag newTag = new(name);
         if (!AllTags.Contains(newTag, _TagComparer))
         {
            AllTags.Add(newTag);
            return newTag;
         }
         else
         {
            return AllTags.First((t) => t.Name == name);
         }
      }

      public static void RemoveTag(Tag tag)
      {
         RemoveTagEvent?.Invoke(null, tag);
      }

      public static void RemoveTag(string tagName)
      {
         Tag tempTag = new(tagName);
         if (AllTags.Contains(tempTag, _TagComparer))
         {
            RemoveTagEvent?.Invoke(null, AllTags.First(t => t == tempTag));
         }
      }
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

      public Guid ID
      {
         get { return _id; }
         set
         {
            _id = value;
            OnPropertyChanged();
         }
      }
      #endregion
   }
}

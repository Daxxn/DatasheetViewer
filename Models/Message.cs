using MVVMLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatasheetViewer.Models
{
   public enum MessageType
   {
      Error,
      Info,
      Warning,
      Finished
   };

   public class Message : Model
   {
      #region - Fields & Properties
      private string _text;
      private MessageType _type;
      #endregion

      #region - Constructors
      public Message(string msg)
      {
         Text = msg;
         Type = MessageType.Info;
      }
      public Message(string msg, MessageType type)
      {
         Text = msg;
         Type = type;
      }
      #endregion

      #region - Methods

      #endregion

      #region - Full Properties
      public string Text
      {
         get { return _text; }
         set
         {
            _text = value;
            OnPropertyChanged();
         }
      }

      public MessageType Type
      {
         get { return _type; }
         set
         {
            _type = value;
            OnPropertyChanged();
         }
      }
      #endregion
   }
}

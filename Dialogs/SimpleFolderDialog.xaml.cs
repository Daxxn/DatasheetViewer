using DatasheetViewer.ViewModels;
using Syncfusion.SfSkinManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DatasheetViewer.Dialogs
{
   /// <summary>
   /// Interaction logic for SimpleFolderDialog.xaml
   /// </summary>
   public partial class SimpleFolderDialog : Window
   {
      public SimpleFolderDialog()
      {
         SfSkinManager.SetTheme(this, new Theme("FluentDark"));
         DataContext = MainViewModel.FolderDialogVM;
         MainViewModel.FolderDialogVM.CloseDialogEvent += FolderDialogVM_CloseDialogEvent;
         InitializeComponent();
      }

      private void FolderDialogVM_CloseDialogEvent(object sender, EventArgs e)
      {
         Close();
      }

      private void KeyBinding_Changed(object sender, EventArgs e)
      {
         ApplicationCommands.Paste.Execute(null, PathTextBox);
      }

      private void PathTextBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
      {
         PathTextBox.SelectAll();
      }

      private void Close_Click(object sender, RoutedEventArgs e)
      {
         Close();
      }
   }
}

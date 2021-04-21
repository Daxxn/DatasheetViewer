using DatasheetViewer.ViewModels;
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
   /// Interaction logic for OpenFolderDialog.xaml
   /// </summary>
   public partial class OpenFolderDialog : Window
   {
      public OpenFolderViewModel VM { get; private set; }
      public OpenFolderDialog()
      {
         InitializeComponent();
         VM = new();
         DataContext = VM;
      }

      private void Window_Loaded(object sender, RoutedEventArgs e)
      {
         VM.OnLoad();
      }
   }
}

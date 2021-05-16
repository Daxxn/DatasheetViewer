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
   /// Interaction logic for DatasheetEditDialog.xaml
   /// </summary>
   public partial class DatasheetEditDialog : Window
   {
      public DatasheetEditDialog()
      {
         SfSkinManager.SetTheme(this, new Theme("FluentDark"));

         DataContext = MainViewModel.EditVM;
         DatasheetEditViewModel.EndEditEvent += DatasheetEditViewModel_EndEditEvent;
         InitializeComponent();
      }

      private void DatasheetEditViewModel_EndEditEvent(object sender, List<Models.Datasheet> e)
      {
         Close();
      }

      private void Cancel_Click(object sender, RoutedEventArgs e)
      {
         Close();
      }
   }
}

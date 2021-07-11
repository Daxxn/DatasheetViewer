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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DatasheetViewer.Views
{
   /// <summary>
   /// Interaction logic for BasicCalculatorsView.xaml
   /// </summary>
   public partial class BasicCalculatorsView : UserControl
   {
      private CalculatorsViewModel VM { get; set; }
      public BasicCalculatorsView()
      {
         VM = new();
         DataContext = VM;
         InitializeComponent();
         CalcSelectorCombo.SelectionChanged += VM.BeginOhmsLawCalc;
      }
   }
}

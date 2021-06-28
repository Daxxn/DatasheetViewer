using DatasheetViewer.Dialogs;
using DatasheetViewer.Models;
using DatasheetViewer.ViewModels;
using Syncfusion.Pdf.Interactive;
using Syncfusion.SfSkinManager;
using Syncfusion.Themes.FluentDark.WPF;
using Syncfusion.Themes.MaterialDark.WPF;
using Syncfusion.Windows.PdfViewer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DatasheetViewer
{
   /// <summary>
   /// Interaction logic for MainWindow.xaml
   /// </summary>
   public partial class MainWindow : Window
   {
      /// <summary>
      /// Names of the unused controls.
      /// </summary>
      private readonly string[] _disabledTools = new string[]
      {
         "PART_ButtonTextSearch",
         "Part_CursorTools",
         "PART_MarqueeZoom",
         "PART_SelectTool",
         "PART_ButtonSignature",
         "PART_SelectTool",
         "PART_HandTool",
         "PART_Stamp",
         "PART_AnnotationsSeparator",
         "PART_ButtonTextBoxFont",
         "PART_FreeText",
         "PART_Fill",
         "PART_Shapes",
         "PART_Strikethrough",
         "PART_Underline",
         "PART_Highlight",
         "PART_Ink",
         "PART_AnnotationToolsSeparator",
         "PART_ZoomToolsSeparator_1",
         "PART_ComboBoxCurrentZoomLevel",
         "PART_ZoomToolsSeparator_0",
      };
      private MainViewModel VM { get; set; }
      public DatasheetEditDialog EditorWindow { get; private set; }
      private Settings AppSettings { get; set; } = Settings.AppSettings;
      public MainWindow()
      {
         #region Theme Init
         FluentDarkThemeSettings themeSettings = new()
         {
            PrimaryBackground = new SolidColorBrush(Color.FromRgb(50, 100, 150)),
            PrimaryForeground = new SolidColorBrush(Color.FromRgb(100, 100, 100)),
            FontFamily = new("FiraCode")
         };
         SfSkinManager.RegisterThemeSettings("FluentDark", themeSettings);
         SfSkinManager.ApplyStylesOnApplication = true;
         SfSkinManager.SetTheme(this, new Theme("FluentDark"));
         #endregion
         InitializeComponent();
         VM = new MainViewModel();
         DataContext = VM;
         Loaded += VM.WindowLoadedEvent;
         Loaded += MainWindow_Loaded;
         DatasheetTagsView.SelectionChanged += VM.SelectedTagChanged;

         AutoScanCheckbox.Checked += VM.AutoScanChanged;
         AutoScanCheckbox.Unchecked += VM.AutoScanChanged;
      }

      private void MainWindow_Loaded(object sender, RoutedEventArgs e)
      {
         if (AppSettings is null) return;

         if (AppSettings.OpenOnStartup)
         {
            DatasheetListExpander.IsExpanded = true;
         }
      }

      /// <summary>
      /// Removed unused controls from the PFDViewer Component.
      /// </summary>
      private void PDFView_Loaded(object sender, RoutedEventArgs e)
      {
         PDFView.EnableRedactionTool = false;
         PDFView.EnableLayers = false;
         PDFView.FormSettings.IsIconVisible = false;
         DocumentToolbar toolbar = PDFView.Template.FindName("PART_Toolbar", PDFView) as DocumentToolbar;

         foreach (var toolName in _disabledTools)
         {
            var btn = toolbar.Template.FindName(toolName, toolbar) as Button;
            if (btn is not null)
            {
               btn.Visibility = Visibility.Collapsed;
            } else
            {
               Rectangle rect = toolbar.Template.FindName(toolName, toolbar) as Rectangle;
               if (rect is not null)
               {
                  rect.Visibility = Visibility.Collapsed;
               }
               else
               {
                  ToggleButton tgl = toolbar.Template.FindName(toolName, toolbar) as ToggleButton;
                  if (tgl is not null)
                  {
                     tgl.Visibility = Visibility.Collapsed;
                  }
               }
            }
         }
      }

      private void SelectFolder_Click(object sender, RoutedEventArgs e)
      {
         SimpleFolderDialog dialog = new();
         dialog.ShowDialog();
         DatasheetListExpander.IsExpanded = true;
      }

      private void DatasheetsList_SelectionChange(object sender, SelectionChangedEventArgs e)
      {
         DatasheetInfoExpander.IsExpanded = true;
      }

      private void Edit_Click(object sender, RoutedEventArgs e)
      {
         VM.EditDatasheetsCmd.Execute(null);
         EditorWindow = new();
         EditorWindow.Closed += Edit_Close;
         EditorWindow.Closed += VM.EditCompleted;
         EditorWindow.Show();
         VM.IsEditorNOTOpen = false;
      }

      private void Edit_Close(object sender, EventArgs e)
      {
         VM.IsEditorNOTOpen = true;
         EditorWindow = null;
      }

      private void PDFView_PageSelected(object sender, PageSelectedEventArgs e)
      {

      }
   }
}

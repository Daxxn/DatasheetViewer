using DatasheetViewer.Models;
using DatasheetViewer.ViewModels;
using Syncfusion.SfSkinManager;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace DatasheetViewer
{
   /// <summary>
   /// Interaction logic for App.xaml
   /// </summary>
   public partial class App : Application
   {
      protected override void OnExit(ExitEventArgs e)
      {
         Settings.Save();
         base.OnExit(e);
      }

      protected override void OnStartup(StartupEventArgs e)
      {
         Settings.Open();
         base.OnStartup(e);
      }
   }
}

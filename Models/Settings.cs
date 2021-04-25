﻿using JsonReaderLibrary;
using MVVMLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatasheetViewer.Models
{
   public class Settings : Model
   {
      #region - Fields & Properties
      private static Settings _appSettings;
      private string _lastUsedPath;
      private bool _openOnStartup;
      private bool _autoScan;
      #endregion

      #region - Constructors
      public Settings() { }
      #endregion

      #region - Methods
      public static void Open()
      {
         try
         {
            string settingsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "DatasheetViewer", "settings.json");
            if (File.Exists(settingsPath))
            {
               _appSettings = JsonReader.OpenJsonFile<Settings>(settingsPath);
            }
         }
         catch (Exception) { }
      }

      public static void Save()
      {
         try
         {
            string settingsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "DatasheetViewer", "settings.json");
            if (!Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "DatasheetViewer")))
            {
               Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "DatasheetViewer"));
            }
            JsonReader.SaveJsonFile(settingsPath, AppSettings, true);
         }
         catch (Exception) { }
      }
      #endregion

      #region - Full Properties
      public static Settings AppSettings
      {
         get
         {
            if (_appSettings is null)
            {
               _appSettings = new();
            }
            return _appSettings;
         }
      }

      public string LastUsedPath
      {
         get { return _lastUsedPath; }
         set
         {
            _lastUsedPath = value;
            OnPropertyChanged();
         }
      }

      public bool OpenOnStartup
      {
         get { return _openOnStartup; }
         set
         {
            _openOnStartup = value;
            OnPropertyChanged();
         }
      }

      public bool AutoScan
      {
         get { return _autoScan; }
         set
         {
            _autoScan = value;
            OnPropertyChanged();
         }
      }
      #endregion
   }
}
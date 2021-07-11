using DatasheetViewer.Models;
using MVVMLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatasheetViewer.ViewModels
{
   public class CalculatorsViewModel : ViewModel
   {
      #region - Fields & Properties
      private OhmsLawModel _ohmsLaw;

      public Command BeginOhmsLawCalcCmd { get; init; }
      #endregion

      #region - Constructors
      public CalculatorsViewModel()
      {
         OhmsLaw = new();
         BeginOhmsLawCalcCmd = new(o => BeginOhmsLawCalc(o, null));
      }
      #endregion

      #region - Methods
      public void BeginOhmsLawCalc(object sender, EventArgs e)
      {
         OhmsLaw.Calc();
      }
      #endregion

      #region - Full Properties
      public OhmsLawModel OhmsLaw
      {
         get { return _ohmsLaw; }
         set
         {
            _ohmsLaw = value;
            OnPropertyChanged();
         }
      }
      #endregion
   }
}

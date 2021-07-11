using MVVMLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatasheetViewer.Models
{
   public enum CalcSelector
   {
      Voltage,
      Resistance,
      Current,
      Power
   };

   public class OhmsLawModel : Model
   {
      #region - Fields & Properties
      private CalcSelector _selector = CalcSelector.Voltage;
      private double _voltage;
      private double _resistance;
      private double _current;
      private double _power;
      #endregion

      #region - Constructors
      public OhmsLawModel() { }
      #endregion

      #region - Methods
      public void Calc()
      {
         switch (Selector)
         {
            case CalcSelector.Voltage:
               Voltage = GetVoltage();
               break;
            case CalcSelector.Resistance:
               Resistance = GetResistance();
               break;
            case CalcSelector.Current:
               Current = GetCurrent();
               break;
            case CalcSelector.Power:
               Power = GetPower();
               break;
            default:
               throw new Exception("Unknown Selector enum state.");
         }
      }

      private double GetVoltage()
      {
         if (Current != 0)
         {
            if (Resistance != 0)
            {
               return Current * Resistance;
            }
            return Power / Current;
         }
         return Math.Sqrt(Power * Resistance);
      }

      private double GetResistance()
      {
         if (Current != 0)
         {
            if (Voltage != 0)
            {
               return Voltage / Current;
            }
            return Power / Math.Pow(Current, 2);
         }
         if (Power != 0)
         {
            return Math.Pow(Voltage, 2) / Power;
         }
         return 0;
      }

      private double GetCurrent()
      {
         if (Resistance != 0)
         {
            if (Voltage != 0)
            {
               return Voltage / Resistance;
            }
            return Math.Sqrt(Power / Resistance);
         }
         if (Power != 0 && Voltage != 0)
         {
            return Power / Voltage;
         }
         return 0;
      }

      private double GetPower()
      {
         if (Resistance != 0)
         {
            if (Voltage != 0)
            {
               return Math.Pow(Voltage, 2) / Resistance;
            }
            return Math.Pow(Current, 2) * Resistance;
         }
         return Voltage * Current;

      }
      #endregion

      #region - Full Properties
      public CalcSelector Selector
      {
         get { return _selector; }
         set
         {
            _selector = value;
            OnPropertyChanged();
         }
      }

      public double Voltage
      {
         get { return _voltage; }
         set
         {
            _voltage = value;
            OnPropertyChanged();
         }
      }

      public double Resistance
      {
         get { return _resistance; }
         set
         {
            _resistance = value;
            OnPropertyChanged();
         }
      }

      public double Current
      {
         get { return _current; }
         set
         {
            _current = value;
            OnPropertyChanged();
         }
      }

      public double Power
      {
         get { return _power; }
         set
         {
            _power = value;
            OnPropertyChanged();
         }
      }
      #endregion
   }
}

using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BoostYourBIMTerrificTools
{
    [Transaction(TransactionMode.ReadOnly)]
    public class Units2021FamilyTypeUpgrade : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elementSet)
        {

            List<Tuple<string, string>> data = new List<Tuple<string, string>>
            {
                new Tuple<string, string>("BRITISH_THERMAL_UNIT_PER_FAHRENHEIT", "BRITISH_THERMAL_UNITS_PER_DEGREE_FAHRENHEIT"),
                new Tuple<string, string>("BRITISH_THERMAL_UNITS_PER_HOUR_FOOT_FAHRENHEIT", "BRITISH_THERMAL_UNITS_PER_HOUR_FOOT_DEGREE_FAHRENHEIT"),
                new Tuple<string, string>("BRITISH_THERMAL_UNITS_PER_HOUR_SQUARE_FOOT_FAHRENHEIT", "BRITISH_THERMAL_UNITS_PER_HOUR_SQUARE_FOOT_DEGREE_FAHRENHEIT"),
                new Tuple<string, string>("BRITISH_THERMAL_UNITS_PER_POUND_FAHRENHEIT", "BRITISH_THERMAL_UNITS_PER_POUND_DEGREE_FAHRENHEIT"),
                new Tuple<string, string>("CELSIUS_DIFFERENCE", "CELSIUS_INTERVAL"),
                new Tuple<string, string>("CUBIC_SQUARE_METERS_PER_KILOWATTS", "SQUARE_METERS_PER_KILOWATT"),
                new Tuple<string, string>("DECANEWTON_METERS", "DEKANEWTON_METERS"),
                new Tuple<string, string>("DECANEWTON_METERS_PER_METER", "DEKANEWTON_METERS_PER_METER"),
                new Tuple<string, string>("DECANEWTONS", "DEKANEWTONS"),
                new Tuple<string, string>("DECANEWTONS_PER_METER", "DEKANEWTONS_PER_METER"),
                new Tuple<string, string>("DECANEWTONS_PER_SQUARE_METER", "DEKANEWTONS_PER_SQUARE_METER"),
                new Tuple<string, string>("DECIMAL DEGREES", "DEGREES"),
                new Tuple<string, string>("DECIMAL US SURVEY FEET", "US_SURVEY_FEET"),
                new Tuple<string, string>("DEGREES", "DEGREES_MINUTES_SECONDS"),
                new Tuple<string, string>("DUT_BRITISH_THERMAL_UNITS_PER_HOUR_CUBIC_FOOT", "BRITISH_THERMAL_UNITS_PER_HOUR_CUBIC_FOOT"),
                new Tuple<string, string>("DUT_BRITISH_THERMAL_UNITS_PER_HOUR_SQUARE_FOOT", "BRITISH_THERMAL_UNITS_PER_HOUR_SQUARE_FOOT"),
                new Tuple<string, string>("DUT_TON_OF_REFRIGERATION", "TONS_OF_REFRIGERATION"),
                new Tuple<string, string>("FAHRENHEIT_DIFFERENCE", "FAHRENHEIT_INTERVAL"),
                new Tuple<string, string>("FRACTIONAL FEET", "FEET_AND_FRACTIONAL_INCHES"),
                new Tuple<string, string>("FRACTIONAL INCHES", "FRACTIONAL_INCHES"),
                new Tuple<string, string>("GALLONS", "US_GALLONS"),
                new Tuple<string, string>("GALLONS_US_PER_HOUR", "US_GALLONS_PER_HOUR"),
                new Tuple<string, string>("GALLONS_US_PER_MINUTE", "US_GALLONS_PER_MINUTE"),
                new Tuple<string, string>("GRADS", "GRADIANS"),
                new Tuple<string, string>("HOUR_SQUARE_FOOT_FAHRENHEIT_PER_BRITISH_THERMAL_UNIT", "HOUR_SQUARE_FOOT_DEGREES_FAHRENHEIT_PER_BRITISH_THERMAL_UNIT"),
                new Tuple<string, string>("INV_KILONEWTONS", "INVERSE_KILONEWTONS"),
                new Tuple<string, string>("INV_KIPS", "INVERSE_KIPS"),
                new Tuple<string, string>("JOULES_PER_GRAM_CELSIUS", "JOULES_PER_GRAM_DEGREE_CELSIUS"),
                new Tuple<string, string>("JOULES_PER_KILOGRAM_CELSIUS", "JOULES_PER_KILOGRAM_DEGREE_CELSIUS"),
                new Tuple<string, string>("KELVIN_DIFFERENCE", "KELVIN_INTERVAL"),
                new Tuple<string, string>("KILOGRAMS_MASS_PER_METER", "KILOGRAMS_PER_METER"),
                new Tuple<string, string>("LITERS_PER_SECOND_KILOWATTS", "LITERS_PER_SECOND_KILOWATT"),
                new Tuple<string, string>("METERS AND CENTIMETERS", "METERS_AND_CENTIMETERS"),
                new Tuple<string, string>("MICROINCHES_PER_INCH_FAHRENHEIT", "MICROINCHES_PER_INCH_DEGREE_FAHRENHEIT"),
                new Tuple<string, string>("MICROMETERS_PER_METER_CELSIUS", "MICROMETERS_PER_METER_DEGREE_CELSIUS"),
                new Tuple<string, string>("NUMBER_FIXED", "FIXED"),
                new Tuple<string, string>("PERMILLE", "PER_MILLE"),
                new Tuple<string, string>("POUNDS", "POUNDS_FORCE"),
                new Tuple<string, string>("POUNDS_MASS_PER_SQUARE_METER", "KILOGRAMS_PER_SQUARE_METER"),
                new Tuple<string, string>("POUNDS_FORCE_FORCE_FORCE_PER_SQUARE_INCH", "POUNDS_FORCE_PER_SQUARE_INCH"),
                new Tuple<string, string>("RANKINE_DIFFERENCE", "RANKINE_INTERVAL"),
                new Tuple<string, string>("RATIO1", "RATIO_1"),
                new Tuple<string, string>("RATIO10", "RATIO_10"),
                new Tuple<string, string>("RATIO12", "RATIO_12"),
                new Tuple<string, string>("RISE_10_FOOT", "RISE_10_FEET"),
                new Tuple<string, string>("RISE_FOOT", "RISE_1_FOOT"),
                new Tuple<string, string>("RISE_INCHES", "RISE_12_INCHES"),
                new Tuple<string, string>("RISE_MMS", "RISE_1000_MILLIMETERS"),
                new Tuple<string, string>("SQUARE_METER_KELVIN_PER_WATT", "SQUARE_METER_KELVINS_PER_WATT"),
                new Tuple<string, string>("SQUARE_METERS_PER_METER", "SQUARE_METERS_PER_KILONEWTON_METER")
            };

            System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog
            {
                Description = "Select Top Level Folder",
                ShowNewFolderButton = false,
            };
            if (folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
            {
                return Result.Cancelled;
            }
            string folder = folderBrowserDialog1.SelectedPath;
            foreach (string file in Directory.GetFiles(folder, "*.txt", SearchOption.AllDirectories))
            {
                string text = File.ReadAllText(file);
                foreach (Tuple<string, string> tuple in data)
                {
                    text = Regex.Replace(text, "##" + tuple.Item1 + "##", "##" + tuple.Item2 + "##", RegexOptions.IgnoreCase);
                    text = Regex.Replace(text, "##" + tuple.Item1 + ",", "##" + tuple.Item2 + ",", RegexOptions.IgnoreCase);
                }

                using (StreamWriter sw = new StreamWriter(file))
                {
                    sw.Write(text, false);
                }
            }

            return Result.Succeeded;
        }

    }

}

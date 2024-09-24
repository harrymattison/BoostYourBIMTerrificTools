using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using System.Diagnostics;

namespace WallOpeningArea
{
    public partial class SearchConfigForm : System.Windows.Forms.Form
    {
        ExternalCommandData _commandData;

        public SearchConfigForm(ExternalCommandData commandData)
        {
            InitializeComponent();
            _commandData = commandData;
        }

        private void Config_Load(object sender, EventArgs e)
        {
#if PREFORGETYPEID
            FormatOptions fo = _commandData.Application.ActiveUIDocument.Document.GetUnits().GetFormatOptions(UnitType.UT_Area);
            DisplayUnitType dut = fo.DisplayUnits;// e.g. DUT_SQUARE_METERS
            string unitLbl = LabelUtils.GetLabelFor(dut); // e.g. Sq meters 
#else
            FormatOptions fo = _commandData.Application.ActiveUIDocument.Document.GetUnits().GetFormatOptions(SpecTypeId.Area);
            ForgeTypeId symbolTypeId = fo.GetSymbolTypeId();// e.g. DUT_SQUARE_METERS
            string unitLbl = LabelUtils.GetLabelForSymbol(symbolTypeId); // e.g. Sq meters 
#endif

            // Put a label on the form
            lblUnitName.Text = unitLbl;
        }

        /// <summary>
        /// Entered area (in Square Feet)
        /// </summary>
        public double AreaField
        {
            get
            {
#if PREFORGETYPEID
                FormatOptions fo = _commandData.Application.ActiveUIDocument.Document.GetUnits().GetFormatOptions(UnitType.UT_Area);
                double enteredValue = (double)txtArea.Value;
                return enteredValue * ConverstionFactorToSqFeet(fo.DisplayUnits);
#else
                FormatOptions fo = _commandData.Application.ActiveUIDocument.Document.GetUnits().GetFormatOptions(SpecTypeId.Area);
                double enteredValue = (double)txtArea.Value;
                return enteredValue * ConverstionFactorToSqFeet(fo.GetUnitTypeId());
#endif
            }
        }

#if PREFORGETYPEID
        private double ConverstionFactorToSqFeet(DisplayUnitType unitFrom)
        {
            switch (unitFrom)
            {
                case DisplayUnitType.DUT_ACRES:
                    return Acres_To_SqFeet;
                case DisplayUnitType.DUT_HECTARES:
                    return Hectares_To_SqFeet;
                case DisplayUnitType.DUT_SQUARE_FEET:
                    return 1;
                case DisplayUnitType.DUT_SQUARE_METERS:
                    return (1 / SqFeet_To_SqMeter);
                case DisplayUnitType.DUT_SQUARE_INCHES:
                    return SqInche_To_SqFeet;
                case DisplayUnitType.DUT_SQUARE_CENTIMETERS:
                    return SqCm_To_SqFeet;
                case DisplayUnitType.DUT_SQUARE_MILLIMETERS:
                    return SqMm_To_SqFeet;
            }
            return 1;
        }
#else
        private double ConverstionFactorToSqFeet(ForgeTypeId unitFrom)
        {
            if (unitFrom == UnitTypeId.Acres)
                return Acres_To_SqFeet;
            else if (unitFrom == UnitTypeId.Hectares)
                return Hectares_To_SqFeet;
            else if (unitFrom == UnitTypeId.SquareFeet)
                return 1;
            else if (unitFrom == UnitTypeId.SquareMeters)
                return (1 / SqFeet_To_SqMeter);
            else if (unitFrom == UnitTypeId.SquareInches)
                return SqInche_To_SqFeet;
            else if (unitFrom == UnitTypeId.SquareCentimeters)
                return SqCm_To_SqFeet;
            else if (unitFrom == UnitTypeId.SquareMillimeters)
                return SqMm_To_SqFeet;
            else
                return 1;
        }
#endif

        private const double SqInche_To_SqFeet = 0.00694444444;
        private const double SqCm_To_SqFeet = 0.00107639104;
        private const double SqMm_To_SqFeet = 0.0000107639104;
        private const double Acres_To_SqFeet = 43560;
        private const double Hectares_To_SqFeet = 107639;
        private const double SqFeet_To_SqMeter = 0.09290304;

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (txtArea.Value <= 0)
            {
                Autodesk.Revit.UI.TaskDialog.Show("Error", "Value must be greater than 0.");
                return;
            }
            SharedParameterFunctions.PARAMETER_SMALL_OPEN_NAME = "Opening Area smaller max";
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://www.patreon.com/BoostYourBIM");
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://boostyourbim.wordpress.com/learn/");
        }
    }
}

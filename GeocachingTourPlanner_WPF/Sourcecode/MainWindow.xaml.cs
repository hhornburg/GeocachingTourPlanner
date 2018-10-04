using GeocachingTourPlanner.IO;
using GeocachingTourPlanner.Routing;
using GeocachingTourPlanner.Types;
using Itinero;
using Itinero.LocalGeo;
using Mapsui;
using Mapsui.Geometries;
using Mapsui.Layers;
using Mapsui.Projection;
using Mapsui.Providers;
using Mapsui.Styles;
using Mapsui.UI;
using Mapsui.UI.Wpf;
using Mapsui.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;
using static GeocachingTourPlanner.Routing.RoutePlanner;

namespace GeocachingTourPlanner.UI
{

    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //Browser
            try
            {
                //FIX webBrowser1.Navigate(new Uri(Application.StartupPath + "\\first-steps.html"));
            }
            catch (Exception)
            {
                MessageBox.Show("Can't show you the first steps, cause the needed html file is missing.");
            }


            //Map
            mapControl.Map.Layers.Add(OpenStreetMap.CreateTileLayer());
            TooltipCanvas.Visibility = Visibility.Collapsed;
        }

        #region Overview
        #region UI Events
        private void OpenWikiButton_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("https://github.com/pingurus/GeocachingTourPlanner/wiki");
        }

        private void ImportOSMDataButton_Click(object sender, RoutedEventArgs e)
        {
            Fileoperations.ImportOSMData();
        }

        private void ImportGeocachesButton_Click(object sender, RoutedEventArgs e)
        {
            Fileoperations.ImportGeocaches();
            Map_RenewGeocacheLayer();
        }

        private void setGeocachedatabaseButton_Click(object sender, RoutedEventArgs e)
        {
            if (App.DB.OpenExistingDBFile(Databases.Geocaches))
            {
                Fileoperations.ReadGeocaches();
                Map_RenewGeocacheLayer();
            }
        }

        private void setRoutingprofiledatabaseButton_Click(object sender, RoutedEventArgs e)
        {
            if (App.DB.OpenExistingDBFile(Databases.Routingprofiles))
            {
                Fileoperations.ReadRoutingprofiles();
            }
        }

        private void setRatingprofiledatabaseButton_Click(object sender, RoutedEventArgs e)
        {
            if (App.DB.OpenExistingDBFile(Databases.Ratingprofiles))
            {
                Fileoperations.ReadRatingprofiles();
            }
        }

        private void setRouterDBButton_Click(object sender, RoutedEventArgs e)
        {
            if (App.DB.OpenExistingDBFile(Databases.RouterDB))
            {
                Fileoperations.ReadRouterDB();
            }
        }

        private void OpenInBrowser(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        private void NewRatingprofileDatabaseButton_Click(object sender, RoutedEventArgs e)
        {
            Fileoperations.NewRatingprofileDatabase();
        }

        private void NewRoutingprofilesDatabaseButton_Click(object sender, RoutedEventArgs e)
        {
            Fileoperations.NewRoutingprofileDatabase();
        }
        #endregion
        #region Accessors
        public void SetRouterDBLabel(string text)
        {
            Application.Current.Dispatcher.BeginInvoke( //You never know from which thread it is called
                new Action(() =>
                {
                    RouterDBStateLabel.Text = text;
                }));
        }

        public void Geocaches_ListChanged(object sender, ListChangedEventArgs e)
        {
            Application.Current.Dispatcher.BeginInvoke( //You never know from which thread it is called
                   new Action(() =>
                   {
                       GeocachesStateLabel.Text = App.Geocaches.Count.ToString() + " Geocaches loaded";
                       Map_RenewGeocacheLayer();
                   }));
        }
        #endregion
        #endregion

        #region Settings
        #region UI Events
        #region Autotargetselection Max
        bool AutotargetselectionMax_TextChanged = false;
        private void AutotargetselectionMaxTextbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            AutotargetselectionMax_TextChanged = true;

        }

        private void AutotargetselectionMaxTextBox_Leave(object sender, RoutedEventArgs e)
        {
            if (AutotargetselectionMax_TextChanged)
            {
                if (int.TryParse(AutotargetselectionMaxValue.Text, out int Value))
                {
                    if (Value > 100)
                    {
                        MessageBox.Show("Percentage can't be bigger than 100");
                        AutotargetselectionMaxValue.Text = 100.ToString();
                    }
                    else if (Value < 0)
                    {
                        MessageBox.Show("Percentage can't be smaller than 0");
                        AutotargetselectionMaxValue.Text = (App.DB.PercentageOfDistanceInAutoTargetselection_Min * 100 + 1).ToString();
                    }
                    else if (Value < 100 * App.DB.PercentageOfDistanceInAutoTargetselection_Min)
                    {
                        MessageBox.Show("Percentage can't be smaller than that of the Minimum");
                        AutotargetselectionMaxValue.Text = (App.DB.PercentageOfDistanceInAutoTargetselection_Min * 100 + 1).ToString();
                    }
                    else
                    {

                        App.DB.PercentageOfDistanceInAutoTargetselection_Max = (Value / 100f);
                    }
                }
                else if (AutotargetselectionMaxValue.Text.Length != 0)
                {
                    MessageBox.Show("Enter valid integers only.");
                }
            }
            AutotargetselectionMax_TextChanged = false;
        }
        #endregion

        #region Autotargetselection Min
        bool AutotargetselectionMin_TextChanged = false;
        private void AutotargetselectionMinTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            AutotargetselectionMin_TextChanged = true;
        }

        private void AutotargetselectionMinTextBox_Leave(object sender, RoutedEventArgs e)
        {
            if (AutotargetselectionMin_TextChanged)
            {
                if (int.TryParse(AutotargetselectionMinValue.Text, out int Value))
                {
                    if (Value > 100)
                    {
                        MessageBox.Show("Percentage can't be bigger than 100");
                        AutotargetselectionMinValue.Text = (App.DB.PercentageOfDistanceInAutoTargetselection_Max * 100 - 1).ToString();
                    }
                    else if (Value < 0)
                    {
                        MessageBox.Show("Percentage can't be smaller than 0");
                        AutotargetselectionMinValue.Text = 0.ToString();
                    }
                    else if (Value > 100 * App.DB.PercentageOfDistanceInAutoTargetselection_Max * 100)
                    {
                        MessageBox.Show("Percentage can't be bigger than that of the Maximum");
                        AutotargetselectionMinValue.Text = (App.DB.PercentageOfDistanceInAutoTargetselection_Max - 1).ToString();
                    }
                    else
                    {

                        App.DB.PercentageOfDistanceInAutoTargetselection_Min = (Value / 100f);
                    }
                }
                else if (AutotargetselectionMinValue.Text.Length != 0)
                {
                    MessageBox.Show("Enter valid integers only.");
                }
            }
            AutotargetselectionMin_TextChanged = false;
        }
        #endregion

        private void RoutefindingWidth_Textbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(RouteFindingWidthValue.Text, out int Value))
            {
                App.DB.RoutefindingWidth = Value;
            }
            else if (RouteFindingWidthValue.Text.Length != 0)
            {
                MessageBox.Show("Enter valid integers only.");
            }
        }

        private void LiveDisplayRouteCalculationCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            App.DB.DisplayLiveCalculation = true;
        }
        private void LiveDisplayRouteCalculationCheckbox_Unchecked(object sender, RoutedEventArgs e)
        {
            App.DB.DisplayLiveCalculation = false;
        }

        private void MarkerSizeValue_Changed(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            App.MarkerStyleCache.Clear();
            App.DB.MarkerSize = (int)MarkerSizeValue.Value;
            Map_RenewGeocacheLayer();
        }
        #endregion
        #region Methods
        /// <summary>
        /// Reads the Settings from the DB and writes them to the according textboxes
        /// </summary>
        public void UpdateSettingsTextBoxes()
        {
            RouteFindingWidthValue.Text = App.DB.RoutefindingWidth.ToString();
            MarkerSizeValue.Value = App.DB.MarkerSize;
            AutotargetselectionMinValue.Text = (App.DB.PercentageOfDistanceInAutoTargetselection_Min * 100).ToString();
            AutotargetselectionMaxValue.Text = (App.DB.PercentageOfDistanceInAutoTargetselection_Max * 100).ToString();
            LiveDisplayRouteCalculationValue.IsChecked = App.DB.DisplayLiveCalculation;
        }
        #endregion
        #endregion

        #region Status
        /// <summary>
        /// Updates the Statusbar
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ProgressToShow"></param>
        public void UpdateStatus(string message, int ProgressToShow = 0)
        {
            Application.Current.Dispatcher.BeginInvoke(
                 new Action(() =>
                 {
                     File.AppendAllText("Log.txt", "[" + DateTime.Now + "]: " + message + "\n");
                     if (ProgressToShow != 0)//If the status changes while this is still processing and the new one isn't time consuming (currently there are only two time consuming methods, which cannot run simultaneously), the tooltip still chows the old information, so one can still check what happens
                     {

                         StatusProgressBar.ToolTip = new ToolTip().Content = message;
                         StatusProgressBar.Value = ProgressToShow;
                     }
                     else if (StatusProgressBar.Value == 100)//Thus the previous operation has finished
                     {
                         StatusProgressBar.Value = 0;
                         StatusProgressBar.ToolTip = new ToolTip().Content = message;
                     }
                     StatusLabel.Text = message;
                 }));
        }
        #endregion

        #region unspecific helpers
        /// <summary>
        /// Sets the text of all TextBoxchildren of the specified parent to "0"
        /// </summary>
        /// <param name="parent"></param>
        private void SetAllEmptyChildTextboxesToZero(DependencyObject parent)
        {
            TextBox TB;
            do
            {
                TB = ReturnFirstEmptyTextBox(parent);
                if (TB != null)
                {
                    TB.Text = "0";
                }
            } while (TB != null);
        }

        private TextBox ReturnFirstEmptyTextBox(DependencyObject Control)
        {
            if (Control != null)
            {
                if (Control.GetType() == typeof(TextBox))
                {
                    return (TextBox)Control;
                }
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(Control); i++)
                {
                    TextBox childReturn = ReturnFirstEmptyTextBox(VisualTreeHelper.GetChild(Control, i));
                    if (childReturn != null && childReturn.Text == "")
                    {
                        return childReturn;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Empties all textboxes child textboxes
        /// </summary>
        /// <param name="parent"></param>
        private void ClearAllChildTextboxes(DependencyObject parent)
        {
            TextBox TB;
            do
            {
                TB = ReturnFirstNotEmptyTextBox(parent);
                if (TB != null)
                {
                    TB.Text = "";
                }
            } while (TB != null);
        }

        private TextBox ReturnFirstNotEmptyTextBox(DependencyObject Control)
        {
            if (Control != null)
            {
                if (Control.GetType() == typeof(TextBox))
                {
                    return (TextBox)Control;
                }
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(Control); i++)
                {
                    TextBox childReturn = ReturnFirstNotEmptyTextBox(VisualTreeHelper.GetChild(Control, i));
                    if (childReturn != null && childReturn.Text != "")
                    {
                        return childReturn;
                    }
                }
            }
            return null;
        }

        private Result<Coordinate> ExtractCoordinates(string text)
        {
            if (text.Length == 0)
            {
                return new Result<Coordinate>("Empty Text");
            }

            text = text.Replace(',', '.');
            text = text.ToLower();

            int IndexOfLatitude = text.IndexOfAny(new char[] { 'n', 's' });
            int IndexOfLongitude = text.IndexOfAny(new char[] { 'e', 'w' });

            if ((IndexOfLatitude == -1 ^ IndexOfLongitude == -1))//Both ave to be either -1 or not -1, since the format has to be the same
            {
                MessageBox.Show("Please decide on a coordinate format");
                return new Result<Coordinate>("FormatError");
            }
            else if (IndexOfLongitude == -1)//Don't care which one, but I know coordinates are formatted with +/-
            {
                if (text.IndexOfAny(new char[] { ' ', ';' }) == -1)
                {
                    MessageBox.Show("I can't split the coordinates you entered");
                    return new Result<Coordinate>("FormatError");
                }

                string lat_string = text.Substring(0, text.IndexOfAny(new char[] { ' ', ';' }));
                string lon_string = text.Substring(text.IndexOfAny(new char[] { ' ', ';' }) + 1);

                string allowedChars = "01234567890.,";
                lon_string = lon_string.Where(c => allowedChars.Contains(c)).ToString();
                lat_string = lat_string.Where(c => allowedChars.Contains(c)).ToString();

                if (float.TryParse(lat_string, NumberStyles.Any, CultureInfo.InvariantCulture, out float lat_float) && float.TryParse(lon_string, NumberStyles.Any, CultureInfo.InvariantCulture, out float lon_float))
                {
                    return new Result<Coordinate>(new Coordinate(lat_float, lon_float));
                }
                else
                {
                    MessageBox.Show("Please check the coordinate you entered");
                    return new Result<Coordinate>("FormatError");
                }
            }
            else
            {

                string lat_string = null;
                string lon_string = null;

                if (IndexOfLatitude < IndexOfLongitude)
                {
                    lat_string = text.Substring(1, IndexOfLongitude - 1).Replace(" ", "").Replace(";", "");//Don't take Letters
                    lon_string = text.Substring(IndexOfLongitude + 1).Replace(" ", "").Replace(";", "");
                }
                else if (IndexOfLatitude > IndexOfLongitude)
                {
                    lon_string = text.Substring(1, IndexOfLatitude - 1).Replace(" ", "").Replace(";", "");//Don't take Letters
                    lat_string = text.Substring(IndexOfLatitude + 1).Replace(" ", "").Replace(";", "");
                }

                if (float.TryParse(lat_string, out float lat_float) && float.TryParse(lon_string, out float lon_float))
                {
                    if (text[IndexOfLatitude] == 's')
                    {
                        lat_float = -lat_float;
                    }

                    if (text[IndexOfLongitude] == 's')
                    {
                        lon_float = -lon_float;
                    }

                    return new Result<Coordinate>(new Coordinate(lat_float, lon_float));
                }
                else
                {
                    MessageBox.Show("Please check the coordinate you entered");
                    return new Result<Coordinate>("FormatError");
                }
            }
        }


        #endregion


    }
    /// <summary>
    /// Quasi enum for layer names
    /// </summary>
    public static class Layers
    {
        public static readonly string GeocacheLayer = "Geocaches";
        public static readonly string WaypointLayer = "Waypoints";
        public static readonly string CurrentRouteLayer = "CurrentRoute";
    }
}

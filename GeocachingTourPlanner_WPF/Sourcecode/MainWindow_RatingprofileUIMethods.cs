using GeocachingTourPlanner.IO;
using GeocachingTourPlanner.Routing;
using GeocachingTourPlanner.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace GeocachingTourPlanner.UI
{
    public partial class MainWindow : Window
    {
        #region Events
        private void RatingprofileSaveOnly_Click(object sender, RoutedEventArgs e)
        {
            CreateRatingprofile();
        }

        private void RatingprofileSaveApply_Click(object sender, RoutedEventArgs e)
        {
            CreateRatingprofile();
            RateGeocaches();
        }

        private void DeleteRatingprofileButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (Ratingprofile RP in App.Ratingprofiles.Where(x => x.Name == App.DB.ActiveRatingprofile.Name).ToList())
            {
                App.Ratingprofiles.Remove(RP);
            }

            ClearAllChildTextboxes(RatingprofilesSettingsGrid);
            UpdateStatus("deleted ratingprofile");
        }

        private void Ratingprofile_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (EditRatingprofileCombobox.SelectedItem == null)
            {
                if (App.DB.ActiveRatingprofile != null)
                {
                    EditRatingprofileCombobox.SelectedItem = App.DB.ActiveRatingprofile.Name;
                }
            }
            else
            {
                Ratingprofile SelectedRatingprofile = App.Ratingprofiles.First(x => x.Name == EditRatingprofileCombobox.SelectedItem.ToString());
                App.DB.ActiveRatingprofile = SelectedRatingprofile;
                try
                {
                    //Name des Profils
                    RatingprofileName.Text = SelectedRatingprofile.Name;

                    //Prioritäten
                    TypePriorityValue.Text = SelectedRatingprofile.TypePriority.ToString();
                    SizePriorityValue.Text = SelectedRatingprofile.SizePriority.ToString();
                    DPriorityValue.Text = SelectedRatingprofile.DPriority.ToString();
                    TPriorityValue.Text = SelectedRatingprofile.TPriority.ToString();

                    //TypenValueungen
                    TraditionalValue.Text = SelectedRatingprofile.TypeRatings.First(x => x.Key == GeocacheType.Traditional).Value.ToString();
                    EarthcacheValue.Text = SelectedRatingprofile.TypeRatings.First(x => x.Key == GeocacheType.EarthCache).Value.ToString();
                    MultiValue.Text = SelectedRatingprofile.TypeRatings.First(x => x.Key == GeocacheType.Multi).Value.ToString();
                    MysteryValue.Text = SelectedRatingprofile.TypeRatings.First(x => x.Key == GeocacheType.Mystery).Value.ToString();
                    LetterboxValue.Text = SelectedRatingprofile.TypeRatings.First(x => x.Key == GeocacheType.Letterbox).Value.ToString();
                    VirtualValue.Text = SelectedRatingprofile.TypeRatings.First(x => x.Key == GeocacheType.Virtual).Value.ToString();
                    OtherTypeValue.Text = SelectedRatingprofile.TypeRatings.First(x => x.Key == GeocacheType.Other).Value.ToString();
                    WebcamValue.Text = SelectedRatingprofile.TypeRatings.First(x => x.Key == GeocacheType.Webcam).Value.ToString();
                    WherigoValue.Text = SelectedRatingprofile.TypeRatings.First(x => x.Key == GeocacheType.Wherigo).Value.ToString();

                    //Größe
                    LargeValue.Text = SelectedRatingprofile.SizeRatings.First(x => x.Key == GeocacheSize.Large).Value.ToString();
                    RegularValue.Text = SelectedRatingprofile.SizeRatings.First(x => x.Key == GeocacheSize.Regular).Value.ToString();
                    SmallValue.Text = SelectedRatingprofile.SizeRatings.First(x => x.Key == GeocacheSize.Small).Value.ToString();
                    MicroValue.Text = SelectedRatingprofile.SizeRatings.First(x => x.Key == GeocacheSize.Micro).Value.ToString();
                    OtherSizeValue.Text = SelectedRatingprofile.SizeRatings.First(x => x.Key == GeocacheSize.Other).Value.ToString();

                    //D
                    D1Value.Text = SelectedRatingprofile.DRatings.First(x => x.Key == 1).Value.ToString();
                    D15Value.Text = SelectedRatingprofile.DRatings.First(x => x.Key == 1.5).Value.ToString();
                    D2Value.Text = SelectedRatingprofile.DRatings.First(x => x.Key == 2).Value.ToString();
                    D25Value.Text = SelectedRatingprofile.DRatings.First(x => x.Key == 2.5).Value.ToString();
                    D3Value.Text = SelectedRatingprofile.DRatings.First(x => x.Key == 3).Value.ToString();
                    D35Value.Text = SelectedRatingprofile.DRatings.First(x => x.Key == 3.5).Value.ToString();
                    D4Value.Text = SelectedRatingprofile.DRatings.First(x => x.Key == 4).Value.ToString();
                    D45Value.Text = SelectedRatingprofile.DRatings.First(x => x.Key == 4.5).Value.ToString();
                    D5Value.Text = SelectedRatingprofile.DRatings.First(x => x.Key == 5).Value.ToString();

                    //T
                    T1Value.Text = SelectedRatingprofile.TRatings.First(x => x.Key == 1).Value.ToString();
                    T15Value.Text = SelectedRatingprofile.TRatings.First(x => x.Key == 1.5).Value.ToString();
                    T2Value.Text = SelectedRatingprofile.TRatings.First(x => x.Key == 2).Value.ToString();
                    T25Value.Text = SelectedRatingprofile.TRatings.First(x => x.Key == 2.5).Value.ToString();
                    T3Value.Text = SelectedRatingprofile.TRatings.First(x => x.Key == 3).Value.ToString();
                    T35Value.Text = SelectedRatingprofile.TRatings.First(x => x.Key == 3.5).Value.ToString();
                    T4Value.Text = SelectedRatingprofile.TRatings.First(x => x.Key == 4).Value.ToString();
                    T45Value.Text = SelectedRatingprofile.TRatings.First(x => x.Key == 4.5).Value.ToString();
                    T5Value.Text = SelectedRatingprofile.TRatings.First(x => x.Key == 5).Value.ToString();

                    //Sonstige
                    NMFlagValue.Text = SelectedRatingprofile.NMPenalty.ToString();
                    if (SelectedRatingprofile.Yearmode == Yearmode.multiply)
                    {
                        AgeValue.SelectedItem = AgeValue.Items[0];
                    }
                    else
                    {
                        AgeValue.SelectedItem = AgeValue.Items[1];
                    }
                    AgeFactorValue.Text = SelectedRatingprofile.Yearfactor.ToString();
                }
                catch (Exception ex)
                {
                    MessageBoxResult aw = MessageBox.Show("There seems to be an error in this file. Do you want to delete it?", "Fehler", MessageBoxButton.YesNo, MessageBoxImage.Error);
                    if (aw == MessageBoxResult.Yes)
                    {
                        App.Ratingprofiles.Remove(SelectedRatingprofile);
                    }
                }
            }
        }

        /// <summary>
        /// Keeps the Dropdownmenu updated
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Ratingprofiles_ListChanged(object sender, ListChangedEventArgs e)
        {
            EditRatingprofileCombobox.Items.Clear();

            foreach (Ratingprofile profile in App.Ratingprofiles)
            {
                EditRatingprofileCombobox.Items.Add(profile.Name);
            }
            RatingprofilesStateLabel.Text = App.Ratingprofiles.Count.ToString() + " Ratingprofiles loaded";
        }

        #endregion
        #region Methods
        private void CreateRatingprofile()
        {
            SetAllEmptyChildTextboxesToZero(RatingprofilesSettingsGrid);
            Ratingprofile Profile = new Ratingprofile();
            if (RatingprofileName.Text == "")
            {
                MessageBox.Show("Please set a name");
                return;
            }
            try
            {
                Profile.Name = RatingprofileName.Text;
                Profile.TypePriority = int.Parse(TypePriorityValue.Text);
                Profile.TypeRatings = new List<SerializableKeyValuePair<GeocacheType, int>>();
                Profile.TypeRatings.Add(new SerializableKeyValuePair<GeocacheType, int>(GeocacheType.EarthCache, int.Parse(EarthcacheValue.Text)));
                Profile.TypeRatings.Add(new SerializableKeyValuePair<GeocacheType, int>(GeocacheType.Letterbox, int.Parse(LetterboxValue.Text)));
                Profile.TypeRatings.Add(new SerializableKeyValuePair<GeocacheType, int>(GeocacheType.Multi, int.Parse(MultiValue.Text)));
                Profile.TypeRatings.Add(new SerializableKeyValuePair<GeocacheType, int>(GeocacheType.Mystery, int.Parse(MysteryValue.Text)));
                Profile.TypeRatings.Add(new SerializableKeyValuePair<GeocacheType, int>(GeocacheType.Other, int.Parse(OtherTypeValue.Text)));
                Profile.TypeRatings.Add(new SerializableKeyValuePair<GeocacheType, int>(GeocacheType.Traditional, int.Parse(TraditionalValue.Text)));
                Profile.TypeRatings.Add(new SerializableKeyValuePair<GeocacheType, int>(GeocacheType.Virtual, int.Parse(VirtualValue.Text)));
                Profile.TypeRatings.Add(new SerializableKeyValuePair<GeocacheType, int>(GeocacheType.Webcam, int.Parse(WebcamValue.Text)));
                Profile.TypeRatings.Add(new SerializableKeyValuePair<GeocacheType, int>(GeocacheType.Wherigo, int.Parse(WherigoValue.Text)));

                Profile.SizePriority = int.Parse(SizePriorityValue.Text);
                Profile.SizeRatings = new List<SerializableKeyValuePair<GeocacheSize, int>>();
                Profile.SizeRatings.Add(new SerializableKeyValuePair<GeocacheSize, int>(GeocacheSize.Large, int.Parse(LargeValue.Text)));
                Profile.SizeRatings.Add(new SerializableKeyValuePair<GeocacheSize, int>(GeocacheSize.Micro, int.Parse(MicroValue.Text)));
                Profile.SizeRatings.Add(new SerializableKeyValuePair<GeocacheSize, int>(GeocacheSize.Other, int.Parse(OtherSizeValue.Text)));
                Profile.SizeRatings.Add(new SerializableKeyValuePair<GeocacheSize, int>(GeocacheSize.Regular, int.Parse(RegularValue.Text)));
                Profile.SizeRatings.Add(new SerializableKeyValuePair<GeocacheSize, int>(GeocacheSize.Small, int.Parse(SmallValue.Text)));

                Profile.DPriority = int.Parse(DPriorityValue.Text);
                Profile.DRatings = new List<SerializableKeyValuePair<float, int>>();
                Profile.DRatings.Add(new SerializableKeyValuePair<float, int>(1f, int.Parse(D1Value.Text)));
                Profile.DRatings.Add(new SerializableKeyValuePair<float, int>(1.5f, int.Parse(D15Value.Text)));
                Profile.DRatings.Add(new SerializableKeyValuePair<float, int>(2f, int.Parse(D2Value.Text)));
                Profile.DRatings.Add(new SerializableKeyValuePair<float, int>(2.5f, int.Parse(D25Value.Text)));
                Profile.DRatings.Add(new SerializableKeyValuePair<float, int>(3f, int.Parse(D3Value.Text)));
                Profile.DRatings.Add(new SerializableKeyValuePair<float, int>(3.5f, int.Parse(D35Value.Text)));
                Profile.DRatings.Add(new SerializableKeyValuePair<float, int>(4f, int.Parse(D4Value.Text)));
                Profile.DRatings.Add(new SerializableKeyValuePair<float, int>(4.5f, int.Parse(D45Value.Text)));
                Profile.DRatings.Add(new SerializableKeyValuePair<float, int>(5f, int.Parse(D5Value.Text)));

                Profile.TPriority = int.Parse(TPriorityValue.Text);
                Profile.TRatings = new List<SerializableKeyValuePair<float, int>>();
                Profile.TRatings.Add(new SerializableKeyValuePair<float, int>(1f, int.Parse(T1Value.Text)));
                Profile.TRatings.Add(new SerializableKeyValuePair<float, int>(1.5f, int.Parse(T15Value.Text)));
                Profile.TRatings.Add(new SerializableKeyValuePair<float, int>(2f, int.Parse(T2Value.Text)));
                Profile.TRatings.Add(new SerializableKeyValuePair<float, int>(2.5f, int.Parse(T25Value.Text)));
                Profile.TRatings.Add(new SerializableKeyValuePair<float, int>(3f, int.Parse(T3Value.Text)));
                Profile.TRatings.Add(new SerializableKeyValuePair<float, int>(3.5f, int.Parse(T35Value.Text)));
                Profile.TRatings.Add(new SerializableKeyValuePair<float, int>(4f, int.Parse(T4Value.Text)));
                Profile.TRatings.Add(new SerializableKeyValuePair<float, int>(4.5f, int.Parse(T45Value.Text)));
                Profile.TRatings.Add(new SerializableKeyValuePair<float, int>(5f, int.Parse(T5Value.Text)));

                if (!int.TryParse(NMFlagValue.Text.Replace("-", ""), out int Value))
                {
                    MessageBox.Show("Please write only positive whole numbers into the field with the NMPenalty");
                }
                else
                {
                    Profile.NMPenalty = Value;
                }

                if (AgeValue.SelectedItem.ToString().ToLower().Contains("multiply"))
                {
                    Profile.Yearmode = Yearmode.multiply;
                }
                else
                {
                    Profile.Yearmode = Yearmode.square_n_divide;
                }

                Profile.Yearfactor = int.Parse(AgeFactorValue.Text);

                if (Profile.Yearmode == Yearmode.square_n_divide && Profile.Yearfactor == 0)
                {
                    MessageBox.Show("Don't you dare to divide by 0!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

            }
            catch (FormatException)
            {
                MessageBox.Show("Please fill all fields");
                return;
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Please select something from all comboboxes");
                return;
            }

            //Eintragen des neuen Profils
            foreach (Ratingprofile BP in App.Ratingprofiles.Where(x => x.Name == Profile.Name).ToList())//Make sure only one profile with a name exists
            {
                App.Ratingprofiles.Remove(BP);
            }
            UpdateStatus("Ratingprofile saved");
            App.Ratingprofiles.Add(Profile);
            App.DB.ActiveRatingprofile = Profile;
            EditRatingprofileCombobox.SelectedItem = Profile.Name;
        }

        private bool RateGeocaches()
        {
            Ratingprofile ratingprofile;

            if (App.DB.ActiveRatingprofile != null)
            {
                ratingprofile = App.DB.ActiveRatingprofile;
            }
            else
            {
                MessageBox.Show("Please select a Ratingprofile");
                return false;
            }
            foreach (Geocache GC in App.Geocaches)
            {
                GC.Rate(ratingprofile);
            }
            App.Geocaches = new SortableBindingList<Geocache>(App.Geocaches.OrderByDescending(x => x.Rating).ToList());
            Startup.BindLists();//Since bindiing is lost when new list is created
            App.DB.MaximalRating = App.Geocaches[0].Rating;//Da sortierte Liste
            App.DB.MinimalRating = App.Geocaches[App.Geocaches.Count - 1].Rating;
            Map_RenewGeocacheLayer();
            UpdateStatus("Geocaches rated");
            Fileoperations.Backup(Databases.Geocaches);
            return true;
        }

        /// <summary>
        /// Sets the specified Ratingprofile 
        /// </summary>
        /// <param name="RP"></param>
        public void SetRatingprofile(Ratingprofile RP)
        {
            if (EditRatingprofileCombobox.Items.Cast<ComboBoxItem>().Count(x => x.Content.ToString() == RP.Name) > 0)
            {
                EditRatingprofileCombobox.SelectedItem = EditRatingprofileCombobox.Items.Cast<ComboBoxItem>().First(x => x.Content.ToString() == RP.Name);
            }
        }
        #endregion
    }
}

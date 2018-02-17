using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GeocachingTourPlanner
{
    public partial class NewRatingProfileWindow : Form
    {
        public NewRatingProfileWindow()
        {
            InitializeComponent();
        }

        public NewRatingProfileWindow(Ratingprofile RatingprofileToEdit)
        {
            InitializeComponent();
			try
			{
				Text = "Edit Profile";
				//Name des Profils
				NameValue.Text = RatingprofileToEdit.Name;

				//Prioritäten
				TypePriorityvalue.SelectedItem = TypePriorityvalue.Items[TypePriorityvalue.Items.IndexOf(RatingprofileToEdit.TypePriority.ToString())];
				GrößenPrioritätValue.SelectedItem = GrößenPrioritätValue.Items[TypePriorityvalue.Items.IndexOf(RatingprofileToEdit.SizePriority.ToString())];
				DPrioritätenValue.SelectedItem = DPrioritätenValue.Items[TypePriorityvalue.Items.IndexOf(RatingprofileToEdit.DPriority.ToString())];
				TPrioritätenValue.SelectedItem = TPrioritätenValue.Items[TypePriorityvalue.Items.IndexOf(RatingprofileToEdit.TPriority.ToString())];

				//TypenValueungen
				Traditionalvalue.SelectedItem = Traditionalvalue.Items[Traditionalvalue.Items.IndexOf(RatingprofileToEdit.TypeRatings.First(x => x.Key == GeocacheType.Traditional).Value.ToString())];
				EarthcacheValue.SelectedItem = EarthcacheValue.Items[EarthcacheValue.Items.IndexOf(RatingprofileToEdit.TypeRatings.First(x => x.Key == GeocacheType.EarthCache).Value.ToString())];
				Multivalue.SelectedItem = Multivalue.Items[Multivalue.Items.IndexOf(RatingprofileToEdit.TypeRatings.First(x => x.Key == GeocacheType.Multi).Value.ToString())];
				MysteryValue.SelectedItem = MysteryValue.Items[MysteryValue.Items.IndexOf(RatingprofileToEdit.TypeRatings.First(x => x.Key == GeocacheType.Mystery).Value.ToString())];
				LetterboxValue.SelectedItem = LetterboxValue.Items[LetterboxValue.Items.IndexOf(RatingprofileToEdit.TypeRatings.First(x => x.Key == GeocacheType.Letterbox).Value.ToString())];
				VirtualValue.SelectedItem = VirtualValue.Items[VirtualValue.Items.IndexOf(RatingprofileToEdit.TypeRatings.First(x => x.Key == GeocacheType.Virtual).Value.ToString())];
				OtherTypeValue.SelectedItem = OtherTypeValue.Items[OtherTypeValue.Items.IndexOf(RatingprofileToEdit.TypeRatings.First(x => x.Key == GeocacheType.Other).Value.ToString())];
				WebcamValue.SelectedItem = WebcamValue.Items[WebcamValue.Items.IndexOf(RatingprofileToEdit.TypeRatings.First(x => x.Key == GeocacheType.Webcam).Value.ToString())];
				WherigoValue.SelectedItem = WherigoValue.Items[WherigoValue.Items.IndexOf(RatingprofileToEdit.TypeRatings.First(x => x.Key == GeocacheType.Wherigo).Value.ToString())];

				//Größe
				LargeValue.SelectedItem = LargeValue.Items[LargeValue.Items.IndexOf(RatingprofileToEdit.SizeRatings.First(x => x.Key == GeocacheSize.Large).Value.ToString())];
				RegularValue.SelectedItem = RegularValue.Items[RegularValue.Items.IndexOf(RatingprofileToEdit.SizeRatings.First(x => x.Key == GeocacheSize.Regular).Value.ToString())];
				SmallValue.SelectedItem = SmallValue.Items[SmallValue.Items.IndexOf(RatingprofileToEdit.SizeRatings.First(x => x.Key == GeocacheSize.Small).Value.ToString())];
				MicroValue.SelectedItem = MicroValue.Items[MicroValue.Items.IndexOf(RatingprofileToEdit.SizeRatings.First(x => x.Key == GeocacheSize.Micro).Value.ToString())];
				OtherGrößeValue.SelectedItem = OtherGrößeValue.Items[OtherGrößeValue.Items.IndexOf(RatingprofileToEdit.SizeRatings.First(x => x.Key == GeocacheSize.Other).Value.ToString())];

				//D
				D1Value.SelectedItem = D1Value.Items[D1Value.Items.IndexOf(RatingprofileToEdit.DRatings.First(x => x.Key == 1).Value.ToString())];
				D15Value.SelectedItem = D15Value.Items[D15Value.Items.IndexOf(RatingprofileToEdit.DRatings.First(x => x.Key == 1.5).Value.ToString())];
				D2Value.SelectedItem = D2Value.Items[D2Value.Items.IndexOf(RatingprofileToEdit.DRatings.First(x => x.Key == 2).Value.ToString())];
				D25Value.SelectedItem = D25Value.Items[D25Value.Items.IndexOf(RatingprofileToEdit.DRatings.First(x => x.Key == 2.5).Value.ToString())];
				D3Value.SelectedItem = D3Value.Items[D3Value.Items.IndexOf(RatingprofileToEdit.DRatings.First(x => x.Key == 3).Value.ToString())];
				D35Value.SelectedItem = D35Value.Items[D35Value.Items.IndexOf(RatingprofileToEdit.DRatings.First(x => x.Key == 3.5).Value.ToString())];
				D4Value.SelectedItem = D4Value.Items[D4Value.Items.IndexOf(RatingprofileToEdit.DRatings.First(x => x.Key == 4).Value.ToString())];
				D45Value.SelectedItem = D45Value.Items[D45Value.Items.IndexOf(RatingprofileToEdit.DRatings.First(x => x.Key == 4.5).Value.ToString())];
				D5Value.SelectedItem = D5Value.Items[D5Value.Items.IndexOf(RatingprofileToEdit.DRatings.First(x => x.Key == 5).Value.ToString())];

				//T
				T1Value.SelectedItem = T1Value.Items[T1Value.Items.IndexOf(RatingprofileToEdit.DRatings.First(x => x.Key == 1).Value.ToString())];
				T15Value.SelectedItem = T15Value.Items[T15Value.Items.IndexOf(RatingprofileToEdit.DRatings.First(x => x.Key == 1.5).Value.ToString())];
				T2Value.SelectedItem = T2Value.Items[T2Value.Items.IndexOf(RatingprofileToEdit.DRatings.First(x => x.Key == 2).Value.ToString())];
				T25Value.SelectedItem = T25Value.Items[T25Value.Items.IndexOf(RatingprofileToEdit.DRatings.First(x => x.Key == 2.5).Value.ToString())];
				T3Value.SelectedItem = T3Value.Items[T3Value.Items.IndexOf(RatingprofileToEdit.DRatings.First(x => x.Key == 3).Value.ToString())];
				T35Value.SelectedItem = T35Value.Items[T35Value.Items.IndexOf(RatingprofileToEdit.DRatings.First(x => x.Key == 3.5).Value.ToString())];
				T4Value.SelectedItem = T4Value.Items[T4Value.Items.IndexOf(RatingprofileToEdit.DRatings.First(x => x.Key == 4).Value.ToString())];
				T45Value.SelectedItem = T45Value.Items[T45Value.Items.IndexOf(RatingprofileToEdit.DRatings.First(x => x.Key == 4.5).Value.ToString())];
				T5Value.SelectedItem = T5Value.Items[T5Value.Items.IndexOf(RatingprofileToEdit.DRatings.First(x => x.Key == 5).Value.ToString())];

				//Sonstige
				NMFlagValue.Text = RatingprofileToEdit.NMPenalty.ToString();
				if (RatingprofileToEdit.Yearmode == true)
				{
					AgeValue.SelectedItem = AgeValue.Items[0];
				}
				else
				{
					AgeValue.SelectedItem = AgeValue.Items[1];
				}
				AlterZahlValue.SelectedItem = AlterZahlValue.Items[AlterZahlValue.Items.IndexOf(RatingprofileToEdit.Yearfactor.ToString())];
			}
			catch (Exception)
			{
				DialogResult aw = MessageBox.Show("Es scheint einen Fehler in der Datei zu diesem Profil zu geben. Wollen sie es Löschen?", "Fehler", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
				if (aw == DialogResult.Yes)
				{
					Program.Ratingprofiles.Remove(RatingprofileToEdit);
				}
			}
		}
            

        private void CancelNewProfileButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void CreateButton_Click(object sender, EventArgs e)
        {
            Ratingprofile Profil = new Ratingprofile();
            if (NameValue.Text == null)
            {
                MessageBox.Show("Bitte Namen festlegen");
                return;
            }
            try
            {
                Profil.Name = NameValue.Text;
                Profil.TypePriority = int.Parse(TypePriorityvalue.Text);
                Profil.TypeRatings = new List<KeyValuePair<GeocacheType, int>>();
                Profil.TypeRatings.Add(new KeyValuePair<GeocacheType, int>(GeocacheType.EarthCache, int.Parse(EarthcacheValue.Text)));
                Profil.TypeRatings.Add(new KeyValuePair<GeocacheType, int>(GeocacheType.Letterbox, int.Parse(LetterboxValue.Text)));
                Profil.TypeRatings.Add(new KeyValuePair<GeocacheType, int>(GeocacheType.Multi, int.Parse(Multivalue.Text)));
                Profil.TypeRatings.Add(new KeyValuePair<GeocacheType, int>(GeocacheType.Mystery, int.Parse(MysteryValue.Text)));
                Profil.TypeRatings.Add(new KeyValuePair<GeocacheType, int>(GeocacheType.Other, int.Parse(OtherTypeValue.Text)));
                Profil.TypeRatings.Add(new KeyValuePair<GeocacheType, int>(GeocacheType.Traditional, int.Parse(Traditionalvalue.Text)));
                Profil.TypeRatings.Add(new KeyValuePair<GeocacheType, int>(GeocacheType.Virtual, int.Parse(VirtualValue.Text)));
                Profil.TypeRatings.Add(new KeyValuePair<GeocacheType, int>(GeocacheType.Webcam, int.Parse(WebcamValue.Text)));
                Profil.TypeRatings.Add(new KeyValuePair<GeocacheType, int>(GeocacheType.Wherigo, int.Parse(WherigoValue.Text)));

                Profil.SizePriority = int.Parse(GrößenPrioritätValue.Text);
                Profil.SizeRatings = new List<KeyValuePair<GeocacheSize, int>>();
                Profil.SizeRatings.Add(new KeyValuePair<GeocacheSize, int>(GeocacheSize.Large, int.Parse(LargeValue.Text)));
                Profil.SizeRatings.Add(new KeyValuePair<GeocacheSize, int>(GeocacheSize.Micro, int.Parse(MicroValue.Text)));
                Profil.SizeRatings.Add(new KeyValuePair<GeocacheSize, int>(GeocacheSize.Other, int.Parse(OtherGrößeValue.Text)));
                Profil.SizeRatings.Add(new KeyValuePair<GeocacheSize, int>(GeocacheSize.Regular, int.Parse(RegularValue.Text)));
                Profil.SizeRatings.Add(new KeyValuePair<GeocacheSize, int>(GeocacheSize.Small, int.Parse(SmallValue.Text)));

                Profil.DPriority = int.Parse(DPrioritätenValue.Text);
                Profil.DRatings = new List<KeyValuePair<float, int>>();
                Profil.DRatings.Add(new KeyValuePair<float, int>(1f, int.Parse(D1Value.Text)));
                Profil.DRatings.Add(new KeyValuePair<float, int>(1.5f, int.Parse(D15Value.Text)));
                Profil.DRatings.Add(new KeyValuePair<float, int>(2f, int.Parse(D2Value.Text)));
                Profil.DRatings.Add(new KeyValuePair<float, int>(2.5f, int.Parse(D25Value.Text)));
                Profil.DRatings.Add(new KeyValuePair<float, int>(3f, int.Parse(D3Value.Text)));
                Profil.DRatings.Add(new KeyValuePair<float, int>(3.5f, int.Parse(D35Value.Text)));
                Profil.DRatings.Add(new KeyValuePair<float, int>(4f, int.Parse(D4Value.Text)));
                Profil.DRatings.Add(new KeyValuePair<float, int>(4.5f, int.Parse(D45Value.Text)));
                Profil.DRatings.Add(new KeyValuePair<float, int>(5f, int.Parse(D5Value.Text)));

                Profil.TPriority = int.Parse(TPrioritätenValue.Text);
                Profil.TRatings = new List<KeyValuePair<float, int>>();
                Profil.TRatings.Add(new KeyValuePair<float, int>(1f, int.Parse(T1Value.Text)));
                Profil.TRatings.Add(new KeyValuePair<float, int>(1.5f, int.Parse(T15Value.Text)));
                Profil.TRatings.Add(new KeyValuePair<float, int>(2f, int.Parse(T2Value.Text)));
                Profil.TRatings.Add(new KeyValuePair<float, int>(2.5f, int.Parse(T25Value.Text)));
                Profil.TRatings.Add(new KeyValuePair<float, int>(3f, int.Parse(T3Value.Text)));
                Profil.TRatings.Add(new KeyValuePair<float, int>(3.5f, int.Parse(T35Value.Text)));
                Profil.TRatings.Add(new KeyValuePair<float, int>(4f, int.Parse(T4Value.Text)));
                Profil.TRatings.Add(new KeyValuePair<float, int>(4.5f, int.Parse(T45Value.Text)));
                Profil.TRatings.Add(new KeyValuePair<float, int>(5f, int.Parse(T5Value.Text)));

                if(!int.TryParse(NMFlagValue.Text.Replace("-",""),out int Value))
                {
                    MessageBox.Show("Please write only positive whole numbers into the field with the NMPenalty");
                }
                else
                {
                    Profil.NMPenalty = Value;
                }
             
                if(AgeValue.SelectedItem.ToString()== "multiply with")
                {
                    Profil.Yearmode = true;
                }
                else
                {
                    Profil.Yearmode = false;
                }

                Profil.Yearfactor = int.Parse(AlterZahlValue.Text);

            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Please fill all fields");
                return;
            }

            //Eintragen des neuen Profils
            foreach(Ratingprofile BP in Program.Ratingprofiles.Where(x => x.Name == Profil.Name).ToList())
			{
				Program.Ratingprofiles.Remove(BP);
			}
            Program.Ratingprofiles.Add(Profil);
			if (Program.Backup(Program.Ratingprofiles))
			{
				Close();
			}
        }

		private void Dropdown_SelectedIndexChanged(object sender, EventArgs e)
		{
			((ComboBox)sender).Text = ((ComboBox)sender).SelectedItem.ToString();//So I can just check the text and it doesn't matter whether the user typed it or selected it
		}
	}
}

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
    public partial class NeuesBewertungsProfilFenster : Form
    {
        public NeuesBewertungsProfilFenster()
        {
            InitializeComponent();
        }

        public NeuesBewertungsProfilFenster(Ratingprofile bewertungsprofil)
        {
            InitializeComponent();
			try
			{
				Text = "Profil bearbeiten";
				//Name des Profils
				NameWert.Text = bewertungsprofil.Name;

				//Prioritäten
				TypPrioritätenWert.SelectedItem = TypPrioritätenWert.Items[TypPrioritätenWert.Items.IndexOf(bewertungsprofil.TypePriority.ToString())];
				GrößenPrioritätWert.SelectedItem = GrößenPrioritätWert.Items[TypPrioritätenWert.Items.IndexOf(bewertungsprofil.SizePriority.ToString())];
				DPrioritätenWert.SelectedItem = DPrioritätenWert.Items[TypPrioritätenWert.Items.IndexOf(bewertungsprofil.DPriority.ToString())];
				TPrioritätenWert.SelectedItem = TPrioritätenWert.Items[TypPrioritätenWert.Items.IndexOf(bewertungsprofil.TPriority.ToString())];

				//Typenwertungen
				TraditionalWert.SelectedItem = TraditionalWert.Items[TraditionalWert.Items.IndexOf(bewertungsprofil.TypeRatings.First(x => x.Key == GeocacheType.Traditional).Value.ToString())];
				EarthcacheWert.SelectedItem = EarthcacheWert.Items[EarthcacheWert.Items.IndexOf(bewertungsprofil.TypeRatings.First(x => x.Key == GeocacheType.EarthCache).Value.ToString())];
				MultiWert.SelectedItem = MultiWert.Items[MultiWert.Items.IndexOf(bewertungsprofil.TypeRatings.First(x => x.Key == GeocacheType.Multi).Value.ToString())];
				MysteryWert.SelectedItem = MysteryWert.Items[MysteryWert.Items.IndexOf(bewertungsprofil.TypeRatings.First(x => x.Key == GeocacheType.Mystery).Value.ToString())];
				LetterboxWert.SelectedItem = LetterboxWert.Items[LetterboxWert.Items.IndexOf(bewertungsprofil.TypeRatings.First(x => x.Key == GeocacheType.Letterbox).Value.ToString())];
				VirtualWert.SelectedItem = VirtualWert.Items[VirtualWert.Items.IndexOf(bewertungsprofil.TypeRatings.First(x => x.Key == GeocacheType.Virtual).Value.ToString())];
				OtherTypWert.SelectedItem = OtherTypWert.Items[OtherTypWert.Items.IndexOf(bewertungsprofil.TypeRatings.First(x => x.Key == GeocacheType.Other).Value.ToString())];
				WebcamWert.SelectedItem = WebcamWert.Items[WebcamWert.Items.IndexOf(bewertungsprofil.TypeRatings.First(x => x.Key == GeocacheType.Webcam).Value.ToString())];
				WherigoWert.SelectedItem = WherigoWert.Items[WherigoWert.Items.IndexOf(bewertungsprofil.TypeRatings.First(x => x.Key == GeocacheType.Wherigo).Value.ToString())];

				//Größe
				LargeWert.SelectedItem = LargeWert.Items[LargeWert.Items.IndexOf(bewertungsprofil.SizeRatings.First(x => x.Key == GeocacheSize.Large).Value.ToString())];
				RegularWert.SelectedItem = RegularWert.Items[RegularWert.Items.IndexOf(bewertungsprofil.SizeRatings.First(x => x.Key == GeocacheSize.Regular).Value.ToString())];
				SmallWert.SelectedItem = SmallWert.Items[SmallWert.Items.IndexOf(bewertungsprofil.SizeRatings.First(x => x.Key == GeocacheSize.Small).Value.ToString())];
				MicroWert.SelectedItem = MicroWert.Items[MicroWert.Items.IndexOf(bewertungsprofil.SizeRatings.First(x => x.Key == GeocacheSize.Micro).Value.ToString())];
				OtherGrößeWert.SelectedItem = OtherGrößeWert.Items[OtherGrößeWert.Items.IndexOf(bewertungsprofil.SizeRatings.First(x => x.Key == GeocacheSize.Other).Value.ToString())];

				//D
				D1Wert.SelectedItem = D1Wert.Items[D1Wert.Items.IndexOf(bewertungsprofil.DRatings.First(x => x.Key == 1).Value.ToString())];
				D15Wert.SelectedItem = D15Wert.Items[D15Wert.Items.IndexOf(bewertungsprofil.DRatings.First(x => x.Key == 1.5).Value.ToString())];
				D2Wert.SelectedItem = D2Wert.Items[D2Wert.Items.IndexOf(bewertungsprofil.DRatings.First(x => x.Key == 2).Value.ToString())];
				D25Wert.SelectedItem = D25Wert.Items[D25Wert.Items.IndexOf(bewertungsprofil.DRatings.First(x => x.Key == 2.5).Value.ToString())];
				D3Wert.SelectedItem = D3Wert.Items[D3Wert.Items.IndexOf(bewertungsprofil.DRatings.First(x => x.Key == 3).Value.ToString())];
				D35Wert.SelectedItem = D35Wert.Items[D35Wert.Items.IndexOf(bewertungsprofil.DRatings.First(x => x.Key == 3.5).Value.ToString())];
				D4Wert.SelectedItem = D4Wert.Items[D4Wert.Items.IndexOf(bewertungsprofil.DRatings.First(x => x.Key == 4).Value.ToString())];
				D45Wert.SelectedItem = D45Wert.Items[D45Wert.Items.IndexOf(bewertungsprofil.DRatings.First(x => x.Key == 4.5).Value.ToString())];
				D5Wert.SelectedItem = D5Wert.Items[D5Wert.Items.IndexOf(bewertungsprofil.DRatings.First(x => x.Key == 5).Value.ToString())];

				//T
				T1Wert.SelectedItem = T1Wert.Items[T1Wert.Items.IndexOf(bewertungsprofil.DRatings.First(x => x.Key == 1).Value.ToString())];
				T15Wert.SelectedItem = T15Wert.Items[T15Wert.Items.IndexOf(bewertungsprofil.DRatings.First(x => x.Key == 1.5).Value.ToString())];
				T2Wert.SelectedItem = T2Wert.Items[T2Wert.Items.IndexOf(bewertungsprofil.DRatings.First(x => x.Key == 2).Value.ToString())];
				T25Wert.SelectedItem = T25Wert.Items[T25Wert.Items.IndexOf(bewertungsprofil.DRatings.First(x => x.Key == 2.5).Value.ToString())];
				T3Wert.SelectedItem = T3Wert.Items[T3Wert.Items.IndexOf(bewertungsprofil.DRatings.First(x => x.Key == 3).Value.ToString())];
				T35Wert.SelectedItem = T35Wert.Items[T35Wert.Items.IndexOf(bewertungsprofil.DRatings.First(x => x.Key == 3.5).Value.ToString())];
				T4Wert.SelectedItem = T4Wert.Items[T4Wert.Items.IndexOf(bewertungsprofil.DRatings.First(x => x.Key == 4).Value.ToString())];
				T45Wert.SelectedItem = T45Wert.Items[T45Wert.Items.IndexOf(bewertungsprofil.DRatings.First(x => x.Key == 4.5).Value.ToString())];
				T5Wert.SelectedItem = T5Wert.Items[T5Wert.Items.IndexOf(bewertungsprofil.DRatings.First(x => x.Key == 5).Value.ToString())];

				//Sonstige
				NMFlagWert.Text = bewertungsprofil.NMPenalty.ToString();
				if (bewertungsprofil.Yearmode == true)
				{
					AlterWert.SelectedItem = AlterWert.Items[0];
				}
				else
				{
					AlterWert.SelectedItem = AlterWert.Items[1];
				}
				AlterZahlWert.SelectedItem = AlterZahlWert.Items[AlterZahlWert.Items.IndexOf(bewertungsprofil.Yearfactor.ToString())];
			}
			catch (Exception)
			{
				DialogResult aw = MessageBox.Show("Es scheint einen Fehler in der Datei zu diesem Profil zu geben. Wollen sie es Löschen?", "Fehler", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
				if (aw == DialogResult.Yes)
				{
					Program.Ratingprofiles.Remove(bewertungsprofil);
				}
			}
		}
            

        private void AbbrechenKnopf_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ErstellenKnopf_Click(object sender, EventArgs e)
        {
            Ratingprofile Profil = new Ratingprofile();
            if (NameWert.Text == null)
            {
                MessageBox.Show("Bitte Namen festlegen");
                return;
            }
            try
            {
                Profil.Name = NameWert.Text;
                Profil.TypePriority = int.Parse(TypPrioritätenWert.SelectedItem.ToString());
                Profil.TypeRatings = new List<KeyValuePair<GeocacheType, int>>();
                Profil.TypeRatings.Add(new KeyValuePair<GeocacheType, int>(GeocacheType.EarthCache, int.Parse(EarthcacheWert.SelectedItem.ToString())));
                Profil.TypeRatings.Add(new KeyValuePair<GeocacheType, int>(GeocacheType.Letterbox, int.Parse(LetterboxWert.SelectedItem.ToString())));
                Profil.TypeRatings.Add(new KeyValuePair<GeocacheType, int>(GeocacheType.Multi, int.Parse(MultiWert.SelectedItem.ToString())));
                Profil.TypeRatings.Add(new KeyValuePair<GeocacheType, int>(GeocacheType.Mystery, int.Parse(MysteryWert.SelectedItem.ToString())));
                Profil.TypeRatings.Add(new KeyValuePair<GeocacheType, int>(GeocacheType.Other, int.Parse(OtherTypWert.SelectedItem.ToString())));
                Profil.TypeRatings.Add(new KeyValuePair<GeocacheType, int>(GeocacheType.Traditional, int.Parse(TraditionalWert.SelectedItem.ToString())));
                Profil.TypeRatings.Add(new KeyValuePair<GeocacheType, int>(GeocacheType.Virtual, int.Parse(VirtualWert.SelectedItem.ToString())));
                Profil.TypeRatings.Add(new KeyValuePair<GeocacheType, int>(GeocacheType.Webcam, int.Parse(WebcamWert.SelectedItem.ToString())));
                Profil.TypeRatings.Add(new KeyValuePair<GeocacheType, int>(GeocacheType.Wherigo, int.Parse(WherigoWert.SelectedItem.ToString())));

                Profil.SizePriority = int.Parse(GrößenPrioritätWert.SelectedItem.ToString());
                Profil.SizeRatings = new List<KeyValuePair<GeocacheSize, int>>();
                Profil.SizeRatings.Add(new KeyValuePair<GeocacheSize, int>(GeocacheSize.Large, int.Parse(LargeWert.SelectedItem.ToString())));
                Profil.SizeRatings.Add(new KeyValuePair<GeocacheSize, int>(GeocacheSize.Micro, int.Parse(MicroWert.SelectedItem.ToString())));
                Profil.SizeRatings.Add(new KeyValuePair<GeocacheSize, int>(GeocacheSize.Other, int.Parse(OtherGrößeWert.SelectedItem.ToString())));
                Profil.SizeRatings.Add(new KeyValuePair<GeocacheSize, int>(GeocacheSize.Regular, int.Parse(RegularWert.SelectedItem.ToString())));
                Profil.SizeRatings.Add(new KeyValuePair<GeocacheSize, int>(GeocacheSize.Small, int.Parse(SmallWert.SelectedItem.ToString())));

                Profil.DPriority = int.Parse(DPrioritätenWert.SelectedItem.ToString());
                Profil.DRatings = new List<KeyValuePair<float, int>>();
                Profil.DRatings.Add(new KeyValuePair<float, int>(1f, int.Parse(D1Wert.SelectedItem.ToString())));
                Profil.DRatings.Add(new KeyValuePair<float, int>(1.5f, int.Parse(D15Wert.SelectedItem.ToString())));
                Profil.DRatings.Add(new KeyValuePair<float, int>(2f, int.Parse(D2Wert.SelectedItem.ToString())));
                Profil.DRatings.Add(new KeyValuePair<float, int>(2.5f, int.Parse(D25Wert.SelectedItem.ToString())));
                Profil.DRatings.Add(new KeyValuePair<float, int>(3f, int.Parse(D3Wert.SelectedItem.ToString())));
                Profil.DRatings.Add(new KeyValuePair<float, int>(3.5f, int.Parse(D35Wert.SelectedItem.ToString())));
                Profil.DRatings.Add(new KeyValuePair<float, int>(4f, int.Parse(D4Wert.SelectedItem.ToString())));
                Profil.DRatings.Add(new KeyValuePair<float, int>(4.5f, int.Parse(D45Wert.SelectedItem.ToString())));
                Profil.DRatings.Add(new KeyValuePair<float, int>(5f, int.Parse(D5Wert.SelectedItem.ToString())));

                Profil.TPriority = int.Parse(TPrioritätenWert.SelectedItem.ToString());
                Profil.TRatings = new List<KeyValuePair<float, int>>();
                Profil.TRatings.Add(new KeyValuePair<float, int>(1f, int.Parse(T1Wert.SelectedItem.ToString())));
                Profil.TRatings.Add(new KeyValuePair<float, int>(1.5f, int.Parse(T15Wert.SelectedItem.ToString())));
                Profil.TRatings.Add(new KeyValuePair<float, int>(2f, int.Parse(T2Wert.SelectedItem.ToString())));
                Profil.TRatings.Add(new KeyValuePair<float, int>(2.5f, int.Parse(T25Wert.SelectedItem.ToString())));
                Profil.TRatings.Add(new KeyValuePair<float, int>(3f, int.Parse(T3Wert.SelectedItem.ToString())));
                Profil.TRatings.Add(new KeyValuePair<float, int>(3.5f, int.Parse(T35Wert.SelectedItem.ToString())));
                Profil.TRatings.Add(new KeyValuePair<float, int>(4f, int.Parse(T4Wert.SelectedItem.ToString())));
                Profil.TRatings.Add(new KeyValuePair<float, int>(4.5f, int.Parse(T45Wert.SelectedItem.ToString())));
                Profil.TRatings.Add(new KeyValuePair<float, int>(5f, int.Parse(T5Wert.SelectedItem.ToString())));

                if(!int.TryParse(NMFlagWert.Text.Replace("-",""),out int wert))
                {
                    MessageBox.Show("Bitte nur positive Zahlen in das Feld mit den Strafpunkten für Needs Maintenance eingeben");
                }
                else
                {
                    Profil.NMPenalty = wert;
                }
             
                if(AlterWert.SelectedItem.ToString()== "mit x multiplizieren")
                {
                    Profil.Yearmode = true;
                }
                else
                {
                    Profil.Yearmode = false;
                }

                Profil.Yearfactor = int.Parse(AlterZahlWert.SelectedItem.ToString());

            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Bitte alle Felder ausfüllen");
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
		
	}
}

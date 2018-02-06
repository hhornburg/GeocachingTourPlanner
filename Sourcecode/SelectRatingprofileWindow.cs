using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tourenplaner
{
	public partial class BewertungsprofilAuswählen : Form
	{
		public BewertungsprofilAuswählen()
		{
			InitializeComponent();
		}

		private void OKButton_Click(object sender, EventArgs e)
		{
			if (ProfilCombobox.SelectedItem != null)
			{
				Ratingprofile bewertungsprofil = Program.Ratingprofiles.First(x => x.Name == ProfilCombobox.SelectedItem.ToString());
				foreach (Geocache GC in Program.Geocaches)
				{
					GC.Bewerten(bewertungsprofil);
				}
				Program.Geocaches.OrderByDescending(x => x.Rating);
				Program.MainWindow.GeocacheTable.Sort(Program.MainWindow.GeocacheTable.Columns["Bewertung"], ListSortDirection.Descending);
				Program.DB.MaximalRating = Program.Geocaches[0].Rating;//Da sortierte Liste
				Program.DB.MinimalRating = Program.Geocaches[Program.Geocaches.Count-1].Rating;
				Program.Backup(Program.Geocaches);
				Close();
			}
		}

		private void AbbrechenButton_Click(object sender, EventArgs e)
		{
			Close();
		}
	}
}

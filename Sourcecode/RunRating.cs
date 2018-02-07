using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GeocachingTourPlanner
{
	public partial class RunRating : Form
	{
		public RunRating()
		{
			InitializeComponent();
		}

		private void StartRatingButton_Click(object sender, EventArgs e)
		{
			if (RatingProfilesCombobox.SelectedItem != null)
			{
				if (RatingProfilesCombobox.SelectedItem == null)
				{
					MessageBox.Show("Please select a Ratingprofile");
					return;
				}

				Ratingprofile bewertungsprofil = Program.Ratingprofiles.First(x => x.Name == RatingProfilesCombobox.SelectedItem.ToString());
				foreach (Geocache GC in Program.Geocaches)
				{
					GC.Rate(bewertungsprofil);
				}
				Program.Geocaches.OrderByDescending(x => x.Rating);
				Program.MainWindow.GeocacheTable.Sort(Program.MainWindow.GeocacheTable.Columns["Rating"], ListSortDirection.Descending);
				Program.DB.MaximalRating = Program.Geocaches[0].Rating;//Da sortierte Liste
				Program.DB.MinimalRating = Program.Geocaches[Program.Geocaches.Count - 1].Rating;
				Program.Backup(Program.Geocaches);
				Close();
			}
		}

		private void CancelRatingButton_Click(object sender, EventArgs e)
		{
			Close();
		}
	}
}

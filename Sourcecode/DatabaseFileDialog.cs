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
	public partial class DatabaseFileDialog : Form
	{
		Databases ThisDB;

		public DatabaseFileDialog(Databases Database)
		{
			InitializeComponent();

			switch (Database)
			{
				case Databases.Geocaches:
					MessageText.Text = "Couldn't find a geocaches database.";
					New_ImportButton.Text = "Import Pocket query";
					break;
				case Databases.Ratingprofiles:
					MessageText.Text = "Couldn't find a ratingprofiles database.";
					New_ImportButton.Text = "Create new File";
					break;
				case Databases.Routingprofiles:
					MessageText.Text = "Couldn't find a routingprofiles database.";
					New_ImportButton.Text = "Create new File";
					break;
				case Databases.RouterDB:
					MessageText.Text = "Couldn't find a RouterDB database.";
					New_ImportButton.Text = "Import .pbf File";
					break;
			}
		}

		private void SetButton_Click(object sender, EventArgs e)
		{
			Program.DB.SetDatabaseFilepath(ThisDB);
			DialogResult = DialogResult.Retry;
		}

		private void New_ImportButton_Click(object sender, EventArgs e)
		{
			switch (ThisDB)
			{
				case Databases.Geocaches:
					Fileoperations.ImportGeocaches();
					break;
				case Databases.Ratingprofiles:
					Fileoperations.NewRatingprofileDatabase();
					break;
				case Databases.Routingprofiles:
					Fileoperations.NewRoutingprofileDatabase();
					break;
				case Databases.RouterDB:
					Fileoperations.ImportOSMData();
					break;
			}
			DialogResult = DialogResult.Retry;
		}

		private void CancelButton_Click(object sender, EventArgs e)
		{
			Close();
			DialogResult = DialogResult.Cancel;
		}
	}
}

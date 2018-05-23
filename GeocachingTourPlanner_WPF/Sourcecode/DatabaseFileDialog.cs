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

		/// <summary>
		/// Use only with .ShowDialog(). Returns OK if a database is set.
		/// </summary>
		/// <param name="Database">Database for which thiis dialog is shown</param>
		/// <param name="AllowImport">Defaults to true. If  false, the import button is removed</param>
		public DatabaseFileDialog(Databases Database, bool AllowImport = true)
		{
			InitializeComponent();

			switch (Database)
			{
				case Databases.Geocaches:
					MessageText.Text = "Couldn't find a geocaches database.";
					if (AllowImport)
					{
						New_ImportButton.Text = "Import Pocket query";
					}
					else
					{
						New_ImportButton.Dispose();
					}
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
					if (AllowImport)
					{
						New_ImportButton.Text = "Import .pbf File";
					}
					else
					{
						New_ImportButton.Dispose();
					}
			break;
			}
		}

		private void OpenButton_Click(object sender, EventArgs e)
		{
			if (App.DB.OpenExistingDBFile(ThisDB))
			{
				DialogResult = DialogResult.OK;
			}
			else
			{
				DialogResult = DialogResult.Cancel;
			}
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

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
	public partial class NewRoutingprofileWindow : Form
	{
		public NewRoutingprofileWindow()
		{
			InitializeComponent();
		}

		public NewRoutingprofileWindow(Routingprofile RoutingProfileToEdit)
		{
			InitializeComponent();
			NameValue.Text = RoutingProfileToEdit.Name;

			//Distance
			MaxDistance.Text = RoutingProfileToEdit.MaxDistance.ToString();
			PenaltyPerExtraKM.Text = RoutingProfileToEdit.PenaltyPerExtraKM.ToString();

			//Time
			MaxTime.Text = RoutingProfileToEdit.MaxTime.ToString();
			PenaltyPerExtraKM.Text = RoutingProfileToEdit.PenaltyPerExtra10min.ToString();
			TimePerGeocache.Text = RoutingProfileToEdit.TimePerGeocache.ToString();

			//Profile
			/*VehicleValue.Text = RoutingProfileToEdit.ItineroProfile.Name;
			switch (RoutingProfileToEdit.ItineroProfile.Metric)
			{
				case Itinero.Profiles.ProfileMetric.DistanceInMeters:

					ModeValue.Text = "Distance";
					break;

				case Itinero.Profiles.ProfileMetric.TimeInSeconds:
					ModeValue.SelectedText = "Time";
					break;
			}*/
		}

		private void CancelNewProfileButton_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void CreateButton_Click(object sender, EventArgs e)
		{
			Routingprofile Profile = new Routingprofile();
			if (NameValue.Text == null)
			{
				MessageBox.Show("Please set Name");
				return;
			}
			try
			{
				Profile.Name = NameValue.Text;

				Profile.MaxDistance = int.Parse(MaxDistance.Text);
				Profile.PenaltyPerExtraKM = int.Parse(PenaltyPerExtraKM.Text);

				Profile.MaxTime = int.Parse(MaxTime.Text);
				Profile.PenaltyPerExtra10min = int.Parse(PenaltyPerExtra10min.Text);
				Profile.TimePerGeocache = int.Parse(TimePerGeocache.Text);
				/*
				Profile.ItineroProfile = Itinero.Profiles.Profile.GetRegistered(VehicleValue.Text);
				*/
			}
			catch (NullReferenceException)
			{
				MessageBox.Show("Please fill all fields");
				return;
			}

			//Eintragen des neuen Profils
			foreach (Routingprofile BP in Program.Routingprofiles.Where(x => x.Name == Profile.Name).ToList())
			{
				Program.Routingprofiles.Remove(BP);
			}
			Program.Routingprofiles.Add(Profile);
			if (Program.Backup(Program.Routingprofiles))
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

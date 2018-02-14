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
	public partial class AcceptLicenseWindow : Form
	{
		public AcceptLicenseWindow()
		{
			InitializeComponent();
		}

		public bool AcceptedLicense;

		private void Accept_Button_Click(object sender, EventArgs e)
		{
			AcceptedLicense = true;
			Close();
		}

		private void Cancel_Button_Click(object sender, EventArgs e)
		{
			AcceptedLicense = false;
			Close();
		}
	}
}

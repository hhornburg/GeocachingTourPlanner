namespace Tourenplaner
{
	partial class BewertungsprofilAuswählen
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.OKbutton = new System.Windows.Forms.Button();
			this.AbbrechenButton = new System.Windows.Forms.Button();
			this.ProfilCombobox = new System.Windows.Forms.ComboBox();
			this.tableLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.AutoSize = true;
			this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel1.ColumnCount = 3;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.Controls.Add(this.OKbutton, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.AbbrechenButton, 2, 0);
			this.tableLayoutPanel1.Controls.Add(this.ProfilCombobox, 0, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 1;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(440, 51);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// OKbutton
			// 
			this.OKbutton.Location = new System.Drawing.Point(203, 3);
			this.OKbutton.Name = "OKbutton";
			this.OKbutton.Size = new System.Drawing.Size(75, 23);
			this.OKbutton.TabIndex = 0;
			this.OKbutton.Text = "OK";
			this.OKbutton.UseVisualStyleBackColor = true;
			this.OKbutton.Click += new System.EventHandler(this.OKButton_Click);
			// 
			// AbbrechenButton
			// 
			this.AbbrechenButton.Location = new System.Drawing.Point(284, 3);
			this.AbbrechenButton.Name = "AbbrechenButton";
			this.AbbrechenButton.Size = new System.Drawing.Size(75, 23);
			this.AbbrechenButton.TabIndex = 1;
			this.AbbrechenButton.Text = "Abbrechen";
			this.AbbrechenButton.UseVisualStyleBackColor = true;
			this.AbbrechenButton.Click += new System.EventHandler(this.AbbrechenButton_Click);
			// 
			// ProfilCombobox
			// 
			this.ProfilCombobox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ProfilCombobox.FormattingEnabled = true;
			this.ProfilCombobox.Location = new System.Drawing.Point(3, 3);
			this.ProfilCombobox.Name = "ProfilCombobox";
			this.ProfilCombobox.Size = new System.Drawing.Size(194, 21);
			this.ProfilCombobox.TabIndex = 2;
			// 
			// BewertungsprofilAuswählen
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.ClientSize = new System.Drawing.Size(440, 51);
			this.Controls.Add(this.tableLayoutPanel1);
			this.MaximizeBox = false;
			this.Name = "BewertungsprofilAuswählen";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.Text = "Mit welchem Profil soll bewertet werden?";
			this.tableLayoutPanel1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Button OKbutton;
		private System.Windows.Forms.Button AbbrechenButton;
		public System.Windows.Forms.ComboBox ProfilCombobox;
	}
}
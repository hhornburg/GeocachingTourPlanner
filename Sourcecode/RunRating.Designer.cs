namespace GeocachingTourPlanner
{
	partial class RunRating
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
			this.RoutingprofileFormTable = new System.Windows.Forms.TableLayoutPanel();
			this.RatingProfilesCombobox = new System.Windows.Forms.ComboBox();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.CancelRatingButton = new System.Windows.Forms.Button();
			this.StartRatingButton = new System.Windows.Forms.Button();
			this.RoutingprofileFormTable.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// RoutingprofileFormTable
			// 
			this.RoutingprofileFormTable.ColumnCount = 2;
			this.RoutingprofileFormTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.RoutingprofileFormTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.RoutingprofileFormTable.Controls.Add(this.RatingProfilesCombobox, 0, 0);
			this.RoutingprofileFormTable.Dock = System.Windows.Forms.DockStyle.Fill;
			this.RoutingprofileFormTable.Location = new System.Drawing.Point(0, 0);
			this.RoutingprofileFormTable.Name = "RoutingprofileFormTable";
			this.RoutingprofileFormTable.RowCount = 1;
			this.RoutingprofileFormTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.RoutingprofileFormTable.Size = new System.Drawing.Size(503, 219);
			this.RoutingprofileFormTable.TabIndex = 0;
			// 
			// RatingProfilesCombobox
			// 
			this.RatingProfilesCombobox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.RatingProfilesCombobox.FormattingEnabled = true;
			this.RatingProfilesCombobox.Location = new System.Drawing.Point(3, 3);
			this.RatingProfilesCombobox.Name = "RatingProfilesCombobox";
			this.RatingProfilesCombobox.Size = new System.Drawing.Size(245, 21);
			this.RatingProfilesCombobox.TabIndex = 0;
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 3;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
			this.tableLayoutPanel1.Controls.Add(this.CancelRatingButton, 2, 0);
			this.tableLayoutPanel1.Controls.Add(this.StartRatingButton, 1, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 194);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 1;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(503, 25);
			this.tableLayoutPanel1.TabIndex = 1;
			// 
			// CancelRatingButton
			// 
			this.CancelRatingButton.Dock = System.Windows.Forms.DockStyle.Fill;
			this.CancelRatingButton.Location = new System.Drawing.Point(403, 0);
			this.CancelRatingButton.Margin = new System.Windows.Forms.Padding(0);
			this.CancelRatingButton.Name = "CancelRatingButton";
			this.CancelRatingButton.Size = new System.Drawing.Size(100, 25);
			this.CancelRatingButton.TabIndex = 0;
			this.CancelRatingButton.Text = "Cancel";
			this.CancelRatingButton.UseVisualStyleBackColor = true;
			this.CancelRatingButton.Click += new System.EventHandler(this.CancelRatingButton_Click);
			// 
			// StartRatingButton
			// 
			this.StartRatingButton.Dock = System.Windows.Forms.DockStyle.Fill;
			this.StartRatingButton.Location = new System.Drawing.Point(303, 0);
			this.StartRatingButton.Margin = new System.Windows.Forms.Padding(0);
			this.StartRatingButton.Name = "StartRatingButton";
			this.StartRatingButton.Size = new System.Drawing.Size(100, 25);
			this.StartRatingButton.TabIndex = 1;
			this.StartRatingButton.Text = "Start Rating";
			this.StartRatingButton.UseVisualStyleBackColor = true;
			this.StartRatingButton.Click += new System.EventHandler(this.StartRatingButton_Click);
			// 
			// RunRating
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(503, 219);
			this.Controls.Add(this.tableLayoutPanel1);
			this.Controls.Add(this.RoutingprofileFormTable);
			this.Name = "RunRating";
			this.Text = "Run Rating";
			this.RoutingprofileFormTable.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel RoutingprofileFormTable;
		public System.Windows.Forms.ComboBox RatingProfilesCombobox;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Button CancelRatingButton;
		private System.Windows.Forms.Button StartRatingButton;
	}
}
using System.Windows.Forms;

namespace GeocachingTourPlanner
{
	partial class RunRouting
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
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.label1 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.ProfilesCombobox = new System.Windows.Forms.ComboBox();
			this.TargetCombobox = new System.Windows.Forms.ComboBox();
			this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
			this.Map = new GMap.NET.WindowsForms.GMapControl();
			this.label2 = new System.Windows.Forms.Label();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.CancelRatingButton = new System.Windows.Forms.Button();
			this.StartRatingButton = new System.Windows.Forms.Button();
			this.RoutingprofileFormTable.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			this.tableLayoutPanel3.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// RoutingprofileFormTable
			// 
			this.RoutingprofileFormTable.ColumnCount = 2;
			this.RoutingprofileFormTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.RoutingprofileFormTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.RoutingprofileFormTable.Controls.Add(this.tableLayoutPanel2, 0, 0);
			this.RoutingprofileFormTable.Controls.Add(this.tableLayoutPanel3, 1, 0);
			this.RoutingprofileFormTable.Dock = System.Windows.Forms.DockStyle.Fill;
			this.RoutingprofileFormTable.Location = new System.Drawing.Point(0, 0);
			this.RoutingprofileFormTable.Name = "RoutingprofileFormTable";
			this.RoutingprofileFormTable.RowCount = 1;
			this.RoutingprofileFormTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.RoutingprofileFormTable.Size = new System.Drawing.Size(678, 336);
			this.RoutingprofileFormTable.TabIndex = 0;
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.ColumnCount = 1;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.Controls.Add(this.label1, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.label3, 0, 2);
			this.tableLayoutPanel2.Controls.Add(this.ProfilesCombobox, 0, 1);
			this.tableLayoutPanel2.Controls.Add(this.TargetCombobox, 0, 3);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 4;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel2.Size = new System.Drawing.Size(333, 330);
			this.tableLayoutPanel2.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label1.Location = new System.Drawing.Point(3, 3);
			this.label1.Margin = new System.Windows.Forms.Padding(3);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(327, 19);
			this.label1.TabIndex = 0;
			this.label1.Text = "Select Routingprofile";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label3.Location = new System.Drawing.Point(3, 53);
			this.label3.Margin = new System.Windows.Forms.Padding(3);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(327, 19);
			this.label3.TabIndex = 2;
			this.label3.Text = "Target Geocache";
			// 
			// ProfilesCombobox
			// 
			this.ProfilesCombobox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ProfilesCombobox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.ProfilesCombobox.FormattingEnabled = true;
			this.ProfilesCombobox.Location = new System.Drawing.Point(3, 28);
			this.ProfilesCombobox.Name = "ProfilesCombobox";
			this.ProfilesCombobox.Size = new System.Drawing.Size(327, 21);
			this.ProfilesCombobox.TabIndex = 3;
			this.ProfilesCombobox.SelectedIndexChanged += new System.EventHandler(this.ProfilesCombobox_SelectedIndexChanged);
			// 
			// TargetCombobox
			// 
			this.TargetCombobox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TargetCombobox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.TargetCombobox.FormattingEnabled = true;
			this.TargetCombobox.Location = new System.Drawing.Point(3, 78);
			this.TargetCombobox.Name = "TargetCombobox";
			this.TargetCombobox.Size = new System.Drawing.Size(327, 21);
			this.TargetCombobox.TabIndex = 5;
			this.TargetCombobox.SelectedIndexChanged += new System.EventHandler(this.Dropdown_SelectedIndexChanged);
			// 
			// tableLayoutPanel3
			// 
			this.tableLayoutPanel3.ColumnCount = 1;
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel3.Controls.Add(this.Map, 0, 1);
			this.tableLayoutPanel3.Controls.Add(this.label2, 0, 0);
			this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel3.Location = new System.Drawing.Point(342, 3);
			this.tableLayoutPanel3.Name = "tableLayoutPanel3";
			this.tableLayoutPanel3.RowCount = 2;
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel3.Size = new System.Drawing.Size(333, 330);
			this.tableLayoutPanel3.TabIndex = 1;
			// 
			// Map
			// 
			this.Map.Bearing = 0F;
			this.Map.CanDragMap = true;
			this.Map.Dock = System.Windows.Forms.DockStyle.Fill;
			this.Map.EmptyTileColor = System.Drawing.SystemColors.ButtonFace;
			this.Map.GrayScaleMode = false;
			this.Map.HelperLineOption = GMap.NET.WindowsForms.HelperLineOptions.DontShow;
			this.Map.LevelsKeepInMemmory = 5;
			this.Map.Location = new System.Drawing.Point(3, 28);
			this.Map.MarkersEnabled = true;
			this.Map.MaxZoom = 18;
			this.Map.MinZoom = 5;
			this.Map.MouseWheelZoomEnabled = true;
			this.Map.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionAndCenter;
			this.Map.Name = "Map";
			this.Map.NegativeMode = false;
			this.Map.PolygonsEnabled = true;
			this.Map.RetryLoadTile = 0;
			this.Map.RoutesEnabled = true;
			this.Map.ScaleMode = GMap.NET.WindowsForms.ScaleModes.Integer;
			this.Map.SelectedAreaFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(65)))), ((int)(((byte)(105)))), ((int)(((byte)(225)))));
			this.Map.ShowTileGridLines = false;
			this.Map.Size = new System.Drawing.Size(327, 299);
			this.Map.TabIndex = 1;
			this.Map.Zoom = 0D;
			this.Map.OnMapDrag += new GMap.NET.MapDrag(this.Map_OnMapDrag);
			this.Map.OnMapZoomChanged += new GMap.NET.MapZoomChanged(this.Map_OnMapZoomChanged);
			this.Map.Load += new System.EventHandler(this.Map_Load);
			this.Map.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Map_MouseUp);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label2.Location = new System.Drawing.Point(3, 3);
			this.label2.Margin = new System.Windows.Forms.Padding(3);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(327, 19);
			this.label2.TabIndex = 0;
			this.label2.Text = "Select Startingpoint";
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
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 311);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 1;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(678, 25);
			this.tableLayoutPanel1.TabIndex = 1;
			// 
			// CancelRatingButton
			// 
			this.CancelRatingButton.Dock = System.Windows.Forms.DockStyle.Fill;
			this.CancelRatingButton.Location = new System.Drawing.Point(578, 0);
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
			this.StartRatingButton.Location = new System.Drawing.Point(478, 0);
			this.StartRatingButton.Margin = new System.Windows.Forms.Padding(0);
			this.StartRatingButton.Name = "StartRatingButton";
			this.StartRatingButton.Size = new System.Drawing.Size(100, 25);
			this.StartRatingButton.TabIndex = 1;
			this.StartRatingButton.Text = "Start Routing";
			this.StartRatingButton.UseVisualStyleBackColor = true;
			this.StartRatingButton.Click += new System.EventHandler(this.StartRatingButton_Click);
			// 
			// RunRouting
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(678, 336);
			this.Controls.Add(this.tableLayoutPanel1);
			this.Controls.Add(this.RoutingprofileFormTable);
			this.Name = "RunRouting";
			this.Text = "Start Routing";
			this.RoutingprofileFormTable.ResumeLayout(false);
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			this.tableLayoutPanel3.ResumeLayout(false);
			this.tableLayoutPanel3.PerformLayout();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel RoutingprofileFormTable;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Button CancelRatingButton;
		private System.Windows.Forms.Button StartRatingButton;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ComboBox ProfilesCombobox;
		private System.Windows.Forms.ComboBox TargetCombobox;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
		public GMap.NET.WindowsForms.GMapControl Map;
		private System.Windows.Forms.Label label2;
	}
}
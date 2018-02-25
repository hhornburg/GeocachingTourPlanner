namespace GeocachingTourPlanner
{
    partial class Form1
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.FileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.ImportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.geocachesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.oSMDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.ExportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.RunToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.RateGeocachesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.CreateRouteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.RateGeocachesCreateRouteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.SettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.RatingprofilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparatorRating = new System.Windows.Forms.ToolStripSeparator();
			this.NewRatingprofileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.RoutingprofilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparatorRouting = new System.Windows.Forms.ToolStripSeparator();
			this.NewRoutingprofileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.setGeocachedatabaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.setRoutingprofiledatabaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.setRatingprofiledatabaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.setRouterDBToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.Tabcontainer = new System.Windows.Forms.TabControl();
			this.GeocachesTab = new System.Windows.Forms.TabPage();
			this.GeocacheTable = new System.Windows.Forms.DataGridView();
			this.MapTab = new System.Windows.Forms.TabPage();
			this.MapTab_SideMenu = new System.Windows.Forms.TableLayoutPanel();
			this.MediumGeocachesCheckbox = new System.Windows.Forms.CheckBox();
			this.BestGeocachesCheckbox = new System.Windows.Forms.CheckBox();
			this.WorstGeocachesCheckbox = new System.Windows.Forms.CheckBox();
			this.Map = new GMap.NET.WindowsForms.GMapControl();
			this.menuStrip1.SuspendLayout();
			this.Tabcontainer.SuspendLayout();
			this.GeocachesTab.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.GeocacheTable)).BeginInit();
			this.MapTab.SuspendLayout();
			this.MapTab_SideMenu.SuspendLayout();
			this.SuspendLayout();
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileToolStripMenuItem,
            this.RunToolStripMenuItem,
            this.SettingsToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(1162, 24);
			this.menuStrip1.TabIndex = 0;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// FileToolStripMenuItem
			// 
			this.FileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ImportToolStripMenuItem,
            this.ExportToolStripMenuItem});
			this.FileToolStripMenuItem.Name = "FileToolStripMenuItem";
			this.FileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
			this.FileToolStripMenuItem.Text = "File";
			// 
			// ImportToolStripMenuItem
			// 
			this.ImportToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.geocachesToolStripMenuItem,
            this.oSMDataToolStripMenuItem});
			this.ImportToolStripMenuItem.Name = "ImportToolStripMenuItem";
			this.ImportToolStripMenuItem.Size = new System.Drawing.Size(110, 22);
			this.ImportToolStripMenuItem.Text = "Import";
			// 
			// geocachesToolStripMenuItem
			// 
			this.geocachesToolStripMenuItem.Name = "geocachesToolStripMenuItem";
			this.geocachesToolStripMenuItem.Size = new System.Drawing.Size(131, 22);
			this.geocachesToolStripMenuItem.Text = "Geocaches";
			this.geocachesToolStripMenuItem.Click += new System.EventHandler(this.LoadGeocachesToolStripMenuItem_Click);
			// 
			// oSMDataToolStripMenuItem
			// 
			this.oSMDataToolStripMenuItem.Name = "oSMDataToolStripMenuItem";
			this.oSMDataToolStripMenuItem.Size = new System.Drawing.Size(131, 22);
			this.oSMDataToolStripMenuItem.Text = "OSM Data";
			this.oSMDataToolStripMenuItem.Click += new System.EventHandler(this.OSMDataToolStripMenuItem_Click);
			// 
			// ExportToolStripMenuItem
			// 
			this.ExportToolStripMenuItem.Name = "ExportToolStripMenuItem";
			this.ExportToolStripMenuItem.Size = new System.Drawing.Size(110, 22);
			this.ExportToolStripMenuItem.Text = "Export";
			// 
			// RunToolStripMenuItem
			// 
			this.RunToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.RateGeocachesToolStripMenuItem,
            this.CreateRouteToolStripMenuItem,
            this.RateGeocachesCreateRouteToolStripMenuItem});
			this.RunToolStripMenuItem.Name = "RunToolStripMenuItem";
			this.RunToolStripMenuItem.Size = new System.Drawing.Size(40, 20);
			this.RunToolStripMenuItem.Text = "Run";
			// 
			// RateGeocachesToolStripMenuItem
			// 
			this.RateGeocachesToolStripMenuItem.Name = "RateGeocachesToolStripMenuItem";
			this.RateGeocachesToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
			this.RateGeocachesToolStripMenuItem.Text = "Rate geocaches";
			this.RateGeocachesToolStripMenuItem.Click += new System.EventHandler(this.RateGeocachesToolStripMenuItem_Click);
			// 
			// CreateRouteToolStripMenuItem
			// 
			this.CreateRouteToolStripMenuItem.Name = "CreateRouteToolStripMenuItem";
			this.CreateRouteToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
			this.CreateRouteToolStripMenuItem.Text = "Create Route";
			this.CreateRouteToolStripMenuItem.Click += new System.EventHandler(this.CreateRouteToolStripMenuItem_Click);
			// 
			// RateGeocachesCreateRouteToolStripMenuItem
			// 
			this.RateGeocachesCreateRouteToolStripMenuItem.Name = "RateGeocachesCreateRouteToolStripMenuItem";
			this.RateGeocachesCreateRouteToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
			this.RateGeocachesCreateRouteToolStripMenuItem.Text = "Both";
			// 
			// SettingsToolStripMenuItem
			// 
			this.SettingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.RatingprofilesToolStripMenuItem,
            this.RoutingprofilesToolStripMenuItem,
            this.toolStripSeparator3,
            this.setGeocachedatabaseToolStripMenuItem,
            this.setRoutingprofiledatabaseToolStripMenuItem,
            this.setRatingprofiledatabaseToolStripMenuItem,
            this.setRouterDBToolStripMenuItem});
			this.SettingsToolStripMenuItem.Name = "SettingsToolStripMenuItem";
			this.SettingsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
			this.SettingsToolStripMenuItem.Text = "Settings";
			// 
			// RatingprofilesToolStripMenuItem
			// 
			this.RatingprofilesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparatorRating,
            this.NewRatingprofileToolStripMenuItem});
			this.RatingprofilesToolStripMenuItem.Name = "RatingprofilesToolStripMenuItem";
			this.RatingprofilesToolStripMenuItem.Size = new System.Drawing.Size(216, 22);
			this.RatingprofilesToolStripMenuItem.Text = "Ratingprofiles";
			// 
			// toolStripSeparatorRating
			// 
			this.toolStripSeparatorRating.Name = "toolStripSeparatorRating";
			this.toolStripSeparatorRating.Size = new System.Drawing.Size(132, 6);
			// 
			// NewRatingprofileToolStripMenuItem
			// 
			this.NewRatingprofileToolStripMenuItem.Name = "NewRatingprofileToolStripMenuItem";
			this.NewRatingprofileToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
			this.NewRatingprofileToolStripMenuItem.Text = "New Profile";
			this.NewRatingprofileToolStripMenuItem.Click += new System.EventHandler(this.NewRatingprofileToolStripMenuItem_Click);
			// 
			// RoutingprofilesToolStripMenuItem
			// 
			this.RoutingprofilesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparatorRouting,
            this.NewRoutingprofileToolStripMenuItem});
			this.RoutingprofilesToolStripMenuItem.Name = "RoutingprofilesToolStripMenuItem";
			this.RoutingprofilesToolStripMenuItem.Size = new System.Drawing.Size(216, 22);
			this.RoutingprofilesToolStripMenuItem.Text = "Routingprofiles";
			// 
			// toolStripSeparatorRouting
			// 
			this.toolStripSeparatorRouting.Name = "toolStripSeparatorRouting";
			this.toolStripSeparatorRouting.Size = new System.Drawing.Size(132, 6);
			// 
			// NewRoutingprofileToolStripMenuItem
			// 
			this.NewRoutingprofileToolStripMenuItem.Name = "NewRoutingprofileToolStripMenuItem";
			this.NewRoutingprofileToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
			this.NewRoutingprofileToolStripMenuItem.Text = "New Profile";
			this.NewRoutingprofileToolStripMenuItem.Click += new System.EventHandler(this.NewRoutingprofileToolStripMenuItem_Click);
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(213, 6);
			// 
			// setGeocachedatabaseToolStripMenuItem
			// 
			this.setGeocachedatabaseToolStripMenuItem.Name = "setGeocachedatabaseToolStripMenuItem";
			this.setGeocachedatabaseToolStripMenuItem.Size = new System.Drawing.Size(216, 22);
			this.setGeocachedatabaseToolStripMenuItem.Text = "Set Geocachedatabase";
			this.setGeocachedatabaseToolStripMenuItem.Click += new System.EventHandler(this.setGeocachedatabaseToolStripMenuItem_Click);
			// 
			// setRoutingprofiledatabaseToolStripMenuItem
			// 
			this.setRoutingprofiledatabaseToolStripMenuItem.Name = "setRoutingprofiledatabaseToolStripMenuItem";
			this.setRoutingprofiledatabaseToolStripMenuItem.Size = new System.Drawing.Size(216, 22);
			this.setRoutingprofiledatabaseToolStripMenuItem.Text = "Set Routingprofiledatabase";
			this.setRoutingprofiledatabaseToolStripMenuItem.Click += new System.EventHandler(this.setRoutingprofiledatabaseToolStripMenuItem_Click);
			// 
			// setRatingprofiledatabaseToolStripMenuItem
			// 
			this.setRatingprofiledatabaseToolStripMenuItem.Name = "setRatingprofiledatabaseToolStripMenuItem";
			this.setRatingprofiledatabaseToolStripMenuItem.Size = new System.Drawing.Size(216, 22);
			this.setRatingprofiledatabaseToolStripMenuItem.Text = "Set Ratingprofiledatabase";
			this.setRatingprofiledatabaseToolStripMenuItem.Click += new System.EventHandler(this.setRatingprofiledatabaseToolStripMenuItem_Click);
			// 
			// setRouterDBToolStripMenuItem
			// 
			this.setRouterDBToolStripMenuItem.Name = "setRouterDBToolStripMenuItem";
			this.setRouterDBToolStripMenuItem.Size = new System.Drawing.Size(216, 22);
			this.setRouterDBToolStripMenuItem.Text = "Set RouterDB";
			this.setRouterDBToolStripMenuItem.Click += new System.EventHandler(this.setRouterDBToolStripMenuItem_Click);
			// 
			// Tabcontainer
			// 
			this.Tabcontainer.Controls.Add(this.GeocachesTab);
			this.Tabcontainer.Controls.Add(this.MapTab);
			this.Tabcontainer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.Tabcontainer.Location = new System.Drawing.Point(0, 24);
			this.Tabcontainer.Name = "Tabcontainer";
			this.Tabcontainer.SelectedIndex = 0;
			this.Tabcontainer.Size = new System.Drawing.Size(1162, 418);
			this.Tabcontainer.TabIndex = 1;
			// 
			// GeocachesTab
			// 
			this.GeocachesTab.Controls.Add(this.GeocacheTable);
			this.GeocachesTab.Location = new System.Drawing.Point(4, 22);
			this.GeocachesTab.Name = "GeocachesTab";
			this.GeocachesTab.Padding = new System.Windows.Forms.Padding(3);
			this.GeocachesTab.Size = new System.Drawing.Size(1154, 392);
			this.GeocachesTab.TabIndex = 0;
			this.GeocachesTab.Text = "Geocaches";
			this.GeocachesTab.UseVisualStyleBackColor = true;
			// 
			// GeocacheTable
			// 
			this.GeocacheTable.AllowUserToOrderColumns = true;
			this.GeocacheTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.GeocacheTable.Dock = System.Windows.Forms.DockStyle.Fill;
			this.GeocacheTable.Location = new System.Drawing.Point(3, 3);
			this.GeocacheTable.Name = "GeocacheTable";
			this.GeocacheTable.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.GeocacheTable.Size = new System.Drawing.Size(1148, 386);
			this.GeocacheTable.TabIndex = 0;
			// 
			// MapTab
			// 
			this.MapTab.Controls.Add(this.MapTab_SideMenu);
			this.MapTab.Controls.Add(this.Map);
			this.MapTab.Location = new System.Drawing.Point(4, 22);
			this.MapTab.Name = "MapTab";
			this.MapTab.Padding = new System.Windows.Forms.Padding(3);
			this.MapTab.Size = new System.Drawing.Size(1154, 392);
			this.MapTab.TabIndex = 1;
			this.MapTab.Text = "Map";
			this.MapTab.UseVisualStyleBackColor = true;
			// 
			// MapTab_SideMenu
			// 
			this.MapTab_SideMenu.AutoSize = true;
			this.MapTab_SideMenu.ColumnCount = 1;
			this.MapTab_SideMenu.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.MapTab_SideMenu.Controls.Add(this.MediumGeocachesCheckbox, 0, 1);
			this.MapTab_SideMenu.Controls.Add(this.BestGeocachesCheckbox, 0, 0);
			this.MapTab_SideMenu.Controls.Add(this.WorstGeocachesCheckbox, 0, 2);
			this.MapTab_SideMenu.Dock = System.Windows.Forms.DockStyle.Left;
			this.MapTab_SideMenu.Location = new System.Drawing.Point(3, 3);
			this.MapTab_SideMenu.Name = "MapTab_SideMenu";
			this.MapTab_SideMenu.RowCount = 3;
			this.MapTab_SideMenu.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.MapTab_SideMenu.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.MapTab_SideMenu.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.MapTab_SideMenu.Size = new System.Drawing.Size(156, 386);
			this.MapTab_SideMenu.TabIndex = 1;
			// 
			// MediumGeocachesCheckbox
			// 
			this.MediumGeocachesCheckbox.AutoSize = true;
			this.MediumGeocachesCheckbox.Checked = true;
			this.MediumGeocachesCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
			this.MediumGeocachesCheckbox.Location = new System.Drawing.Point(3, 26);
			this.MediumGeocachesCheckbox.Name = "MediumGeocachesCheckbox";
			this.MediumGeocachesCheckbox.Size = new System.Drawing.Size(150, 17);
			this.MediumGeocachesCheckbox.TabIndex = 0;
			this.MediumGeocachesCheckbox.Text = "Show medium Geocaches";
			this.MediumGeocachesCheckbox.UseVisualStyleBackColor = true;
			this.MediumGeocachesCheckbox.CheckedChanged += new System.EventHandler(this.MediumCheckbox_CheckedChanged);
			// 
			// BestGeocachesCheckbox
			// 
			this.BestGeocachesCheckbox.AutoSize = true;
			this.BestGeocachesCheckbox.Checked = true;
			this.BestGeocachesCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
			this.BestGeocachesCheckbox.Location = new System.Drawing.Point(3, 3);
			this.BestGeocachesCheckbox.Name = "BestGeocachesCheckbox";
			this.BestGeocachesCheckbox.Size = new System.Drawing.Size(134, 17);
			this.BestGeocachesCheckbox.TabIndex = 1;
			this.BestGeocachesCheckbox.Text = "Show best Geocaches";
			this.BestGeocachesCheckbox.UseVisualStyleBackColor = true;
			this.BestGeocachesCheckbox.CheckedChanged += new System.EventHandler(this.BestCheckbox_CheckedChanged);
			// 
			// WorstGeocachesCheckbox
			// 
			this.WorstGeocachesCheckbox.AutoSize = true;
			this.WorstGeocachesCheckbox.Checked = true;
			this.WorstGeocachesCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
			this.WorstGeocachesCheckbox.Location = new System.Drawing.Point(3, 49);
			this.WorstGeocachesCheckbox.Name = "WorstGeocachesCheckbox";
			this.WorstGeocachesCheckbox.Size = new System.Drawing.Size(139, 17);
			this.WorstGeocachesCheckbox.TabIndex = 2;
			this.WorstGeocachesCheckbox.Text = "Show worst Geocaches";
			this.WorstGeocachesCheckbox.UseVisualStyleBackColor = true;
			this.WorstGeocachesCheckbox.CheckedChanged += new System.EventHandler(this.WorstCheckbox_CheckedChanged);
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
			this.Map.Location = new System.Drawing.Point(3, 3);
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
			this.Map.Size = new System.Drawing.Size(1148, 386);
			this.Map.TabIndex = 0;
			this.Map.Zoom = 0D;
			this.Map.OnMarkerClick += new GMap.NET.WindowsForms.MarkerClick(this.Map_OnMarkerClick);
			this.Map.OnMapDrag += new GMap.NET.MapDrag(this.Map_OnMapDrag);
			this.Map.OnMapZoomChanged += new GMap.NET.MapZoomChanged(this.Map_OnMapZoomChanged);
			this.Map.Load += new System.EventHandler(this.Map_Load);
			this.Map.Enter += new System.EventHandler(this.Map_Enter);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1162, 442);
			this.Controls.Add(this.Tabcontainer);
			this.Controls.Add(this.menuStrip1);
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "Form1";
			this.Text = "GeocachingTourPlanner";
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.Tabcontainer.ResumeLayout(false);
			this.GeocachesTab.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.GeocacheTable)).EndInit();
			this.MapTab.ResumeLayout(false);
			this.MapTab.PerformLayout();
			this.MapTab_SideMenu.ResumeLayout(false);
			this.MapTab_SideMenu.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem FileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ImportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ExportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem RunToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem RateGeocachesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem CreateRouteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem RateGeocachesCreateRouteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SettingsToolStripMenuItem;
        private System.Windows.Forms.TabControl Tabcontainer;
        private System.Windows.Forms.TabPage GeocachesTab;
        private System.Windows.Forms.TabPage MapTab;
        public System.Windows.Forms.DataGridView GeocacheTable;
        public System.Windows.Forms.ToolStripMenuItem RatingprofilesToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem RoutingprofilesToolStripMenuItem;
        public System.Windows.Forms.ToolStripSeparator toolStripSeparatorRating;
        public System.Windows.Forms.ToolStripMenuItem NewRatingprofileToolStripMenuItem;
		private System.Windows.Forms.CheckBox MediumGeocachesCheckbox;
		private System.Windows.Forms.CheckBox BestGeocachesCheckbox;
		private System.Windows.Forms.CheckBox WorstGeocachesCheckbox;
		public GMap.NET.WindowsForms.GMapControl Map;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
		private System.Windows.Forms.ToolStripMenuItem setGeocachedatabaseToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem setRoutingprofiledatabaseToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem setRatingprofiledatabaseToolStripMenuItem;
		public System.Windows.Forms.ToolStripMenuItem NewRoutingprofileToolStripMenuItem;
		public System.Windows.Forms.ToolStripSeparator toolStripSeparatorRouting;
		private System.Windows.Forms.ToolStripMenuItem geocachesToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem oSMDataToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem setRouterDBToolStripMenuItem;
		public System.Windows.Forms.TableLayoutPanel MapTab_SideMenu;
	}
}


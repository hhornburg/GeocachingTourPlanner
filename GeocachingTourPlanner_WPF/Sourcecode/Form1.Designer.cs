using System;
using System.Windows.Forms;

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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
			this.UpmostTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
			this.Tabcontainer = new System.Windows.Forms.TabControl();
			this.MapTab = new System.Windows.Forms.TabPage();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.Map = new GMap.NET.WindowsForms.GMapControl();
			this.MapTab_SideMenu = new System.Windows.Forms.TableLayoutPanel();
			this.BestGeocachesCheckbox = new System.Windows.Forms.CheckBox();
			this.WorstGeocachesCheckbox = new System.Windows.Forms.CheckBox();
			this.MediumGeocachesCheckbox = new System.Windows.Forms.CheckBox();
			this.tableLayoutPanel10 = new System.Windows.Forms.TableLayoutPanel();
			this.RateGeocachesButton = new System.Windows.Forms.Button();
			this.CreateRouteButton = new System.Windows.Forms.Button();
			this.SelectedRoutingprofileCombobox = new System.Windows.Forms.ComboBox();
			this.SelectedRatingprofileCombobox = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label41 = new System.Windows.Forms.Label();
			this.label42 = new System.Windows.Forms.Label();
			this.label43 = new System.Windows.Forms.Label();
			this.StartpointTextbox = new System.Windows.Forms.TextBox();
			this.EndpointTextbox = new System.Windows.Forms.TextBox();
			this.GeocachesTab = new System.Windows.Forms.TabPage();
			this.GeocacheTable = new System.Windows.Forms.DataGridView();
			this.LeftTabs = new System.Windows.Forms.TabControl();
			this.Firststeps = new System.Windows.Forms.TabPage();
			this.tableLayoutPanel17 = new System.Windows.Forms.TableLayoutPanel();
			this.webBrowser1 = new System.Windows.Forms.WebBrowser();
			this.OpenWikiButton = new System.Windows.Forms.Button();
			this.Overviewpage = new System.Windows.Forms.TabPage();
			this.NameStateTable = new System.Windows.Forms.TableLayoutPanel();
			this.StateTableLayout = new System.Windows.Forms.TableLayoutPanel();
			this.GeocachesStateLabel = new System.Windows.Forms.Label();
			this.RatingprofilesStateLabel = new System.Windows.Forms.Label();
			this.RoutingprofilesStateLabel = new System.Windows.Forms.Label();
			this.RouterDBStateLabel = new System.Windows.Forms.Label();
			this.SetGeocacheDBButton = new System.Windows.Forms.Button();
			this.ImportPQButton = new System.Windows.Forms.Button();
			this.SetRatingprofileDBButton = new System.Windows.Forms.Button();
			this.SetRoutingprofileDBButton = new System.Windows.Forms.Button();
			this.SetRouterDBButton = new System.Windows.Forms.Button();
			this.ImportPbfButton = new System.Windows.Forms.Button();
			this.GetPQLabel = new System.Windows.Forms.LinkLabel();
			this.GetPbfLabel = new System.Windows.Forms.LinkLabel();
			this.NewRatingprofileDatbaseButton = new System.Windows.Forms.Button();
			this.NewRoutingprofileDatabaseButton = new System.Windows.Forms.Button();
			this.NameLabel = new System.Windows.Forms.Label();
			this.Ratingprofiles = new System.Windows.Forms.TabPage();
			this.panel1 = new System.Windows.Forms.Panel();
			this.RatingprofilesSettingsTabelLayout = new System.Windows.Forms.TableLayoutPanel();
			this.AgeGroupBox = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel8 = new System.Windows.Forms.TableLayoutPanel();
			this.AgeFactorValue = new System.Windows.Forms.TextBox();
			this.AgeValue = new System.Windows.Forms.ComboBox();
			this.DValueungGroupBox = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
			this.label22 = new System.Windows.Forms.Label();
			this.label23 = new System.Windows.Forms.Label();
			this.label26 = new System.Windows.Forms.Label();
			this.label27 = new System.Windows.Forms.Label();
			this.label28 = new System.Windows.Forms.Label();
			this.label29 = new System.Windows.Forms.Label();
			this.label30 = new System.Windows.Forms.Label();
			this.label24 = new System.Windows.Forms.Label();
			this.label25 = new System.Windows.Forms.Label();
			this.label21 = new System.Windows.Forms.Label();
			this.DPriorityValue = new System.Windows.Forms.TextBox();
			this.D5Value = new System.Windows.Forms.TextBox();
			this.D35Value = new System.Windows.Forms.TextBox();
			this.D2Value = new System.Windows.Forms.TextBox();
			this.D45Value = new System.Windows.Forms.TextBox();
			this.D4Value = new System.Windows.Forms.TextBox();
			this.D3Value = new System.Windows.Forms.TextBox();
			this.D15Value = new System.Windows.Forms.TextBox();
			this.D25Value = new System.Windows.Forms.TextBox();
			this.D1Value = new System.Windows.Forms.TextBox();
			this.GeocachetypGroupBox = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel7 = new System.Windows.Forms.TableLayoutPanel();
			this.WebcamValue = new System.Windows.Forms.TextBox();
			this.WherigoValue = new System.Windows.Forms.TextBox();
			this.MysteryValue = new System.Windows.Forms.TextBox();
			this.OtherTypeValue = new System.Windows.Forms.TextBox();
			this.LetterboxValue = new System.Windows.Forms.TextBox();
			this.MultiValue = new System.Windows.Forms.TextBox();
			this.VirtualValue = new System.Windows.Forms.TextBox();
			this.EarthcacheValue = new System.Windows.Forms.TextBox();
			this.TypePriorityValue = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.label11 = new System.Windows.Forms.Label();
			this.label14 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label19 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.TraditionalValue = new System.Windows.Forms.TextBox();
			this.RatingprofileInfoLabel = new System.Windows.Forms.Label();
			this.GeocachegrößeGroupBox = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
			this.RegularValue = new System.Windows.Forms.TextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.label12 = new System.Windows.Forms.Label();
			this.label16 = new System.Windows.Forms.Label();
			this.label18 = new System.Windows.Forms.Label();
			this.label17 = new System.Windows.Forms.Label();
			this.label13 = new System.Windows.Forms.Label();
			this.SizePriorityValue = new System.Windows.Forms.TextBox();
			this.LargeValue = new System.Windows.Forms.TextBox();
			this.SmallValue = new System.Windows.Forms.TextBox();
			this.MicroValue = new System.Windows.Forms.TextBox();
			this.OtherSizeValue = new System.Windows.Forms.TextBox();
			this.TValueungGroupbox = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
			this.label32 = new System.Windows.Forms.Label();
			this.label33 = new System.Windows.Forms.Label();
			this.label36 = new System.Windows.Forms.Label();
			this.label38 = new System.Windows.Forms.Label();
			this.label39 = new System.Windows.Forms.Label();
			this.label40 = new System.Windows.Forms.Label();
			this.label37 = new System.Windows.Forms.Label();
			this.label34 = new System.Windows.Forms.Label();
			this.label35 = new System.Windows.Forms.Label();
			this.label31 = new System.Windows.Forms.Label();
			this.TPriorityValue = new System.Windows.Forms.TextBox();
			this.T5Value = new System.Windows.Forms.TextBox();
			this.T35Value = new System.Windows.Forms.TextBox();
			this.T2Value = new System.Windows.Forms.TextBox();
			this.T45Value = new System.Windows.Forms.TextBox();
			this.T3Value = new System.Windows.Forms.TextBox();
			this.T15Value = new System.Windows.Forms.TextBox();
			this.T4Value = new System.Windows.Forms.TextBox();
			this.T25Value = new System.Windows.Forms.TextBox();
			this.T1Value = new System.Windows.Forms.TextBox();
			this.Other = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
			this.label15 = new System.Windows.Forms.Label();
			this.NMFlagValue = new System.Windows.Forms.TextBox();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.label55 = new System.Windows.Forms.Label();
			this.EditRatingprofileCombobox = new System.Windows.Forms.ComboBox();
			this.SaveRatingprofileLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
			this.DeleteRatingprofileButton = new System.Windows.Forms.Button();
			this.CreateRatingprofileButton = new System.Windows.Forms.Button();
			this.label54 = new System.Windows.Forms.Label();
			this.RatingProfileName = new System.Windows.Forms.TextBox();
			this.Routingprofiles = new System.Windows.Forms.TabPage();
			this.SaveRoutingProfileTableLayout = new System.Windows.Forms.TableLayoutPanel();
			this.DeleteRoutingprofileButton = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.label52 = new System.Windows.Forms.Label();
			this.RoutingProfileName = new System.Windows.Forms.TextBox();
			this.RoutingprofilesSettingsTableLayout = new System.Windows.Forms.TableLayoutPanel();
			this.RoutingCoreGroupbox = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel11 = new System.Windows.Forms.TableLayoutPanel();
			this.label45 = new System.Windows.Forms.Label();
			this.label46 = new System.Windows.Forms.Label();
			this.VehicleCombobox = new System.Windows.Forms.ComboBox();
			this.MetricCombobox = new System.Windows.Forms.ComboBox();
			this.DistanceGroupBox = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel12 = new System.Windows.Forms.TableLayoutPanel();
			this.MaxDistance = new System.Windows.Forms.TextBox();
			this.PenaltyPerExtraKm = new System.Windows.Forms.TextBox();
			this.label47 = new System.Windows.Forms.Label();
			this.label48 = new System.Windows.Forms.Label();
			this.TimeGroupBox = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel13 = new System.Windows.Forms.TableLayoutPanel();
			this.label49 = new System.Windows.Forms.Label();
			this.label50 = new System.Windows.Forms.Label();
			this.label51 = new System.Windows.Forms.Label();
			this.MaxTime = new System.Windows.Forms.TextBox();
			this.TimePerGeocache = new System.Windows.Forms.TextBox();
			this.PenaltyPerExtra10min = new System.Windows.Forms.TextBox();
			this.label44 = new System.Windows.Forms.Label();
			this.tableLayoutPanel14 = new System.Windows.Forms.TableLayoutPanel();
			this.label53 = new System.Windows.Forms.Label();
			this.EditRoutingprofileCombobox = new System.Windows.Forms.ComboBox();
			this.Settings = new System.Windows.Forms.TabPage();
			this.SettingsTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
			this.RoutingsettingsGroupbox = new System.Windows.Forms.GroupBox();
			this.RoutingSettingsTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
			this.label59 = new System.Windows.Forms.Label();
			this.RoutefindingWidth_Textbox = new System.Windows.Forms.TextBox();
			this.Autotargetselection = new System.Windows.Forms.CheckBox();
			this.LiveDisplayRouteCalculationCheckbox = new System.Windows.Forms.CheckBox();
			this.AutotargetSelectionMaxDistanceLabel = new System.Windows.Forms.Label();
			this.AutotargetselectionMinLabel = new System.Windows.Forms.Label();
			this.AutotargetselectionMaxTextBox = new System.Windows.Forms.TextBox();
			this.AutotargetselectionMinTextBox = new System.Windows.Forms.TextBox();
			this.Display_groupBox = new System.Windows.Forms.GroupBox();
			this.DisplaySettingsTableLayoutSettings = new System.Windows.Forms.TableLayoutPanel();
			this.IconSizeLabel = new System.Windows.Forms.Label();
			this.MarkerSizeTrackBar = new System.Windows.Forms.TrackBar();
			this.StatusbarTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
			this.StatusLabel = new System.Windows.Forms.Label();
			this.ProgressBar = new System.Windows.Forms.ProgressBar();
			this.UpmostTableLayoutPanel.SuspendLayout();
			this.Tabcontainer.SuspendLayout();
			this.MapTab.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.MapTab_SideMenu.SuspendLayout();
			this.tableLayoutPanel10.SuspendLayout();
			this.GeocachesTab.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.GeocacheTable)).BeginInit();
			this.LeftTabs.SuspendLayout();
			this.Firststeps.SuspendLayout();
			this.tableLayoutPanel17.SuspendLayout();
			this.Overviewpage.SuspendLayout();
			this.NameStateTable.SuspendLayout();
			this.StateTableLayout.SuspendLayout();
			this.Ratingprofiles.SuspendLayout();
			this.panel1.SuspendLayout();
			this.RatingprofilesSettingsTabelLayout.SuspendLayout();
			this.AgeGroupBox.SuspendLayout();
			this.tableLayoutPanel8.SuspendLayout();
			this.DValueungGroupBox.SuspendLayout();
			this.tableLayoutPanel4.SuspendLayout();
			this.GeocachetypGroupBox.SuspendLayout();
			this.tableLayoutPanel7.SuspendLayout();
			this.GeocachegrößeGroupBox.SuspendLayout();
			this.tableLayoutPanel3.SuspendLayout();
			this.TValueungGroupbox.SuspendLayout();
			this.tableLayoutPanel5.SuspendLayout();
			this.Other.SuspendLayout();
			this.tableLayoutPanel6.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			this.SaveRatingprofileLayoutPanel.SuspendLayout();
			this.Routingprofiles.SuspendLayout();
			this.SaveRoutingProfileTableLayout.SuspendLayout();
			this.RoutingprofilesSettingsTableLayout.SuspendLayout();
			this.RoutingCoreGroupbox.SuspendLayout();
			this.tableLayoutPanel11.SuspendLayout();
			this.DistanceGroupBox.SuspendLayout();
			this.tableLayoutPanel12.SuspendLayout();
			this.TimeGroupBox.SuspendLayout();
			this.tableLayoutPanel13.SuspendLayout();
			this.tableLayoutPanel14.SuspendLayout();
			this.Settings.SuspendLayout();
			this.SettingsTableLayoutPanel.SuspendLayout();
			this.RoutingsettingsGroupbox.SuspendLayout();
			this.RoutingSettingsTableLayoutPanel.SuspendLayout();
			this.Display_groupBox.SuspendLayout();
			this.DisplaySettingsTableLayoutSettings.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.MarkerSizeTrackBar)).BeginInit();
			this.StatusbarTableLayoutPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// UpmostTableLayoutPanel
			// 
			this.UpmostTableLayoutPanel.ColumnCount = 2;
			this.UpmostTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 400F));
			this.UpmostTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.UpmostTableLayoutPanel.Controls.Add(this.Tabcontainer, 1, 0);
			this.UpmostTableLayoutPanel.Controls.Add(this.LeftTabs, 0, 0);
			this.UpmostTableLayoutPanel.Controls.Add(this.StatusbarTableLayoutPanel, 0, 1);
			this.UpmostTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.UpmostTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
			this.UpmostTableLayoutPanel.Name = "UpmostTableLayoutPanel";
			this.UpmostTableLayoutPanel.RowCount = 2;
			this.UpmostTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.UpmostTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.UpmostTableLayoutPanel.Size = new System.Drawing.Size(1388, 610);
			this.UpmostTableLayoutPanel.TabIndex = 2;
			// 
			// Tabcontainer
			// 
			this.Tabcontainer.Controls.Add(this.MapTab);
			this.Tabcontainer.Controls.Add(this.GeocachesTab);
			this.Tabcontainer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.Tabcontainer.Location = new System.Drawing.Point(400, 0);
			this.Tabcontainer.Margin = new System.Windows.Forms.Padding(0);
			this.Tabcontainer.Name = "Tabcontainer";
			this.Tabcontainer.SelectedIndex = 0;
			this.Tabcontainer.Size = new System.Drawing.Size(988, 590);
			this.Tabcontainer.TabIndex = 3;
			// 
			// MapTab
			// 
			this.MapTab.Controls.Add(this.tableLayoutPanel1);
			this.MapTab.Location = new System.Drawing.Point(4, 22);
			this.MapTab.Name = "MapTab";
			this.MapTab.Padding = new System.Windows.Forms.Padding(3);
			this.MapTab.Size = new System.Drawing.Size(980, 564);
			this.MapTab.TabIndex = 1;
			this.MapTab.Text = "Map";
			this.MapTab.UseVisualStyleBackColor = true;
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.Controls.Add(this.Map, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.MapTab_SideMenu, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel10, 0, 1);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 2;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(974, 558);
			this.tableLayoutPanel1.TabIndex = 2;
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
			this.Map.Location = new System.Drawing.Point(165, 3);
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
			this.Map.Size = new System.Drawing.Size(806, 452);
			this.Map.TabIndex = 2;
			this.Map.Zoom = 0D;
			this.Map.OnMarkerClick += new GMap.NET.WindowsForms.MarkerClick(this.Map_OnMarkerClick);
			this.Map.OnMapDrag += new GMap.NET.MapDrag(this.Map_OnMapDrag);
			this.Map.OnMapZoomChanged += new GMap.NET.MapZoomChanged(this.Map_OnMapZoomChanged);
			this.Map.Load += new System.EventHandler(this.Map_Load);
			this.Map.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Map_Click);
			this.Map.MouseEnter += new System.EventHandler(this.Map_Load);
			this.Map.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Map_MouseUp);
			// 
			// MapTab_SideMenu
			// 
			this.MapTab_SideMenu.AutoScroll = true;
			this.MapTab_SideMenu.AutoSize = true;
			this.MapTab_SideMenu.ColumnCount = 1;
			this.MapTab_SideMenu.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.MapTab_SideMenu.Controls.Add(this.BestGeocachesCheckbox, 0, 1);
			this.MapTab_SideMenu.Controls.Add(this.WorstGeocachesCheckbox, 0, 3);
			this.MapTab_SideMenu.Controls.Add(this.MediumGeocachesCheckbox, 0, 2);
			this.MapTab_SideMenu.Dock = System.Windows.Forms.DockStyle.Top;
			this.MapTab_SideMenu.Location = new System.Drawing.Point(3, 3);
			this.MapTab_SideMenu.Name = "MapTab_SideMenu";
			this.MapTab_SideMenu.RowCount = 4;
			this.MapTab_SideMenu.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.MapTab_SideMenu.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.MapTab_SideMenu.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.MapTab_SideMenu.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.MapTab_SideMenu.Size = new System.Drawing.Size(156, 69);
			this.MapTab_SideMenu.TabIndex = 1;
			// 
			// BestGeocachesCheckbox
			// 
			this.BestGeocachesCheckbox.AutoSize = true;
			this.BestGeocachesCheckbox.Checked = true;
			this.BestGeocachesCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
			this.BestGeocachesCheckbox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.BestGeocachesCheckbox.Location = new System.Drawing.Point(3, 3);
			this.BestGeocachesCheckbox.Name = "BestGeocachesCheckbox";
			this.BestGeocachesCheckbox.Size = new System.Drawing.Size(150, 17);
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
			this.WorstGeocachesCheckbox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.WorstGeocachesCheckbox.Location = new System.Drawing.Point(3, 49);
			this.WorstGeocachesCheckbox.Name = "WorstGeocachesCheckbox";
			this.WorstGeocachesCheckbox.Size = new System.Drawing.Size(150, 17);
			this.WorstGeocachesCheckbox.TabIndex = 2;
			this.WorstGeocachesCheckbox.Text = "Show worst Geocaches";
			this.WorstGeocachesCheckbox.UseVisualStyleBackColor = true;
			this.WorstGeocachesCheckbox.CheckedChanged += new System.EventHandler(this.WorstCheckbox_CheckedChanged);
			// 
			// MediumGeocachesCheckbox
			// 
			this.MediumGeocachesCheckbox.AutoSize = true;
			this.MediumGeocachesCheckbox.Checked = true;
			this.MediumGeocachesCheckbox.CheckState = System.Windows.Forms.CheckState.Checked;
			this.MediumGeocachesCheckbox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.MediumGeocachesCheckbox.Location = new System.Drawing.Point(3, 26);
			this.MediumGeocachesCheckbox.Name = "MediumGeocachesCheckbox";
			this.MediumGeocachesCheckbox.Size = new System.Drawing.Size(150, 17);
			this.MediumGeocachesCheckbox.TabIndex = 0;
			this.MediumGeocachesCheckbox.Text = "Show medium Geocaches";
			this.MediumGeocachesCheckbox.UseVisualStyleBackColor = true;
			this.MediumGeocachesCheckbox.CheckedChanged += new System.EventHandler(this.MediumCheckbox_CheckedChanged);
			// 
			// tableLayoutPanel10
			// 
			this.tableLayoutPanel10.ColumnCount = 3;
			this.tableLayoutPanel1.SetColumnSpan(this.tableLayoutPanel10, 2);
			this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
			this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
			this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
			this.tableLayoutPanel10.Controls.Add(this.RateGeocachesButton, 2, 3);
			this.tableLayoutPanel10.Controls.Add(this.CreateRouteButton, 1, 3);
			this.tableLayoutPanel10.Controls.Add(this.SelectedRoutingprofileCombobox, 1, 1);
			this.tableLayoutPanel10.Controls.Add(this.SelectedRatingprofileCombobox, 2, 1);
			this.tableLayoutPanel10.Controls.Add(this.label1, 1, 0);
			this.tableLayoutPanel10.Controls.Add(this.label41, 2, 0);
			this.tableLayoutPanel10.Controls.Add(this.label42, 0, 0);
			this.tableLayoutPanel10.Controls.Add(this.label43, 0, 2);
			this.tableLayoutPanel10.Controls.Add(this.StartpointTextbox, 0, 1);
			this.tableLayoutPanel10.Controls.Add(this.EndpointTextbox, 0, 3);
			this.tableLayoutPanel10.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel10.Location = new System.Drawing.Point(3, 461);
			this.tableLayoutPanel10.Name = "tableLayoutPanel10";
			this.tableLayoutPanel10.RowCount = 4;
			this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel10.Size = new System.Drawing.Size(968, 94);
			this.tableLayoutPanel10.TabIndex = 3;
			// 
			// RateGeocachesButton
			// 
			this.RateGeocachesButton.Dock = System.Windows.Forms.DockStyle.Fill;
			this.RateGeocachesButton.Location = new System.Drawing.Point(647, 70);
			this.RateGeocachesButton.Name = "RateGeocachesButton";
			this.RateGeocachesButton.Size = new System.Drawing.Size(318, 21);
			this.RateGeocachesButton.TabIndex = 0;
			this.RateGeocachesButton.Text = "Rate Geocaches";
			this.RateGeocachesButton.UseVisualStyleBackColor = true;
			this.RateGeocachesButton.Click += new System.EventHandler(this.RateGeocachesButtonClick);
			// 
			// CreateRouteButton
			// 
			this.CreateRouteButton.Dock = System.Windows.Forms.DockStyle.Fill;
			this.CreateRouteButton.Location = new System.Drawing.Point(325, 70);
			this.CreateRouteButton.Name = "CreateRouteButton";
			this.CreateRouteButton.Size = new System.Drawing.Size(316, 21);
			this.CreateRouteButton.TabIndex = 1;
			this.CreateRouteButton.Text = "Create Route";
			this.CreateRouteButton.UseVisualStyleBackColor = true;
			this.CreateRouteButton.Click += new System.EventHandler(this.CreateRouteButtonClick);
			// 
			// SelectedRoutingprofileCombobox
			// 
			this.SelectedRoutingprofileCombobox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
			this.SelectedRoutingprofileCombobox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
			this.SelectedRoutingprofileCombobox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.SelectedRoutingprofileCombobox.FormattingEnabled = true;
			this.SelectedRoutingprofileCombobox.Location = new System.Drawing.Point(325, 23);
			this.SelectedRoutingprofileCombobox.Name = "SelectedRoutingprofileCombobox";
			this.SelectedRoutingprofileCombobox.Size = new System.Drawing.Size(316, 21);
			this.SelectedRoutingprofileCombobox.TabIndex = 2;
			this.SelectedRoutingprofileCombobox.SelectedIndexChanged += new System.EventHandler(this.Dropdown_SelectedIndexChanged);
			// 
			// SelectedRatingprofileCombobox
			// 
			this.SelectedRatingprofileCombobox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
			this.SelectedRatingprofileCombobox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
			this.SelectedRatingprofileCombobox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.SelectedRatingprofileCombobox.FormattingEnabled = true;
			this.SelectedRatingprofileCombobox.Location = new System.Drawing.Point(647, 23);
			this.SelectedRatingprofileCombobox.Name = "SelectedRatingprofileCombobox";
			this.SelectedRatingprofileCombobox.Size = new System.Drawing.Size(318, 21);
			this.SelectedRatingprofileCombobox.TabIndex = 3;
			this.SelectedRatingprofileCombobox.SelectedIndexChanged += new System.EventHandler(this.Dropdown_SelectedIndexChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label1.Location = new System.Drawing.Point(325, 3);
			this.label1.Margin = new System.Windows.Forms.Padding(3);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(316, 14);
			this.label1.TabIndex = 4;
			this.label1.Text = "Select Routingprofile";
			// 
			// label41
			// 
			this.label41.AutoSize = true;
			this.label41.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label41.Location = new System.Drawing.Point(647, 3);
			this.label41.Margin = new System.Windows.Forms.Padding(3);
			this.label41.Name = "label41";
			this.label41.Size = new System.Drawing.Size(318, 14);
			this.label41.TabIndex = 5;
			this.label41.Text = "SelectRatingprofiles";
			// 
			// label42
			// 
			this.label42.AutoSize = true;
			this.label42.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label42.Location = new System.Drawing.Point(3, 3);
			this.label42.Margin = new System.Windows.Forms.Padding(3);
			this.label42.Name = "label42";
			this.label42.Size = new System.Drawing.Size(316, 14);
			this.label42.TabIndex = 6;
			this.label42.Text = "Startpoint";
			// 
			// label43
			// 
			this.label43.AutoSize = true;
			this.label43.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label43.Location = new System.Drawing.Point(3, 50);
			this.label43.Margin = new System.Windows.Forms.Padding(3);
			this.label43.Name = "label43";
			this.label43.Size = new System.Drawing.Size(316, 14);
			this.label43.TabIndex = 7;
			this.label43.Text = "Endpoint";
			// 
			// StartpointTextbox
			// 
			this.StartpointTextbox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.StartpointTextbox.Location = new System.Drawing.Point(3, 23);
			this.StartpointTextbox.Name = "StartpointTextbox";
			this.StartpointTextbox.Size = new System.Drawing.Size(316, 20);
			this.StartpointTextbox.TabIndex = 8;
			this.StartpointTextbox.TextChanged += new System.EventHandler(this.StartpointTextbox_TextChanged);
			this.StartpointTextbox.Leave += new System.EventHandler(this.StartpointTextbox_Leave);
			// 
			// EndpointTextbox
			// 
			this.EndpointTextbox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.EndpointTextbox.Location = new System.Drawing.Point(3, 70);
			this.EndpointTextbox.Name = "EndpointTextbox";
			this.EndpointTextbox.Size = new System.Drawing.Size(316, 20);
			this.EndpointTextbox.TabIndex = 9;
			this.EndpointTextbox.TextChanged += new System.EventHandler(this.EndpointTextbox_TextChanged);
			this.EndpointTextbox.Leave += new System.EventHandler(this.EndpointTextbox_Leave);
			// 
			// GeocachesTab
			// 
			this.GeocachesTab.Controls.Add(this.GeocacheTable);
			this.GeocachesTab.Location = new System.Drawing.Point(4, 22);
			this.GeocachesTab.Name = "GeocachesTab";
			this.GeocachesTab.Padding = new System.Windows.Forms.Padding(3);
			this.GeocachesTab.Size = new System.Drawing.Size(980, 564);
			this.GeocachesTab.TabIndex = 0;
			this.GeocachesTab.Text = "Geocaches";
			this.GeocachesTab.UseVisualStyleBackColor = true;
			// 
			// GeocacheTable
			// 
			this.GeocacheTable.AllowUserToAddRows = false;
			this.GeocacheTable.AllowUserToDeleteRows = false;
			this.GeocacheTable.AllowUserToOrderColumns = true;
			this.GeocacheTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.GeocacheTable.Dock = System.Windows.Forms.DockStyle.Fill;
			this.GeocacheTable.Location = new System.Drawing.Point(3, 3);
			this.GeocacheTable.Name = "GeocacheTable";
			this.GeocacheTable.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.GeocacheTable.Size = new System.Drawing.Size(974, 558);
			this.GeocacheTable.TabIndex = 0;
			// 
			// LeftTabs
			// 
			this.LeftTabs.Controls.Add(this.Firststeps);
			this.LeftTabs.Controls.Add(this.Overviewpage);
			this.LeftTabs.Controls.Add(this.Ratingprofiles);
			this.LeftTabs.Controls.Add(this.Routingprofiles);
			this.LeftTabs.Controls.Add(this.Settings);
			this.LeftTabs.Dock = System.Windows.Forms.DockStyle.Fill;
			this.LeftTabs.Location = new System.Drawing.Point(0, 0);
			this.LeftTabs.Margin = new System.Windows.Forms.Padding(0);
			this.LeftTabs.Name = "LeftTabs";
			this.LeftTabs.SelectedIndex = 0;
			this.LeftTabs.Size = new System.Drawing.Size(400, 590);
			this.LeftTabs.TabIndex = 2;
			// 
			// Firststeps
			// 
			this.Firststeps.Controls.Add(this.tableLayoutPanel17);
			this.Firststeps.Location = new System.Drawing.Point(4, 22);
			this.Firststeps.Name = "Firststeps";
			this.Firststeps.Size = new System.Drawing.Size(392, 564);
			this.Firststeps.TabIndex = 4;
			this.Firststeps.Text = "First steps";
			this.Firststeps.UseVisualStyleBackColor = true;
			// 
			// tableLayoutPanel17
			// 
			this.tableLayoutPanel17.ColumnCount = 1;
			this.tableLayoutPanel17.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel17.Controls.Add(this.webBrowser1, 0, 0);
			this.tableLayoutPanel17.Controls.Add(this.OpenWikiButton, 0, 1);
			this.tableLayoutPanel17.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel17.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel17.Name = "tableLayoutPanel17";
			this.tableLayoutPanel17.RowCount = 2;
			this.tableLayoutPanel17.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel17.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
			this.tableLayoutPanel17.Size = new System.Drawing.Size(392, 564);
			this.tableLayoutPanel17.TabIndex = 0;
			// 
			// webBrowser1
			// 
			this.webBrowser1.AllowWebBrowserDrop = false;
			this.webBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.webBrowser1.Location = new System.Drawing.Point(3, 3);
			this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
			this.webBrowser1.Name = "webBrowser1";
			this.webBrowser1.Size = new System.Drawing.Size(386, 528);
			this.webBrowser1.TabIndex = 1;
			this.webBrowser1.Navigating += new System.Windows.Forms.WebBrowserNavigatingEventHandler(this.webBrowser1_Navigating);
			// 
			// OpenWikiButton
			// 
			this.OpenWikiButton.Dock = System.Windows.Forms.DockStyle.Fill;
			this.OpenWikiButton.Location = new System.Drawing.Point(3, 537);
			this.OpenWikiButton.Name = "OpenWikiButton";
			this.OpenWikiButton.Size = new System.Drawing.Size(386, 24);
			this.OpenWikiButton.TabIndex = 2;
			this.OpenWikiButton.Text = "Open Wiki in Browser";
			this.OpenWikiButton.UseVisualStyleBackColor = true;
			this.OpenWikiButton.Click += new System.EventHandler(this.OpenWikiButton_Click);
			// 
			// Overviewpage
			// 
			this.Overviewpage.Controls.Add(this.NameStateTable);
			this.Overviewpage.Location = new System.Drawing.Point(4, 22);
			this.Overviewpage.Name = "Overviewpage";
			this.Overviewpage.Padding = new System.Windows.Forms.Padding(3);
			this.Overviewpage.Size = new System.Drawing.Size(392, 564);
			this.Overviewpage.TabIndex = 2;
			this.Overviewpage.Text = "Overview";
			this.Overviewpage.UseVisualStyleBackColor = true;
			// 
			// NameStateTable
			// 
			this.NameStateTable.ColumnCount = 1;
			this.NameStateTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.NameStateTable.Controls.Add(this.StateTableLayout, 0, 1);
			this.NameStateTable.Controls.Add(this.NameLabel, 0, 0);
			this.NameStateTable.Dock = System.Windows.Forms.DockStyle.Fill;
			this.NameStateTable.Location = new System.Drawing.Point(3, 3);
			this.NameStateTable.Name = "NameStateTable";
			this.NameStateTable.RowCount = 2;
			this.NameStateTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
			this.NameStateTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 90F));
			this.NameStateTable.Size = new System.Drawing.Size(386, 558);
			this.NameStateTable.TabIndex = 0;
			// 
			// StateTableLayout
			// 
			this.StateTableLayout.ColumnCount = 3;
			this.StateTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
			this.StateTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.StateTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.StateTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
			this.StateTableLayout.Controls.Add(this.GeocachesStateLabel, 0, 0);
			this.StateTableLayout.Controls.Add(this.RatingprofilesStateLabel, 0, 2);
			this.StateTableLayout.Controls.Add(this.RoutingprofilesStateLabel, 0, 4);
			this.StateTableLayout.Controls.Add(this.RouterDBStateLabel, 0, 6);
			this.StateTableLayout.Controls.Add(this.SetGeocacheDBButton, 1, 0);
			this.StateTableLayout.Controls.Add(this.ImportPQButton, 2, 0);
			this.StateTableLayout.Controls.Add(this.SetRatingprofileDBButton, 1, 2);
			this.StateTableLayout.Controls.Add(this.SetRoutingprofileDBButton, 1, 4);
			this.StateTableLayout.Controls.Add(this.SetRouterDBButton, 1, 6);
			this.StateTableLayout.Controls.Add(this.ImportPbfButton, 2, 6);
			this.StateTableLayout.Controls.Add(this.GetPQLabel, 0, 1);
			this.StateTableLayout.Controls.Add(this.GetPbfLabel, 0, 7);
			this.StateTableLayout.Controls.Add(this.NewRatingprofileDatbaseButton, 2, 2);
			this.StateTableLayout.Controls.Add(this.NewRoutingprofileDatabaseButton, 2, 4);
			this.StateTableLayout.Dock = System.Windows.Forms.DockStyle.Top;
			this.StateTableLayout.Location = new System.Drawing.Point(3, 58);
			this.StateTableLayout.Name = "StateTableLayout";
			this.StateTableLayout.RowCount = 9;
			this.StateTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.StateTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.StateTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.StateTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.StateTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.StateTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.StateTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.StateTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.StateTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.StateTableLayout.Size = new System.Drawing.Size(380, 461);
			this.StateTableLayout.TabIndex = 2;
			// 
			// GeocachesStateLabel
			// 
			this.GeocachesStateLabel.AutoSize = true;
			this.GeocachesStateLabel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.GeocachesStateLabel.Location = new System.Drawing.Point(3, 3);
			this.GeocachesStateLabel.Margin = new System.Windows.Forms.Padding(3);
			this.GeocachesStateLabel.Name = "GeocachesStateLabel";
			this.GeocachesStateLabel.Size = new System.Drawing.Size(136, 13);
			this.GeocachesStateLabel.TabIndex = 1;
			this.GeocachesStateLabel.Text = "0 Geocaches loaded";
			this.GeocachesStateLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// RatingprofilesStateLabel
			// 
			this.RatingprofilesStateLabel.AutoSize = true;
			this.RatingprofilesStateLabel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.RatingprofilesStateLabel.Location = new System.Drawing.Point(3, 65);
			this.RatingprofilesStateLabel.Margin = new System.Windows.Forms.Padding(3);
			this.RatingprofilesStateLabel.Name = "RatingprofilesStateLabel";
			this.RatingprofilesStateLabel.Size = new System.Drawing.Size(136, 13);
			this.RatingprofilesStateLabel.TabIndex = 2;
			this.RatingprofilesStateLabel.Text = "0 Ratingprofiles loaded";
			this.RatingprofilesStateLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// RoutingprofilesStateLabel
			// 
			this.RoutingprofilesStateLabel.AutoSize = true;
			this.RoutingprofilesStateLabel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.RoutingprofilesStateLabel.Location = new System.Drawing.Point(3, 125);
			this.RoutingprofilesStateLabel.Margin = new System.Windows.Forms.Padding(3);
			this.RoutingprofilesStateLabel.Name = "RoutingprofilesStateLabel";
			this.RoutingprofilesStateLabel.Size = new System.Drawing.Size(136, 13);
			this.RoutingprofilesStateLabel.TabIndex = 3;
			this.RoutingprofilesStateLabel.Text = "0 Routingprofiles loaded";
			this.RoutingprofilesStateLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// RouterDBStateLabel
			// 
			this.RouterDBStateLabel.AutoSize = true;
			this.RouterDBStateLabel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.RouterDBStateLabel.Location = new System.Drawing.Point(3, 185);
			this.RouterDBStateLabel.Margin = new System.Windows.Forms.Padding(3);
			this.RouterDBStateLabel.Name = "RouterDBStateLabel";
			this.RouterDBStateLabel.Size = new System.Drawing.Size(136, 13);
			this.RouterDBStateLabel.TabIndex = 4;
			this.RouterDBStateLabel.Text = "No RouterDB loaded";
			this.RouterDBStateLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// SetGeocacheDBButton
			// 
			this.SetGeocacheDBButton.Dock = System.Windows.Forms.DockStyle.Fill;
			this.SetGeocacheDBButton.Location = new System.Drawing.Point(145, 3);
			this.SetGeocacheDBButton.Name = "SetGeocacheDBButton";
			this.StateTableLayout.SetRowSpan(this.SetGeocacheDBButton, 2);
			this.SetGeocacheDBButton.Size = new System.Drawing.Size(112, 56);
			this.SetGeocacheDBButton.TabIndex = 1;
			this.SetGeocacheDBButton.Text = "Set Geocache Database";
			this.SetGeocacheDBButton.UseVisualStyleBackColor = true;
			this.SetGeocacheDBButton.Click += new System.EventHandler(this.setGeocachedatabaseButton_Click);
			// 
			// ImportPQButton
			// 
			this.ImportPQButton.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ImportPQButton.Location = new System.Drawing.Point(263, 3);
			this.ImportPQButton.Name = "ImportPQButton";
			this.StateTableLayout.SetRowSpan(this.ImportPQButton, 2);
			this.ImportPQButton.Size = new System.Drawing.Size(114, 56);
			this.ImportPQButton.TabIndex = 2;
			this.ImportPQButton.Text = "Import Pocket Query";
			this.ImportPQButton.UseVisualStyleBackColor = true;
			this.ImportPQButton.Click += new System.EventHandler(this.ImportGeocachesButton_Click);
			// 
			// SetRatingprofileDBButton
			// 
			this.SetRatingprofileDBButton.Dock = System.Windows.Forms.DockStyle.Fill;
			this.SetRatingprofileDBButton.Location = new System.Drawing.Point(145, 64);
			this.SetRatingprofileDBButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.SetRatingprofileDBButton.Name = "SetRatingprofileDBButton";
			this.StateTableLayout.SetRowSpan(this.SetRatingprofileDBButton, 2);
			this.SetRatingprofileDBButton.Size = new System.Drawing.Size(112, 56);
			this.SetRatingprofileDBButton.TabIndex = 3;
			this.SetRatingprofileDBButton.Text = "Set Ratingprofile Database";
			this.SetRatingprofileDBButton.UseVisualStyleBackColor = true;
			this.SetRatingprofileDBButton.Click += new System.EventHandler(this.setRatingprofiledatabaseButton_Click);
			// 
			// SetRoutingprofileDBButton
			// 
			this.SetRoutingprofileDBButton.Dock = System.Windows.Forms.DockStyle.Fill;
			this.SetRoutingprofileDBButton.Location = new System.Drawing.Point(145, 124);
			this.SetRoutingprofileDBButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.SetRoutingprofileDBButton.Name = "SetRoutingprofileDBButton";
			this.StateTableLayout.SetRowSpan(this.SetRoutingprofileDBButton, 2);
			this.SetRoutingprofileDBButton.Size = new System.Drawing.Size(112, 56);
			this.SetRoutingprofileDBButton.TabIndex = 5;
			this.SetRoutingprofileDBButton.Text = "Set Routingprofile Database";
			this.SetRoutingprofileDBButton.UseVisualStyleBackColor = true;
			this.SetRoutingprofileDBButton.Click += new System.EventHandler(this.setRoutingprofiledatabaseButton_Click);
			// 
			// SetRouterDBButton
			// 
			this.SetRouterDBButton.AutoSize = true;
			this.SetRouterDBButton.Dock = System.Windows.Forms.DockStyle.Fill;
			this.SetRouterDBButton.Location = new System.Drawing.Point(145, 184);
			this.SetRouterDBButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.SetRouterDBButton.Name = "SetRouterDBButton";
			this.StateTableLayout.SetRowSpan(this.SetRouterDBButton, 2);
			this.SetRouterDBButton.Size = new System.Drawing.Size(112, 56);
			this.SetRouterDBButton.TabIndex = 7;
			this.SetRouterDBButton.Text = "Set Router Database";
			this.SetRouterDBButton.UseVisualStyleBackColor = true;
			this.SetRouterDBButton.Click += new System.EventHandler(this.setRouterDBButton_Click);
			// 
			// ImportPbfButton
			// 
			this.ImportPbfButton.AutoSize = true;
			this.ImportPbfButton.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ImportPbfButton.Location = new System.Drawing.Point(263, 184);
			this.ImportPbfButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.ImportPbfButton.Name = "ImportPbfButton";
			this.StateTableLayout.SetRowSpan(this.ImportPbfButton, 2);
			this.ImportPbfButton.Size = new System.Drawing.Size(114, 56);
			this.ImportPbfButton.TabIndex = 8;
			this.ImportPbfButton.Text = "Import .pbf file";
			this.ImportPbfButton.UseVisualStyleBackColor = true;
			this.ImportPbfButton.Click += new System.EventHandler(this.ImportOSMDataButton_Click);
			// 
			// GetPQLabel
			// 
			this.GetPQLabel.AutoSize = true;
			this.GetPQLabel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.GetPQLabel.Location = new System.Drawing.Point(3, 22);
			this.GetPQLabel.Margin = new System.Windows.Forms.Padding(3);
			this.GetPQLabel.Name = "GetPQLabel";
			this.GetPQLabel.Size = new System.Drawing.Size(136, 37);
			this.GetPQLabel.TabIndex = 0;
			this.GetPQLabel.TabStop = true;
			this.GetPQLabel.Text = "Get Pocket Query";
			this.GetPQLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.GetPQLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.GetPQLabel_LinkClicked);
			// 
			// GetPbfLabel
			// 
			this.GetPbfLabel.AutoSize = true;
			this.GetPbfLabel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.GetPbfLabel.Location = new System.Drawing.Point(3, 204);
			this.GetPbfLabel.Margin = new System.Windows.Forms.Padding(3);
			this.GetPbfLabel.Name = "GetPbfLabel";
			this.GetPbfLabel.Size = new System.Drawing.Size(136, 35);
			this.GetPbfLabel.TabIndex = 5;
			this.GetPbfLabel.TabStop = true;
			this.GetPbfLabel.Text = "Get OSM pbf file";
			this.GetPbfLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.GetPbfLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.GetPbfLabel_LinkClicked);
			// 
			// NewRatingprofileDatbaseButton
			// 
			this.NewRatingprofileDatbaseButton.Dock = System.Windows.Forms.DockStyle.Fill;
			this.NewRatingprofileDatbaseButton.Location = new System.Drawing.Point(263, 65);
			this.NewRatingprofileDatbaseButton.Name = "NewRatingprofileDatbaseButton";
			this.StateTableLayout.SetRowSpan(this.NewRatingprofileDatbaseButton, 2);
			this.NewRatingprofileDatbaseButton.Size = new System.Drawing.Size(114, 54);
			this.NewRatingprofileDatbaseButton.TabIndex = 4;
			this.NewRatingprofileDatbaseButton.Text = "New, empty Database";
			this.NewRatingprofileDatbaseButton.UseVisualStyleBackColor = true;
			this.NewRatingprofileDatbaseButton.Click += new System.EventHandler(this.NewRatingprofileDatabaseButton_Click);
			// 
			// NewRoutingprofileDatabaseButton
			// 
			this.NewRoutingprofileDatabaseButton.Dock = System.Windows.Forms.DockStyle.Fill;
			this.NewRoutingprofileDatabaseButton.Location = new System.Drawing.Point(263, 125);
			this.NewRoutingprofileDatabaseButton.Name = "NewRoutingprofileDatabaseButton";
			this.StateTableLayout.SetRowSpan(this.NewRoutingprofileDatabaseButton, 2);
			this.NewRoutingprofileDatabaseButton.Size = new System.Drawing.Size(114, 54);
			this.NewRoutingprofileDatabaseButton.TabIndex = 6;
			this.NewRoutingprofileDatabaseButton.Text = "New, empty Database";
			this.NewRoutingprofileDatabaseButton.UseVisualStyleBackColor = true;
			this.NewRoutingprofileDatabaseButton.Click += new System.EventHandler(this.NewRoutingprofilesDatabaseButton_Click);
			// 
			// NameLabel
			// 
			this.NameLabel.AutoSize = true;
			this.NameStateTable.SetColumnSpan(this.NameLabel, 2);
			this.NameLabel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.NameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 80F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Document, ((byte)(0)));
			this.NameLabel.Location = new System.Drawing.Point(5, 5);
			this.NameLabel.Margin = new System.Windows.Forms.Padding(5);
			this.NameLabel.Name = "NameLabel";
			this.NameLabel.Size = new System.Drawing.Size(376, 45);
			this.NameLabel.TabIndex = 0;
			this.NameLabel.Text = "GeocachingTourPlanner";
			this.NameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// Ratingprofiles
			// 
			this.Ratingprofiles.Controls.Add(this.panel1);
			this.Ratingprofiles.Controls.Add(this.SaveRatingprofileLayoutPanel);
			this.Ratingprofiles.Location = new System.Drawing.Point(4, 22);
			this.Ratingprofiles.Name = "Ratingprofiles";
			this.Ratingprofiles.Padding = new System.Windows.Forms.Padding(3);
			this.Ratingprofiles.Size = new System.Drawing.Size(392, 564);
			this.Ratingprofiles.TabIndex = 0;
			this.Ratingprofiles.Text = "Ratingprofiles";
			this.Ratingprofiles.UseVisualStyleBackColor = true;
			// 
			// panel1
			// 
			this.panel1.AutoScroll = true;
			this.panel1.Controls.Add(this.RatingprofilesSettingsTabelLayout);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(3, 3);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(386, 528);
			this.panel1.TabIndex = 10;
			// 
			// RatingprofilesSettingsTabelLayout
			// 
			this.RatingprofilesSettingsTabelLayout.AutoSize = true;
			this.RatingprofilesSettingsTabelLayout.ColumnCount = 1;
			this.RatingprofilesSettingsTabelLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.RatingprofilesSettingsTabelLayout.Controls.Add(this.AgeGroupBox, 0, 6);
			this.RatingprofilesSettingsTabelLayout.Controls.Add(this.DValueungGroupBox, 0, 4);
			this.RatingprofilesSettingsTabelLayout.Controls.Add(this.GeocachetypGroupBox, 0, 2);
			this.RatingprofilesSettingsTabelLayout.Controls.Add(this.RatingprofileInfoLabel, 0, 0);
			this.RatingprofilesSettingsTabelLayout.Controls.Add(this.GeocachegrößeGroupBox, 0, 3);
			this.RatingprofilesSettingsTabelLayout.Controls.Add(this.TValueungGroupbox, 0, 5);
			this.RatingprofilesSettingsTabelLayout.Controls.Add(this.Other, 0, 7);
			this.RatingprofilesSettingsTabelLayout.Controls.Add(this.tableLayoutPanel2, 0, 1);
			this.RatingprofilesSettingsTabelLayout.Dock = System.Windows.Forms.DockStyle.Top;
			this.RatingprofilesSettingsTabelLayout.Location = new System.Drawing.Point(0, 0);
			this.RatingprofilesSettingsTabelLayout.Name = "RatingprofilesSettingsTabelLayout";
			this.RatingprofilesSettingsTabelLayout.RowCount = 8;
			this.RatingprofilesSettingsTabelLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.RatingprofilesSettingsTabelLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.RatingprofilesSettingsTabelLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.RatingprofilesSettingsTabelLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.RatingprofilesSettingsTabelLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.RatingprofilesSettingsTabelLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.RatingprofilesSettingsTabelLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.RatingprofilesSettingsTabelLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.RatingprofilesSettingsTabelLayout.Size = new System.Drawing.Size(369, 683);
			this.RatingprofilesSettingsTabelLayout.TabIndex = 4;
			// 
			// AgeGroupBox
			// 
			this.AgeGroupBox.AutoSize = true;
			this.AgeGroupBox.Controls.Add(this.tableLayoutPanel8);
			this.AgeGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.AgeGroupBox.Location = new System.Drawing.Point(3, 581);
			this.AgeGroupBox.Name = "AgeGroupBox";
			this.AgeGroupBox.Size = new System.Drawing.Size(363, 46);
			this.AgeGroupBox.TabIndex = 5;
			this.AgeGroupBox.TabStop = false;
			this.AgeGroupBox.Text = "Age";
			// 
			// tableLayoutPanel8
			// 
			this.tableLayoutPanel8.AutoSize = true;
			this.tableLayoutPanel8.ColumnCount = 2;
			this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 80F));
			this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
			this.tableLayoutPanel8.Controls.Add(this.AgeFactorValue, 0, 0);
			this.tableLayoutPanel8.Controls.Add(this.AgeValue, 0, 0);
			this.tableLayoutPanel8.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel8.Location = new System.Drawing.Point(3, 16);
			this.tableLayoutPanel8.Name = "tableLayoutPanel8";
			this.tableLayoutPanel8.RowCount = 1;
			this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel8.Size = new System.Drawing.Size(357, 27);
			this.tableLayoutPanel8.TabIndex = 0;
			// 
			// AgeFactorValue
			// 
			this.AgeFactorValue.Location = new System.Drawing.Point(288, 3);
			this.AgeFactorValue.Name = "AgeFactorValue";
			this.AgeFactorValue.Size = new System.Drawing.Size(30, 20);
			this.AgeFactorValue.TabIndex = 24;
			// 
			// AgeValue
			// 
			this.AgeValue.Dock = System.Windows.Forms.DockStyle.Left;
			this.AgeValue.FormattingEnabled = true;
			this.AgeValue.Items.AddRange(new object[] {
            "multiply with",
            "square and divide by"});
			this.AgeValue.Location = new System.Drawing.Point(3, 3);
			this.AgeValue.Name = "AgeValue";
			this.AgeValue.Size = new System.Drawing.Size(206, 21);
			this.AgeValue.TabIndex = 1;
			this.AgeValue.SelectedIndexChanged += new System.EventHandler(this.Dropdown_SelectedIndexChanged);
			// 
			// DValueungGroupBox
			// 
			this.DValueungGroupBox.AutoSize = true;
			this.DValueungGroupBox.Controls.Add(this.tableLayoutPanel4);
			this.DValueungGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.DValueungGroupBox.Location = new System.Drawing.Point(3, 375);
			this.DValueungGroupBox.Name = "DValueungGroupBox";
			this.DValueungGroupBox.Size = new System.Drawing.Size(363, 97);
			this.DValueungGroupBox.TabIndex = 2;
			this.DValueungGroupBox.TabStop = false;
			this.DValueungGroupBox.Text = "D-Rating";
			// 
			// tableLayoutPanel4
			// 
			this.tableLayoutPanel4.AutoSize = true;
			this.tableLayoutPanel4.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel4.ColumnCount = 8;
			this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel4.Controls.Add(this.label22, 2, 1);
			this.tableLayoutPanel4.Controls.Add(this.label23, 2, 2);
			this.tableLayoutPanel4.Controls.Add(this.label26, 2, 0);
			this.tableLayoutPanel4.Controls.Add(this.label27, 4, 2);
			this.tableLayoutPanel4.Controls.Add(this.label28, 6, 0);
			this.tableLayoutPanel4.Controls.Add(this.label29, 6, 1);
			this.tableLayoutPanel4.Controls.Add(this.label30, 6, 2);
			this.tableLayoutPanel4.Controls.Add(this.label24, 4, 0);
			this.tableLayoutPanel4.Controls.Add(this.label25, 4, 1);
			this.tableLayoutPanel4.Controls.Add(this.label21, 0, 0);
			this.tableLayoutPanel4.Controls.Add(this.DPriorityValue, 1, 0);
			this.tableLayoutPanel4.Controls.Add(this.D5Value, 3, 0);
			this.tableLayoutPanel4.Controls.Add(this.D35Value, 5, 0);
			this.tableLayoutPanel4.Controls.Add(this.D2Value, 7, 0);
			this.tableLayoutPanel4.Controls.Add(this.D45Value, 3, 1);
			this.tableLayoutPanel4.Controls.Add(this.D4Value, 3, 2);
			this.tableLayoutPanel4.Controls.Add(this.D3Value, 5, 1);
			this.tableLayoutPanel4.Controls.Add(this.D15Value, 7, 1);
			this.tableLayoutPanel4.Controls.Add(this.D25Value, 5, 2);
			this.tableLayoutPanel4.Controls.Add(this.D1Value, 7, 2);
			this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 16);
			this.tableLayoutPanel4.Name = "tableLayoutPanel4";
			this.tableLayoutPanel4.RowCount = 3;
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel4.Size = new System.Drawing.Size(357, 78);
			this.tableLayoutPanel4.TabIndex = 1;
			// 
			// label22
			// 
			this.label22.AutoSize = true;
			this.label22.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label22.Location = new System.Drawing.Point(83, 29);
			this.label22.Margin = new System.Windows.Forms.Padding(3);
			this.label22.Name = "label22";
			this.label22.Size = new System.Drawing.Size(30, 20);
			this.label22.TabIndex = 0;
			this.label22.Text = "D4.5";
			// 
			// label23
			// 
			this.label23.AutoSize = true;
			this.label23.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label23.Location = new System.Drawing.Point(83, 55);
			this.label23.Margin = new System.Windows.Forms.Padding(3);
			this.label23.Name = "label23";
			this.label23.Size = new System.Drawing.Size(30, 20);
			this.label23.TabIndex = 1;
			this.label23.Text = "D4";
			// 
			// label26
			// 
			this.label26.AutoSize = true;
			this.label26.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label26.Location = new System.Drawing.Point(83, 3);
			this.label26.Margin = new System.Windows.Forms.Padding(3);
			this.label26.Name = "label26";
			this.label26.Size = new System.Drawing.Size(30, 20);
			this.label26.TabIndex = 4;
			this.label26.Text = "D5";
			// 
			// label27
			// 
			this.label27.AutoSize = true;
			this.label27.Location = new System.Drawing.Point(155, 55);
			this.label27.Margin = new System.Windows.Forms.Padding(3);
			this.label27.Name = "label27";
			this.label27.Size = new System.Drawing.Size(30, 13);
			this.label27.TabIndex = 7;
			this.label27.Text = "D2.5";
			// 
			// label28
			// 
			this.label28.AutoSize = true;
			this.label28.Location = new System.Drawing.Point(227, 3);
			this.label28.Margin = new System.Windows.Forms.Padding(3);
			this.label28.Name = "label28";
			this.label28.Size = new System.Drawing.Size(21, 13);
			this.label28.TabIndex = 8;
			this.label28.Text = "D2";
			// 
			// label29
			// 
			this.label29.AutoSize = true;
			this.label29.Location = new System.Drawing.Point(227, 29);
			this.label29.Margin = new System.Windows.Forms.Padding(3);
			this.label29.Name = "label29";
			this.label29.Size = new System.Drawing.Size(30, 13);
			this.label29.TabIndex = 9;
			this.label29.Text = "D1.5";
			// 
			// label30
			// 
			this.label30.AutoSize = true;
			this.label30.Location = new System.Drawing.Point(227, 55);
			this.label30.Margin = new System.Windows.Forms.Padding(3);
			this.label30.Name = "label30";
			this.label30.Size = new System.Drawing.Size(21, 13);
			this.label30.TabIndex = 10;
			this.label30.Text = "D1";
			// 
			// label24
			// 
			this.label24.AutoSize = true;
			this.label24.Location = new System.Drawing.Point(155, 3);
			this.label24.Margin = new System.Windows.Forms.Padding(3);
			this.label24.Name = "label24";
			this.label24.Size = new System.Drawing.Size(30, 13);
			this.label24.TabIndex = 2;
			this.label24.Text = "D3.5";
			// 
			// label25
			// 
			this.label25.AutoSize = true;
			this.label25.Location = new System.Drawing.Point(155, 29);
			this.label25.Margin = new System.Windows.Forms.Padding(3);
			this.label25.Name = "label25";
			this.label25.Size = new System.Drawing.Size(21, 13);
			this.label25.TabIndex = 3;
			this.label25.Text = "D3";
			// 
			// label21
			// 
			this.label21.AutoSize = true;
			this.label21.Location = new System.Drawing.Point(3, 3);
			this.label21.Margin = new System.Windows.Forms.Padding(3);
			this.label21.Name = "label21";
			this.label21.Size = new System.Drawing.Size(38, 13);
			this.label21.TabIndex = 5;
			this.label21.Text = "Priority";
			// 
			// DPriorityValue
			// 
			this.DPriorityValue.Location = new System.Drawing.Point(47, 3);
			this.DPriorityValue.Name = "DPriorityValue";
			this.DPriorityValue.Size = new System.Drawing.Size(30, 20);
			this.DPriorityValue.TabIndex = 24;
			// 
			// D5Value
			// 
			this.D5Value.Location = new System.Drawing.Point(119, 3);
			this.D5Value.Name = "D5Value";
			this.D5Value.Size = new System.Drawing.Size(30, 20);
			this.D5Value.TabIndex = 25;
			// 
			// D35Value
			// 
			this.D35Value.Location = new System.Drawing.Point(191, 3);
			this.D35Value.Name = "D35Value";
			this.D35Value.Size = new System.Drawing.Size(30, 20);
			this.D35Value.TabIndex = 26;
			// 
			// D2Value
			// 
			this.D2Value.Location = new System.Drawing.Point(263, 3);
			this.D2Value.Name = "D2Value";
			this.D2Value.Size = new System.Drawing.Size(30, 20);
			this.D2Value.TabIndex = 27;
			// 
			// D45Value
			// 
			this.D45Value.Location = new System.Drawing.Point(119, 29);
			this.D45Value.Name = "D45Value";
			this.D45Value.Size = new System.Drawing.Size(30, 20);
			this.D45Value.TabIndex = 28;
			// 
			// D4Value
			// 
			this.D4Value.Location = new System.Drawing.Point(119, 55);
			this.D4Value.Name = "D4Value";
			this.D4Value.Size = new System.Drawing.Size(30, 20);
			this.D4Value.TabIndex = 29;
			// 
			// D3Value
			// 
			this.D3Value.Location = new System.Drawing.Point(191, 29);
			this.D3Value.Name = "D3Value";
			this.D3Value.Size = new System.Drawing.Size(30, 20);
			this.D3Value.TabIndex = 30;
			// 
			// D15Value
			// 
			this.D15Value.Location = new System.Drawing.Point(263, 29);
			this.D15Value.Name = "D15Value";
			this.D15Value.Size = new System.Drawing.Size(30, 20);
			this.D15Value.TabIndex = 31;
			// 
			// D25Value
			// 
			this.D25Value.Location = new System.Drawing.Point(191, 55);
			this.D25Value.Name = "D25Value";
			this.D25Value.Size = new System.Drawing.Size(30, 20);
			this.D25Value.TabIndex = 32;
			// 
			// D1Value
			// 
			this.D1Value.Location = new System.Drawing.Point(263, 55);
			this.D1Value.Name = "D1Value";
			this.D1Value.Size = new System.Drawing.Size(30, 20);
			this.D1Value.TabIndex = 33;
			// 
			// GeocachetypGroupBox
			// 
			this.GeocachetypGroupBox.AutoSize = true;
			this.GeocachetypGroupBox.Controls.Add(this.tableLayoutPanel7);
			this.GeocachetypGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.GeocachetypGroupBox.Location = new System.Drawing.Point(3, 143);
			this.GeocachetypGroupBox.Name = "GeocachetypGroupBox";
			this.GeocachetypGroupBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.GeocachetypGroupBox.Size = new System.Drawing.Size(363, 123);
			this.GeocachetypGroupBox.TabIndex = 0;
			this.GeocachetypGroupBox.TabStop = false;
			this.GeocachetypGroupBox.Text = "Geocachetype";
			// 
			// tableLayoutPanel7
			// 
			this.tableLayoutPanel7.AutoSize = true;
			this.tableLayoutPanel7.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel7.ColumnCount = 6;
			this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel7.Controls.Add(this.WebcamValue, 5, 3);
			this.tableLayoutPanel7.Controls.Add(this.WherigoValue, 3, 3);
			this.tableLayoutPanel7.Controls.Add(this.MysteryValue, 1, 3);
			this.tableLayoutPanel7.Controls.Add(this.OtherTypeValue, 5, 2);
			this.tableLayoutPanel7.Controls.Add(this.LetterboxValue, 3, 2);
			this.tableLayoutPanel7.Controls.Add(this.MultiValue, 1, 2);
			this.tableLayoutPanel7.Controls.Add(this.VirtualValue, 5, 1);
			this.tableLayoutPanel7.Controls.Add(this.EarthcacheValue, 3, 1);
			this.tableLayoutPanel7.Controls.Add(this.TypePriorityValue, 1, 0);
			this.tableLayoutPanel7.Controls.Add(this.label6, 0, 0);
			this.tableLayoutPanel7.Controls.Add(this.label3, 0, 2);
			this.tableLayoutPanel7.Controls.Add(this.label4, 0, 3);
			this.tableLayoutPanel7.Controls.Add(this.label9, 0, 1);
			this.tableLayoutPanel7.Controls.Add(this.label10, 2, 1);
			this.tableLayoutPanel7.Controls.Add(this.label11, 2, 2);
			this.tableLayoutPanel7.Controls.Add(this.label14, 2, 3);
			this.tableLayoutPanel7.Controls.Add(this.label5, 4, 1);
			this.tableLayoutPanel7.Controls.Add(this.label19, 4, 2);
			this.tableLayoutPanel7.Controls.Add(this.label8, 4, 3);
			this.tableLayoutPanel7.Controls.Add(this.TraditionalValue, 1, 1);
			this.tableLayoutPanel7.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel7.Location = new System.Drawing.Point(3, 16);
			this.tableLayoutPanel7.Name = "tableLayoutPanel7";
			this.tableLayoutPanel7.RowCount = 4;
			this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel7.Size = new System.Drawing.Size(357, 104);
			this.tableLayoutPanel7.TabIndex = 0;
			// 
			// WebcamValue
			// 
			this.WebcamValue.Location = new System.Drawing.Point(261, 81);
			this.WebcamValue.Name = "WebcamValue";
			this.WebcamValue.Size = new System.Drawing.Size(30, 20);
			this.WebcamValue.TabIndex = 36;
			// 
			// WherigoValue
			// 
			this.WherigoValue.Location = new System.Drawing.Point(169, 81);
			this.WherigoValue.Name = "WherigoValue";
			this.WherigoValue.Size = new System.Drawing.Size(30, 20);
			this.WherigoValue.TabIndex = 35;
			// 
			// MysteryValue
			// 
			this.MysteryValue.Location = new System.Drawing.Point(65, 81);
			this.MysteryValue.Name = "MysteryValue";
			this.MysteryValue.Size = new System.Drawing.Size(30, 20);
			this.MysteryValue.TabIndex = 34;
			// 
			// OtherTypeValue
			// 
			this.OtherTypeValue.Location = new System.Drawing.Point(261, 55);
			this.OtherTypeValue.Name = "OtherTypeValue";
			this.OtherTypeValue.Size = new System.Drawing.Size(30, 20);
			this.OtherTypeValue.TabIndex = 33;
			// 
			// LetterboxValue
			// 
			this.LetterboxValue.Location = new System.Drawing.Point(169, 55);
			this.LetterboxValue.Name = "LetterboxValue";
			this.LetterboxValue.Size = new System.Drawing.Size(30, 20);
			this.LetterboxValue.TabIndex = 32;
			// 
			// MultiValue
			// 
			this.MultiValue.Location = new System.Drawing.Point(65, 55);
			this.MultiValue.Name = "MultiValue";
			this.MultiValue.Size = new System.Drawing.Size(30, 20);
			this.MultiValue.TabIndex = 31;
			// 
			// VirtualValue
			// 
			this.VirtualValue.Location = new System.Drawing.Point(261, 29);
			this.VirtualValue.Name = "VirtualValue";
			this.VirtualValue.Size = new System.Drawing.Size(30, 20);
			this.VirtualValue.TabIndex = 30;
			// 
			// EarthcacheValue
			// 
			this.EarthcacheValue.Location = new System.Drawing.Point(169, 29);
			this.EarthcacheValue.Name = "EarthcacheValue";
			this.EarthcacheValue.Size = new System.Drawing.Size(30, 20);
			this.EarthcacheValue.TabIndex = 29;
			// 
			// TypePriorityValue
			// 
			this.TypePriorityValue.Location = new System.Drawing.Point(65, 3);
			this.TypePriorityValue.Name = "TypePriorityValue";
			this.TypePriorityValue.Size = new System.Drawing.Size(30, 20);
			this.TypePriorityValue.TabIndex = 24;
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label6.Location = new System.Drawing.Point(3, 3);
			this.label6.Margin = new System.Windows.Forms.Padding(3);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(56, 20);
			this.label6.TabIndex = 5;
			this.label6.Text = "Priority";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label3.Location = new System.Drawing.Point(3, 55);
			this.label3.Margin = new System.Windows.Forms.Padding(3);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(56, 20);
			this.label3.TabIndex = 0;
			this.label3.Text = "Multi";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label4.Location = new System.Drawing.Point(3, 81);
			this.label4.Margin = new System.Windows.Forms.Padding(3);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(56, 20);
			this.label4.TabIndex = 1;
			this.label4.Text = "Mystery";
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label9.Location = new System.Drawing.Point(3, 29);
			this.label9.Margin = new System.Windows.Forms.Padding(3);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(56, 20);
			this.label9.TabIndex = 4;
			this.label9.Text = "Traditional";
			// 
			// label10
			// 
			this.label10.AutoSize = true;
			this.label10.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label10.Location = new System.Drawing.Point(101, 29);
			this.label10.Margin = new System.Windows.Forms.Padding(3);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(62, 20);
			this.label10.TabIndex = 7;
			this.label10.Text = "Earthcache";
			// 
			// label11
			// 
			this.label11.AutoSize = true;
			this.label11.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label11.Location = new System.Drawing.Point(101, 55);
			this.label11.Margin = new System.Windows.Forms.Padding(3);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(62, 20);
			this.label11.TabIndex = 8;
			this.label11.Text = "Letterbox";
			// 
			// label14
			// 
			this.label14.AutoSize = true;
			this.label14.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label14.Location = new System.Drawing.Point(101, 81);
			this.label14.Margin = new System.Windows.Forms.Padding(3);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(62, 20);
			this.label14.TabIndex = 9;
			this.label14.Text = "Wherigo";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(205, 29);
			this.label5.Margin = new System.Windows.Forms.Padding(3);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(36, 13);
			this.label5.TabIndex = 2;
			this.label5.Text = "Virtual";
			// 
			// label19
			// 
			this.label19.AutoSize = true;
			this.label19.Location = new System.Drawing.Point(205, 55);
			this.label19.Margin = new System.Windows.Forms.Padding(3);
			this.label19.Name = "label19";
			this.label19.Size = new System.Drawing.Size(33, 13);
			this.label19.TabIndex = 10;
			this.label19.Text = "Other";
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(205, 81);
			this.label8.Margin = new System.Windows.Forms.Padding(3);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(50, 13);
			this.label8.TabIndex = 3;
			this.label8.Text = "Webcam";
			// 
			// TraditionalValue
			// 
			this.TraditionalValue.Location = new System.Drawing.Point(65, 29);
			this.TraditionalValue.Name = "TraditionalValue";
			this.TraditionalValue.Size = new System.Drawing.Size(30, 20);
			this.TraditionalValue.TabIndex = 23;
			// 
			// RatingprofileInfoLabel
			// 
			this.RatingprofileInfoLabel.AutoSize = true;
			this.RatingprofilesSettingsTabelLayout.SetColumnSpan(this.RatingprofileInfoLabel, 2);
			this.RatingprofileInfoLabel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.RatingprofileInfoLabel.Location = new System.Drawing.Point(3, 3);
			this.RatingprofileInfoLabel.Margin = new System.Windows.Forms.Padding(3);
			this.RatingprofileInfoLabel.Name = "RatingprofileInfoLabel";
			this.RatingprofileInfoLabel.Size = new System.Drawing.Size(363, 104);
			this.RatingprofileInfoLabel.TabIndex = 6;
			this.RatingprofileInfoLabel.Text = resources.GetString("RatingprofileInfoLabel.Text");
			// 
			// GeocachegrößeGroupBox
			// 
			this.GeocachegrößeGroupBox.AutoSize = true;
			this.GeocachegrößeGroupBox.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.GeocachegrößeGroupBox.Controls.Add(this.tableLayoutPanel3);
			this.GeocachegrößeGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.GeocachegrößeGroupBox.Location = new System.Drawing.Point(3, 272);
			this.GeocachegrößeGroupBox.Name = "GeocachegrößeGroupBox";
			this.GeocachegrößeGroupBox.Size = new System.Drawing.Size(363, 97);
			this.GeocachegrößeGroupBox.TabIndex = 1;
			this.GeocachegrößeGroupBox.TabStop = false;
			this.GeocachegrößeGroupBox.Text = "Geocachesize";
			// 
			// tableLayoutPanel3
			// 
			this.tableLayoutPanel3.AutoSize = true;
			this.tableLayoutPanel3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel3.ColumnCount = 6;
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel3.Controls.Add(this.RegularValue, 0, 2);
			this.tableLayoutPanel3.Controls.Add(this.label7, 0, 0);
			this.tableLayoutPanel3.Controls.Add(this.label12, 0, 2);
			this.tableLayoutPanel3.Controls.Add(this.label16, 0, 1);
			this.tableLayoutPanel3.Controls.Add(this.label18, 4, 1);
			this.tableLayoutPanel3.Controls.Add(this.label17, 2, 2);
			this.tableLayoutPanel3.Controls.Add(this.label13, 2, 1);
			this.tableLayoutPanel3.Controls.Add(this.SizePriorityValue, 1, 0);
			this.tableLayoutPanel3.Controls.Add(this.LargeValue, 1, 1);
			this.tableLayoutPanel3.Controls.Add(this.SmallValue, 3, 1);
			this.tableLayoutPanel3.Controls.Add(this.MicroValue, 3, 2);
			this.tableLayoutPanel3.Controls.Add(this.OtherSizeValue, 5, 1);
			this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 16);
			this.tableLayoutPanel3.Name = "tableLayoutPanel3";
			this.tableLayoutPanel3.RowCount = 3;
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.Size = new System.Drawing.Size(357, 78);
			this.tableLayoutPanel3.TabIndex = 1;
			// 
			// RegularValue
			// 
			this.RegularValue.Location = new System.Drawing.Point(53, 55);
			this.RegularValue.Name = "RegularValue";
			this.RegularValue.Size = new System.Drawing.Size(30, 20);
			this.RegularValue.TabIndex = 29;
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label7.Location = new System.Drawing.Point(3, 3);
			this.label7.Margin = new System.Windows.Forms.Padding(3);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(44, 20);
			this.label7.TabIndex = 5;
			this.label7.Text = "Priority";
			// 
			// label12
			// 
			this.label12.AutoSize = true;
			this.label12.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label12.Location = new System.Drawing.Point(3, 55);
			this.label12.Margin = new System.Windows.Forms.Padding(3);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(44, 20);
			this.label12.TabIndex = 0;
			this.label12.Text = "Regular";
			// 
			// label16
			// 
			this.label16.AutoSize = true;
			this.label16.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label16.Location = new System.Drawing.Point(3, 29);
			this.label16.Margin = new System.Windows.Forms.Padding(3);
			this.label16.Name = "label16";
			this.label16.Size = new System.Drawing.Size(44, 20);
			this.label16.TabIndex = 4;
			this.label16.Text = "Large";
			// 
			// label18
			// 
			this.label18.AutoSize = true;
			this.label18.Location = new System.Drawing.Point(164, 29);
			this.label18.Margin = new System.Windows.Forms.Padding(3);
			this.label18.Name = "label18";
			this.label18.Size = new System.Drawing.Size(33, 13);
			this.label18.TabIndex = 8;
			this.label18.Text = "Other";
			// 
			// label17
			// 
			this.label17.AutoSize = true;
			this.label17.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label17.Location = new System.Drawing.Point(89, 55);
			this.label17.Margin = new System.Windows.Forms.Padding(3);
			this.label17.Name = "label17";
			this.label17.Size = new System.Drawing.Size(33, 20);
			this.label17.TabIndex = 7;
			this.label17.Text = "Micro";
			// 
			// label13
			// 
			this.label13.AutoSize = true;
			this.label13.Location = new System.Drawing.Point(89, 29);
			this.label13.Margin = new System.Windows.Forms.Padding(3);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(32, 13);
			this.label13.TabIndex = 1;
			this.label13.Text = "Small";
			// 
			// SizePriorityValue
			// 
			this.SizePriorityValue.Location = new System.Drawing.Point(53, 3);
			this.SizePriorityValue.Name = "SizePriorityValue";
			this.SizePriorityValue.Size = new System.Drawing.Size(30, 20);
			this.SizePriorityValue.TabIndex = 24;
			// 
			// LargeValue
			// 
			this.LargeValue.Location = new System.Drawing.Point(53, 29);
			this.LargeValue.Name = "LargeValue";
			this.LargeValue.Size = new System.Drawing.Size(30, 20);
			this.LargeValue.TabIndex = 26;
			// 
			// SmallValue
			// 
			this.SmallValue.Location = new System.Drawing.Point(128, 29);
			this.SmallValue.Name = "SmallValue";
			this.SmallValue.Size = new System.Drawing.Size(30, 20);
			this.SmallValue.TabIndex = 27;
			// 
			// MicroValue
			// 
			this.MicroValue.Location = new System.Drawing.Point(128, 55);
			this.MicroValue.Name = "MicroValue";
			this.MicroValue.Size = new System.Drawing.Size(30, 20);
			this.MicroValue.TabIndex = 25;
			// 
			// OtherSizeValue
			// 
			this.OtherSizeValue.Location = new System.Drawing.Point(203, 29);
			this.OtherSizeValue.Name = "OtherSizeValue";
			this.OtherSizeValue.Size = new System.Drawing.Size(30, 20);
			this.OtherSizeValue.TabIndex = 28;
			// 
			// TValueungGroupbox
			// 
			this.TValueungGroupbox.AutoSize = true;
			this.TValueungGroupbox.Controls.Add(this.tableLayoutPanel5);
			this.TValueungGroupbox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TValueungGroupbox.Location = new System.Drawing.Point(3, 478);
			this.TValueungGroupbox.Name = "TValueungGroupbox";
			this.TValueungGroupbox.Size = new System.Drawing.Size(363, 97);
			this.TValueungGroupbox.TabIndex = 3;
			this.TValueungGroupbox.TabStop = false;
			this.TValueungGroupbox.Text = "T-Rating";
			// 
			// tableLayoutPanel5
			// 
			this.tableLayoutPanel5.AutoSize = true;
			this.tableLayoutPanel5.ColumnCount = 8;
			this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel5.Controls.Add(this.label32, 2, 1);
			this.tableLayoutPanel5.Controls.Add(this.label33, 2, 2);
			this.tableLayoutPanel5.Controls.Add(this.label36, 2, 0);
			this.tableLayoutPanel5.Controls.Add(this.label38, 6, 0);
			this.tableLayoutPanel5.Controls.Add(this.label39, 6, 1);
			this.tableLayoutPanel5.Controls.Add(this.label40, 6, 2);
			this.tableLayoutPanel5.Controls.Add(this.label37, 4, 2);
			this.tableLayoutPanel5.Controls.Add(this.label34, 4, 0);
			this.tableLayoutPanel5.Controls.Add(this.label35, 4, 1);
			this.tableLayoutPanel5.Controls.Add(this.label31, 0, 0);
			this.tableLayoutPanel5.Controls.Add(this.TPriorityValue, 1, 0);
			this.tableLayoutPanel5.Controls.Add(this.T5Value, 3, 0);
			this.tableLayoutPanel5.Controls.Add(this.T35Value, 5, 0);
			this.tableLayoutPanel5.Controls.Add(this.T2Value, 7, 0);
			this.tableLayoutPanel5.Controls.Add(this.T45Value, 3, 1);
			this.tableLayoutPanel5.Controls.Add(this.T3Value, 5, 1);
			this.tableLayoutPanel5.Controls.Add(this.T15Value, 7, 1);
			this.tableLayoutPanel5.Controls.Add(this.T4Value, 3, 2);
			this.tableLayoutPanel5.Controls.Add(this.T25Value, 5, 2);
			this.tableLayoutPanel5.Controls.Add(this.T1Value, 7, 2);
			this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel5.Location = new System.Drawing.Point(3, 16);
			this.tableLayoutPanel5.Name = "tableLayoutPanel5";
			this.tableLayoutPanel5.RowCount = 3;
			this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel5.Size = new System.Drawing.Size(357, 78);
			this.tableLayoutPanel5.TabIndex = 1;
			// 
			// label32
			// 
			this.label32.AutoSize = true;
			this.label32.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label32.Location = new System.Drawing.Point(83, 29);
			this.label32.Margin = new System.Windows.Forms.Padding(3);
			this.label32.Name = "label32";
			this.label32.Size = new System.Drawing.Size(29, 20);
			this.label32.TabIndex = 0;
			this.label32.Text = "T4.5";
			// 
			// label33
			// 
			this.label33.AutoSize = true;
			this.label33.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label33.Location = new System.Drawing.Point(83, 55);
			this.label33.Margin = new System.Windows.Forms.Padding(3);
			this.label33.Name = "label33";
			this.label33.Size = new System.Drawing.Size(29, 20);
			this.label33.TabIndex = 1;
			this.label33.Text = "T4";
			// 
			// label36
			// 
			this.label36.AutoSize = true;
			this.label36.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label36.Location = new System.Drawing.Point(83, 3);
			this.label36.Margin = new System.Windows.Forms.Padding(3);
			this.label36.Name = "label36";
			this.label36.Size = new System.Drawing.Size(29, 20);
			this.label36.TabIndex = 4;
			this.label36.Text = "T5";
			// 
			// label38
			// 
			this.label38.AutoSize = true;
			this.label38.Location = new System.Drawing.Point(225, 3);
			this.label38.Margin = new System.Windows.Forms.Padding(3);
			this.label38.Name = "label38";
			this.label38.Size = new System.Drawing.Size(20, 13);
			this.label38.TabIndex = 8;
			this.label38.Text = "T2";
			// 
			// label39
			// 
			this.label39.AutoSize = true;
			this.label39.Location = new System.Drawing.Point(225, 29);
			this.label39.Margin = new System.Windows.Forms.Padding(3);
			this.label39.Name = "label39";
			this.label39.Size = new System.Drawing.Size(29, 13);
			this.label39.TabIndex = 9;
			this.label39.Text = "T1.5";
			// 
			// label40
			// 
			this.label40.AutoSize = true;
			this.label40.Location = new System.Drawing.Point(225, 55);
			this.label40.Margin = new System.Windows.Forms.Padding(3);
			this.label40.Name = "label40";
			this.label40.Size = new System.Drawing.Size(20, 13);
			this.label40.TabIndex = 10;
			this.label40.Text = "T1";
			// 
			// label37
			// 
			this.label37.AutoSize = true;
			this.label37.Location = new System.Drawing.Point(154, 55);
			this.label37.Margin = new System.Windows.Forms.Padding(3);
			this.label37.Name = "label37";
			this.label37.Size = new System.Drawing.Size(29, 13);
			this.label37.TabIndex = 7;
			this.label37.Text = "T2.5";
			// 
			// label34
			// 
			this.label34.AutoSize = true;
			this.label34.Location = new System.Drawing.Point(154, 3);
			this.label34.Margin = new System.Windows.Forms.Padding(3);
			this.label34.Name = "label34";
			this.label34.Size = new System.Drawing.Size(29, 13);
			this.label34.TabIndex = 2;
			this.label34.Text = "T3.5";
			// 
			// label35
			// 
			this.label35.AutoSize = true;
			this.label35.Location = new System.Drawing.Point(154, 29);
			this.label35.Margin = new System.Windows.Forms.Padding(3);
			this.label35.Name = "label35";
			this.label35.Size = new System.Drawing.Size(20, 13);
			this.label35.TabIndex = 3;
			this.label35.Text = "T3";
			// 
			// label31
			// 
			this.label31.AutoSize = true;
			this.label31.Location = new System.Drawing.Point(3, 3);
			this.label31.Margin = new System.Windows.Forms.Padding(3);
			this.label31.Name = "label31";
			this.label31.Size = new System.Drawing.Size(38, 13);
			this.label31.TabIndex = 5;
			this.label31.Text = "Priority";
			// 
			// TPriorityValue
			// 
			this.TPriorityValue.Location = new System.Drawing.Point(47, 3);
			this.TPriorityValue.Name = "TPriorityValue";
			this.TPriorityValue.Size = new System.Drawing.Size(30, 20);
			this.TPriorityValue.TabIndex = 24;
			// 
			// T5Value
			// 
			this.T5Value.Location = new System.Drawing.Point(118, 3);
			this.T5Value.Name = "T5Value";
			this.T5Value.Size = new System.Drawing.Size(30, 20);
			this.T5Value.TabIndex = 25;
			// 
			// T35Value
			// 
			this.T35Value.Location = new System.Drawing.Point(189, 3);
			this.T35Value.Name = "T35Value";
			this.T35Value.Size = new System.Drawing.Size(30, 20);
			this.T35Value.TabIndex = 26;
			// 
			// T2Value
			// 
			this.T2Value.Location = new System.Drawing.Point(260, 3);
			this.T2Value.Name = "T2Value";
			this.T2Value.Size = new System.Drawing.Size(30, 20);
			this.T2Value.TabIndex = 27;
			// 
			// T45Value
			// 
			this.T45Value.Location = new System.Drawing.Point(118, 29);
			this.T45Value.Name = "T45Value";
			this.T45Value.Size = new System.Drawing.Size(30, 20);
			this.T45Value.TabIndex = 28;
			// 
			// T3Value
			// 
			this.T3Value.Location = new System.Drawing.Point(189, 29);
			this.T3Value.Name = "T3Value";
			this.T3Value.Size = new System.Drawing.Size(30, 20);
			this.T3Value.TabIndex = 29;
			// 
			// T15Value
			// 
			this.T15Value.Location = new System.Drawing.Point(260, 29);
			this.T15Value.Name = "T15Value";
			this.T15Value.Size = new System.Drawing.Size(30, 20);
			this.T15Value.TabIndex = 30;
			// 
			// T4Value
			// 
			this.T4Value.Location = new System.Drawing.Point(118, 55);
			this.T4Value.Name = "T4Value";
			this.T4Value.Size = new System.Drawing.Size(30, 20);
			this.T4Value.TabIndex = 31;
			// 
			// T25Value
			// 
			this.T25Value.Location = new System.Drawing.Point(189, 55);
			this.T25Value.Name = "T25Value";
			this.T25Value.Size = new System.Drawing.Size(30, 20);
			this.T25Value.TabIndex = 32;
			// 
			// T1Value
			// 
			this.T1Value.Location = new System.Drawing.Point(260, 55);
			this.T1Value.Name = "T1Value";
			this.T1Value.Size = new System.Drawing.Size(30, 20);
			this.T1Value.TabIndex = 33;
			// 
			// Other
			// 
			this.Other.Controls.Add(this.tableLayoutPanel6);
			this.Other.Dock = System.Windows.Forms.DockStyle.Fill;
			this.Other.Location = new System.Drawing.Point(3, 633);
			this.Other.Name = "Other";
			this.Other.Size = new System.Drawing.Size(363, 47);
			this.Other.TabIndex = 4;
			this.Other.TabStop = false;
			this.Other.Text = "Other";
			// 
			// tableLayoutPanel6
			// 
			this.tableLayoutPanel6.AutoSize = true;
			this.tableLayoutPanel6.ColumnCount = 2;
			this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 80F));
			this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
			this.tableLayoutPanel6.Controls.Add(this.label15, 0, 0);
			this.tableLayoutPanel6.Controls.Add(this.NMFlagValue, 1, 0);
			this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel6.Location = new System.Drawing.Point(3, 16);
			this.tableLayoutPanel6.Name = "tableLayoutPanel6";
			this.tableLayoutPanel6.RowCount = 1;
			this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel6.Size = new System.Drawing.Size(357, 28);
			this.tableLayoutPanel6.TabIndex = 0;
			// 
			// label15
			// 
			this.label15.AutoSize = true;
			this.label15.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label15.Location = new System.Drawing.Point(3, 3);
			this.label15.Margin = new System.Windows.Forms.Padding(3);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(279, 26);
			this.label15.TabIndex = 0;
			this.label15.Text = "Points substracted for a Needs maintenance attribute\r\nIf you are unsure, set it t" +
    "o 100";
			// 
			// NMFlagValue
			// 
			this.NMFlagValue.Dock = System.Windows.Forms.DockStyle.Left;
			this.NMFlagValue.Location = new System.Drawing.Point(288, 3);
			this.NMFlagValue.Name = "NMFlagValue";
			this.NMFlagValue.Size = new System.Drawing.Size(47, 20);
			this.NMFlagValue.TabIndex = 1;
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.ColumnCount = 2;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.Controls.Add(this.label55, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.EditRatingprofileCombobox, 1, 0);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 113);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 1;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.Size = new System.Drawing.Size(363, 24);
			this.tableLayoutPanel2.TabIndex = 7;
			// 
			// label55
			// 
			this.label55.AutoSize = true;
			this.label55.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label55.Location = new System.Drawing.Point(3, 3);
			this.label55.Margin = new System.Windows.Forms.Padding(3);
			this.label55.Name = "label55";
			this.label55.Size = new System.Drawing.Size(125, 18);
			this.label55.TabIndex = 0;
			this.label55.Text = "Select base Ratingprofile";
			// 
			// EditRatingprofileCombobox
			// 
			this.EditRatingprofileCombobox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.EditRatingprofileCombobox.FormattingEnabled = true;
			this.EditRatingprofileCombobox.Location = new System.Drawing.Point(134, 3);
			this.EditRatingprofileCombobox.Name = "EditRatingprofileCombobox";
			this.EditRatingprofileCombobox.Size = new System.Drawing.Size(226, 21);
			this.EditRatingprofileCombobox.TabIndex = 1;
			this.EditRatingprofileCombobox.SelectedIndexChanged += new System.EventHandler(this.Dropdown_SelectedIndexChanged);
			// 
			// SaveRatingprofileLayoutPanel
			// 
			this.SaveRatingprofileLayoutPanel.AutoSize = true;
			this.SaveRatingprofileLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.SaveRatingprofileLayoutPanel.ColumnCount = 4;
			this.SaveRatingprofileLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
			this.SaveRatingprofileLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
			this.SaveRatingprofileLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
			this.SaveRatingprofileLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
			this.SaveRatingprofileLayoutPanel.Controls.Add(this.DeleteRatingprofileButton, 3, 0);
			this.SaveRatingprofileLayoutPanel.Controls.Add(this.CreateRatingprofileButton, 2, 0);
			this.SaveRatingprofileLayoutPanel.Controls.Add(this.label54, 0, 0);
			this.SaveRatingprofileLayoutPanel.Controls.Add(this.RatingProfileName, 1, 0);
			this.SaveRatingprofileLayoutPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.SaveRatingprofileLayoutPanel.Location = new System.Drawing.Point(3, 531);
			this.SaveRatingprofileLayoutPanel.Name = "SaveRatingprofileLayoutPanel";
			this.SaveRatingprofileLayoutPanel.RowCount = 1;
			this.SaveRatingprofileLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
			this.SaveRatingprofileLayoutPanel.Size = new System.Drawing.Size(386, 30);
			this.SaveRatingprofileLayoutPanel.TabIndex = 9;
			// 
			// DeleteRatingprofileButton
			// 
			this.DeleteRatingprofileButton.AutoSize = true;
			this.DeleteRatingprofileButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.DeleteRatingprofileButton.Dock = System.Windows.Forms.DockStyle.Fill;
			this.DeleteRatingprofileButton.Location = new System.Drawing.Point(311, 3);
			this.DeleteRatingprofileButton.MinimumSize = new System.Drawing.Size(50, 20);
			this.DeleteRatingprofileButton.Name = "DeleteRatingprofileButton";
			this.DeleteRatingprofileButton.Size = new System.Drawing.Size(72, 24);
			this.DeleteRatingprofileButton.TabIndex = 0;
			this.DeleteRatingprofileButton.Text = "Delete";
			this.DeleteRatingprofileButton.UseVisualStyleBackColor = true;
			this.DeleteRatingprofileButton.Click += new System.EventHandler(this.DeleteRatingprofileButton_Click);
			// 
			// CreateRatingprofileButton
			// 
			this.CreateRatingprofileButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.CreateRatingprofileButton.Dock = System.Windows.Forms.DockStyle.Fill;
			this.CreateRatingprofileButton.Location = new System.Drawing.Point(234, 3);
			this.CreateRatingprofileButton.MinimumSize = new System.Drawing.Size(50, 20);
			this.CreateRatingprofileButton.Name = "CreateRatingprofileButton";
			this.CreateRatingprofileButton.Size = new System.Drawing.Size(71, 24);
			this.CreateRatingprofileButton.TabIndex = 1;
			this.CreateRatingprofileButton.Text = "Save profile";
			this.CreateRatingprofileButton.UseVisualStyleBackColor = true;
			this.CreateRatingprofileButton.Click += new System.EventHandler(this.CreateRatingprofile);
			// 
			// label54
			// 
			this.label54.AutoSize = true;
			this.label54.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label54.Location = new System.Drawing.Point(3, 3);
			this.label54.Margin = new System.Windows.Forms.Padding(3);
			this.label54.MinimumSize = new System.Drawing.Size(50, 20);
			this.label54.Name = "label54";
			this.label54.Size = new System.Drawing.Size(71, 24);
			this.label54.TabIndex = 2;
			this.label54.Text = "Save as";
			this.label54.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// RatingProfileName
			// 
			this.RatingProfileName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.RatingProfileName.Location = new System.Drawing.Point(80, 5);
			this.RatingProfileName.MinimumSize = new System.Drawing.Size(50, 20);
			this.RatingProfileName.Name = "RatingProfileName";
			this.RatingProfileName.Size = new System.Drawing.Size(148, 20);
			this.RatingProfileName.TabIndex = 3;
			// 
			// Routingprofiles
			// 
			this.Routingprofiles.Controls.Add(this.SaveRoutingProfileTableLayout);
			this.Routingprofiles.Controls.Add(this.RoutingprofilesSettingsTableLayout);
			this.Routingprofiles.Location = new System.Drawing.Point(4, 22);
			this.Routingprofiles.Name = "Routingprofiles";
			this.Routingprofiles.Padding = new System.Windows.Forms.Padding(3);
			this.Routingprofiles.Size = new System.Drawing.Size(392, 564);
			this.Routingprofiles.TabIndex = 1;
			this.Routingprofiles.Text = "Routingprofiles";
			this.Routingprofiles.UseVisualStyleBackColor = true;
			// 
			// SaveRoutingProfileTableLayout
			// 
			this.SaveRoutingProfileTableLayout.AutoSize = true;
			this.SaveRoutingProfileTableLayout.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.SaveRoutingProfileTableLayout.ColumnCount = 4;
			this.SaveRoutingProfileTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
			this.SaveRoutingProfileTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
			this.SaveRoutingProfileTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
			this.SaveRoutingProfileTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
			this.SaveRoutingProfileTableLayout.Controls.Add(this.DeleteRoutingprofileButton, 3, 0);
			this.SaveRoutingProfileTableLayout.Controls.Add(this.button2, 2, 0);
			this.SaveRoutingProfileTableLayout.Controls.Add(this.label52, 0, 0);
			this.SaveRoutingProfileTableLayout.Controls.Add(this.RoutingProfileName, 1, 0);
			this.SaveRoutingProfileTableLayout.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.SaveRoutingProfileTableLayout.Location = new System.Drawing.Point(3, 531);
			this.SaveRoutingProfileTableLayout.Name = "SaveRoutingProfileTableLayout";
			this.SaveRoutingProfileTableLayout.RowCount = 1;
			this.SaveRoutingProfileTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
			this.SaveRoutingProfileTableLayout.Size = new System.Drawing.Size(386, 30);
			this.SaveRoutingProfileTableLayout.TabIndex = 11;
			// 
			// DeleteRoutingprofileButton
			// 
			this.DeleteRoutingprofileButton.AutoSize = true;
			this.DeleteRoutingprofileButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.DeleteRoutingprofileButton.Dock = System.Windows.Forms.DockStyle.Fill;
			this.DeleteRoutingprofileButton.Location = new System.Drawing.Point(311, 3);
			this.DeleteRoutingprofileButton.MinimumSize = new System.Drawing.Size(50, 20);
			this.DeleteRoutingprofileButton.Name = "DeleteRoutingprofileButton";
			this.DeleteRoutingprofileButton.Size = new System.Drawing.Size(72, 24);
			this.DeleteRoutingprofileButton.TabIndex = 0;
			this.DeleteRoutingprofileButton.Text = "Delete";
			this.DeleteRoutingprofileButton.UseVisualStyleBackColor = true;
			this.DeleteRoutingprofileButton.Click += new System.EventHandler(this.DeleteRoutingprofileButton_Click);
			// 
			// button2
			// 
			this.button2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.button2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.button2.Location = new System.Drawing.Point(234, 3);
			this.button2.MinimumSize = new System.Drawing.Size(50, 20);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(71, 24);
			this.button2.TabIndex = 1;
			this.button2.Text = "Save profile";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.CreateRoutingprofile);
			// 
			// label52
			// 
			this.label52.AutoSize = true;
			this.label52.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label52.Location = new System.Drawing.Point(3, 3);
			this.label52.Margin = new System.Windows.Forms.Padding(3);
			this.label52.MinimumSize = new System.Drawing.Size(50, 20);
			this.label52.Name = "label52";
			this.label52.Size = new System.Drawing.Size(71, 24);
			this.label52.TabIndex = 2;
			this.label52.Text = "Save as";
			this.label52.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// RoutingProfileName
			// 
			this.RoutingProfileName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.RoutingProfileName.Location = new System.Drawing.Point(80, 5);
			this.RoutingProfileName.MinimumSize = new System.Drawing.Size(50, 20);
			this.RoutingProfileName.Name = "RoutingProfileName";
			this.RoutingProfileName.Size = new System.Drawing.Size(148, 20);
			this.RoutingProfileName.TabIndex = 3;
			// 
			// RoutingprofilesSettingsTableLayout
			// 
			this.RoutingprofilesSettingsTableLayout.AutoScroll = true;
			this.RoutingprofilesSettingsTableLayout.ColumnCount = 1;
			this.RoutingprofilesSettingsTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.RoutingprofilesSettingsTableLayout.Controls.Add(this.RoutingCoreGroupbox, 0, 2);
			this.RoutingprofilesSettingsTableLayout.Controls.Add(this.DistanceGroupBox, 0, 3);
			this.RoutingprofilesSettingsTableLayout.Controls.Add(this.TimeGroupBox, 0, 4);
			this.RoutingprofilesSettingsTableLayout.Controls.Add(this.label44, 0, 0);
			this.RoutingprofilesSettingsTableLayout.Controls.Add(this.tableLayoutPanel14, 0, 1);
			this.RoutingprofilesSettingsTableLayout.Dock = System.Windows.Forms.DockStyle.Top;
			this.RoutingprofilesSettingsTableLayout.Location = new System.Drawing.Point(3, 3);
			this.RoutingprofilesSettingsTableLayout.Name = "RoutingprofilesSettingsTableLayout";
			this.RoutingprofilesSettingsTableLayout.RowCount = 6;
			this.RoutingprofilesSettingsTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.RoutingprofilesSettingsTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
			this.RoutingprofilesSettingsTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.RoutingprofilesSettingsTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.RoutingprofilesSettingsTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.RoutingprofilesSettingsTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.RoutingprofilesSettingsTableLayout.Size = new System.Drawing.Size(386, 350);
			this.RoutingprofilesSettingsTableLayout.TabIndex = 0;
			// 
			// RoutingCoreGroupbox
			// 
			this.RoutingCoreGroupbox.Controls.Add(this.tableLayoutPanel11);
			this.RoutingCoreGroupbox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.RoutingCoreGroupbox.Location = new System.Drawing.Point(3, 88);
			this.RoutingCoreGroupbox.Name = "RoutingCoreGroupbox";
			this.RoutingCoreGroupbox.Size = new System.Drawing.Size(380, 50);
			this.RoutingCoreGroupbox.TabIndex = 1;
			this.RoutingCoreGroupbox.TabStop = false;
			this.RoutingCoreGroupbox.Text = "RoutingCore";
			// 
			// tableLayoutPanel11
			// 
			this.tableLayoutPanel11.ColumnCount = 4;
			this.tableLayoutPanel11.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel11.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel11.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel11.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel11.Controls.Add(this.label45, 0, 0);
			this.tableLayoutPanel11.Controls.Add(this.label46, 2, 0);
			this.tableLayoutPanel11.Controls.Add(this.VehicleCombobox, 1, 0);
			this.tableLayoutPanel11.Controls.Add(this.MetricCombobox, 3, 0);
			this.tableLayoutPanel11.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel11.Location = new System.Drawing.Point(3, 16);
			this.tableLayoutPanel11.Name = "tableLayoutPanel11";
			this.tableLayoutPanel11.RowCount = 1;
			this.tableLayoutPanel11.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel11.Size = new System.Drawing.Size(374, 31);
			this.tableLayoutPanel11.TabIndex = 0;
			// 
			// label45
			// 
			this.label45.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label45.AutoSize = true;
			this.label45.Location = new System.Drawing.Point(3, 9);
			this.label45.Margin = new System.Windows.Forms.Padding(3);
			this.label45.Name = "label45";
			this.label45.Size = new System.Drawing.Size(42, 13);
			this.label45.TabIndex = 0;
			this.label45.Text = "Vehicle";
			// 
			// label46
			// 
			this.label46.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label46.AutoSize = true;
			this.label46.Location = new System.Drawing.Point(178, 9);
			this.label46.Margin = new System.Windows.Forms.Padding(3);
			this.label46.Name = "label46";
			this.label46.Size = new System.Drawing.Size(36, 13);
			this.label46.TabIndex = 1;
			this.label46.Text = "Metric";
			// 
			// VehicleCombobox
			// 
			this.VehicleCombobox.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.VehicleCombobox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
			this.VehicleCombobox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
			this.VehicleCombobox.FormattingEnabled = true;
			this.VehicleCombobox.Items.AddRange(new object[] {
            "Car",
            "Pedestrian",
            "Bicycle"});
			this.VehicleCombobox.Location = new System.Drawing.Point(51, 5);
			this.VehicleCombobox.Name = "VehicleCombobox";
			this.VehicleCombobox.Size = new System.Drawing.Size(121, 21);
			this.VehicleCombobox.TabIndex = 2;
			this.VehicleCombobox.SelectedIndexChanged += new System.EventHandler(this.Dropdown_SelectedIndexChanged);
			// 
			// MetricCombobox
			// 
			this.MetricCombobox.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.MetricCombobox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
			this.MetricCombobox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
			this.MetricCombobox.FormattingEnabled = true;
			this.MetricCombobox.Items.AddRange(new object[] {
            "Fastest",
            "Shortest"});
			this.MetricCombobox.Location = new System.Drawing.Point(220, 5);
			this.MetricCombobox.Name = "MetricCombobox";
			this.MetricCombobox.Size = new System.Drawing.Size(145, 21);
			this.MetricCombobox.TabIndex = 3;
			this.MetricCombobox.SelectedIndexChanged += new System.EventHandler(this.Dropdown_SelectedIndexChanged);
			// 
			// DistanceGroupBox
			// 
			this.DistanceGroupBox.Controls.Add(this.tableLayoutPanel12);
			this.DistanceGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.DistanceGroupBox.Location = new System.Drawing.Point(3, 144);
			this.DistanceGroupBox.Name = "DistanceGroupBox";
			this.DistanceGroupBox.Size = new System.Drawing.Size(380, 60);
			this.DistanceGroupBox.TabIndex = 2;
			this.DistanceGroupBox.TabStop = false;
			this.DistanceGroupBox.Text = "Distance";
			// 
			// tableLayoutPanel12
			// 
			this.tableLayoutPanel12.ColumnCount = 4;
			this.tableLayoutPanel12.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tableLayoutPanel12.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tableLayoutPanel12.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tableLayoutPanel12.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tableLayoutPanel12.Controls.Add(this.MaxDistance, 1, 0);
			this.tableLayoutPanel12.Controls.Add(this.PenaltyPerExtraKm, 3, 0);
			this.tableLayoutPanel12.Controls.Add(this.label47, 0, 0);
			this.tableLayoutPanel12.Controls.Add(this.label48, 2, 0);
			this.tableLayoutPanel12.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel12.Location = new System.Drawing.Point(3, 16);
			this.tableLayoutPanel12.Name = "tableLayoutPanel12";
			this.tableLayoutPanel12.RowCount = 1;
			this.tableLayoutPanel12.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel12.Size = new System.Drawing.Size(374, 41);
			this.tableLayoutPanel12.TabIndex = 0;
			// 
			// MaxDistance
			// 
			this.MaxDistance.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.MaxDistance.Location = new System.Drawing.Point(96, 10);
			this.MaxDistance.Name = "MaxDistance";
			this.MaxDistance.Size = new System.Drawing.Size(86, 20);
			this.MaxDistance.TabIndex = 0;
			// 
			// PenaltyPerExtraKm
			// 
			this.PenaltyPerExtraKm.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.PenaltyPerExtraKm.Location = new System.Drawing.Point(282, 10);
			this.PenaltyPerExtraKm.Name = "PenaltyPerExtraKm";
			this.PenaltyPerExtraKm.Size = new System.Drawing.Size(86, 20);
			this.PenaltyPerExtraKm.TabIndex = 1;
			// 
			// label47
			// 
			this.label47.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label47.AutoSize = true;
			this.label47.Location = new System.Drawing.Point(3, 7);
			this.label47.Margin = new System.Windows.Forms.Padding(3);
			this.label47.Name = "label47";
			this.label47.Size = new System.Drawing.Size(86, 26);
			this.label47.TabIndex = 2;
			this.label47.Text = "Max Distance in km";
			// 
			// label48
			// 
			this.label48.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label48.AutoSize = true;
			this.label48.Location = new System.Drawing.Point(189, 7);
			this.label48.Margin = new System.Windows.Forms.Padding(3);
			this.label48.Name = "label48";
			this.label48.Size = new System.Drawing.Size(86, 26);
			this.label48.TabIndex = 3;
			this.label48.Text = "Penalty per extra km";
			// 
			// TimeGroupBox
			// 
			this.TimeGroupBox.Controls.Add(this.tableLayoutPanel13);
			this.TimeGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TimeGroupBox.Location = new System.Drawing.Point(3, 210);
			this.TimeGroupBox.Name = "TimeGroupBox";
			this.TimeGroupBox.Size = new System.Drawing.Size(380, 100);
			this.TimeGroupBox.TabIndex = 3;
			this.TimeGroupBox.TabStop = false;
			this.TimeGroupBox.Text = "Time";
			// 
			// tableLayoutPanel13
			// 
			this.tableLayoutPanel13.ColumnCount = 4;
			this.tableLayoutPanel13.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tableLayoutPanel13.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tableLayoutPanel13.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tableLayoutPanel13.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tableLayoutPanel13.Controls.Add(this.label49, 0, 0);
			this.tableLayoutPanel13.Controls.Add(this.label50, 2, 0);
			this.tableLayoutPanel13.Controls.Add(this.label51, 0, 1);
			this.tableLayoutPanel13.Controls.Add(this.MaxTime, 1, 0);
			this.tableLayoutPanel13.Controls.Add(this.TimePerGeocache, 1, 1);
			this.tableLayoutPanel13.Controls.Add(this.PenaltyPerExtra10min, 3, 0);
			this.tableLayoutPanel13.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel13.Location = new System.Drawing.Point(3, 16);
			this.tableLayoutPanel13.Name = "tableLayoutPanel13";
			this.tableLayoutPanel13.RowCount = 2;
			this.tableLayoutPanel13.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel13.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel13.Size = new System.Drawing.Size(374, 81);
			this.tableLayoutPanel13.TabIndex = 0;
			// 
			// label49
			// 
			this.label49.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label49.AutoSize = true;
			this.label49.Location = new System.Drawing.Point(3, 13);
			this.label49.Name = "label49";
			this.label49.Size = new System.Drawing.Size(79, 13);
			this.label49.TabIndex = 0;
			this.label49.Text = "Max time in min";
			// 
			// label50
			// 
			this.label50.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label50.AutoSize = true;
			this.label50.Location = new System.Drawing.Point(189, 7);
			this.label50.Margin = new System.Windows.Forms.Padding(3);
			this.label50.Name = "label50";
			this.label50.Size = new System.Drawing.Size(86, 26);
			this.label50.TabIndex = 1;
			this.label50.Text = "Penalty per extra 10 min";
			// 
			// label51
			// 
			this.label51.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label51.AutoSize = true;
			this.label51.Location = new System.Drawing.Point(3, 47);
			this.label51.Margin = new System.Windows.Forms.Padding(3);
			this.label51.Name = "label51";
			this.label51.Size = new System.Drawing.Size(87, 26);
			this.label51.TabIndex = 2;
			this.label51.Text = "Time needed per Geocache";
			// 
			// MaxTime
			// 
			this.MaxTime.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.MaxTime.Location = new System.Drawing.Point(96, 10);
			this.MaxTime.Name = "MaxTime";
			this.MaxTime.Size = new System.Drawing.Size(86, 20);
			this.MaxTime.TabIndex = 3;
			// 
			// TimePerGeocache
			// 
			this.TimePerGeocache.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.TimePerGeocache.Location = new System.Drawing.Point(96, 50);
			this.TimePerGeocache.Name = "TimePerGeocache";
			this.TimePerGeocache.Size = new System.Drawing.Size(86, 20);
			this.TimePerGeocache.TabIndex = 4;
			// 
			// PenaltyPerExtra10min
			// 
			this.PenaltyPerExtra10min.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.PenaltyPerExtra10min.Location = new System.Drawing.Point(282, 10);
			this.PenaltyPerExtra10min.Name = "PenaltyPerExtra10min";
			this.PenaltyPerExtra10min.Size = new System.Drawing.Size(86, 20);
			this.PenaltyPerExtra10min.TabIndex = 5;
			// 
			// label44
			// 
			this.label44.AutoSize = true;
			this.label44.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label44.Location = new System.Drawing.Point(3, 3);
			this.label44.Margin = new System.Windows.Forms.Padding(3);
			this.label44.Name = "label44";
			this.label44.Size = new System.Drawing.Size(380, 39);
			this.label44.TabIndex = 4;
			this.label44.Text = "The Penalty per extra km should be chosen in a way, that it equals the amount of " +
    "points a Geocache must have that you\'d go the extra km. Same goes for the penalt" +
    "y for extra 10 min.";
			// 
			// tableLayoutPanel14
			// 
			this.tableLayoutPanel14.ColumnCount = 2;
			this.tableLayoutPanel14.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel14.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel14.Controls.Add(this.label53, 0, 0);
			this.tableLayoutPanel14.Controls.Add(this.EditRoutingprofileCombobox, 1, 0);
			this.tableLayoutPanel14.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel14.Location = new System.Drawing.Point(3, 48);
			this.tableLayoutPanel14.Name = "tableLayoutPanel14";
			this.tableLayoutPanel14.RowCount = 1;
			this.tableLayoutPanel14.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel14.Size = new System.Drawing.Size(380, 34);
			this.tableLayoutPanel14.TabIndex = 11;
			// 
			// label53
			// 
			this.label53.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label53.AutoSize = true;
			this.label53.Location = new System.Drawing.Point(3, 10);
			this.label53.Margin = new System.Windows.Forms.Padding(3);
			this.label53.Name = "label53";
			this.label53.Size = new System.Drawing.Size(131, 13);
			this.label53.TabIndex = 0;
			this.label53.Text = "Select base Routingprofile";
			// 
			// EditRoutingprofileCombobox
			// 
			this.EditRoutingprofileCombobox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.EditRoutingprofileCombobox.FormattingEnabled = true;
			this.EditRoutingprofileCombobox.Location = new System.Drawing.Point(193, 3);
			this.EditRoutingprofileCombobox.Name = "EditRoutingprofileCombobox";
			this.EditRoutingprofileCombobox.Size = new System.Drawing.Size(184, 21);
			this.EditRoutingprofileCombobox.TabIndex = 1;
			this.EditRoutingprofileCombobox.SelectedIndexChanged += new System.EventHandler(this.Dropdown_SelectedIndexChanged);
			// 
			// Settings
			// 
			this.Settings.Controls.Add(this.SettingsTableLayoutPanel);
			this.Settings.Location = new System.Drawing.Point(4, 22);
			this.Settings.Name = "Settings";
			this.Settings.Size = new System.Drawing.Size(392, 564);
			this.Settings.TabIndex = 3;
			this.Settings.Text = "Settings";
			this.Settings.UseVisualStyleBackColor = true;
			// 
			// SettingsTableLayoutPanel
			// 
			this.SettingsTableLayoutPanel.AutoSize = true;
			this.SettingsTableLayoutPanel.ColumnCount = 1;
			this.SettingsTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.SettingsTableLayoutPanel.Controls.Add(this.RoutingsettingsGroupbox, 0, 0);
			this.SettingsTableLayoutPanel.Controls.Add(this.Display_groupBox, 0, 1);
			this.SettingsTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.SettingsTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
			this.SettingsTableLayoutPanel.Name = "SettingsTableLayoutPanel";
			this.SettingsTableLayoutPanel.RowCount = 2;
			this.SettingsTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.SettingsTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.SettingsTableLayoutPanel.Size = new System.Drawing.Size(392, 564);
			this.SettingsTableLayoutPanel.TabIndex = 0;
			// 
			// RoutingsettingsGroupbox
			// 
			this.RoutingsettingsGroupbox.AutoSize = true;
			this.RoutingsettingsGroupbox.Controls.Add(this.RoutingSettingsTableLayoutPanel);
			this.RoutingsettingsGroupbox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.RoutingsettingsGroupbox.Location = new System.Drawing.Point(3, 3);
			this.RoutingsettingsGroupbox.MinimumSize = new System.Drawing.Size(0, 270);
			this.RoutingsettingsGroupbox.Name = "RoutingsettingsGroupbox";
			this.RoutingsettingsGroupbox.Size = new System.Drawing.Size(386, 270);
			this.RoutingsettingsGroupbox.TabIndex = 0;
			this.RoutingsettingsGroupbox.TabStop = false;
			this.RoutingsettingsGroupbox.Text = "Routing settings";
			// 
			// RoutingSettingsTableLayoutPanel
			// 
			this.RoutingSettingsTableLayoutPanel.AutoSize = true;
			this.RoutingSettingsTableLayoutPanel.ColumnCount = 2;
			this.RoutingSettingsTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 80F));
			this.RoutingSettingsTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
			this.RoutingSettingsTableLayoutPanel.Controls.Add(this.label59, 0, 2);
			this.RoutingSettingsTableLayoutPanel.Controls.Add(this.RoutefindingWidth_Textbox, 1, 2);
			this.RoutingSettingsTableLayoutPanel.Controls.Add(this.Autotargetselection, 0, 0);
			this.RoutingSettingsTableLayoutPanel.Controls.Add(this.LiveDisplayRouteCalculationCheckbox, 0, 1);
			this.RoutingSettingsTableLayoutPanel.Controls.Add(this.AutotargetSelectionMaxDistanceLabel, 0, 5);
			this.RoutingSettingsTableLayoutPanel.Controls.Add(this.AutotargetselectionMinLabel, 0, 6);
			this.RoutingSettingsTableLayoutPanel.Controls.Add(this.AutotargetselectionMaxTextBox, 1, 5);
			this.RoutingSettingsTableLayoutPanel.Controls.Add(this.AutotargetselectionMinTextBox, 1, 6);
			this.RoutingSettingsTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.RoutingSettingsTableLayoutPanel.Location = new System.Drawing.Point(3, 16);
			this.RoutingSettingsTableLayoutPanel.Name = "RoutingSettingsTableLayoutPanel";
			this.RoutingSettingsTableLayoutPanel.RowCount = 5;
			this.RoutingSettingsTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.RoutingSettingsTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.RoutingSettingsTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.RoutingSettingsTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.RoutingSettingsTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.RoutingSettingsTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.RoutingSettingsTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.RoutingSettingsTableLayoutPanel.Size = new System.Drawing.Size(380, 251);
			this.RoutingSettingsTableLayoutPanel.TabIndex = 0;
			// 
			// label59
			// 
			this.label59.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label59.AutoSize = true;
			this.label59.Location = new System.Drawing.Point(3, 49);
			this.label59.Margin = new System.Windows.Forms.Padding(3);
			this.label59.Name = "label59";
			this.label59.Size = new System.Drawing.Size(254, 26);
			this.label59.TabIndex = 7;
			this.label59.Text = "Number of potential target caches considered when Autotargetselection is enabled " +
    "(Default: 4)";
			// 
			// RoutefindingWidth_Textbox
			// 
			this.RoutefindingWidth_Textbox.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.RoutefindingWidth_Textbox.Location = new System.Drawing.Point(307, 52);
			this.RoutefindingWidth_Textbox.Name = "RoutefindingWidth_Textbox";
			this.RoutefindingWidth_Textbox.Size = new System.Drawing.Size(69, 20);
			this.RoutefindingWidth_Textbox.TabIndex = 8;
			this.RoutefindingWidth_Textbox.TextChanged += new System.EventHandler(this.RoutefindingWidth_Textbox_TextChanged);
			// 
			// Autotargetselection
			// 
			this.Autotargetselection.AutoSize = true;
			this.Autotargetselection.Checked = true;
			this.Autotargetselection.CheckState = System.Windows.Forms.CheckState.Checked;
			this.RoutingSettingsTableLayoutPanel.SetColumnSpan(this.Autotargetselection, 2);
			this.Autotargetselection.Dock = System.Windows.Forms.DockStyle.Fill;
			this.Autotargetselection.Location = new System.Drawing.Point(3, 3);
			this.Autotargetselection.Name = "Autotargetselection";
			this.Autotargetselection.Size = new System.Drawing.Size(374, 17);
			this.Autotargetselection.TabIndex = 10;
			this.Autotargetselection.Text = "Autotargetselection (Default: on)";
			this.Autotargetselection.UseVisualStyleBackColor = true;
			this.Autotargetselection.CheckedChanged += new System.EventHandler(this.Autotargetselection_CheckedChanged);
			// 
			// LiveDisplayRouteCalculationCheckbox
			// 
			this.LiveDisplayRouteCalculationCheckbox.AutoSize = true;
			this.RoutingSettingsTableLayoutPanel.SetColumnSpan(this.LiveDisplayRouteCalculationCheckbox, 2);
			this.LiveDisplayRouteCalculationCheckbox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.LiveDisplayRouteCalculationCheckbox.Location = new System.Drawing.Point(3, 26);
			this.LiveDisplayRouteCalculationCheckbox.Name = "LiveDisplayRouteCalculationCheckbox";
			this.LiveDisplayRouteCalculationCheckbox.Size = new System.Drawing.Size(374, 17);
			this.LiveDisplayRouteCalculationCheckbox.TabIndex = 9;
			this.LiveDisplayRouteCalculationCheckbox.Text = "Live Display of calculated Route";
			this.LiveDisplayRouteCalculationCheckbox.UseVisualStyleBackColor = true;
			this.LiveDisplayRouteCalculationCheckbox.CheckedChanged += new System.EventHandler(this.LiveDisplayRouteCalculationCheckbox_CheckedChanged);
			// 
			// AutotargetSelectionMaxDistanceLabel
			// 
			this.AutotargetSelectionMaxDistanceLabel.AutoSize = true;
			this.AutotargetSelectionMaxDistanceLabel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.AutotargetSelectionMaxDistanceLabel.Location = new System.Drawing.Point(3, 81);
			this.AutotargetSelectionMaxDistanceLabel.Margin = new System.Windows.Forms.Padding(3);
			this.AutotargetSelectionMaxDistanceLabel.Name = "AutotargetSelectionMaxDistanceLabel";
			this.AutotargetSelectionMaxDistanceLabel.Size = new System.Drawing.Size(298, 26);
			this.AutotargetSelectionMaxDistanceLabel.TabIndex = 11;
			this.AutotargetSelectionMaxDistanceLabel.Text = "What percentage of the total distance should be used up *at most* in Autotargetse" +
    "lection (Default 90)";
			// 
			// AutotargetselectionMinLabel
			// 
			this.AutotargetselectionMinLabel.AutoSize = true;
			this.AutotargetselectionMinLabel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.AutotargetselectionMinLabel.Location = new System.Drawing.Point(3, 113);
			this.AutotargetselectionMinLabel.Margin = new System.Windows.Forms.Padding(3);
			this.AutotargetselectionMinLabel.Name = "AutotargetselectionMinLabel";
			this.AutotargetselectionMinLabel.Size = new System.Drawing.Size(298, 135);
			this.AutotargetselectionMinLabel.TabIndex = 12;
			this.AutotargetselectionMinLabel.Text = "What percentage of the total distance and time should be used up *at least* in Au" +
    "totargetselection? (Default 75)";
			// 
			// AutotargetselectionMaxTextBox
			// 
			this.AutotargetselectionMaxTextBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.AutotargetselectionMaxTextBox.Location = new System.Drawing.Point(307, 84);
			this.AutotargetselectionMaxTextBox.Name = "AutotargetselectionMaxTextBox";
			this.AutotargetselectionMaxTextBox.Size = new System.Drawing.Size(70, 20);
			this.AutotargetselectionMaxTextBox.TabIndex = 13;
			this.AutotargetselectionMaxTextBox.TextChanged += new System.EventHandler(this.AutotargetselectionMaxTextbox_TextChanged);
			this.AutotargetselectionMaxTextBox.Leave += new System.EventHandler(this.AutotargetselectionMaxTextBox_Leave);
			// 
			// AutotargetselectionMinTextBox
			// 
			this.AutotargetselectionMinTextBox.Location = new System.Drawing.Point(307, 113);
			this.AutotargetselectionMinTextBox.Name = "AutotargetselectionMinTextBox";
			this.AutotargetselectionMinTextBox.Size = new System.Drawing.Size(70, 20);
			this.AutotargetselectionMinTextBox.TabIndex = 14;
			this.AutotargetselectionMinTextBox.TextChanged += new System.EventHandler(this.AutotargetselectionMinTextBox_TextChanged);
			this.AutotargetselectionMinTextBox.Leave += new System.EventHandler(this.AutotargetselectionMinTextBox_Leave);
			// 
			// Display_groupBox
			// 
			this.Display_groupBox.AutoSize = true;
			this.Display_groupBox.Controls.Add(this.DisplaySettingsTableLayoutSettings);
			this.Display_groupBox.Dock = System.Windows.Forms.DockStyle.Top;
			this.Display_groupBox.Location = new System.Drawing.Point(3, 279);
			this.Display_groupBox.Name = "Display_groupBox";
			this.Display_groupBox.Size = new System.Drawing.Size(386, 89);
			this.Display_groupBox.TabIndex = 1;
			this.Display_groupBox.TabStop = false;
			this.Display_groupBox.Text = "Display";
			// 
			// DisplaySettingsTableLayoutSettings
			// 
			this.DisplaySettingsTableLayoutSettings.AutoSize = true;
			this.DisplaySettingsTableLayoutSettings.ColumnCount = 2;
			this.DisplaySettingsTableLayoutSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.DisplaySettingsTableLayoutSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.DisplaySettingsTableLayoutSettings.Controls.Add(this.IconSizeLabel, 0, 0);
			this.DisplaySettingsTableLayoutSettings.Controls.Add(this.MarkerSizeTrackBar, 0, 1);
			this.DisplaySettingsTableLayoutSettings.Dock = System.Windows.Forms.DockStyle.Fill;
			this.DisplaySettingsTableLayoutSettings.Location = new System.Drawing.Point(3, 16);
			this.DisplaySettingsTableLayoutSettings.Name = "DisplaySettingsTableLayoutSettings";
			this.DisplaySettingsTableLayoutSettings.RowCount = 2;
			this.DisplaySettingsTableLayoutSettings.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.DisplaySettingsTableLayoutSettings.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.DisplaySettingsTableLayoutSettings.Size = new System.Drawing.Size(380, 70);
			this.DisplaySettingsTableLayoutSettings.TabIndex = 0;
			// 
			// IconSizeLabel
			// 
			this.IconSizeLabel.AutoSize = true;
			this.DisplaySettingsTableLayoutSettings.SetColumnSpan(this.IconSizeLabel, 2);
			this.IconSizeLabel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.IconSizeLabel.Location = new System.Drawing.Point(3, 3);
			this.IconSizeLabel.Margin = new System.Windows.Forms.Padding(3);
			this.IconSizeLabel.Name = "IconSizeLabel";
			this.IconSizeLabel.Size = new System.Drawing.Size(374, 13);
			this.IconSizeLabel.TabIndex = 0;
			this.IconSizeLabel.Text = "Size of Icons on the map";
			// 
			// MarkerSizeTrackBar
			// 
			this.MarkerSizeTrackBar.BackColor = System.Drawing.SystemColors.Window;
			this.DisplaySettingsTableLayoutSettings.SetColumnSpan(this.MarkerSizeTrackBar, 2);
			this.MarkerSizeTrackBar.Dock = System.Windows.Forms.DockStyle.Top;
			this.MarkerSizeTrackBar.Location = new System.Drawing.Point(3, 22);
			this.MarkerSizeTrackBar.Maximum = 36;
			this.MarkerSizeTrackBar.Minimum = 12;
			this.MarkerSizeTrackBar.Name = "MarkerSizeTrackBar";
			this.MarkerSizeTrackBar.Size = new System.Drawing.Size(374, 45);
			this.MarkerSizeTrackBar.TabIndex = 1;
			this.MarkerSizeTrackBar.Value = 12;
			this.MarkerSizeTrackBar.Scroll += new System.EventHandler(this.MarkerSizeTrackBar_Scroll);
			// 
			// StatusbarTableLayoutPanel
			// 
			this.StatusbarTableLayoutPanel.ColumnCount = 3;
			this.UpmostTableLayoutPanel.SetColumnSpan(this.StatusbarTableLayoutPanel, 2);
			this.StatusbarTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.StatusbarTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.StatusbarTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 1182F));
			this.StatusbarTableLayoutPanel.Controls.Add(this.StatusLabel, 0, 0);
			this.StatusbarTableLayoutPanel.Controls.Add(this.ProgressBar, 1, 0);
			this.StatusbarTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.StatusbarTableLayoutPanel.Location = new System.Drawing.Point(0, 590);
			this.StatusbarTableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
			this.StatusbarTableLayoutPanel.Name = "StatusbarTableLayoutPanel";
			this.StatusbarTableLayoutPanel.RowCount = 1;
			this.StatusbarTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.StatusbarTableLayoutPanel.Size = new System.Drawing.Size(1388, 20);
			this.StatusbarTableLayoutPanel.TabIndex = 4;
			// 
			// StatusLabel
			// 
			this.StatusLabel.AutoSize = true;
			this.StatusLabel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.StatusLabel.Location = new System.Drawing.Point(3, 3);
			this.StatusLabel.Margin = new System.Windows.Forms.Padding(3);
			this.StatusLabel.Name = "StatusLabel";
			this.StatusLabel.Size = new System.Drawing.Size(94, 14);
			this.StatusLabel.TabIndex = 0;
			this.StatusLabel.Text = "Startup Completed";
			// 
			// ProgressBar
			// 
			this.ProgressBar.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ProgressBar.Location = new System.Drawing.Point(103, 3);
			this.ProgressBar.Name = "ProgressBar";
			this.ProgressBar.Size = new System.Drawing.Size(100, 14);
			this.ProgressBar.TabIndex = 1;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1388, 610);
			this.Controls.Add(this.UpmostTableLayoutPanel);
			this.Name = "Form1";
			this.Text = "GeocachingTourPlanner";
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
			this.UpmostTableLayoutPanel.ResumeLayout(false);
			this.Tabcontainer.ResumeLayout(false);
			this.MapTab.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.MapTab_SideMenu.ResumeLayout(false);
			this.MapTab_SideMenu.PerformLayout();
			this.tableLayoutPanel10.ResumeLayout(false);
			this.tableLayoutPanel10.PerformLayout();
			this.GeocachesTab.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.GeocacheTable)).EndInit();
			this.LeftTabs.ResumeLayout(false);
			this.Firststeps.ResumeLayout(false);
			this.tableLayoutPanel17.ResumeLayout(false);
			this.Overviewpage.ResumeLayout(false);
			this.NameStateTable.ResumeLayout(false);
			this.NameStateTable.PerformLayout();
			this.StateTableLayout.ResumeLayout(false);
			this.StateTableLayout.PerformLayout();
			this.Ratingprofiles.ResumeLayout(false);
			this.Ratingprofiles.PerformLayout();
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.RatingprofilesSettingsTabelLayout.ResumeLayout(false);
			this.RatingprofilesSettingsTabelLayout.PerformLayout();
			this.AgeGroupBox.ResumeLayout(false);
			this.AgeGroupBox.PerformLayout();
			this.tableLayoutPanel8.ResumeLayout(false);
			this.tableLayoutPanel8.PerformLayout();
			this.DValueungGroupBox.ResumeLayout(false);
			this.DValueungGroupBox.PerformLayout();
			this.tableLayoutPanel4.ResumeLayout(false);
			this.tableLayoutPanel4.PerformLayout();
			this.GeocachetypGroupBox.ResumeLayout(false);
			this.GeocachetypGroupBox.PerformLayout();
			this.tableLayoutPanel7.ResumeLayout(false);
			this.tableLayoutPanel7.PerformLayout();
			this.GeocachegrößeGroupBox.ResumeLayout(false);
			this.GeocachegrößeGroupBox.PerformLayout();
			this.tableLayoutPanel3.ResumeLayout(false);
			this.tableLayoutPanel3.PerformLayout();
			this.TValueungGroupbox.ResumeLayout(false);
			this.TValueungGroupbox.PerformLayout();
			this.tableLayoutPanel5.ResumeLayout(false);
			this.tableLayoutPanel5.PerformLayout();
			this.Other.ResumeLayout(false);
			this.Other.PerformLayout();
			this.tableLayoutPanel6.ResumeLayout(false);
			this.tableLayoutPanel6.PerformLayout();
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			this.SaveRatingprofileLayoutPanel.ResumeLayout(false);
			this.SaveRatingprofileLayoutPanel.PerformLayout();
			this.Routingprofiles.ResumeLayout(false);
			this.Routingprofiles.PerformLayout();
			this.SaveRoutingProfileTableLayout.ResumeLayout(false);
			this.SaveRoutingProfileTableLayout.PerformLayout();
			this.RoutingprofilesSettingsTableLayout.ResumeLayout(false);
			this.RoutingprofilesSettingsTableLayout.PerformLayout();
			this.RoutingCoreGroupbox.ResumeLayout(false);
			this.tableLayoutPanel11.ResumeLayout(false);
			this.tableLayoutPanel11.PerformLayout();
			this.DistanceGroupBox.ResumeLayout(false);
			this.tableLayoutPanel12.ResumeLayout(false);
			this.tableLayoutPanel12.PerformLayout();
			this.TimeGroupBox.ResumeLayout(false);
			this.tableLayoutPanel13.ResumeLayout(false);
			this.tableLayoutPanel13.PerformLayout();
			this.tableLayoutPanel14.ResumeLayout(false);
			this.tableLayoutPanel14.PerformLayout();
			this.Settings.ResumeLayout(false);
			this.Settings.PerformLayout();
			this.SettingsTableLayoutPanel.ResumeLayout(false);
			this.SettingsTableLayoutPanel.PerformLayout();
			this.RoutingsettingsGroupbox.ResumeLayout(false);
			this.RoutingsettingsGroupbox.PerformLayout();
			this.RoutingSettingsTableLayoutPanel.ResumeLayout(false);
			this.RoutingSettingsTableLayoutPanel.PerformLayout();
			this.Display_groupBox.ResumeLayout(false);
			this.Display_groupBox.PerformLayout();
			this.DisplaySettingsTableLayoutSettings.ResumeLayout(false);
			this.DisplaySettingsTableLayoutSettings.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.MarkerSizeTrackBar)).EndInit();
			this.StatusbarTableLayoutPanel.ResumeLayout(false);
			this.StatusbarTableLayoutPanel.PerformLayout();
			this.ResumeLayout(false);

        }

		#endregion

		private System.Windows.Forms.TableLayoutPanel UpmostTableLayoutPanel;
		private System.Windows.Forms.TabControl Tabcontainer;
		private System.Windows.Forms.TabPage MapTab;
		public System.Windows.Forms.TableLayoutPanel MapTab_SideMenu;
		private System.Windows.Forms.CheckBox MediumGeocachesCheckbox;
		private System.Windows.Forms.CheckBox BestGeocachesCheckbox;
		private System.Windows.Forms.CheckBox WorstGeocachesCheckbox;
		private System.Windows.Forms.TabPage GeocachesTab;
		public System.Windows.Forms.DataGridView GeocacheTable;
		private System.Windows.Forms.TabPage Overviewpage;
		private System.Windows.Forms.TableLayoutPanel NameStateTable;
		private System.Windows.Forms.TableLayoutPanel StateTableLayout;
		private System.Windows.Forms.LinkLabel GetPQLabel;
		private System.Windows.Forms.Label RatingprofilesStateLabel;
		private System.Windows.Forms.Label RoutingprofilesStateLabel;
		private System.Windows.Forms.LinkLabel GetPbfLabel;
		private System.Windows.Forms.Button SetGeocacheDBButton;
		private System.Windows.Forms.Button ImportPQButton;
		private System.Windows.Forms.Button SetRatingprofileDBButton;
		private System.Windows.Forms.Button SetRoutingprofileDBButton;
		private System.Windows.Forms.Button SetRouterDBButton;
		private System.Windows.Forms.Button ImportPbfButton;
		private System.Windows.Forms.Label NameLabel;
		private System.Windows.Forms.TabPage Ratingprofiles;
		private System.Windows.Forms.TabPage Routingprofiles;
		private System.Windows.Forms.TabPage Settings;
		private System.Windows.Forms.TableLayoutPanel SaveRatingprofileLayoutPanel;
		private System.Windows.Forms.Button DeleteRatingprofileButton;
		private System.Windows.Forms.Button CreateRatingprofileButton;
		private System.Windows.Forms.Label label54;
		private System.Windows.Forms.TextBox RatingProfileName;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		public GMap.NET.WindowsForms.GMapControl Map;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel10;
		private System.Windows.Forms.Button RateGeocachesButton;
		private System.Windows.Forms.Button CreateRouteButton;
		private System.Windows.Forms.ComboBox SelectedRoutingprofileCombobox;
		private System.Windows.Forms.ComboBox SelectedRatingprofileCombobox;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label41;
		private System.Windows.Forms.Label label42;
		private System.Windows.Forms.Label label43;
		private System.Windows.Forms.TextBox StartpointTextbox;
		private System.Windows.Forms.TextBox EndpointTextbox;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.TableLayoutPanel RatingprofilesSettingsTabelLayout;
		private System.Windows.Forms.GroupBox AgeGroupBox;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel8;
		private System.Windows.Forms.ComboBox AgeValue;
		private System.Windows.Forms.GroupBox DValueungGroupBox;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
		private System.Windows.Forms.Label label22;
		private System.Windows.Forms.Label label23;
		private System.Windows.Forms.Label label26;
		private System.Windows.Forms.Label label27;
		private System.Windows.Forms.Label label28;
		private System.Windows.Forms.Label label29;
		private System.Windows.Forms.Label label30;
		private System.Windows.Forms.Label label24;
		private System.Windows.Forms.Label label25;
		private System.Windows.Forms.Label label21;
		private System.Windows.Forms.GroupBox GeocachetypGroupBox;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel7;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.Label label14;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label19;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label RatingprofileInfoLabel;
		private System.Windows.Forms.GroupBox GeocachegrößeGroupBox;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.Label label16;
		private System.Windows.Forms.Label label18;
		private System.Windows.Forms.Label label17;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.GroupBox TValueungGroupbox;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
		private System.Windows.Forms.Label label32;
		private System.Windows.Forms.Label label33;
		private System.Windows.Forms.Label label36;
		private System.Windows.Forms.Label label38;
		private System.Windows.Forms.Label label39;
		private System.Windows.Forms.Label label40;
		private System.Windows.Forms.Label label37;
		private System.Windows.Forms.Label label34;
		private System.Windows.Forms.Label label35;
		private System.Windows.Forms.Label label31;
		private System.Windows.Forms.GroupBox Other;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
		private System.Windows.Forms.Label label15;
		private System.Windows.Forms.TextBox NMFlagValue;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private System.Windows.Forms.Label label55;
		private System.Windows.Forms.ComboBox EditRatingprofileCombobox;
		private System.Windows.Forms.TableLayoutPanel RoutingprofilesSettingsTableLayout;
		private System.Windows.Forms.GroupBox RoutingCoreGroupbox;
		private System.Windows.Forms.GroupBox DistanceGroupBox;
		private System.Windows.Forms.GroupBox TimeGroupBox;
		private System.Windows.Forms.Label label44;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel11;
		private System.Windows.Forms.Label label45;
		private System.Windows.Forms.Label label46;
		private System.Windows.Forms.ComboBox VehicleCombobox;
		private System.Windows.Forms.ComboBox MetricCombobox;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel12;
		private System.Windows.Forms.TextBox MaxDistance;
		private System.Windows.Forms.TextBox PenaltyPerExtraKm;
		private System.Windows.Forms.Label label47;
		private System.Windows.Forms.Label label48;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel13;
		private System.Windows.Forms.Label label49;
		private System.Windows.Forms.Label label50;
		private System.Windows.Forms.Label label51;
		private System.Windows.Forms.TextBox MaxTime;
		private System.Windows.Forms.TextBox TimePerGeocache;
		private System.Windows.Forms.TextBox PenaltyPerExtra10min;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel14;
		private System.Windows.Forms.Label label53;
		private System.Windows.Forms.ComboBox EditRoutingprofileCombobox;
		private System.Windows.Forms.TableLayoutPanel SaveRoutingProfileTableLayout;
		private System.Windows.Forms.Button DeleteRoutingprofileButton;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Label label52;
		public System.Windows.Forms.TextBox RoutingProfileName;
		private System.Windows.Forms.TableLayoutPanel SettingsTableLayoutPanel;
		private System.Windows.Forms.GroupBox RoutingsettingsGroupbox;
		private System.Windows.Forms.TableLayoutPanel RoutingSettingsTableLayoutPanel;
		private System.Windows.Forms.Label label59;
		private System.Windows.Forms.TextBox RoutefindingWidth_Textbox;
		private System.Windows.Forms.CheckBox LiveDisplayRouteCalculationCheckbox;
		private System.Windows.Forms.CheckBox Autotargetselection;
		private System.Windows.Forms.TabPage Firststeps;
		public System.Windows.Forms.TabControl LeftTabs;
		public System.Windows.Forms.Label GeocachesStateLabel;
		public System.Windows.Forms.Label RouterDBStateLabel;
		private System.Windows.Forms.Button NewRatingprofileDatbaseButton;
		private System.Windows.Forms.Button NewRoutingprofileDatabaseButton;

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel17;
		private System.Windows.Forms.WebBrowser webBrowser1;

		private System.Windows.Forms.TextBox AgeFactorValue;
		private System.Windows.Forms.TextBox DPriorityValue;
		private System.Windows.Forms.TextBox D5Value;
		private System.Windows.Forms.TextBox D35Value;
		private System.Windows.Forms.TextBox D2Value;
		private System.Windows.Forms.TextBox D45Value;
		private System.Windows.Forms.TextBox D4Value;
		private System.Windows.Forms.TextBox D3Value;
		private System.Windows.Forms.TextBox D15Value;
		private System.Windows.Forms.TextBox D25Value;
		private System.Windows.Forms.TextBox D1Value;
		private System.Windows.Forms.TextBox WebcamValue;
		private System.Windows.Forms.TextBox WherigoValue;
		private System.Windows.Forms.TextBox MysteryValue;
		private System.Windows.Forms.TextBox OtherTypeValue;
		private System.Windows.Forms.TextBox LetterboxValue;
		private System.Windows.Forms.TextBox MultiValue;
		private System.Windows.Forms.TextBox VirtualValue;
		private System.Windows.Forms.TextBox EarthcacheValue;
		private System.Windows.Forms.TextBox TypePriorityValue;
		private System.Windows.Forms.TextBox TraditionalValue;
		private System.Windows.Forms.TextBox RegularValue;
		private System.Windows.Forms.TextBox SizePriorityValue;
		private System.Windows.Forms.TextBox LargeValue;
		private System.Windows.Forms.TextBox SmallValue;
		private System.Windows.Forms.TextBox MicroValue;
		private System.Windows.Forms.TextBox OtherSizeValue;
		private System.Windows.Forms.TextBox TPriorityValue;
		private System.Windows.Forms.TextBox T5Value;
		private System.Windows.Forms.TextBox T35Value;
		private System.Windows.Forms.TextBox T2Value;
		private System.Windows.Forms.TextBox T45Value;
		private System.Windows.Forms.TextBox T3Value;
		private System.Windows.Forms.TextBox T15Value;
		private System.Windows.Forms.TextBox T4Value;
		private System.Windows.Forms.TextBox T25Value;
		private System.Windows.Forms.TextBox T1Value;
		private Button OpenWikiButton;
		private GroupBox Display_groupBox;
		private TableLayoutPanel DisplaySettingsTableLayoutSettings;
		private Label IconSizeLabel;
		private TrackBar MarkerSizeTrackBar;
		private TableLayoutPanel StatusbarTableLayoutPanel;
		private Label StatusLabel;
		private ProgressBar ProgressBar;
		private Label AutotargetSelectionMaxDistanceLabel;
		private Label AutotargetselectionMinLabel;
		private TextBox AutotargetselectionMaxTextBox;
		private TextBox AutotargetselectionMinTextBox;
	}
}


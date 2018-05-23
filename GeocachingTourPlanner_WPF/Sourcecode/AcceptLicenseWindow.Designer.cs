namespace GeocachingTourPlanner
{
	partial class AcceptLicenseWindow
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AcceptLicenseWindow));
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.Cancel_Button = new System.Windows.Forms.Button();
			this.Accept_Button = new System.Windows.Forms.Button();
			this.richTextBox1 = new System.Windows.Forms.RichTextBox();
			this.tableLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 3;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
			this.tableLayoutPanel1.Controls.Add(this.Cancel_Button, 2, 0);
			this.tableLayoutPanel1.Controls.Add(this.Accept_Button, 1, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 419);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 1;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(663, 25);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// Cancel_Button
			// 
			this.Cancel_Button.Dock = System.Windows.Forms.DockStyle.Fill;
			this.Cancel_Button.Location = new System.Drawing.Point(563, 0);
			this.Cancel_Button.Margin = new System.Windows.Forms.Padding(0);
			this.Cancel_Button.Name = "Cancel_Button";
			this.Cancel_Button.Size = new System.Drawing.Size(100, 25);
			this.Cancel_Button.TabIndex = 0;
			this.Cancel_Button.Text = "Cancel";
			this.Cancel_Button.UseVisualStyleBackColor = true;
			this.Cancel_Button.Click += new System.EventHandler(this.Cancel_Button_Click);
			// 
			// Accept_Button
			// 
			this.Accept_Button.Dock = System.Windows.Forms.DockStyle.Fill;
			this.Accept_Button.Location = new System.Drawing.Point(463, 0);
			this.Accept_Button.Margin = new System.Windows.Forms.Padding(0);
			this.Accept_Button.Name = "Accept_Button";
			this.Accept_Button.Size = new System.Drawing.Size(100, 25);
			this.Accept_Button.TabIndex = 1;
			this.Accept_Button.Text = "I Accept";
			this.Accept_Button.UseVisualStyleBackColor = true;
			this.Accept_Button.Click += new System.EventHandler(this.Accept_Button_Click);
			// 
			// richTextBox1
			// 
			this.richTextBox1.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.richTextBox1.Location = new System.Drawing.Point(0, 0);
			this.richTextBox1.Name = "richTextBox1";
			this.richTextBox1.ReadOnly = true;
			this.richTextBox1.Size = new System.Drawing.Size(663, 419);
			this.richTextBox1.TabIndex = 2;
			this.richTextBox1.Text = resources.GetString("richTextBox1.Text");
			// 
			// AcceptLicenseWindow
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(663, 444);
			this.Controls.Add(this.richTextBox1);
			this.Controls.Add(this.tableLayoutPanel1);
			this.Name = "AcceptLicenseWindow";
			this.Text = "License";
			this.tableLayoutPanel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Button Cancel_Button;
		private System.Windows.Forms.Button Accept_Button;
		private System.Windows.Forms.RichTextBox richTextBox1;
	}
}
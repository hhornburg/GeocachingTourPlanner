namespace GeocachingTourPlanner
{
	partial class DatabaseFileDialog
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
			this.CancelButton = new System.Windows.Forms.Button();
			this.New_ImportButton = new System.Windows.Forms.Button();
			this.SetButton = new System.Windows.Forms.Button();
			this.MessageText = new System.Windows.Forms.Label();
			this.tableLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 3;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.Controls.Add(this.CancelButton, 2, 1);
			this.tableLayoutPanel1.Controls.Add(this.New_ImportButton, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.SetButton, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.MessageText, 0, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 2;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(315, 118);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// CancelButton
			// 
			this.CancelButton.AutoSize = true;
			this.CancelButton.Location = new System.Drawing.Point(237, 91);
			this.CancelButton.Name = "CancelButton";
			this.CancelButton.Size = new System.Drawing.Size(75, 23);
			this.CancelButton.TabIndex = 0;
			this.CancelButton.Text = "Ignore";
			this.CancelButton.UseVisualStyleBackColor = true;
			this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
			// 
			// New_ImportButton
			// 
			this.New_ImportButton.AutoSize = true;
			this.New_ImportButton.Location = new System.Drawing.Point(156, 91);
			this.New_ImportButton.Name = "New_ImportButton";
			this.New_ImportButton.Size = new System.Drawing.Size(75, 23);
			this.New_ImportButton.TabIndex = 1;
			this.New_ImportButton.Text = "New File";
			this.New_ImportButton.UseVisualStyleBackColor = true;
			this.New_ImportButton.Click += new System.EventHandler(this.New_ImportButton_Click);
			// 
			// SetButton
			// 
			this.SetButton.AutoSize = true;
			this.SetButton.Dock = System.Windows.Forms.DockStyle.Right;
			this.SetButton.Location = new System.Drawing.Point(53, 91);
			this.SetButton.Name = "SetButton";
			this.SetButton.Size = new System.Drawing.Size(97, 24);
			this.SetButton.TabIndex = 2;
			this.SetButton.Text = "Open existing file";
			this.SetButton.UseVisualStyleBackColor = true;
			this.SetButton.Click += new System.EventHandler(this.OpenButton_Click);
			// 
			// MessageText
			// 
			this.MessageText.AutoSize = true;
			this.MessageText.BackColor = System.Drawing.SystemColors.Window;
			this.tableLayoutPanel1.SetColumnSpan(this.MessageText, 3);
			this.MessageText.Dock = System.Windows.Forms.DockStyle.Fill;
			this.MessageText.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.MessageText.Location = new System.Drawing.Point(0, 0);
			this.MessageText.Margin = new System.Windows.Forms.Padding(0);
			this.MessageText.Name = "MessageText";
			this.MessageText.Size = new System.Drawing.Size(315, 88);
			this.MessageText.TabIndex = 3;
			this.MessageText.Text = "Couldn\'t find a file for your Database";
			this.MessageText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// DatabaseFileDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Control;
			this.ClientSize = new System.Drawing.Size(315, 118);
			this.Controls.Add(this.tableLayoutPanel1);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "DatabaseFileDialog";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Warning";
			this.TopMost = true;
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Button CancelButton;
		private System.Windows.Forms.Button New_ImportButton;
		private System.Windows.Forms.Button SetButton;
		private System.Windows.Forms.Label MessageText;
	}
}
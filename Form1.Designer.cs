namespace PaladinIngester
{
    partial class Form1
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
            this.OutputPanel = new System.Windows.Forms.Panel();
            this.cam1 = new System.Windows.Forms.CheckBox();
            this.cam2 = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.openMediaFile = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // OutputPanel
            // 
            this.OutputPanel.Location = new System.Drawing.Point(236, 12);
            this.OutputPanel.Name = "OutputPanel";
            this.OutputPanel.Size = new System.Drawing.Size(617, 424);
            this.OutputPanel.TabIndex = 0;
            this.OutputPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OutputPanel_MouseDown);
            this.OutputPanel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OutputPanel_MouseUp);
            // 
            // cam1
            // 
            this.cam1.AutoSize = true;
            this.cam1.Location = new System.Drawing.Point(13, 13);
            this.cam1.Name = "cam1";
            this.cam1.Size = new System.Drawing.Size(86, 17);
            this.cam1.TabIndex = 1;
            this.cam1.Text = "Lenovo Cam";
            this.cam1.UseVisualStyleBackColor = true;
            this.cam1.CheckedChanged += new System.EventHandler(this.cam1_CheckedChanged);
            // 
            // cam2
            // 
            this.cam2.AutoSize = true;
            this.cam2.Location = new System.Drawing.Point(13, 37);
            this.cam2.Name = "cam2";
            this.cam2.Size = new System.Drawing.Size(67, 17);
            this.cam2.TabIndex = 2;
            this.cam2.Text = "Life Cam";
            this.cam2.UseVisualStyleBackColor = true;
            this.cam2.CheckedChanged += new System.EventHandler(this.cam2_CheckedChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(13, 61);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "Add Media";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // openMediaFile
            // 
            this.openMediaFile.FileName = "soul.gif";
            this.openMediaFile.InitialDirectory = "C:\\";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(865, 448);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.cam2);
            this.Controls.Add(this.cam1);
            this.Controls.Add(this.OutputPanel);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel OutputPanel;
        private System.Windows.Forms.CheckBox cam1;
        private System.Windows.Forms.CheckBox cam2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.OpenFileDialog openMediaFile;
    }
}


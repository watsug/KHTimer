namespace KHTimerApp
{
    partial class SetTimeForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SetTimeForm));
            this.textTime = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // textTime
            // 
            this.textTime.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.textTime.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 48F);
            this.textTime.ForeColor = System.Drawing.Color.Yellow;
            this.textTime.Location = new System.Drawing.Point(0, 0);
            this.textTime.Name = "textTime";
            this.textTime.Size = new System.Drawing.Size(238, 80);
            this.textTime.TabIndex = 0;
            this.textTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textTime.TextChanged += new System.EventHandler(this.textTime_TextChanged);
            this.textTime.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textTime_KeyDown);
            this.textTime.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textTime_KeyPress);
            // 
            // SetTimeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(238, 80);
            this.Controls.Add(this.textTime);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SetTimeForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "SetTimeForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textTime;
    }
}
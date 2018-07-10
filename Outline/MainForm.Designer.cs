namespace com.gscoder.gis.vector.outline
{
    partial class MainForm
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
			this.components = new System.ComponentModel.Container();
			this.pboxToolTip = new System.Windows.Forms.ToolTip(this.components);
			this.btnClear = new System.Windows.Forms.Button();
			this.btnOutline = new System.Windows.Forms.Button();
			this.curveBox = new com.gscoder.gis.vector.outline.CurveBox();
			this.SuspendLayout();
			// 
			// btnClear
			// 
			this.btnClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.btnClear.Location = new System.Drawing.Point(12, 409);
			this.btnClear.Name = "btnClear";
			this.btnClear.Size = new System.Drawing.Size(108, 23);
			this.btnClear.TabIndex = 1;
			this.btnClear.Text = "Clear";
			this.btnClear.UseVisualStyleBackColor = true;
			this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
			// 
			// btnOutline
			// 
			this.btnOutline.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOutline.Location = new System.Drawing.Point(587, 409);
			this.btnOutline.Name = "btnOutline";
			this.btnOutline.Size = new System.Drawing.Size(108, 23);
			this.btnOutline.TabIndex = 1;
			this.btnOutline.Text = "Outline";
			this.btnOutline.UseVisualStyleBackColor = true;
			this.btnOutline.Click += new System.EventHandler(this.btnOutline_Click);
			// 
			// curveBox
			// 
			this.curveBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.curveBox.BackColor = System.Drawing.Color.White;
			this.curveBox.Location = new System.Drawing.Point(12, 12);
			this.curveBox.Name = "curveBox";
			this.curveBox.Size = new System.Drawing.Size(683, 391);
			this.curveBox.TabIndex = 0;
			this.curveBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.drawAreaPictureBox_MouseDown);
			this.curveBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.drawAreaPictureBox_MouseMove);
			this.curveBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.drawAreaPictureBox_MouseUp);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(709, 444);
			this.Controls.Add(this.btnOutline);
			this.Controls.Add(this.btnClear);
			this.Controls.Add(this.curveBox);
			this.Name = "MainForm";
			this.Text = "MainForm";
			this.ResumeLayout(false);

        }

        #endregion

		private CurveBox curveBox;
		private System.Windows.Forms.ToolTip pboxToolTip;
		private System.Windows.Forms.Button btnClear;
		private System.Windows.Forms.Button btnOutline;
    }
}


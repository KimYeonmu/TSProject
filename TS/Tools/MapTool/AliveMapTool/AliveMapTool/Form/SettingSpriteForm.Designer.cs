using System.Drawing;

namespace AliveMapTool
{
    partial class SettingSpriteForm
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
            this.iTalk_ThemeContainer1 = new iTalk.iTalk_ThemeContainer();
            this.iTalk_Label3 = new iTalk.iTalk_Label();
            this.iTalk_Label2 = new iTalk.iTalk_Label();
            this.iTalk_Button_11 = new iTalk.iTalk_Button_1();
            this.AnimationTextBox_X = new iTalk.iTalk_TextBox_Small();
            this.iTalk_Label1 = new iTalk.iTalk_Label();
            this.AnimationTextBox_Y = new iTalk.iTalk_TextBox_Small();
            this.iTalk_ThemeContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // iTalk_ThemeContainer1
            // 
            this.iTalk_ThemeContainer1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(246)))), ((int)(((byte)(246)))));
            this.iTalk_ThemeContainer1.Controls.Add(this.iTalk_Label3);
            this.iTalk_ThemeContainer1.Controls.Add(this.iTalk_Label2);
            this.iTalk_ThemeContainer1.Controls.Add(this.iTalk_Button_11);
            this.iTalk_ThemeContainer1.Controls.Add(this.AnimationTextBox_X);
            this.iTalk_ThemeContainer1.Controls.Add(this.iTalk_Label1);
            this.iTalk_ThemeContainer1.Controls.Add(this.AnimationTextBox_Y);
            this.iTalk_ThemeContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.iTalk_ThemeContainer1.Font = new System.Drawing.Font("맑은 고딕", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.iTalk_ThemeContainer1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(142)))), ((int)(((byte)(142)))));
            this.iTalk_ThemeContainer1.Location = new System.Drawing.Point(0, 0);
            this.iTalk_ThemeContainer1.Name = "iTalk_ThemeContainer1";
            this.iTalk_ThemeContainer1.Padding = new System.Windows.Forms.Padding(3, 28, 3, 28);
            this.iTalk_ThemeContainer1.Sizable = true;
            this.iTalk_ThemeContainer1.Size = new System.Drawing.Size(360, 210);
            this.iTalk_ThemeContainer1.SmartBounds = false;
            this.iTalk_ThemeContainer1.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.iTalk_ThemeContainer1.TabIndex = 7;
            this.iTalk_ThemeContainer1.TabStop = false;
            this.iTalk_ThemeContainer1.Text = "애니메이션 설정";
            // 
            // iTalk_Label3
            // 
            this.iTalk_Label3.AutoSize = true;
            this.iTalk_Label3.BackColor = System.Drawing.Color.Transparent;
            this.iTalk_Label3.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.iTalk_Label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(142)))), ((int)(((byte)(142)))));
            this.iTalk_Label3.Location = new System.Drawing.Point(128, 77);
            this.iTalk_Label3.Name = "iTalk_Label3";
            this.iTalk_Label3.Size = new System.Drawing.Size(17, 19);
            this.iTalk_Label3.TabIndex = 5;
            this.iTalk_Label3.Text = "Y";
            // 
            // iTalk_Label2
            // 
            this.iTalk_Label2.AutoSize = true;
            this.iTalk_Label2.BackColor = System.Drawing.Color.Transparent;
            this.iTalk_Label2.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.iTalk_Label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(142)))), ((int)(((byte)(142)))));
            this.iTalk_Label2.Location = new System.Drawing.Point(14, 77);
            this.iTalk_Label2.Name = "iTalk_Label2";
            this.iTalk_Label2.Size = new System.Drawing.Size(17, 19);
            this.iTalk_Label2.TabIndex = 4;
            this.iTalk_Label2.Text = "X";
            // 
            // iTalk_Button_11
            // 
            this.iTalk_Button_11.BackColor = System.Drawing.Color.Transparent;
            this.iTalk_Button_11.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.iTalk_Button_11.Image = null;
            this.iTalk_Button_11.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.iTalk_Button_11.Location = new System.Drawing.Point(121, 138);
            this.iTalk_Button_11.Name = "iTalk_Button_11";
            this.iTalk_Button_11.Size = new System.Drawing.Size(120, 30);
            this.iTalk_Button_11.TabIndex = 3;
            this.iTalk_Button_11.Text = "적용";
            this.iTalk_Button_11.TextAlignment = System.Drawing.StringAlignment.Center;
            this.iTalk_Button_11.Click += new System.EventHandler(this.iTalk_Button_11_Click);
            // 
            // AnimationTextBox_X
            // 
            this.AnimationTextBox_X.BackColor = System.Drawing.Color.Transparent;
            this.AnimationTextBox_X.Font = new System.Drawing.Font("Tahoma", 11F);
            this.AnimationTextBox_X.ForeColor = System.Drawing.Color.DimGray;
            this.AnimationTextBox_X.Location = new System.Drawing.Point(39, 71);
            this.AnimationTextBox_X.MaxLength = 32767;
            this.AnimationTextBox_X.Multiline = false;
            this.AnimationTextBox_X.Name = "AnimationTextBox_X";
            this.AnimationTextBox_X.ReadOnly = false;
            this.AnimationTextBox_X.Size = new System.Drawing.Size(74, 33);
            this.AnimationTextBox_X.TabIndex = 2;
            this.AnimationTextBox_X.Text = "1";
            this.AnimationTextBox_X.TextAlignment = System.Windows.Forms.HorizontalAlignment.Left;
            this.AnimationTextBox_X.UseSystemPasswordChar = false;
            // 
            // iTalk_Label1
            // 
            this.iTalk_Label1.AutoSize = true;
            this.iTalk_Label1.BackColor = System.Drawing.Color.Transparent;
            this.iTalk_Label1.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.iTalk_Label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(142)))), ((int)(((byte)(142)))));
            this.iTalk_Label1.Location = new System.Drawing.Point(14, 43);
            this.iTalk_Label1.Name = "iTalk_Label1";
            this.iTalk_Label1.Size = new System.Drawing.Size(134, 19);
            this.iTalk_Label1.TabIndex = 1;
            this.iTalk_Label1.Text = "애니메이션 X,Y 개수";
            // 
            // AnimationTextBox_Y
            // 
            this.AnimationTextBox_Y.BackColor = System.Drawing.Color.Transparent;
            this.AnimationTextBox_Y.Font = new System.Drawing.Font("Tahoma", 11F);
            this.AnimationTextBox_Y.ForeColor = System.Drawing.Color.DimGray;
            this.AnimationTextBox_Y.Location = new System.Drawing.Point(155, 71);
            this.AnimationTextBox_Y.MaxLength = 32767;
            this.AnimationTextBox_Y.Multiline = false;
            this.AnimationTextBox_Y.Name = "AnimationTextBox_Y";
            this.AnimationTextBox_Y.ReadOnly = false;
            this.AnimationTextBox_Y.Size = new System.Drawing.Size(74, 33);
            this.AnimationTextBox_Y.TabIndex = 0;
            this.AnimationTextBox_Y.Text = "1";
            this.AnimationTextBox_Y.TextAlignment = System.Windows.Forms.HorizontalAlignment.Left;
            this.AnimationTextBox_Y.UseSystemPasswordChar = false;
            // 
            // SettingSpriteForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(246)))), ((int)(((byte)(246)))));
            this.ClientSize = new System.Drawing.Size(360, 210);
            this.Controls.Add(this.iTalk_ThemeContainer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "SettingSpriteForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "애니메이션 설정";
            this.TopMost = true;
            this.TransparencyKey = System.Drawing.Color.Fuchsia;
            this.iTalk_ThemeContainer1.ResumeLayout(false);
            this.iTalk_ThemeContainer1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private iTalk.iTalk_ThemeContainer iTalk_ThemeContainer1;
        private iTalk.iTalk_Label iTalk_Label1;
        private iTalk.iTalk_TextBox_Small AnimationTextBox_Y;
        private iTalk.iTalk_TextBox_Small AnimationTextBox_X;
        private iTalk.iTalk_Button_1 iTalk_Button_11;
        private iTalk.iTalk_Label iTalk_Label3;
        private iTalk.iTalk_Label iTalk_Label2;
    }
}
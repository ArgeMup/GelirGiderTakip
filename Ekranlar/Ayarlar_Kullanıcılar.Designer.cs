namespace Gelir_Gider_Takip.Ekranlar
{
    partial class Ayarlar_Kullanıcılar
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
            panel2 = new System.Windows.Forms.Panel();
            Ekran = new ArgeMup.HazirKod.Ekranlar.Kullanıcılar();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // panel2
            // 
            panel2.Anchor = System.Windows.Forms.AnchorStyles.None;
            panel2.Controls.Add(Ekran);
            panel2.Location = new System.Drawing.Point(11, 13);
            panel2.Margin = new System.Windows.Forms.Padding(2);
            panel2.Name = "panel2";
            panel2.Size = new System.Drawing.Size(740, 423);
            panel2.TabIndex = 4;
            // 
            // Ekran
            // 
            Ekran.Dock = System.Windows.Forms.DockStyle.Fill;
            Ekran.Location = new System.Drawing.Point(0, 0);
            Ekran.Name = "Ekran";
            Ekran.Size = new System.Drawing.Size(740, 423);
            Ekran.TabIndex = 0;
            // 
            // Ayarlar_Kullanıcılar
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(762, 451);
            Controls.Add(panel2);
            KeyPreview = true;
            Margin = new System.Windows.Forms.Padding(2);
            Name = "Ayarlar_Kullanıcılar";
            Text = "Seçiminiz";
            TopMost = true;
            KeyPress += Ayarlar_Kullanıcılar_KeyPress;
            panel2.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private System.Windows.Forms.Panel panel2;
        private global::ArgeMup.HazirKod.Ekranlar.Kullanıcılar Ekran;
    }
}
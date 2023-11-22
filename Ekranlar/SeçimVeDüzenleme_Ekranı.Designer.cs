namespace Gelir_Gider_Takip.Ekranlar
{
    partial class SeçimVeDüzenleme_Ekranı
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
            İşyerleriVeMuhatapGrupları = new ArgeMup.HazirKod.Ekranlar.ListeKutusu();
            Geri = new System.Windows.Forms.Button();
            Seç = new System.Windows.Forms.Button();
            panel1 = new System.Windows.Forms.Panel();
            panel2 = new System.Windows.Forms.Panel();
            Ayraç = new System.Windows.Forms.SplitContainer();
            Muhataplar = new ArgeMup.HazirKod.Ekranlar.ListeKutusu();
            Açıklama = new System.Windows.Forms.Label();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)Ayraç).BeginInit();
            Ayraç.Panel1.SuspendLayout();
            Ayraç.Panel2.SuspendLayout();
            Ayraç.SuspendLayout();
            SuspendLayout();
            // 
            // İşyerleriVeMuhatapGrupları
            // 
            İşyerleriVeMuhatapGrupları.Dock = System.Windows.Forms.DockStyle.Fill;
            İşyerleriVeMuhatapGrupları.Location = new System.Drawing.Point(0, 0);
            İşyerleriVeMuhatapGrupları.Margin = new System.Windows.Forms.Padding(2);
            İşyerleriVeMuhatapGrupları.Name = "İşyerleriVeMuhatapGrupları";
            İşyerleriVeMuhatapGrupları.Padding = new System.Windows.Forms.Padding(3);
            İşyerleriVeMuhatapGrupları.Size = new System.Drawing.Size(148, 317);
            İşyerleriVeMuhatapGrupları.TabIndex = 0;
            İşyerleriVeMuhatapGrupları.GeriBildirim_İşlemi += İşyerleriVeMuhatapGrupları_GeriBildirim_İşlemi;
            // 
            // Geri
            // 
            Geri.Dock = System.Windows.Forms.DockStyle.Left;
            Geri.Location = new System.Drawing.Point(0, 0);
            Geri.Name = "Geri";
            Geri.Size = new System.Drawing.Size(149, 30);
            Geri.TabIndex = 1;
            Geri.Text = "Geri";
            Geri.UseVisualStyleBackColor = true;
            Geri.Click += Geri_Click;
            // 
            // Seç
            // 
            Seç.Dock = System.Windows.Forms.DockStyle.Fill;
            Seç.Location = new System.Drawing.Point(149, 0);
            Seç.Name = "Seç";
            Seç.Size = new System.Drawing.Size(251, 30);
            Seç.TabIndex = 2;
            Seç.Text = "Seç";
            Seç.UseVisualStyleBackColor = true;
            Seç.Click += Seç_Click;
            // 
            // panel1
            // 
            panel1.Controls.Add(Seç);
            panel1.Controls.Add(Geri);
            panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            panel1.Location = new System.Drawing.Point(0, 393);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(400, 30);
            panel1.TabIndex = 3;
            // 
            // panel2
            // 
            panel2.Anchor = System.Windows.Forms.AnchorStyles.None;
            panel2.Controls.Add(Ayraç);
            panel2.Controls.Add(Açıklama);
            panel2.Controls.Add(panel1);
            panel2.Location = new System.Drawing.Point(21, 13);
            panel2.Margin = new System.Windows.Forms.Padding(2);
            panel2.Name = "panel2";
            panel2.Size = new System.Drawing.Size(400, 423);
            panel2.TabIndex = 4;
            // 
            // Ayraç
            // 
            Ayraç.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            Ayraç.Dock = System.Windows.Forms.DockStyle.Fill;
            Ayraç.Location = new System.Drawing.Point(0, 74);
            Ayraç.Name = "Ayraç";
            // 
            // Ayraç.Panel1
            // 
            Ayraç.Panel1.Controls.Add(İşyerleriVeMuhatapGrupları);
            // 
            // Ayraç.Panel2
            // 
            Ayraç.Panel2.Controls.Add(Muhataplar);
            Ayraç.Size = new System.Drawing.Size(400, 319);
            Ayraç.SplitterDistance = 150;
            Ayraç.TabIndex = 5;
            // 
            // Muhataplar
            // 
            Muhataplar.Dock = System.Windows.Forms.DockStyle.Fill;
            Muhataplar.Location = new System.Drawing.Point(0, 0);
            Muhataplar.Margin = new System.Windows.Forms.Padding(2);
            Muhataplar.Name = "Muhataplar";
            Muhataplar.Padding = new System.Windows.Forms.Padding(3);
            Muhataplar.Size = new System.Drawing.Size(244, 317);
            Muhataplar.TabIndex = 1;
            Muhataplar.GeriBildirim_İşlemi += Muhataplar_GeriBildirim_İşlemi;
            // 
            // Açıklama
            // 
            Açıklama.Dock = System.Windows.Forms.DockStyle.Top;
            Açıklama.Location = new System.Drawing.Point(0, 0);
            Açıklama.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            Açıklama.Name = "Açıklama";
            Açıklama.Size = new System.Drawing.Size(400, 74);
            Açıklama.TabIndex = 4;
            Açıklama.Text = "Açıklama\r\n_";
            Açıklama.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SeçimVeDüzenleme_Ekranı
            // 
            AcceptButton = Seç;
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            CancelButton = Geri;
            ClientSize = new System.Drawing.Size(440, 451);
            Controls.Add(panel2);
            Margin = new System.Windows.Forms.Padding(2);
            Name = "SeçimVeDüzenleme_Ekranı";
            Text = "Seçiminiz";
            TopMost = true;
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            Ayraç.Panel1.ResumeLayout(false);
            Ayraç.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)Ayraç).EndInit();
            Ayraç.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private ArgeMup.HazirKod.Ekranlar.ListeKutusu İşyerleriVeMuhatapGrupları;
        private System.Windows.Forms.Button Geri;
        private System.Windows.Forms.Button Seç;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label Açıklama;
        private System.Windows.Forms.SplitContainer Ayraç;
        private ArgeMup.HazirKod.Ekranlar.ListeKutusu Muhataplar;
    }
}
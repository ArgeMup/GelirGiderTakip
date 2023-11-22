namespace Gelir_Gider_Takip.Ekranlar
{
    partial class Açılış_Ekranı
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
            GelirGiderEkle = new System.Windows.Forms.Button();
            Cari = new System.Windows.Forms.Button();
            İşyeriSeçimi = new System.Windows.Forms.Button();
            Ayarlar = new System.Windows.Forms.Button();
            ParolayıDeğiştir = new System.Windows.Forms.Button();
            panel1 = new System.Windows.Forms.Panel();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // GelirGiderEkle
            // 
            GelirGiderEkle.Dock = System.Windows.Forms.DockStyle.Top;
            GelirGiderEkle.Enabled = false;
            GelirGiderEkle.Location = new System.Drawing.Point(0, 88);
            GelirGiderEkle.Name = "GelirGiderEkle";
            GelirGiderEkle.Size = new System.Drawing.Size(346, 44);
            GelirGiderEkle.TabIndex = 2;
            GelirGiderEkle.Text = "Gelir Gider Ekle";
            GelirGiderEkle.UseVisualStyleBackColor = true;
            GelirGiderEkle.Click += GelirGiderEkle_Click;
            // 
            // Cari
            // 
            Cari.Dock = System.Windows.Forms.DockStyle.Top;
            Cari.Enabled = false;
            Cari.Location = new System.Drawing.Point(0, 44);
            Cari.Name = "Cari";
            Cari.Size = new System.Drawing.Size(346, 44);
            Cari.TabIndex = 1;
            Cari.Text = "Cari Döküm";
            Cari.UseVisualStyleBackColor = true;
            Cari.Click += Cari_Click;
            // 
            // İşyeriSeçimi
            // 
            İşyeriSeçimi.Dock = System.Windows.Forms.DockStyle.Top;
            İşyeriSeçimi.Location = new System.Drawing.Point(0, 0);
            İşyeriSeçimi.Name = "İşyeriSeçimi";
            İşyeriSeçimi.Size = new System.Drawing.Size(346, 44);
            İşyeriSeçimi.TabIndex = 0;
            İşyeriSeçimi.Text = "İşyeri Seçimi : İşyeri Adı";
            İşyeriSeçimi.UseVisualStyleBackColor = true;
            İşyeriSeçimi.Click += İşyeriSeçimi_Click;
            // 
            // Ayarlar
            // 
            Ayarlar.Dock = System.Windows.Forms.DockStyle.Top;
            Ayarlar.Enabled = false;
            Ayarlar.Location = new System.Drawing.Point(0, 132);
            Ayarlar.Name = "Ayarlar";
            Ayarlar.Size = new System.Drawing.Size(346, 44);
            Ayarlar.TabIndex = 5;
            Ayarlar.Text = "Ayarlar";
            Ayarlar.UseVisualStyleBackColor = true;
            Ayarlar.Click += Ayarlar_Click;
            // 
            // ParolayıDeğiştir
            // 
            ParolayıDeğiştir.Dock = System.Windows.Forms.DockStyle.Top;
            ParolayıDeğiştir.Location = new System.Drawing.Point(0, 176);
            ParolayıDeğiştir.Name = "ParolayıDeğiştir";
            ParolayıDeğiştir.Size = new System.Drawing.Size(346, 44);
            ParolayıDeğiştir.TabIndex = 6;
            ParolayıDeğiştir.Text = "Parolayı Değiştir";
            ParolayıDeğiştir.UseVisualStyleBackColor = true;
            ParolayıDeğiştir.Click += ParolayıDeğiştir_Click;
            // 
            // panel1
            // 
            panel1.Anchor = System.Windows.Forms.AnchorStyles.None;
            panel1.Controls.Add(ParolayıDeğiştir);
            panel1.Controls.Add(Ayarlar);
            panel1.Controls.Add(GelirGiderEkle);
            panel1.Controls.Add(Cari);
            panel1.Controls.Add(İşyeriSeçimi);
            panel1.Location = new System.Drawing.Point(15, 15);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(346, 255);
            panel1.TabIndex = 7;
            // 
            // Açılış_Ekranı
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(375, 293);
            Controls.Add(panel1);
            Name = "Açılış_Ekranı";
            Text = "Açılış_Ekranı";
            Shown += Açılış_Ekranı_Shown;
            panel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private System.Windows.Forms.Button GelirGiderEkle;
        private System.Windows.Forms.Button Cari;
        private System.Windows.Forms.Button İşyeriSeçimi;
        private System.Windows.Forms.Button Ayarlar;
        private System.Windows.Forms.Button ParolayıDeğiştir;
        private System.Windows.Forms.Panel panel1;
    }
}
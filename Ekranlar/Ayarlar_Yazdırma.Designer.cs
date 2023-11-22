namespace Gelir_Gider_Takip.Ekranlar
{
    partial class Ayarlar_Yazdırma
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Ayarlar_Yazdırma));
            printDialog1 = new System.Windows.Forms.PrintDialog();
            Önizleme = new System.Windows.Forms.PrintPreviewControl();
            printPreviewDialog1 = new System.Windows.Forms.PrintPreviewDialog();
            Yazcılar = new System.Windows.Forms.ComboBox();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            KenarBoşluğu = new System.Windows.Forms.NumericUpDown();
            label3 = new System.Windows.Forms.Label();
            KarakterKümeleri = new System.Windows.Forms.ComboBox();
            KarakterBüyüklüğü = new System.Windows.Forms.NumericUpDown();
            ÖnYüzler_Kaydet = new System.Windows.Forms.Button();
            groupBox2 = new System.Windows.Forms.GroupBox();
            label5 = new System.Windows.Forms.Label();
            FirmaLogo_Yükseklik = new System.Windows.Forms.NumericUpDown();
            label6 = new System.Windows.Forms.Label();
            FirmaLogo_Genişlik = new System.Windows.Forms.NumericUpDown();
            İpucu = new System.Windows.Forms.ToolTip(components);
            DosyayaYazdır = new System.Windows.Forms.CheckBox();
            RenkliHücreler = new System.Windows.Forms.CheckBox();
            panel1 = new System.Windows.Forms.Panel();
            label4 = new System.Windows.Forms.Label();
            YatayGörünüm = new System.Windows.Forms.CheckBox();
            TabloŞablonu = new System.Windows.Forms.ComboBox();
            label7 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)KenarBoşluğu).BeginInit();
            ((System.ComponentModel.ISupportInitialize)KarakterBüyüklüğü).BeginInit();
            groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)FirmaLogo_Yükseklik).BeginInit();
            ((System.ComponentModel.ISupportInitialize)FirmaLogo_Genişlik).BeginInit();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // printDialog1
            // 
            printDialog1.UseEXDialog = true;
            // 
            // Önizleme
            // 
            Önizleme.AutoZoom = false;
            Önizleme.Dock = System.Windows.Forms.DockStyle.Fill;
            Önizleme.Location = new System.Drawing.Point(377, 0);
            Önizleme.Margin = new System.Windows.Forms.Padding(4);
            Önizleme.Name = "Önizleme";
            Önizleme.Size = new System.Drawing.Size(590, 417);
            Önizleme.TabIndex = 0;
            Önizleme.Zoom = 1D;
            // 
            // printPreviewDialog1
            // 
            printPreviewDialog1.AutoScrollMargin = new System.Drawing.Size(0, 0);
            printPreviewDialog1.AutoScrollMinSize = new System.Drawing.Size(0, 0);
            printPreviewDialog1.ClientSize = new System.Drawing.Size(400, 300);
            printPreviewDialog1.Enabled = true;
            printPreviewDialog1.Icon = (System.Drawing.Icon)resources.GetObject("printPreviewDialog1.Icon");
            printPreviewDialog1.Name = "printPreviewDialog1";
            printPreviewDialog1.ShowIcon = false;
            printPreviewDialog1.Visible = false;
            // 
            // Yazcılar
            // 
            Yazcılar.Dock = System.Windows.Forms.DockStyle.Top;
            Yazcılar.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            Yazcılar.FormattingEnabled = true;
            Yazcılar.Location = new System.Drawing.Point(5, 121);
            Yazcılar.Margin = new System.Windows.Forms.Padding(4);
            Yazcılar.Name = "Yazcılar";
            Yazcılar.Size = new System.Drawing.Size(346, 28);
            Yazcılar.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Dock = System.Windows.Forms.DockStyle.Top;
            label1.Location = new System.Drawing.Point(5, 101);
            label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(54, 20);
            label1.TabIndex = 2;
            label1.Text = "Yazıcı";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Dock = System.Windows.Forms.DockStyle.Top;
            label2.Location = new System.Drawing.Point(5, 195);
            label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(133, 20);
            label2.TabIndex = 3;
            label2.Text = "Karakter Kümesi";
            // 
            // KenarBoşluğu
            // 
            KenarBoşluğu.Dock = System.Windows.Forms.DockStyle.Top;
            KenarBoşluğu.Location = new System.Drawing.Point(5, 169);
            KenarBoşluğu.Margin = new System.Windows.Forms.Padding(4);
            KenarBoşluğu.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            KenarBoşluğu.Name = "KenarBoşluğu";
            KenarBoşluğu.Size = new System.Drawing.Size(346, 26);
            KenarBoşluğu.TabIndex = 7;
            KenarBoşluğu.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Dock = System.Windows.Forms.DockStyle.Top;
            label3.Location = new System.Drawing.Point(5, 149);
            label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(233, 20);
            label3.TabIndex = 6;
            label3.Text = "Sayfanın Kenar Boşluğu (mm)";
            // 
            // KarakterKümeleri
            // 
            KarakterKümeleri.Dock = System.Windows.Forms.DockStyle.Top;
            KarakterKümeleri.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            KarakterKümeleri.FormattingEnabled = true;
            KarakterKümeleri.Location = new System.Drawing.Point(5, 215);
            KarakterKümeleri.Margin = new System.Windows.Forms.Padding(4);
            KarakterKümeleri.Name = "KarakterKümeleri";
            KarakterKümeleri.Size = new System.Drawing.Size(346, 28);
            KarakterKümeleri.TabIndex = 14;
            // 
            // KarakterBüyüklüğü
            // 
            KarakterBüyüklüğü.Dock = System.Windows.Forms.DockStyle.Top;
            KarakterBüyüklüğü.Location = new System.Drawing.Point(5, 263);
            KarakterBüyüklüğü.Margin = new System.Windows.Forms.Padding(4);
            KarakterBüyüklüğü.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            KarakterBüyüklüğü.Name = "KarakterBüyüklüğü";
            KarakterBüyüklüğü.Size = new System.Drawing.Size(346, 26);
            KarakterBüyüklüğü.TabIndex = 18;
            KarakterBüyüklüğü.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // ÖnYüzler_Kaydet
            // 
            ÖnYüzler_Kaydet.Dock = System.Windows.Forms.DockStyle.Top;
            ÖnYüzler_Kaydet.Enabled = false;
            ÖnYüzler_Kaydet.Location = new System.Drawing.Point(5, 453);
            ÖnYüzler_Kaydet.Name = "ÖnYüzler_Kaydet";
            ÖnYüzler_Kaydet.Size = new System.Drawing.Size(346, 44);
            ÖnYüzler_Kaydet.TabIndex = 19;
            ÖnYüzler_Kaydet.Text = "Kaydet";
            ÖnYüzler_Kaydet.UseVisualStyleBackColor = true;
            ÖnYüzler_Kaydet.Click += ÖnYüzler_Kaydet_Click;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(label5);
            groupBox2.Controls.Add(FirmaLogo_Yükseklik);
            groupBox2.Controls.Add(label6);
            groupBox2.Controls.Add(FirmaLogo_Genişlik);
            groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            groupBox2.Location = new System.Drawing.Point(5, 289);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new System.Drawing.Size(346, 140);
            groupBox2.TabIndex = 22;
            groupBox2.TabStop = false;
            groupBox2.Text = "Firma Logosu";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(7, 25);
            label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(114, 20);
            label5.TabIndex = 8;
            label5.Text = "Genişlik (mm)";
            // 
            // FirmaLogo_Yükseklik
            // 
            FirmaLogo_Yükseklik.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            FirmaLogo_Yükseklik.Location = new System.Drawing.Point(11, 103);
            FirmaLogo_Yükseklik.Margin = new System.Windows.Forms.Padding(4);
            FirmaLogo_Yükseklik.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            FirmaLogo_Yükseklik.Name = "FirmaLogo_Yükseklik";
            FirmaLogo_Yükseklik.Size = new System.Drawing.Size(328, 26);
            FirmaLogo_Yükseklik.TabIndex = 9;
            İpucu.SetToolTip(FirmaLogo_Yükseklik, "Logo dosyası <Kullanıcı Dosyaları> klasörünün içinde\r\nLOGO.jpg,  LOGO.png veya LOGO.bmp \r\nolarak bulunmalıdır");
            FirmaLogo_Yükseklik.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new System.Drawing.Point(7, 79);
            label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(123, 20);
            label6.TabIndex = 15;
            label6.Text = "Yükseklik (mm)";
            // 
            // FirmaLogo_Genişlik
            // 
            FirmaLogo_Genişlik.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            FirmaLogo_Genişlik.Location = new System.Drawing.Point(11, 49);
            FirmaLogo_Genişlik.Margin = new System.Windows.Forms.Padding(4);
            FirmaLogo_Genişlik.Name = "FirmaLogo_Genişlik";
            FirmaLogo_Genişlik.Size = new System.Drawing.Size(328, 26);
            FirmaLogo_Genişlik.TabIndex = 18;
            İpucu.SetToolTip(FirmaLogo_Genişlik, "Logo dosyası <Kullanıcı Dosyaları> klasörünün içinde\r\nLOGO.jpg,  LOGO.png veya LOGO.bmp \r\nolarak bulunmalıdır");
            FirmaLogo_Genişlik.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // İpucu
            // 
            İpucu.AutomaticDelay = 100;
            İpucu.AutoPopDelay = 10000;
            İpucu.InitialDelay = 100;
            İpucu.IsBalloon = true;
            İpucu.ReshowDelay = 20;
            İpucu.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            İpucu.ToolTipTitle = "Yazdırma";
            İpucu.UseAnimation = false;
            İpucu.UseFading = false;
            // 
            // DosyayaYazdır
            // 
            DosyayaYazdır.AutoSize = true;
            DosyayaYazdır.Dock = System.Windows.Forms.DockStyle.Top;
            DosyayaYazdır.Location = new System.Drawing.Point(5, 429);
            DosyayaYazdır.Name = "DosyayaYazdır";
            DosyayaYazdır.Size = new System.Drawing.Size(346, 24);
            DosyayaYazdır.TabIndex = 24;
            DosyayaYazdır.Text = "Dosyaya Yazdır";
            DosyayaYazdır.UseVisualStyleBackColor = true;
            // 
            // RenkliHücreler
            // 
            RenkliHücreler.AutoSize = true;
            RenkliHücreler.Dock = System.Windows.Forms.DockStyle.Top;
            RenkliHücreler.Location = new System.Drawing.Point(5, 53);
            RenkliHücreler.Name = "RenkliHücreler";
            RenkliHücreler.Size = new System.Drawing.Size(346, 24);
            RenkliHücreler.TabIndex = 25;
            RenkliHücreler.Text = "Renkli Hücreler";
            RenkliHücreler.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            panel1.AutoScroll = true;
            panel1.Controls.Add(ÖnYüzler_Kaydet);
            panel1.Controls.Add(DosyayaYazdır);
            panel1.Controls.Add(groupBox2);
            panel1.Controls.Add(KarakterBüyüklüğü);
            panel1.Controls.Add(label4);
            panel1.Controls.Add(KarakterKümeleri);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(KenarBoşluğu);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(Yazcılar);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(YatayGörünüm);
            panel1.Controls.Add(RenkliHücreler);
            panel1.Controls.Add(TabloŞablonu);
            panel1.Controls.Add(label7);
            panel1.Dock = System.Windows.Forms.DockStyle.Left;
            panel1.Location = new System.Drawing.Point(0, 0);
            panel1.Name = "panel1";
            panel1.Padding = new System.Windows.Forms.Padding(5);
            panel1.Size = new System.Drawing.Size(377, 417);
            panel1.TabIndex = 26;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Dock = System.Windows.Forms.DockStyle.Top;
            label4.Location = new System.Drawing.Point(5, 243);
            label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(154, 20);
            label4.TabIndex = 26;
            label4.Text = "Karakter Büyüklüğü";
            // 
            // YatayGörünüm
            // 
            YatayGörünüm.AutoSize = true;
            YatayGörünüm.Dock = System.Windows.Forms.DockStyle.Top;
            YatayGörünüm.Location = new System.Drawing.Point(5, 77);
            YatayGörünüm.Name = "YatayGörünüm";
            YatayGörünüm.Size = new System.Drawing.Size(346, 24);
            YatayGörünüm.TabIndex = 27;
            YatayGörünüm.Text = "Yatay Görünüm";
            YatayGörünüm.UseVisualStyleBackColor = true;
            // 
            // TabloŞablonu
            // 
            TabloŞablonu.Dock = System.Windows.Forms.DockStyle.Top;
            TabloŞablonu.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            TabloŞablonu.FormattingEnabled = true;
            TabloŞablonu.Location = new System.Drawing.Point(5, 25);
            TabloŞablonu.Margin = new System.Windows.Forms.Padding(4);
            TabloŞablonu.Name = "TabloŞablonu";
            TabloŞablonu.Size = new System.Drawing.Size(346, 28);
            TabloŞablonu.TabIndex = 28;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Dock = System.Windows.Forms.DockStyle.Top;
            label7.Location = new System.Drawing.Point(5, 5);
            label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label7.Name = "label7";
            label7.Size = new System.Drawing.Size(115, 20);
            label7.TabIndex = 29;
            label7.Text = "Tablo Şablonu";
            // 
            // Ayarlar_Yazdırma
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(967, 417);
            Controls.Add(Önizleme);
            Controls.Add(panel1);
            Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            KeyPreview = true;
            Margin = new System.Windows.Forms.Padding(4);
            Name = "Ayarlar_Yazdırma";
            Text = "Yazdırma";
            TopMost = true;
            ((System.ComponentModel.ISupportInitialize)KenarBoşluğu).EndInit();
            ((System.ComponentModel.ISupportInitialize)KarakterBüyüklüğü).EndInit();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)FirmaLogo_Yükseklik).EndInit();
            ((System.ComponentModel.ISupportInitialize)FirmaLogo_Genişlik).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.PrintDialog printDialog1;
        private System.Windows.Forms.PrintPreviewControl Önizleme;
        private System.Windows.Forms.PrintPreviewDialog printPreviewDialog1;
        private System.Windows.Forms.ComboBox Yazcılar;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown KenarBoşluğu;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox KarakterKümeleri;
        private System.Windows.Forms.NumericUpDown KarakterBüyüklüğü;
        private System.Windows.Forms.Button ÖnYüzler_Kaydet;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown FirmaLogo_Yükseklik;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown FirmaLogo_Genişlik;
        private System.Windows.Forms.ToolTip İpucu;
        private System.Windows.Forms.CheckBox DosyayaYazdır;
        private System.Windows.Forms.CheckBox RenkliHücreler;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox YatayGörünüm;
        private System.Windows.Forms.ComboBox TabloŞablonu;
        private System.Windows.Forms.Label label7;
    }
}
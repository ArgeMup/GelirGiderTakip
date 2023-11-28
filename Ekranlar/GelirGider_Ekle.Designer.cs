namespace Gelir_Gider_Takip.Ekranlar
{
    partial class GelirGider_Ekle
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            İpucu = new System.Windows.Forms.ToolTip(components);
            ParaBirimi = new System.Windows.Forms.ComboBox();
            Notlar = new System.Windows.Forms.TextBox();
            ÖdemeTarihi_Değeri = new System.Windows.Forms.DateTimePicker();
            ÖdemeTarihi_Yazısı = new System.Windows.Forms.Label();
            Miktar = new System.Windows.Forms.NumericUpDown();
            Gider = new System.Windows.Forms.RadioButton();
            Gelir = new System.Windows.Forms.RadioButton();
            ÖnYüzler_Kaydet = new System.Windows.Forms.Button();
            Tablo = new System.Windows.Forms.DataGridView();
            Tablo_Taksit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            Tablo_ÖdemeTarihi = new System.Windows.Forms.DataGridViewTextBoxColumn();
            Tablo_Miktarı = new System.Windows.Forms.DataGridViewTextBoxColumn();
            panel4 = new System.Windows.Forms.Panel();
            Taksit_Adet = new System.Windows.Forms.NumericUpDown();
            label4 = new System.Windows.Forms.Label();
            panel3 = new System.Windows.Forms.Panel();
            Taksit_Dönem = new System.Windows.Forms.ComboBox();
            Taksit_Dönem_Adet = new System.Windows.Forms.NumericUpDown();
            label6 = new System.Windows.Forms.Label();
            Üyelik_BitişTarihi = new System.Windows.Forms.DateTimePicker();
            label1 = new System.Windows.Forms.Label();
            Üyelik_Dönem = new System.Windows.Forms.ComboBox();
            Üyelik_Dönem_Adet = new System.Windows.Forms.NumericUpDown();
            label2 = new System.Windows.Forms.Label();
            İşyeri_Grup_Muhatap = new System.Windows.Forms.Button();
            Ayraç_Detaylar_Taksit = new System.Windows.Forms.SplitContainer();
            Durum = new System.Windows.Forms.Panel();
            Durum_Ödenmedi = new System.Windows.Forms.RadioButton();
            Durum_Ödendi = new System.Windows.Forms.RadioButton();
            Üyelik = new System.Windows.Forms.CheckBox();
            Üyelik_Grubu = new System.Windows.Forms.GroupBox();
            panel6 = new System.Windows.Forms.Panel();
            panel5 = new System.Windows.Forms.Panel();
            Peşinat = new System.Windows.Forms.Panel();
            PeşinatMiktarı = new System.Windows.Forms.NumericUpDown();
            label7 = new System.Windows.Forms.Label();
            panel8 = new System.Windows.Forms.Panel();
            Avans = new System.Windows.Forms.RadioButton();
            label5 = new System.Windows.Forms.Label();
            panel7 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)Miktar).BeginInit();
            ((System.ComponentModel.ISupportInitialize)Tablo).BeginInit();
            panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)Taksit_Adet).BeginInit();
            panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)Taksit_Dönem_Adet).BeginInit();
            ((System.ComponentModel.ISupportInitialize)Üyelik_Dönem_Adet).BeginInit();
            ((System.ComponentModel.ISupportInitialize)Ayraç_Detaylar_Taksit).BeginInit();
            Ayraç_Detaylar_Taksit.Panel1.SuspendLayout();
            Ayraç_Detaylar_Taksit.Panel2.SuspendLayout();
            Ayraç_Detaylar_Taksit.SuspendLayout();
            Durum.SuspendLayout();
            Üyelik_Grubu.SuspendLayout();
            panel6.SuspendLayout();
            panel5.SuspendLayout();
            Peşinat.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)PeşinatMiktarı).BeginInit();
            panel8.SuspendLayout();
            panel7.SuspendLayout();
            SuspendLayout();
            // 
            // İpucu
            // 
            İpucu.AutomaticDelay = 100;
            İpucu.AutoPopDelay = 10000;
            İpucu.InitialDelay = 100;
            İpucu.IsBalloon = true;
            İpucu.ReshowDelay = 20;
            İpucu.UseAnimation = false;
            İpucu.UseFading = false;
            // 
            // ParaBirimi
            // 
            ParaBirimi.Dock = System.Windows.Forms.DockStyle.Right;
            ParaBirimi.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            ParaBirimi.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            ParaBirimi.FormattingEnabled = true;
            ParaBirimi.Items.AddRange(new object[] { "₺", "Avro", "Dolar" });
            ParaBirimi.Location = new System.Drawing.Point(134, 0);
            ParaBirimi.Name = "ParaBirimi";
            ParaBirimi.Size = new System.Drawing.Size(88, 36);
            ParaBirimi.TabIndex = 18;
            İpucu.SetToolTip(ParaBirimi, "Para birimi");
            ParaBirimi.SelectedIndexChanged += AyarDeğişti;
            // 
            // Notlar
            // 
            Notlar.Dock = System.Windows.Forms.DockStyle.Fill;
            Notlar.Location = new System.Drawing.Point(0, 144);
            Notlar.Multiline = true;
            Notlar.Name = "Notlar";
            Notlar.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            Notlar.Size = new System.Drawing.Size(391, 68);
            Notlar.TabIndex = 19;
            İpucu.SetToolTip(Notlar, "Notlar");
            Notlar.WordWrap = false;
            Notlar.TextChanged += AyarDeğişti;
            // 
            // ÖdemeTarihi_Değeri
            // 
            ÖdemeTarihi_Değeri.Dock = System.Windows.Forms.DockStyle.Top;
            ÖdemeTarihi_Değeri.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            ÖdemeTarihi_Değeri.Location = new System.Drawing.Point(0, 20);
            ÖdemeTarihi_Değeri.Name = "ÖdemeTarihi_Değeri";
            ÖdemeTarihi_Değeri.Size = new System.Drawing.Size(391, 32);
            ÖdemeTarihi_Değeri.TabIndex = 15;
            ÖdemeTarihi_Değeri.ValueChanged += AyarDeğişti;
            // 
            // ÖdemeTarihi_Yazısı
            // 
            ÖdemeTarihi_Yazısı.AutoSize = true;
            ÖdemeTarihi_Yazısı.Dock = System.Windows.Forms.DockStyle.Top;
            ÖdemeTarihi_Yazısı.Location = new System.Drawing.Point(0, 0);
            ÖdemeTarihi_Yazısı.Name = "ÖdemeTarihi_Yazısı";
            ÖdemeTarihi_Yazısı.Size = new System.Drawing.Size(185, 20);
            ÖdemeTarihi_Yazısı.TabIndex = 16;
            ÖdemeTarihi_Yazısı.Text = "Ödemenin Yapılacağı Tarih";
            // 
            // Miktar
            // 
            Miktar.DecimalPlaces = 2;
            Miktar.Dock = System.Windows.Forms.DockStyle.Fill;
            Miktar.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            Miktar.Increment = new decimal(new int[] { 100, 0, 0, 0 });
            Miktar.Location = new System.Drawing.Point(0, 0);
            Miktar.Maximum = new decimal(new int[] { 1241513983, 370409800, 542101, 0 });
            Miktar.Name = "Miktar";
            Miktar.Size = new System.Drawing.Size(134, 36);
            Miktar.TabIndex = 4;
            Miktar.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            Miktar.ValueChanged += AyarDeğişti;
            // 
            // Gider
            // 
            Gider.Appearance = System.Windows.Forms.Appearance.Button;
            Gider.AutoSize = true;
            Gider.Checked = true;
            Gider.Dock = System.Windows.Forms.DockStyle.Right;
            Gider.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            Gider.Location = new System.Drawing.Point(334, 0);
            Gider.Name = "Gider";
            Gider.Size = new System.Drawing.Size(57, 36);
            Gider.TabIndex = 10;
            Gider.TabStop = true;
            Gider.Text = "Gider";
            Gider.UseVisualStyleBackColor = true;
            // 
            // Gelir
            // 
            Gelir.Appearance = System.Windows.Forms.Appearance.Button;
            Gelir.AutoSize = true;
            Gelir.Dock = System.Windows.Forms.DockStyle.Right;
            Gelir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            Gelir.Location = new System.Drawing.Point(222, 0);
            Gelir.Name = "Gelir";
            Gelir.Size = new System.Drawing.Size(52, 36);
            Gelir.TabIndex = 10;
            Gelir.Text = "Gelir";
            Gelir.UseVisualStyleBackColor = true;
            // 
            // ÖnYüzler_Kaydet
            // 
            ÖnYüzler_Kaydet.AutoSize = true;
            ÖnYüzler_Kaydet.BackColor = System.Drawing.Color.YellowGreen;
            ÖnYüzler_Kaydet.Dock = System.Windows.Forms.DockStyle.Right;
            ÖnYüzler_Kaydet.Enabled = false;
            ÖnYüzler_Kaydet.Location = new System.Drawing.Point(691, 0);
            ÖnYüzler_Kaydet.Name = "ÖnYüzler_Kaydet";
            ÖnYüzler_Kaydet.Size = new System.Drawing.Size(65, 46);
            ÖnYüzler_Kaydet.TabIndex = 16;
            ÖnYüzler_Kaydet.Text = "Kaydet";
            ÖnYüzler_Kaydet.UseVisualStyleBackColor = false;
            ÖnYüzler_Kaydet.Click += ÖnYüzler_Kaydet_Click;
            // 
            // Tablo
            // 
            Tablo.AllowUserToAddRows = false;
            Tablo.AllowUserToDeleteRows = false;
            Tablo.AllowUserToResizeColumns = false;
            Tablo.AllowUserToResizeRows = false;
            Tablo.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            Tablo.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            Tablo.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            Tablo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            Tablo.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { Tablo_Taksit, Tablo_ÖdemeTarihi, Tablo_Miktarı });
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            Tablo.DefaultCellStyle = dataGridViewCellStyle2;
            Tablo.Dock = System.Windows.Forms.DockStyle.Fill;
            Tablo.Location = new System.Drawing.Point(0, 52);
            Tablo.MultiSelect = false;
            Tablo.Name = "Tablo";
            Tablo.ReadOnly = true;
            Tablo.RowHeadersVisible = false;
            Tablo.RowHeadersWidth = 51;
            Tablo.RowTemplate.Height = 29;
            Tablo.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            Tablo.ShowCellErrors = false;
            Tablo.ShowCellToolTips = false;
            Tablo.ShowEditingIcon = false;
            Tablo.ShowRowErrors = false;
            Tablo.Size = new System.Drawing.Size(353, 305);
            Tablo.TabIndex = 28;
            // 
            // Tablo_Taksit
            // 
            Tablo_Taksit.HeaderText = "Taksit";
            Tablo_Taksit.MinimumWidth = 6;
            Tablo_Taksit.Name = "Tablo_Taksit";
            Tablo_Taksit.ReadOnly = true;
            Tablo_Taksit.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Tablo_ÖdemeTarihi
            // 
            Tablo_ÖdemeTarihi.HeaderText = "Ödeme Tarihi";
            Tablo_ÖdemeTarihi.MinimumWidth = 6;
            Tablo_ÖdemeTarihi.Name = "Tablo_ÖdemeTarihi";
            Tablo_ÖdemeTarihi.ReadOnly = true;
            Tablo_ÖdemeTarihi.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Tablo_Miktarı
            // 
            Tablo_Miktarı.HeaderText = "Miktarı";
            Tablo_Miktarı.MinimumWidth = 6;
            Tablo_Miktarı.Name = "Tablo_Miktarı";
            Tablo_Miktarı.ReadOnly = true;
            Tablo_Miktarı.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // panel4
            // 
            panel4.Controls.Add(Taksit_Adet);
            panel4.Controls.Add(label4);
            panel4.Dock = System.Windows.Forms.DockStyle.Top;
            panel4.Location = new System.Drawing.Point(0, 26);
            panel4.Margin = new System.Windows.Forms.Padding(2);
            panel4.Name = "panel4";
            panel4.Size = new System.Drawing.Size(353, 26);
            panel4.TabIndex = 30;
            // 
            // Taksit_Adet
            // 
            Taksit_Adet.Dock = System.Windows.Forms.DockStyle.Fill;
            Taksit_Adet.Location = new System.Drawing.Point(113, 0);
            Taksit_Adet.Maximum = new decimal(new int[] { 1241513983, 370409800, 542101, 0 });
            Taksit_Adet.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            Taksit_Adet.Name = "Taksit_Adet";
            Taksit_Adet.Size = new System.Drawing.Size(240, 27);
            Taksit_Adet.TabIndex = 25;
            Taksit_Adet.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            Taksit_Adet.Value = new decimal(new int[] { 1, 0, 0, 0 });
            Taksit_Adet.ValueChanged += AyarDeğişti;
            // 
            // label4
            // 
            label4.Dock = System.Windows.Forms.DockStyle.Left;
            label4.Location = new System.Drawing.Point(0, 0);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(113, 26);
            label4.TabIndex = 24;
            label4.Text = "Taksit Sayısı";
            // 
            // panel3
            // 
            panel3.Controls.Add(Taksit_Dönem);
            panel3.Controls.Add(Taksit_Dönem_Adet);
            panel3.Controls.Add(label6);
            panel3.Dock = System.Windows.Forms.DockStyle.Top;
            panel3.Location = new System.Drawing.Point(0, 0);
            panel3.Margin = new System.Windows.Forms.Padding(2);
            panel3.Name = "panel3";
            panel3.Size = new System.Drawing.Size(353, 26);
            panel3.TabIndex = 29;
            // 
            // Taksit_Dönem
            // 
            Taksit_Dönem.Dock = System.Windows.Forms.DockStyle.Fill;
            Taksit_Dönem.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            Taksit_Dönem.FormattingEnabled = true;
            Taksit_Dönem.Items.AddRange(new object[] { "Günde 1 taksit", "Haftada 1 taksit", "Ayda 1 taksit", "Yılda 1 taksit" });
            Taksit_Dönem.Location = new System.Drawing.Point(223, 0);
            Taksit_Dönem.Name = "Taksit_Dönem";
            Taksit_Dönem.Size = new System.Drawing.Size(130, 28);
            Taksit_Dönem.TabIndex = 30;
            Taksit_Dönem.SelectedIndexChanged += AyarDeğişti;
            // 
            // Taksit_Dönem_Adet
            // 
            Taksit_Dönem_Adet.Dock = System.Windows.Forms.DockStyle.Left;
            Taksit_Dönem_Adet.Location = new System.Drawing.Point(113, 0);
            Taksit_Dönem_Adet.Maximum = new decimal(new int[] { 1241513983, 370409800, 542101, 0 });
            Taksit_Dönem_Adet.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            Taksit_Dönem_Adet.Name = "Taksit_Dönem_Adet";
            Taksit_Dönem_Adet.Size = new System.Drawing.Size(110, 27);
            Taksit_Dönem_Adet.TabIndex = 29;
            Taksit_Dönem_Adet.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            Taksit_Dönem_Adet.Value = new decimal(new int[] { 1, 0, 0, 0 });
            Taksit_Dönem_Adet.ValueChanged += AyarDeğişti;
            // 
            // label6
            // 
            label6.Dock = System.Windows.Forms.DockStyle.Left;
            label6.Location = new System.Drawing.Point(0, 0);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(113, 26);
            label6.TabIndex = 28;
            label6.Text = "Her";
            // 
            // Üyelik_BitişTarihi
            // 
            Üyelik_BitişTarihi.Checked = false;
            Üyelik_BitişTarihi.Dock = System.Windows.Forms.DockStyle.Fill;
            Üyelik_BitişTarihi.Location = new System.Drawing.Point(113, 0);
            Üyelik_BitişTarihi.Name = "Üyelik_BitişTarihi";
            Üyelik_BitişTarihi.ShowCheckBox = true;
            Üyelik_BitişTarihi.Size = new System.Drawing.Size(272, 27);
            Üyelik_BitişTarihi.TabIndex = 20;
            Üyelik_BitişTarihi.ValueChanged += AyarDeğişti;
            // 
            // label1
            // 
            label1.Dock = System.Windows.Forms.DockStyle.Left;
            label1.Location = new System.Drawing.Point(0, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(113, 26);
            label1.TabIndex = 21;
            label1.Text = "Bitiş Tarihi";
            // 
            // Üyelik_Dönem
            // 
            Üyelik_Dönem.Dock = System.Windows.Forms.DockStyle.Fill;
            Üyelik_Dönem.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            Üyelik_Dönem.FormattingEnabled = true;
            Üyelik_Dönem.Items.AddRange(new object[] { "Günde 1 kez", "Haftada 1 kez", "Ayda 1 kez", "Yılda 1 kez" });
            Üyelik_Dönem.Location = new System.Drawing.Point(223, 0);
            Üyelik_Dönem.Name = "Üyelik_Dönem";
            Üyelik_Dönem.Size = new System.Drawing.Size(162, 28);
            Üyelik_Dönem.TabIndex = 25;
            Üyelik_Dönem.SelectedIndexChanged += AyarDeğişti;
            // 
            // Üyelik_Dönem_Adet
            // 
            Üyelik_Dönem_Adet.Dock = System.Windows.Forms.DockStyle.Left;
            Üyelik_Dönem_Adet.Location = new System.Drawing.Point(113, 0);
            Üyelik_Dönem_Adet.Maximum = new decimal(new int[] { 1241513983, 370409800, 542101, 0 });
            Üyelik_Dönem_Adet.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            Üyelik_Dönem_Adet.Name = "Üyelik_Dönem_Adet";
            Üyelik_Dönem_Adet.Size = new System.Drawing.Size(110, 27);
            Üyelik_Dönem_Adet.TabIndex = 24;
            Üyelik_Dönem_Adet.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            Üyelik_Dönem_Adet.Value = new decimal(new int[] { 1, 0, 0, 0 });
            Üyelik_Dönem_Adet.ValueChanged += AyarDeğişti;
            // 
            // label2
            // 
            label2.Dock = System.Windows.Forms.DockStyle.Left;
            label2.Location = new System.Drawing.Point(0, 0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(113, 26);
            label2.TabIndex = 23;
            label2.Text = "Her";
            // 
            // İşyeri_Grup_Muhatap
            // 
            İşyeri_Grup_Muhatap.AutoSize = true;
            İşyeri_Grup_Muhatap.Dock = System.Windows.Forms.DockStyle.Fill;
            İşyeri_Grup_Muhatap.Location = new System.Drawing.Point(0, 0);
            İşyeri_Grup_Muhatap.Name = "İşyeri_Grup_Muhatap";
            İşyeri_Grup_Muhatap.Size = new System.Drawing.Size(691, 46);
            İşyeri_Grup_Muhatap.TabIndex = 17;
            İşyeri_Grup_Muhatap.Text = "İşyeri | çalışan | tahta kafa";
            İşyeri_Grup_Muhatap.UseVisualStyleBackColor = true;
            İşyeri_Grup_Muhatap.Click += İşyeri_Grup_Muhatap_Click;
            // 
            // Ayraç_Detaylar_Taksit
            // 
            Ayraç_Detaylar_Taksit.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            Ayraç_Detaylar_Taksit.Dock = System.Windows.Forms.DockStyle.Fill;
            Ayraç_Detaylar_Taksit.Location = new System.Drawing.Point(3, 49);
            Ayraç_Detaylar_Taksit.Name = "Ayraç_Detaylar_Taksit";
            // 
            // Ayraç_Detaylar_Taksit.Panel1
            // 
            Ayraç_Detaylar_Taksit.Panel1.Controls.Add(Notlar);
            Ayraç_Detaylar_Taksit.Panel1.Controls.Add(Durum);
            Ayraç_Detaylar_Taksit.Panel1.Controls.Add(Üyelik);
            Ayraç_Detaylar_Taksit.Panel1.Controls.Add(Üyelik_Grubu);
            Ayraç_Detaylar_Taksit.Panel1.Controls.Add(Peşinat);
            Ayraç_Detaylar_Taksit.Panel1.Controls.Add(panel8);
            Ayraç_Detaylar_Taksit.Panel1.Controls.Add(label5);
            Ayraç_Detaylar_Taksit.Panel1.Controls.Add(ÖdemeTarihi_Değeri);
            Ayraç_Detaylar_Taksit.Panel1.Controls.Add(ÖdemeTarihi_Yazısı);
            // 
            // Ayraç_Detaylar_Taksit.Panel2
            // 
            Ayraç_Detaylar_Taksit.Panel2.Controls.Add(Tablo);
            Ayraç_Detaylar_Taksit.Panel2.Controls.Add(panel4);
            Ayraç_Detaylar_Taksit.Panel2.Controls.Add(panel3);
            Ayraç_Detaylar_Taksit.Size = new System.Drawing.Size(756, 361);
            Ayraç_Detaylar_Taksit.SplitterDistance = 395;
            Ayraç_Detaylar_Taksit.TabIndex = 18;
            // 
            // Durum
            // 
            Durum.Controls.Add(Durum_Ödenmedi);
            Durum.Controls.Add(Durum_Ödendi);
            Durum.Dock = System.Windows.Forms.DockStyle.Bottom;
            Durum.Location = new System.Drawing.Point(0, 212);
            Durum.Name = "Durum";
            Durum.Size = new System.Drawing.Size(391, 38);
            Durum.TabIndex = 4;
            // 
            // Durum_Ödenmedi
            // 
            Durum_Ödenmedi.Appearance = System.Windows.Forms.Appearance.Button;
            Durum_Ödenmedi.AutoSize = true;
            Durum_Ödenmedi.Checked = true;
            Durum_Ödenmedi.Dock = System.Windows.Forms.DockStyle.Fill;
            Durum_Ödenmedi.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            Durum_Ödenmedi.Location = new System.Drawing.Point(116, 0);
            Durum_Ödenmedi.Name = "Durum_Ödenmedi";
            Durum_Ödenmedi.Size = new System.Drawing.Size(275, 38);
            Durum_Ödenmedi.TabIndex = 12;
            Durum_Ödenmedi.TabStop = true;
            Durum_Ödenmedi.Text = "Ödenmedi";
            Durum_Ödenmedi.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            Durum_Ödenmedi.UseVisualStyleBackColor = true;
            // 
            // Durum_Ödendi
            // 
            Durum_Ödendi.Appearance = System.Windows.Forms.Appearance.Button;
            Durum_Ödendi.Dock = System.Windows.Forms.DockStyle.Left;
            Durum_Ödendi.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            Durum_Ödendi.Location = new System.Drawing.Point(0, 0);
            Durum_Ödendi.Name = "Durum_Ödendi";
            Durum_Ödendi.Size = new System.Drawing.Size(116, 38);
            Durum_Ödendi.TabIndex = 11;
            Durum_Ödendi.Text = "Ödendi";
            Durum_Ödendi.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            Durum_Ödendi.UseVisualStyleBackColor = true;
            // 
            // Üyelik
            // 
            Üyelik.AutoSize = true;
            Üyelik.Dock = System.Windows.Forms.DockStyle.Bottom;
            Üyelik.Location = new System.Drawing.Point(0, 250);
            Üyelik.Name = "Üyelik";
            Üyelik.Size = new System.Drawing.Size(391, 24);
            Üyelik.TabIndex = 21;
            Üyelik.Text = "Üyelik";
            Üyelik.UseVisualStyleBackColor = true;
            Üyelik.CheckedChanged += AyarDeğişti;
            // 
            // Üyelik_Grubu
            // 
            Üyelik_Grubu.Controls.Add(panel6);
            Üyelik_Grubu.Controls.Add(panel5);
            Üyelik_Grubu.Dock = System.Windows.Forms.DockStyle.Bottom;
            Üyelik_Grubu.Location = new System.Drawing.Point(0, 274);
            Üyelik_Grubu.Name = "Üyelik_Grubu";
            Üyelik_Grubu.Size = new System.Drawing.Size(391, 83);
            Üyelik_Grubu.TabIndex = 20;
            Üyelik_Grubu.TabStop = false;
            // 
            // panel6
            // 
            panel6.Controls.Add(Üyelik_BitişTarihi);
            panel6.Controls.Add(label1);
            panel6.Dock = System.Windows.Forms.DockStyle.Top;
            panel6.Location = new System.Drawing.Point(3, 49);
            panel6.Margin = new System.Windows.Forms.Padding(2);
            panel6.Name = "panel6";
            panel6.Size = new System.Drawing.Size(385, 26);
            panel6.TabIndex = 3;
            // 
            // panel5
            // 
            panel5.Controls.Add(Üyelik_Dönem);
            panel5.Controls.Add(Üyelik_Dönem_Adet);
            panel5.Controls.Add(label2);
            panel5.Dock = System.Windows.Forms.DockStyle.Top;
            panel5.Location = new System.Drawing.Point(3, 23);
            panel5.Margin = new System.Windows.Forms.Padding(2);
            panel5.Name = "panel5";
            panel5.Size = new System.Drawing.Size(385, 26);
            panel5.TabIndex = 2;
            // 
            // Peşinat
            // 
            Peşinat.Controls.Add(PeşinatMiktarı);
            Peşinat.Controls.Add(label7);
            Peşinat.Dock = System.Windows.Forms.DockStyle.Top;
            Peşinat.Location = new System.Drawing.Point(0, 108);
            Peşinat.Name = "Peşinat";
            Peşinat.Size = new System.Drawing.Size(391, 36);
            Peşinat.TabIndex = 37;
            // 
            // PeşinatMiktarı
            // 
            PeşinatMiktarı.DecimalPlaces = 2;
            PeşinatMiktarı.Dock = System.Windows.Forms.DockStyle.Fill;
            PeşinatMiktarı.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            PeşinatMiktarı.Increment = new decimal(new int[] { 100, 0, 0, 0 });
            PeşinatMiktarı.Location = new System.Drawing.Point(105, 0);
            PeşinatMiktarı.Maximum = new decimal(new int[] { 1241513983, 370409800, 542101, 0 });
            PeşinatMiktarı.Name = "PeşinatMiktarı";
            PeşinatMiktarı.Size = new System.Drawing.Size(286, 36);
            PeşinatMiktarı.TabIndex = 4;
            PeşinatMiktarı.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            PeşinatMiktarı.ValueChanged += AyarDeğişti;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Dock = System.Windows.Forms.DockStyle.Left;
            label7.Location = new System.Drawing.Point(0, 0);
            label7.Name = "label7";
            label7.Size = new System.Drawing.Size(105, 20);
            label7.TabIndex = 38;
            label7.Text = "Peşinat Miktarı";
            // 
            // panel8
            // 
            panel8.Controls.Add(Miktar);
            panel8.Controls.Add(ParaBirimi);
            panel8.Controls.Add(Gelir);
            panel8.Controls.Add(Avans);
            panel8.Controls.Add(Gider);
            panel8.Dock = System.Windows.Forms.DockStyle.Top;
            panel8.Location = new System.Drawing.Point(0, 72);
            panel8.Name = "panel8";
            panel8.Size = new System.Drawing.Size(391, 36);
            panel8.TabIndex = 18;
            // 
            // Avans
            // 
            Avans.Appearance = System.Windows.Forms.Appearance.Button;
            Avans.AutoSize = true;
            Avans.Dock = System.Windows.Forms.DockStyle.Right;
            Avans.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            Avans.Location = new System.Drawing.Point(274, 0);
            Avans.Name = "Avans";
            Avans.Size = new System.Drawing.Size(60, 36);
            Avans.TabIndex = 19;
            Avans.Text = "Avans";
            Avans.UseVisualStyleBackColor = true;
            Avans.Visible = false;
            Avans.CheckedChanged += AyarDeğişti;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Dock = System.Windows.Forms.DockStyle.Top;
            label5.Location = new System.Drawing.Point(0, 52);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(108, 20);
            label5.TabIndex = 36;
            label5.Text = "Ödeme Miktarı";
            // 
            // panel7
            // 
            panel7.Controls.Add(İşyeri_Grup_Muhatap);
            panel7.Controls.Add(ÖnYüzler_Kaydet);
            panel7.Dock = System.Windows.Forms.DockStyle.Top;
            panel7.Location = new System.Drawing.Point(3, 3);
            panel7.Name = "panel7";
            panel7.Size = new System.Drawing.Size(756, 46);
            panel7.TabIndex = 21;
            // 
            // GelirGider_Ekle
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(762, 413);
            Controls.Add(Ayraç_Detaylar_Taksit);
            Controls.Add(panel7);
            KeyPreview = true;
            Name = "GelirGider_Ekle";
            Padding = new System.Windows.Forms.Padding(3);
            Shown += GelirGider_Ekle_Shown;
            KeyPress += GelirGider_Ekle_KeyPress;
            ((System.ComponentModel.ISupportInitialize)Miktar).EndInit();
            ((System.ComponentModel.ISupportInitialize)Tablo).EndInit();
            panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)Taksit_Adet).EndInit();
            panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)Taksit_Dönem_Adet).EndInit();
            ((System.ComponentModel.ISupportInitialize)Üyelik_Dönem_Adet).EndInit();
            Ayraç_Detaylar_Taksit.Panel1.ResumeLayout(false);
            Ayraç_Detaylar_Taksit.Panel1.PerformLayout();
            Ayraç_Detaylar_Taksit.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)Ayraç_Detaylar_Taksit).EndInit();
            Ayraç_Detaylar_Taksit.ResumeLayout(false);
            Durum.ResumeLayout(false);
            Durum.PerformLayout();
            Üyelik_Grubu.ResumeLayout(false);
            panel6.ResumeLayout(false);
            panel5.ResumeLayout(false);
            Peşinat.ResumeLayout(false);
            Peşinat.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)PeşinatMiktarı).EndInit();
            panel8.ResumeLayout(false);
            panel8.PerformLayout();
            panel7.ResumeLayout(false);
            panel7.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.ToolTip İpucu;
        private System.Windows.Forms.NumericUpDown Miktar;
        private System.Windows.Forms.RadioButton Gelir;
        private System.Windows.Forms.RadioButton Gider;
        private System.Windows.Forms.Label ÖdemeTarihi_Yazısı;
        private System.Windows.Forms.DateTimePicker ÖdemeTarihi_Değeri;
        private System.Windows.Forms.DataGridView Tablo;
        private System.Windows.Forms.Button ÖnYüzler_Kaydet;
        private System.Windows.Forms.DataGridViewTextBoxColumn Tablo_Taksit;
        private System.Windows.Forms.DataGridViewTextBoxColumn Tablo_ÖdemeTarihi;
        private System.Windows.Forms.DataGridViewTextBoxColumn Tablo_Miktarı;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.NumericUpDown Taksit_Adet;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.ComboBox Taksit_Dönem;
        private System.Windows.Forms.NumericUpDown Taksit_Dönem_Adet;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DateTimePicker Üyelik_BitişTarihi;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox Üyelik_Dönem;
        private System.Windows.Forms.NumericUpDown Üyelik_Dönem_Adet;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button İşyeri_Grup_Muhatap;
        private System.Windows.Forms.SplitContainer Ayraç_Detaylar_Taksit;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.ComboBox ParaBirimi;
        private System.Windows.Forms.TextBox Notlar;
        private System.Windows.Forms.GroupBox Üyelik_Grubu;
        private System.Windows.Forms.CheckBox Üyelik;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.RadioButton Avans;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel Peşinat;
        private System.Windows.Forms.NumericUpDown PeşinatMiktarı;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel Durum;
        private System.Windows.Forms.RadioButton Durum_Ödenmedi;
        private System.Windows.Forms.RadioButton Durum_Ödendi;
    }
}
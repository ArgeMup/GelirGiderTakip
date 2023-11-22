namespace Gelir_Gider_Takip.Ekranlar
{
    partial class Ayarlar
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
            Yazdırma = new System.Windows.Forms.Button();
            Muhataplar = new System.Windows.Forms.Button();
            İşyeriAyarları = new System.Windows.Forms.Button();
            Kullanıcılar = new System.Windows.Forms.Button();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // panel2
            // 
            panel2.Anchor = System.Windows.Forms.AnchorStyles.None;
            panel2.Controls.Add(Yazdırma);
            panel2.Controls.Add(Muhataplar);
            panel2.Controls.Add(İşyeriAyarları);
            panel2.Controls.Add(Kullanıcılar);
            panel2.Location = new System.Drawing.Point(12, 11);
            panel2.Margin = new System.Windows.Forms.Padding(2);
            panel2.Name = "panel2";
            panel2.Size = new System.Drawing.Size(294, 208);
            panel2.TabIndex = 4;
            // 
            // Yazdırma
            // 
            Yazdırma.Anchor = System.Windows.Forms.AnchorStyles.None;
            Yazdırma.Location = new System.Drawing.Point(10, 107);
            Yazdırma.Name = "Yazdırma";
            Yazdırma.Size = new System.Drawing.Size(274, 44);
            Yazdırma.TabIndex = 5;
            Yazdırma.Text = "Yazdırma";
            Yazdırma.UseVisualStyleBackColor = true;
            Yazdırma.Click += Yazdırma_Click;
            // 
            // Muhataplar
            // 
            Muhataplar.Anchor = System.Windows.Forms.AnchorStyles.None;
            Muhataplar.Location = new System.Drawing.Point(10, 57);
            Muhataplar.Name = "Muhataplar";
            Muhataplar.Size = new System.Drawing.Size(274, 44);
            Muhataplar.TabIndex = 4;
            Muhataplar.Text = "Muhataplar";
            Muhataplar.UseVisualStyleBackColor = true;
            Muhataplar.Click += Muhataplar_Click;
            // 
            // İşyeriAyarları
            // 
            İşyeriAyarları.Anchor = System.Windows.Forms.AnchorStyles.None;
            İşyeriAyarları.Location = new System.Drawing.Point(10, 7);
            İşyeriAyarları.Name = "İşyeriAyarları";
            İşyeriAyarları.Size = new System.Drawing.Size(274, 44);
            İşyeriAyarları.TabIndex = 2;
            İşyeriAyarları.Text = "İşyeri Ayarları";
            İşyeriAyarları.UseVisualStyleBackColor = true;
            İşyeriAyarları.Visible = false;
            İşyeriAyarları.Click += İşyeriAyarları_Click;
            // 
            // Kullanıcılar
            // 
            Kullanıcılar.Anchor = System.Windows.Forms.AnchorStyles.None;
            Kullanıcılar.Location = new System.Drawing.Point(10, 157);
            Kullanıcılar.Name = "Kullanıcılar";
            Kullanıcılar.Size = new System.Drawing.Size(274, 44);
            Kullanıcılar.TabIndex = 1;
            Kullanıcılar.Text = "Kullanıcılar";
            Kullanıcılar.UseVisualStyleBackColor = true;
            Kullanıcılar.Click += Kullanıcılar_Click;
            // 
            // Ayarlar
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(317, 236);
            Controls.Add(panel2);
            KeyPreview = true;
            Margin = new System.Windows.Forms.Padding(2);
            Name = "Ayarlar";
            Text = "Seçiminiz";
            TopMost = true;
            KeyPress += Ayarlar_KeyPress;
            panel2.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button İşyeriAyarları;
        private System.Windows.Forms.Button Kullanıcılar;
        private System.Windows.Forms.Button Muhataplar;
        private System.Windows.Forms.Button Yazdırma;
    }
}
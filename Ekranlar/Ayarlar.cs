using System;
using System.Windows.Forms;

namespace Gelir_Gider_Takip.Ekranlar
{
    public partial class Ayarlar : Form
    {
        public Ayarlar()
        {
            InitializeComponent();

            Kullanıcılar.Visible = AnaKontrolcü.İlkAçılışAyarları == null;
        }
        private void Ayarlar_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape) Close();
        }

        private void İşyeriAyarları_Click(object sender, EventArgs e)
        {

        }
        private void Muhataplar_Click(object sender, System.EventArgs e)
        {
            Önyüz.Aç(new Ayarlar_Muhataplar());
        }
        private void Kullanıcılar_Click(object sender, EventArgs e)
        {
            Önyüz.Aç(ArgeMup.HazirKod.Ekranlar.Kullanıcılar.Önyüz_Ayarlar(Font.Size));
        }
        private void Yazdırma_Click(object sender, EventArgs e)
        {
            Önyüz.Aç(new Ayarlar_Yazdırma(true));
        }
    }
}

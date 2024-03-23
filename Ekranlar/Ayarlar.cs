using System;
using System.Windows.Forms;

namespace Gelir_Gider_Takip.Ekranlar
{
    public partial class Ayarlar : Form
    {
        public Ayarlar()
        {
            InitializeComponent();

            Kullanıcılar.Visible = Önyüz.İlkAçılışAyarları == null;
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
            Önyüz.Aç(new Ayarlar_Kullanıcılar(ArgeMup.HazirKod.Ekranlar.Kullanıcılar2.İşlemTürü_.Ayarlar));
        }
        private void Yazdırma_Click(object sender, EventArgs e)
        {
            Önyüz.Aç(new Ayarlar_Yazdırma(true));
        }
    }
}

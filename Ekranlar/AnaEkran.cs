using System;
using System.Drawing;
using System.Windows.Forms;

namespace Gelir_Gider_Takip.Ekranlar
{
    public partial class AnaEkran : Form
    {
        public AnaEkran()
        {
            InitializeComponent();

            Önyüz.AnaEkran = this;

            if (AnaKontrolcü.YanUygulamaOlarakÇalışıyor)
            {
                switch (AnaKontrolcü.Şube_Talep.Kullanıcı_Komut)
                {
                    case AnaKontrolcü.Şube_Talep_Komut_.Sayfa_Ayarlar:
                        Önyüz.Aç(new Ayarlar());
                        break;

                    case AnaKontrolcü.Şube_Talep_Komut_.Sayfa_CariDöküm:
                        Önyüz.Aç(new Cari_Döküm());
                        break;

                    case AnaKontrolcü.Şube_Talep_Komut_.Sayfa_GelirGiderEkle:
                        Rectangle rr = Screen.PrimaryScreen.WorkingArea;
                        Width = rr.Width / 2;
                        Height = rr.Height / 2;
                        Left = rr.Width / 4;
                        Top = rr.Height / 4;
                        WindowState = FormWindowState.Normal;
                        Önyüz.Aç(new GelirGider_Ekle());
                        break;
                }
            }
            else Önyüz.Aç(new Açılış_Ekranı());
        }
        private void AnaEkran_Load(object sender, EventArgs e)
        {
            Icon = Properties.Resources.ico;
        }
        private void AnaEkran_Resize(object sender, System.EventArgs e)
        {
            Form ekran = sender as Form;

            if (ekran.IsDisposed || ekran.Disposing) return;

            if (WindowState == FormWindowState.Minimized)
            {
                if (AnaKontrolcü.YanUygulamaOlarakÇalışıyor) Önyüz.PencereleriKapat();
                else
                {
                    Banka_Ortak.Yedekle_Tümü();

                    if (AnaKontrolcü.ParolaKontrolüGerekiyorMu)
                    {
                        //şifre sayfasını aç
                        AnaKontrolcü.GirişYap(true);
                    }
                }
            }
        }
    }
}
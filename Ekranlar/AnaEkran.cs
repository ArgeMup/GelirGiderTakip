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
            bool YeniYazılımKontrolünüYap = false;

            if (AnaKontrolcü.İlkAçılışAyarları == null)
            {
                YeniYazılımKontrolünüYap = true;
                Önyüz.Aç(new Açılış_Ekranı());
            }
            else
            {
                switch (AnaKontrolcü.İlkAçılışAyarları.Kullanıcı_Komut)
                {
                    case AnaKontrolcü.İlkAçılışAyarları_Komut_.Sayfa_Ayarlar:
                        YeniYazılımKontrolünüYap = true;
                        Önyüz.Aç(new Ayarlar());
                        break;

                    case AnaKontrolcü.İlkAçılışAyarları_Komut_.Sayfa_CariDöküm:
                        YeniYazılımKontrolünüYap = true;
                        Önyüz.Aç(new Cari_Döküm());
                        break;

                    case AnaKontrolcü.İlkAçılışAyarları_Komut_.Sayfa_GelirGiderEkle:
                        Rectangle rr = Screen.PrimaryScreen.WorkingArea;
                        Width = rr.Width / 2;
                        Height = rr.Height / 2;
                        Left = rr.Width / 4;
                        Top = rr.Height / 4;
                        WindowState = FormWindowState.Normal;
                        Önyüz.Aç(new GelirGider_Ekle());
                        break;

                    default: throw new Exception("İlkAçılışAyarları.Kullanıcı_Komut (" + AnaKontrolcü.İlkAçılışAyarları.Kullanıcı_Komut + ")");
                }
            }

#if DEBUG || RELEASE
            Ortak.YeniYazılımKontrolü.Durdur();
#else
            if (YeniYazılımKontrolünüYap)
            {
                if (!System.IO.File.Exists(Ortak.Klasör_KullanıcıDosyaları + "YeniSurumuKontrolEtme.txt"))
                {
                    Ortak.YeniYazılımKontrolü.Başlat(new Uri("https://github.com/ArgeMup/GelirGiderTakip/blob/main/bin/Yay%C4%B1nla/Gelir%20Gider%20Takip.exe?raw=true"), _YeniYazılımKontrolü_GeriBildirim_);

                    void _YeniYazılımKontrolü_GeriBildirim_(bool Sonuç, string Açıklama)
                    {
                        if (Açıklama.Contains("github")) Önyüz.SürümKontrolMesajı = "Bağlantı kurulamadı";
                        else if (Açıklama == "Durduruldu") return;
                        else Önyüz.SürümKontrolMesajı = Açıklama;

                        Invoke(new Action(() =>
                        {
                            Text = Text.Replace("Yeni sürüm kontrol ediliyor", Önyüz.SürümKontrolMesajı);
                        }));
                    }
                }
                else Ortak.YeniYazılımKontrolü.Durdur();
            }
#endif  
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
                if (AnaKontrolcü.İlkAçılışAyarları != null) Application.Exit();
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
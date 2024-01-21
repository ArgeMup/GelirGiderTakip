using Gelir_Gider_Takip.Ekranlar;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Gelir_Gider_Takip
{
    public partial class AnaEkran : Form
    {
        DateTime SonHarekenAnı = default;

        public AnaEkran()
        {
            InitializeComponent();

            Önyüz.AnaEkran = this;
            bool YeniYazılımKontrolünüYap = false;

            if (Önyüz.İlkAçılışAyarları == null)
            {
                Activated += AnaEkran_Activated;
                Deactivate += AnaEkran_Deactivate;
                Shown += AnaEkran_Shown;
                YeniYazılımKontrolünüYap = true;
            }
            else
            {
                switch (Önyüz.İlkAçılışAyarları.Kullanıcı_Komut)
                {
                    case Banka1.İlkAçılışAyarları_Komut_.Sayfa_Ayarlar:
                        YeniYazılımKontrolünüYap = true;
                        Önyüz.Aç(new Ayarlar());
                        break;

                    case Banka1.İlkAçılışAyarları_Komut_.Sayfa_CariDöküm:
                        YeniYazılımKontrolünüYap = true;
                        Önyüz.Aç(new Cari_Döküm());
                        break;

                    case Banka1.İlkAçılışAyarları_Komut_.Sayfa_GelirGiderEkle:
                        Rectangle rr = Screen.PrimaryScreen.WorkingArea;
                        Width = rr.Width / 2;
                        Height = rr.Height / 2;
                        Left = rr.Width / 4;
                        Top = rr.Height / 4;
                        WindowState = FormWindowState.Normal;
                        Önyüz.Aç(new GelirGider_Ekle());
                        break;

                    default: throw new Exception("İlkAçılışAyarları.Kullanıcı_Komut (" + Önyüz.İlkAçılışAyarları.Kullanıcı_Komut + ")");
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
        private void AnaEkran_Shown(object sender, EventArgs e)
        {
            if (Ortak.Banka.Ayarlar.Kullanıcılar.ParolaKontrolüGerekiyorMu)
            {
                if (!Önyüz.Öndeki_ParolaGirişEkranıMı && (DateTime.Now - SonHarekenAnı).TotalSeconds > 15)
                {
                    Ayarlar_Kullanıcılar ekran = new Ayarlar_Kullanıcılar(ArgeMup.HazirKod.Ekranlar.Kullanıcılar.İşlemTürü_.Giriş);
                    ekran.FormClosed += _Ekran_FormClosed_;
                    Önyüz.Aç(ekran);

                    void _Ekran_FormClosed_(object? sender, FormClosedEventArgs e)
                    {
                        if (!Ortak.Banka.İzinliMi(Banka1.Ayarlar_Kullanıcılar_İzin.Boşta_)) return;

                        Önyüz.Aç(new Açılış_Ekranı());
                    }
                }
            }
            else if (SonHarekenAnı == default)
            {
                SonHarekenAnı = DateTime.Now.AddDays(-1);
                Önyüz.Aç(new Açılış_Ekranı());
            }
        }
        private void AnaEkran_Deactivate(object sender, EventArgs e)
        {
            SonHarekenAnı = DateTime.Now;
        }
        private void AnaEkran_Activated(object sender, System.EventArgs e)
        {
            AnaEkran_Shown(null, null);
        }
        private void AnaEkran_Resize(object sender, System.EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                if (Önyüz.İlkAçılışAyarları != null) Application.Exit();
                else
                {
                    SonHarekenAnı = DateTime.Now.AddDays(-1);
                    AnaEkran_Shown(null, null);
                }
            }
        }
        private void AnaEkran_FormClosed(object sender, FormClosedEventArgs e)
        {
            Ortak.Kapan(e.CloseReason.ToString());
        }
    }
}
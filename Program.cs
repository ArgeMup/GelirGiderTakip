using ArgeMup.HazirKod;
using ArgeMup.HazirKod.Ekİşlemler;
using Gelir_Gider_Takip.Ekranlar;
using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace Gelir_Gider_Takip
{
    internal static class Program
    {
        static UygulamaOncedenCalistirildiMi_ UyÖnÇa;
        static bool İstisnaOluştu = false;

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] BaşlangıçParametreleri)
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.

            Günlük.Başlat(Kendi.Klasörü + "\\Günlük");

            UyÖnÇa = new UygulamaOncedenCalistirildiMi_();
            if (UyÖnÇa.KontrolEt())
            {
                Günlük.Ekle("DiğerUygulamayıKapat", Hemen: true);
                UyÖnÇa.DiğerUygulamayıKapat(true);
            }
 
            Application.ThreadException += new ThreadExceptionEventHandler(BeklenmeyenDurum_KullanıcıArayüzü);
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(BeklenmeyenDurum_Uygulama);

#if DEBUG || RELEASE
            BaşlangıçParametreleri = new string[] { "Komut.mup" };
#endif
            
            if (BaşlangıçParametreleri != null && BaşlangıçParametreleri.Length == 1 && File.Exists(BaşlangıçParametreleri[0]))
            {
                string prl = 7578575578 + " 9985853 ArGeMuP " + 6 + " Gelir ŞemsiPaşa " + 587383678757 + " Gider " + 4 + "Pasajı " + 8689686868 + " Takip Uygulaması 55558368387 " + 29719542172;
                string içerik = BaşlangıçParametreleri[0].DosyaYolu_Oku_BaytDizisi().Düzelt(prl.BaytDizisine()).Yazıya();
                if (içerik.DoluMu(true))
                {
                    Depo_ depo = new Depo_(içerik);
                    Değişken_ değişken = new Değişken_();
                    AnaKontrolcü.İlkAçılışAyarları = (AnaKontrolcü.İlkAçılışAyarları_)değişken.Üret(typeof(AnaKontrolcü.İlkAçılışAyarları_), depo["ArGeMuP"]);
                }
            }

            AnaKontrolcü.Açıl();

            if (AnaKontrolcü.İlkAçılışAyarları != null)
            {
                Banka_Ortak.Başlat();

                switch (AnaKontrolcü.İlkAçılışAyarları.Kullanıcı_Komut)
                {
                    case AnaKontrolcü.İlkAçılışAyarları_Komut_.Ekle_GelirGider:
                        if (AnaKontrolcü.İlkAçılışAyarları.Ekle_GelirGider_Talepler != null && AnaKontrolcü.İlkAçılışAyarları.Ekle_GelirGider_Talepler.Count > 0)
                        {
                            foreach (var Talep in AnaKontrolcü.İlkAçılışAyarları.Ekle_GelirGider_Talepler)
                            {
                                if (Talep.Ekle_Miktar < 0) throw new Exception("Miktar 0 dan küçük olamaz " + Talep.Ekle_MuhatapGrubuAdı + " " + Talep.Ekle_MuhatapAdı + " " + Talep.Ekle_Miktar);

                                Banka1.İşyeri_Ödeme_ ödeme = Ortak.Banka.Seçilenİşyeri.Ödemeler_Bul(Talep.Ekle_MuhatapGrubuAdı, Talep.Ekle_MuhatapAdı, Talep.Ekle_KayıtTarihi.Value);
                                if (ödeme != null) ödeme.YeniİşlemEkle(Talep.Ekle_Tipi, Talep.Ekle_Durumu, Talep.Ekle_Miktar, Talep.Ekle_Notlar, DateOnly.FromDateTime(Talep.Ekle_İlkÖdemeTarihi));
                                else
                                {
                                    var muhatap = Ortak.Banka.Seçilenİşyeri.Muhatap_Aç(Talep.Ekle_MuhatapGrubuAdı, Talep.Ekle_MuhatapAdı, true);
                                    if (muhatap == null) throw new Exception("Muhatap bulunamadı " + Talep.Ekle_MuhatapGrubuAdı + " " + Talep.Ekle_MuhatapAdı);

                                    var gelir_gider = muhatap.GelirGider_Oluştur(Talep.Ekle_Tipi, Talep.Ekle_Durumu, Talep.Ekle_Miktar, Talep.Ekle_ParaBirimi,
                                        Talep.Ekle_İlkÖdemeTarihi, Talep.Ekle_Notlar,
                                        Talep.Ekle_Taksit_Sayısı, Talep.Ekle_Taksit_Dönem, Talep.Ekle_Taksit_Dönem_Adet,
                                        null, null, Talep.Ekle_KayıtTarihi);
                                    muhatap.GelirGider_Ekle(gelir_gider);
                                }
                            }

                            Banka_Ortak.DeğişiklikleriKaydet();
                        }
                        Açıkla(null);
                        return;

                    case AnaKontrolcü.İlkAçılışAyarları_Komut_.Yazdır:
                        var cd = new Cari_Döküm(Cari_Döküm.AçılışTürü_.Gizli);

                        if (AnaKontrolcü.İlkAçılışAyarları.Kullanıcı_Komut_EkTanım == null || AnaKontrolcü.İlkAçılışAyarları.Kullanıcı_Komut_EkTanım.Length < 1) throw new Exception("if (AçılışDetayları == null || AçılışDetayları.Length < 1)");
                        string şablon = AnaKontrolcü.İlkAçılışAyarları.Kullanıcı_Komut_EkTanım.Length >= 2 ? AnaKontrolcü.İlkAçılışAyarları.Kullanıcı_Komut_EkTanım[1] : null;
                        string sonuç = cd.Şablon_Seç_TabloyuOluştur(şablon);

                        if (sonuç.DoluMu()) Açıkla(sonuç);
                        else if (cd.Tablo.RowCount == 0) Açıkla("Hiç kayıt bulunamadı");
                        else
                        {
                            Ayarlar_Yazdırma yzdrm = new Ayarlar_Yazdırma();
                            yzdrm.Yazdır(cd.Tablo, AnaKontrolcü.İlkAçılışAyarları.Kullanıcı_Komut_EkTanım[0]);

                            Açıkla(null);
                        }
                        return;
                }
            }

            ApplicationConfiguration.Initialize();
            AnaKontrolcü.GirişYap(false);
            Application.Run(new Form() { Opacity = 0, ShowInTaskbar = false, Visible = false });

            AnaKontrolcü.Kapan("Normal");
            if (!İstisnaOluştu) Açıkla(null);
        }

        static void BeklenmeyenDurum_Uygulama(object sender, UnhandledExceptionEventArgs e)
        {
            AppDomain.CurrentDomain.UnhandledException -= BeklenmeyenDurum_Uygulama;
            BeklenmeyenDurum((Exception)e.ExceptionObject);
        }
        static void BeklenmeyenDurum_KullanıcıArayüzü(object sender, ThreadExceptionEventArgs t)
        {
            Application.ThreadException -= BeklenmeyenDurum_KullanıcıArayüzü;
            BeklenmeyenDurum(t.Exception);
        }
        static void BeklenmeyenDurum(Exception ex)
        {
            İstisnaOluştu = true;
            ex.Günlük(Hemen: true);

            try
            {
                Banka_Ortak.Yedekle_Banka_Kurtar();

                string hata = "Bir sorun oluştu, uygulama yedekler ile kontrol edildi ve bir sorun görülmedi." + Environment.NewLine + Environment.NewLine +
                        "Uygulama kapatılacak." + Environment.NewLine + Environment.NewLine +
                        "Lütfen son işleminizi kontrol ediniz." + Environment.NewLine + Environment.NewLine +
                        ex.Message;

                Açıkla(hata);
                AnaKontrolcü.Kapan("BeklenmeyenDurum");
            }
            catch (Exception exxx)
            {
                exxx.Günlük(Hemen: true);

                string hata = "BÜYÜK bir SORUN oluştu, dosyalarınız KULLANILAMAZ durumda olabilir." + Environment.NewLine + Environment.NewLine +
                    "Uygulama kapatılacak. Lütfen son işleminizi kontrol ediniz." + Environment.NewLine + Environment.NewLine +
                    ex.Message + Environment.NewLine + Environment.NewLine +
                    "Üstteki hata mesajını üst üste 3. kez görüyorsanız şunları deneyebilirsiniz." + Environment.NewLine +
                    "1. Uygulamayı kapatıp BANKA klasörü içeriğini tümüyle silin." + Environment.NewLine +
                    "2. YEDEK klasöründeki en yeni yedeği (zip dosyası) BANKA klasörü içerisine çıkartın.";

                Açıkla(hata);
            }
            finally
            {
                Application.Exit();
            }
        }
        static void Açıkla(string HatanınAçıklaması)
        {
            if (AnaKontrolcü.İlkAçılışAyarları == null)
            {
                //tıklanarak açıldı
                if (HatanınAçıklaması != null) MessageBox.Show(HatanınAçıklaması, "Gelir Gider Takip");
            }
            else
            {
                //komut satırından açıldı
                if (HatanınAçıklaması == null) HatanınAçıklaması = "Tamam " + AnaKontrolcü.İlkAçılışAyarları?.Benzersiz_Tanımlayıcı; //hata yok

                HatanınAçıklaması.Dosyaİçeriği_Yaz(Kendi.Klasörü + "\\Sonuç.mup");
            }
        }
    }
}
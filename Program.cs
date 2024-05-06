using ArgeMup.HazirKod;
using ArgeMup.HazirKod.Ekİşlemler;
using System;
using System.Threading;
using System.Windows.Forms;

namespace Gelir_Gider_Takip
{
    internal static class Program
    {
        static UygulamaOncedenCalistirildiMi_ UyÖnÇa;

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
            ApplicationConfiguration.Initialize();

#if DEBUG
            Günlük.GenelSeviye = Günlük.Seviye.HazirKod;
            //BaşlangıçParametreleri = new string[] { "5555" };
#endif
            if (BaşlangıçParametreleri == null || BaşlangıçParametreleri.Length != 1 || !int.TryParse(BaşlangıçParametreleri[0], out int ŞubeErişimNoktası)) ŞubeErişimNoktası = 0;
            AnaKontrolcü.Açıl(ŞubeErişimNoktası);

            AnaKontrolcü.BoştaBekleyenAnaUygulama = new Form() { Opacity = 0, ShowInTaskbar = false, Visible = false };
            Application.Run(AnaKontrolcü.BoştaBekleyenAnaUygulama);

            AnaKontrolcü.Kapan("Normal");
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
            ex.Günlük(Hemen: true);

            try
            {
                bool işlem_yapıldı = Banka_Ortak.Yedekle_SürümYükseltmeÖncesiYedeği_Kurtar();
                if (!işlem_yapıldı) Banka_Ortak.Yedekle_Banka_Kurtar();

                string hata = "Bir sorun oluştu, uygulama kapatılacak." + Environment.NewLine + Environment.NewLine +
                        "Lütfen son işleminizi kontrol ediniz." + Environment.NewLine + Environment.NewLine +
                        ex.Message;

                MessageBox.Show(hata, "Gelir Gider Takip");
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

                MessageBox.Show(hata, "Gelir Gider Takip");
            }
            finally
            {
                Application.Exit();
            }
        }
    }
}
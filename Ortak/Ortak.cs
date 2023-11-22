using ArgeMup.HazirKod;
using ArgeMup.HazirKod.Ekİşlemler;
using Gelir_Gider_Takip.Ekranlar;
using System;
using System.Drawing;
using System.IO;

namespace Gelir_Gider_Takip
{
    public static class Ortak
    {
        public static string Klasör_Banka;
        public static string Klasör_Banka2;
        public static string Klasör_İçYedek;
        public static string Klasör_KullanıcıDosyaları;
        public static string Klasör_Gecici = Klasör.Depolama(Klasör.Kapsamı.Geçici, Uygulama:Kendi.Adı + "_Geçici", Sürüm: "") + "\\";
        public static Bitmap Firma_Logo;
        public static Banka1 Banka;
        public static YeniYazılımKontrolü_ YeniYazılımKontrolü = new YeniYazılımKontrolü_();
        public static Çalıştır_ Çalıştır = new Çalıştır_();

        public static Color Renk_Kırmızı = Color.FromArgb(int.Parse("FFEF9595", System.Globalization.NumberStyles.HexNumber));
        public static Color Renk_Sarı = Color.FromArgb(int.Parse("FFEBEF95", System.Globalization.NumberStyles.HexNumber));
        public static Color Renk_Yeşil = Color.FromArgb(int.Parse("FFD0F0C0", System.Globalization.NumberStyles.HexNumber));
        public static Color Renk_Gri = Color.FromArgb(int.Parse("FFD9D9D9", System.Globalization.NumberStyles.HexNumber));
        public static Color Renk_Gelir = Renk_Yeşil;
        public static Color Renk_Gider = Renk_Kırmızı;
        public static Color Renk_Mavi = Color.FromArgb(int.Parse("FFE0FFFF", System.Globalization.NumberStyles.HexNumber));

        public static void Başlat()
        {
            string AnaKlasör = null;
            if (Önyüz.İlkAçılışAyarları != null)
            {
                if (Önyüz.İlkAçılışAyarları.İşyeri_Adı.BoşMu(true)) throw new Exception("if (Önyüz.İlkAçılışAyarları.İşyeri_Adı.BoşMu(true))");

                if (Önyüz.İlkAçılışAyarları.KayıtKlasörü.BoşMu()) throw new System.Exception("if (Önyüz.İlkAçılışAyarları.KayıtKlasörü.BoşMu())");
                AnaKlasör = Önyüz.İlkAçılışAyarları.KayıtKlasörü.TrimEnd('\\');

                if (Önyüz.İlkAçılışAyarları.İşyeri_LogoDosyaYolu.DoluMu() && File.Exists(Önyüz.İlkAçılışAyarları.İşyeri_LogoDosyaYolu)) Firma_Logo = new Bitmap(Önyüz.İlkAçılışAyarları.İşyeri_LogoDosyaYolu);

                //Üst uygulamanın parolasını kullan
                if (Önyüz.İlkAçılışAyarları.İşyeri_Parolası.DoluMu())
                {
                    Parola.Yazı += Önyüz.İlkAçılışAyarları.İşyeri_Parolası;
                    Parola.Dizi = Parola.Yazı.BaytDizisine();
                }
            }

            if (AnaKlasör == null) AnaKlasör = Kendi.Klasörü;
            if (Firma_Logo == null) Firma_Logo = Properties.Resources.logo_512_seffaf;

            Klasör_Banka = AnaKlasör + "\\Banka\\";
            Klasör_Banka2 = AnaKlasör + "\\Banka2\\";
            Klasör_İçYedek = AnaKlasör + "\\Yedek\\";
            Klasör_KullanıcıDosyaları = AnaKlasör + "\\Kullanıcı Dosyaları\\";

            Banka_Ortak.Başlat();
        }
        public static void Kapan(string Bilgi)
        {
            Günlük.Ekle("Kapatıldı " + Bilgi, Hemen: true);
            Banka_Ortak.Yedekle_Tümü();

            YeniYazılımKontrolü.Durdur();
            Ekranlar.Önyüz.Durdur();
            Çalıştır.Dispose();
            Klasör.Sil(Klasör_Gecici);

            ArgeMup.HazirKod.ArkaPlan.Ortak.Çalışsın = false;
        }

        //İşyeri Adı        | null
        //Muhatap Grup Adı  | Muhatap Adı
        public static void Seçtirt(SeçimVeDüzenleme_Ekranı.Türü_ Türü, Action<string, string> GeriBildirimİşlemi)
        {
            SeçimVeDüzenleme_Ekranı Seçtirme = new SeçimVeDüzenleme_Ekranı(Türü, GeriBildirimİşlemi);
            Önyüz.Aç(Seçtirme);
        }

        public static bool Klasör_TamKopya(string Kaynak, string Hedef, bool DoğrulamaKodunuKontrolEt_Yavaşlatır = true)
        {
            int ZamanAşımı_msn = Environment.TickCount + 15000;
            while (ZamanAşımı_msn > Environment.TickCount)
            {
                try
                {
                    if (Klasör.Kopyala(Kaynak, Hedef, true, DoğrulamaKodunuKontrolEt_Yavaşlatır)) return true;
                }
                catch (Exception ex) { ex.Günlük(); }

                System.Threading.Thread.Sleep(100);
            }

            return false;
        }
    }

    public static class Döviz
    {
        static DateTime EnSonGüncelleme = DateTime.MinValue;
        static string Çıktı_yazı = null;
        static string[] Çıktı_dizi = null; //tcmb dolar avro, diğer dolar avro

        public static void KurlarıAl(Action<string, string[]> İşlem)
        {
            System.Threading.Tasks.Task.Run(() =>
            {
                if ((DateTime.Now - EnSonGüncelleme).TotalMinutes > 5 || Çıktı_yazı.BoşMu() || Çıktı_yazı.Split(new string[] { "Okunamadı" }, StringSplitOptions.None).Length > 2)
                {
                    Çıktı_yazı = null;
                    Çıktı_dizi = new string[] { "-1", "-1", "-1", "-1" };

                    string Dosya_TCMB = Ortak.Klasör_Gecici + "TCMB_Kurlar.xml";
                    Dosya.Sil(Dosya_TCMB);
                    YeniYazılımKontrolü_ Kaynak_TCMB = new YeniYazılımKontrolü_();
                    Kaynak_TCMB.Başlat(new Uri("https://www.tcmb.gov.tr/kurlar/today.xml"), HedefDosyaYolu: Dosya_TCMB);

                    string Dosya_GenelPara = Ortak.Klasör_Gecici + "GenelPara_Kurlar.xml";
                    Dosya.Sil(Dosya_GenelPara);
                    YeniYazılımKontrolü_ Kaynak_GenelPara = new YeniYazılımKontrolü_();
                    Kaynak_GenelPara.Başlat(new Uri("https://api.genelpara.com/embed/doviz.json"), HedefDosyaYolu: Dosya_GenelPara);

                    int za = Environment.TickCount + 15000;
                    while (ArgeMup.HazirKod.ArkaPlan.Ortak.Çalışsın && za > Environment.TickCount &&
                    (!Kaynak_TCMB.KontrolTamamlandı || !Kaynak_GenelPara.KontrolTamamlandı)) System.Threading.Thread.Sleep(350);

                    if (File.Exists(Dosya_TCMB))
                    {
                        try
                        {
                            System.Xml.XmlDocument xmlVerisi = new System.Xml.XmlDocument();
                            xmlVerisi.LoadXml(Dosya_TCMB.DosyaYolu_Oku_Yazı());

                            string Tarih = xmlVerisi.SelectSingleNode("Tarih_Date").Attributes["Tarih"].InnerText;
                            string dolar = xmlVerisi.SelectSingleNode(string.Format("Tarih_Date/Currency[@Kod='{0}']/BanknoteSelling", "USD")).InnerText;
                            string avro = xmlVerisi.SelectSingleNode(string.Format("Tarih_Date/Currency[@Kod='{0}']/BanknoteSelling", "EUR")).InnerText;
                            Çıktı_yazı +=
                            "TCMB " + Tarih + " 15:30" + Environment.NewLine +
                            "Dolar = " + dolar + " ₺" + Environment.NewLine +
                            "Avro = " + avro + " ₺" + Environment.NewLine;

                            Çıktı_dizi[0] = dolar;
                            Çıktı_dizi[1] = avro;
                        }
                        catch (Exception) { }
                    }
                    else Çıktı_yazı += "TCMB Okunamadı" + Environment.NewLine;

                    if (File.Exists(Dosya_GenelPara))
                    {
                        try
                        {
                            string içerik = Dosya_GenelPara.DosyaYolu_Oku_Yazı();
                            string dolar = _Al_(içerik, @"""USD"":{""satis"":""", @"""");
                            string avro = _Al_(içerik, @"""EUR"":{""satis"":""", @"""");

                            Çıktı_yazı +=
                            "Diğer " + File.GetLastWriteTime(Dosya_GenelPara).Yazıya("dd.MM.yyyy HH:mm") + Environment.NewLine +
                            "Dolar = " + dolar + " ₺" + Environment.NewLine +
                            "Avro = " + avro + " ₺";

                            Çıktı_dizi[2] = dolar;
                            Çıktı_dizi[3] = avro;

                            string _Al_(string Girdi, string Başlangıç, string Bitiş)
                            {
                                int knm_başla = Girdi.IndexOf(Başlangıç);
                                if (knm_başla < 0) return null;

                                knm_başla += Başlangıç.Length;
                                int knm_bitir = Girdi.IndexOf(Bitiş, knm_başla);
                                if (knm_bitir < 0) return null;

                                return Girdi.Substring(knm_başla, knm_bitir - knm_başla);
                            }
                        }
                        catch (Exception) { }
                    }
                    else Çıktı_yazı += "Diğer Okunamadı";

                    EnSonGüncelleme = DateTime.Now;
                    Kaynak_TCMB.Dispose();
                    Kaynak_GenelPara.Dispose();
                }

                İşlem?.Invoke(Çıktı_yazı, Çıktı_dizi);
            });
        }
        /// <param name="İşlem">Action<Dolar, Avro> </param>
        public static void KurlarıAl(Action<double, double> İşlem)
        {
            KurlarıAl(_GeriBildirimİşlemi_DövizKuru_);

            void _GeriBildirimİşlemi_DövizKuru_(string Yazı, string[] Dizi)
            {
                double[] sayı_olarak = new double[Dizi.Length];
                for (int i = 0; i < Dizi.Length; i++) { sayı_olarak[i] = Dizi[i].NoktalıSayıya(); }

                //[0] TCMB Dolar, Avro
                //[2] Diğer Dolar, Avro

                İşlem(sayı_olarak[0] > sayı_olarak[2] ? sayı_olarak[0] : sayı_olarak[2], sayı_olarak[1] > sayı_olarak[3] ? sayı_olarak[1] : sayı_olarak[3]);
            }
        }
    }
}

using ArgeMup.HazirKod;
using ArgeMup.HazirKod.Ekİşlemler;
using Gelir_Gider_Takip.Ekranlar;
using System;
using System.Collections.Generic;
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
        public static YeniYazılımKontrolü_ YeniYazılımKontrolü;
        public static Çalıştır_ Çalıştır = new Çalıştır_();
        public static string Sistem_KullanıcıAdı = "V" + Kendi.Sürümü_Dosya;
        public static string GizliElemanBaşlangıcı = ".:Gizli:. ";

        public static Color Renk_Kırmızı = Color.FromArgb(int.Parse("FFEF9595", System.Globalization.NumberStyles.HexNumber));
        public static Color Renk_Sarı = Color.FromArgb(int.Parse("FFEBEF95", System.Globalization.NumberStyles.HexNumber));
        public static Color Renk_Yeşil = Color.FromArgb(int.Parse("FFD0F0C0", System.Globalization.NumberStyles.HexNumber));
        public static Color Renk_Gri = Color.FromArgb(int.Parse("FFD9D9D9", System.Globalization.NumberStyles.HexNumber));
        public static Color Renk_Mavi = Color.FromArgb(int.Parse("FFE0FFFF", System.Globalization.NumberStyles.HexNumber));
        public static Color Renk_Gelir = Renk_Yeşil;
        public static Color Renk_Gider = Renk_Kırmızı;
        public static Color Renk_KontrolNoktası = Renk_Mavi;

        //İşyeri Adı        | null
        //Muhatap Grup Adı  | Muhatap Adı
        public static void Seçtirt(SeçimVeDüzenleme_Ekranı.Türü_ Türü, Action<string, string> GeriBildirimİşlemi)
        {
            SeçimVeDüzenleme_Ekranı Seçtirme = new SeçimVeDüzenleme_Ekranı(Türü, GeriBildirimİşlemi);
            Önyüz.Aç(Seçtirme);
        }

        public static bool Klasör_TamKopya(string Kaynak, string Hedef, bool DoğrulamaKodunuKontrolEt_Yavaşlatır = true, bool AynıDoğrulamaKodunaSahipİse_DiğerFarklılıklarıGörmezdenGel = false)
        {
            int ZamanAşımı_msn = Environment.TickCount + 15000;
            while (ZamanAşımı_msn > Environment.TickCount)
            {
                try
                {
                    if (Directory.Exists(Kaynak))
                    {
                        if (Klasör.Kopyala(Kaynak, Hedef, true, DoğrulamaKodunuKontrolEt_Yavaşlatır, AynıDoğrulamaKodunaSahipİse_DiğerFarklılıklarıGörmezdenGel: AynıDoğrulamaKodunaSahipİse_DiğerFarklılıklarıGörmezdenGel)) return true;
                    }
                    else
                    {
                        Klasör.Sil(Hedef);
                        return true;
                    }
                }
                catch (Exception ex) { ex.Günlük(); }

                System.Threading.Thread.Sleep(100);
            }

            return false;
        }

        public static double ParaBirimi_Dönüştür(double Miktar, Banka1.İşyeri_Ödeme_.ParaBirimi_ Girdi, Banka1.İşyeri_Ödeme_.ParaBirimi_ Çıktı)
        {
            if (Girdi == Çıktı) return Miktar;

            double Avro, Dolar;
            if (!Döviz.Oku(out Avro, out Dolar))
            {
                int za = Environment.TickCount + 15000;
                bool Bitti = false;

                Döviz.KurlarıAl(_GeriBildirimİşlemi_DövizKurları_);
                void _GeriBildirimİşlemi_DövizKurları_(double _Avro_, double _Dolar_)
                {
                    Avro = _Avro_;
                    Dolar = _Dolar_;
                    Bitti = true;
                }

                while (ArgeMup.HazirKod.ArkaPlan.Ortak.Çalışsın && !Bitti && za > Environment.TickCount)
                {
                    System.Threading.Thread.Sleep(35);
                    System.Windows.Forms.Application.DoEvents();
                }

                if (!Bitti) return -1;
            }

            //önce TL yap
            switch (Girdi)
            {
                case Banka1.İşyeri_Ödeme_.ParaBirimi_.TürkLirası:                       break;
                case Banka1.İşyeri_Ödeme_.ParaBirimi_.Avro:         Miktar *= Avro;     break;
                case Banka1.İşyeri_Ödeme_.ParaBirimi_.Dolar:        Miktar *= Dolar;    break;

                default: throw new Exception("Girdi para birimi (" + Girdi + ") uygun değil");
            }

            //dönüştür
            switch (Çıktı)
            {
                case Banka1.İşyeri_Ödeme_.ParaBirimi_.TürkLirası:                       break;
                case Banka1.İşyeri_Ödeme_.ParaBirimi_.Avro:         Miktar /= Avro;     break;
                case Banka1.İşyeri_Ödeme_.ParaBirimi_.Dolar:        Miktar /= Dolar;    break;

                default: throw new Exception("Çıktı para birimi (" + Çıktı + ") uygun değil");
            }

            return Miktar;
        }
    }

    public static class Döviz
    {
        static DateTime EnSonGüncelleme = DateTime.MinValue;
        static string Çıktı_yazı = null;
        static double[] Çıktı_dizi = null; //tcmb dolar avro, diğer dolar avro
        static double _Avro, _Dolar;

        /// <param name="İşlem">Action<Avro, Dolar> </param>
        public static void KurlarıAl(Action<double, double> İşlem)
        {
            System.Threading.Tasks.Task.Run(() =>
            {
                if ((DateTime.Now - EnSonGüncelleme).TotalMinutes > 5 || Çıktı_yazı.BoşMu() || Çıktı_yazı.Split(new string[] { "Okunamadı" }, StringSplitOptions.None).Length > 2)
                {
                    Çıktı_yazı = null;
                    Çıktı_dizi = new double[] { -1, -1, -1, -1 };

                    Dosya.AğÜzerinde_ dsy_tcmb = new Dosya.AğÜzerinde_(new Uri("https://www.tcmb.gov.tr/kurlar/today.xml"), null);
                    Dosya.AğÜzerinde_ dsy_diğer = new Dosya.AğÜzerinde_(new Uri("https://api.coinbase.com/v2/exchange-rates?currency=TRY"), null);

                    int za = Environment.TickCount + 16000;
                    while (ArgeMup.HazirKod.ArkaPlan.Ortak.Çalışsın && za > Environment.TickCount && 
                            (!dsy_tcmb.KontrolTamamlandı || !dsy_diğer.KontrolTamamlandı)) System.Threading.Thread.Sleep(350);

                    if (dsy_tcmb.Sonuç)
                    {
                        try
                        {
                            System.Xml.XmlDocument xmlVerisi = new System.Xml.XmlDocument();
                            xmlVerisi.LoadXml((dsy_tcmb.HedefDosyaYolu_Veya_İçeriği as byte[]).Yazıya());

                            string Tarih = xmlVerisi.SelectSingleNode("Tarih_Date").Attributes["Tarih"].InnerText;
                            string dolar = xmlVerisi.SelectSingleNode(string.Format("Tarih_Date/Currency[@Kod='{0}']/BanknoteSelling", "USD")).InnerText;
                            string avro = xmlVerisi.SelectSingleNode(string.Format("Tarih_Date/Currency[@Kod='{0}']/BanknoteSelling", "EUR")).InnerText;
                            Çıktı_yazı +=
                            "TCMB " + Tarih + " 15:30" + Environment.NewLine +
                            "Dolar = " + dolar + " ₺" + Environment.NewLine +
                            "Avro = " + avro + " ₺" + Environment.NewLine;

                            Çıktı_dizi[0] = dolar.NoktalıSayıya();
                            Çıktı_dizi[1] = avro.NoktalıSayıya();
                        }
                        catch (Exception) { }
                    }
                    else Çıktı_yazı += "TCMB Okunamadı" + Environment.NewLine;

                    if (dsy_diğer.Sonuç)
                    {
                        try
                        {
                            string içerik = (dsy_diğer.HedefDosyaYolu_Veya_İçeriği as byte[]).Yazıya();
                            string dolar = _Al_(içerik, @"""USD"":""", @""",");
                            string avro = _Al_(içerik, @"""EUR"":""", @""",");

                            Çıktı_dizi[2] = 1.0 / dolar.NoktalıSayıya();
                            Çıktı_dizi[3] = 1.0 / avro.NoktalıSayıya();

                            Çıktı_yazı +=
                                "Diğer " + DateTime.Now.Yazıya("dd.MM.yyyy HH:mm") + Environment.NewLine +
                                "Dolar = " + Banka_Ortak.Yazdır_Ücret(Çıktı_dizi[2], Banka1.İşyeri_Ödeme_.ParaBirimi_.TürkLirası) + Environment.NewLine +
                                "Avro = " + Banka_Ortak.Yazdır_Ücret(Çıktı_dizi[3], Banka1.İşyeri_Ödeme_.ParaBirimi_.TürkLirası);

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
                    dsy_tcmb.Dispose();
                    dsy_diğer.Dispose();

                    //[0] TCMB Dolar, Avro
                    //[2] Diğer Dolar, Avro
                    _Dolar = Çıktı_dizi[0] > Çıktı_dizi[2] ? Çıktı_dizi[0] : Çıktı_dizi[2];
                    _Avro = Çıktı_dizi[1] > Çıktı_dizi[3] ? Çıktı_dizi[1] : Çıktı_dizi[3];
                }

                İşlem?.Invoke(_Avro, _Dolar);
            });
        }

        public static bool Oku(out double Avro, out double Dolar)
        {
            Avro = _Avro;
            Dolar = _Dolar;

            return (DateTime.Now - EnSonGüncelleme).TotalMinutes <= 5;
        }
    }
}

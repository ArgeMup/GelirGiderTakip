using ArgeMup.HazirKod;
using ArgeMup.HazirKod.Ekİşlemler;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Gelir_Gider_Takip
{
    public class AnaKontrolcü
    {
        public static İlkAçılışAyarları_ İlkAçılışAyarları;
        public static string KökParola
        {
            get
            {
                return ArgeMup.HazirKod.Ekranlar.Kullanıcılar.KökParola;
            }
            set
            {
                ArgeMup.HazirKod.Ekranlar.Kullanıcılar._Ayarlar_Üst_.KökParola = value;
                ArgeMup.HazirKod.Ekranlar.Kullanıcılar._Ayarlar_Üst_.KökParola_Dizi = value.BaytDizisine();
            }
        }
        public static byte[] KökParola_Dizi
        {
            get
            {
                return ArgeMup.HazirKod.Ekranlar.Kullanıcılar.KökParola_Dizi;
            }
        }
        public static bool ParolaKontrolüGerekiyorMu
        {
            get
            {
                return ArgeMup.HazirKod.Ekranlar.Kullanıcılar.ParolaKontrolüGerekiyorMu;
            }
            set
            {
                ArgeMup.HazirKod.Ekranlar.Kullanıcılar._Ayarlar_Üst_.ParolaKontrolüGerekiyorMu = value;
            }
        }
        public static string KullanıcıAdı
        {
            get
            {
                return ArgeMup.HazirKod.Ekranlar.Kullanıcılar.KullanıcıAdı;
            }
        }
        public static bool İzinliMi(Kullanıcılar_İzin İzin)
        {
            return ArgeMup.HazirKod.Ekranlar.Kullanıcılar.İzinliMi(İzin == Kullanıcılar_İzin.Boşta_ ? null : İzin);
        }
        public static bool İzinliMi(IEnumerable<Kullanıcılar_İzin> İzinler)
        {
            foreach (Kullanıcılar_İzin izin in İzinler)
            {
                if (İzinliMi(izin)) return true;
            }

            return false;
        }
        public static ArgeMup.HazirKod.Ekranlar.Kullanıcılar.Ayarlar_Üst_.Ayarlar_Kullanıcı_ GeçerliKullanıcı
        {
            get
            {
                return ArgeMup.HazirKod.Ekranlar.Kullanıcılar._Ayarlar_Üst_.GeçerliKullanıcı;
            }
            set
            {
                ArgeMup.HazirKod.Ekranlar.Kullanıcılar._Ayarlar_Üst_.GeçerliKullanıcı = value;
            }
        }

        public static void Açıl()
        {
            string AnaKlasör = null;
            ArgeMup.HazirKod.Ekranlar.Kullanıcılar._Ayarlar_Üst_ = new ArgeMup.HazirKod.Ekranlar.Kullanıcılar.Ayarlar_Üst_();
            KökParola = 7578575578 + " 9985853 ArGeMuP " + 6 + " Gelir ŞemsiPaşa " + 587383678757 + " Gider " + 4 + "Pasajı " + 8689686868 + " Takip Uygulaması 55558368387 " + 29719542172;

            if (İlkAçılışAyarları != null)
            {
                if (İlkAçılışAyarları.İşyeri_Adı.BoşMu(true)) throw new Exception("if (AnaKontrolcü.İlkAçılışAyarları.İşyeri_Adı.BoşMu(true))");

                if (İlkAçılışAyarları.KayıtKlasörü.BoşMu()) throw new System.Exception("if (AnaKontrolcü.İlkAçılışAyarları.KayıtKlasörü.BoşMu())");
                AnaKlasör = İlkAçılışAyarları.KayıtKlasörü.TrimEnd('\\');

                if (İlkAçılışAyarları.İşyeri_LogoDosyaYolu.DoluMu() && File.Exists(İlkAçılışAyarları.İşyeri_LogoDosyaYolu)) Ortak.Firma_Logo = new Bitmap(İlkAçılışAyarları.İşyeri_LogoDosyaYolu);

                //Üst uygulamanın parolasını kullan
                KökParola += İlkAçılışAyarları.İşyeri_Parolası;
            }
            else
            {
                AnaKlasör = Kendi.Klasörü;
                Ortak.Firma_Logo = Properties.Resources.logo_512_seffaf;
            }

            Ortak.Klasör_Banka = AnaKlasör + "\\Banka\\";
            Ortak.Klasör_Banka2 = AnaKlasör + "\\Banka2\\";
            Ortak.Klasör_İçYedek = AnaKlasör + "\\Yedek\\";
            Ortak.Klasör_KullanıcıDosyaları = AnaKlasör + "\\Kullanıcı Dosyaları\\";

            Klasör.Oluştur(Ortak.Klasör_Banka);
            Klasör.Oluştur(Ortak.Klasör_İçYedek);
            Klasör.Oluştur(Ortak.Klasör_KullanıcıDosyaları);
            Klasör.Oluştur(Ortak.Klasör_Gecici);

            Banka_Ortak.Yedekle_SürümYükseltmeÖncesiYedeği_Kurtar();

            //sürüm geçişi sonrası silinecek
            if (!File.Exists(Ortak.Klasör_Banka + @"ArgeMup.HazirKod_Cdiyez.Ekranlar.Kullanıcılar.Ayarlar") && Klasör.Listele_Dosya(Ortak.Klasör_Banka).Length > 0)
            {
                DoğrulamaKodu.KontrolEt.Durum_ snç = DoğrulamaKodu.KontrolEt.Klasör(Ortak.Klasör_Banka, SearchOption.AllDirectories, KökParola);
                ("Bütünlük Kontrolü Sürüm Geçişi " + snç).Günlük();
                if (snç != DoğrulamaKodu.KontrolEt.Durum_.Aynı)
                {
#if DEBUG
                        return;
#else
                    throw new Exception("Bütünlük Kontrolü Sürüm Geçişi " + snç);
#endif
                }

                //yeni sürüme geçirme
                Banka_Ortak.Yedekle_SürümYükseltmeÖncesiYedeği();
                Günlük.Ekle("Banka yeni sürüme geçirme aşama 1 tamam");

                DahaCokKarmasiklastirma_ DaÇoKa = new DahaCokKarmasiklastirma_();
                foreach (string DosyaYolu in Directory.GetFiles(Ortak.Klasör_Banka, "*.mup", SearchOption.AllDirectories))
                {
                    _Düzelt_Aç_(DosyaYolu);
                }
                Günlük.Ekle("Banka yeni sürüme geçirme aşama 2 tamam");
                void _Düzelt_Aç_(string _DosyaYolu_)
                {
                    byte[] _İçerik_ = _DosyaYolu_.DosyaYolu_Oku_BaytDizisi();
                    if (_İçerik_ == null || _İçerik_.Length == 0) throw new Exception("_İçerik_ == null || _İçerik_.Length == 0");

                    //Şifre çözme
                    byte[] _çıktı_ = DaÇoKa.Düzelt(_İçerik_, KökParola_Dizi);

                    //Ara dosya
                    string Gecici_zip_dosyası = Path.GetRandomFileName();
                    while (File.Exists(Ortak.Klasör_Gecici + Gecici_zip_dosyası)) Gecici_zip_dosyası = Path.GetRandomFileName();
                    Gecici_zip_dosyası = Ortak.Klasör_Gecici + Gecici_zip_dosyası;
                    File.WriteAllBytes(Gecici_zip_dosyası, _çıktı_);
                    _çıktı_ = null;

                    //Açma
                    using (System.IO.Compression.ZipArchive Arşiv = System.IO.Compression.ZipFile.OpenRead(Gecici_zip_dosyası))
                    {
                        byte[] dizi_içerik = null, dizi_doko = null;

                        System.IO.Compression.ZipArchiveEntry Biri = Arşiv.GetEntry("doko");
                        if (Biri != null)
                        {
                            using (Stream Akış = Biri.Open())
                            {
                                dizi_doko = new byte[Biri.Length];
                                Akış.ReadExactly(dizi_doko, 0, (int)Biri.Length); //ReadExactly
                            }
                        }

                        if (dizi_doko != null && dizi_doko.Length > 0)
                        {
                            string doko = dizi_doko.Yazıya();
                            string[] bölünmüş = doko.Split(';');
                            if (bölünmüş.Length == 2)
                            {
                                string tarih_saat = bölünmüş[0];
                                doko = bölünmüş[1];
                                if (!string.IsNullOrEmpty(doko))
                                {
                                    Biri = Arşiv.GetEntry(tarih_saat);
                                    if (Biri != null)
                                    {
                                        using (Stream Akış = Biri.Open())
                                        {
                                            dizi_içerik = new byte[Biri.Length];
                                            Akış.ReadExactly(dizi_içerik, 0, (int)Biri.Length);
                                        }
                                    }

                                    if (dizi_içerik != null && dizi_içerik.Length > 0)
                                    {
                                        if (doko == DoğrulamaKodu.Üret.BaytDizisinden(dizi_içerik).HexYazıya())
                                        {
                                            _çıktı_ = dizi_içerik;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    Dosya.Sil(Gecici_zip_dosyası);

                    if (_çıktı_ == null || _çıktı_.Length == 0) throw new Exception(_DosyaYolu_ + " dosyası arızalı");

                    //depo sürüm yükseltme
                    _çıktı_ = new Depo_(_çıktı_.Yazıya()).YazıyaDönüştür().BaytDizisine();

                    if (İlkAçılışAyarları != null) _çıktı_ = Banka_Ortak.Dosya_SıkıştırKarıştır(_çıktı_, İlkAçılışAyarları.İşyeri_Parolası.BaytDizisine());
                    File.WriteAllBytes(_DosyaYolu_, _çıktı_);
                }

                //Yeni kul dosyasının oluşturulması
                (Ortak.Klasör_Banka + @"ArgeMup.HazirKod_Cdiyez.Ekranlar.Kullanıcılar.Ayarlar").DosyaYolu_Yaz(" ");
                //Doko üretilmesi
                DoğrulamaKodu.Üret.Klasörden(Ortak.Klasör_Banka, true, SearchOption.AllDirectories, İlkAçılışAyarları == null ? null : İlkAçılışAyarları.İşyeri_Parolası);
                Günlük.Ekle("Banka yeni sürüme geçirme aşama 7 tamam");

                Banka_Ortak.Yedekle_SürümYükseltmeÖncesiYedeği_Sil();
                Günlük.Ekle("Banka yeni sürüme geçirme aşama 8 tamam");
            }

            List<Enum> İzinler = new List<Enum>();
            for (int i = 0; i < (int)Kullanıcılar_İzin.DiziElemanSayısı_; i++) İzinler.Add(((Kullanıcılar_İzin)i));
            ArgeMup.HazirKod.Ekranlar.Kullanıcılar.Başlat(İzinler, Kullanıcılar_İzin.Ayarları_değiştirebilir, GeriBildirimİşlemi_Önyüz_Ayarlar_Değişti, Ortak.Klasör_Banka + @"ArgeMup.HazirKod_Cdiyez.Ekranlar.Kullanıcılar.Ayarlar", Kendi.Adı, "Gelir Gider Takip");
        
            if (İlkAçılışAyarları != null)
            {
                GeçerliKullanıcı = new ArgeMup.HazirKod.Ekranlar.Kullanıcılar.Ayarlar_Üst_.Ayarlar_Kullanıcı_
                {
                    Adı = İlkAçılışAyarları.Kullanıcı_Adı,
                    Rol_İzinleri = İlkAçılışAyarları.Kullanıcı_Rolİzinleri
                };

                KökParola = İlkAçılışAyarları.İşyeri_Parolası;
                ParolaKontrolüGerekiyorMu = true;
            }
        }
        static void GeriBildirimİşlemi_Önyüz_Ayarlar_Değişti(string AyarlarDosyaYolu, string AyarlarDosyaYolu_İçeriği, string Mevcut_KökParola, string Eski_KökParola)
        {
            if (Mevcut_KökParola != Eski_KökParola)
            {
                Banka_Ortak.Yedekle_SürümYükseltmeÖncesiYedeği();
                Günlük.Ekle("Banka yeni sürüme geçirme aşama 1 tamam");

                List<string> dsy_lar = new List<string>();
                dsy_lar.AddRange(Klasör.Listele_Dosya(Ortak.Klasör_Banka, "*.mup", SearchOption.AllDirectories));
                Günlük.Ekle("Banka yeni sürüme geçirme aşama 2 tamam");

                bool _Mevcut_KökParola_VarMı_ = Mevcut_KökParola.DoluMu(true);
                byte[] _Mevcut_KökParola_ = _Mevcut_KökParola_VarMı_ ? Mevcut_KökParola.BaytDizisine_HexYazıdan() : null;
                bool _Eski_KökParola_VarMı_ = Eski_KökParola.DoluMu(true);
                byte[] _Eski_KökParola_ = _Eski_KökParola_VarMı_ ? Eski_KökParola.BaytDizisine_HexYazıdan() : null;
                foreach (string dsy in dsy_lar)
                {
                    byte[] içerik = File.ReadAllBytes(dsy);
                    if (_Eski_KökParola_VarMı_) içerik = Banka_Ortak.Dosya_DüzeltAç(içerik, _Eski_KökParola_);
                    if (_Mevcut_KökParola_VarMı_) içerik = Banka_Ortak.Dosya_SıkıştırKarıştır(içerik, _Mevcut_KökParola_);
                    File.WriteAllBytes(dsy, içerik);
                }
                Günlük.Ekle("Banka yeni sürüme geçirme aşama 3 tamam");

                Banka_Ortak.Yedekle_SürümYükseltmeÖncesiYedeği_Sil();
                Günlük.Ekle("Banka yeni sürüme geçirme aşama 4 tamam");
            }

            AyarlarDosyaYolu.DosyaYolu_Yaz(AyarlarDosyaYolu_İçeriği);
            DoğrulamaKodu.Üret.Klasörden(Ortak.Klasör_Banka, true, SearchOption.AllDirectories, Mevcut_KökParola);
            Günlük.Ekle("Banka yeni sürüme geçirme aşama 5 tamam");
        }

        static bool İlkAçılışKontrolleriniYapıldı = false;
        public static void GirişYap(bool Küçültülmüş)
        {
            Ekranlar.Önyüz.PencereleriKapat();
            GeriBildirimİşlemi_Önyüz_Giriş(İlkAçılışAyarları == null ? ArgeMup.HazirKod.Ekranlar.Kullanıcılar.GirişİşlemiSonucu_.Hatalı : ArgeMup.HazirKod.Ekranlar.Kullanıcılar.GirişİşlemiSonucu_.Başarılı);

            void GeriBildirimİşlemi_Önyüz_Giriş(ArgeMup.HazirKod.Ekranlar.Kullanıcılar.GirişİşlemiSonucu_ GirişİşlemiSonucu)
            {
                switch (GirişİşlemiSonucu)
                {
                    case ArgeMup.HazirKod.Ekranlar.Kullanıcılar.GirişİşlemiSonucu_.Başarılı:
                        if (!İlkAçılışKontrolleriniYapıldı)
                        {
                            Banka_Ortak.Başlat();

                            İlkAçılışKontrolleriniYapıldı = true;
                        }

                        new Ekranlar.AnaEkran().Show();
                        break;

                    case ArgeMup.HazirKod.Ekranlar.Kullanıcılar.GirişİşlemiSonucu_.Kapatıldı:
                        Application.Exit();
                        break;

                    default:
                        ArgeMup.HazirKod.Ekranlar.Kullanıcılar.Önyüz_Giriş(GeriBildirimİşlemi_Önyüz_Giriş, Küçültülmüş, 15);
                        break;
                }
            }
        }
        
        public static void Kapan(string Bilgi)
        {
            Günlük.Ekle("Kapatıldı " + Bilgi, Hemen: true);
            Banka_Ortak.Yedekle_Tümü();

            Ortak.YeniYazılımKontrolü.Durdur();
            Ekranlar.Önyüz.PencereleriKapat();
            Ortak.Çalıştır?.Dispose(); Ortak.Çalıştır = null;
            Klasör.Sil(Ortak.Klasör_Gecici);

            ArgeMup.HazirKod.ArkaPlan.Ortak.Çalışsın = false;
        }

        public enum Kullanıcılar_İzin
        {
            Boşta_,
            Ayarları_değiştirebilir,
            Cari_döküm_içinde_işlem_yapabilir,
            Cari_dökümü_görebilir,
            Avans_peşinat_taksit_ve_üyelik_ekleyebilir, //Ödeme tarihini değiştirebilir
            Gelir_gider_ekleyebilir,                    //Ödeme tarihini değiştiremez

            SonEleman_,
            DiziElemanSayısı_ = SonEleman_
        };
        public class İlkAçılışAyarları_
        {
            [Değişken_.Niteliği.Adını_Değiştir("Bt")] public string Benzersiz_Tanımlayıcı;
            [Değişken_.Niteliği.Adını_Değiştir("İy", 0)] public string İşyeri_Adı = "ArGeMuP";
            [Değişken_.Niteliği.Adını_Değiştir("İy", 1)] public string İşyeri_Parolası;
            [Değişken_.Niteliği.Adını_Değiştir("İy", 2)] public string İşyeri_LogoDosyaYolu;
            [Değişken_.Niteliği.Adını_Değiştir("Kk")] public string KayıtKlasörü;
            [Değişken_.Niteliği.Adını_Değiştir("Mu")] public Dictionary<string, List<string>> SabitMuhataplar; // Grup ve elemanları, adı Çalışan olan grup uygulamada çalışanlar olarak değerlendirilir

            [Değişken_.Niteliği.Adını_Değiştir("KuR")] public bool[] Kullanıcı_Rolİzinleri;
            [Değişken_.Niteliği.Adını_Değiştir("Ku", 0)] public string Kullanıcı_Adı;
            [Değişken_.Niteliği.Adını_Değiştir("Ku", 1)] public İlkAçılışAyarları_Komut_ Kullanıcı_Komut;
            [Değişken_.Niteliği.Adını_Değiştir("KuEt")] public string[] Kullanıcı_Komut_EkTanım;    //Yazdır                : pdf dosya yolu + Şablon Adı
                                                                                                    //Sayfa_GelirGiderEkle  : Gelir veya Gider veya Boş

            [Değişken_.Niteliği.Adını_Değiştir("E GeGi")] public List<İlkAçılışAyarları_Ekle_GelirGider_Talep_> Ekle_GelirGider_Talepler;
        }
        public enum İlkAçılışAyarları_Komut_ { Boşta, Sayfa_GelirGiderEkle, Sayfa_CariDöküm, Sayfa_Ayarlar, Ekle_GelirGider, Yazdır };
        public class İlkAçılışAyarları_Ekle_GelirGider_Talep_
        {
            [Değişken_.Niteliği.Adını_Değiştir("T", 0)] public string Ekle_MuhatapGrubuAdı;
            [Değişken_.Niteliği.Adını_Değiştir("T", 1)] public string Ekle_MuhatapAdı;
            [Değişken_.Niteliği.Adını_Değiştir("T", 2)] public Banka1.İşyeri_Ödeme_İşlem_.Tipi_ Ekle_Tipi;
            [Değişken_.Niteliği.Adını_Değiştir("T", 3)] public Banka1.İşyeri_Ödeme_İşlem_.Durum_ Ekle_Durumu;
            [Değişken_.Niteliği.Adını_Değiştir("T", 4)] public double Ekle_Miktar;
            [Değişken_.Niteliği.Adını_Değiştir("T", 5)] public Banka1.İşyeri_Ödeme_.ParaBirimi_ Ekle_ParaBirimi;
            [Değişken_.Niteliği.Adını_Değiştir("T", 6)] public DateTime Ekle_İlkÖdemeTarihi;
            [Değişken_.Niteliği.Adını_Değiştir("T", 7)] public string Ekle_Notlar;
            [Değişken_.Niteliği.Adını_Değiştir("T", 8)] public int Ekle_Taksit_Sayısı;
            [Değişken_.Niteliği.Adını_Değiştir("T", 9)] public Banka1.Muhatap_Üyelik_.Dönem_ Ekle_Taksit_Dönem;
            [Değişken_.Niteliği.Adını_Değiştir("T", 10)] public int Ekle_Taksit_Dönem_Adet;
            [Değişken_.Niteliği.Adını_Değiştir("T", 11)] public DateTime? Ekle_KayıtTarihi = null;
        }
    }
}
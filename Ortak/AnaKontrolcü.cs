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
        public static bool YanUygulamaOlarakÇalışıyor
        {
            get => Şube != null;
        }
        public static string KökParola
        {
            get
            {
                return ArgeMup.HazirKod.Ekranlar.Kullanıcılar.KökParola;
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
        public static Şube_Talep_ Şube_Talep;
        public static Form BoştaBekleyenAnaUygulama;

        static YanUygulama.Şube_ Şube;
        static System.Threading.Mutex Kilit;

        public static void Açıl(int ŞubeErişimNoktası)
        {
            if (ŞubeErişimNoktası > 0)
            {
                Şube = new YanUygulama.Şube_(ŞubeErişimNoktası, GeriBildirim_İşlemi_Uygulama);
                Kilit = new System.Threading.Mutex();

#if DEBUG
                ArgeMup.HazirKod.ArkaPlan.Hatırlatıcı_ h = new ArgeMup.HazirKod.ArkaPlan.Hatırlatıcı_();
                h.Ekle("a", DateTime.Now.AddSeconds(1), null, _h_işlem_);
                int _h_işlem_(string Adı, object Hatırlatıcı)
                {
                    try
                    {
                        BoştaBekleyenAnaUygulama.Invoke(new Action(() =>
                        {
                            GeriBildirim_İşlemi_Uygulama(true, new Depo_(Dosya.Oku_Yazı("Komut.mup"), null, false).YazıyaDönüştür().BaytDizisine(), null);
                        }));
                    }
                    catch (Exception ex)
                    {
                        ex.Günlük(null, Günlük.Seviye.HazirKod);
                    }
                    
                    Application.Exit();
                    return -1;
                }
#endif
            }
            else
            {
                İlkAçılışKontrolleriniYap();
                GirişYap(false);
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
                byte[] _Eski_KökParola_ = _Eski_KökParola_VarMı_ ? Eski_KökParola.Length == 64 ? /*yeni*/ArgeMup.HazirKod.Dönüştürme.D_HexYazı.BaytDizisine(Eski_KökParola) : /*eski*/ArgeMup.HazirKod.Dönüştürme.D_Yazı.BaytDizisine(Eski_KökParola) : null;
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

            if (!YanUygulamaOlarakÇalışıyor) Dosya.Yaz(AyarlarDosyaYolu, AyarlarDosyaYolu_İçeriği);
            DoğrulamaKodu.Üret.Klasörden(Ortak.Klasör_Banka, true, SearchOption.AllDirectories, Mevcut_KökParola);
            Günlük.Ekle("Banka yeni sürüme geçirme aşama son tamam");
        }
        static void GeriBildirim_İşlemi_Uygulama(bool BağlantıKuruldu, byte[] Bilgi, string Açıklama)
        {
            if (!BağlantıKuruldu)
            {
#if !DEBUG
                Application.Exit(); 
#endif
                return;
            }

            string içerik = Bilgi.Yazıya();
            if (içerik.BoşMu() || !Kilit.WaitOne(5000)) return;

            Şube_Talep = null;
            Şube_Talep_Cevap_ Cevap = new Şube_Talep_Cevap_();
            Depo_ Depo;
            bool HataOldu = false;

            try
            {
                Ekranlar.Önyüz.PencereleriKapat();

                Depo = new Depo_(içerik);
                Şube_Talep = (Şube_Talep_)Banka_Ortak.Değişken.Üret(typeof(Şube_Talep_), Depo["ArGeMuP"]);

                Cevap.Benzersiz_Tanımlayıcı = Şube_Talep.Benzersiz_Tanımlayıcı;

                if (!İlkAçılışKontrolleriYapıldı) 
                { 
                    İlkAçılışKontrolleriniYap(); 
                    Banka_Ortak.Başlat();

                    İlkAçılışKontrolleriYapıldı = true;
                }

                ArgeMup.HazirKod.Ekranlar.Kullanıcılar._Ayarlar_Üst_.GeçerliKullanıcı.Adı = Şube_Talep.Kullanıcı_Adı;
                ArgeMup.HazirKod.Ekranlar.Kullanıcılar._Ayarlar_Üst_.GeçerliKullanıcı.Rol_İzinleri = Şube_Talep.Kullanıcı_Rolİzinleri;

                switch (Şube_Talep.Kullanıcı_Komut)
                {
                    case Şube_Talep_Komut_.Ekle_GelirGider:
                        if (Şube_Talep.Ekle_GelirGider_Talepler != null && Şube_Talep.Ekle_GelirGider_Talepler.Count > 0)
                        {
                            foreach (var Talep in Şube_Talep.Ekle_GelirGider_Talepler)
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
                        Cevap.Başarılı = true;
                        break;

                    case Şube_Talep_Komut_.Yazdır:
                        var cd = new Ekranlar.Cari_Döküm(Ekranlar.Cari_Döküm.AçılışTürü_.Gizli);

                        if (Şube_Talep.Kullanıcı_Komut_EkTanım == null || Şube_Talep.Kullanıcı_Komut_EkTanım.Length < 1) throw new Exception("if (AçılışDetayları == null || AçılışDetayları.Length < 1)");
                        string şablon = Şube_Talep.Kullanıcı_Komut_EkTanım.Length >= 2 ? Şube_Talep.Kullanıcı_Komut_EkTanım[1] : null;
                        string sonuç = cd.Şablon_Seç_TabloyuOluştur(şablon);

                        if (sonuç.DoluMu()) Cevap.Detaylar = new string[] { sonuç };
                        else if (cd.Tablo.RowCount == 0) Cevap.Detaylar = new string[] { "Hiç kayıt bulunamadı" };
                        else
                        {
                            Ekranlar.Ayarlar_Yazdırma yzdrm = new Ekranlar.Ayarlar_Yazdırma();
                            yzdrm.Yazdır(cd.Tablo, Şube_Talep.Kullanıcı_Komut_EkTanım[0]);
                            Cevap.Başarılı = true;
                        }
                        break;

                    case Şube_Talep_Komut_.Sayfa_Ayarlar:
                        if (İzinliMi(Kullanıcılar_İzin.Ayarları_değiştirebilir)) _SayfaAç_();
                        else Cevap.Detaylar = new string[] { "İzinleri kontrol ediniz." };
                        break;

                    case Şube_Talep_Komut_.Sayfa_CariDöküm:
                        if (İzinliMi(Kullanıcılar_İzin.Cari_dökümü_görebilir)) _SayfaAç_();
                        else Cevap.Detaylar = new string[] { "İzinleri kontrol ediniz." };
                        break;

                    case Şube_Talep_Komut_.Sayfa_GelirGiderEkle:
                        if (İzinliMi(Kullanıcılar_İzin.Gelir_gider_ekleyebilir)) _SayfaAç_();
                        else Cevap.Detaylar = new string[] { "İzinleri kontrol ediniz." };
                        break;

                    case Şube_Talep_Komut_.Kontrol:
                        if (Şube_Talep.Kullanıcı_Komut_EkTanım != null && Şube_Talep.Kullanıcı_Komut_EkTanım.Length == 3)
                        {
                            Cevap.Başarılı = true;

                            if (Şube_Talep.Kullanıcı_Komut_EkTanım[0] == "1")
                            {
                                //PencereleriKapat
                                Ekranlar.Önyüz.PencereleriKapat();
                            }

                            if (Şube_Talep.Kullanıcı_Komut_EkTanım[1] == "1")
                            {
                                //Yedekle
                                bool yedeklendi = Banka_Ortak.Yedekle_Tümü();
                                Cevap.Başarılı &= !yedeklendi || (yedeklendi && Banka_Ortak.Yedekleme_Hatalar.BoşMu());
                                Cevap.Detaylar = new string[] { Cevap.Başarılı ? (yedeklendi ? "1" : "0") : Banka_Ortak.Yedekleme_Hatalar };
                            }

                            if (Şube_Talep.Kullanıcı_Komut_EkTanım[2] == "1")
                            {
                                //Durdur
                                Application.Exit();
                            }
                        }
                        else Cevap.Detaylar = new string[] { "if (Şube_Talep.Kullanıcı_Komut_EkTanım != null && Şube_Talep.Kullanıcı_Komut_EkTanım.Length == 3)" };
                        break;

                    case Şube_Talep_Komut_.İşyeriParolasınıDeğiştir:
                        string yok = "_YOK_";
                        if (Şube_Talep.Kullanıcı_Komut_EkTanım == null || Şube_Talep.Kullanıcı_Komut_EkTanım.Length != 4 ||
                            Şube_Talep.Kullanıcı_Komut_EkTanım[0].BoşMu(true) || Şube_Talep.Kullanıcı_Komut_EkTanım[1].BoşMu(true) ||
                            Şube_Talep.Kullanıcı_Komut_EkTanım[0] != Şube_Talep.Kullanıcı_Komut_EkTanım[2] ||
                            Şube_Talep.Kullanıcı_Komut_EkTanım[1] != Şube_Talep.Kullanıcı_Komut_EkTanım[3] ||
                            (KökParola.DoluMu(true) ? KökParola != Şube_Talep.Kullanıcı_Komut_EkTanım[0] : Şube_Talep.Kullanıcı_Komut_EkTanım[0] != yok))
                        {
                            Cevap.Detaylar = new string[] { "Girdileri kontrol ediniz" };
                        }
                        else
                        {
                            string mevcut_parola = Şube_Talep.Kullanıcı_Komut_EkTanım[0] == yok ? null : Şube_Talep.Kullanıcı_Komut_EkTanım[0];
                            string yeni_parola = Şube_Talep.Kullanıcı_Komut_EkTanım[1] == yok ? null : Şube_Talep.Kullanıcı_Komut_EkTanım[1];
                            GeriBildirimİşlemi_Önyüz_Ayarlar_Değişti(null, null, yeni_parola, mevcut_parola);
                            Cevap.Başarılı = true;

                            //Bu noktadan sonra yeni işlem yapmadan önce yeniden başlatılmalı
                            ArgeMup.HazirKod.Ekranlar.Kullanıcılar._Ayarlar_Üst_ = null;
                        }
                        break;

                    default: throw new Exception("Şube_Talep.Kullanıcı_Komut (" + Şube_Talep.Kullanıcı_Komut + ")");
                }

                void _SayfaAç_()
                {
                    BoştaBekleyenAnaUygulama.Invoke(new Action(() =>
                    {
                        new Ekranlar.AnaEkran().Show();
                        W32_3.SetForegroundWindow(Ekranlar.Önyüz.AnaEkran.Handle);
                    }));
                    Cevap.Başarılı = true;
                }
            }
            catch (Exception ex)
            {
                Cevap.Detaylar = new string[] { ex.ToString() };
                HataOldu = true;
            }

            Kilit.ReleaseMutex();

#if DEBUG
            (Cevap.Başarılı ? "Başarılı" : Cevap.Detaylar?[0]).Günlük("GeriBildirim_İşlemi_Uygulama " + Şube_Talep?.Kullanıcı_Komut.ToString() + " ");
#else
            Depo = new Depo_();
            Banka_Ortak.Değişken.Depola(Cevap, Depo["ArGeMuP"]);
            byte[] cevap_dizi = Depo.YazıyaDönüştür().BaytDizisine();
            Şube.Gönder(cevap_dizi);
#endif

            if (HataOldu) Application.Exit(); //Bilgi gönderildikten sonra kapan
        }

        static bool İlkAçılışKontrolleriYapıldı = false;
        static void İlkAçılışKontrolleriniYap()
        {
            string AnaKlasör = null;

            if (YanUygulamaOlarakÇalışıyor)
            {
                if (Şube_Talep.İşyeri_Adı.BoşMu(true)) throw new Exception("if (Şube_Talep.İşyeri_Adı.BoşMu(true))");

                if (Şube_Talep.KayıtKlasörü.BoşMu()) throw new System.Exception("if (Şube_Talep.KayıtKlasörü.BoşMu())");
                AnaKlasör = Şube_Talep.KayıtKlasörü.TrimEnd('\\');

                if (Şube_Talep.İşyeri_LogoDosyaYolu.DoluMu() && File.Exists(Şube_Talep.İşyeri_LogoDosyaYolu)) Ortak.Firma_Logo = new Bitmap(Şube_Talep.İşyeri_LogoDosyaYolu);
            }
            else
            {
                AnaKlasör = Kendi.Klasörü;
            }

            Ortak.Firma_Logo ??= Properties.Resources.logo_512_seffaf;
            Ortak.Klasör_Banka = AnaKlasör + "\\Banka\\";
            Ortak.Klasör_Banka2 = AnaKlasör + "\\Banka2\\";
            Ortak.Klasör_İçYedek = AnaKlasör + "\\Yedek\\";
            Ortak.Klasör_KullanıcıDosyaları = AnaKlasör + "\\Kullanıcı Dosyaları\\";

            Klasör.Oluştur(Ortak.Klasör_Banka);
            Klasör.Oluştur(Ortak.Klasör_İçYedek);
            Klasör.Oluştur(Ortak.Klasör_KullanıcıDosyaları);
            if (YanUygulamaOlarakÇalışıyor) Klasör.Sil(Ortak.Klasör_Gecici);
            Klasör.Oluştur(Ortak.Klasör_Gecici);

            Banka_Ortak.Yedekle_SürümYükseltmeÖncesiYedeği_Kurtar();

            List<Enum> İzinler = new List<Enum>();
            for (int i = 0; i < (int)Kullanıcılar_İzin.DiziElemanSayısı_; i++) İzinler.Add(((Kullanıcılar_İzin)i));
            ArgeMup.HazirKod.Ekranlar.Kullanıcılar.Başlat(İzinler, Kullanıcılar_İzin.Ayarları_değiştirebilir, GeriBildirimİşlemi_Önyüz_Ayarlar_Değişti, Ortak.Klasör_Banka + @"ArgeMup.HazirKod_Cdiyez.Ekranlar.Kullanıcılar.Ayarlar", Kendi.Adı, "Gelir Gider Takip");

            if (YanUygulamaOlarakÇalışıyor)
            {
                ArgeMup.HazirKod.Ekranlar.Kullanıcılar._Ayarlar_Üst_.GeçerliKullanıcı = new ArgeMup.HazirKod.Ekranlar.Kullanıcılar.Ayarlar_Üst_.Ayarlar_Kullanıcı_();
                ArgeMup.HazirKod.Ekranlar.Kullanıcılar._Ayarlar_Üst_.KökParola = Şube_Talep.İşyeri_Parolası == "_YOK_" ? null : Şube_Talep.İşyeri_Parolası;
                ArgeMup.HazirKod.Ekranlar.Kullanıcılar._Ayarlar_Üst_.KökParola_Dizi = Şube_Talep.İşyeri_Parolası == "_YOK_" ? null : Şube_Talep.İşyeri_Parolası.Length == 64 ? /*yeni*/ArgeMup.HazirKod.Dönüştürme.D_HexYazı.BaytDizisine(Şube_Talep.İşyeri_Parolası) : /*eski*/ArgeMup.HazirKod.Dönüştürme.D_Yazı.BaytDizisine(Şube_Talep.İşyeri_Parolası);
                ArgeMup.HazirKod.Ekranlar.Kullanıcılar._Ayarlar_Üst_.ParolaKontrolüGerekiyorMu = true;
                Ekranlar.Önyüz.SürümKontrolMesajı = null;
            }
            else
            {
                #if !DEBUG
                    if (!File.Exists(Ortak.Klasör_KullanıcıDosyaları + "YeniSurumuKontrolEtme.txt"))
                    {
                        Ortak.YeniYazılımKontrolü = new YeniYazılımKontrolü_();
                        Ortak.YeniYazılımKontrolü.Başlat(new Uri("https://github.com/ArgeMup/GelirGiderTakip/blob/main/bin/Yay%C4%B1nla/Gelir%20Gider%20Takip.exe?raw=true"), _YeniYazılımKontrolü_GeriBildirim_);

                        void _YeniYazılımKontrolü_GeriBildirim_(bool Sonuç, string Açıklama)
                        {
                            if (Açıklama.Contains("github")) Ekranlar.Önyüz.SürümKontrolMesajı = "Bağlantı kurulamadı";
                            else if (Açıklama == "Durduruldu") return;
                            else Ekranlar.Önyüz.SürümKontrolMesajı = Açıklama;

                            Ekranlar.Önyüz.AnaEkran?.Invoke(new Action(() =>
                            {
                                Ekranlar.Önyüz.AnaEkran.Text = Ekranlar.Önyüz.AnaEkran.Text.Replace("Yeni sürüm kontrol ediliyor", Ekranlar.Önyüz.SürümKontrolMesajı);
                            }));
                        }
                    }
                #endif
            }
        }

        public static void GirişYap(bool Küçültülmüş)
        {
            Ekranlar.Önyüz.PencereleriKapat();
            GeriBildirimİşlemi_Önyüz_Giriş(YanUygulamaOlarakÇalışıyor ? ArgeMup.HazirKod.Ekranlar.Kullanıcılar.GirişİşlemiSonucu_.Başarılı : ArgeMup.HazirKod.Ekranlar.Kullanıcılar.GirişİşlemiSonucu_.Hatalı);

            void GeriBildirimİşlemi_Önyüz_Giriş(ArgeMup.HazirKod.Ekranlar.Kullanıcılar.GirişİşlemiSonucu_ GirişİşlemiSonucu)
            {
                switch (GirişİşlemiSonucu)
                {
                    case ArgeMup.HazirKod.Ekranlar.Kullanıcılar.GirişİşlemiSonucu_.Başarılı:
                        if (!İlkAçılışKontrolleriYapıldı)
                        {
                            Banka_Ortak.Başlat();

                            İlkAçılışKontrolleriYapıldı = true;
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
            if (!YanUygulamaOlarakÇalışıyor) Banka_Ortak.Yedekle_Tümü();

            Ortak.YeniYazılımKontrolü?.Durdur(); Ortak.YeniYazılımKontrolü = null;
            Ekranlar.Önyüz.PencereleriKapat();
            Şube?.Dispose(); Şube = null;
            Kilit?.Dispose(); Kilit = null;
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
        public class Şube_Talep_
        {
            [Değişken_.Niteliği.Adını_Değiştir("Bt")] public string Benzersiz_Tanımlayıcı;
            [Değişken_.Niteliği.Adını_Değiştir("İy", 0)] public string İşyeri_Adı = "ArGeMuP";
            [Değişken_.Niteliği.Adını_Değiştir("İy", 1)] public string İşyeri_Parolası;
            [Değişken_.Niteliği.Adını_Değiştir("İy", 2)] public string İşyeri_LogoDosyaYolu;
            [Değişken_.Niteliği.Adını_Değiştir("Kk")] public string KayıtKlasörü;
            [Değişken_.Niteliği.Adını_Değiştir("Mu")] public Dictionary<string, List<string>> SabitMuhataplar; // Grup ve elemanları, adı Çalışan olan grup uygulamada çalışanlar olarak değerlendirilir

            [Değişken_.Niteliği.Adını_Değiştir("KuR")] public bool[] Kullanıcı_Rolİzinleri = new bool[(int)Kullanıcılar_İzin.DiziElemanSayısı_];
            [Değişken_.Niteliği.Adını_Değiştir("Ku", 0)] public string Kullanıcı_Adı;
            [Değişken_.Niteliği.Adını_Değiştir("Ku", 1)] public Şube_Talep_Komut_ Kullanıcı_Komut;
            [Değişken_.Niteliği.Adını_Değiştir("KuEt")] public string[] Kullanıcı_Komut_EkTanım;    //Yazdır                    : pdf dosya yolu + Şablon Adı
                                                                                                    //Sayfa_GelirGiderEkle      : Gelir veya Gider veya Boş
                                                                                                    //İşyeriParolasınıDeğiştir  : Mevcut_Parola + Yeni_Parola + Mevcut_Parola + Yeni_Parola
                                                                                                    //                              Parola kullanılmayacak ise içeriği _YOK_ olmalı,
                                                                                                    //                              yada ArgeMup.HazirKod.Dönüştürme.D_HexYazı.BaytDizisinden( Rastgele.BaytDizisi(en az 32) ) ile üretilmeli

            [Değişken_.Niteliği.Adını_Değiştir("E GeGi")] public List<Şube_Talep_Ekle_GelirGider_> Ekle_GelirGider_Talepler;
        }
        public enum Şube_Talep_Komut_ { Boşta, Sayfa_GelirGiderEkle, Sayfa_CariDöküm, Sayfa_Ayarlar, Ekle_GelirGider, Yazdır, Kontrol, İşyeriParolasınıDeğiştir };
        public class Şube_Talep_Ekle_GelirGider_
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
        class Şube_Talep_Cevap_
        {
            [Değişken_.Niteliği.Adını_Değiştir("C", 0)] public string Benzersiz_Tanımlayıcı;
            [Değişken_.Niteliği.Adını_Değiştir("C", 1)] public bool Başarılı;

            [Değişken_.Niteliği.Adını_Değiştir("D")] public string[] Detaylar;
        }
    }
}
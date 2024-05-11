using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.IO;
using ArgeMup.HazirKod;
using ArgeMup.HazirKod.Dönüştürme;
using ArgeMup.HazirKod.Ekİşlemler;
using System.Linq;

namespace Gelir_Gider_Takip
{
    public static class Banka1_Ekİşlemler
    {
        public static bool GelirMi(this Banka1.İşyeri_Ödeme_İşlem_.Tipi_ Tipi)
        {
            return Tipi >= Banka1.İşyeri_Ödeme_İşlem_.Tipi_.Gelir;
        }
        public static bool ÖdendiMi(this Banka1.İşyeri_Ödeme_İşlem_.Durum_ Durumu)
        {
            return Durumu >= Banka1.İşyeri_Ödeme_İşlem_.Durum_.TamÖdendi;
        }
        public static string Yazdır(this Banka1.İşyeri_Ödeme_İşlem_.Tipi_ Tipi)
        {
            switch (Tipi)
            {
                case Banka1.İşyeri_Ödeme_İşlem_.Tipi_.Gider: return "Gider";
                case Banka1.İşyeri_Ödeme_İşlem_.Tipi_.Gelir: return "Gelir";
                case Banka1.İşyeri_Ödeme_İşlem_.Tipi_.MaaşÖdemesi: return "Maaş ödemesi";
                case Banka1.İşyeri_Ödeme_İşlem_.Tipi_.AvansVerilmesi: return "Avans verilmesi";
                case Banka1.İşyeri_Ödeme_İşlem_.Tipi_.AvansÖdemesi: return "Avans ödemesi";
                case Banka1.İşyeri_Ödeme_İşlem_.Tipi_.KontrolNoktası: return "Kontrol noktası";
                default: throw new Exception("Tipi(" + Tipi + ") uygun değil");
            }
        }
        public static string Yazdır(this Banka1.İşyeri_Ödeme_İşlem_.Durum_ Durumu)
        {
            switch (Durumu)
            {
                case Banka1.İşyeri_Ödeme_İşlem_.Durum_.Ödenmedi: return "Ödenmedi";
                case Banka1.İşyeri_Ödeme_İşlem_.Durum_.KısmenÖdendi: return "Kısmen ödendi";
                case Banka1.İşyeri_Ödeme_İşlem_.Durum_.TamÖdendi: return "Tam ödendi";
                case Banka1.İşyeri_Ödeme_İşlem_.Durum_.İptalEdildi: return "İptal edildi";
                case Banka1.İşyeri_Ödeme_İşlem_.Durum_.PeşinatÖdendi: return "Peşinat ödendi";
                case Banka1.İşyeri_Ödeme_İşlem_.Durum_.KısmiÖdemeYapıldı: return "Kısmi ödeme yapıldı";

                default: throw new Exception("Durumu(" + Durumu + ") uygun değil");
            }
        }
        public static string Yazdır(this Banka1.İşyeri_Ödeme_Taksit_ Taksit)
        {
            if (Taksit == null) return null;

            return Taksit.Taksit_No + " / " + Taksit.Taksit_Sayısı;
        }
    }
    public class Banka1
    {
        public İşyeri_ Seçilenİşyeri;
        public const string Çalışan_Yazısı = "Çalışan";
        public Dictionary<string, İşyeri_> İşyerleri; //işyeri adı ve içeriği
        public Ayarlar_ Ayarlar;

        public void Başlat()
        {
            //Ayarların açılması
            Ayarlar = Banka_Ortak.Sınıf_Aç(typeof(Ayarlar_), "Ay") as Ayarlar_;
            if (Ayarlar == null) throw new Exception("Ayarlar(İy) == null");
            while (Ayarlar.SonBankaKayıt > DateTime.Now)
            {
                string msg = "Son kayıt saati : " + Ayarlar.SonBankaKayıt + Environment.NewLine +
                    "Bilgisayarınızın saati : " + DateTime.Now.Yazıya() + Environment.NewLine + Environment.NewLine +
                    "Muhtemelen bilgisayarınızın saati geri kaldı, lütfen düzeltip devam ediniz";

                System.Windows.Forms.MessageBox.Show(msg.Günlük(), "Bütünlük Kontrolü");
            }

            //İşyerlerinin açılması
            İşyerleri = new Dictionary<string, İşyeri_>();
            string[] işyeri_kls_leri = Directory.GetDirectories(Ortak.Klasör_Banka, "*", SearchOption.TopDirectoryOnly);
            if (işyeri_kls_leri != null && işyeri_kls_leri.Length > 0)
            {
                int kesme = Ortak.Klasör_Banka.Length;

                foreach (string işyeri in işyeri_kls_leri)
                {
                    string kls = işyeri.Substring(kesme);
                    İşyeri_ sınıf = Banka_Ortak.Sınıf_Aç(typeof(İşyeri_), kls + "\\İy") as İşyeri_;

                    if (sınıf == null) throw new Exception("sınıf(" + kls + "\\İy) == null");
                    if (sınıf.GöbekAdı != kls) throw new Exception("İşyeri:" + işyeri + " sınıf.GöbekAdı(" + sınıf.GöbekAdı + ") != kls(" + kls + ")");

                    İşyerleri.Add(sınıf.İşyeriAdı, sınıf);
                }
            }
        }
        public List<string> İşyeri_Listele()
        {
            return İşyerleri.Keys.ToList();
        }
        public void İşyeri_Ekle(string Adı)
        {
            if (Adı.BoşMu(true) || İşyerleri.ContainsKey(Adı)) return;
            
            İşyeri_ işyeri = new İşyeri_() { İşyeriAdı = Adı, GöbekAdı = Banka_Ortak.YeniKlasörAdıOluştur(Ortak.Klasör_Banka) };
            işyeri.DeğişiklikYapıldı = true;
            İşyerleri.Add(Adı, işyeri);
        }
        public void İşyeri_AdınıDeğiştir(string Adı, string YeniAdı)
        {
            İşyeri_ işyeri = İşyerleri[Adı];
            işyeri.İşyeriAdı = YeniAdı;
            işyeri.DeğişiklikYapıldı = true;

            İşyerleri.Remove(Adı);
            İşyerleri.Add(YeniAdı, işyeri);
        }
        public İşyeri_? İşyeri_Aç(string Adı)
        {
            if (Adı.BoşMu(true) || !İşyerleri.TryGetValue(Adı, out İşyeri_ işyeri)) return null;

            if (işyeri.İşyeriAdı != Adı) throw new Exception("işyeri.İşyeriAdı(" + işyeri.İşyeriAdı + ") != Adı(" + Adı + ")");

            if (işyeri.Üyelik_ZamanıGelenleriKaydet()) Banka_Ortak.DeğişiklikleriKaydet();

            Banka_Ortak.Yazdır_Sıfırla();

            return işyeri;
        }
        public void İşyeri_Sil(string Adı)
        {
            string kls = Ortak.Klasör_Banka + İşyerleri[Adı].GöbekAdı;
            Klasör.Sil(kls);

            İşyerleri.Remove(Adı);
        }

        #region Depolama Sınıfları
        public class İşyeri_ : Banka_Ortak.IBanka_Tanımlayıcı_
        {
            [Değişken_.Niteliği.Adını_Değiştir("Ö")] public int AylıkÜcretÖdemeGünü = 1;
            [Değişken_.Niteliği.Adını_Değiştir("Ge")] public double[] ToplamGelir = new double[(int)İşyeri_Ödeme_.ParaBirimi_.ElemanSayısı]; //0. eleman kullanılmıyor
            [Değişken_.Niteliği.Adını_Değiştir("Gi")] public double[] ToplamGider = new double[(int)İşyeri_Ödeme_.ParaBirimi_.ElemanSayısı]; //0. eleman kullanılmıyor
            [Değişken_.Niteliği.Adını_Değiştir("GeÖ")] public double[] ÖdenmişToplamGelir = new double[(int)İşyeri_Ödeme_.ParaBirimi_.ElemanSayısı]; //0. eleman kullanılmıyor
            [Değişken_.Niteliği.Adını_Değiştir("GiÖ")] public double[] ÖdenmişToplamGider = new double[(int)İşyeri_Ödeme_.ParaBirimi_.ElemanSayısı]; //0. eleman kullanılmıyor
            [Değişken_.Niteliği.Adını_Değiştir("M")] public Dictionary<string, Dictionary<string, string>> MuhatapGrubuAdı_MuhatapAdı_GöbekAdı = new Dictionary<string, Dictionary<string, string>>();

            [Değişken_.Niteliği.Bunu_Kesinlikle_Kullanma] public Dictionary<string, Muhatap_> Muhataplar = new Dictionary<string, Muhatap_>(); //Göbek adı ve içeriği
            [Değişken_.Niteliği.Bunu_Kesinlikle_Kullanma] public Dictionary<string, İşyeri_BirYıllıkDönem_> Ödemeler = new Dictionary<string, İşyeri_BirYıllıkDönem_>(); //yıl ve ödemeler
            [Değişken_.Niteliği.Bunu_Kesinlikle_Kullanma] public Dictionary<string, List<string>> MuhatapGrubuVeMuhatapİsimleri_Sabit = new Dictionary<string, List<string>>();

            #region İşlemler
            public string İşyeri_Klasörü { get { return Ortak.Klasör_Banka + GöbekAdı + "\\"; } }

            public List<string> MuhatapGrubu_Listele(bool Sabit = false)
            {
                return Sabit ? MuhatapGrubuVeMuhatapİsimleri_Sabit.Keys.ToList() : MuhatapGrubuAdı_MuhatapAdı_GöbekAdı.Keys.ToList();
            }
            public List<string> MuhatapGrubu_Listele_Tümü()
            {
                List<string> tümü = new List<string>();
                tümü.AddRange(MuhatapGrubu_Listele());
                tümü.AddRange(MuhatapGrubu_Listele(true));
                return tümü.Distinct().ToList();
            }
            public void MuhatapGrubu_Ekle(string GrupAdı)
            {
                MuhatapGrubuAdı_MuhatapAdı_GöbekAdı.Add(GrupAdı, new Dictionary<string, string>());

                DeğişiklikYapıldı = true;
            }
            public void MuhatapGrubu_AdınıDeğiştir(string GrupAdı, string YeniGrupAdı)
            {
                Dictionary<string, string> grup_içeriği = MuhatapGrubuAdı_MuhatapAdı_GöbekAdı[GrupAdı];
                foreach (var muhatap_adı in grup_içeriği.Keys)
                {
                    Muhatap_ muhatap = Muhatap_Aç(GrupAdı, muhatap_adı, true);
                    muhatap.MuhatapGrubuAdı = YeniGrupAdı;
                    muhatap.DeğişiklikYapıldı = true;
                }
                MuhatapGrubuAdı_MuhatapAdı_GöbekAdı.Remove(GrupAdı);
                MuhatapGrubuAdı_MuhatapAdı_GöbekAdı.Add(YeniGrupAdı, grup_içeriği);

                foreach (string yıl in Ödemeler_Listele_Yıllar())
                {
                    İşyeri_BirYıllıkDönem_ BirYıllıkDönem = Ödemeler_Listele_BirYıllıkDönem(yıl);

                    foreach (İşyeri_Ödeme_ Ödeme in BirYıllıkDönem.Ödemeler)
                    {
                        if (Ödeme.MuhatapGrubuAdı == GrupAdı)
                        {
                            Ödeme.MuhatapGrubuAdı = YeniGrupAdı;
                            BirYıllıkDönem.DeğişiklikYapıldı = true;
                        }
                    }
                }

                DeğişiklikYapıldı = true;
            }
            public void MuhatapGrubu_SıralamasınıDeğiştir(List<string> YeniSıralama)
            {
                MuhatapGrubuAdı_MuhatapAdı_GöbekAdı = MuhatapGrubuAdı_MuhatapAdı_GöbekAdı.Sırala(YeniSıralama) as Dictionary<string, Dictionary<string, string>>;

                DeğişiklikYapıldı = true;
            }
            public void MuhatapGrubu_Sil(string Adı)
            {
                foreach (string GöbekAdı in MuhatapGrubuAdı_MuhatapAdı_GöbekAdı[Adı].Values)
                {
                    string kls = İşyeri_Klasörü + "Mu\\" + GöbekAdı;
                    Klasör.Sil(kls);
                }

                MuhatapGrubuAdı_MuhatapAdı_GöbekAdı.Remove(Adı);
                Muhataplar.Clear();

                DeğişiklikYapıldı = true;
            }
            public List<string> Muhatap_Listele(string GrupAdı, bool Sabit = false)
            {
                if (Sabit)
                {
                    if (MuhatapGrubuVeMuhatapİsimleri_Sabit.ContainsKey(GrupAdı)) return MuhatapGrubuVeMuhatapİsimleri_Sabit[GrupAdı];
                }
                else
                {
                    if (MuhatapGrubuAdı_MuhatapAdı_GöbekAdı.ContainsKey(GrupAdı)) return MuhatapGrubuAdı_MuhatapAdı_GöbekAdı[GrupAdı].Keys.ToList();
                }

                return new List<string>();
            }
            public List<string> Muhatap_Listele_Tümü(string GrupAdı)
            {
                List<string> tümü = new List<string>();
                tümü.AddRange(Muhatap_Listele(GrupAdı));
                tümü.AddRange(Muhatap_Listele(GrupAdı, true));
                return tümü.Distinct().ToList();
            }
            public string Muhatap_Ekle(string GrupAdı, string MuhatapAdı)
            {
                string GöbekAdı = Banka_Ortak.YeniKlasörAdıOluştur(İşyeri_Klasörü + "Mu");
                string kls = İşyeri_Klasörü + "Mu\\" + GöbekAdı;
                Klasör.Oluştur(kls);

                MuhatapGrubuAdı_MuhatapAdı_GöbekAdı[GrupAdı].Add(MuhatapAdı, GöbekAdı);
                
                DeğişiklikYapıldı = true;

                if(GrupAdı == Çalışan_Yazısı)
                {
                    Muhatap_ Muhatap = Muhatap_Aç(GrupAdı, MuhatapAdı, true);
                    Muhatap.Çalışan = new Muhatap_Çalışan_();

                    Muhatap_Çalışan_ÖzlükHakkı_ öh = new Muhatap_Çalışan_ÖzlükHakkı_();
                    öh.Türü = Muhatap_Çalışan_ÖzlükHakkı_.Türü_.YeniÇalışan;
                    öh.GerçekleştirenKullanıcıAdı = AnaKontrolcü.KullanıcıAdı;
                    DateTime dt = DateTime.Now;
                    Muhatap.Çalışan.ÖzlükHakkı_Ekle(öh, dt);

                    Muhatap.DeğişiklikYapıldı = true;
                }

                return GöbekAdı;
            }
            public void Muhatap_AdınıDeğiştir(string GrupAdı, string MuhatapAdı, string YeniMuhatapAdı)
            {
                Muhatap_ muhatap = Muhatap_Aç(GrupAdı, MuhatapAdı, true);
                muhatap.MuhatapAdı = YeniMuhatapAdı;
                muhatap.DeğişiklikYapıldı = true;
                Dictionary<string, string> grup = MuhatapGrubuAdı_MuhatapAdı_GöbekAdı[GrupAdı];
                string GöbekAdı = grup[MuhatapAdı];
                grup.Remove(MuhatapAdı);
                grup.Add(YeniMuhatapAdı, GöbekAdı);

                foreach (string yıl in Ödemeler_Listele_Yıllar())
                {
                    İşyeri_BirYıllıkDönem_ BirYıllıkDönem = Ödemeler_Listele_BirYıllıkDönem(yıl);

                    foreach (İşyeri_Ödeme_ Ödeme in BirYıllıkDönem.Ödemeler)
                    {
                        if (Ödeme.MuhatapGrubuAdı == GrupAdı && Ödeme.MuhatapAdı == MuhatapAdı)
                        {
                            Ödeme.MuhatapAdı = YeniMuhatapAdı;
                            BirYıllıkDönem.DeğişiklikYapıldı = true;
                        }
                    }
                }

                DeğişiklikYapıldı = true;
            }
            public void Muhatap_SıralamasınıDeğiştir(string GrupAdı, List<string> YeniSıralama)
            {
                Dictionary<string, string> EskiGrupİçeriği = MuhatapGrubuAdı_MuhatapAdı_GöbekAdı[GrupAdı];
                Dictionary<string, string> YeniGrupİçeriği = EskiGrupİçeriği.Sırala(YeniSıralama) as Dictionary<string, string>;
                MuhatapGrubuAdı_MuhatapAdı_GöbekAdı[GrupAdı] = YeniGrupİçeriği;

                DeğişiklikYapıldı = true;
            }
            public void Muhatap_Sil(string GrupAdı, string MuhatapAdı)
            {
                Muhatap_ muhatap = Muhatap_Aç(GrupAdı, MuhatapAdı);
                if (muhatap != null)
                {
                    string kls = İşyeri_Klasörü + "Mu\\" + muhatap.MuhatapGöbekAdı;
                    Klasör.Sil(kls);

                    MuhatapGrubuAdı_MuhatapAdı_GöbekAdı[GrupAdı].Remove(MuhatapAdı);
                }

                Muhataplar.Clear();

                DeğişiklikYapıldı = true;
            }
            public Muhatap_? Muhatap_Aç(string GrupAdı, string MuhatapAdı, bool YoksaOluştur = false)
            {
                if (GrupAdı.BoşMu(true) || MuhatapAdı.BoşMu(true)) return null;

                if(!MuhatapGrubuAdı_MuhatapAdı_GöbekAdı.TryGetValue(GrupAdı, out Dictionary<string, string> _GrupİçindekiMuhatapAdları_) ||
                   !_GrupİçindekiMuhatapAdları_.TryGetValue(MuhatapAdı, out string _GrupİçindekiMuhatabınGöbekAdı_))
                {
                    if ((!MuhatapGrubuVeMuhatapİsimleri_Sabit.TryGetValue(GrupAdı, out List<string> _GrupİçindekiMuhatapAdları_2_) || !_GrupİçindekiMuhatapAdları_2_.Contains(MuhatapAdı)) &&
                        !YoksaOluştur) return null;

                    if (_GrupİçindekiMuhatapAdları_ == null) MuhatapGrubu_Ekle(GrupAdı);
                    _GrupİçindekiMuhatabınGöbekAdı_ = Muhatap_Ekle(GrupAdı, MuhatapAdı);
                }

                if (Muhataplar.TryGetValue(_GrupİçindekiMuhatabınGöbekAdı_, out Muhatap_ muhatap)) return muhatap;
                else
                {
                    string dsy_muhatap_ay = GöbekAdı + "\\Mu\\" + _GrupİçindekiMuhatabınGöbekAdı_ + "\\Ay";
                    if (!Banka_Ortak.Sınıf_DosyaVarMı(dsy_muhatap_ay))
                    {
                        muhatap = (Muhatap_)Banka_Ortak.Sınıf_Oluştur(typeof(Muhatap_), null);
                        muhatap.DeğişiklikYapıldı = true;
                        muhatap.MuhatapGrubuAdı = GrupAdı;
                        muhatap.MuhatapAdı = MuhatapAdı;
                        muhatap.MuhatapGöbekAdı = _GrupİçindekiMuhatabınGöbekAdı_;
                    }
                    else
                    {
                        muhatap = Banka_Ortak.Sınıf_Aç(typeof(Muhatap_), dsy_muhatap_ay) as Muhatap_;
                        if (muhatap == null ||
                            muhatap.MuhatapGöbekAdı != _GrupİçindekiMuhatabınGöbekAdı_ ||
                            muhatap.MuhatapGrubuAdı != GrupAdı ||
                            muhatap.MuhatapAdı != MuhatapAdı
                            ) throw new Exception("Uyumsuzluk hatası." +
                                "İşyeri:" + İşyeriAdı +
                                ", muhatap == null:" + (muhatap == null) +
                                ", muhatap.MuhatapGöbekAdı:" + muhatap.MuhatapGöbekAdı +
                                ", _GrupİçindekiMuhatabınGöbekAdı_:" + _GrupİçindekiMuhatabınGöbekAdı_ +
                                ", muhatap.MuhatapGrubuAdı:" + muhatap.MuhatapGrubuAdı +
                                ", GrupAdı:" + GrupAdı +
                                ", muhatap.MuhatapAdı:" + muhatap.MuhatapAdı +
                                ", MuhatapAdı:" + MuhatapAdı);
                    }

                    muhatap.İşyeri = this;
                }
                
                Muhataplar.Add(_GrupİçindekiMuhatabınGöbekAdı_, muhatap);
                return muhatap;
            }
            public List<İşyeri_Ödeme_> Muhatap_Ödenmemiş_AvansÖdemeleri(string MuhatapAdı)
            {
                List<İşyeri_Ödeme_> liste = new List<İşyeri_Ödeme_>();

                foreach (string yıl in Ödemeler_Listele_Yıllar())
                {
                    İşyeri_BirYıllıkDönem_ BirYıllıkDönem = Ödemeler_Listele_BirYıllıkDönem(yıl);

                    foreach (İşyeri_Ödeme_ Ödeme in BirYıllıkDönem.Ödemeler)
                    {
                        if (Ödeme.MuhatapGrubuAdı == Çalışan_Yazısı && 
                            Ödeme.MuhatapAdı == MuhatapAdı && 
                            Ödeme.Tipi == İşyeri_Ödeme_İşlem_.Tipi_.AvansÖdemesi &&
                            !Ödeme.Durumu.ÖdendiMi())
                        {
                            liste.Add(Ödeme);
                        }
                    }
                }

                return liste;
            }
            public List<string> Ödemeler_Listele_Yıllar()
            {
                List<string> yıllar = new List<string>();

                if (Directory.Exists(İşyeri_Klasörü + "Ka"))
                {
                    string[] yıllar_dsy_lar = Directory.GetFiles(İşyeri_Klasörü + "Ka", "*.mup", SearchOption.TopDirectoryOnly);
                    foreach (string dsy in yıllar_dsy_lar)
                    {
                        yıllar.Add(Path.GetFileNameWithoutExtension(dsy));
                    }
                }

                return yıllar;
            }
            public İşyeri_BirYıllıkDönem_ Ödemeler_Listele_BirYıllıkDönem(string Yıl)
            {
                if (!Ödemeler.TryGetValue(Yıl, out İşyeri_BirYıllıkDönem_ BirYıllıkDönem))
                {
                    string dsy = GöbekAdı + "\\Ka\\" + Yıl;
                    if (Banka_Ortak.Sınıf_DosyaVarMı(dsy))
                    {
                        BirYıllıkDönem = Banka_Ortak.Sınıf_Aç(typeof(İşyeri_BirYıllıkDönem_), dsy) as İşyeri_BirYıllıkDönem_;
                        if (BirYıllıkDönem.Yıl != Yıl) throw new Exception("İşyeri: " + İşyeriAdı + " BirYıllıkDönem.Yıl(" + BirYıllıkDönem.Yıl + ") != Yıl(" + Yıl + ")");
                    }
                    else
                    {
                        BirYıllıkDönem = Banka_Ortak.Sınıf_Oluştur(typeof(İşyeri_BirYıllıkDönem_), null) as İşyeri_BirYıllıkDönem_;
                        BirYıllıkDönem.Yıl = Yıl;
                    }

                    BirYıllıkDönem.İşyeri = this;
                    Ödemeler.Add(Yıl, BirYıllıkDönem);
                }

                return BirYıllıkDönem;
            }
            public İşyeri_Ödeme_ Ödemeler_Bul(string GrupAdı, string MuhatapAdı, DateTime İlkİşlemTarihi)
            {
                if (GrupAdı.BoşMu(true) || MuhatapAdı.BoşMu(true) || İlkİşlemTarihi.Year < 2023) throw new ArgumentException("Girdiler hatalı " + GrupAdı + " " + MuhatapAdı + " " + İlkİşlemTarihi.Yazıya());

                İşyeri_BirYıllıkDönem_ BirYıllıkDönem = Ortak.Banka.Seçilenİşyeri.Ödemeler_Listele_BirYıllıkDönem(İlkİşlemTarihi.Year.Yazıya());
                IEnumerable<İşyeri_Ödeme_> bulunanlar = BirYıllıkDönem.Ödemeler.Where(x => x.MuhatapGrubuAdı == GrupAdı && x.MuhatapAdı == MuhatapAdı && x.İlkİşlemTarihi == İlkİşlemTarihi);
                if (bulunanlar.Count() == 1) return bulunanlar.First();

                return null; //taksitli ödemeye denk gelirse görmezden gel
            }
            public bool Üyelik_ZamanıGelenleriKaydet()
            {
                bool EnAz1ÜyelikÖdemesioluşturuldu = false;

                foreach (string MuhatapGrupAdı in MuhatapGrubu_Listele_Tümü())
                {
                    foreach (string MuhatapAdı in Muhatap_Listele_Tümü(MuhatapGrupAdı))
                    {
                        Muhatap_ muhatap = Muhatap_Aç(MuhatapGrupAdı, MuhatapAdı);
                        if (muhatap == null) continue;

                        EnAz1ÜyelikÖdemesioluşturuldu |= muhatap.Üyelik_ZamanıGelenleriKaydet();
                    }
                }

                return EnAz1ÜyelikÖdemesioluşturuldu;
            }
            public List<İşyeri_Ödeme_> Üyelik_OlacaklarıHesapla(DateOnly BitişTarihi, bool VeKaydet = false)
            {
                List<İşyeri_Ödeme_> Ödemeler = new List<İşyeri_Ödeme_>();

                foreach (string MuhatapGrupAdı in MuhatapGrubu_Listele_Tümü())
                {
                    foreach (string MuhatapAdı in Muhatap_Listele_Tümü(MuhatapGrupAdı))
                    {
                        Muhatap_ muhatap = Muhatap_Aç(MuhatapGrupAdı, MuhatapAdı);
                        if (muhatap == null) continue;

                        Ödemeler.AddRange(muhatap.Üyelik_OlacaklarıHesapla(BitişTarihi, VeKaydet));
                    }
                }

                return Ödemeler;
            }
            public void ToplamGelirGider_Güncelle(
                İşyeri_Ödeme_İşlem_.Tipi_ EskiTip, İşyeri_Ödeme_İşlem_.Durum_ EskiDurum, double EskiMiktar, 
                İşyeri_Ödeme_İşlem_.Tipi_ YeniTip, İşyeri_Ödeme_İşlem_.Durum_ YeniDurum, double YeniMiktar, 
                İşyeri_Ödeme_.ParaBirimi_ ParaBirimi)
            {
                if (EskiTip == YeniTip && EskiDurum == YeniDurum && EskiMiktar == YeniMiktar) return;

                if (EskiDurum == İşyeri_Ödeme_İşlem_.Durum_.İptalEdildi) EskiMiktar = 0;
                if (YeniDurum == İşyeri_Ödeme_İşlem_.Durum_.İptalEdildi) YeniMiktar = 0;
                int SıraNo = (int)ParaBirimi;

                if (EskiMiktar != 0)
                {
                    if (EskiTip.GelirMi())
                    {
                        ToplamGelir[SıraNo] -= EskiMiktar;

                        if (EskiDurum.ÖdendiMi())
                        {
                            ÖdenmişToplamGelir[SıraNo] -= EskiMiktar;
                        }
                    }
                    else
                    {
                        ToplamGider[SıraNo] -= EskiMiktar;

                        if (EskiDurum.ÖdendiMi())
                        {
                            ÖdenmişToplamGider[SıraNo] -= EskiMiktar;
                        }
                    }
                }

                if (YeniMiktar != 0)
                {
                    if (YeniTip.GelirMi())
                    {
                        ToplamGelir[SıraNo] += YeniMiktar;

                        if (YeniDurum.ÖdendiMi())
                        {
                            ÖdenmişToplamGelir[SıraNo] += YeniMiktar;
                        }
                    }
                    else
                    {
                        ToplamGider[SıraNo] += YeniMiktar;

                        if (YeniDurum.ÖdendiMi())
                        {
                            ÖdenmişToplamGider[SıraNo] += YeniMiktar;
                        }
                    }
                }

                DeğişiklikYapıldı = true;

                Banka_Ortak.Yazdır_Sıfırla();
            }
            public DateOnly EnYakınMaaşGünü()
            {
                DateOnly tt = DateOnly.FromDateTime(DateTime.Now);
                
                if (tt.Day > AylıkÜcretÖdemeGünü)
                {
                    //maaş günü geçti, sonraki aya kaydet
                    tt = tt.AddMonths(1);
                }
                tt = new DateOnly(tt.Year, tt.Month, AylıkÜcretÖdemeGünü);

                return tt;
            }
            public void KontrolNoktasıEkle(DateTime Tarihi, string Notları)
            {
                İşyeri_Ödeme_İşlem_ işlem = new İşyeri_Ödeme_İşlem_();
                işlem.ÖdemeninYapılacağıTarih = DateOnly.FromDateTime(Tarihi);
                işlem.Tipi = İşyeri_Ödeme_İşlem_.Tipi_.KontrolNoktası;
                işlem.Notlar = Notları;
                işlem.GerçekleştirenKullanıcıAdı = AnaKontrolcü.KullanıcıAdı;

                DateTime KayıtTarihi = DateTime.Now;
                İşyeri_Ödeme_ Ödeme = new İşyeri_Ödeme_();
                Ödeme.İşlem_Ekle(işlem, KayıtTarihi);

                İşyeri_BirYıllıkDönem_ BirYıllıkDönem = Ödemeler_Listele_BirYıllıkDönem(KayıtTarihi.Year.Yazıya());
                BirYıllıkDönem.Ödemeler.AddRange(new List<İşyeri_Ödeme_>() { Ödeme });
                BirYıllıkDönem.DeğişiklikYapıldı = true;
            }
            #endregion

            #region Kayıt
            string Banka_Ortak.IBanka_Tanımlayıcı_.SınıfAdı { get => _SınıfAdı_; set => _SınıfAdı_ = value; }
            [Değişken_.Niteliği.Adını_Değiştir("A", 0)] string _SınıfAdı_;
            [Değişken_.Niteliği.Adını_Değiştir("A", 1)] public string İşyeriAdı;
            [Değişken_.Niteliği.Adını_Değiştir("A", 2)] public string GöbekAdı;
            [Değişken_.Niteliği.Bunu_Kesinlikle_Kullanma] public bool DeğişiklikYapıldı;
            public bool Kaydet()
            {
                bool EnAz1DeğişiklikKaydedildi = false;

                foreach (İşyeri_BirYıllıkDönem_ YılaGöreÖdemeler in Ödemeler.Values)
                {
                    EnAz1DeğişiklikKaydedildi |= YılaGöreÖdemeler.Kaydet(GöbekAdı);
                }

                foreach (Muhatap_ muhatap in Muhataplar.Values)
                {
                    EnAz1DeğişiklikKaydedildi |= muhatap.Kaydet(GöbekAdı);
                }

                if (DeğişiklikYapıldı)
                {
                    Banka_Ortak.Sınıf_Kaydet(this, GöbekAdı + "\\İy");
                    EnAz1DeğişiklikKaydedildi |= true;

                    DeğişiklikYapıldı = false;
                }

                return EnAz1DeğişiklikKaydedildi;
            }
            #endregion
        }
        public class İşyeri_BirYıllıkDönem_ : Banka_Ortak.IBanka_Tanımlayıcı_
        {
            [Değişken_.Niteliği.Bunu_Kesinlikle_Kullanma] public İşyeri_ İşyeri;
            [Değişken_.Niteliği.Adını_Değiştir("Y")] public string Yıl;
            [Değişken_.Niteliği.Adını_Değiştir("T")] public List<İşyeri_Ödeme_> Ödemeler = new List<İşyeri_Ödeme_>();

            #region Kayıt
            string Banka_Ortak.IBanka_Tanımlayıcı_.SınıfAdı { get => _SınıfAdı_; set => _SınıfAdı_ = value; }
            [Değişken_.Niteliği.Adını_Değiştir("A", 0)] string _SınıfAdı_;
            [Değişken_.Niteliği.Bunu_Kesinlikle_Kullanma] public bool DeğişiklikYapıldı;
            public bool Kaydet(string İşyeriGöbekAdı)
            {
                if (!DeğişiklikYapıldı) return false;

                Banka_Ortak.Sınıf_Kaydet(this, İşyeriGöbekAdı + "\\Ka\\" + Yıl);
                DeğişiklikYapıldı = false;
                return true;
            }
            #endregion
        }
        public class İşyeri_Ödeme_
        {
            [Değişken_.Niteliği.Adını_Değiştir("Ö", 0)] public string MuhatapGrubuAdı;
            [Değişken_.Niteliği.Adını_Değiştir("Ö", 1)] public string MuhatapAdı;
            [Değişken_.Niteliği.Adını_Değiştir("Ö", 2)] public ParaBirimi_ ParaBirimi;
            [Değişken_.Niteliği.Adını_Değiştir("Ö", 3)] public DateTime? Üyelik_KayıtTarihi;
            [Değişken_.Niteliği.Bunu_Kesinlikle_Kullanma] public bool Üyelik_HenüzKaydedilmemişBirÖdeme;

            [Değişken_.Niteliği.Adını_Değiştir("T")] public İşyeri_Ödeme_Taksit_ Taksit;
            [Değişken_.Niteliği.Adını_Değiştir("G")] public Dictionary<DateTime, İşyeri_Ödeme_İşlem_> İşlemler = new Dictionary<DateTime, İşyeri_Ödeme_İşlem_>();

            #region İşlemler
            public enum ParaBirimi_ { Boşta, TürkLirası, Avro, Dolar, ElemanSayısı = 4 };
            public string Yazdır_Miktarı()
            {
                return Banka_Ortak.Yazdır_Ücret(Miktarı, ParaBirimi);
            }
            public İşyeri_Ödeme_İşlem_.Tipi_ Tipi
            {
                get
                {
                    return İşlemler.Last().Value.Tipi;
                }
            }
            public İşyeri_Ödeme_İşlem_.Durum_ Durumu
            {
                get
                {
                    return İşlemler.Last().Value.Durumu;
                }
            }
            public bool GeciktiMi
            {
                get
                {
                    İşyeri_Ödeme_İşlem_ son_işlem = İşlemler.Last().Value;
                    return !son_işlem.Durumu.ÖdendiMi() &&
                           son_işlem.ÖdemeninYapılacağıTarih <= DateOnly.FromDateTime(DateTime.Now);
                }
            }
            public DateTime İlkİşlemTarihi
            {
                get
                {
                    return İşlemler.First().Key;
                }
            }
            public DateTime SonİşlemTarihi
            {
                get
                {
                    return İşlemler.Last().Key;
                }
            }
            public double Miktarı
            {
                get
                {
                    return İşlemler.Last().Value.Miktarı;
                }
            }
            public DateOnly ÖdemeninYapılacağıTarih
            {
                get
                {
                    return İşlemler.Last().Value.ÖdemeninYapılacağıTarih;
                }
            }
            public string Notlar
            {
                get
                {
                    //ilk ve son notlar aynı ise ikinci kez kaydedilmemesi için null olarak kaydedilenleri atla
                    for (int i = İşlemler.Count - 1; i >= 0; i--)
                    {
                        İşyeri_Ödeme_İşlem_ işlem = İşlemler.Values.ElementAt(i);
                        if (işlem.Notlar.DoluMu()) return işlem.Notlar;
                    }

                    throw new Exception("Ödemenin notları boş olmamalı " + MuhatapGrubuAdı + " " + MuhatapAdı + " " + İlkİşlemTarihi.Yazıya());
                }
            }
            public string GerçekleştirenKullanıcıAdı
            {
                get
                {
                    return İşlemler.Last().Value.GerçekleştirenKullanıcıAdı;
                }
            }
            public void İşlem_Ekle(İşyeri_Ödeme_İşlem_ İşlem, DateTime Anahtar)
            {
                while (İşlemler.ContainsKey(Anahtar)) Anahtar = Anahtar.AddMilliseconds(1);

                İşlemler.Add(Anahtar, İşlem);
            }

            public void YeniİşlemEkle(İşyeri_Ödeme_İşlem_.Tipi_ Yeni_Tip, İşyeri_Ödeme_İşlem_.Durum_ Yeni_Durum, double Yeni_Miktar, string Yeni_Notlar = null, DateOnly? Yeni_ÖdemeTarihi = null, DateTime? Yeni_KayıtTarihi = null)
            {
                İşyeri_BirYıllıkDönem_ BirYıllıkDönem = Ortak.Banka.Seçilenİşyeri.Ödemeler_Listele_BirYıllıkDönem(İlkİşlemTarihi.Year.Yazıya());

                if (Üyelik_HenüzKaydedilmemişBirÖdeme)
                {
                    Üyelik_HenüzKaydedilmemişBirÖdeme = false;

                    Muhatap_ muhatap = Ortak.Banka.Seçilenİşyeri.Muhatap_Aç(MuhatapGrubuAdı, MuhatapAdı, true);
                    muhatap.Üyelik_SisteminTetiklemesiniEngelle(Üyelik_KayıtTarihi.Value, ÖdemeninYapılacağıTarih);
                    muhatap.GelirGider_Ekle(new List<İşyeri_Ödeme_>() { this }, BirYıllıkDönem);
                }
                else if (!BirYıllıkDönem.Ödemeler.Contains(this)) throw new Exception("Döneme ait olmayan ödeme işlemi - YeniİşlemEkle");

                BirYıllıkDönem.İşyeri.ToplamGelirGider_Güncelle(
                    Tipi, Durumu, Miktarı,
                    Yeni_Tip, Yeni_Durum, Yeni_Miktar,
                    ParaBirimi);

                if (Yeni_Notlar == Notlar) Yeni_Notlar = null;
                if (Yeni_ÖdemeTarihi == null) Yeni_ÖdemeTarihi = ÖdemeninYapılacağıTarih;
                if (Yeni_KayıtTarihi == null) Yeni_KayıtTarihi = DateTime.Now;

                İşyeri_Ödeme_İşlem_ işlem = new İşyeri_Ödeme_İşlem_();
                işlem.Tipi = Yeni_Tip;
                işlem.Durumu = Yeni_Durum;
                işlem.Miktarı = Yeni_Miktar;
                işlem.Notlar = Yeni_Notlar;
                işlem.ÖdemeninYapılacağıTarih = Yeni_ÖdemeTarihi.Value;
                işlem.GerçekleştirenKullanıcıAdı = AnaKontrolcü.KullanıcıAdı;

                İşlem_Ekle(işlem, Yeni_KayıtTarihi.Value);
                BirYıllıkDönem.DeğişiklikYapıldı = true;
            }
            public void Öde(double ÖdenilenMiktar, ParaBirimi_ ÖdenilenParaBirimi, string Notları = null, DateTime? KalanÖdemeninYapılacağıTarih = null, DateTime? KayıtTarihi = null)
            {
                İşyeri_BirYıllıkDönem_ BirYıllıkDönem = Ortak.Banka.Seçilenİşyeri.Ödemeler_Listele_BirYıllıkDönem(İlkİşlemTarihi.Year.Yazıya());
                Muhatap_ muhatap = BirYıllıkDönem.İşyeri.Muhatap_Aç(MuhatapGrubuAdı, MuhatapAdı, true);
                
                if (Üyelik_HenüzKaydedilmemişBirÖdeme)
                {
                    Üyelik_HenüzKaydedilmemişBirÖdeme = false;

                    muhatap.Üyelik_SisteminTetiklemesiniEngelle(Üyelik_KayıtTarihi.Value, ÖdemeninYapılacağıTarih);
                    muhatap.GelirGider_Ekle(new List<İşyeri_Ödeme_>() { this }, BirYıllıkDönem);
                }

                if (KalanÖdemeninYapılacağıTarih == null) KalanÖdemeninYapılacağıTarih = ÖdemeninYapılacağıTarih.ToDateTime(new TimeOnly());
                DateOnly KalanÖdemeninYapılacağıTarih_do = DateOnly.FromDateTime(KalanÖdemeninYapılacağıTarih.Value);
                if (KayıtTarihi == null) KayıtTarihi = DateTime.Now;

                if (ÖdenilenMiktar == Miktarı && ÖdenilenParaBirimi == ParaBirimi)
                {
                    YeniİşlemEkle(Tipi, İşyeri_Ödeme_İşlem_.Durum_.TamÖdendi, Miktarı, Notları, KalanÖdemeninYapılacağıTarih_do, KayıtTarihi);
                }
                else
                {
                    double KısmiÖdemeMiktarı_TürkLirasıOlarak = Ortak.ParaBirimi_Dönüştür(ÖdenilenMiktar, ÖdenilenParaBirimi, ParaBirimi_.TürkLirası);
                    double TamÖdemeMiktarı_TürkLirasıOlarak = Ortak.ParaBirimi_Dönüştür(Miktarı, ParaBirimi, ParaBirimi_.TürkLirası);
                    double KalanÖdemeMiktarı_TürkLirasıOlarak = TamÖdemeMiktarı_TürkLirasıOlarak - KısmiÖdemeMiktarı_TürkLirasıOlarak;
                    if (KalanÖdemeMiktarı_TürkLirasıOlarak < 0) throw new Exception("KısmiÖdemeMiktarı > TamÖdemeMiktarı");
                    double KalanÖdemeMiktarı_ÖdemeParaBiriminde = Ortak.ParaBirimi_Dönüştür(KalanÖdemeMiktarı_TürkLirasıOlarak, ParaBirimi_.TürkLirası, ParaBirimi);
                    double KısmiÖdemeMiktarı_ÖdemeParaBiriminde = Ortak.ParaBirimi_Dönüştür(ÖdenilenMiktar, ÖdenilenParaBirimi, ParaBirimi);

                    //ana ödemenin içine kısmi ödemenin işlenmesi
                    YeniİşlemEkle(Tipi, İşyeri_Ödeme_İşlem_.Durum_.KısmenÖdendi, KalanÖdemeMiktarı_ÖdemeParaBiriminde, Notları, KalanÖdemeninYapılacağıTarih_do, KayıtTarihi);

                    //ana ödemeden bağımsız tam ödeme oluşturulması
                    string açıklama = ParaBirimi != ÖdenilenParaBirimi ? "(" + Banka_Ortak.Yazdır_Ücret(ÖdenilenMiktar, ÖdenilenParaBirimi) + ")" : null;
                    açıklama += (açıklama.DoluMu() ? " " : null) + Notlar;
                    muhatap.GelirGider_Ekle(muhatap.GelirGider_Oluştur_KısmiÖdeme(Tipi, KayıtTarihi.Value, KısmiÖdemeMiktarı_ÖdemeParaBiriminde, ParaBirimi, KalanÖdemeninYapılacağıTarih.Value, açıklama, Taksit, Üyelik_KayıtTarihi));
                }
            }
            #endregion
        }
        public class İşyeri_Ödeme_İşlem_
        {
            public enum Tipi_ { Boşta, Gider, MaaşÖdemesi, AvansVerilmesi, Gelir, AvansÖdemesi, KontrolNoktası } //Gelir vaya gider olarak sınıflandırılabilen alt sınıflar
            public enum Durum_ { Boşta, Ödenmedi, KısmenÖdendi, TamÖdendi, İptalEdildi, KısmiÖdemeYapıldı, PeşinatÖdendi }

            [Değişken_.Niteliği.Adını_Değiştir("İ", 0)] public Tipi_ Tipi;
            [Değişken_.Niteliği.Adını_Değiştir("İ", 1)] public Durum_ Durumu;
            [Değişken_.Niteliği.Adını_Değiştir("İ", 2)] public double Miktarı;
            [Değişken_.Niteliği.Adını_Değiştir("İ", 3)] public string Notlar;
            [Değişken_.Niteliği.Adını_Değiştir("İ", 4)] public DateOnly ÖdemeninYapılacağıTarih;
            [Değişken_.Niteliği.Adını_Değiştir("İ", 5)] public string GerçekleştirenKullanıcıAdı;
        }
        public class İşyeri_Ödeme_Taksit_
        {
            [Değişken_.Niteliği.Adını_Değiştir("T", 0)] public int Taksit_Sayısı;
            [Değişken_.Niteliği.Adını_Değiştir("T", 1)] public int Taksit_No;
            [Değişken_.Niteliği.Adını_Değiştir("T", 2)] public Muhatap_Üyelik_.Dönem_ Dönemi;
            [Değişken_.Niteliği.Adını_Değiştir("T", 3)] public int Dönem_Adet; //Her <Adet> dönemde 1 kez
        }

        public class Muhatap_ : Banka_Ortak.IBanka_Tanımlayıcı_
        {
            [Değişken_.Niteliği.Bunu_Kesinlikle_Kullanma] public İşyeri_ İşyeri;
            [Değişken_.Niteliği.Adını_Değiştir("N")] public string Notlar;
            [Değişken_.Niteliği.Adını_Değiştir("Ü")] public Dictionary<DateTime, Muhatap_Üyelik_> Üyelikler; //KayıtTarihi ve detayları
            [Değişken_.Niteliği.Adını_Değiştir("Ç")] public Muhatap_Çalışan_ Çalışan;

            #region İşlemler
            public bool Üyelik_ZamanıGelenleriKaydet()
            {
                List<İşyeri_Ödeme_> ödemeler = Üyelik_OlacaklarıHesapla(DateOnly.FromDateTime(DateTime.Now), true);
                if (ödemeler.Count <= 0) return false;
                
                GelirGider_Ekle(ödemeler);
                DeğişiklikYapıldı = true;
                return true;
            }
            public List<İşyeri_Ödeme_> Üyelik_OlacaklarıHesapla(DateOnly BitişTarihi, bool VeKaydet)
            {
                List<İşyeri_Ödeme_> ödemeler_tümü = new List<İşyeri_Ödeme_>();

                if (!MuhatapGrubuAdı.StartsWith(Ortak.GizliElemanBaşlangıcı) && 
                    !MuhatapAdı.StartsWith(Ortak.GizliElemanBaşlangıcı) &&
                    Üyelikler != null)
                {
                    foreach (KeyValuePair<DateTime, Muhatap_Üyelik_> üyelik in Üyelikler)
                    {
                        List<İşyeri_Ödeme_> ödemeler_üyelik = new List<İşyeri_Ödeme_>();
                        DateOnly? ÜyelikBitişTarihi = üyelik.Value.BitişTarihi == null ? DateOnly.MaxValue : üyelik.Value.BitişTarihi;
                        DateOnly İlkÖdemeTarihi = üyelik.Value.İlkÖdemeninYapılacağıTarih;

                        İşyeri_Ödeme_Taksit_ taksit = üyelik.Value.Taksit;
                        if (taksit == null) taksit = new İşyeri_Ödeme_Taksit_() { Taksit_Sayısı = 1 };

                        while (ÜyelikBitişTarihi >= İlkÖdemeTarihi && İlkÖdemeTarihi <= BitişTarihi)
                        {
                            bool Üret = false;
                            string notlar = null;
                            if (üyelik.Value.Tipi == İşyeri_Ödeme_İşlem_.Tipi_.MaaşÖdemesi)
                            {
                                if (MuhatapGrubuAdı == Çalışan_Yazısı && Çalışan != null && Çalışan.İştenAyrılışTarihi == null && üyelik.Value.Miktarı > 0)
                                {
                                    Üret = true;
                                    notlar = İlkÖdemeTarihi.Yazıya("MMM yyyy", System.Threading.Thread.CurrentThread.CurrentCulture);
                                }
                            }
                            else
                            {
                                Üret = true;
                                notlar = üyelik.Value.Notlar;
                            }

                            if (Üret) ödemeler_üyelik.AddRange(GelirGider_Oluştur(üyelik.Value.Tipi, İşyeri_Ödeme_İşlem_.Durum_.Ödenmedi, üyelik.Value.Miktarı, üyelik.Value.ParaBirimi, İlkÖdemeTarihi.ToDateTime(new TimeOnly()), notlar, taksit.Taksit_Sayısı, taksit.Dönemi, taksit.Dönem_Adet, üyelik.Key, BitişTarihi));
                            
                            İlkÖdemeTarihi = Banka_Ortak.SonrakiTarihiHesapla(İlkÖdemeTarihi, üyelik.Value.Dönemi, üyelik.Value.Dönem_Adet);

                            if (VeKaydet) üyelik.Value.İlkÖdemeninYapılacağıTarih = İlkÖdemeTarihi;
                        }

                        if (üyelik.Value.ZamanıGelmedenİşlemYapılan_İlkÖdemeTarihleri != null)
                        {
                            foreach (DateOnly ÖdemeGünü in üyelik.Value.ZamanıGelmedenİşlemYapılan_İlkÖdemeTarihleri)
                            {
                                ödemeler_üyelik.RemoveAll(x => x.ÖdemeninYapılacağıTarih == ÖdemeGünü);
                            }

                            if (VeKaydet) üyelik.Value.ZamanıGelmedenİşlemYapılan_İlkÖdemeTarihleri.RemoveAll(x => x < üyelik.Value.İlkÖdemeninYapılacağıTarih);
                        }

                        ödemeler_tümü.AddRange(ödemeler_üyelik);
                    }

                    foreach (İşyeri_Ödeme_ ödeme in ödemeler_tümü)
                    {
                        ödeme.İşlemler.Last().Value.GerçekleştirenKullanıcıAdı = Ortak.Sistem_KullanıcıAdı;
                        ödeme.Üyelik_HenüzKaydedilmemişBirÖdeme = !VeKaydet;
                    }
                }

                return ödemeler_tümü;
            }
            public void Üyelik_SisteminTetiklemesiniEngelle(DateTime ÜyelikKayıtTarihi, DateOnly İlkÖdemeninYapılacağıTarih)
            {
                Muhatap_Üyelik_ üyelik = Üyelikler[ÜyelikKayıtTarihi];
                if (üyelik.ZamanıGelmedenİşlemYapılan_İlkÖdemeTarihleri == null) üyelik.ZamanıGelmedenİşlemYapılan_İlkÖdemeTarihleri = new List<DateOnly>();

                üyelik.ZamanıGelmedenİşlemYapılan_İlkÖdemeTarihleri.Add(İlkÖdemeninYapılacağıTarih);

                DeğişiklikYapıldı = true;
            }
            public DateTime Üyelik_Ekle(İşyeri_Ödeme_İşlem_.Tipi_ Tipi, double Miktar, İşyeri_Ödeme_.ParaBirimi_ ParaBirimi,
                    DateTime İlkÖdemeTarihi, string Notlar,
                    int Taksit_Sayısı, Muhatap_Üyelik_.Dönem_ Taksit_Dönem, int Taksit_Dönem_Adet,
                    Muhatap_Üyelik_.Dönem_ Üyelik_Dönem, int Üyelik_Dönem_Adet, DateTime? ÜyelikBitişTarihi, bool ZamanıGelenleriKaydet)
            {
                Muhatap_Üyelik_ üyelik = new Muhatap_Üyelik_();
                üyelik.Tipi = Tipi;
                üyelik.İlkÖdemeninYapılacağıTarih = DateOnly.FromDateTime(İlkÖdemeTarihi);
                üyelik.Miktarı = Miktar;
                üyelik.ParaBirimi = ParaBirimi;
                üyelik.Dönemi = Üyelik_Dönem;
                üyelik.Dönem_Adet = Üyelik_Dönem_Adet;
                üyelik.Notlar = Notlar;
                üyelik.GerçekleştirenKullanıcıAdı = AnaKontrolcü.KullanıcıAdı;
                üyelik.BitişTarihi = ÜyelikBitişTarihi == null ? null : DateOnly.FromDateTime(ÜyelikBitişTarihi.Value);

                if (Taksit_Sayısı > 1)
                {
                    İşyeri_Ödeme_Taksit_ taksit = new İşyeri_Ödeme_Taksit_();
                    taksit.Taksit_Sayısı = Taksit_Sayısı;
                    taksit.Taksit_No = 0;
                    taksit.Dönemi = Taksit_Dönem;
                    taksit.Dönem_Adet = Taksit_Dönem_Adet;

                    üyelik.Taksit = taksit;
                }

                if (Üyelikler == null) Üyelikler = new Dictionary<DateTime, Muhatap_Üyelik_>();
                DateTime KayıtTarihi = DateTime.Now;
                while (Üyelikler.ContainsKey(KayıtTarihi)) KayıtTarihi = KayıtTarihi.AddMilliseconds(1);
                Üyelikler.Add(KayıtTarihi, üyelik);
                DeğişiklikYapıldı = true;

                if (ZamanıGelenleriKaydet) Üyelik_ZamanıGelenleriKaydet();

                return KayıtTarihi;
            }
            public void Üyelik_Düzenle(DateTime ÜyelikKayıtTarihi, 
                    İşyeri_Ödeme_İşlem_.Tipi_ Tipi, double Miktar, İşyeri_Ödeme_.ParaBirimi_ ParaBirimi,
                    DateTime İlkÖdemeTarihi, string Notlar,
                    int Taksit_Sayısı, Muhatap_Üyelik_.Dönem_ Taksit_Dönem, int Taksit_Dönem_Adet,
                    Muhatap_Üyelik_.Dönem_ Üyelik_Dönem, int Üyelik_Dönem_Adet, DateTime? ÜyelikBitişTarihi)
            {
                DateTime ÜyelikKayıtTarihi_Üretilen = Üyelik_Ekle(Tipi, Miktar, ParaBirimi,
                    İlkÖdemeTarihi, Notlar,
                    Taksit_Sayısı, Taksit_Dönem, Taksit_Dönem_Adet,
                    Üyelik_Dönem, Üyelik_Dönem_Adet, ÜyelikBitişTarihi, false);

                Muhatap_Üyelik_ yeni = Üyelikler[ÜyelikKayıtTarihi_Üretilen];
                Muhatap_Üyelik_ eski = Üyelikler[ÜyelikKayıtTarihi];

                yeni.ZamanıGelmedenİşlemYapılan_İlkÖdemeTarihleri = eski.ZamanıGelmedenİşlemYapılan_İlkÖdemeTarihleri;

                Üyelikler.Remove(ÜyelikKayıtTarihi_Üretilen);
                Üyelikler[ÜyelikKayıtTarihi] = yeni;

                Üyelik_ZamanıGelenleriKaydet();
            }

            public List<İşyeri_Ödeme_> GelirGider_Oluştur(İşyeri_Ödeme_İşlem_.Tipi_ Tipi, İşyeri_Ödeme_İşlem_.Durum_ Durumu, double Miktar, İşyeri_Ödeme_.ParaBirimi_ ParaBirimi,
                    DateTime İlkÖdemeTarihi, string Notlar,
                    int Taksit_Sayısı, Muhatap_Üyelik_.Dönem_ Taksit_Dönem, int Taksit_Dönem_Adet,
                    DateTime? ÜyelikKayıtTarihi, DateOnly? SonÖdemeTarihi = null, DateTime? KayıtTarihi = null)
            {
                if (KayıtTarihi == null) KayıtTarihi = DateTime.Now;
                List<İşyeri_Ödeme_> Liste = new List<İşyeri_Ödeme_>();

                if (Taksit_Sayısı <= 1)
                {
                    DateOnly ÖdemeninYapılacağıTarih = DateOnly.FromDateTime(İlkÖdemeTarihi);
                    if (SonÖdemeTarihi == null || ÖdemeninYapılacağıTarih <= SonÖdemeTarihi)
                    {
                        İşyeri_Ödeme_İşlem_ işlem = new İşyeri_Ödeme_İşlem_();
                        işlem.Tipi = Tipi;
                        işlem.Durumu = Durumu;
                        işlem.Miktarı = Miktar;
                        işlem.Notlar = Notlar;
                        işlem.ÖdemeninYapılacağıTarih = ÖdemeninYapılacağıTarih;
                        işlem.GerçekleştirenKullanıcıAdı = AnaKontrolcü.KullanıcıAdı;

                        İşyeri_Ödeme_ Ödeme = new İşyeri_Ödeme_();
                        Ödeme.MuhatapGrubuAdı = MuhatapGrubuAdı;
                        Ödeme.MuhatapAdı = MuhatapAdı;
                        Ödeme.ParaBirimi = ParaBirimi;
                        Ödeme.Üyelik_KayıtTarihi = ÜyelikKayıtTarihi;
                        Ödeme.İşlem_Ekle(işlem, KayıtTarihi.Value);

                        Liste.Add(Ödeme);
                    }
                }
                else
                {
                    double Taksit_miktarı = Miktar / Taksit_Sayısı;

                    for (int i = 0; i < Taksit_Sayısı; i++)
                    {
                        DateOnly ÖdemeninYapılacağıTarih = DateOnly.FromDateTime(İlkÖdemeTarihi);
                        if (SonÖdemeTarihi != null && ÖdemeninYapılacağıTarih > SonÖdemeTarihi) break;
                        
                        İşyeri_Ödeme_Taksit_ taksit = new İşyeri_Ödeme_Taksit_();
                        taksit.Taksit_Sayısı = Taksit_Sayısı;
                        taksit.Taksit_No = i + 1;
                        taksit.Dönemi = Taksit_Dönem;
                        taksit.Dönem_Adet = Taksit_Dönem_Adet;

                        İşyeri_Ödeme_İşlem_ işlem = new İşyeri_Ödeme_İşlem_();
                        işlem.Tipi = Tipi;
                        işlem.Durumu = Durumu;
                        işlem.Miktarı = Taksit_miktarı;
                        işlem.Notlar = Notlar;
                        işlem.ÖdemeninYapılacağıTarih = ÖdemeninYapılacağıTarih;
                        işlem.GerçekleştirenKullanıcıAdı = AnaKontrolcü.KullanıcıAdı;

                        İşyeri_Ödeme_ Ödeme = new İşyeri_Ödeme_();
                        Ödeme.MuhatapGrubuAdı = MuhatapGrubuAdı;
                        Ödeme.MuhatapAdı = MuhatapAdı;
                        Ödeme.ParaBirimi = ParaBirimi;
                        Ödeme.Üyelik_KayıtTarihi = ÜyelikKayıtTarihi;
                        Ödeme.İşlem_Ekle(işlem, KayıtTarihi.Value);
                        Ödeme.Taksit = taksit;

                        Liste.Add(Ödeme);
                        İlkÖdemeTarihi = Banka_Ortak.SonrakiTarihiHesapla(İlkÖdemeTarihi, Taksit_Dönem, Taksit_Dönem_Adet);
                    }
                }

                return Liste;
            }
            public List<İşyeri_Ödeme_> GelirGider_Oluştur_KısmiÖdeme(İşyeri_Ödeme_İşlem_.Tipi_ Tipi, DateTime KayıtTarihi, double Miktar, İşyeri_Ödeme_.ParaBirimi_ ParaBirimi,
                    DateTime İlkÖdemeTarihi, string Notlar,
                    İşyeri_Ödeme_Taksit_ Taksit, DateTime? ÜyelikKayıtTarihi)
            {
                İşyeri_Ödeme_İşlem_ işlem = new İşyeri_Ödeme_İşlem_();
                işlem.Tipi = Tipi;
                işlem.Durumu = İşyeri_Ödeme_İşlem_.Durum_.KısmiÖdemeYapıldı;
                işlem.Miktarı = Miktar;
                işlem.Notlar = Notlar;
                işlem.ÖdemeninYapılacağıTarih = DateOnly.FromDateTime(İlkÖdemeTarihi);
                işlem.GerçekleştirenKullanıcıAdı = AnaKontrolcü.KullanıcıAdı;

                İşyeri_Ödeme_ Ödeme = new İşyeri_Ödeme_();
                Ödeme.MuhatapGrubuAdı = MuhatapGrubuAdı;
                Ödeme.MuhatapAdı = MuhatapAdı;
                Ödeme.ParaBirimi = ParaBirimi;
                Ödeme.Taksit = Taksit;
                Ödeme.Üyelik_KayıtTarihi = ÜyelikKayıtTarihi;
                Ödeme.İşlem_Ekle(işlem, KayıtTarihi);

                return new List<İşyeri_Ödeme_>() { Ödeme };
            }
            public void GelirGider_Ekle(List<İşyeri_Ödeme_> Ödemeler, İşyeri_BirYıllıkDönem_ BirYıllıkDönem = null)
            {
                if (BirYıllıkDönem == null) BirYıllıkDönem = İşyeri.Ödemeler_Listele_BirYıllıkDönem(DateTime.Now.Year.Yazıya());

                BirYıllıkDönem.Ödemeler.AddRange(Ödemeler);
                BirYıllıkDönem.DeğişiklikYapıldı = true;

                foreach (İşyeri_Ödeme_ ödeme in Ödemeler)
                {
                    İşyeri.ToplamGelirGider_Güncelle(
                        ödeme.Tipi, ödeme.Durumu, 0, 
                        ödeme.Tipi, ödeme.Durumu, ödeme.Miktarı, 
                        ödeme.ParaBirimi);
                }
            }
            
            public Muhatap_Üyelik_ Maaş_Üyelik_Detaylar(out DateTime ÜyelikKayıtTarihi)
            {
                if (Üyelikler == null) Üyelikler = new Dictionary<DateTime, Muhatap_Üyelik_>();
                
                KeyValuePair<DateTime, Muhatap_Üyelik_> detaylar = Üyelikler.FirstOrDefault(x => x.Value.Tipi == İşyeri_Ödeme_İşlem_.Tipi_.MaaşÖdemesi);
                if (detaylar.Value == null)
                {
                    DateTime kayıt_tarihi = Üyelik_Ekle(İşyeri_Ödeme_İşlem_.Tipi_.MaaşÖdemesi, 0, İşyeri_Ödeme_.ParaBirimi_.TürkLirası, İşyeri.EnYakınMaaşGünü().ToDateTime(new TimeOnly()), null,
                        0, Muhatap_Üyelik_.Dönem_.Boşta, 0,
                        Muhatap_Üyelik_.Dönem_.Aylık, 1, null, false);

                    detaylar = Üyelikler.FirstOrDefault(x => x.Value.Tipi == İşyeri_Ödeme_İşlem_.Tipi_.MaaşÖdemesi);
                    detaylar.Value.GerçekleştirenKullanıcıAdı = Ortak.Sistem_KullanıcıAdı;
                }

                ÜyelikKayıtTarihi = detaylar.Key;
                return detaylar.Value;
            }
            public void Maaş_Üyelik_Değiştir(DateTime ÜyelikKayıtTarihi, Muhatap_Üyelik_ Üyelik)
            {
                if (Üyelik.Taksit == null) Üyelik.Taksit = new İşyeri_Ödeme_Taksit_() { Taksit_Sayısı = 0 };

                Üyelik_Düzenle(ÜyelikKayıtTarihi,
                    Üyelik.Tipi, Üyelik.Miktarı, Üyelik.ParaBirimi, Üyelik.İlkÖdemeninYapılacağıTarih.ToDateTime(new TimeOnly()), Üyelik.Notlar,
                    Üyelik.Taksit.Taksit_Sayısı, Üyelik.Taksit.Dönemi, Üyelik.Taksit.Dönem_Adet,
                    Üyelik.Dönemi, Üyelik.Dönem_Adet, Üyelik.BitişTarihi == null ? null : Üyelik.BitişTarihi.Value.ToDateTime(new TimeOnly()));
            }
            #endregion

            #region Kayıt
            string Banka_Ortak.IBanka_Tanımlayıcı_.SınıfAdı { get => _SınıfAdı_; set => _SınıfAdı_ = value; }
            [Değişken_.Niteliği.Adını_Değiştir("A", 0)] string _SınıfAdı_;
            [Değişken_.Niteliği.Adını_Değiştir("A", 1)] public string MuhatapGrubuAdı;
            [Değişken_.Niteliği.Adını_Değiştir("A", 2)] public string MuhatapAdı;
            [Değişken_.Niteliği.Adını_Değiştir("A", 3)] public string MuhatapGöbekAdı;
            [Değişken_.Niteliği.Bunu_Kesinlikle_Kullanma] public bool DeğişiklikYapıldı;
            public bool Kaydet(string İşyeriGöbekAdı)
            {
                if (!DeğişiklikYapıldı) return false;

                Banka_Ortak.Sınıf_Kaydet(this, İşyeriGöbekAdı + "\\Mu\\" + MuhatapGöbekAdı + "\\Ay");
                  
                DeğişiklikYapıldı = false;
                return true;
            }
            #endregion
        }
        public class Muhatap_Üyelik_
        {
            public enum Dönem_ { Boşta, Günlük, Haftalık, Aylık, Yıllık };
            public string Yazdır_Dönem()
            {
                string açıklama = "Her " + Dönem_Adet + " " + (Dönemi == Dönem_.Günlük ? "günde" : (Dönemi == Dönem_.Haftalık ? "haftada" : (Dönemi == Dönem_.Aylık ? "ayda" : Dönemi == Dönem_.Yıllık ? "yılda" : throw new Exception("Dönem(" + Dönemi + ") hatalı")))) + " 1 kez";
                if (Taksit != null) açıklama += Environment.NewLine + "Her " + Taksit.Dönem_Adet + " " + (Taksit.Dönemi == Dönem_.Günlük ? "günde" : (Taksit.Dönemi == Dönem_.Haftalık ? "haftada" : (Taksit.Dönemi == Dönem_.Aylık ? "ayda" : Taksit.Dönemi == Dönem_.Yıllık ? "yılda" : throw new Exception("Taksit.Dönemi(" + Taksit.Dönemi + ") hatalı")))) + " " + Taksit.Taksit_Sayısı + " taksit";

                return açıklama;
            }

            [Değişken_.Niteliği.Adını_Değiştir("Ü", 0)] public DateOnly İlkÖdemeninYapılacağıTarih;
            [Değişken_.Niteliği.Adını_Değiştir("Ü", 1)] public İşyeri_Ödeme_İşlem_.Tipi_ Tipi;
            [Değişken_.Niteliği.Adını_Değiştir("Ü", 2)] public double Miktarı;
            [Değişken_.Niteliği.Adını_Değiştir("Ü", 3)] public İşyeri_Ödeme_.ParaBirimi_ ParaBirimi;
            [Değişken_.Niteliği.Adını_Değiştir("Ü", 4)] public Dönem_ Dönemi;
            [Değişken_.Niteliği.Adını_Değiştir("Ü", 5)] public int Dönem_Adet; //Her <Adet> dönemde 1 kez
            [Değişken_.Niteliği.Adını_Değiştir("Ü", 6)] public string Notlar;
            [Değişken_.Niteliği.Adını_Değiştir("Ü", 7)] public string GerçekleştirenKullanıcıAdı;
            [Değişken_.Niteliği.Adını_Değiştir("Ü", 8)] public DateOnly? BitişTarihi;

            [Değişken_.Niteliği.Adını_Değiştir("T")] public İşyeri_Ödeme_Taksit_ Taksit;

            [Değişken_.Niteliği.Adını_Değiştir("Ö")] public List<DateOnly> ZamanıGelmedenİşlemYapılan_İlkÖdemeTarihleri;
        }
        public class Muhatap_Çalışan_
        {
            [Değişken_.Niteliği.Adını_Değiştir("Ç", 0)] public DateOnly İşeGirişTarihi = DateOnly.FromDateTime(DateTime.Now);
            [Değişken_.Niteliği.Adını_Değiştir("Ç", 1)] public double MevcutİzinGünü;
            [Değişken_.Niteliği.Adını_Değiştir("Ç", 2)] public DateOnly? İştenAyrılışTarihi;

            [Değişken_.Niteliği.Adını_Değiştir("G")] public Dictionary<DateTime, Muhatap_Çalışan_ÖzlükHakkı_> Geçmişİşlemler = new Dictionary<DateTime, Muhatap_Çalışan_ÖzlükHakkı_>(); //KayıtTarihi ve detayları

            #region İşlemler
            public void ÖzlükHakkı_Ekle(Muhatap_Çalışan_ÖzlükHakkı_ İşlem, DateTime Anahtar)
            {
                while (Geçmişİşlemler.ContainsKey(Anahtar)) Anahtar = Anahtar.AddMilliseconds(1);

                Geçmişİşlemler.Add(Anahtar, İşlem);
            }
            #endregion
        }
        public class Muhatap_Çalışan_ÖzlükHakkı_
        {
            //YeniÇalışan -> Yç
            //MaaşGüncelle -> Mg|<Mevcut>|<Güncel>
            //İzinGüncelle -> İg|<Mevcut>|<Güncel> 
            //izinKullan -> İk|<Mevcut>|<Kullanılan>|<BaşlangıçTarihi GG.AA.YYYY>
            public enum Türü_ { Boşta, YeniÇalışan, MaaşGüncelleme, İzinGüncelleme, İzinKullanımı };
            public string Yazdır_Açıklama()
            {
                switch (Türü)
                {
                    case Türü_.YeniÇalışan: return "Yeni çalışan oluşturuldu.";
                    case Türü_.MaaşGüncelleme: return "Aylık net ücret güncellendi. Mevcut:" + Banka_Ortak.Yazdır_Ücret(Mevcut, İşyeri_Ödeme_.ParaBirimi_.TürkLirası) + ", Güncel:" + Banka_Ortak.Yazdır_Ücret(GüncelVeyaKullanım, İşyeri_Ödeme_.ParaBirimi_.TürkLirası);
                    case Türü_.İzinGüncelleme: return "Mevcut izin günü güncellendi. Mevcut:" + Mevcut + ", Güncel:" + GüncelVeyaKullanım;
                    case Türü_.İzinKullanımı: return "İzin kullanıldı. Mevcut:" + Mevcut.Yazıya() + ", Kullanılan:" + GüncelVeyaKullanım + ", " + GerçekleşmeTarihi.Value.ToString(D_TarihSaat.Şablon_Tarih);

                    default:
                        throw new Exception("Türü(" + Türü + ") hatalı");
                }
            }

            [Değişken_.Niteliği.Adını_Değiştir("H", 0)] public Türü_ Türü;
            [Değişken_.Niteliği.Adını_Değiştir("H", 1)] public double Mevcut;
            [Değişken_.Niteliği.Adını_Değiştir("H", 2)] public double GüncelVeyaKullanım;
            [Değişken_.Niteliği.Adını_Değiştir("H", 3)] public string Notlar;
            [Değişken_.Niteliği.Adını_Değiştir("H", 4)] public string GerçekleştirenKullanıcıAdı;
            [Değişken_.Niteliği.Adını_Değiştir("H", 5)] public DateOnly? GerçekleşmeTarihi;
        }

        public class Ayarlar_ : Banka_Ortak.IBanka_Tanımlayıcı_
        {
            [Değişken_.Niteliği.Adını_Değiştir("S")] public DateTime SonBankaKayıt;
            [Değişken_.Niteliği.Adını_Değiştir("Ş")] public Dictionary<string, Ekranlar.Cari_Döküm_Şablon_> CariDökümŞablonlar = new Dictionary<string, Ekranlar.Cari_Döküm_Şablon_>();
            [Değişken_.Niteliği.Adını_Değiştir("Y")] public Ayarlar_Yazdırma_ Yazdırma = new Ayarlar_Yazdırma_();

            #region Kayıt
            string Banka_Ortak.IBanka_Tanımlayıcı_.SınıfAdı { get => _SınıfAdı_; set => _SınıfAdı_ = value; }
            [Değişken_.Niteliği.Adını_Değiştir("A", 0)] string _SınıfAdı_;
            [Değişken_.Niteliği.Bunu_Kesinlikle_Kullanma] public bool DeğişiklikYapıldı;
            public void Kaydet()
            {
                SonBankaKayıt = DateTime.Now;
                Banka_Ortak.Sınıf_Kaydet(this, "Ay");

                DeğişiklikYapıldı = false;
            }
            #endregion
        }
        public class Ayarlar_Yazdırma_
        {
            [Değişken_.Niteliği.Adını_Değiştir("Y", 0)] public string YazıcıAdı;
            [Değişken_.Niteliği.Adını_Değiştir("Y", 1)] public bool DosyayaYazdır = true;
            [Değişken_.Niteliği.Adını_Değiştir("Y", 2)] public string KarakterKümesi = "Calibri";
            [Değişken_.Niteliği.Adını_Değiştir("Y", 3)] public float KenarBoşluğu_mm = 15;
            [Değişken_.Niteliği.Adını_Değiştir("Y", 4)] public float KarakterBüyüklüğü = 8;
            [Değişken_.Niteliği.Adını_Değiştir("Y", 5)] public float FirmaLogo_Genişlik = 30;
            [Değişken_.Niteliği.Adını_Değiştir("Y", 6)] public float FirmaLogo_Yükseklik = 15;
            [Değişken_.Niteliği.Adını_Değiştir("Y", 7)] public bool RenkliHücreler;
            [Değişken_.Niteliği.Adını_Değiştir("Y", 8)] public bool YatayGörünüm;
        }
        #endregion
    }

    #region Ortak
    public static class Banka_Ortak
    {
        public static void Yazdır_Sıfırla()
        {
            Yazdır_Ücret_EnGenişÜcretKarakterSayısı = null;
        }
        public static string Yazdır_Tarih(string Girdi)
        {
            if (string.IsNullOrEmpty(Girdi) || Girdi.Length < 10) return Girdi;

            return Girdi.Substring(0, 10); // dd.MM.yyyy
        }
        public static string Yazdır_Tarih_Gün(TimeSpan Girdi)
        {
            double girdi = Math.Abs(Girdi.TotalDays);
            int gün_olarak = (int)girdi;
            int saat_olarak = (int)((girdi - gün_olarak) * 24.0);
            int hafta_olarak = gün_olarak / 7;
            int ay_olarak = gün_olarak / 30;

            if (gün_olarak == 0 && saat_olarak == 0) return "1 saatten az";
            else if (ay_olarak > 0) return ay_olarak + " ay";
            else if (hafta_olarak > 0) return hafta_olarak + " hafta";
            else return ((gün_olarak > 0 ? gün_olarak + " gün " : null) + (saat_olarak > 0 ? saat_olarak + " saat" : null)).TrimEnd();
        }
        static int[] Yazdır_Ücret_EnGenişÜcretKarakterSayısı = null;
        static string Yazdır_Ücret_Şablon = "{0:#,0.0;-#,0.0;0}";
        public static string Yazdır_Ücret(double Ücret, Banka1.İşyeri_Ödeme_.ParaBirimi_ ParaBirimi, bool SondakiSıfırlarıSil = true, bool EşitGenişlikte = false)
        {
            string çıktı = string.Format(Yazdır_Ücret_Şablon, Ücret);

            if (EşitGenişlikte)
            {
                if (Yazdır_Ücret_EnGenişÜcretKarakterSayısı == null)
                {
                    int[] dizi = new int[(int)Banka1.İşyeri_Ödeme_.ParaBirimi_.ElemanSayısı];

                    double ToplamGelir_ = Yazdır_Dizi_Toplam_TürkLirasıOlarak(Ortak.Banka.Seçilenİşyeri.ToplamGelir);
                    double ToplamGider_ = Yazdır_Dizi_Toplam_TürkLirasıOlarak(Ortak.Banka.Seçilenİşyeri.ToplamGider);
                    int Tge_ = string.Format(Yazdır_Ücret_Şablon, ToplamGelir_).Length;
                    int Tgi_ = string.Format(Yazdır_Ücret_Şablon, ToplamGider_).Length;
                    int Tfa_ = string.Format(Yazdır_Ücret_Şablon, ToplamGelir_ - ToplamGider_).Length;
                    dizi[(int)Banka1.İşyeri_Ödeme_.ParaBirimi_.TürkLirası] = Hesapla.EnBüyük(Tge_, Tgi_, Tfa_);

                    Tge_ = string.Format(Yazdır_Ücret_Şablon, Ortak.Banka.Seçilenİşyeri.ToplamGelir[(int)Banka1.İşyeri_Ödeme_.ParaBirimi_.Avro]).Length;
                    Tgi_ = string.Format(Yazdır_Ücret_Şablon, Ortak.Banka.Seçilenİşyeri.ToplamGider[(int)Banka1.İşyeri_Ödeme_.ParaBirimi_.Avro]).Length;
                    Tfa_ = string.Format(Yazdır_Ücret_Şablon, Ortak.Banka.Seçilenİşyeri.ToplamGelir[(int)Banka1.İşyeri_Ödeme_.ParaBirimi_.Avro] - Ortak.Banka.Seçilenİşyeri.ToplamGider[(int)Banka1.İşyeri_Ödeme_.ParaBirimi_.Avro]).Length;
                    dizi[(int)Banka1.İşyeri_Ödeme_.ParaBirimi_.Avro] = Hesapla.EnBüyük(Tge_, Tgi_, Tfa_);

                    Tge_ = string.Format(Yazdır_Ücret_Şablon, Ortak.Banka.Seçilenİşyeri.ToplamGelir[(int)Banka1.İşyeri_Ödeme_.ParaBirimi_.Dolar]).Length;
                    Tgi_ = string.Format(Yazdır_Ücret_Şablon, Ortak.Banka.Seçilenİşyeri.ToplamGider[(int)Banka1.İşyeri_Ödeme_.ParaBirimi_.Dolar]).Length;
                    Tfa_ = string.Format(Yazdır_Ücret_Şablon, Ortak.Banka.Seçilenİşyeri.ToplamGelir[(int)Banka1.İşyeri_Ödeme_.ParaBirimi_.Dolar] - Ortak.Banka.Seçilenİşyeri.ToplamGider[(int)Banka1.İşyeri_Ödeme_.ParaBirimi_.Dolar]).Length;
                    dizi[(int)Banka1.İşyeri_Ödeme_.ParaBirimi_.Dolar] = Hesapla.EnBüyük(Tge_, Tgi_, Tfa_);

                    Yazdır_Ücret_EnGenişÜcretKarakterSayısı = dizi;
                }

                int krk_sayısı = çıktı.Length;
                if (krk_sayısı < Yazdır_Ücret_EnGenişÜcretKarakterSayısı[(int)ParaBirimi]) çıktı = new string(' ', Yazdır_Ücret_EnGenişÜcretKarakterSayısı[(int)ParaBirimi] - krk_sayısı) + çıktı;
            }
            else if (SondakiSıfırlarıSil && çıktı.EndsWith("0") && çıktı.Length > 2) çıktı = çıktı.Remove(çıktı.Length - 2/*.0*/);

            return çıktı + (ParaBirimi == Banka1.İşyeri_Ödeme_.ParaBirimi_.TürkLirası ? " ₺" : ParaBirimi == Banka1.İşyeri_Ödeme_.ParaBirimi_.Avro ? " €" : ParaBirimi == Banka1.İşyeri_Ödeme_.ParaBirimi_.Dolar ? " $" : throw new Exception("ParaBirimi(" + ParaBirimi + ") uygun değil"));
        }
        public static string Yazdır_Dizi(double[] Dizi, bool EşitGenişlikte)
        {
            return
                Yazdır_Ücret(Yazdır_Dizi_Toplam_TürkLirasıOlarak(Dizi), Banka1.İşyeri_Ödeme_.ParaBirimi_.TürkLirası, true, EşitGenişlikte) +
                " ( " + Yazdır_Ücret(Dizi[(int)Banka1.İşyeri_Ödeme_.ParaBirimi_.TürkLirası], Banka1.İşyeri_Ödeme_.ParaBirimi_.TürkLirası, true, EşitGenişlikte) +
                ", " + Yazdır_Ücret(Dizi[(int)Banka1.İşyeri_Ödeme_.ParaBirimi_.Avro], Banka1.İşyeri_Ödeme_.ParaBirimi_.Avro, true, EşitGenişlikte) +
                ", " + Yazdır_Ücret(Dizi[(int)Banka1.İşyeri_Ödeme_.ParaBirimi_.Dolar], Banka1.İşyeri_Ödeme_.ParaBirimi_.Dolar, true, EşitGenişlikte) + " )";
        }
        public static double[] Yazdır_Dizi_FarkınıAl(double[] A, double[] B, bool SıfırdanKüçükİseSonuçSıfır = false)
        {
            double[] Fark = new double[A.Length];

            for (int i = 0; i < Fark.Length; i++)
            {
                Fark[i] = A[i] - B[i];

                if (Fark[i] < 0 && SıfırdanKüçükİseSonuçSıfır) Fark[i] = 0;
            }

            return Fark;
        }
        public static double Yazdır_Dizi_Toplam_TürkLirasıOlarak(double[] GelirVeyaGiderDizisi)
        {
            double Toplam_TL = 0;
            for (int i = 1; i < GelirVeyaGiderDizisi.Length; i++)
            {
                Toplam_TL += Ortak.ParaBirimi_Dönüştür(GelirVeyaGiderDizisi[i], (Banka1.İşyeri_Ödeme_.ParaBirimi_)i, Banka1.İşyeri_Ödeme_.ParaBirimi_.TürkLirası);
            }

            return Toplam_TL;
        }
        public static string Yazdır_Dizi_GelirGider(double[] Gelir, double[] Gider, bool GelirGideriYazdır, bool KalanıYazdır, bool KalanıKasaOlarakYazdır)
        {
            string çıktı = null;

            if (GelirGideriYazdır)
            {
                çıktı = 
                    "Gelir : " + Yazdır_Dizi(Gelir, true) + Environment.NewLine +
                    "Gider : " + Yazdır_Dizi(Gider, true);
            }
                 
            if (KalanıYazdır)
            {
                double[] Fark = Yazdır_Dizi_FarkınıAl(Gelir, Gider);

                çıktı += (GelirGideriYazdır ? Environment.NewLine : null) + (KalanıKasaOlarakYazdır ? "Kasa  : " : "Kalan : ") + Yazdır_Dizi(Fark, true);
            }

            return çıktı;
        }
        public static string Yazdır_Özet(double[] Gelir, double[] Gider, bool Kurlar, bool Sıkıştıtılmış)
        {
            DateTime tarih = DateTime.Now;
            double[] Gelir_Ödendi = new double[(int)Banka1.İşyeri_Ödeme_.ParaBirimi_.ElemanSayısı];
            double[] Gider_Ödendi = new double[(int)Banka1.İşyeri_Ödeme_.ParaBirimi_.ElemanSayısı];
            double[] Gider_Ödenmedi = new double[(int)Banka1.İşyeri_Ödeme_.ParaBirimi_.ElemanSayısı];
            Banka1.İşyeri_BirYıllıkDönem_ BirYıllıkDönem = Ortak.Banka.Seçilenİşyeri.Ödemeler_Listele_BirYıllıkDönem(tarih.Year.Yazıya());
            foreach (Banka1.İşyeri_Ödeme_ Ödeme in BirYıllıkDönem.Ödemeler)
            {
                if (Ödeme.ÖdemeninYapılacağıTarih.Month == tarih.Month && Ödeme.Durumu != Banka1.İşyeri_Ödeme_İşlem_.Durum_.İptalEdildi)
                {
                    bool ödendi = Ödeme.Durumu.ÖdendiMi();

                    if (Ödeme.Tipi.GelirMi())
                    {
                        if (ödendi) Gelir_Ödendi[(int)Ödeme.ParaBirimi] += Ödeme.Miktarı;
                    }
                    else
                    {
                        if (ödendi) Gider_Ödendi[(int)Ödeme.ParaBirimi] += Ödeme.Miktarı;
                        else Gider_Ödenmedi[(int)Ödeme.ParaBirimi] += Ödeme.Miktarı;
                    }
                }
            }
            foreach (Banka1.İşyeri_Ödeme_ Ödeme in Ortak.Banka.Seçilenİşyeri.Üyelik_OlacaklarıHesapla(new DateOnly(tarih.Year, tarih.Month, DateTime.DaysInMonth(tarih.Year, tarih.Month))))
            {
                if (!Ödeme.Tipi.GelirMi()) Gider_Ödenmedi[(int)Ödeme.ParaBirimi] += Ödeme.Miktarı;
            }
            double Gider_Ödendi_TL = Yazdır_Dizi_Toplam_TürkLirasıOlarak(Gider_Ödendi);
            double Gider_Ödenmedi_TL = Yazdır_Dizi_Toplam_TürkLirasıOlarak(Gider_Ödenmedi);

            double[] Ödenmedi = Yazdır_Dizi_FarkınıAl(Ortak.Banka.Seçilenİşyeri.ToplamGider, Ortak.Banka.Seçilenİşyeri.ÖdenmişToplamGider, true);
            double[] Kasa = Yazdır_Dizi_FarkınıAl(Ortak.Banka.Seçilenİşyeri.ÖdenmişToplamGelir, Ortak.Banka.Seçilenİşyeri.ÖdenmişToplamGider);

            string çıktı = null;
            if (Kurlar) çıktı = "Avro : " + Yazdır_Ücret(Ortak.ParaBirimi_Dönüştür(1, Banka1.İşyeri_Ödeme_.ParaBirimi_.Avro, Banka1.İşyeri_Ödeme_.ParaBirimi_.TürkLirası), Banka1.İşyeri_Ödeme_.ParaBirimi_.TürkLirası) + ", Dolar : " + Yazdır_Ücret(Ortak.ParaBirimi_Dönüştür(1, Banka1.İşyeri_Ödeme_.ParaBirimi_.Dolar, Banka1.İşyeri_Ödeme_.ParaBirimi_.TürkLirası), Banka1.İşyeri_Ödeme_.ParaBirimi_.TürkLirası) + Environment.NewLine + Environment.NewLine;

            çıktı +=        tarih.ToString("MMMM") + " ayının gelirleri" + Environment.NewLine +
                                Yazdır_Ücret(Yazdır_Dizi_Toplam_TürkLirasıOlarak(Gelir_Ödendi), Banka1.İşyeri_Ödeme_.ParaBirimi_.TürkLirası, EşitGenişlikte:Sıkıştıtılmış) + Environment.NewLine +
                            tarih.ToString("MMMM") + " ayının giderleri" + Environment.NewLine +
                                Yazdır_Ücret(Gider_Ödendi_TL, Banka1.İşyeri_Ödeme_.ParaBirimi_.TürkLirası, EşitGenişlikte: Sıkıştıtılmış) + Environment.NewLine +
                            tarih.ToString("MMMM") + " ayının ödenmesi gereken giderleri" + Environment.NewLine +
                                Yazdır_Ücret(Gider_Ödenmedi_TL, Banka1.İşyeri_Ödeme_.ParaBirimi_.TürkLirası, EşitGenişlikte: Sıkıştıtılmış) + Environment.NewLine +
                            tarih.ToString("MMMM") + " ayının toplam gideri" + Environment.NewLine +
                                Yazdır_Ücret(Gider_Ödendi_TL + Gider_Ödenmedi_TL, Banka1.İşyeri_Ödeme_.ParaBirimi_.TürkLirası, EşitGenişlikte: Sıkıştıtılmış) + Environment.NewLine +
                            "Tüm borçlar" + Environment.NewLine +
                                Yazdır_Dizi(Ödenmedi, true) + Environment.NewLine +
                            "Kasa" + Environment.NewLine +
                                Yazdır_Dizi(Kasa, true);
          
            if (Gelir != null && Gider != null)
            {
                çıktı +=    Environment.NewLine + Environment.NewLine + Environment.NewLine +
                            "Tablo kapsamda" + Environment.NewLine +
                                Yazdır_Dizi_GelirGider(Gelir, Gider, true, true, false) + Environment.NewLine + Environment.NewLine +
                            "Genel kapsamda ( Tümü )" + Environment.NewLine +
                                Yazdır_Dizi_GelirGider(Ortak.Banka.Seçilenİşyeri.ToplamGelir, Ortak.Banka.Seçilenİşyeri.ToplamGider, true, true, false) + Environment.NewLine + Environment.NewLine +
                            "Genel kapsamda ( Sadece ödenen )" + Environment.NewLine +
                                Yazdır_Dizi_GelirGider(Ortak.Banka.Seçilenİşyeri.ÖdenmişToplamGelir, Ortak.Banka.Seçilenİşyeri.ÖdenmişToplamGider, true, true, true);
            }

            if (Sıkıştıtılmış)
            {
                string[] özet_dizi = çıktı.Split(Environment.NewLine);
                int özet_en_uzun_yazı = 0;
                for (int i = 0; i < özet_dizi.Length; i += 2) { int uznlk = özet_dizi[i].Length; if (uznlk > özet_en_uzun_yazı) özet_en_uzun_yazı = uznlk; }
                özet_en_uzun_yazı += 1;
                for (int i = 0; i < özet_dizi.Length; i += 2) { özet_dizi[i] += new string(' ', özet_en_uzun_yazı - özet_dizi[i].Length); };
                çıktı = null;
                for (int i = 0; i < özet_dizi.Length; i += 2) çıktı += özet_dizi[i] + özet_dizi[i + 1] + Environment.NewLine;
                çıktı = çıktı.TrimEnd('\r', '\n');
            }

            return çıktı;
        }

        public static DateTime SonrakiTarihiHesapla(DateTime İlkTarih, Banka1.Muhatap_Üyelik_.Dönem_ Dönem, int Dönem_Adet)
        {
            switch (Dönem)
            {
                case Banka1.Muhatap_Üyelik_.Dönem_.Günlük: return İlkTarih.AddDays(Dönem_Adet);
                case Banka1.Muhatap_Üyelik_.Dönem_.Haftalık: return İlkTarih.AddDays(Dönem_Adet * 7);
                case Banka1.Muhatap_Üyelik_.Dönem_.Aylık: return İlkTarih.AddMonths(Dönem_Adet);
                case Banka1.Muhatap_Üyelik_.Dönem_.Yıllık: return İlkTarih.AddYears(Dönem_Adet);

                default: throw new Exception("Dönem(" + Dönem + ") uygun değil");
            }
        }
        public static DateOnly SonrakiTarihiHesapla(DateOnly İlkTarih, Banka1.Muhatap_Üyelik_.Dönem_ Dönem, int Dönem_Adet)
        {
            switch (Dönem)
            {
                case Banka1.Muhatap_Üyelik_.Dönem_.Günlük: return İlkTarih.AddDays(Dönem_Adet);
                case Banka1.Muhatap_Üyelik_.Dönem_.Haftalık: return İlkTarih.AddDays(Dönem_Adet * 7);
                case Banka1.Muhatap_Üyelik_.Dönem_.Aylık: return İlkTarih.AddMonths(Dönem_Adet);
                case Banka1.Muhatap_Üyelik_.Dönem_.Yıllık: return İlkTarih.AddYears(Dönem_Adet);

                default: throw new Exception("Dönem(" + Dönem + ") uygun değil");
            }
        }

        //epo + Sıkıştırma + Şifreleme
        public static Değişken_ Değişken = new Değişken_() { Filtre_BoşVeyaVarsayılanDeğerdeİse_HariçTut = true };
        public interface IBanka_Tanımlayıcı_ { string SınıfAdı { get; set; } }

        public static string YeniKlasörAdıOluştur(string KökKlasör)
        {
            if (!KökKlasör.EndsWith("\\")) KökKlasör += "\\";
            string kls = Path.GetRandomFileName();
            while (Directory.Exists(KökKlasör + kls)) kls = Path.GetRandomFileName();

            return kls;
        }
        public static bool Sınıf_DosyaVarMı(string DosyaYolu)
        {
            DosyaYolu = Ortak.Klasör_Banka + DosyaYolu + ".mup";
            return File.Exists(DosyaYolu);
        }
        public static object Sınıf_Oluştur(Type Tipi, Depo_ Depo)
        {
            bool BoşDepo = Depo == null;
            if (BoşDepo) Depo = new Depo_();
            object sınıf = Değişken.Üret(Tipi, Depo["ArGeMuP"]);

            IBanka_Tanımlayıcı_ tnmlyc = sınıf as IBanka_Tanımlayıcı_;
            if (tnmlyc == null) throw new Exception("tnmlyc == null " + Tipi);

            if (BoşDepo) tnmlyc.SınıfAdı = Tipi.FullName;
            else if (tnmlyc.SınıfAdı != Tipi.FullName) throw new Exception("tnmlyc.SınıfAdı(" + tnmlyc.SınıfAdı + ") != Tipi.FullName(" + Tipi.FullName + ")");

            return sınıf;
        }
        public static object Sınıf_Aç(Type Tipi, string DosyaYolu)
        {
            try
            {
                Depo_ depo = null;
                if (Sınıf_DosyaVarMı(DosyaYolu)) depo = Depo_Aç(DosyaYolu);
                
                return Sınıf_Oluştur(Tipi, depo);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + " " + DosyaYolu);
            }
        }
        public static object Sınıf_Kopyala(object Girdi)
        {
            IDepo_Eleman Depo = new Depo_()["ArGeMuP"];
            Değişken.Depola(Girdi, Depo);
            return Değişken.Üret(Girdi.GetType(), Depo);
        }
        public static void Sınıf_Kaydet(object Sınıf, string DosyaYolu)
        {
            if (Sınıf == null || DosyaYolu.BoşMu(true)) throw new Exception("Sınıf(" + (Sınıf == null) + ") == null || DosyaYolu.BoşMu(true)(" + DosyaYolu + ")");

            IBanka_Tanımlayıcı_ tnmlyc = Sınıf as IBanka_Tanımlayıcı_;
            if (tnmlyc == null) throw new Exception("tnmlyc == null " + Sınıf.GetType().FullName + " " + DosyaYolu);
            if (tnmlyc.SınıfAdı.BoşMu(true)) tnmlyc.SınıfAdı = Sınıf.GetType().FullName;

            Depo_ depo = new Depo_();
            Değişken.Depola(Sınıf, depo["ArGeMuP"]);
            Depo_Kaydet(DosyaYolu, depo);
        }
        static void Depo_Kaydet(string DosyaYolu, Depo_ Depo)
        {
            //Depo
            string içerik = Depo.YazıyaDönüştür();
            if (string.IsNullOrEmpty(içerik)) içerik = " ";

            byte[] çıktı = Dosya_SıkıştırKarıştır(içerik.BaytDizisine());
            DosyaYolu = Ortak.Klasör_Banka + DosyaYolu + ".mup";
            string yedek_dosya_yolu = DosyaYolu + ".yedek";
            Klasör.Oluştur(Dosya.Klasörü(DosyaYolu));

            if (!File.Exists(yedek_dosya_yolu) && File.Exists(DosyaYolu)) File.Move(DosyaYolu, yedek_dosya_yolu);

            File.WriteAllBytes(DosyaYolu, çıktı);

            Dosya.Sil(yedek_dosya_yolu);
        }
        static Depo_ Depo_Aç(string DosyaYolu)
        {
            DosyaYolu = Ortak.Klasör_Banka + DosyaYolu + ".mup";
            if (!File.Exists(DosyaYolu)) return new Depo_();

            Depo_ Depo = null;
            byte[] çıktı = Dosya_DüzeltAç(File.ReadAllBytes(DosyaYolu));
            string okunan = çıktı.Yazıya();
            if (!string.IsNullOrEmpty(okunan)) Depo = new Depo_(okunan);
            if (Depo == null) throw new Exception(DosyaYolu + " dosyası arızalı");

            return Depo;
        }
        public static byte[] Dosya_SıkıştırKarıştır(byte[] İçerik, byte[] Parola = null)
        {
            Parola = Parola ?? AnaKontrolcü.KökParola_Dizi;
            if (İçerik == null || İçerik.Length == 0 || Parola == null) return İçerik;

            //Ara dosya
            string Gecici_zip_dosyası = Path.GetRandomFileName();
            while (File.Exists(Ortak.Klasör_Gecici + Gecici_zip_dosyası)) Gecici_zip_dosyası = Path.GetRandomFileName();
            Gecici_zip_dosyası = Ortak.Klasör_Gecici + Gecici_zip_dosyası;

            //Sıkıştırma
            using (ZipArchive archive = ZipFile.Open(Gecici_zip_dosyası, ZipArchiveMode.Create))
            {
                ZipArchiveEntry biri = archive.CreateEntry("ArGeMuP", CompressionLevel.Optimal);
                using (Stream H = biri.Open())
                {
                    H.Write(İçerik, 0, İçerik.Length);
                }
            }

            //Şifreleme
            byte[] çıktı = DahaCokKarmasiklastirma.Karıştır(File.ReadAllBytes(Gecici_zip_dosyası), Parola);
            Dosya.Sil(Gecici_zip_dosyası);

            return çıktı;
        }
        public static byte[] Dosya_DüzeltAç(byte[] İçerik, byte[] Parola = null)
        {
            Parola = Parola ?? AnaKontrolcü.KökParola_Dizi;
            if (İçerik == null || İçerik.Length == 0 || Parola == null) return İçerik;

            //Şifre çözme
            byte[] çıktı = DahaCokKarmasiklastirma.Düzelt(İçerik, Parola);

            //Ara dosya
            string Gecici_zip_dosyası = Path.GetRandomFileName();
            while (File.Exists(Ortak.Klasör_Gecici + Gecici_zip_dosyası)) Gecici_zip_dosyası = Path.GetRandomFileName();
            Gecici_zip_dosyası = Ortak.Klasör_Gecici + Gecici_zip_dosyası;
            File.WriteAllBytes(Gecici_zip_dosyası, çıktı);

            //Açma
            byte[] dizi_içerik = null;
            using (ZipArchive Arşiv = ZipFile.OpenRead(Gecici_zip_dosyası))
            {
                ZipArchiveEntry Biri = Arşiv.GetEntry("ArGeMuP");
                if (Biri != null)
                {
                    using (Stream Akış = Biri.Open())
                    {
                        dizi_içerik = new byte[Biri.Length];
                        Akış.ReadExactly(dizi_içerik, 0, (int)Biri.Length);
                    }
                }

                if (dizi_içerik == null || dizi_içerik.Length == 0) dizi_içerik = null;      
            }

            Dosya.Sil(Gecici_zip_dosyası);

            return dizi_içerik;
        }

        public static void Başlat()
        {
            DoğrulamaKodu.KontrolEt.Durum_ snç = DoğrulamaKodu.KontrolEt.Klasör(Ortak.Klasör_Banka, SearchOption.AllDirectories, AnaKontrolcü.KökParola);
            Günlük.Ekle("Bütünlük Kontrolü " + snç.ToString());
            switch (snç)
            {
                case DoğrulamaKodu.KontrolEt.Durum_.Aynı:
                    goto Devam;

                case DoğrulamaKodu.KontrolEt.Durum_.DoğrulamaDosyasıYok:
                    Klasör_ kls = new Klasör_(Ortak.Klasör_Banka, DoğrulamaKodunuÜret:false);
                    if (kls.Dosyalar.Count > 0) throw new Exception("Büyük Hata A");
                    goto Devam;

                default:
                case DoğrulamaKodu.KontrolEt.Durum_.DoğrulamaDosyasıİçeriğiHatalı:
                case DoğrulamaKodu.KontrolEt.Durum_.Farklı:
                case DoğrulamaKodu.KontrolEt.Durum_.FazlaKlasörVeyaDosyaVar:
                    snç = DoğrulamaKodu.KontrolEt.Klasör(Ortak.Klasör_Banka2, SearchOption.AllDirectories, AnaKontrolcü.KökParola);
                    Günlük.Ekle("Bütünlük Kontrolü 2 " + snç.ToString());
                    if (snç == DoğrulamaKodu.KontrolEt.Durum_.Aynı)
                    {
                        if (Ortak.Klasör_TamKopya(Ortak.Klasör_Banka2, Ortak.Klasör_Banka))
                        {
                            goto Devam;
                        }
                    }
                    break;
            }

            throw new Exception("Banka klasörünün içeriği hatalı" + Environment.NewLine +
                        "Son yaptığınız işlem yarım kalmış olabilir" + Environment.NewLine +
                        "Yedeği kullanmak için" + Environment.NewLine +
                        "Banka klasörünün içeriğini tamamen silebilir ve" + Environment.NewLine +
                        "Yedek klasöründen en son tarihli yedeğini Banka klasörü içerisine çıkarabilirsiniz");

        Devam:
            Yedekle_Banka();

            Ortak.Banka = new Banka1();
            Ortak.Banka.Başlat();

            if (AnaKontrolcü.YanUygulamaOlarakÇalışıyor)
            {
                if (Ortak.Banka.İşyerleri.Count == 0)
                {
                    Ortak.Banka.İşyeri_Ekle(AnaKontrolcü.Şube_Talep.İşyeri_Adı);
                    Banka_Ortak.DeğişiklikleriKaydet();
                }
                else
                {
                    var içerik = Ortak.Banka.İşyerleri.First();
                    if (içerik.Key != AnaKontrolcü.Şube_Talep.İşyeri_Adı)
                    {
                        Ortak.Banka.İşyerleri.Remove(içerik.Key);
                        Ortak.Banka.İşyerleri.Add(AnaKontrolcü.Şube_Talep.İşyeri_Adı, içerik.Value);
                        içerik.Value.İşyeriAdı = AnaKontrolcü.Şube_Talep.İşyeri_Adı;
                        içerik.Value.DeğişiklikYapıldı = true;
                        Banka_Ortak.DeğişiklikleriKaydet();
                    }
                }

                Ortak.Banka.Seçilenİşyeri = Ortak.Banka.İşyerleri.First().Value;

                if (AnaKontrolcü.Şube_Talep.SabitMuhataplar != null) Ortak.Banka.Seçilenİşyeri.MuhatapGrubuVeMuhatapİsimleri_Sabit = AnaKontrolcü.Şube_Talep.SabitMuhataplar;
            }
        }
        public static void DeğişiklikleriKaydet()
        {
            if (Ortak.Banka.Ayarlar.SonBankaKayıt > DateTime.Now)
            {
                string msg = "Son kayıt saati : " + Ortak.Banka.Ayarlar.SonBankaKayıt.Yazıya() + Environment.NewLine +
                    "Bilgisayarınızın saati : " + DateTime.Now.Yazıya() + Environment.NewLine + Environment.NewLine +
                    "Muhtemelen bilgisayarınızın saati geri kaldı, lütfen düzeltip devam ediniz";

                throw new Exception(msg);
            }
            
            bool EnAz1DeğişiklikKaydedildi = false;
            foreach (Banka1.İşyeri_ işyeri in Ortak.Banka.İşyerleri.Values)
            {
                EnAz1DeğişiklikKaydedildi |= işyeri.Kaydet();
            }

            if (EnAz1DeğişiklikKaydedildi || Ortak.Banka.Ayarlar.DeğişiklikYapıldı)
            {
                Ortak.Banka.Ayarlar.Kaydet();

                DoğrulamaKodu.Üret.Klasörden(Ortak.Klasör_Banka, true, SearchOption.AllDirectories, AnaKontrolcü.KökParola);
                Yedekle_Banka();
                Yedekleme_EnAz1Kez_Değişiklikler_Kaydedildi = true;
            }
        }

        public static bool Yedekleme_Tümü_Çalışıyor = false;
        public static bool Yedekleme_EnAz1Kez_Değişiklikler_Kaydedildi = false;
        public static string Yedekleme_Hatalar = null;
        //public static string[] Kullanıcı_Klasör_Yedek = new string[0];
        public static bool Yedekle_Tümü()
        {
            Günlük.Ekle("Yedekle_Tümü " + Yedekleme_Tümü_Çalışıyor + " " + Yedekleme_EnAz1Kez_Değişiklikler_Kaydedildi);

            if (Yedekleme_Tümü_Çalışıyor || !Yedekleme_EnAz1Kez_Değişiklikler_Kaydedildi) return false;
            Yedekleme_Tümü_Çalışıyor = true;
            Yedekleme_Hatalar = null;
            bool yedekle = false;

            _Yedekle_();
            return yedekle;

            //System.Threading.Tasks.Task.Run(() =>
            //{
            //    _Yedekle_
            //});

            void _Yedekle_()
            {
                try
                {
                    Klasör_ ydk_ler = new Klasör_(Ortak.Klasör_İçYedek, Filtre_Dosya: new string[] { "*.zip" }, DoğrulamaKodunuÜret: false);
                    ydk_ler.Dosya_Sil_SayısınaGöre(15);
                    ydk_ler.Güncelle();

                    if (ydk_ler.Dosyalar.Count == 0) yedekle = true;
                    else
                    {
                        ydk_ler.Sırala_EskidenYeniye();

                        Klasör_ son_ydk = SıkıştırılmışDosya.Listele(ydk_ler.Kök + "\\" + ydk_ler.Dosyalar.Last().Yolu);
                        Klasör_ güncel = new Klasör_(Ortak.Klasör_Banka);
                        Klasör_.Farklılık_ farklar = güncel.Karşılaştır(son_ydk);
                        if (farklar.FarklılıkSayısı > 0)
                        {
                            int içeriği_farklı_dosya_Sayısı = 0;
                            foreach (Klasör_.Fark_Dosya_ a in farklar.Dosyalar)
                            {
                                if (!a.Aynı_Doğrulama_Kodu)
                                {
                                    içeriği_farklı_dosya_Sayısı++;
                                    break;
                                }
                            }
                            if (içeriği_farklı_dosya_Sayısı > 0) yedekle = true;
                        }
                    }

                    if (yedekle)
                    {
                        string k = Ortak.Klasör_Banka;
                        string h = Ortak.Klasör_İçYedek + D_TarihSaat.Yazıya(DateTime.Now, ArgeMup.HazirKod.Dönüştürme.D_TarihSaat.Şablon_DosyaAdı2) + ".zip";

                        SıkıştırılmışDosya.Klasörden(k, h);
                    }

                    //if (Kullanıcı_Klasör_Yedek.Length > 0)
                    //{
                    //    for (int i = 0; i < Kullanıcı_Klasör_Yedek.Length; i++)
                    //    {
                    //        if (string.IsNullOrEmpty(Kullanıcı_Klasör_Yedek[i])) continue;

                    //        bool sonuç = true;
                    //        sonuç &= Ortak.Klasör_TamKopya(Ortak.Klasör_Banka, Kullanıcı_Klasör_Yedek[i] + "Banka");
                    //        sonuç &= Ortak.Klasör_TamKopya(Ortak.Klasör_KullanıcıDosyaları, Kullanıcı_Klasör_Yedek[i] + "Kullanıcı Dosyaları");
                    //        sonuç &= Ortak.Klasör_TamKopya(Ortak.Klasör_İçYedek, Kullanıcı_Klasör_Yedek[i] + "Yedek", false);
                    //        sonuç &= Dosya.Kopyala(Kendi.DosyaYolu, Kullanıcı_Klasör_Yedek[i] + Kendi.DosyaAdı);

                    //        if (!sonuç) Yedekleme_Hatalar += ("Yedek no : " + (i + 1) + " yedekleme başarısız").Günlük() + Environment.NewLine;
                    //    }
                    //}

                    Yedekleme_EnAz1Kez_Değişiklikler_Kaydedildi = false;
                }
                catch (Exception ex) { Yedekleme_Hatalar += ex.Günlük().Message + Environment.NewLine; }

                if (Yedekleme_Hatalar.DoluMu()) Günlük.Ekle("Yedekle_Tümü Bitti " + Yedekleme_Hatalar);
                Yedekleme_Tümü_Çalışıyor = false;
            }
        }
        public static void Yedekle_Banka()
        {
            if (!Ortak.Klasör_TamKopya(Ortak.Klasör_Banka, Ortak.Klasör_Banka2))
            {
                throw new Exception("Yedekle_Banka>" + Ortak.Klasör_Banka + ">" + Ortak.Klasör_Banka2);
            }
        }
        public static void Yedekle_Banka_Kurtar()
        {
            DoğrulamaKodu.KontrolEt.Durum_ snç = DoğrulamaKodu.KontrolEt.Klasör(Ortak.Klasör_Banka, SearchOption.AllDirectories, AnaKontrolcü.KökParola);
            if (snç != DoğrulamaKodu.KontrolEt.Durum_.Aynı)
            {
                snç = DoğrulamaKodu.KontrolEt.Klasör(Ortak.Klasör_Banka2, SearchOption.AllDirectories, AnaKontrolcü.KökParola);
                if (snç != DoğrulamaKodu.KontrolEt.Durum_.Aynı)
                {
                    throw new Exception("Yedekle_Banka_Kurtar>Banka2>" + snç.ToString());
                }

                if (!Ortak.Klasör_TamKopya(Ortak.Klasör_Banka2, Ortak.Klasör_Banka))
                {
                    throw new Exception("Yedekle_Banka_Kurtar>Banka2>Banka");
                }

                snç = DoğrulamaKodu.KontrolEt.Klasör(Ortak.Klasör_Banka, SearchOption.AllDirectories, AnaKontrolcü.KökParola);
                if (snç != DoğrulamaKodu.KontrolEt.Durum_.Aynı)
                {
                    throw new Exception("Yedekle_Banka_Kurtar>Banka>" + snç.ToString());
                }
            }

            Günlük.Ekle("Yedekle_Banka_Kurtar>Başarılı");
        }

        public static void Yedekle_SürümYükseltmeÖncesiYedeği()
        {
            string dsy_ydk = Klasör.ÜstKlasör(Ortak.Klasör_Banka) + "\\SürümYükseltmeÖncesiYedeği.zip";
            if (File.Exists(dsy_ydk)) return;

            List<string> Filtre_Klasör = new List<string>()
            {
                Ortak.Klasör_Banka.TrimEnd('\\') + "*",
                Ortak.Klasör_KullanıcıDosyaları.TrimEnd('\\') + "*"
            };
            Klasör_ birarada = new Klasör_(Kendi.Klasörü, Filtre_Klasör, DoğrulamaKodunuÜret: false);

            string dsy_ydk_gecici = Ortak.Klasör_Gecici + Rastgele.Yazı() + "\\SürümYükseltmeÖncesiYedeği.zip";
            SıkıştırılmışDosya.Klasörden(birarada, dsy_ydk_gecici);

            Dosya.Kopyala(dsy_ydk_gecici, dsy_ydk);
            Dosya.Sil(Ortak.Klasör_Banka + DoğrulamaKodu.DoğrulamaKodu_DosyaAdı);

            Günlük.Ekle("Yedekle_SürümYükseltmeÖncesiYedeği");
        }
        public static void Yedekle_SürümYükseltmeÖncesiYedeği_Sil()
        {
            string dsy_ydk = Klasör.ÜstKlasör(Ortak.Klasör_Banka) + "\\SürümYükseltmeÖncesiYedeği.zip";
            Dosya.Sil(dsy_ydk);
            Günlük.Ekle("Yedekle_SürümYükseltmeÖncesiYedeği_Sil");
        }
        public static bool Yedekle_SürümYükseltmeÖncesiYedeği_Kurtar()
        {
            string dsy_ydk = Klasör.ÜstKlasör(Ortak.Klasör_Banka) + "\\SürümYükseltmeÖncesiYedeği.zip";
            if (!File.Exists(dsy_ydk)) return false;

            string kls_ydk_gecici = Ortak.Klasör_Gecici + Rastgele.Yazı();
            SıkıştırılmışDosya.Klasöre(dsy_ydk, kls_ydk_gecici);

            if (!Ortak.Klasör_TamKopya(kls_ydk_gecici + "\\Banka", Ortak.Klasör_Banka, AynıDoğrulamaKodunaSahipİse_DiğerFarklılıklarıGörmezdenGel: true)) throw new Exception("Yedekle_SürümYükseltmeÖncesiYedeği_Kurtar Banka");
            if (!Ortak.Klasör_TamKopya(kls_ydk_gecici + "\\Banka2", Ortak.Klasör_Banka2, true, AynıDoğrulamaKodunaSahipİse_DiğerFarklılıklarıGörmezdenGel: true)) throw new Exception("Yedekle_SürümYükseltmeÖncesiYedeği_Kurtar Banka2");
            if (!Ortak.Klasör_TamKopya(kls_ydk_gecici + "\\Kullanıcı Dosyaları", Ortak.Klasör_KullanıcıDosyaları, true, AynıDoğrulamaKodunaSahipİse_DiğerFarklılıklarıGörmezdenGel: true)) throw new Exception("Yedekle_SürümYükseltmeÖncesiYedeği_Kurtar Kullanıcı Dosyaları");
            Dosya.Sil(dsy_ydk);

            Günlük.Ekle("Yedekle_SürümYükseltmeÖncesiYedeği_Kurtar");
            return true;
        }
    }
    #endregion
}

using ArgeMup.HazirKod;
using ArgeMup.HazirKod.Ekİşlemler;
using ArgeMup.HazirKod.Ekranlar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;

namespace Gelir_Gider_Takip.Ekranlar
{
    public class Cari_Döküm_Şablon_
    {
        public Cari_Döküm_Şablon_() { }
        public Cari_Döküm_Şablon_(ListeKutusu MuhatapGrubu, ListeKutusu Muhatap)
        {
            this.MuhatapGrubu = MuhatapGrubu;
            this.Muhatap = Muhatap;

            Zamanlama_Aralık = Zamanlama_Aralık_.Bu_ay;
            Sütunlar_Sırala = Sütunlar_Sırala_.Büyükten_küçüğe;
            Sütunlar_Sırala_Sütun = Sütunlar_Sırala_Sütun_.Ödeme_Günü;
        }

        #region ListeKutusu
        [Değişken_.Niteliği.Adını_Değiştir("G")] public List<string> Kapsam_Grup;
        [Değişken_.Niteliği.Adını_Değiştir("M")] public List<string> Kapsam_Muhatap;

        [Değişken_.Niteliği.Bunu_Kesinlikle_Kullanma]
        ListeKutusu MuhatapGrubu, Muhatap;
        #endregion

        #region Zamanlama
        public enum Zamanlama_Aralık_ { Bu_ay, Son_15_gün, Bu_hafta, Bugün, Son_1_gün, Tüm_ödemeler, Sabit_aralık };
        [Değişken_.Niteliği.Adını_Değiştir("Z", 0)]
        Zamanlama_Aralık_ _Zamanlama_Aralık_;
        [Category("1 Zamanlama"), DisplayName("Aralık"), TypeConverter(typeof(TipDönüştürücü_Sıralama))]
        public Zamanlama_Aralık_ Zamanlama_Aralık
        {
            get => _Zamanlama_Aralık_;
            set
            {
                _Zamanlama_Aralık_ = value;

                DateTime t = DateTime.Now;
                switch (_Zamanlama_Aralık_)
                {
                    case Zamanlama_Aralık_.Bu_ay:
                        Zamanlama_Başlangıç = new DateTime(t.Year, t.Month, 1);
                        Zamanlama_Bitiş = new DateTime(t.Year, t.Month, DateTime.DaysInMonth(t.Year, t.Month), 23, 59, 59).AddMilliseconds(999);
                        break;

                    case Zamanlama_Aralık_.Son_15_gün:
                        Zamanlama_Başlangıç = t.AddDays(-15);
                        Zamanlama_Bitiş = t;
                        break;

                    case Zamanlama_Aralık_.Bu_hafta:
                        t = t.AddDays((int)DayOfWeek.Monday - (int)t.DayOfWeek);
                        Zamanlama_Başlangıç = new DateTime(t.Year, t.Month, t.Day);
                        t = t.AddDays(6);
                        Zamanlama_Bitiş = new DateTime(t.Year, t.Month, t.Day, 23, 59, 59).AddMilliseconds(999);
                        break;

                    case Zamanlama_Aralık_.Bugün:
                        Zamanlama_Başlangıç = new DateTime(t.Year, t.Month, t.Day, 0, 0, 0);
                        Zamanlama_Bitiş = new DateTime(t.Year, t.Month, t.Day, 23, 59, 59).AddMilliseconds(999);
                        break;

                    case Zamanlama_Aralık_.Son_1_gün:
                        Zamanlama_Başlangıç = t.AddDays(-1);
                        Zamanlama_Bitiş = t;
                        break;

                    case Zamanlama_Aralık_.Tüm_ödemeler:
                        Zamanlama_Başlangıç = DateTime.MinValue;
                        Zamanlama_Bitiş = DateTime.Now;
                        break;
                }
            }
        }

        public enum Zamanlama_GecikenleriKesinlikleGöster_ { Evet, Hayır };
        [Değişken_.Niteliği.Adını_Değiştir("Z", 1)]
        Zamanlama_GecikenleriKesinlikleGöster_ _Zamanlama_GecikenleriKesinlikleGöster_;
        [Category("1 Zamanlama"), DisplayName("Gecikenleri kesinlikle göster"), TypeConverter(typeof(TipDönüştürücü_Sıralama))]
        public Zamanlama_GecikenleriKesinlikleGöster_ Zamanlama_GecikenleriKesinlikleGöster
        {
            get => _Zamanlama_GecikenleriKesinlikleGöster_;
            set => _Zamanlama_GecikenleriKesinlikleGöster_ = value;
        }

        public enum Zamanlama_Türü_ { Ödeme_tarihi, İlk_işlem_tarihi, Son_işlem_tarihi, İşlem_tarihi };
        [Değişken_.Niteliği.Adını_Değiştir("Z", 2)]
        Zamanlama_Türü_ _Zamanlama_Türü_;
        [Category("1 Zamanlama"), DisplayName("Türü"), TypeConverter(typeof(TipDönüştürücü_Sıralama))]
        public Zamanlama_Türü_ Zamanlama_Türü
        {
            get => _Zamanlama_Türü_;
            set => _Zamanlama_Türü_ = value;
        }

        [Değişken_.Niteliği.Adını_Değiştir("Z", 3)]
        DateTime _Zamanlama_Başlangıç_;
        [Category("1 Zamanlama"), DisplayName("Başlangıç"), TypeConverter(typeof(TipDönüştürücü_TarihSaat))]
        public DateTime Zamanlama_Başlangıç
        {
            get => _Zamanlama_Başlangıç_;
            set => _Zamanlama_Başlangıç_ = value;
        }

        [Değişken_.Niteliği.Adını_Değiştir("Z", 4)]
        DateTime _Zamanlama_Bitiş_;
        [Category("1 Zamanlama"), DisplayName("Bitiş"), TypeConverter(typeof(TipDönüştürücü_TarihSaat))]
        public DateTime Zamanlama_Bitiş
        {
            get => _Zamanlama_Bitiş_;
            set => _Zamanlama_Bitiş_ = value;
        }
        #endregion

        #region Durumu
        public enum Sıralama_ÜçSeçenek_ { Farketmez, Dahil_et, Hariç_tut };

        [Değişken_.Niteliği.Adını_Değiştir("D", 0)]
        Sıralama_ÜçSeçenek_ _Durumu_Ödenmedi_;
        [Category("2 Durumu"), DisplayName("Ödenmedi"), TypeConverter(typeof(TipDönüştürücü_Sıralama))]
        public Sıralama_ÜçSeçenek_ Durumu_Ödenmedi
        {
            get => _Durumu_Ödenmedi_;
            set => _Durumu_Ödenmedi_ = value;
        }

        [Değişken_.Niteliği.Adını_Değiştir("D", 1)]
        Sıralama_ÜçSeçenek_ _Durumu_KısmenÖdendi_;
        [Category("2 Durumu"), DisplayName("Kısmen ödendi"), TypeConverter(typeof(TipDönüştürücü_Sıralama))]
        public Sıralama_ÜçSeçenek_ Durumu_KısmenÖdendi
        {
            get => _Durumu_KısmenÖdendi_;
            set => _Durumu_KısmenÖdendi_ = value;
        }

        [Değişken_.Niteliği.Adını_Değiştir("D", 2)]
        Sıralama_ÜçSeçenek_ _Durumu_TamÖdendi_;
        [Category("2 Durumu"), DisplayName("Tam ödendi"), TypeConverter(typeof(TipDönüştürücü_Sıralama))]
        public Sıralama_ÜçSeçenek_ Durumu_TamÖdendi
        {
            get => _Durumu_TamÖdendi_;
            set => _Durumu_TamÖdendi_ = value;
        }

        [Değişken_.Niteliği.Adını_Değiştir("D", 3)]
        Sıralama_ÜçSeçenek_ _Durumu_KısmiÖdemeYapıldı_;
        [Category("2 Durumu"), DisplayName("Kısmi ödeme yapıldı"), TypeConverter(typeof(TipDönüştürücü_Sıralama))]
        public Sıralama_ÜçSeçenek_ Durumu_KısmiÖdemeYapıldı
        {
            get => _Durumu_KısmiÖdemeYapıldı_;
            set => _Durumu_KısmiÖdemeYapıldı_ = value;
        }

        [Değişken_.Niteliği.Adını_Değiştir("D", 4)]
        Sıralama_ÜçSeçenek_ _Durumu_PeşinatÖdendi_;
        [Category("2 Durumu"), DisplayName("Peşinat ödendi"), TypeConverter(typeof(TipDönüştürücü_Sıralama))]
        public Sıralama_ÜçSeçenek_ Durumu_PeşinatÖdendi
        {
            get => _Durumu_PeşinatÖdendi_;
            set => _Durumu_PeşinatÖdendi_ = value;
        }

        [Değişken_.Niteliği.Adını_Değiştir("D", 5)]
        Sıralama_ÜçSeçenek_ _Durumu_İptalEdildi_;
        [Category("2 Durumu"), DisplayName("İptal edildi"), TypeConverter(typeof(TipDönüştürücü_Sıralama))]
        public Sıralama_ÜçSeçenek_ Durumu_İptalEdildi
        {
            get => _Durumu_İptalEdildi_;
            set => _Durumu_İptalEdildi_ = value;
        }
        #endregion

        #region Tipi
        [Değişken_.Niteliği.Adını_Değiştir("T", 0)]
        Sıralama_ÜçSeçenek_ _Tipi_Gelir_;
        [Category("3 Tipi"), DisplayName("Gelir"), TypeConverter(typeof(TipDönüştürücü_Sıralama))]
        public Sıralama_ÜçSeçenek_ Tipi_Gelir
        {
            get => _Tipi_Gelir_;
            set => _Tipi_Gelir_ = value;
        }

        [Değişken_.Niteliği.Adını_Değiştir("T", 1)]
        Sıralama_ÜçSeçenek_ _Tipi_Gider_;
        [Category("3 Tipi"), DisplayName("Gider"), TypeConverter(typeof(TipDönüştürücü_Sıralama))]
        public Sıralama_ÜçSeçenek_ Tipi_Gider
        {
            get => _Tipi_Gider_;
            set => _Tipi_Gider_ = value;
        }

        [Değişken_.Niteliği.Adını_Değiştir("T", 2)]
        Sıralama_ÜçSeçenek_ _Tipi_MaaşÖdemesi_;
        [Category("3 Tipi"), DisplayName("Maaş ödemesi"), TypeConverter(typeof(TipDönüştürücü_Sıralama))]
        public Sıralama_ÜçSeçenek_ Tipi_MaaşÖdemesi
        {
            get => _Tipi_MaaşÖdemesi_;
            set => _Tipi_MaaşÖdemesi_ = value;
        }

        [Değişken_.Niteliği.Adını_Değiştir("T", 3)]
        Sıralama_ÜçSeçenek_ _Tipi_AvansVerilmesi_;
        [Category("3 Tipi"), DisplayName("Avans verilmesi"), TypeConverter(typeof(TipDönüştürücü_Sıralama))]
        public Sıralama_ÜçSeçenek_ Tipi_AvansVerilmesi
        {
            get => _Tipi_AvansVerilmesi_;
            set => _Tipi_AvansVerilmesi_ = value;
        }

        [Değişken_.Niteliği.Adını_Değiştir("T", 4)]
        Sıralama_ÜçSeçenek_ _Tipi_AvansÖdemesi_;
        [Category("3 Tipi"), DisplayName("Avans ödemesi"), TypeConverter(typeof(TipDönüştürücü_Sıralama))]
        public Sıralama_ÜçSeçenek_ Tipi_AvansÖdemesi
        {
            get => _Tipi_AvansÖdemesi_;
            set => _Tipi_AvansÖdemesi_ = value;
        }

        [Değişken_.Niteliği.Adını_Değiştir("T", 5)]
        Sıralama_ÜçSeçenek_ _Tipi_KontrolNoktası_;
        [Category("3 Tipi"), DisplayName("Kontrol noktası"), TypeConverter(typeof(TipDönüştürücü_Sıralama))]
        public Sıralama_ÜçSeçenek_ Tipi_KontrolNoktası
        {
            get => _Tipi_KontrolNoktası_;
            set => _Tipi_KontrolNoktası_ = value;
        }
        #endregion

        #region Miktar
        public enum Miktar_ParaBirimi_ { Farketmez, Türk_lirası, Avro, Dolar };
        [Değişken_.Niteliği.Adını_Değiştir("Ü", 0)]
        Miktar_ParaBirimi_ _Miktar_ParaBirimi_;
        [Category("4 Miktar"), DisplayName("Para birimi"), TypeConverter(typeof(TipDönüştürücü_Sıralama))]
        public Miktar_ParaBirimi_ Miktar_ParaBirimi
        {
            get => _Miktar_ParaBirimi_;
            set => _Miktar_ParaBirimi_ = value;
        }

        [Değişken_.Niteliği.Adını_Değiştir("Ü", 1)]
        double _Miktar_EnAz_ = -1;
        [Category("4 Miktar"), DisplayName("En az"), TypeConverter(typeof(TipDönüştürücü_NoktalıSayı))]
        public double Miktar_EnAz
        {
            get => _Miktar_EnAz_;
            set => _Miktar_EnAz_ = value;
        }

        [Değişken_.Niteliği.Adını_Değiştir("Ü", 2)]
        double _Miktar_EnÇok_ = -1;
        [Category("4 Miktar"), DisplayName("En çok"), TypeConverter(typeof(TipDönüştürücü_NoktalıSayı))]
        public double Miktar_EnÇok
        {
            get => _Miktar_EnÇok_;
            set => _Miktar_EnÇok_ = value;
        }
        #endregion

        #region Diğer
        public enum Diğer_AltToplam_ { Gerekli_değil, Sadece_grup_için_hesapla, Sadece_muhatap_için_hesapla, Tümü_için_hesapla };
        [Değişken_.Niteliği.Adını_Değiştir("F", 0)]
        Diğer_AltToplam_ _Diğer_AltToplam_;
        [Category("5 Diğer"), DisplayName("Alt toplam"), TypeConverter(typeof(TipDönüştürücü_Sıralama))]
        public Diğer_AltToplam_ Diğer_AltToplam
        {
            get => _Diğer_AltToplam_;
            set => _Diğer_AltToplam_ = value;
        }

        public enum Diğer_Şablon_ { Kullanma, Bu_ayın_gelirleri, Bu_ayın_giderleri, Bu_ayın_ödenmesi_gereken_giderleri, Bu_ayın_toplam_gideri, Tüm_borçlar, Kasa, Her_şey };
        [Değişken_.Niteliği.Bunu_Kesinlikle_Kullanma()]
        Diğer_Şablon_ _Diğer_Şablon_;
        [Category("5 Diğer"), DisplayName("Şablon"), TypeConverter(typeof(TipDönüştürücü_Sıralama))]
        public Diğer_Şablon_ Diğer_Şablon
        {
            get => _Diğer_Şablon_;
            set
            {
                if (value == Diğer_Şablon_.Kullanma) return;
                else if (value == Diğer_Şablon_.Her_şey)
                {
                    Zamanlama_Aralık = Zamanlama_Aralık_.Tüm_ödemeler;
                    Zamanlama_GecikenleriKesinlikleGöster = Zamanlama_GecikenleriKesinlikleGöster_.Evet;
                    Zamanlama_Türü = Zamanlama_Türü_.Son_işlem_tarihi;

                    Durumu_KısmenÖdendi = Sıralama_ÜçSeçenek_.Farketmez;
                    Durumu_KısmiÖdemeYapıldı = Sıralama_ÜçSeçenek_.Farketmez;
                    Durumu_PeşinatÖdendi = Sıralama_ÜçSeçenek_.Farketmez;
                    Durumu_TamÖdendi = Sıralama_ÜçSeçenek_.Farketmez;
                    Durumu_Ödenmedi = Sıralama_ÜçSeçenek_.Farketmez;
                    Durumu_İptalEdildi = Sıralama_ÜçSeçenek_.Farketmez;

                    Tipi_Gelir = Sıralama_ÜçSeçenek_.Farketmez;
                    Tipi_Gider = Sıralama_ÜçSeçenek_.Farketmez;
                    Tipi_AvansVerilmesi = Sıralama_ÜçSeçenek_.Farketmez;
                    Tipi_AvansÖdemesi = Sıralama_ÜçSeçenek_.Farketmez;
                    Tipi_KontrolNoktası = Sıralama_ÜçSeçenek_.Farketmez;
                    Tipi_MaaşÖdemesi = Sıralama_ÜçSeçenek_.Farketmez;

                    Miktar_ParaBirimi = Miktar_ParaBirimi_.Farketmez;
                    Miktar_EnAz = -1;
                    Miktar_EnÇok = -1;

                    Diğer_AltToplam = Diğer_AltToplam_.Gerekli_değil;
                    Sütunlar_Sırala_Sütun = Sütunlar_Sırala_Sütun_.Ödeme_Günü;
                    Sütunlar_Sırala = Sütunlar_Sırala_.Büyükten_küçüğe;

                    Sütunlar_Grup = Sütunlar_Durum_.Göster;
                    Sütunlar_Muhatap = Sütunlar_Durum_.Göster;
                    Sütunlar_Durum = Sütunlar_Durum_.Göster;
                    Sütunlar_İlkİşlemTarihi = Sütunlar_Durum_.Göster;
                    Sütunlar_Kullanıcı = Sütunlar_Durum_.Göster;
                    Sütunlar_Miktar = Sütunlar_Durum_.Göster;
                    Sütunlar_Notlar = Sütunlar_Durum_.Göster;
                    Sütunlar_SonİşlemTarihi = Sütunlar_Durum_.Göster;
                    Sütunlar_Taksit = Sütunlar_Durum_.Göster;
                    Sütunlar_Tip = Sütunlar_Durum_.Göster;
                    Sütunlar_ÖdemeGünü = Sütunlar_Durum_.Göster;
                    Sütunlar_Üyelik = Sütunlar_Durum_.Göster;

                    if (MuhatapGrubu != null)
                    {
                        MuhatapGrubu.SeçilenEleman_Adı = null;
                        Muhatap.SeçilenEleman_Adı = null;
                    }
                }
                else
                {
                    Diğer_Şablon = Diğer_Şablon_.Her_şey;
                    Zamanlama_Türü = Zamanlama_Türü_.Ödeme_tarihi;
                    Zamanlama_GecikenleriKesinlikleGöster = Zamanlama_GecikenleriKesinlikleGöster_.Hayır;
                    Durumu_İptalEdildi = Sıralama_ÜçSeçenek_.Hariç_tut;
                    Tipi_KontrolNoktası = Sıralama_ÜçSeçenek_.Hariç_tut;

                    if (value == Diğer_Şablon_.Bu_ayın_gelirleri)
                    {
                        Zamanlama_Aralık = Zamanlama_Aralık_.Bu_ay;

                        Durumu_KısmenÖdendi = Sıralama_ÜçSeçenek_.Hariç_tut;
                        Durumu_Ödenmedi = Sıralama_ÜçSeçenek_.Hariç_tut;

                        Tipi_Gider = Sıralama_ÜçSeçenek_.Hariç_tut;
                        Tipi_AvansVerilmesi = Sıralama_ÜçSeçenek_.Hariç_tut;
                        Tipi_MaaşÖdemesi = Sıralama_ÜçSeçenek_.Hariç_tut;
                    }
                    else if (value == Diğer_Şablon_.Bu_ayın_giderleri)
                    {
                        Zamanlama_Aralık = Zamanlama_Aralık_.Bu_ay;

                        Durumu_KısmenÖdendi = Sıralama_ÜçSeçenek_.Hariç_tut;
                        Durumu_Ödenmedi = Sıralama_ÜçSeçenek_.Hariç_tut;

                        Tipi_Gelir = Sıralama_ÜçSeçenek_.Hariç_tut;
                        Tipi_AvansÖdemesi = Sıralama_ÜçSeçenek_.Hariç_tut;
                    }
                    else if (value == Diğer_Şablon_.Bu_ayın_ödenmesi_gereken_giderleri)
                    {
                        Zamanlama_Aralık = Zamanlama_Aralık_.Bu_ay;

                        Durumu_KısmiÖdemeYapıldı = Sıralama_ÜçSeçenek_.Hariç_tut;
                        Durumu_PeşinatÖdendi = Sıralama_ÜçSeçenek_.Hariç_tut;
                        Durumu_TamÖdendi = Sıralama_ÜçSeçenek_.Hariç_tut;

                        Tipi_Gelir = Sıralama_ÜçSeçenek_.Hariç_tut;
                        Tipi_AvansÖdemesi = Sıralama_ÜçSeçenek_.Hariç_tut;
                    }
                    else if (value == Diğer_Şablon_.Bu_ayın_toplam_gideri)
                    {
                        Zamanlama_Aralık = Zamanlama_Aralık_.Bu_ay;

                        Tipi_Gelir = Sıralama_ÜçSeçenek_.Hariç_tut;
                        Tipi_AvansÖdemesi = Sıralama_ÜçSeçenek_.Hariç_tut;
                    }
                    else if (value == Diğer_Şablon_.Tüm_borçlar)
                    {
                        Zamanlama_Aralık = Zamanlama_Aralık_.Tüm_ödemeler;
                        Zamanlama_Türü = Zamanlama_Türü_.Son_işlem_tarihi;

                        Durumu_KısmiÖdemeYapıldı = Sıralama_ÜçSeçenek_.Hariç_tut;
                        Durumu_PeşinatÖdendi = Sıralama_ÜçSeçenek_.Hariç_tut;
                        Durumu_TamÖdendi = Sıralama_ÜçSeçenek_.Hariç_tut;

                        Tipi_Gelir = Sıralama_ÜçSeçenek_.Hariç_tut;
                        Tipi_AvansÖdemesi = Sıralama_ÜçSeçenek_.Hariç_tut;
                    }
                    else if (value == Diğer_Şablon_.Kasa)
                    {
                        Zamanlama_Aralık = Zamanlama_Aralık_.Tüm_ödemeler;
                        Zamanlama_Türü = Zamanlama_Türü_.Son_işlem_tarihi;

                        Durumu_KısmenÖdendi = Sıralama_ÜçSeçenek_.Hariç_tut;
                        Durumu_Ödenmedi = Sıralama_ÜçSeçenek_.Hariç_tut;
                    }
                }

                _Diğer_Şablon_ = Diğer_Şablon_.Kullanma;
            }
        }
        #endregion

        #region Sütunlar
        public enum Sütunlar_Sırala_ { Küçükten_büyüğe, Büyükten_küçüğe };
        [Değişken_.Niteliği.Adını_Değiştir("Ş", 0)]
        Sütunlar_Sırala_ _Sütunlar_Sırala_;
        [Category("6 Sütunlar"), DisplayName("Sırala"), TypeConverter(typeof(TipDönüştürücü_Sıralama))]
        public Sütunlar_Sırala_ Sütunlar_Sırala
        {
            get => _Sütunlar_Sırala_;
            set => _Sütunlar_Sırala_ = value;
        }

        public enum Sütunlar_Sırala_Sütun_ { Grup, Muhatap, Ödeme_Günü, Tip, Durum, Notlar, Taksit, Son_İşlem_Tarihi, İlk_İşlem_Tarihi, Kullanıcı };
        [Değişken_.Niteliği.Adını_Değiştir("Ş", 1)]
        Sütunlar_Sırala_Sütun_ _Sütunlar_Sırala_Sütun_;
        [Category("6 Sütunlar"), DisplayName("Sıralanacak Sütun"), TypeConverter(typeof(TipDönüştürücü_Sıralama))]
        public Sütunlar_Sırala_Sütun_ Sütunlar_Sırala_Sütun
        {
            get => _Sütunlar_Sırala_Sütun_;
            set => _Sütunlar_Sırala_Sütun_ = value;
        }

        public enum Sütunlar_Durum_ { Göster, Gizle };

        [Değişken_.Niteliği.Adını_Değiştir("S", 0)]
        Sütunlar_Durum_ _Sütunlar_Grup_;
        [Category("6 Sütunlar"), DisplayName("Grup"), TypeConverter(typeof(TipDönüştürücü_Sıralama))]
        public Sütunlar_Durum_ Sütunlar_Grup
        {
            get => _Sütunlar_Grup_;
            set => _Sütunlar_Grup_ = value;
        }

        [Değişken_.Niteliği.Adını_Değiştir("S", 1)]
        Sütunlar_Durum_ _Sütunlar_Muhatap_;
        [Category("6 Sütunlar"), DisplayName("Muhatap"), TypeConverter(typeof(TipDönüştürücü_Sıralama))]
        public Sütunlar_Durum_ Sütunlar_Muhatap
        {
            get => _Sütunlar_Muhatap_;
            set => _Sütunlar_Muhatap_ = value;
        }

        [Değişken_.Niteliği.Adını_Değiştir("S", 2)]
        Sütunlar_Durum_ _Sütunlar_ÖdemeGünü_;
        [Category("6 Sütunlar"), DisplayName("Ödeme günü"), TypeConverter(typeof(TipDönüştürücü_Sıralama))]
        public Sütunlar_Durum_ Sütunlar_ÖdemeGünü
        {
            get => _Sütunlar_ÖdemeGünü_;
            set => _Sütunlar_ÖdemeGünü_ = value;
        }

        [Değişken_.Niteliği.Adını_Değiştir("S", 3)]
        Sütunlar_Durum_ _Sütunlar_Tip_;
        [Category("6 Sütunlar"), DisplayName("Tip"), TypeConverter(typeof(TipDönüştürücü_Sıralama))]
        public Sütunlar_Durum_ Sütunlar_Tip
        {
            get => _Sütunlar_Tip_;
            set => _Sütunlar_Tip_ = value;
        }

        [Değişken_.Niteliği.Adını_Değiştir("S", 4)]
        Sütunlar_Durum_ _Sütunlar_Durum_;
        [Category("6 Sütunlar"), DisplayName("Durum"), TypeConverter(typeof(TipDönüştürücü_Sıralama))]
        public Sütunlar_Durum_ Sütunlar_Durum
        {
            get => _Sütunlar_Durum_;
            set => _Sütunlar_Durum_ = value;
        }

        [Değişken_.Niteliği.Adını_Değiştir("S", 5)]
        Sütunlar_Durum_ _Sütunlar_Miktar_;
        [Category("6 Sütunlar"), DisplayName("Miktar"), TypeConverter(typeof(TipDönüştürücü_Sıralama))]
        public Sütunlar_Durum_ Sütunlar_Miktar
        {
            get => _Sütunlar_Miktar_;
            set => _Sütunlar_Miktar_ = value;
        }

        [Değişken_.Niteliği.Adını_Değiştir("S", 6)]
        Sütunlar_Durum_ _Sütunlar_Notlar_;
        [Category("6 Sütunlar"), DisplayName("Notlar"), TypeConverter(typeof(TipDönüştürücü_Sıralama))]
        public Sütunlar_Durum_ Sütunlar_Notlar
        {
            get => _Sütunlar_Notlar_;
            set => _Sütunlar_Notlar_ = value;
        }

        [Değişken_.Niteliği.Adını_Değiştir("S", 7)]
        Sütunlar_Durum_ _Sütunlar_Taksit_;
        [Category("6 Sütunlar"), DisplayName("Taksit"), TypeConverter(typeof(TipDönüştürücü_Sıralama))]
        public Sütunlar_Durum_ Sütunlar_Taksit
        {
            get => _Sütunlar_Taksit_;
            set => _Sütunlar_Taksit_ = value;
        }

        [Değişken_.Niteliği.Adını_Değiştir("S", 8)]
        Sütunlar_Durum_ _Sütunlar_Üyelik_;
        [Category("6 Sütunlar"), DisplayName("Üyelik"), TypeConverter(typeof(TipDönüştürücü_Sıralama))]
        public Sütunlar_Durum_ Sütunlar_Üyelik
        {
            get => _Sütunlar_Üyelik_;
            set => _Sütunlar_Üyelik_ = value;
        }

        [Değişken_.Niteliği.Adını_Değiştir("S", 9)]
        Sütunlar_Durum_ _Sütunlar_SonİşlemTarihi_;
        [Category("6 Sütunlar"), DisplayName("Son işlem tarihi"), TypeConverter(typeof(TipDönüştürücü_Sıralama))]
        public Sütunlar_Durum_ Sütunlar_SonİşlemTarihi
        {
            get => _Sütunlar_SonİşlemTarihi_;
            set => _Sütunlar_SonİşlemTarihi_ = value;
        }

        [Değişken_.Niteliği.Adını_Değiştir("S", 10)]
        Sütunlar_Durum_ _Sütunlar_İlkİşlemTarihi_;
        [Category("6 Sütunlar"), DisplayName("İlk işlem tarihi"), TypeConverter(typeof(TipDönüştürücü_Sıralama))]
        public Sütunlar_Durum_ Sütunlar_İlkİşlemTarihi
        {
            get => _Sütunlar_İlkİşlemTarihi_;
            set => _Sütunlar_İlkİşlemTarihi_ = value;
        }

        [Değişken_.Niteliği.Adını_Değiştir("S", 11)]
        Sütunlar_Durum_ _Sütunlar_Kullanıcı_;
        [Category("6 Sütunlar"), DisplayName("Kullanıcı"), TypeConverter(typeof(TipDönüştürücü_Sıralama))]
        public Sütunlar_Durum_ Sütunlar_Kullanıcı
        {
            get => _Sütunlar_Kullanıcı_;
            set => _Sütunlar_Kullanıcı_ = value;
        }
        #endregion

        #region İşlemler
        public Cari_Döküm_Şablon_ Kopyala(ListeKutusu MuhatapGrubu, ListeKutusu Muhatap)
        {
            Cari_Döküm_Şablon_ kopya = (Cari_Döküm_Şablon_)Banka_Ortak.Sınıf_Kopyala(this);
            kopya.MuhatapGrubu = MuhatapGrubu;
            kopya.Muhatap = Muhatap;
            return kopya;
        }
        #endregion

        #region Yardımcı Sınıflar
        class TipDönüştürücü_Sıralama : EnumConverter
        {
            private Type _enumType;
            public TipDönüştürücü_Sıralama(Type type) : base(type)
            {
                _enumType = type;
            }

            public override bool CanConvertTo(ITypeDescriptorContext context, Type destType)
            {
                return destType == typeof(string);
            }
            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destType)
            {
                return value.ToString().Replace('_', ' ');
            }

            public override bool CanConvertFrom(ITypeDescriptorContext context, Type srcType)
            {
                return srcType == typeof(string);
            }
            public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
            {
                return Enum.Parse(_enumType, ((string)value).Replace(' ', '_'));
            }
        }
        class TipDönüştürücü_TarihSaat : TypeConverter
        {
            public override bool CanConvertTo(ITypeDescriptorContext context, Type destType)
            {
                return destType == typeof(string);
            }
            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destType)
            {
                return ((DateTime)value).Yazıya();
            }

            public override bool CanConvertFrom(ITypeDescriptorContext context, Type srcType)
            {
                return srcType == typeof(string);
            }
            public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
            {
                try
                {
                    string girdi = ((string)value).ToLower();
                    if (girdi.BoşMu(true)) return DateTime.Now;

                    if (girdi.StartsWith("gün") || girdi.StartsWith("ay") || girdi.StartsWith("yıl"))
                    {
                        return _ekle_çıkar_();

                        DateTime _ekle_çıkar_()
                        {
                            var gridItem = context as System.Windows.Forms.GridItem;
                            DateTime ilk_tarih = (DateTime)gridItem.Value;

                            if (girdi.StartsWith("ay"))
                            {
                                girdi = girdi.Substring(2);
                                return ilk_tarih.AddMonths(girdi.TamSayıya());
                            }
                            else if (girdi.StartsWith("gün") || girdi.StartsWith("yıl"))
                            {
                                bool gün = girdi.StartsWith("gün");
                                girdi = girdi.Substring(3);

                                return gün ? ilk_tarih.AddDays(girdi.TamSayıya()) : ilk_tarih.AddYears(girdi.TamSayıya());
                            }

                            throw new Exception();
                        }
                    }
                    else if (DateTime.TryParseExact(girdi, ArgeMup.HazirKod.Dönüştürme.D_TarihSaat.Şablon_Tarih_Saat_MiliSaniye, null, DateTimeStyles.None, out DateTime tt)) return tt;
                    else
                    {
                        //1 31 - 1 12 - 0 9999 - 0 23 - 0 59 - 0 - 59 - 0 999
                        string[] dizi = girdi.Split('-');

                        for (int i = 0; i < dizi.Length; i++)
                        {
                            if (i == 2)
                            {
                                int okunan = dizi[i].TamSayıya();
                                if (okunan < 100) dizi[i] = (okunan + 2000).Yazıya();
                            }
                            else if (i == 6)
                            {
                                int uzunluk = dizi[i].Length;
                                if (uzunluk == 1) dizi[i] = "00" + dizi[i];
                                else if (uzunluk == 2) dizi[i] = "0" + dizi[i];
                            }
                            else dizi[i] = dizi[i].TamSayıya().ToString("00");
                        }

                        string şablon = null;
                        switch (dizi.Length)
                        {
                            case 7: şablon = "dd-MM-yyyy-HH-mm-ss-fff"; break;
                            case 6: şablon = "dd-MM-yyyy-HH-mm-ss"; break;
                            case 5: şablon = "dd-MM-yyyy-HH-mm"; break;
                            case 4: şablon = "dd-MM-yyyy-HH"; break;
                            case 3: şablon = "dd-MM-yyyy"; break;
                            case 2: şablon = "dd-MM"; break;
                            case 1: şablon = "dd"; break;
                        }

                        girdi = null;
                        foreach (string aaa in dizi)
                        {
                            girdi += aaa + "-";
                        }
                        return girdi.TrimEnd('-').TarihSaate(şablon);
                    }
                }
                catch (Exception)
                {
                    throw new Exception("Alttaki şablona uygun olacak şekilde gerekli yazıyı oluşturunuz" + Environment.NewLine +
                        "<gün>-<ay>-<yıl>-<saat>-<dakika>-<saniye>-<milisaniye>" + Environment.NewLine +
                        "gün+-<adet> veya ay+-<adet> veya yıl+-<adet> gibi");
                }
            }
        }
        class TipDönüştürücü_NoktalıSayı : TypeConverter
        {
            public override bool CanConvertTo(ITypeDescriptorContext context, Type destType)
            {
                return destType == typeof(string);
            }
            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destType)
            {
                return ((double)value).Yazıya();
            }

            public override bool CanConvertFrom(ITypeDescriptorContext context, Type srcType)
            {
                return srcType == typeof(string);
            }
            public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
            {
                return ((string)value).NoktalıSayıya();
            }
        }
        #endregion
    }
}
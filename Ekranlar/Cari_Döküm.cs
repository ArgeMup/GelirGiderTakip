﻿using ArgeMup.HazirKod;
using ArgeMup.HazirKod.Ekİşlemler;
using ArgeMup.HazirKod.Ekranlar;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Gelir_Gider_Takip.Ekranlar
{
    public partial class Cari_Döküm : Form, IEkran_Dürtü
    {
        public void Güncelle()
        {
            Sorgula_Click(null, null);
        }
        public enum AçılışTürü_ { Normal, İlişkiliOlanlarıListele, DolaylıYoldanİlişkiliOlanlarıListele, SürümleriListele, Gizli };

        List<string> Kapsam_Grup = null, Kapsam_Muhatap = null;
        Banka1.İşyeri_Ödeme_ Ortak_Kullanım_Ödeme = null;
        AçılışTürü_ AçılışTürü;
        ArgeMup.HazirKod.Ekranlar.ListeKutusu.Ayarlar_ ListeKutusu_Ayarlar;
        ListeKutusu Sorgula_Şablonlar;
        const char Sorgula_AltToplamlar_Ayraç = 'é';
        readonly Font Sorgula_Kakü_kontrolNoktası;
        Cari_Döküm_Şablon_ Şablon;
        bool Cari_döküm_içinde_işlem_yapabilir = AnaKontrolcü.İzinliMi(AnaKontrolcü.Kullanıcılar_İzin.Cari_döküm_içinde_işlem_yapabilir);

        public Cari_Döküm(AçılışTürü_ AçılışTürü = AçılışTürü_.Normal, Banka1.İşyeri_Ödeme_ Ödeme = null)
        {
            InitializeComponent();
            if (AçılışTürü == AçılışTürü_.Gizli) Opacity = 0;

            Sorgula_Kakü_kontrolNoktası = new Font("Consolas", Tablo.DefaultCellStyle.Font.Size);

            Sorgula_MuhatapGrubu = new ArgeMup.HazirKod.Ekranlar.ListeKutusu();
            Ayraç_MuhatapGrubu_Muhatap.Panel1.Controls.Add(Sorgula_MuhatapGrubu);
            Sorgula_MuhatapGrubu.Dock = DockStyle.Fill;
            Sorgula_MuhatapGrubu.GeriBildirim_İşlemi += Sorgula_MuhatapGrubu_GeriBildirim_İşlemi;

            Sorgula_Muhatap = new ArgeMup.HazirKod.Ekranlar.ListeKutusu();
            Ayraç_MuhatapGrubu_Muhatap.Panel2.Controls.Add(Sorgula_Muhatap);
            Sorgula_Muhatap.Dock = DockStyle.Fill;
            Sorgula_Muhatap.GeriBildirim_İşlemi += Sorgula_Muhatap_GeriBildirim_İşlemi;

            Sorgula_Şablonlar = new ArgeMup.HazirKod.Ekranlar.ListeKutusu();
            Ayraç_Şablonlar_SorgulamaSeçenekleri.Panel1.Controls.Add(Sorgula_Şablonlar);
            Sorgula_Şablonlar.Dock = DockStyle.Fill;
            Sorgula_Şablonlar.GeriBildirim_İşlemi += Sorgula_Şablonlar_GeriBildirim_İşlemi;

            Sonuçlar_Ekranı.Dock = DockStyle.Fill;
            Ödeme_Ekranı.Dock = DockStyle.Fill;
            Düzenleme_Ekranı.Dock = DockStyle.Fill;
            KontrolNoktası_Ekranı.Dock = DockStyle.Fill;
            ÇokluSeçim_Ekranı.Dock = DockStyle.Fill;

            this.Ortak_Kullanım_Ödeme = Ödeme;
            this.AçılışTürü = AçılışTürü;

            ListeKutusu_Ayarlar = new ArgeMup.HazirKod.Ekranlar.ListeKutusu.Ayarlar_() { ÇokluSeçim = ListeKutusu.Ayarlar_.ÇokluSeçim_.CtrlTuşuİle };
            ListeKutusu_Ayarlar.TümTuşlarıKapat();

            if (!Cari_döküm_içinde_işlem_yapabilir)
            {
                Öde.Visible = false;
                Düzenle.Visible = false;
                KontrolNoktasıEkle.Visible = false;
                Yazdır.Visible = false;
                ÇokluSeçim.Visible = false;
            }
            Ekle.Visible = AnaKontrolcü.İzinliMi(AnaKontrolcü.Kullanıcılar_İzin.Gelir_gider_ekleyebilir);

            Ayraç_Filtre_TabloSonuç.SplitterDistance = Height * 30 / 100;

            Sorgula_MuhatapGrubu.Başlat(Ortak.Banka.Seçilenİşyeri.MuhatapGrubu_Listele(true), Ortak.Banka.Seçilenİşyeri.MuhatapGrubu_Listele(), "Muhatap Grupları", ListeKutusu_Ayarlar);

            ListeKutusu.Ayarlar_ ListeKutusu_Ayarlar_Şablon = new ListeKutusu.Ayarlar_(
                Eklenebilir: AnaKontrolcü.İzinliMi(AnaKontrolcü.Kullanıcılar_İzin.Ayarları_değiştirebilir),
                Silinebilir: AnaKontrolcü.İzinliMi(AnaKontrolcü.Kullanıcılar_İzin.Ayarları_değiştirebilir),
                ElemanKonumu: AnaKontrolcü.İzinliMi(AnaKontrolcü.Kullanıcılar_İzin.Ayarları_değiştirebilir) ? ListeKutusu.Ayarlar_.ElemanKonumu_.Değiştirilebilir : ListeKutusu.Ayarlar_.ElemanKonumu_.OlduğuGibi,
                AdıDeğiştirilebilir: false, Gizlenebilir: false);
            Sorgula_Şablonlar.Başlat(null, Ortak.Banka.Ayarlar.CariDökümŞablonlar.Keys.ToList(), "Şablonlar", ListeKutusu_Ayarlar_Şablon);

            Şablon = new Cari_Döküm_Şablon_(Sorgula_MuhatapGrubu, Sorgula_Muhatap);
            SorgulamaDetayları.SelectedObject = Şablon;

            ÇokluSeçim_Ertele_SüreKadar_Onay.Tag = ÇokluSeçim_Ertele_TamTarih_Onay;
            ÇokluSeçim_Ertele_TamTarih_Onay.Tag = ÇokluSeçim_Ertele_SüreKadar_Onay;
        }
        private void Cari_Döküm_Shown(object sender, EventArgs e)
        {
            switch (AçılışTürü)
            {
                default:
                case AçılışTürü_.Normal:
                    Sorgula_Click(null, null);
                    break;

                case AçılışTürü_.İlişkiliOlanlarıListele:
                    Şablon.Zamanlama_Türü = Cari_Döküm_Şablon_.Zamanlama_Türü_.İlk_işlem_tarihi;
                    Şablon.Zamanlama_Aralık = Cari_Döküm_Şablon_.Zamanlama_Aralık_.Sabit_aralık;
                    Şablon.Zamanlama_Başlangıç = Ortak_Kullanım_Ödeme.İlkİşlemTarihi;
                    Şablon.Zamanlama_Bitiş = Ortak_Kullanım_Ödeme.İlkİşlemTarihi;
                    Şablon.Zamanlama_GecikenleriKesinlikleGöster = Cari_Döküm_Şablon_.Zamanlama_GecikenleriKesinlikleGöster_.Hayır;

                    Sorgula_MuhatapGrubu.SeçilenEleman_Adı = Ortak_Kullanım_Ödeme.MuhatapGrubuAdı;
                    Sorgula_Muhatap.SeçilenEleman_Adı = Ortak_Kullanım_Ödeme.MuhatapAdı;
                    Sorgula_Click(null, null);
                    break;

                case AçılışTürü_.DolaylıYoldanİlişkiliOlanlarıListele:
                    Şablon.Zamanlama_Türü = Cari_Döküm_Şablon_.Zamanlama_Türü_.İşlem_tarihi;
                    Şablon.Zamanlama_Aralık = Cari_Döküm_Şablon_.Zamanlama_Aralık_.Sabit_aralık;
                    Şablon.Zamanlama_Başlangıç = Ortak_Kullanım_Ödeme.İlkİşlemTarihi;
                    Şablon.Zamanlama_Bitiş = Ortak_Kullanım_Ödeme.İlkİşlemTarihi;
                    Şablon.Zamanlama_GecikenleriKesinlikleGöster = Cari_Döküm_Şablon_.Zamanlama_GecikenleriKesinlikleGöster_.Hayır;

                    Sorgula_MuhatapGrubu.SeçilenEleman_Adı = Ortak_Kullanım_Ödeme.MuhatapGrubuAdı;
                    Sorgula_Muhatap.SeçilenEleman_Adı = Ortak_Kullanım_Ödeme.MuhatapAdı;
                    Sorgula_Click(null, null);
                    break;

                case AçılışTürü_.SürümleriListele:
                    Ayraç_Filtre_TabloSonuç.Panel1Collapsed = true;
                    KontrolNoktasıEkle.Enabled = false;

                    int sutun_sayısı = Tablo.ColumnCount;
                    List<DataGridViewRow> dizi = new List<DataGridViewRow>();
                    Tablo_SonİşlemTarihi.Visible = false;
                    string Son_Not = null;
                    Tablo_İlkİşlemTarihi.HeaderText = "İşlem Tarihi";

                    foreach (KeyValuePair<DateTime, Banka1.İşyeri_Ödeme_İşlem_> işlem in Ortak_Kullanım_Ödeme.İşlemler)
                    {
                        if (işlem.Value.Notlar.DoluMu()) Son_Not = işlem.Value.Notlar;

                        bool üyelik_olarak_göster = Ortak_Kullanım_Ödeme.Üyelik_KayıtTarihi != null;
                        bool gelir_olarak_göster = Ortak_Kullanım_Ödeme.Tipi.GelirMi();

                        object[] dizin = new object[sutun_sayısı];
                        dizin[Tablo_MuhatapGrubu.Index] = Ortak_Kullanım_Ödeme.MuhatapGrubuAdı;
                        dizin[Tablo_Muhatap.Index] = Ortak_Kullanım_Ödeme.MuhatapAdı;
                        dizin[Tablo_ÖdemeTarihi.Index] = işlem.Value.ÖdemeninYapılacağıTarih;
                        dizin[Tablo_Tip.Index] = işlem.Value.Tipi.Yazdır();
                        dizin[Tablo_Durum.Index] = işlem.Value.Durumu.Yazdır();
                        dizin[Tablo_Notlar.Index] = Son_Not;
                        dizin[Tablo_Miktar.Index] = Banka_Ortak.Yazdır_Ücret(işlem.Value.Miktarı, Ortak_Kullanım_Ödeme.ParaBirimi);
                        dizin[Tablo_Taksit.Index] = Ortak_Kullanım_Ödeme.Taksit.Yazdır();
                        dizin[Tablo_Üyelik.Index] = üyelik_olarak_göster;
                        dizin[Tablo_İlkİşlemTarihi.Index] = işlem.Key;
                        dizin[Tablo_KullanıcıAdı.Index] = işlem.Value.GerçekleştirenKullanıcıAdı;

                        DataGridViewRow _1_satır_dizisi_ = new DataGridViewRow();
                        dizi.Add(_1_satır_dizisi_);
                        _1_satır_dizisi_.CreateCells(Tablo, dizin);

                        _1_satır_dizisi_.Cells[Tablo_Miktar.Index].Style.BackColor = gelir_olarak_göster ? Ortak.Renk_Gelir : Ortak.Renk_Gider;
                        _1_satır_dizisi_.Cells[Tablo_Miktar.Index].ToolTipText = gelir_olarak_göster ? "Gelir" : "Gider";
                        if (üyelik_olarak_göster) _1_satır_dizisi_.Cells[Tablo_Üyelik.Index].ToolTipText = Ortak_Kullanım_Ödeme.Üyelik_KayıtTarihi.Value.Yazıya();
                        _1_satır_dizisi_.Cells[Tablo_Durum.Index].Style.BackColor = DurumRengi(işlem.Value.Durumu);
                    }

                    dizi.Reverse();
                    Tablo.Rows.AddRange(dizi.ToArray());
                    Tablo.ClearSelection();
                    break;
            }
        }
        private void Cari_Döküm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape) Close();
        }
        public static Color DurumRengi(Banka1.İşyeri_Ödeme_İşlem_.Durum_ Durum)
        {
            switch (Durum)
            {
                case Banka1.İşyeri_Ödeme_İşlem_.Durum_.Ödenmedi: return Ortak.Renk_Kırmızı;
                case Banka1.İşyeri_Ödeme_İşlem_.Durum_.KısmenÖdendi: return Ortak.Renk_Sarı;
                case Banka1.İşyeri_Ödeme_İşlem_.Durum_.TamÖdendi:
                case Banka1.İşyeri_Ödeme_İşlem_.Durum_.KısmiÖdemeYapıldı:
                case Banka1.İşyeri_Ödeme_İşlem_.Durum_.PeşinatÖdendi: return Ortak.Renk_Yeşil;
                case Banka1.İşyeri_Ödeme_İşlem_.Durum_.İptalEdildi: return Ortak.Renk_Gri;

                default: throw new Exception("Durum(" + Durum + ") uygun değil");
            }
        }
        public string Şablon_Seç_TabloyuOluştur(string Adı)
        {
            if (Adı.DoluMu() && !Sorgula_Şablonlar.Tüm_Elemanlar.Contains(Adı)) return "Yazdırma şablonu (" + Adı + ") bulunamadı";

            if (Sorgula_Şablonlar.SeçilenEleman_Adı == Adı) Sorgula_Click(null, null);
            else Sorgula_Şablonlar.SeçilenEleman_Adı = Adı;

            return null;
        }

        private bool Sorgula_MuhatapGrubu_GeriBildirim_İşlemi(string Adı, ArgeMup.HazirKod.Ekranlar.ListeKutusu.İşlemTürü Türü, string YeniAdı)
        {
            if (Türü == ArgeMup.HazirKod.Ekranlar.ListeKutusu.İşlemTürü.ElemanSeçildi)
            {
                Kapsam_Grup = Sorgula_MuhatapGrubu.SeçilenEleman_Adları;
                Kapsam_Muhatap = null;

                if (Kapsam_Grup.Count == 1)
                {
                    Sorgula_Muhatap.Başlat(Ortak.Banka.Seçilenİşyeri.Muhatap_Listele(Sorgula_MuhatapGrubu.SeçilenEleman_Adı, true), Ortak.Banka.Seçilenİşyeri.Muhatap_Listele(Sorgula_MuhatapGrubu.SeçilenEleman_Adı), "Muhataplar", ListeKutusu_Ayarlar);
                }
                else Sorgula_Muhatap.Başlat(null, null, "Muhataplar", ListeKutusu_Ayarlar);

                if (Kapsam_Grup.Count == 0 || Kapsam_Grup.Count == Sorgula_MuhatapGrubu.Tüm_Elemanlar.Count) Kapsam_Grup = null;
            }
            return true;
        }
        private bool Sorgula_Muhatap_GeriBildirim_İşlemi(string Adı, ArgeMup.HazirKod.Ekranlar.ListeKutusu.İşlemTürü Türü, string YeniAdı)
        {
            if (Türü == ArgeMup.HazirKod.Ekranlar.ListeKutusu.İşlemTürü.ElemanSeçildi)
            {
                Kapsam_Muhatap = Sorgula_Muhatap.SeçilenEleman_Adları;

                if (Kapsam_Muhatap.Count == 0 || Kapsam_Muhatap.Count == Sorgula_Muhatap.Tüm_Elemanlar.Count) Kapsam_Muhatap = null;
            }
            return true;
        }
        private bool Sorgula_Şablonlar_GeriBildirim_İşlemi(string Adı, ArgeMup.HazirKod.Ekranlar.ListeKutusu.İşlemTürü Türü, string YeniAdı)
        {
            if (Adı.BoşMu()) return false;

            switch (Türü)
            {
                case ListeKutusu.İşlemTürü.YeniEklendi:
                    Şablon.Kapsam_Grup = Kapsam_Grup;
                    Şablon.Kapsam_Muhatap = Kapsam_Muhatap;

                    Ortak.Banka.Ayarlar.CariDökümŞablonlar[Adı] = Şablon;
                    Ortak.Banka.Ayarlar.DeğişiklikYapıldı = true;
                    Banka_Ortak.DeğişiklikleriKaydet();
                    break;

                case ListeKutusu.İşlemTürü.ElemanSeçildi:
                    if (!Ortak.Banka.Ayarlar.CariDökümŞablonlar.TryGetValue(Adı, out Şablon)) Şablon = new Cari_Döküm_Şablon_(Sorgula_MuhatapGrubu, Sorgula_Muhatap);
                    else Şablon = Şablon.Kopyala(Sorgula_MuhatapGrubu, Sorgula_Muhatap);

                    SorgulamaDetayları.SelectedObject = Şablon;
                    Sorgula_MuhatapGrubu.SeçilenEleman_Adları = Şablon.Kapsam_Grup;
                    Sorgula_Muhatap.SeçilenEleman_Adları = Şablon.Kapsam_Muhatap;

                    Sorgula_Click(null, null);
                    break;

                case ListeKutusu.İşlemTürü.KonumDeğişikliğiKaydedildi:
                    Ortak.Banka.Ayarlar.CariDökümŞablonlar = Ortak.Banka.Ayarlar.CariDökümŞablonlar.Sırala(Sorgula_Şablonlar.Tüm_Elemanlar) as Dictionary<string, Ekranlar.Cari_Döküm_Şablon_>;
                    Ortak.Banka.Ayarlar.DeğişiklikYapıldı = true;
                    Banka_Ortak.DeğişiklikleriKaydet();
                    break;

                case ListeKutusu.İşlemTürü.Silindi:
                    Ortak.Banka.Ayarlar.CariDökümŞablonlar.Remove(Adı);
                    Ortak.Banka.Ayarlar.DeğişiklikYapıldı = true;
                    Banka_Ortak.DeğişiklikleriKaydet();
                    break;
            }

            return true;
        }
        private void SorgulamaDetayları_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            if (e.ChangedItem.Value == null || e.ChangedItem.Value.GetType() != typeof(DateTime)) return;

            Şablon.Zamanlama_Aralık = Cari_Döküm_Şablon_.Zamanlama_Aralık_.Sabit_aralık;

            SorgulamaDetayları.Refresh();
        }
        private void Sorgula_Click(object sender, EventArgs e)
        {
            Sorgula.Enabled = false;
            TabloİçeriğiArama.BackColor = Ortak.Renk_Kırmızı;
            Öde_Geri_Click(null, null);
            Düzenle_Geri_Click(null, null);
            KontrolNoktası_Geri_Click(null, null);
            ÇokluSeçim_Geri_Click(null, null);
            Application.DoEvents();

            if (Şablon.Zamanlama_Aralık != Cari_Döküm_Şablon_.Zamanlama_Aralık_.Sabit_aralık) Şablon.Zamanlama_Aralık = Şablon.Zamanlama_Aralık; //tarihleri güncelle
            SorgulamaDetayları.Refresh();
            DateOnly başlangıç_d = DateOnly.FromDateTime(Şablon.Zamanlama_Başlangıç);
            DateOnly bitiş_d = DateOnly.FromDateTime(Şablon.Zamanlama_Bitiş);

            Açıklamalar.Text = null;
            Tablo.Rows.Clear();
            TabloİçeriğiArama.Text = null;
            Tablo_SelectionChanged(null, null);
            if (Tablo.SortedColumn != null)
            {
                DataGridViewColumn col = Tablo.SortedColumn;
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
                col.SortMode = DataGridViewColumnSortMode.Automatic;
            }

            DateOnly şimdi = DateOnly.FromDateTime(DateTime.Now);
            double[] Toplam_Gelir = new double[(int)Banka1.İşyeri_Ödeme_.ParaBirimi_.ElemanSayısı];
            double[] Toplam_Gider = new double[(int)Banka1.İşyeri_Ödeme_.ParaBirimi_.ElemanSayısı];
            int sutun_sayısı = Tablo.ColumnCount;
            List<DataGridViewRow> dizi = new List<DataGridViewRow>();
            Dictionary<string, double> AltToplamlar = new Dictionary<string, double>();

            foreach (string yıl in Ortak.Banka.Seçilenİşyeri.Ödemeler_Listele_Yıllar())
            {
                //istenmeyen yılları atla
                int yıll = yıl.TamSayıya();
                if (Şablon.Zamanlama_Türü == Cari_Döküm_Şablon_.Zamanlama_Türü_.İlk_işlem_tarihi && (yıll < başlangıç_d.Year || yıll > bitiş_d.Year)) continue;

                Banka1.İşyeri_BirYıllıkDönem_ BirYıllıkDönem = Ortak.Banka.Seçilenİşyeri.Ödemeler_Listele_BirYıllıkDönem(yıl);
                foreach (Banka1.İşyeri_Ödeme_ ödeme in BirYıllıkDönem.Ödemeler)
                {
                    _TabloyaEkle_(ödeme);
                }
            }

            //Zaman aralığı içindeki gelecekteki üyeliklere ait ödemeler
            foreach (Banka1.İşyeri_Ödeme_ ödeme in Ortak.Banka.Seçilenİşyeri.Üyelik_OlacaklarıHesapla(bitiş_d))
            {
                _TabloyaEkle_(ödeme);
            }

            if (Şablon.Diğer_AltToplam != Cari_Döküm_Şablon_.Diğer_AltToplam_.Gerekli_değil)
            {
                if (AltToplamlar.Keys.Count == 0) AltToplamlar.Add("1" + Sorgula_AltToplamlar_Ayraç + "Geçerli kayıt bulunamadı" + Sorgula_AltToplamlar_Ayraç + "Geçerli kayıt bulunamadı", 0);

                foreach (KeyValuePair<string, double> AltToplam in AltToplamlar)
                {
                    object[] dizin = new object[sutun_sayısı];
                    string[] AltToplam_Ad_Dizisi = AltToplam.Key.Split(Sorgula_AltToplamlar_Ayraç);

                    //parabirimi_sayısı + Grup
                    //parabirimi_sayısı + Grup + muhatap
                    if (AltToplam_Ad_Dizisi.Length == 3) dizin[Tablo_Muhatap.Index] = AltToplam_Ad_Dizisi[2];
                    dizin[Tablo_MuhatapGrubu.Index] = AltToplam_Ad_Dizisi[1];
                    dizin[Tablo_Tip.Index] = "Alt toplam";
                    dizin[Tablo_Miktar.Index] = Banka_Ortak.Yazdır_Ücret(AltToplam.Value, (Banka1.İşyeri_Ödeme_.ParaBirimi_)AltToplam_Ad_Dizisi[0].TamSayıya());

                    DataGridViewRow _1_satır_dizisi_ = new DataGridViewRow();
                    dizi.Add(_1_satır_dizisi_);
                    _1_satır_dizisi_.CreateCells(Tablo, dizin);
                    _1_satır_dizisi_.Cells[Tablo_Tip.Index].Style.BackColor = Ortak.Renk_Sarı;
                }
            }

            Tablo_Durum.Visible = Şablon.Sütunlar_Durum == Cari_Döküm_Şablon_.Sütunlar_Durum_.Göster;
            Tablo_İlkİşlemTarihi.Visible = Şablon.Sütunlar_İlkİşlemTarihi == Cari_Döküm_Şablon_.Sütunlar_Durum_.Göster;
            Tablo_KullanıcıAdı.Visible = Şablon.Sütunlar_Kullanıcı == Cari_Döküm_Şablon_.Sütunlar_Durum_.Göster;
            Tablo_Notlar.Visible = Şablon.Sütunlar_Notlar == Cari_Döküm_Şablon_.Sütunlar_Durum_.Göster;
            Tablo_SonİşlemTarihi.Visible = Şablon.Sütunlar_SonİşlemTarihi == Cari_Döküm_Şablon_.Sütunlar_Durum_.Göster;
            Tablo_Taksit.Visible = Şablon.Sütunlar_Taksit == Cari_Döküm_Şablon_.Sütunlar_Durum_.Göster;
            Tablo_Tip.Visible = Şablon.Sütunlar_Tip == Cari_Döküm_Şablon_.Sütunlar_Durum_.Göster;
            Tablo_ÖdemeTarihi.Visible = Şablon.Sütunlar_ÖdemeGünü == Cari_Döküm_Şablon_.Sütunlar_Durum_.Göster;
            Tablo_Üyelik.Visible = Şablon.Sütunlar_Üyelik == Cari_Döküm_Şablon_.Sütunlar_Durum_.Göster;
            Tablo_MuhatapGrubu.Visible = Şablon.Sütunlar_Grup == Cari_Döküm_Şablon_.Sütunlar_Durum_.Göster;
            Tablo_Muhatap.Visible = Şablon.Sütunlar_Muhatap == Cari_Döküm_Şablon_.Sütunlar_Durum_.Göster;
            Tablo_Miktar.Visible = Şablon.Sütunlar_Miktar == Cari_Döküm_Şablon_.Sütunlar_Durum_.Göster;
            Tablo.Rows.AddRange(dizi.ToArray());
            DataGridViewTextBoxColumn sıralama_sütunu = null;
            switch (Şablon.Sütunlar_Sırala_Sütun)
            {
                case Cari_Döküm_Şablon_.Sütunlar_Sırala_Sütun_.Grup: sıralama_sütunu = Tablo_MuhatapGrubu; break;
                case Cari_Döküm_Şablon_.Sütunlar_Sırala_Sütun_.Muhatap: sıralama_sütunu = Tablo_Muhatap; break;
                case Cari_Döküm_Şablon_.Sütunlar_Sırala_Sütun_.Ödeme_Günü: sıralama_sütunu = Tablo_ÖdemeTarihi; break;
                case Cari_Döküm_Şablon_.Sütunlar_Sırala_Sütun_.Tip: sıralama_sütunu = Tablo_Tip; break;
                case Cari_Döküm_Şablon_.Sütunlar_Sırala_Sütun_.Durum: sıralama_sütunu = Tablo_Durum; break;
                case Cari_Döküm_Şablon_.Sütunlar_Sırala_Sütun_.Notlar: sıralama_sütunu = Tablo_Notlar; break;
                case Cari_Döküm_Şablon_.Sütunlar_Sırala_Sütun_.Taksit: sıralama_sütunu = Tablo_Taksit; break;
                case Cari_Döküm_Şablon_.Sütunlar_Sırala_Sütun_.Son_İşlem_Tarihi: sıralama_sütunu = Tablo_SonİşlemTarihi; break;
                case Cari_Döküm_Şablon_.Sütunlar_Sırala_Sütun_.İlk_İşlem_Tarihi: sıralama_sütunu = Tablo_İlkİşlemTarihi; break;
                case Cari_Döküm_Şablon_.Sütunlar_Sırala_Sütun_.Kullanıcı: sıralama_sütunu = Tablo_KullanıcıAdı; break;
            }
            Tablo.Sort(sıralama_sütunu, Şablon.Sütunlar_Sırala == Cari_Döküm_Şablon_.Sütunlar_Sırala_.Büyükten_küçüğe ? System.ComponentModel.ListSortDirection.Descending : System.ComponentModel.ListSortDirection.Ascending);
            Tablo.ClearSelection();

            Açıklamalar.Text = Banka_Ortak.Yazdır_Özet(Toplam_Gelir, Toplam_Gider, true, false);
            Sorgula.Enabled = true;
            Yazdır.Enabled = dizi.Count() > 0;
            TabloİçeriğiArama.BackColor = Color.White;
            Sorgula.Text = "Sorgula (" + Tablo.RowCount + ")";

            void _TabloyaEkle_(Banka1.İşyeri_Ödeme_ _ödeme_)
            {
                KeyValuePair<DateTime, Banka1.İşyeri_Ödeme_İşlem_> ilk_işlem = _ödeme_.İşlemler.First();
                KeyValuePair<DateTime, Banka1.İşyeri_Ödeme_İşlem_> son_işlem = _ödeme_.İşlemler.Last();

                Banka1.İşyeri_Ödeme_İşlem_.Tipi_ tipi = son_işlem.Value.Tipi;
                DataGridViewRow _1_satır_dizisi_;
                if (tipi == Banka1.İşyeri_Ödeme_İşlem_.Tipi_.KontrolNoktası)
                {
                    if (Şablon.Tipi_KontrolNoktası == Cari_Döküm_Şablon_.Sıralama_ÜçSeçenek_.Hariç_tut) return;

                    DateTime trh = Şablon.Zamanlama_Türü == Cari_Döküm_Şablon_.Zamanlama_Türü_.İlk_işlem_tarihi ? ilk_işlem.Key : ilk_işlem.Value.ÖdemeninYapılacağıTarih.ToDateTime(new TimeOnly());
                    if (trh < Şablon.Zamanlama_Başlangıç || trh > Şablon.Zamanlama_Bitiş) return; //kapsam dışında

                    object[] dizin = new object[sutun_sayısı];
                    dizin[Tablo_Tip.Index] = son_işlem.Value.Tipi.Yazdır();
                    dizin[Tablo_ÖdemeTarihi.Index] = ilk_işlem.Value.ÖdemeninYapılacağıTarih;
                    dizin[Tablo_Notlar.Index] = _ödeme_.Notlar;
                    dizin[Tablo_SonİşlemTarihi.Index] = ilk_işlem.Key;
                    dizin[Tablo_İlkİşlemTarihi.Index] = ilk_işlem.Key;
                    dizin[Tablo_KullanıcıAdı.Index] = son_işlem.Value.GerçekleştirenKullanıcıAdı;

                    _1_satır_dizisi_ = new DataGridViewRow();
                    dizi.Add(_1_satır_dizisi_);
                    _1_satır_dizisi_.CreateCells(Tablo, dizin);

                    for (int i = 0; i < sutun_sayısı; i++)
                    {
                        _1_satır_dizisi_.Cells[i].Style.BackColor = Ortak.Renk_KontrolNoktası;
                    }
                    _1_satır_dizisi_.Cells[Tablo_Notlar.Index].Style.Font = Sorgula_Kakü_kontrolNoktası;
                }
                else
                {
                    Banka1.İşyeri_Ödeme_İşlem_.Durum_ durumu = son_işlem.Value.Durumu;
                    bool gelir_olarak_göster = son_işlem.Value.Tipi.GelirMi();
                    bool üyelik_olarak_göster = _ödeme_.Üyelik_KayıtTarihi != null;
                    bool gecikti_olarak_göster = !durumu.ÖdendiMi() && son_işlem.Value.ÖdemeninYapılacağıTarih <= şimdi;
                    bool iptaledildi_olarak_göster = durumu == Banka1.İşyeri_Ödeme_İşlem_.Durum_.İptalEdildi;

                    if (gecikti_olarak_göster && Şablon.Zamanlama_GecikenleriKesinlikleGöster == Cari_Döküm_Şablon_.Zamanlama_GecikenleriKesinlikleGöster_.Evet) { }
                    else
                    {
                        if (Şablon.Zamanlama_Türü == Cari_Döküm_Şablon_.Zamanlama_Türü_.İşlem_tarihi)
                        {
                            foreach (DateTime trh in _ödeme_.İşlemler.Keys)
                            {
                                if (Ortak_Kullanım_Ödeme != null)
                                {
                                    if (Ortak_Kullanım_Ödeme.İşlemler.ContainsKey(trh)) goto _ödeme_İşlemler_Keys_Devam_;
                                }
                                else
                                {
                                    if (trh >= Şablon.Zamanlama_Başlangıç && trh <= Şablon.Zamanlama_Bitiş) goto _ödeme_İşlemler_Keys_Devam_;
                                }
                            }
                            return; //kapsam dışında

                        _ödeme_İşlemler_Keys_Devam_:;
                        }
                        else
                        {
                            DateTime trh;
                            switch (Şablon.Zamanlama_Türü)
                            {
                                case Cari_Döküm_Şablon_.Zamanlama_Türü_.Ödeme_tarihi: trh = son_işlem.Value.ÖdemeninYapılacağıTarih.ToDateTime(new TimeOnly()); break;
                                case Cari_Döküm_Şablon_.Zamanlama_Türü_.Son_işlem_tarihi: trh = son_işlem.Key; break;
                                case Cari_Döküm_Şablon_.Zamanlama_Türü_.İlk_işlem_tarihi: trh = ilk_işlem.Key; break;
                                default: trh = DateTime.MinValue; break;
                            }

                            if (trh < Şablon.Zamanlama_Başlangıç || trh > Şablon.Zamanlama_Bitiş) return; //kapsam dışında
                        }
                    }

                    bool listele = (Kapsam_Grup == null || Kapsam_Grup.Contains(_ödeme_.MuhatapGrubuAdı)) &&
                                   (Kapsam_Muhatap == null || Kapsam_Muhatap.Contains(_ödeme_.MuhatapAdı));
                    if (!listele) return; //kapsam dışında

                    listele = Şablon.Diğer_AltToplam != Cari_Döküm_Şablon_.Diğer_AltToplam_.Gerekli_değil && !iptaledildi_olarak_göster;
                    if (listele)
                    {
                        string anahtar = ((int)_ödeme_.ParaBirimi).Yazıya() + Sorgula_AltToplamlar_Ayraç + _ödeme_.MuhatapGrubuAdı;
                        double miktar = 0;

                        if (Şablon.Diğer_AltToplam == Cari_Döküm_Şablon_.Diğer_AltToplam_.Sadece_grup_için_hesapla || Şablon.Diğer_AltToplam == Cari_Döküm_Şablon_.Diğer_AltToplam_.Tümü_için_hesapla)
                        {
                            if (AltToplamlar.ContainsKey(anahtar)) miktar = AltToplamlar[anahtar];
                            if (gelir_olarak_göster) miktar += _ödeme_.Miktarı;
                            else miktar -= _ödeme_.Miktarı;
                            AltToplamlar[anahtar] = miktar;
                        }

                        if (Şablon.Diğer_AltToplam == Cari_Döküm_Şablon_.Diğer_AltToplam_.Sadece_muhatap_için_hesapla || Şablon.Diğer_AltToplam == Cari_Döküm_Şablon_.Diğer_AltToplam_.Tümü_için_hesapla)
                        {
                            anahtar += Sorgula_AltToplamlar_Ayraç + _ödeme_.MuhatapAdı;
                            miktar = 0;
                            if (AltToplamlar.ContainsKey(anahtar)) miktar = AltToplamlar[anahtar];
                            if (gelir_olarak_göster) miktar += _ödeme_.Miktarı;
                            else miktar -= _ödeme_.Miktarı;
                            AltToplamlar[anahtar] = miktar;
                        }
                    }

                    switch (durumu)
                    {
                        case Banka1.İşyeri_Ödeme_İşlem_.Durum_.Ödenmedi:
                            if (Şablon.Durumu_Ödenmedi == Cari_Döküm_Şablon_.Sıralama_ÜçSeçenek_.Hariç_tut) return;
                            else if (Şablon.Durumu_Ödenmedi == Cari_Döküm_Şablon_.Sıralama_ÜçSeçenek_.Dahil_et) goto Dahil_et;
                            break;

                        case Banka1.İşyeri_Ödeme_İşlem_.Durum_.KısmenÖdendi:
                            if (Şablon.Durumu_KısmenÖdendi == Cari_Döküm_Şablon_.Sıralama_ÜçSeçenek_.Hariç_tut) return;
                            else if (Şablon.Durumu_KısmenÖdendi == Cari_Döküm_Şablon_.Sıralama_ÜçSeçenek_.Dahil_et) goto Dahil_et;
                            break;

                        case Banka1.İşyeri_Ödeme_İşlem_.Durum_.TamÖdendi:
                            if (Şablon.Durumu_TamÖdendi == Cari_Döküm_Şablon_.Sıralama_ÜçSeçenek_.Hariç_tut) return;
                            else if (Şablon.Durumu_TamÖdendi == Cari_Döküm_Şablon_.Sıralama_ÜçSeçenek_.Dahil_et) goto Dahil_et;
                            break;

                        case Banka1.İşyeri_Ödeme_İşlem_.Durum_.İptalEdildi:
                            if (Şablon.Durumu_İptalEdildi == Cari_Döküm_Şablon_.Sıralama_ÜçSeçenek_.Hariç_tut) return;
                            else if (Şablon.Durumu_İptalEdildi == Cari_Döküm_Şablon_.Sıralama_ÜçSeçenek_.Dahil_et) goto Dahil_et;
                            break;

                        case Banka1.İşyeri_Ödeme_İşlem_.Durum_.KısmiÖdemeYapıldı:
                            if (Şablon.Durumu_KısmiÖdemeYapıldı == Cari_Döküm_Şablon_.Sıralama_ÜçSeçenek_.Hariç_tut) return;
                            else if (Şablon.Durumu_KısmiÖdemeYapıldı == Cari_Döküm_Şablon_.Sıralama_ÜçSeçenek_.Dahil_et) goto Dahil_et;
                            break;

                        case Banka1.İşyeri_Ödeme_İşlem_.Durum_.PeşinatÖdendi:
                            if (Şablon.Durumu_PeşinatÖdendi == Cari_Döküm_Şablon_.Sıralama_ÜçSeçenek_.Hariç_tut) return;
                            else if (Şablon.Durumu_PeşinatÖdendi == Cari_Döküm_Şablon_.Sıralama_ÜçSeçenek_.Dahil_et) goto Dahil_et;
                            break;

                        default: throw new Exception("Durumu (" + durumu + ") uygun değil");
                    }

                    switch (tipi)
                    {
                        case Banka1.İşyeri_Ödeme_İşlem_.Tipi_.Gider:
                            if (Şablon.Tipi_Gider == Cari_Döküm_Şablon_.Sıralama_ÜçSeçenek_.Hariç_tut) return;
                            else if (Şablon.Tipi_Gider == Cari_Döküm_Şablon_.Sıralama_ÜçSeçenek_.Dahil_et) goto Dahil_et;
                            break;

                        case Banka1.İşyeri_Ödeme_İşlem_.Tipi_.MaaşÖdemesi:
                            if (Şablon.Tipi_MaaşÖdemesi == Cari_Döküm_Şablon_.Sıralama_ÜçSeçenek_.Hariç_tut) return;
                            else if (Şablon.Tipi_MaaşÖdemesi == Cari_Döküm_Şablon_.Sıralama_ÜçSeçenek_.Dahil_et) goto Dahil_et;
                            break;

                        case Banka1.İşyeri_Ödeme_İşlem_.Tipi_.AvansVerilmesi:
                            if (Şablon.Tipi_AvansVerilmesi == Cari_Döküm_Şablon_.Sıralama_ÜçSeçenek_.Hariç_tut) return;
                            else if (Şablon.Tipi_AvansVerilmesi == Cari_Döküm_Şablon_.Sıralama_ÜçSeçenek_.Dahil_et) goto Dahil_et;
                            break;

                        case Banka1.İşyeri_Ödeme_İşlem_.Tipi_.Gelir:
                            if (Şablon.Tipi_Gelir == Cari_Döküm_Şablon_.Sıralama_ÜçSeçenek_.Hariç_tut) return;
                            else if (Şablon.Tipi_Gelir == Cari_Döküm_Şablon_.Sıralama_ÜçSeçenek_.Dahil_et) goto Dahil_et;
                            break;

                        case Banka1.İşyeri_Ödeme_İşlem_.Tipi_.AvansÖdemesi:
                            if (Şablon.Tipi_AvansÖdemesi == Cari_Döküm_Şablon_.Sıralama_ÜçSeçenek_.Hariç_tut) return;
                            else if (Şablon.Tipi_AvansÖdemesi == Cari_Döküm_Şablon_.Sıralama_ÜçSeçenek_.Dahil_et) goto Dahil_et;
                            break;

                        default: throw new Exception("Tipi (" + tipi + ") uygun değil");
                    }

                    if (Şablon.Miktar_ParaBirimi == Cari_Döküm_Şablon_.Miktar_ParaBirimi_.Farketmez) goto Dahil_et;
                    else if (Şablon.Miktar_EnAz == -1 && Şablon.Miktar_EnÇok == -1)
                    {
                        switch (Şablon.Miktar_ParaBirimi)
                        {
                            case Cari_Döküm_Şablon_.Miktar_ParaBirimi_.Türk_lirası:
                                if (_ödeme_.ParaBirimi != Banka1.İşyeri_Ödeme_.ParaBirimi_.TürkLirası) return;
                                break;

                            case Cari_Döküm_Şablon_.Miktar_ParaBirimi_.Avro:
                                if (_ödeme_.ParaBirimi != Banka1.İşyeri_Ödeme_.ParaBirimi_.Avro) return;
                                break;

                            case Cari_Döküm_Şablon_.Miktar_ParaBirimi_.Dolar:
                                if (_ödeme_.ParaBirimi != Banka1.İşyeri_Ödeme_.ParaBirimi_.Dolar) return;
                                break;
                        }
                    }
                    else
                    {
                        double karşılığı = Ortak.ParaBirimi_Dönüştür(son_işlem.Value.Miktarı, _ödeme_.ParaBirimi, (Banka1.İşyeri_Ödeme_.ParaBirimi_)Şablon.Miktar_ParaBirimi);
                        if ((Şablon.Miktar_EnAz >= 0 && karşılığı < Şablon.Miktar_EnAz) ||
                            (Şablon.Miktar_EnÇok >= 0 && karşılığı > Şablon.Miktar_EnÇok)) return;
                    }

                Dahil_et:
                    if (!iptaledildi_olarak_göster)
                    {
                        if (gelir_olarak_göster)
                        {
                            Toplam_Gelir[(int)_ödeme_.ParaBirimi] += son_işlem.Value.Miktarı;
                        }
                        else
                        {
                            Toplam_Gider[(int)_ödeme_.ParaBirimi] += son_işlem.Value.Miktarı;
                        }
                    }

                    object[] dizin = new object[sutun_sayısı];
                    dizin[Tablo_MuhatapGrubu.Index] = _ödeme_.MuhatapGrubuAdı;
                    dizin[Tablo_Muhatap.Index] = _ödeme_.MuhatapAdı;
                    dizin[Tablo_ÖdemeTarihi.Index] = son_işlem.Value.ÖdemeninYapılacağıTarih;
                    dizin[Tablo_Tip.Index] = son_işlem.Value.Tipi.Yazdır();
                    dizin[Tablo_Durum.Index] = son_işlem.Value.Durumu.Yazdır();
                    dizin[Tablo_Notlar.Index] = _ödeme_.Notlar;
                    dizin[Tablo_Miktar.Index] = Banka_Ortak.Yazdır_Ücret(son_işlem.Value.Miktarı, _ödeme_.ParaBirimi);
                    dizin[Tablo_Taksit.Index] = _ödeme_.Taksit.Yazdır();
                    dizin[Tablo_Üyelik.Index] = üyelik_olarak_göster;
                    dizin[Tablo_SonİşlemTarihi.Index] = son_işlem.Key;
                    dizin[Tablo_İlkİşlemTarihi.Index] = _ödeme_.İşlemler.First().Key;
                    dizin[Tablo_KullanıcıAdı.Index] = son_işlem.Value.GerçekleştirenKullanıcıAdı;

                    _1_satır_dizisi_ = new DataGridViewRow();
                    dizi.Add(_1_satır_dizisi_);
                    _1_satır_dizisi_.CreateCells(Tablo, dizin);
                    _1_satır_dizisi_.Tag = _ödeme_;

                    if (_ödeme_.Üyelik_HenüzKaydedilmemişBirÖdeme)
                    {
                        //Gelecek Döneem ait
                        _1_satır_dizisi_.Cells[Tablo_ÖdemeTarihi.Index].Style.BackColor = Ortak.Renk_Mavi;
                        _1_satır_dizisi_.Cells[Tablo_ÖdemeTarihi.Index].ToolTipText = "Sadece bilgi amaçlıdır" + Environment.NewLine +
                            Banka_Ortak.Yazdır_Tarih_Gün(son_işlem.Value.ÖdemeninYapılacağıTarih.ToDateTime(new TimeOnly()) - şimdi.ToDateTime(new TimeOnly())) + " daha var";
                    }
                    else
                    {
                        //Normal
                        string açıklama = Banka_Ortak.Yazdır_Tarih_Gün(şimdi.ToDateTime(new TimeOnly()) - son_işlem.Value.ÖdemeninYapılacağıTarih.ToDateTime(new TimeOnly()));
                        if (gecikti_olarak_göster) _1_satır_dizisi_.Cells[Tablo_ÖdemeTarihi.Index].ToolTipText = Banka_Ortak.Yazdır_Tarih_Gün(şimdi.ToDateTime(new TimeOnly()) - son_işlem.Value.ÖdemeninYapılacağıTarih.ToDateTime(new TimeOnly())) + " GECİKTİ";
                        else _1_satır_dizisi_.Cells[Tablo_ÖdemeTarihi.Index].ToolTipText = durumu.ÖdendiMi() ? null : açıklama + " daha var";
                    }

                    if (_ödeme_.İşlemler.Count > 1)
                    {
                        _1_satır_dizisi_.Cells[Tablo_SonİşlemTarihi.Index].ToolTipText = "Sürüm : " + _ödeme_.İşlemler.Count;
                        _1_satır_dizisi_.Cells[Tablo_SonİşlemTarihi.Index].Style.BackColor = Ortak.Renk_Mavi; //çok sürümlü
                    }
                    _1_satır_dizisi_.Cells[Tablo_Miktar.Index].Style.BackColor = gelir_olarak_göster ? Ortak.Renk_Gelir : Ortak.Renk_Gider;
                    _1_satır_dizisi_.Cells[Tablo_Miktar.Index].ToolTipText = gelir_olarak_göster ? "Gelir" : "Gider";
                    if (gecikti_olarak_göster) _1_satır_dizisi_.Cells[Tablo_ÖdemeTarihi.Index].Style.BackColor = Ortak.Renk_Kırmızı;
                    if (üyelik_olarak_göster) _1_satır_dizisi_.Cells[Tablo_Üyelik.Index].ToolTipText = _ödeme_.Üyelik_KayıtTarihi.Value.Yazıya();
                    _1_satır_dizisi_.Cells[Tablo_Durum.Index].Style.BackColor = DurumRengi(_ödeme_.Durumu);
                }
            }
        }

        bool TabloİçeriğiArama_Çalışıyor = false;
        bool TabloİçeriğiArama_KapatmaTalebi = false;
        int TabloİçeriğiArama_Tik = 0;
        int TabloİçeriğiArama_Sayac_Bulundu = 0;
        private void TabloİçeriğiArama_TextChanged(object sender, EventArgs e)
        {
            TabloİçeriğiArama_Tik = Environment.TickCount + 100;
            if (TabloİçeriğiArama.Text.Length < 2)
            {
                if (TabloİçeriğiArama_Sayac_Bulundu != 0)
                {
                    TabloİçeriğiArama.BackColor = Ortak.Renk_Kırmızı;

                    for (int satır = 0; satır < Tablo.RowCount; satır++)
                    {
                        Tablo.Rows[satır].Visible = true;
                        if (TabloİçeriğiArama_Tik < Environment.TickCount) { Application.DoEvents(); TabloİçeriğiArama_Tik = Environment.TickCount + 100; }
                    }

                    TabloİçeriğiArama.BackColor = Color.White;
                    TabloİçeriğiArama_Sayac_Bulundu = 0;
                }

                Sorgula.Text = "Sorgula (" + Tablo.RowCount + ")";
                return;
            }

            if (TabloİçeriğiArama_Çalışıyor) { TabloİçeriğiArama_KapatmaTalebi = true; return; }

            TabloİçeriğiArama_Çalışıyor = true;
            TabloİçeriğiArama_KapatmaTalebi = false;
            TabloİçeriğiArama_Sayac_Bulundu = 0;
            TabloİçeriğiArama_Tik = Environment.TickCount + 500;
            TabloİçeriğiArama.BackColor = Ortak.Renk_Kırmızı;

            Font Kalın = new Font(Tablo.Font, FontStyle.Bold);
            string[] arananlar = TabloİçeriğiArama.Text.ToLower().Split(' ');
            for (int satır = 0; satır < Tablo.RowCount && !TabloİçeriğiArama_KapatmaTalebi; satır++)
            {
                bool bulundu = false;
                for (int sutun = 0; sutun < Tablo.Columns.Count; sutun++)
                {
                    string içerik;
                    if (Tablo[sutun, satır].Value is bool) continue;
                    else if (Tablo[sutun, satır].Value is DateOnly) içerik = ((DateOnly)Tablo[sutun, satır].Value).Yazıya();
                    else if (Tablo[sutun, satır].Value is DateTime) içerik = ((DateTime)Tablo[sutun, satır].Value).Yazıya();
                    else içerik = (string)Tablo[sutun, satır].Value;

                    if (!string.IsNullOrEmpty(içerik))
                    {
                        içerik = içerik.ToLower();
                        int bulundu_adet = 0;
                        foreach (string arn in arananlar)
                        {
                            if (!içerik.Contains(arn)) break;

                            bulundu_adet++;
                        }

                        if (bulundu_adet == arananlar.Length)
                        {
                            Tablo[sutun, satır].Style.Font = Kalın;
                            bulundu = true;
                        }
                        else Tablo[sutun, satır].Style.Font = null;
                    }
                }

                Tablo.Rows[satır].Visible = bulundu;
                if (bulundu) TabloİçeriğiArama_Sayac_Bulundu++;

                if (TabloİçeriğiArama_Tik < Environment.TickCount) { Application.DoEvents(); TabloİçeriğiArama_Tik = Environment.TickCount + 500; }
            }

            if (TabloİçeriğiArama_Sayac_Bulundu == 0) TabloİçeriğiArama_Sayac_Bulundu = -1;
            else Sorgula.Text = "Sorgula (" + TabloİçeriğiArama_Sayac_Bulundu + "/" + Tablo.RowCount + ")";

            TabloİçeriğiArama.BackColor = Color.White;
            TabloİçeriğiArama_Çalışıyor = false;

            if (TabloİçeriğiArama_KapatmaTalebi) TabloİçeriğiArama_TextChanged(null, null);
            TabloİçeriğiArama_KapatmaTalebi = false;
        }
        private void Tablo_SelectionChanged(object sender, EventArgs e)
        {
            int SeçilenSatırSayısı = Tablo.SelectedRows.Count;

            if (SeçilenSatırSayısı < 1 || SeçilenSatırSayısı > 1)
            {
                Öde.Enabled = false;
                Düzenle.Enabled = false;
                DoğrudanİlişkiliÖdemeleriListele.Enabled = false;
                DolaylıİlişkiliÖdemeleriListele.Enabled = false;
                SürümleriListele.Enabled = false;
                ÇokluSeçim.Enabled = false;
            }

            if (SeçilenSatırSayısı >= 1 && Cari_döküm_içinde_işlem_yapabilir)
            {
                int adet_Ödenebilir = 0, adet_Ertelenebilir = 0, adet_ÖdenmediOlarakİşaretlenebilir = 0, adet_İptalEdilebilir = 0;
                foreach (DataGridViewRow str in Tablo.SelectedRows)
                {
                    if (!str.Visible) continue;

                    ÇokluSeçim_HangiİşlemlereUygun(str.Index, out bool Ödenebilir, out bool Ertelenebilir, out bool ÖdenmediOlarakİşaretlenebilir, out bool İptalEdilebilir);
                    if (Ödenebilir) adet_Ödenebilir++;
                    if (Ertelenebilir) adet_Ertelenebilir++;
                    if (ÖdenmediOlarakİşaretlenebilir) adet_ÖdenmediOlarakİşaretlenebilir++;
                    if (İptalEdilebilir) adet_İptalEdilebilir++;
                }

                ÇokluSeçim_TamÖde.Enabled = adet_Ödenebilir == SeçilenSatırSayısı;
                ÇokluSeçim_Ertele.Enabled = adet_Ertelenebilir == SeçilenSatırSayısı;
                ÇokluSeçim_ÖdenmediOlarakİşaretler.Enabled = adet_ÖdenmediOlarakİşaretlenebilir == SeçilenSatırSayısı;
                ÇokluSeçim_İptalEt.Enabled = adet_İptalEdilebilir == SeçilenSatırSayısı;

                ÇokluSeçim.Enabled = ÇokluSeçim_TamÖde.Enabled || ÇokluSeçim_Ertele.Enabled || ÇokluSeçim_ÖdenmediOlarakİşaretler.Enabled || ÇokluSeçim_İptalEt.Enabled;
                ÇokluSeçim_Ekranı_Açıklama.Text = "Seçilen " + SeçilenSatırSayısı + " ödemeyi";
            }

            if (SeçilenSatırSayısı == 1)
            {
                int SatırNo = Tablo.SelectedRows[0].Index;
                if (SatırNo >= 0 && Tablo.Rows[SatırNo].Tag != null)
                {
                    Banka1.İşyeri_Ödeme_ ödeme = Tablo.Rows[SatırNo].Tag as Banka1.İşyeri_Ödeme_;
                    bool Üyelik_HenüzKaydedilmemişBirÖdeme_Değil = !ödeme.Üyelik_HenüzKaydedilmemişBirÖdeme;

                    if (Cari_döküm_içinde_işlem_yapabilir)
                    {
                        Öde.Enabled = !ödeme.Durumu.ÖdendiMi();
                        Düzenle.Enabled = true;
                    }

                    List<Banka1.İşyeri_Ödeme_> doğrudan, dolaylı;
                    (doğrudan, dolaylı) = ödeme.İlişkiliOlanlarıBul();
                    DoğrudanİlişkiliÖdemeleriListele.Enabled = Üyelik_HenüzKaydedilmemişBirÖdeme_Değil && Ortak_Kullanım_Ödeme != ödeme && doğrudan.Count > 0;
                    DolaylıİlişkiliÖdemeleriListele.Enabled = Üyelik_HenüzKaydedilmemişBirÖdeme_Değil && Ortak_Kullanım_Ödeme != ödeme && dolaylı.Count > 0;
                    SürümleriListele.Enabled = Üyelik_HenüzKaydedilmemişBirÖdeme_Değil && ödeme.İşlemler.Count > 1;
                }
            }
        }

        private void Ekle_Click(object sender, EventArgs e)
        {
            Önyüz.Aç(new GelirGider_Ekle());
        }
        private void DoğrudanİlişkiliÖdemeler_Click(object sender, EventArgs e)
        {
            int SatırNo = Tablo.SelectedCells[0].RowIndex;
            Banka1.İşyeri_Ödeme_ ödeme = Tablo.Rows[SatırNo].Tag as Banka1.İşyeri_Ödeme_;

            Cari_Döküm cd = new Cari_Döküm(AçılışTürü_.İlişkiliOlanlarıListele, ödeme);
            Önyüz.Aç(cd);
        }
        private void DolaylıİlişkiliÖdemeleriListele_Click(object sender, EventArgs e)
        {
            int SatırNo = Tablo.SelectedCells[0].RowIndex;
            Banka1.İşyeri_Ödeme_ ödeme = Tablo.Rows[SatırNo].Tag as Banka1.İşyeri_Ödeme_;

            Cari_Döküm cd = new Cari_Döküm(AçılışTürü_.DolaylıYoldanİlişkiliOlanlarıListele, ödeme);
            Önyüz.Aç(cd);
        }
        private void SürümleriListele_Click(object sender, EventArgs e)
        {
            int SatırNo = Tablo.SelectedCells[0].RowIndex;
            Banka1.İşyeri_Ödeme_ ödeme = Tablo.Rows[SatırNo].Tag as Banka1.İşyeri_Ödeme_;

            Cari_Döküm cd = new Cari_Döküm(AçılışTürü_.SürümleriListele, ödeme);
            Önyüz.Aç(cd);
        }
        private void Yazdır_Click(object sender, EventArgs e)
        {
            string dsy = Ortak.Klasör_Gecici + DateTime.Now.Yazıya(ArgeMup.HazirKod.Dönüştürme.D_TarihSaat.Şablon_DosyaAdı2) + ".pdf";
            Ayarlar_Yazdırma yzdrm = new Ayarlar_Yazdırma();
            yzdrm.Yazdır(Tablo, dsy);
            yzdrm.Dispose();

            Ortak.Çalıştır.UygulamayaİşletimSistemiKararVersin(dsy);
        }
        #region Ödeme
        private void Öde_Click(object sender, EventArgs e)
        {
            if (Tablo.SelectedCells.Count == 0) return;
            int SatırNo = Tablo.SelectedCells[0].RowIndex;
            Banka1.İşyeri_Ödeme_ ödeme = Tablo.Rows[SatırNo].Tag as Banka1.İşyeri_Ödeme_;

            Öde_AvansÖdemesi_Onay.Checked = false;
            bool avans_ödeme_ekranını_kapat = true;
            if (ödeme.Tipi == Banka1.İşyeri_Ödeme_İşlem_.Tipi_.MaaşÖdemesi)
            {
                List<Banka1.İşyeri_Ödeme_> avans_ödemeleri = Ortak.Banka.Seçilenİşyeri.Muhatap_Ödenmemiş_AvansÖdemeleri(ödeme.MuhatapAdı);
                if (avans_ödemeleri.Count > 0)
                {
                    avans_ödeme_ekranını_kapat = false;

                    Ödeme_Ekranı_AvansÖdemesi.Tag = avans_ödemeleri;
                    Ödeme_Ekranı_AvansÖdemesi.Visible = true;
                    Ödeme_Ekranı_2.Left = Ödeme_Ekranı_AvansÖdemesi.Left - 10 - Ödeme_Ekranı_2.Width;

                    double toplam_avans = 0;
                    avans_ödemeleri.ForEach(x => toplam_avans += x.Miktarı);
                    Öde_AvansÖdemesi_Onay.Text = "Avans ödemesi al ( Toplam : " + Banka_Ortak.Yazdır_Ücret(toplam_avans, Banka1.İşyeri_Ödeme_.ParaBirimi_.TürkLirası) + " )";

                    Öde_AvansÖdemesi_Miktar.Value = 0;
                    Öde_AvansÖdemesi_Miktar.Maximum = (decimal)Hesapla.EnKüçük(ödeme.Miktarı, toplam_avans);
                }
            }
            if (avans_ödeme_ekranını_kapat)
            {
                Ödeme_Ekranı_AvansÖdemesi.Visible = false;
                Ödeme_Ekranı_2.Left = (Ödeme_Ekranı.Width - Ödeme_Ekranı_2.Width) / 2;
            }

            Öde_TamÖdeme.Text = "Tam Ödeme ( " + ödeme.Yazdır_Miktarı() + " )";
            Öde_TamÖdeme.Tag = ödeme;
            Öde_KısmiÖdeme.Text = "Kısmi Ödeme";
            Öde_KısmiÖdeme_ParaBirimi.SelectedIndex = ((int)ödeme.ParaBirimi) - 1;
            Öde_MuhatapVeGrupAdı.Text = ödeme.MuhatapGrubuAdı + "          " + ödeme.MuhatapAdı;
            Öde_Notlar.Text = ödeme.Notlar;
            Öde_KısmiÖdeme_Miktar.Value = 0;
            Öde_KalanÖdemeTarihi.Value = ödeme.ÖdemeninYapılacağıTarih.ToDateTime(new TimeOnly());

            Öde_TamÖdeme.Checked = true;
            Ödeme_Ekranı.Visible = true;
            Sonuçlar_Ekranı.Visible = false;

            if (!avans_ödeme_ekranını_kapat) Öde_AvansÖdemesi_Miktar_ValueChanged(null, null);
        }
        private void Öde_TamÖdeme_CheckedChanged(object sender, EventArgs e)
        {
            Öde_Kaydet.Enabled = Öde_TamÖdeme.Checked || (Öde_KısmiÖdeme.Checked && Öde_KısmiÖdeme_Miktar.Value > 0 && Öde_KısmiÖdeme_Miktar.BackColor == Color.White);
            Öde_KısmiÖdeme_Ekran.Enabled = Öde_KısmiÖdeme.Checked;
        }
        private void Öde_KısmiÖdeme_Miktar_ParaBirimi_Değişti(object sender, EventArgs e)
        {
            Öde_Kaydet.Enabled = false;
            if (Öde_KısmiÖdeme_Miktar.Value == 0) return;

            Banka1.İşyeri_Ödeme_ ödeme = Öde_TamÖdeme.Tag as Banka1.İşyeri_Ödeme_;
            Banka1.İşyeri_Ödeme_.ParaBirimi_ Seçilen_parabirimi = (Banka1.İşyeri_Ödeme_.ParaBirimi_)(Öde_KısmiÖdeme_ParaBirimi.SelectedIndex + 1);

            double KısmiÖdemeMiktarı_TürkLirasıOlarak = Ortak.ParaBirimi_Dönüştür((double)Öde_KısmiÖdeme_Miktar.Value, Seçilen_parabirimi, Banka1.İşyeri_Ödeme_.ParaBirimi_.TürkLirası);
            double TamÖdemeMiktarı_TürkLirasıOlarak = Ortak.ParaBirimi_Dönüştür(ödeme.Miktarı, ödeme.ParaBirimi, Banka1.İşyeri_Ödeme_.ParaBirimi_.TürkLirası);
            double KalanÖdemeMiktarı_TürkLirasıOlarak = TamÖdemeMiktarı_TürkLirasıOlarak - KısmiÖdemeMiktarı_TürkLirasıOlarak;
            double KalanÖdemeMiktarı_ÖdemeParaBiriminde = Ortak.ParaBirimi_Dönüştür(KalanÖdemeMiktarı_TürkLirasıOlarak, Banka1.İşyeri_Ödeme_.ParaBirimi_.TürkLirası, ödeme.ParaBirimi);
            if (KalanÖdemeMiktarı_ÖdemeParaBiriminde > 0)
            {
                Öde_KısmiÖdeme_Miktar.BackColor = Color.White;
                Öde_Kaydet.Enabled = true;
            }
            else
            {
                Öde_KısmiÖdeme_Miktar.BackColor = Ortak.Renk_Kırmızı;
            }
            Öde_KısmiÖdeme.Text = "Kısmi Ödeme ( Kalan : " + Banka_Ortak.Yazdır_Ücret(KalanÖdemeMiktarı_ÖdemeParaBiriminde, ödeme.ParaBirimi) + " )";
        }
        private void Öde_Geri_Click(object sender, EventArgs e)
        {
            Öde.Enabled = false;
            Sonuçlar_Ekranı.Visible = true;
            Ödeme_Ekranı.Visible = false;
        }
        private void Öde_Kaydet_Click(object sender, EventArgs e)
        {
            Öde_Notlar.Text = Öde_Notlar.Text.Trim();
            if (Öde_Notlar.Text.BoşMu())
            {
                Öde_Notlar.Text = null;
                MessageBox.Show("Notlar kısmını doldurunuz", "Ödeme Ekranı");
                Öde_Notlar.Focus();
                return;
            }

            DialogResult Dr = MessageBox.Show("Ödeme işlemi kaydedilecek. İşleme devam etmek istiyor musunuz?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            if (Dr == DialogResult.No) return;

            Banka1.İşyeri_Ödeme_ ödeme = Öde_TamÖdeme.Tag as Banka1.İşyeri_Ödeme_;
            if (ödeme.Tipi == Banka1.İşyeri_Ödeme_İşlem_.Tipi_.AvansÖdemesi)
            {
                Dr = MessageBox.Show("Seçtiğiniz avans ödemesi, maaş ödemesi aşamasında uygulama tarafından ödenecektir, elle ödeme yapılması gerekli değildir." + Environment.NewLine + Environment.NewLine +
                    "Yinede işleme devam etmek istiyor musunuz?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (Dr == DialogResult.No) return;
            }

            if (Öde_AvansÖdemesi_Onay.Checked && Öde_AvansÖdemesi_Miktar.Value > 0)
            {
                bool en_az_1_kayıt_yapıldı = false;
                DateTime KayıtTarihi = DateTime.Now;
                foreach (DataGridViewRow satır in Öde_AvansÖdemesi_Tablo.Rows)
                {
                    double satır_miktar = (double)satır.Cells[Öde_AvansÖdemesi_Tablo_Mikar.Index].Tag;
                    Banka1.İşyeri_Ödeme_ satır_ödeme = satır.Tag as Banka1.İşyeri_Ödeme_;

                    switch (satır_ödeme.Tipi)
                    {
                        case Banka1.İşyeri_Ödeme_İşlem_.Tipi_.MaaşÖdemesi:
                            ödeme.YeniİşlemEkle(satır_ödeme.Tipi, Banka1.İşyeri_Ödeme_İşlem_.Durum_.TamÖdendi, satır_ödeme.Miktarı, Öde_Notlar.Text + Environment.NewLine + Banka_Ortak.Yazdır_Ücret(satır_ödeme.Miktarı - satır_miktar, satır_ödeme.ParaBirimi) + " avans ödemesi alındı" + Environment.NewLine + "Maaş olarak " + Banka_Ortak.Yazdır_Ücret(satır_miktar, satır_ödeme.ParaBirimi) + " ödendi", null, KayıtTarihi);
                            en_az_1_kayıt_yapıldı = true;
                            break;

                        case Banka1.İşyeri_Ödeme_İşlem_.Tipi_.AvansÖdemesi:
                            if (satır_miktar == 0) continue;

                            satır_ödeme.Öde(satır_miktar, satır_ödeme.ParaBirimi, satır_ödeme.Notlar, null, KayıtTarihi);
                            en_az_1_kayıt_yapıldı = true;
                            break;

                        default: continue;
                    }
                }

                if (!en_az_1_kayıt_yapıldı) return;
            }
            else
            {
                double ÖdenilenMiktar;
                Banka1.İşyeri_Ödeme_.ParaBirimi_ ÖdenilenParaBirimi;
                DateTime? KalanÖdemeninYapılacağıTarih;

                if (Öde_TamÖdeme.Checked)
                {
                    ÖdenilenMiktar = ödeme.Miktarı;
                    ÖdenilenParaBirimi = ödeme.ParaBirimi;
                    KalanÖdemeninYapılacağıTarih = null;
                }
                else if (Öde_KısmiÖdeme.Checked)
                {
                    ÖdenilenMiktar = (double)Öde_KısmiÖdeme_Miktar.Value;
                    ÖdenilenParaBirimi = (Banka1.İşyeri_Ödeme_.ParaBirimi_)(Öde_KısmiÖdeme_ParaBirimi.SelectedIndex + 1);
                    KalanÖdemeninYapılacağıTarih = Öde_KalanÖdemeTarihi.Value;
                }
                else return;

                ödeme.Öde(ÖdenilenMiktar, ÖdenilenParaBirimi, Öde_Notlar.Text, KalanÖdemeninYapılacağıTarih);
            }

            Banka_Ortak.DeğişiklikleriKaydet();
            Sorgula_Click(null, null);
        }
        #region Avans ödemesi
        private void Öde_AvansÖdemesi_Onay_CheckedChanged(object sender, EventArgs e)
        {
            Öde_TamÖdeme.Checked = Öde_AvansÖdemesi_Onay.Checked;
            Öde_TamÖdeme.Checked = false;
            Öde_TamÖdeme.Enabled = !Öde_AvansÖdemesi_Onay.Checked;
            Öde_KısmiÖdeme.Enabled = !Öde_AvansÖdemesi_Onay.Checked;

            Öde_AvansÖdemesi_Miktar_ValueChanged(null, null);
        }
        private void Öde_AvansÖdemesi_Miktar_ValueChanged(object sender, EventArgs e)
        {
            Banka1.İşyeri_Ödeme_ maaş_ödemesi = Öde_TamÖdeme.Tag as Banka1.İşyeri_Ödeme_;
            List<Banka1.İşyeri_Ödeme_> avans_ödemeleri = Ödeme_Ekranı_AvansÖdemesi.Tag as List<Banka1.İşyeri_Ödeme_>;

            double ödenecek_avans = Öde_AvansÖdemesi_Onay.Checked ? (double)Öde_AvansÖdemesi_Miktar.Value : 0;
            double ödenecek_maaş = maaş_ödemesi.Miktarı - ödenecek_avans;

            Öde_AvansÖdemesi_Tablo.Rows.Clear(); ;
            _Ekle_(maaş_ödemesi, ödenecek_maaş, null);

            foreach (var ödeme in avans_ödemeleri)
            {
                if (ödenecek_avans > 0)
                {
                    if (ödeme.Miktarı <= ödenecek_avans)
                    {
                        ödenecek_avans -= ödeme.Miktarı;

                        _Ekle_(ödeme, ödeme.Miktarı, ödeme.Taksit);
                    }
                    else
                    {
                        _Ekle_(ödeme, ödenecek_avans, ödeme.Taksit);

                        ödenecek_avans = 0;
                    }
                }
                else _Ekle_(ödeme, 0, ödeme.Taksit);
            }

            Öde_AvansÖdemesi_Tablo.ClearSelection();
            Öde_Kaydet.Enabled = Öde_AvansÖdemesi_Onay.Checked && Öde_AvansÖdemesi_Miktar.Value > 0;

            void _Ekle_(Banka1.İşyeri_Ödeme_ _ödeme_, double _miktar_, Banka1.İşyeri_Ödeme_Taksit_ Taksit)
            {
                int satır_no = Öde_AvansÖdemesi_Tablo.Rows.Add(new object[] {
                    _ödeme_.Tipi.Yazdır() + (Taksit == null ? null : ", taksit " + Taksit.Yazdır()) + Environment.NewLine + _ödeme_.Notlar,
                    Banka_Ortak.Yazdır_Ücret(_ödeme_.Miktarı, maaş_ödemesi.ParaBirimi),
                    Banka_Ortak.Yazdır_Ücret(_miktar_, maaş_ödemesi.ParaBirimi)
                });

                Öde_AvansÖdemesi_Tablo.Rows[satır_no].Tag = _ödeme_;
                Öde_AvansÖdemesi_Tablo.Rows[satır_no].Cells[Öde_AvansÖdemesi_Tablo_Mikar.Index].Tag = _miktar_;
                Öde_AvansÖdemesi_Tablo.Rows[satır_no].Cells[Öde_AvansÖdemesi_Tablo_Ödenecek.Index].Style.BackColor = Cari_Döküm.DurumRengi(_miktar_ == 0 ? Banka1.İşyeri_Ödeme_İşlem_.Durum_.Ödenmedi : _miktar_ == _ödeme_.Miktarı ? Banka1.İşyeri_Ödeme_İşlem_.Durum_.TamÖdendi : Banka1.İşyeri_Ödeme_İşlem_.Durum_.KısmenÖdendi);
            }
        }
        #endregion
        #endregion
        #region Düzenleme
        private void Düzenle_Click(object sender, EventArgs e)
        {
            if (Tablo.SelectedCells.Count == 0) return;
            int SatırNo = Tablo.SelectedCells[0].RowIndex;
            Banka1.İşyeri_Ödeme_ ödeme = Tablo.Rows[SatırNo].Tag as Banka1.İşyeri_Ödeme_;

            List<Banka1.İşyeri_Ödeme_> doğrudan, dolaylı;
            (doğrudan, dolaylı) = ödeme.İlişkiliOlanlarıBul();
            if (doğrudan.Count + dolaylı.Count > 0)
            {
                DialogResult Dr2 = MessageBox.Show(ödeme.MuhatapGrubuAdı + " -> " + ödeme.MuhatapAdı + " -> " + ödeme.Tipi.Yazdır() + " için " + (doğrudan.Count + dolaylı.Count) + " adet daha ilişkili ödeme bulundu." + Environment.NewLine + Environment.NewLine +
                    "Seçili ödemenin haricindeki ödemelerde bir değişiklik YAPILMAYACAK." + Environment.NewLine + Environment.NewLine +
                    "Bu durum ileride bir TUTARSIZLIĞA sebep olabilir." + Environment.NewLine + Environment.NewLine +
                    "İlgili ödemeyi seçip, \"Dolaylı olarak ilişkili ödemeler\" tuşuna basıp durumu değerlendirmeniz tavsiye edilir" + Environment.NewLine + Environment.NewLine +
                    "İşleme devam etmek istiyor musunuz?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (Dr2 == DialogResult.No) return;
            }

            Düzenle_MuhatapVeGrupAdı.Text = ödeme.MuhatapGrubuAdı + "          " + ödeme.MuhatapAdı;
            Düzenle_Miktar.Value = (decimal)ödeme.Miktarı;
            Düzenle_ParaBirimi.SelectedIndex = ((int)ödeme.ParaBirimi) - 1;
            Düzenle_ÖdemeninYapılacağıTarih.Value = ödeme.ÖdemeninYapılacağıTarih.ToDateTime(new TimeOnly());
            Düzenle_Tip.SelectedIndex = (int)ödeme.Tipi - 1;
            Düzenle_Durum.SelectedIndex = (int)ödeme.Durumu - 1;
            Düzenle_Notlar.Text = ödeme.Notlar;
            Düzenle_Kaydet.Tag = ödeme;

            Düzenleme_Ekranı.Visible = true;
            Sonuçlar_Ekranı.Visible = false;
        }
        private void Düzenle_Geri_Click(object sender, EventArgs e)
        {
            Düzenle.Enabled = false;
            Sonuçlar_Ekranı.Visible = true;
            Düzenleme_Ekranı.Visible = false;
        }
        private void Düzenle_Kaydet_Click(object sender, EventArgs e)
        {
            Düzenle_Notlar.Text = Düzenle_Notlar.Text.Trim();
            if (Düzenle_Notlar.Text.BoşMu())
            {
                Düzenle_Notlar.Text = null;
                MessageBox.Show("Notlar kısmını doldurunuz", "Ödeme Ekranı");
                Düzenle_Notlar.Focus();
                return;
            }

            DialogResult Dr = MessageBox.Show("Değişiklikler kaydedilecek. İşleme devam etmek istiyor musunuz?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            if (Dr == DialogResult.No) return;

            Banka1.İşyeri_Ödeme_ ödeme = Düzenle_Kaydet.Tag as Banka1.İşyeri_Ödeme_;
            Banka1.İşyeri_Ödeme_İşlem_.Tipi_ tipi = (Banka1.İşyeri_Ödeme_İşlem_.Tipi_)Düzenle_Tip.SelectedIndex + 1;
            Banka1.İşyeri_Ödeme_İşlem_.Durum_ durumu = (Banka1.İşyeri_Ödeme_İşlem_.Durum_)Düzenle_Durum.SelectedIndex + 1;

            if (ödeme.Üyelik_HenüzKaydedilmemişBirÖdeme)
            {
                ödeme.Öde(ödeme.Miktarı, ödeme.ParaBirimi, Düzenle_Notlar.Text, Düzenle_ÖdemeninYapılacağıTarih.Value);

                if (ödeme.Tipi != tipi || ödeme.Durumu != durumu || ödeme.Miktarı != (double)Düzenle_Miktar.Value) ödeme.YeniİşlemEkle(tipi, durumu, (double)Düzenle_Miktar.Value);
            }
            else ödeme.YeniİşlemEkle(tipi, durumu, (double)Düzenle_Miktar.Value, Düzenle_Notlar.Text, DateOnly.FromDateTime(Düzenle_ÖdemeninYapılacağıTarih.Value));

            Banka_Ortak.DeğişiklikleriKaydet();
            Sorgula_Click(null, null);
        }
        #endregion
        #region Çoklu Seçim
        void ÇokluSeçim_HangiİşlemlereUygun(int SatırNo, out bool Ödenebilir, out bool Ertelenebilir, out bool ÖdenmediOlarakİşaretlenebilir, out bool İptalEdilebilir)
        {
            Ödenebilir = false;
            Ertelenebilir = false;
            ÖdenmediOlarakİşaretlenebilir = false;
            İptalEdilebilir = false;

            Banka1.İşyeri_Ödeme_ ödeme = Tablo.Rows[SatırNo].Tag as Banka1.İşyeri_Ödeme_;
            if (ödeme == null || ödeme.Tipi == Banka1.İşyeri_Ödeme_İşlem_.Tipi_.KontrolNoktası) return;

            Ödenebilir = !ödeme.Durumu.ÖdendiMi();
            Ertelenebilir = Ödenebilir && !ödeme.Üyelik_HenüzKaydedilmemişBirÖdeme;
            ÖdenmediOlarakİşaretlenebilir = !Ödenebilir;
            İptalEdilebilir = ödeme.Durumu != Banka1.İşyeri_Ödeme_İşlem_.Durum_.İptalEdildi;
            Ödenebilir &= !(ödeme.Tipi == Banka1.İşyeri_Ödeme_İşlem_.Tipi_.AvansÖdemesi || ödeme.Tipi == Banka1.İşyeri_Ödeme_İşlem_.Tipi_.MaaşÖdemesi);
        }
        private void ÇokluSeçim_Click(object sender, EventArgs e)
        {
            ÇokluSeçim_TamÖde.Checked = false;

            ÇokluSeçim_Ertele.Checked = false;
            DateTime t = DateTime.Now.AddDays(7);
            ÇokluSeçim_Ertele_TamTarih_Tarih.Value = new DateTime(t.Year, t.Month, t.Day);
            ÇokluSeçim_Ertele_SüreKadar_Adet.Value = 1;
            ÇokluSeçim_Ertele_SüreKadar_Dönem.SelectedIndex = 1;//Hafta

            ÇokluSeçim_ÖdenmediOlarakİşaretler.Checked = false;
            ÇokluSeçim_İptalEt.Checked = false;
            ÇokluSeçim_Kaydet.Enabled = false;

            ÇokluSeçim_Ekranı.Visible = true;
            Sonuçlar_Ekranı.Visible = false;
        }
        private void ÇokluSeçim_Geri_Click(object sender, EventArgs e)
        {
            Sonuçlar_Ekranı.Visible = true;
            ÇokluSeçim_Ekranı.Visible = false;
        }
        private void ÇokluSeçim_Ertele_x_Onay_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton tıklanan = sender as RadioButton;
            RadioButton diğeri = tıklanan.Tag as RadioButton;
            diğeri.Checked = !tıklanan.Checked;
        }
        private void ÇokluSeçim_Ertele_SüreKadar_x_AyarDeğişti(object sender, EventArgs e)
        {
            ÇokluSeçim_Ertele_SüreKadar_Onay.Checked = true;
        }
        private void ÇokluSeçim_Ertele_TamTarih_x_AyarDeğişti(object sender, EventArgs e)
        {
            ÇokluSeçim_Ertele_TamTarih_Onay.Checked = true;
        }
        private void ÇokluSeçim_Ertele_CheckedChanged(object sender, EventArgs e)
        {
            ÇokluSeçim_Ertele_SüreKadar.Enabled = ÇokluSeçim_Ertele.Checked;
            ÇokluSeçim_Ertele_TamTarih.Enabled = ÇokluSeçim_Ertele.Checked;

            ÇokluSeçim_AyarDeğişti(null, null);
        }
        private void ÇokluSeçim_AyarDeğişti(object sender, EventArgs e)
        {
            ÇokluSeçim_Kaydet.Enabled = true;
        }
        private void ÇokluSeçim_Kaydet_Click(object sender, EventArgs e)
        {
            DialogResult Dr = MessageBox.Show("Seçilen " + Tablo.SelectedRows.Count + " adet ödeme için işleme devam etmek istiyor musunuz?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            if (Dr == DialogResult.No) return;

            foreach (DataGridViewRow satır in Tablo.SelectedRows)
            {
                Banka1.İşyeri_Ödeme_ ödeme = satır.Tag as Banka1.İşyeri_Ödeme_;

                List<Banka1.İşyeri_Ödeme_> doğrudan, dolaylı;
                (doğrudan, dolaylı) = ödeme.İlişkiliOlanlarıBul();
                if (doğrudan.Count + dolaylı.Count > 0)
                {
                    DialogResult Dr2 = MessageBox.Show(ödeme.MuhatapGrubuAdı + " -> " + ödeme.MuhatapAdı + " -> " + ödeme.Tipi.Yazdır() + " için " + (doğrudan.Count + dolaylı.Count) + " adet daha ilişkili ödeme bulundu." + Environment.NewLine + Environment.NewLine +
                    "Seçili ödemenin haricindeki ödemelerde bir değişiklik YAPILMAYACAK." + Environment.NewLine + Environment.NewLine +
                    "Bu durum ileride bir TUTARSIZLIĞA sebep olabilir." + Environment.NewLine + Environment.NewLine +
                    "İlgili ödemeyi seçip, \"Dolaylı olarak ilişkili ödemeler\" tuşuna basıp durumu değerlendirmeniz tavsiye edilir" + Environment.NewLine + Environment.NewLine +
                    "İşleme devam etmek istiyor musunuz?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                    if (Dr2 == DialogResult.No) continue;
                }

                if (ÇokluSeçim_TamÖde.Checked)
                {
                    ödeme.Öde(ödeme.Miktarı, ödeme.ParaBirimi);
                }
                else
                {
                    if (ÇokluSeçim_Ertele.Checked)
                    {
                        DateOnly ÖdemeninYapılacağıTarih = ÇokluSeçim_Ertele_SüreKadar_Onay.Checked ? Banka_Ortak.SonrakiTarihiHesapla(ödeme.ÖdemeninYapılacağıTarih, (Banka1.Muhatap_Üyelik_.Dönem_)(ÇokluSeçim_Ertele_SüreKadar_Dönem.SelectedIndex + 1), (int)ÇokluSeçim_Ertele_SüreKadar_Adet.Value) : DateOnly.FromDateTime(ÇokluSeçim_Ertele_TamTarih_Tarih.Value);
                        ödeme.YeniİşlemEkle(ödeme.Tipi, ödeme.Durumu, ödeme.Miktarı, null, ÖdemeninYapılacağıTarih);
                    }
                    else if (ÇokluSeçim_ÖdenmediOlarakİşaretler.Checked)
                    {
                        ödeme.YeniİşlemEkle(ödeme.Tipi, Banka1.İşyeri_Ödeme_İşlem_.Durum_.Ödenmedi, ödeme.Miktarı);
                    }
                    else if (ÇokluSeçim_İptalEt.Checked)
                    {
                        ödeme.YeniİşlemEkle(ödeme.Tipi, Banka1.İşyeri_Ödeme_İşlem_.Durum_.İptalEdildi, ödeme.Miktarı);
                    }
                }
            }

            Banka_Ortak.DeğişiklikleriKaydet();
            Sorgula_Click(null, null);
        }
        #endregion
        #region Kayıt Noktası Ekleme
        private void KontrolNoktasıEkle_Click(object sender, EventArgs e)
        {
            KontrolNoktası_Tarihi.Value = DateTime.Now;
            KontrolNoktası_Notları.Text = null;

            KontrolNoktası_Ekranı.Visible = true;
            Sonuçlar_Ekranı.Visible = false;
        }
        private void KontrolNoktası_Geri_Click(object sender, EventArgs e)
        {
            Sonuçlar_Ekranı.Visible = true;
            KontrolNoktası_Ekranı.Visible = false;
        }
        private void KontrolNoktası_Kaydet_Click(object sender, EventArgs e)
        {
            KontrolNoktası_Notları.Text = KontrolNoktası_Notları.Text.Trim();
            DialogResult Dr = MessageBox.Show("Kontrol noktası oluşturulacak. İşleme devam etmek istiyor musunuz?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            if (Dr == DialogResult.No) return;

            if (KontrolNoktası_Notları.Text.DoluMu()) KontrolNoktası_Notları.Text += Environment.NewLine + Environment.NewLine;
            KontrolNoktası_Notları.Text += Banka_Ortak.Yazdır_Özet(null, null, true, true);

            Ortak.Banka.Seçilenİşyeri.KontrolNoktasıEkle(KontrolNoktası_Tarihi.Value, KontrolNoktası_Notları.Text);
            Banka_Ortak.DeğişiklikleriKaydet();
            Sorgula_Click(null, null);
        }
        #endregion
    }
}
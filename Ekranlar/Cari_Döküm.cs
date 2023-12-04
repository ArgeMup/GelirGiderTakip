using ArgeMup.HazirKod.Ekİşlemler;
using ArgeMup.HazirKod.Ekranlar;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Gelir_Gider_Takip.Ekranlar
{
    public partial class Cari_Döküm : Form
    {
        public enum AçılışTürü_ { Normal, İlişkiliOlanlarıListele, SürümleriListele, Gizli };

        List<string> Kapsam_Grup = null, Kapsam_Muhatap = null;
        Banka1.İşyeri_Ödeme_ Ödeme = null;
        AçılışTürü_ AçılışTürü;
        ArgeMup.HazirKod.Ekranlar.ListeKutusu.Ayarlar_ ListeKutusu_Ayarlar;
        ListeKutusu Sorgula_Şablonlar;
        Cari_Döküm_Şablon_ Şablon;

        public Cari_Döküm(AçılışTürü_ AçılışTürü = AçılışTürü_.Normal, Banka1.İşyeri_Ödeme_ Ödeme = null)
        {
            InitializeComponent();
            if (AçılışTürü == AçılışTürü_.Gizli) Opacity = 0;

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

            this.Ödeme = Ödeme;
            this.AçılışTürü = AçılışTürü;

            ListeKutusu_Ayarlar = new ArgeMup.HazirKod.Ekranlar.ListeKutusu.Ayarlar_() { ÇokluSeçim = ListeKutusu.Ayarlar_.ÇokluSeçim_.SolFareTuşuİle };
            ListeKutusu_Ayarlar.TümTuşlarıKapat();

            if (!Ortak.Banka.İzinliMi(Banka1.Ayarlar_Kullanıcılar_İzin.Cari_döküm_içinde_işlem_yapabilir))
            {
                Öde.Visible = false;
                Düzenle.Visible = false;
                KontrolNoktasıEkle.Visible = false;
                Yazdır.Visible = false;
            }

            Ayraç_Filtre_TabloSonuç.SplitterDistance = Height / 5;

            Sorgula_MuhatapGrubu.Başlat(Ortak.Banka.Seçilenİşyeri.MuhatapGrubu_Listele(true), Ortak.Banka.Seçilenİşyeri.MuhatapGrubu_Listele(), "Muhatap Grupları", ListeKutusu_Ayarlar);

            ListeKutusu.Ayarlar_ ListeKutusu_Ayarlar_Şablon = new ListeKutusu.Ayarlar_(
                Eklenebilir: Ortak.Banka.İzinliMi(Banka1.Ayarlar_Kullanıcılar_İzin.Ayarları_değiştirebilir),
                Silinebilir: Ortak.Banka.İzinliMi(Banka1.Ayarlar_Kullanıcılar_İzin.Ayarları_değiştirebilir),
                ElemanKonumu: Ortak.Banka.İzinliMi(Banka1.Ayarlar_Kullanıcılar_İzin.Ayarları_değiştirebilir) ? ListeKutusu.Ayarlar_.ElemanKonumu_.Değiştirilebilir : ListeKutusu.Ayarlar_.ElemanKonumu_.OlduğuGibi,
                AdıDeğiştirilebilir: false, Gizlenebilir: false);
            Sorgula_Şablonlar.Başlat(null, Ortak.Banka.Ayarlar.CariDökümŞablonlar.Keys.ToList(), "Şablonlar", ListeKutusu_Ayarlar_Şablon);

            Şablon = new Cari_Döküm_Şablon_(Sorgula_MuhatapGrubu, Sorgula_Muhatap);
            SorgulamaDetayları.SelectedObject = Şablon;
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
                    Şablon.Zamanlama_Türü = Cari_Döküm_Şablon_.Zamanlama_Türü_.Kayıt_tarihi;
                    Şablon.Zamanlama_Başlangıç = Ödeme.İlkKayıtTarihi;
                    Şablon.Zamanlama_Bitiş = Ödeme.İlkKayıtTarihi;
                    Sorgula_MuhatapGrubu.SeçilenEleman_Adı = Ödeme.MuhatapGrubuAdı;
                    Sorgula_Muhatap.SeçilenEleman_Adı = Ödeme.MuhatapAdı;
                    Sorgula_Click(null, null);
                    break;

                case AçılışTürü_.SürümleriListele:
                    int sutun_sayısı = Tablo.ColumnCount;
                    List<DataGridViewRow> dizi = new List<DataGridViewRow>();
                    Tablo_SonİşlemTarihi.Visible = false;
                    string Son_Not = null;

                    foreach (KeyValuePair<DateTime, Banka1.İşyeri_Ödeme_İşlem_> işlem in Ödeme.İşlemler)
                    {
                        if (işlem.Value.Notlar.DoluMu()) Son_Not = işlem.Value.Notlar;

                        bool üyelik_olarak_göster = Ödeme.Üyelik_KayıtTarihi != null;
                        bool gelir_olarak_göster = Ödeme.Tipi.GelirMi();

                        object[] dizin = new object[sutun_sayısı];
                        dizin[Tablo_MuhatapGrubu.Index] = Ödeme.MuhatapGrubuAdı;
                        dizin[Tablo_Muhatap.Index] = Ödeme.MuhatapAdı;
                        dizin[Tablo_ÖdemeTarihi.Index] = işlem.Value.ÖdemeninYapılacağıTarih;
                        dizin[Tablo_Tip.Index] = işlem.Value.Tipi.Yazdır();
                        dizin[Tablo_Durum.Index] = işlem.Value.Durumu.Yazdır();
                        dizin[Tablo_Notlar.Index] = Son_Not;
                        dizin[Tablo_Miktar.Index] = Banka_Ortak.Yazdır_Ücret(işlem.Value.Miktarı, Ödeme.ParaBirimi);
                        dizin[Tablo_Taksit.Index] = Ödeme.Taksit.Yazdır();
                        dizin[Tablo_Üyelik.Index] = üyelik_olarak_göster;
                        dizin[Tablo_KayıtTarihi.Index] = işlem.Key;
                        dizin[Tablo_KullanıcıAdı.Index] = işlem.Value.GerçekleştirenKullanıcıAdı;

                        DataGridViewRow _1_satır_dizisi_ = new DataGridViewRow();
                        dizi.Add(_1_satır_dizisi_);
                        _1_satır_dizisi_.CreateCells(Tablo, dizin);

                        _1_satır_dizisi_.Cells[Tablo_Miktar.Index].Style.BackColor = gelir_olarak_göster ? Ortak.Renk_Gelir : Ortak.Renk_Gider;
                        _1_satır_dizisi_.Cells[Tablo_Miktar.Index].ToolTipText = gelir_olarak_göster ? "Gelir" : "Gider";
                        if (üyelik_olarak_göster) _1_satır_dizisi_.Cells[Tablo_Üyelik.Index].ToolTipText = Ödeme.Üyelik_KayıtTarihi.Value.Yazıya();
                        _1_satır_dizisi_.Cells[Tablo_Durum.Index].Style.BackColor = DurumRengi(işlem.Value.Durumu);
                        if (Ödeme.GeciktiMi) _1_satır_dizisi_.Cells[Tablo_ÖdemeTarihi.Index].Style.BackColor = Ortak.Renk_Kırmızı;
                    }

                    Tablo.Rows.AddRange(dizi.ToArray());
                    Tablo.ClearSelection();
                    break;
            }
        }
        private void GelirGider_Ekle_KeyPress(object sender, KeyPressEventArgs e)
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
                    Dictionary<string, Cari_Döküm_Şablon_> YeniListe = new Dictionary<string, Cari_Döküm_Şablon_>();
                    foreach (string şablonadı in Sorgula_Şablonlar.Tüm_Elemanlar)
                    {
                        Cari_Döküm_Şablon_ şablon = Ortak.Banka.Ayarlar.CariDökümŞablonlar[şablonadı];
                        YeniListe.Add(şablonadı, şablon);
                    }
                    Ortak.Banka.Ayarlar.CariDökümŞablonlar = YeniListe;
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
        private void Sorgula_Click(object sender, EventArgs e)
        {
            Sorgula.Enabled = false;
            Öde_Geri_Click(null, null);
            Düzenle_Geri_Click(null, null);
            KontrolNoktası_Geri_Click(null, null);

            DateOnly başlangıç_d = DateOnly.FromDateTime(Şablon.Zamanlama_Başlangıç);
            DateOnly bitiş_d = DateOnly.FromDateTime(Şablon.Zamanlama_Bitiş);

            Açıklamalar.Text = null;
            Tablo.Rows.Clear();
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
            const char AltToplamlar_Ayraç = 'é';

            foreach (string yıl in Ortak.Banka.Seçilenİşyeri.Ödemeler_Listele_Yıllar())
            {
                //istenmeyen yılları atla
                int yıll = yıl.TamSayıya();
                if (Şablon.Zamanlama_Türü == Cari_Döküm_Şablon_.Zamanlama_Türü_.Kayıt_tarihi && (yıll < başlangıç_d.Year || yıll > bitiş_d.Year)) continue;

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
                if (AltToplamlar.Keys.Count == 0) AltToplamlar.Add("1" + AltToplamlar_Ayraç + "Geçerli kayıt bulunamadı" + AltToplamlar_Ayraç + "Geçerli kayıt bulunamadı", 0);
                bool MaaşÖdemeleriİçin = Şablon.Diğer_ÇalışanMaaşHesabı != Cari_Döküm_Şablon_.Diğer_ÇalışanMaaşHesabı_.Gerekli_değil;

                foreach (KeyValuePair<string, double> AltToplam in AltToplamlar)
                {
                    object[] dizin = new object[sutun_sayısı];
                    string[] AltToplam_Ad_Dizisi = AltToplam.Key.Split(AltToplamlar_Ayraç);

                    //parabirimi_sayısı + Grup
                    //parabirimi_sayısı + Grup + muhatap
                    if (AltToplam_Ad_Dizisi.Length == 3) dizin[Tablo_Muhatap.Index] = AltToplam_Ad_Dizisi[2];
                    dizin[Tablo_MuhatapGrubu.Index] = AltToplam_Ad_Dizisi[1];
                    dizin[Tablo_Tip.Index] = MaaşÖdemeleriİçin ? "Net ücret" : "Alt toplam";
                    dizin[Tablo_Miktar.Index] = Banka_Ortak.Yazdır_Ücret(MaaşÖdemeleriİçin ? AltToplam.Value * -1 : AltToplam.Value, (Banka1.İşyeri_Ödeme_.ParaBirimi_)AltToplam_Ad_Dizisi[0].TamSayıya());

                    DataGridViewRow _1_satır_dizisi_ = new DataGridViewRow();
                    dizi.Add(_1_satır_dizisi_);
                    _1_satır_dizisi_.CreateCells(Tablo, dizin);
                    _1_satır_dizisi_.Cells[Tablo_Tip.Index].Style.BackColor = Ortak.Renk_Sarı;
                }
            }

            Tablo_Durum.Visible = Şablon.Sütunlar_Durum == Cari_Döküm_Şablon_.Sütunlar_Durum_.Göster;
            Tablo_KayıtTarihi.Visible = Şablon.Sütunlar_KayıtTarihi == Cari_Döküm_Şablon_.Sütunlar_Durum_.Göster;
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
                case Cari_Döküm_Şablon_.Sütunlar_Sırala_Sütun_.Kayıt_Tarihi: sıralama_sütunu = Tablo_KayıtTarihi; break;
                case Cari_Döküm_Şablon_.Sütunlar_Sırala_Sütun_.Kullanıcı: sıralama_sütunu = Tablo_KullanıcıAdı; break;
            }
            Tablo.Sort(sıralama_sütunu, Şablon.Sütunlar_Sırala == Cari_Döküm_Şablon_.Sütunlar_Sırala_.Büyükten_küçüğe ? System.ComponentModel.ListSortDirection.Descending : System.ComponentModel.ListSortDirection.Ascending);
            Tablo.ClearSelection();

            Açıklamalar.Text =
                "Tabloda listelenen işlemler kapsamında" + Environment.NewLine +
                Banka_Ortak.Yazdır_GelirGider(Toplam_Gelir, Toplam_Gider) + Environment.NewLine + Environment.NewLine +
                "Genel kapsamda (Tümü)" + Environment.NewLine +
                Banka_Ortak.Yazdır_GelirGider(Ortak.Banka.Seçilenİşyeri.ToplamGelir, Ortak.Banka.Seçilenİşyeri.ToplamGider) + Environment.NewLine + Environment.NewLine +
                "Genel kapsamda (Sadece Ödenen)" + Environment.NewLine +
                Banka_Ortak.Yazdır_GelirGider(Ortak.Banka.Seçilenİşyeri.ÖdenmişToplamGelir, Ortak.Banka.Seçilenİşyeri.ÖdenmişToplamGider);
            Sorgula.Enabled = true;
            Yazdır.Enabled = dizi.Count() > 0;

            Döviz.KurlarıAl(_GeriBildirimİşlemi_DövizKurları_);
            void _GeriBildirimİşlemi_DövizKurları_(double Avro, double Dolar)
            {
                Ödeme_Ekranı.Invoke(new Action(() =>
                {
                    Açıklamalar.Text = "Avro : " + Banka_Ortak.Yazdır_Ücret(Avro, Banka1.İşyeri_Ödeme_.ParaBirimi_.TürkLirası) + ", Dolar : " + Banka_Ortak.Yazdır_Ücret(Dolar, Banka1.İşyeri_Ödeme_.ParaBirimi_.TürkLirası) +
                        Environment.NewLine + Environment.NewLine + Açıklamalar.Text;
                }));
            }

            void _TabloyaEkle_(Banka1.İşyeri_Ödeme_ _ödeme_)
            {
                KeyValuePair<DateTime, Banka1.İşyeri_Ödeme_İşlem_> ilk_işlem = _ödeme_.İşlemler.First();
                KeyValuePair<DateTime, Banka1.İşyeri_Ödeme_İşlem_> son_işlem = _ödeme_.İşlemler.Last();

                Banka1.İşyeri_Ödeme_İşlem_.Tipi_ tipi = son_işlem.Value.Tipi;
                DataGridViewRow _1_satır_dizisi_;
                if (tipi == Banka1.İşyeri_Ödeme_İşlem_.Tipi_.KontrolNoktası)
                {
                    if (Şablon.Tipi_KontrolNoktası == Cari_Döküm_Şablon_.Sıralama_ÜçSeçenek_.Hariç_tut) return;

                    DateTime trh = Şablon.Zamanlama_Türü == Cari_Döküm_Şablon_.Zamanlama_Türü_.Kayıt_tarihi ? ilk_işlem.Key : ilk_işlem.Value.ÖdemeninYapılacağıTarih.ToDateTime(new TimeOnly());
                    if (trh < Şablon.Zamanlama_Başlangıç || trh > Şablon.Zamanlama_Bitiş) return; //kapsam dışında

                    object[] dizin = new object[sutun_sayısı];
                    dizin[Tablo_Tip.Index] = son_işlem.Value.Tipi.Yazdır();
                    dizin[Tablo_ÖdemeTarihi.Index] = ilk_işlem.Value.ÖdemeninYapılacağıTarih;
                    dizin[Tablo_Notlar.Index] = _ödeme_.Notlar;
                    dizin[Tablo_SonİşlemTarihi.Index] = ilk_işlem.Key;
                    dizin[Tablo_KayıtTarihi.Index] = ilk_işlem.Key;
                    dizin[Tablo_KullanıcıAdı.Index] = son_işlem.Value.GerçekleştirenKullanıcıAdı;

                    _1_satır_dizisi_ = new DataGridViewRow();
                    dizi.Add(_1_satır_dizisi_);
                    _1_satır_dizisi_.CreateCells(Tablo, dizin);

                    for (int i = 0; i < sutun_sayısı; i++)
                    {
                        _1_satır_dizisi_.Cells[i].Style.BackColor = Ortak.Renk_KontrolNoktası;
                    }
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
                        DateTime trh;
                        switch (Şablon.Zamanlama_Türü)
                        {
                            case Cari_Döküm_Şablon_.Zamanlama_Türü_.Ödeme_tarihi: trh = son_işlem.Value.ÖdemeninYapılacağıTarih.ToDateTime(new TimeOnly()); break;
                            case Cari_Döküm_Şablon_.Zamanlama_Türü_.Son_işlem_tarihi: trh = son_işlem.Key; break;
                            case Cari_Döküm_Şablon_.Zamanlama_Türü_.Kayıt_tarihi: trh = ilk_işlem.Key; break;
                            default: trh = DateTime.MinValue; break;
                        }

                        if (trh < Şablon.Zamanlama_Başlangıç || trh > Şablon.Zamanlama_Bitiş) return; //kapsam dışında
                    }

                    bool listele = (Kapsam_Grup == null || Kapsam_Grup.Contains(_ödeme_.MuhatapGrubuAdı)) &&
                                   (Kapsam_Muhatap == null || Kapsam_Muhatap.Contains(_ödeme_.MuhatapAdı));
                    if (!listele) return; //kapsam dışında

                    if (Şablon.Diğer_AltToplam != Cari_Döküm_Şablon_.Diğer_AltToplam_.Gerekli_değil && !iptaledildi_olarak_göster &&
                        !(Şablon.Diğer_ÇalışanMaaşHesabı != Cari_Döküm_Şablon_.Diğer_ÇalışanMaaşHesabı_.Gerekli_değil && tipi == Banka1.İşyeri_Ödeme_İşlem_.Tipi_.AvansVerilmesi))
                    {
                        string anahtar = ((int)_ödeme_.ParaBirimi).Yazıya() + AltToplamlar_Ayraç + _ödeme_.MuhatapGrubuAdı;
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
                            anahtar += AltToplamlar_Ayraç + _ödeme_.MuhatapAdı;
                            miktar = 0;
                            if (AltToplamlar.ContainsKey(anahtar)) miktar = AltToplamlar[anahtar];
                            if (gelir_olarak_göster) miktar += _ödeme_.Miktarı;
                            else miktar -= _ödeme_.Miktarı;
                            AltToplamlar[anahtar] = miktar;
                        }
                    }

                    if (Şablon.Diğer_ÇalışanMaaşHesabı == Cari_Döküm_Şablon_.Diğer_ÇalışanMaaşHesabı_.Basit) return;

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
                    dizin[Tablo_KayıtTarihi.Index] = _ödeme_.İşlemler.First().Key;
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

                return;
            }

            if (TabloİçeriğiArama_Çalışıyor) { TabloİçeriğiArama_KapatmaTalebi = true; return; }

            TabloİçeriğiArama_Çalışıyor = true;
            TabloİçeriğiArama_KapatmaTalebi = false;
            TabloİçeriğiArama_Sayac_Bulundu = 0;
            TabloİçeriğiArama_Tik = Environment.TickCount + 500;
            TabloİçeriğiArama.BackColor = Ortak.Renk_Kırmızı;

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

                    if (string.IsNullOrEmpty(içerik)) Tablo[sutun, satır].Style.BackColor = Color.White;
                    else
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
                            Tablo[sutun, satır].Style.BackColor = Ortak.Renk_Yeşil;
                            bulundu = true;
                        }
                        else Tablo[sutun, satır].Style.BackColor = Color.White;
                    }
                }

                Tablo.Rows[satır].Visible = bulundu;
                if (bulundu) TabloİçeriğiArama_Sayac_Bulundu++;

                if (TabloİçeriğiArama_Tik < Environment.TickCount) { Application.DoEvents(); TabloİçeriğiArama_Tik = Environment.TickCount + 500; }
            }

            if (TabloİçeriğiArama_Sayac_Bulundu == 0) TabloİçeriğiArama_Sayac_Bulundu = -1;

            TabloİçeriğiArama.BackColor = Color.White;
            TabloİçeriğiArama_Çalışıyor = false;
            Tablo.ClearSelection();

            if (TabloİçeriğiArama_KapatmaTalebi) TabloİçeriğiArama_TextChanged(null, null);
            TabloİçeriğiArama_KapatmaTalebi = false;
        }
        private void Tablo_SelectionChanged(object sender, EventArgs e)
        {
            if (Tablo.SelectedRows.Count != 1)
            {
                Öde.Enabled = false;
                Düzenle.Enabled = false;
                İlişkiliÖdemeleriListele.Enabled = false;
                SürümleriListele.Enabled = false;
                return;
            }

            int SatırNo = Tablo.SelectedRows[0].Index;
            if (SatırNo >= 0 && Tablo.Rows[SatırNo].Tag != null)
            {
                Banka1.İşyeri_Ödeme_ ödeme = Tablo.Rows[SatırNo].Tag as Banka1.İşyeri_Ödeme_;
                bool Üyelik_HenüzKaydedilmemişBirÖdeme_Değil = !ödeme.Üyelik_HenüzKaydedilmemişBirÖdeme;

                if (Öde.Visible)
                {
                    Öde.Enabled = !ödeme.Durumu.ÖdendiMi();
                    Düzenle.Enabled = Üyelik_HenüzKaydedilmemişBirÖdeme_Değil;
                }

                İlişkiliÖdemeleriListele.Enabled = Üyelik_HenüzKaydedilmemişBirÖdeme_Değil;
                SürümleriListele.Enabled = Üyelik_HenüzKaydedilmemişBirÖdeme_Değil && ödeme.İşlemler.Count > 1;
            }
        }

        private void İlişkiliÖdemeleriListele_Click(object sender, EventArgs e)
        {
            int SatırNo = Tablo.SelectedCells[0].RowIndex;
            Banka1.İşyeri_Ödeme_ ödeme = Tablo.Rows[SatırNo].Tag as Banka1.İşyeri_Ödeme_;

            Cari_Döküm cd = new Cari_Döküm(AçılışTürü_.İlişkiliOlanlarıListele, ödeme);
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
        }
        private void Öde_TamÖdeme_CheckedChanged(object sender, EventArgs e)
        {
            Öde_Kaydet.Enabled = Öde_TamÖdeme.Checked || Öde_KısmiÖdeme_Miktar.Value > 0;
        }
        private void Öde_KısmiÖdeme_Miktar_ParaBirimi_Değişti(object sender, EventArgs e)
        {
            Öde_KısmiÖdeme.Checked = true;
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
                Öde_KısmiÖdeme_Miktar.Tag = KalanÖdemeMiktarı_ÖdemeParaBiriminde;
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
            string yıl = ödeme.İlkKayıtTarihi.Year.Yazıya(); //ilk kayıt tarihi
            Banka1.İşyeri_BirYıllıkDönem_ BirYıllıkDönem = Ortak.Banka.Seçilenİşyeri.Ödemeler_Listele_BirYıllıkDönem(yıl);

            if (ödeme.Üyelik_HenüzKaydedilmemişBirÖdeme)
            {
                ödeme.Üyelik_HenüzKaydedilmemişBirÖdeme = false;

                Banka1.Muhatap_ muhatap = Ortak.Banka.Seçilenİşyeri.Muhatap_Aç(ödeme.MuhatapGrubuAdı, ödeme.MuhatapAdı);
                muhatap.Üyelik_SisteminTetiklemesiniEngelle(ödeme.Üyelik_KayıtTarihi.Value, ödeme.ÖdemeninYapılacağıTarih);
                muhatap.GelirGider_Ekle(new List<Banka1.İşyeri_Ödeme_>() { ödeme }, BirYıllıkDönem);
            }

            if (Öde_TamÖdeme.Checked)
            {
                BirYıllıkDönem.Güncelle(ödeme, ödeme.Tipi, Banka1.İşyeri_Ödeme_İşlem_.Durum_.TamÖdendi, ödeme.Miktarı, Öde_Notlar.Text);
            }
            else
            {
                //ana ödemenin içine kısmi ödemenin işlenmesi
                double KalanÖdemeMiktarı_ÖdemeParaBiriminde = (double)Öde_KısmiÖdeme_Miktar.Tag;
                double YapılanÖdeme_ÖdemeParaBiriminde = ödeme.Miktarı - KalanÖdemeMiktarı_ÖdemeParaBiriminde;
                BirYıllıkDönem.Güncelle(ödeme, ödeme.Tipi, Banka1.İşyeri_Ödeme_İşlem_.Durum_.KısmenÖdendi, KalanÖdemeMiktarı_ÖdemeParaBiriminde, Öde_Notlar.Text, DateOnly.FromDateTime(Öde_KalanÖdemeTarihi.Value));

                //ana ödemeden bağımsız tam ödeme oluşturulması
                Banka1.İşyeri_Ödeme_.ParaBirimi_ İstenenParBirimi = (Banka1.İşyeri_Ödeme_.ParaBirimi_)(Öde_KısmiÖdeme_ParaBirimi.SelectedIndex + 1);
                string açıklama = ödeme.ParaBirimi != İstenenParBirimi ? "(" + Banka_Ortak.Yazdır_Ücret((double)Öde_KısmiÖdeme_Miktar.Value, İstenenParBirimi) + ")" : null;
                açıklama += (açıklama.DoluMu() ? Environment.NewLine : null) + Öde_Notlar.Text;

                Banka1.Muhatap_ muhatap = Ortak.Banka.Seçilenİşyeri.Muhatap_Aç(ödeme.MuhatapGrubuAdı, ödeme.MuhatapAdı, true);
                muhatap.GelirGider_Ekle(muhatap.GelirGider_Oluştur_KısmiÖdeme(ödeme.Tipi, ödeme.İlkKayıtTarihi, YapılanÖdeme_ÖdemeParaBiriminde, ödeme.ParaBirimi, Öde_KalanÖdemeTarihi.Value, açıklama, ödeme.Taksit, ödeme.Üyelik_KayıtTarihi));
            }

            Banka_Ortak.DeğişiklikleriKaydet();
            Sorgula_Click(null, null);
        }
        #endregion
        #region Düzenleme
        private void Düzenle_Click(object sender, EventArgs e)
        {
            if (Tablo.SelectedCells.Count == 0) return;
            int SatırNo = Tablo.SelectedCells[0].RowIndex;
            Banka1.İşyeri_Ödeme_ ödeme = Tablo.Rows[SatırNo].Tag as Banka1.İşyeri_Ödeme_;

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
            string yıl = ödeme.İlkKayıtTarihi.Year.Yazıya(); //ilk kayıt tarihi
            Banka1.İşyeri_BirYıllıkDönem_ BirYıllıkDönem = Ortak.Banka.Seçilenİşyeri.Ödemeler_Listele_BirYıllıkDönem(yıl);

            BirYıllıkDönem.Güncelle(ödeme, (Banka1.İşyeri_Ödeme_İşlem_.Tipi_)Düzenle_Tip.SelectedIndex + 1, (Banka1.İşyeri_Ödeme_İşlem_.Durum_)Düzenle_Durum.SelectedIndex + 1, (double)Düzenle_Miktar.Value, Düzenle_Notlar.Text, DateOnly.FromDateTime(Düzenle_ÖdemeninYapılacağıTarih.Value));

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
            KontrolNoktası_Notları.Text += "Genel kapsamda (Tümü)" + Environment.NewLine +
                Banka_Ortak.Yazdır_GelirGider(Ortak.Banka.Seçilenİşyeri.ToplamGelir, Ortak.Banka.Seçilenİşyeri.ToplamGider) + Environment.NewLine + Environment.NewLine +
                "Genel kapsamda (Sadece Ödenen)" + Environment.NewLine +
                Banka_Ortak.Yazdır_GelirGider(Ortak.Banka.Seçilenİşyeri.ÖdenmişToplamGelir, Ortak.Banka.Seçilenİşyeri.ÖdenmişToplamGider);

            Ortak.Banka.Seçilenİşyeri.KontrolNoktasıEkle(KontrolNoktası_Tarihi.Value, KontrolNoktası_Notları.Text);
            Banka_Ortak.DeğişiklikleriKaydet();
            Sorgula_Click(null, null);
        }
        #endregion
    }
}
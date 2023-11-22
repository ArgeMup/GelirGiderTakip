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

        public Cari_Döküm(AçılışTürü_ AçılışTürü = AçılışTürü_.Normal, Banka1.İşyeri_Ödeme_ Ödeme = null)
        {
            InitializeComponent();
            if (AçılışTürü == AçılışTürü_.Gizli) Opacity = 0;

            Sorgula_MuhatapGrubu = new ArgeMup.HazirKod.Ekranlar.ListeKutusu();
            splitContainer2.Panel1.Controls.Add(Sorgula_MuhatapGrubu);
            Sorgula_MuhatapGrubu.Dock = DockStyle.Fill;
            Sorgula_MuhatapGrubu.GeriBildirim_İşlemi += Sorgula_MuhatapGrubu_GeriBildirim_İşlemi;

            Sorgula_Muhatap = new ArgeMup.HazirKod.Ekranlar.ListeKutusu();
            splitContainer2.Panel2.Controls.Add(Sorgula_Muhatap);
            Sorgula_Muhatap.Dock = DockStyle.Fill;
            Sorgula_Muhatap.GeriBildirim_İşlemi += Sorgula_Muhatap_GeriBildirim_İşlemi;

            Sorgula_Şablonlar = new ArgeMup.HazirKod.Ekranlar.ListeKutusu();
            panel4.Controls.Add(Sorgula_Şablonlar);
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

            Sorgula_KıstasSeçimi.SelectedIndex = 0;
            Sorgula_TarihAralığı.SelectedIndex = 0;
        }
        public void Cari_Döküm_Load(object sender, EventArgs e)
        {
            Ayraç_Seçenekler_Muhataplar.SplitterDistance = Ayraç_Seçenekler_Muhataplar.Width * 25 / 100;
            Ayraç_FiltreSeçenekler_Kıstaslar_Seçenekler.SplitterDistance = Ayraç_FiltreSeçenekler_Kıstaslar_Seçenekler.Width * 50 / 100;
            Ayraç_Filtre_TabloSonuç.SplitterDistance = Height / 5;

            Sorgula_MuhatapGrubu.Başlat(Ortak.Banka.Seçilenİşyeri.MuhatapGrubu_Listele(true), Ortak.Banka.Seçilenİşyeri.MuhatapGrubu_Listele(), "Muhatap Grupları", ListeKutusu_Ayarlar);

            ListeKutusu.Ayarlar_ ListeKutusu_Ayarlar_Şablon = new ListeKutusu.Ayarlar_(
                Eklenebilir: Ortak.Banka.İzinliMi(Banka1.Ayarlar_Kullanıcılar_İzin.Ayarları_değiştirebilir),
                Silinebilir: Ortak.Banka.İzinliMi(Banka1.Ayarlar_Kullanıcılar_İzin.Ayarları_değiştirebilir),
                ElemanKonumu: Ortak.Banka.İzinliMi(Banka1.Ayarlar_Kullanıcılar_İzin.Ayarları_değiştirebilir) ? ListeKutusu.Ayarlar_.ElemanKonumu_.Değiştirilebilir : ListeKutusu.Ayarlar_.ElemanKonumu_.OlduğuGibi,
                AdıDeğiştirilebilir: false, Gizlenebilir: false);
            Sorgula_Şablonlar.Başlat(null, Ortak.Banka.Ayarlar.CariDökümŞablonlar.Keys.ToList(), "Şablonlar", ListeKutusu_Ayarlar_Şablon);
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
                    Sorgula_TümünüSeç_Click();
                    Sorgula_Tarih_Kayıt.Checked = true;
                    Sorgula_Başlangıç.Value = Ödeme.İlkKayıtTarihi;
                    Sorgula_Bitiş.Value = Ödeme.İlkKayıtTarihi;
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

                        bool üyelik_olarak_göster = Ödeme.ÜyelikKayıtTarihi != null;
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
                        if (üyelik_olarak_göster) _1_satır_dizisi_.Cells[Tablo_Üyelik.Index].ToolTipText = Ödeme.ÜyelikKayıtTarihi.Value.Yazıya();
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
        public void Şablon_Seç_TabloyuOluştur(string Adı)
        {
            if (Adı.DoluMu() && !Sorgula_Şablonlar.Tüm_Elemanlar.Contains(Adı)) throw new Exception("Yazdırma şablonu (" + Adı + ") bulunamadı");

            if (Sorgula_Şablonlar.SeçilenEleman_Adı == Adı) Sorgula_Click(null, null);
            else Sorgula_Şablonlar.SeçilenEleman_Adı = Adı;
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
            Banka1.Ayarlar_CariDöküm_Şablon_ şablon;

            switch (Türü)
            {
                case ListeKutusu.İşlemTürü.YeniEklendi:
                    şablon = new Banka1.Ayarlar_CariDöküm_Şablon_();
                    şablon.TarihTürü = Sorgula_Tarih_Ödeme.Checked ? Banka1.Ayarlar_CariDöküm_Şablon_.TarihTürü_.ÖdemeTarihi :
                                        Sorgula_Tarih_Sonİşlem.Checked ? Banka1.Ayarlar_CariDöküm_Şablon_.TarihTürü_.SonİşlemTarihi : Banka1.Ayarlar_CariDöküm_Şablon_.TarihTürü_.KayıtTarihi;
                    şablon.TarihAralığı = Sorgula_TarihAralığı.SelectedIndex;
                    şablon.Kıstas_Seçim = Sorgula_KıstasSeçimi.SelectedIndex;
                    şablon.Gecikti = Sorgula_Gecikti.Checked;
                    şablon.Ödenmedi = Sorgula_Ödenmedi.Checked;
                    şablon.KısmenÖdendi = Sorgula_KısmenÖdendi.Checked;
                    şablon.TamÖdendi = Sorgula_TamÖdendi.Checked;
                    şablon.MaaşÖdemesi = Sorgula_MaaşÖdemesi.Checked;
                    şablon.AvansVerilmesi = Sorgula_AvansVerilmesi.Checked;
                    şablon.AvansÖdemesi = Sorgula_AvansÖdemesi.Checked;
                    şablon.PeşinatÖdendi = Sorgula_PeşinatÖdendi.Checked;
                    şablon.KısmiÖdemeYapıldı = Sorgula_KısmiÖdemeYapıldı.Checked;
                    şablon.KontrolNoktası = Sorgula_KontrolNoktası.Checked;
                    şablon.Taksitli = Sorgula_Taksitli.Checked;
                    şablon.Üyelik = Sorgula_Üyelik.Checked;
                    şablon.İptalEdildi = Sorgula_İptalEdildi.Checked;
                    şablon.Maaşlar = Sorgula_Maaşlar.Checked;
                    şablon.AltToplam = Sorgula_AltToplam.Checked;
                    şablon.Gelir = Sorgula_Gelir.Checked;
                    şablon.Gider = Sorgula_Gider.Checked;
                    şablon.Kapsam_Grup = Kapsam_Grup;
                    şablon.Kapsam_Muhatap = Kapsam_Muhatap;

                    Ortak.Banka.Ayarlar.CariDökümŞablonlar[Adı] = şablon;
                    Ortak.Banka.Ayarlar.DeğişiklikYapıldı = true;
                    Banka_Ortak.DeğişiklikleriKaydet();
                    break;

                case ListeKutusu.İşlemTürü.ElemanSeçildi:
                    if (!Ortak.Banka.Ayarlar.CariDökümŞablonlar.TryGetValue(Adı, out şablon)) şablon = new Banka1.Ayarlar_CariDöküm_Şablon_();
                    switch (şablon.TarihTürü)
                    {
                        case Banka1.Ayarlar_CariDöküm_Şablon_.TarihTürü_.ÖdemeTarihi:
                            Sorgula_Tarih_Ödeme.Checked = true;
                            break;
                        case Banka1.Ayarlar_CariDöküm_Şablon_.TarihTürü_.SonİşlemTarihi:
                            Sorgula_Tarih_Sonİşlem.Checked = true;
                            break;
                        case Banka1.Ayarlar_CariDöküm_Şablon_.TarihTürü_.KayıtTarihi:
                            Sorgula_Tarih_Kayıt.Checked = true;
                            break;
                    }

                    Sorgula_TarihAralığı.SelectedIndex = şablon.TarihAralığı;
                    Sorgula_KıstasSeçimi.SelectedIndex = şablon.Kıstas_Seçim;
                    Sorgula_Gecikti.Checked = şablon.Gecikti;
                    Sorgula_Ödenmedi.Checked = şablon.Ödenmedi;
                    Sorgula_KısmenÖdendi.Checked = şablon.KısmenÖdendi;
                    Sorgula_TamÖdendi.Checked = şablon.TamÖdendi;
                    Sorgula_MaaşÖdemesi.Checked = şablon.MaaşÖdemesi;
                    Sorgula_AvansVerilmesi.Checked = şablon.AvansVerilmesi;
                    Sorgula_AvansÖdemesi.Checked = şablon.AvansÖdemesi;
                    Sorgula_PeşinatÖdendi.Checked = şablon.PeşinatÖdendi;
                    Sorgula_KısmiÖdemeYapıldı.Checked = şablon.KısmiÖdemeYapıldı;
                    Sorgula_KontrolNoktası.Checked = şablon.KontrolNoktası;
                    Sorgula_Taksitli.Checked = şablon.Taksitli;
                    Sorgula_Üyelik.Checked = şablon.Üyelik;
                    Sorgula_İptalEdildi.Checked = şablon.İptalEdildi;
                    Sorgula_Maaşlar.Checked = şablon.Maaşlar;
                    Sorgula_AltToplam.Checked = şablon.AltToplam;
                    Sorgula_Gelir.Checked = şablon.Gelir;
                    Sorgula_Gider.Checked = şablon.Gider;
                    Sorgula_MuhatapGrubu.SeçilenEleman_Adları = şablon.Kapsam_Grup;
                    Sorgula_Muhatap.SeçilenEleman_Adları = şablon.Kapsam_Muhatap;

                    Sorgula_Click(null, null);
                    break;

                case ListeKutusu.İşlemTürü.KonumDeğişikliğiKaydedildi:
                    Dictionary<string, Banka1.Ayarlar_CariDöküm_Şablon_> YeniListe = new Dictionary<string, Banka1.Ayarlar_CariDöküm_Şablon_>();
                    foreach (string şablonadı in Sorgula_Şablonlar.Tüm_Elemanlar)
                    {
                        şablon = Ortak.Banka.Ayarlar.CariDökümŞablonlar[şablonadı];
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
        private void Sorgula_Maaşlar_CheckedChanged(object sender, EventArgs e)
        {
            Sorgula_Bitiş.Enabled = !Sorgula_Maaşlar.Checked;

            if (Sorgula_Maaşlar.Checked)
            {
                Sorgula_TümünüSeç_Click();

                DateTime tt = DateTime.Now;
                Sorgula_Başlangıç.Value = new DateTime(tt.Year, tt.Month, Ortak.Banka.Seçilenİşyeri.AylıkÜcretÖdemeGünü);
            }
        }
        private void Sorgula_AltToplam_CheckedChanged(object sender, EventArgs e)
        {
            if (Sorgula_AltToplam.Checked) Sorgula_TümünüSeç_Click();
        }
        private void Sorgula_TarihAralığı_SelectedIndexChanged(object sender, EventArgs e)
        {
            DateTime tt = DateTime.Now;
            tt = new DateTime(tt.Year, tt.Month, tt.Day);

            switch (Sorgula_TarihAralığı.SelectedIndex)
            {
                case 0: //bu ay
                    Sorgula_Başlangıç.Value = new DateTime(tt.Year, tt.Month, 1);
                    Sorgula_Bitiş.Value = new DateTime(tt.Year, tt.Month, DateTime.DaysInMonth(tt.Year, tt.Month), 23, 59, 59);
                    break;

                case 1://bu hafta
                    tt = tt.AddDays((int)DayOfWeek.Monday - (int)tt.DayOfWeek);
                    Sorgula_Başlangıç.Value = new DateTime(tt.Year, tt.Month, tt.Day);
                    tt = tt.AddDays(6);
                    Sorgula_Bitiş.Value = new DateTime(tt.Year, tt.Month, tt.Day, 23, 59, 59);
                    break;

                default:
                case 2://bugün
                    Sorgula_Başlangıç.Value = tt;
                    Sorgula_Bitiş.Value = new DateTime(tt.Year, tt.Month, tt.Day, 23, 59, 59);
                    break;
            }
        }
        void Sorgula_TümünüSeç_Click()
        {
            Sorgula_TümünüSeç_Click(null, null);
            if (!Sorgula_Gelir.Checked) Sorgula_TümünüSeç_Click(null, null);
        }
        private void Sorgula_TümünüSeç_Click(object sender, EventArgs e)
        {
            bool yeni_değer = !Sorgula_Gelir.Checked;
            foreach (Control ctrl in Sorgula_Seçenekleri.Controls)
            {
                CheckBox ctr = ctrl as CheckBox;
                if (ctr != null && ctrl != Sorgula_Maaşlar && ctrl != Sorgula_AltToplam)
                {
                    (ctrl as CheckBox).Checked = yeni_değer;
                }
            }
        }
        private void Sorgula_Click(object sender, EventArgs e)
        {
            Sorgula.Enabled = false;
            Öde_Geri_Click(null, null);
            Düzenle_Geri_Click(null, null);
            KontrolNoktası_Geri_Click(null, null);

            if (Sorgula_Maaşlar.Checked)
            {
                Sorgula_Başlangıç.Value = new DateTime(Sorgula_Başlangıç.Value.Year, Sorgula_Başlangıç.Value.Month, Ortak.Banka.Seçilenİşyeri.AylıkÜcretÖdemeGünü);
                Sorgula_Bitiş.Value = Sorgula_Başlangıç.Value.AddMonths(1).AddDays(-1);
                Sorgula_Tarih_Ödeme.Checked = true;
            }

            DateTime başlangıç_dt = Sorgula_Başlangıç.Value;
            DateTime bitiş_dt = Sorgula_Bitiş.Value;
            if (başlangıç_dt > bitiş_dt)
            {
                DateTime gecici = başlangıç_dt;
                başlangıç_dt = bitiş_dt;
                bitiş_dt = gecici;
            }
            DateOnly başlangıç_d = DateOnly.FromDateTime(başlangıç_dt);
            DateOnly bitiş_d = DateOnly.FromDateTime(bitiş_dt);

            Açıklamalar.Text = null;
            Tablo.Rows.Clear();
            Tablo_CellMouseClick(null, null);
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
            Dictionary<string, double> Maaşlar = new Dictionary<string, double>();
            Dictionary<string, double> AltToplamlar = new Dictionary<string, double>();
            const char AltToplamlar_Ayraç = 'é';

            Tablo_Taksit.Visible = Sorgula_Taksitli.Checked;
            Tablo_Üyelik.Visible = Sorgula_Üyelik.Checked;
            Tablo_SonİşlemTarihi.Visible = Sorgula_Tarih_Sonİşlem.Checked;
            Tablo_KayıtTarihi.Visible = Sorgula_Tarih_Kayıt.Checked;

            foreach (string yıl in Ortak.Banka.Seçilenİşyeri.Ödemeler_Listele_Yıllar())
            {
                int yıll = yıl.TamSayıya();
                if (Sorgula_Tarih_Kayıt.Checked && (yıll < başlangıç_d.Year || yıll > bitiş_d.Year)) continue;

                Banka1.İşyeri_BirYıllıkDönem_ BirYıllıkDönem = Ortak.Banka.Seçilenİşyeri.Ödemeler_Listele_BirYıllıkDönem(yıl);
                foreach (Banka1.İşyeri_Ödeme_ ödeme in BirYıllıkDönem.Ödemeler)
                {
                    _TabloyaEkle_(ödeme, true);
                }
            }

            //Zaman aralığı içindeki gelecekteki üyeliklere ait ödemeler
            foreach (Banka1.İşyeri_Ödeme_ ödeme in Ortak.Banka.Seçilenİşyeri.Üyelik_OlacaklarıHesapla(bitiş_d))
            {
                _TabloyaEkle_(ödeme);
            }

            if (Maaşlar.Keys.Count > 0)
            {
                foreach (KeyValuePair<string, double> maaş in Maaşlar)
                {
                    object[] dizin = new object[sutun_sayısı];
                    dizin[Tablo_MuhatapGrubu.Index] = Banka1.Çalışan_Yazısı;
                    dizin[Tablo_Muhatap.Index] = maaş.Key;
                    dizin[Tablo_ÖdemeTarihi.Index] = başlangıç_d;
                    dizin[Tablo_Tip.Index] = "Hesaplanan aylık ücret";
                    dizin[Tablo_Miktar.Index] = Banka_Ortak.Yazdır_Ücret(maaş.Value, Banka1.İşyeri_Ödeme_.ParaBirimi_.TürkLirası);

                    DataGridViewRow _1_satır_dizisi_ = new DataGridViewRow();
                    dizi.Add(_1_satır_dizisi_);
                    _1_satır_dizisi_.CreateCells(Tablo, dizin);
                    _1_satır_dizisi_.Cells[Tablo_Tip.Index].Style.BackColor = Ortak.Renk_Sarı;
                }
            }

            if (AltToplamlar.Keys.Count > 0)
            {
                foreach (KeyValuePair<string, double> AltToplam in AltToplamlar)
                {
                    object[] dizin = new object[sutun_sayısı];
                    string[] AltToplam_Ad_Dizisi = AltToplam.Key.Split(AltToplamlar_Ayraç);

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

            Tablo.Rows.AddRange(dizi.ToArray());
            Tablo.Sort(Sorgula_Maaşlar.Checked || Sorgula_AltToplam.Checked ? Tablo_Muhatap :
                        Sorgula_Tarih_Ödeme.Checked ? Tablo_ÖdemeTarihi :
                        Sorgula_Tarih_Sonİşlem.Checked ? Tablo_SonİşlemTarihi : Tablo_KayıtTarihi, System.ComponentModel.ListSortDirection.Descending);
            Tablo.ClearSelection();

            Sorgula_Başlangıç.Value = new DateTime(Sorgula_Başlangıç.Value.Year, Sorgula_Başlangıç.Value.Month, Sorgula_Başlangıç.Value.Day);
            Sorgula_Bitiş.Value = new DateTime(Sorgula_Bitiş.Value.Year, Sorgula_Bitiş.Value.Month, Sorgula_Bitiş.Value.Day, 23, 59, 59, 999);

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
            void _GeriBildirimİşlemi_DövizKurları_(double Dolar, double Avro)
            {
                Ödeme_Ekranı.Invoke(new Action(() =>
                {
                    Açıklamalar.Text = "Avro : " + Banka_Ortak.Yazdır_Ücret(Avro, Banka1.İşyeri_Ödeme_.ParaBirimi_.TürkLirası) + ", Dolar : " + Banka_Ortak.Yazdır_Ücret(Dolar, Banka1.İşyeri_Ödeme_.ParaBirimi_.TürkLirası) +
                        Environment.NewLine + Environment.NewLine + Açıklamalar.Text;
                }));
            }

            void _TabloyaEkle_(Banka1.İşyeri_Ödeme_ _ödeme_, bool Eşle = false)
            {
                KeyValuePair<DateTime, Banka1.İşyeri_Ödeme_İşlem_> ilk_işlem = _ödeme_.İşlemler.First();
                KeyValuePair<DateTime, Banka1.İşyeri_Ödeme_İşlem_> son_işlem = _ödeme_.İşlemler.Last();

                Banka1.İşyeri_Ödeme_İşlem_.Tipi_ tipi = son_işlem.Value.Tipi;
                if (tipi == Banka1.İşyeri_Ödeme_İşlem_.Tipi_.KontrolNoktası)
                {
                    if (!Sorgula_KontrolNoktası.Checked) return;

                    DateTime trh = Sorgula_Tarih_Kayıt.Checked ? ilk_işlem.Key : ilk_işlem.Value.ÖdemeninYapılacağıTarih.ToDateTime(new TimeOnly());
                    if (trh < başlangıç_dt || trh > bitiş_dt) return; //kapsam dışında

                    object[] dizin = new object[sutun_sayısı];
                    dizin[Tablo_Tip.Index] = son_işlem.Value.Tipi.Yazdır();
                    dizin[Tablo_ÖdemeTarihi.Index] = ilk_işlem.Value.ÖdemeninYapılacağıTarih;
                    dizin[Tablo_Notlar.Index] = _ödeme_.Notlar;
                    dizin[Tablo_SonİşlemTarihi.Index] = ilk_işlem.Key;
                    dizin[Tablo_KayıtTarihi.Index] = ilk_işlem.Key;
                    dizin[Tablo_KullanıcıAdı.Index] = son_işlem.Value.GerçekleştirenKullanıcıAdı;

                    DataGridViewRow _1_satır_dizisi_ = new DataGridViewRow();
                    dizi.Add(_1_satır_dizisi_);
                    _1_satır_dizisi_.CreateCells(Tablo, dizin);

                    for (int i = 0; i < sutun_sayısı; i++)
                    {
                        _1_satır_dizisi_.Cells[i].Style.BackColor = Ortak.Renk_Mavi;
                    }
                }
                else
                {
                    Banka1.İşyeri_Ödeme_İşlem_.Durum_ durumu = son_işlem.Value.Durumu;
                    bool gelir_olarak_göster = son_işlem.Value.Tipi.GelirMi();
                    bool üyelik_olarak_göster = _ödeme_.ÜyelikKayıtTarihi != null;
                    bool gecikti_olarak_göster = !durumu.ÖdendiMi() && son_işlem.Value.ÖdemeninYapılacağıTarih <= şimdi;
                    bool iptaledildi_olarak_göster = durumu == Banka1.İşyeri_Ödeme_İşlem_.Durum_.İptalEdildi;

                    DateTime ödeme_zamanı = son_işlem.Value.ÖdemeninYapılacağıTarih.ToDateTime(new TimeOnly());
                    if (Sorgula_Maaşlar.Checked &&
                        _ödeme_.MuhatapGrubuAdı == Banka1.Çalışan_Yazısı &&
                        _ödeme_.Tipi != Banka1.İşyeri_Ödeme_İşlem_.Tipi_.AvansVerilmesi &&
                        ödeme_zamanı >= başlangıç_dt &&
                        ödeme_zamanı <= bitiş_dt &&
                        !iptaledildi_olarak_göster &&
                        (Kapsam_Muhatap == null || Kapsam_Muhatap.Contains(_ödeme_.MuhatapAdı)))
                    {
                        double miktar = 0;
                        if (Maaşlar.ContainsKey(_ödeme_.MuhatapAdı)) miktar = Maaşlar[_ödeme_.MuhatapAdı];

                        if (gelir_olarak_göster) miktar -= _ödeme_.Miktarı;
                        else miktar += _ödeme_.Miktarı;

                        Maaşlar[_ödeme_.MuhatapAdı] = miktar;
                    }

                    DateTime trh = DateTime.MinValue;
                    if (Sorgula_Tarih_Ödeme.Checked) trh = ödeme_zamanı;
                    else if (Sorgula_Tarih_Sonİşlem.Checked) trh = son_işlem.Key;
                    else trh = ilk_işlem.Key; //ilk kayıt tar
                    if ((trh < başlangıç_dt || trh > bitiş_dt) &&
                        !(Sorgula_Tarih_Ödeme.Checked && gecikti_olarak_göster && Sorgula_Gecikti.Checked && !Sorgula_Maaşlar.Checked)) return; //kapsam dışında

                    bool listele = (Kapsam_Grup == null || Kapsam_Grup.Contains(_ödeme_.MuhatapGrubuAdı)) &&
                                    (Kapsam_Muhatap == null || Kapsam_Muhatap.Contains(_ödeme_.MuhatapAdı));
                    if (!listele) return;

                    if (Sorgula_AltToplam.Checked &&
                            !iptaledildi_olarak_göster &&
                            _ödeme_.MuhatapGrubuAdı != Banka1.Çalışan_Yazısı)
                    {
                        string anahtar = ((int)_ödeme_.ParaBirimi).Yazıya() + AltToplamlar_Ayraç + _ödeme_.MuhatapGrubuAdı;
                        double miktar = 0;
                        if (AltToplamlar.ContainsKey(anahtar)) miktar = AltToplamlar[anahtar];
                        if (gelir_olarak_göster) miktar += _ödeme_.Miktarı;
                        else miktar -= _ödeme_.Miktarı;
                        AltToplamlar[anahtar] = miktar;

                        anahtar += AltToplamlar_Ayraç + _ödeme_.MuhatapAdı;
                        miktar = 0;
                        if (AltToplamlar.ContainsKey(anahtar)) miktar = AltToplamlar[anahtar];
                        if (gelir_olarak_göster) miktar += _ödeme_.Miktarı;
                        else miktar -= _ödeme_.Miktarı;
                        AltToplamlar[anahtar] = miktar;
                    }

                    if (Sorgula_KıstasSeçimi.SelectedIndex == 0)
                    {
                        //en az 1 kıstasa uyan
                        listele =
                            (tipi.GelirMi() && Sorgula_Gelir.Checked) ||
                            (!tipi.GelirMi() && Sorgula_Gider.Checked) ||
                            (gecikti_olarak_göster && Sorgula_Gecikti.Checked) ||
                            (durumu == Banka1.İşyeri_Ödeme_İşlem_.Durum_.Ödenmedi && Sorgula_Ödenmedi.Checked) ||
                            (durumu == Banka1.İşyeri_Ödeme_İşlem_.Durum_.KısmenÖdendi && Sorgula_KısmenÖdendi.Checked) ||
                            (durumu == Banka1.İşyeri_Ödeme_İşlem_.Durum_.TamÖdendi && Sorgula_TamÖdendi.Checked) ||
                            (tipi == Banka1.İşyeri_Ödeme_İşlem_.Tipi_.MaaşÖdemesi && Sorgula_MaaşÖdemesi.Checked) ||
                            (tipi == Banka1.İşyeri_Ödeme_İşlem_.Tipi_.AvansVerilmesi && Sorgula_AvansVerilmesi.Checked) ||
                            (tipi == Banka1.İşyeri_Ödeme_İşlem_.Tipi_.AvansÖdemesi && Sorgula_AvansÖdemesi.Checked) ||
                            (_ödeme_.Taksit != null && Sorgula_Taksitli.Checked) ||
                            (üyelik_olarak_göster && Sorgula_Üyelik.Checked) ||
                            (durumu == Banka1.İşyeri_Ödeme_İşlem_.Durum_.PeşinatÖdendi && Sorgula_PeşinatÖdendi.Checked) ||
                            (durumu == Banka1.İşyeri_Ödeme_İşlem_.Durum_.KısmiÖdemeYapıldı && Sorgula_KısmiÖdemeYapıldı.Checked) ||
                            (iptaledildi_olarak_göster && Sorgula_İptalEdildi.Checked);
                    }
                    else
                    {
                        //tüm kıstaslara uyan
                        listele =
                            (tipi.GelirMi() && !Sorgula_Gelir.Checked) ||
                            (!tipi.GelirMi() && !Sorgula_Gider.Checked) ||
                            (gecikti_olarak_göster && !Sorgula_Gecikti.Checked) ||
                            (durumu == Banka1.İşyeri_Ödeme_İşlem_.Durum_.Ödenmedi && !Sorgula_Ödenmedi.Checked) ||
                            (durumu == Banka1.İşyeri_Ödeme_İşlem_.Durum_.KısmenÖdendi && !Sorgula_KısmenÖdendi.Checked) ||
                            (durumu == Banka1.İşyeri_Ödeme_İşlem_.Durum_.TamÖdendi && !Sorgula_TamÖdendi.Checked) ||
                            (tipi == Banka1.İşyeri_Ödeme_İşlem_.Tipi_.MaaşÖdemesi && !Sorgula_MaaşÖdemesi.Checked) ||
                            (tipi == Banka1.İşyeri_Ödeme_İşlem_.Tipi_.AvansVerilmesi && !Sorgula_AvansVerilmesi.Checked) ||
                            (tipi == Banka1.İşyeri_Ödeme_İşlem_.Tipi_.AvansÖdemesi && !Sorgula_AvansÖdemesi.Checked) ||
                            (_ödeme_.Taksit != null && !Sorgula_Taksitli.Checked) ||
                            (üyelik_olarak_göster && !Sorgula_Üyelik.Checked) ||
                            (durumu == Banka1.İşyeri_Ödeme_İşlem_.Durum_.PeşinatÖdendi && !Sorgula_PeşinatÖdendi.Checked) ||
                            (durumu == Banka1.İşyeri_Ödeme_İşlem_.Durum_.KısmiÖdemeYapıldı && !Sorgula_KısmiÖdemeYapıldı.Checked) ||
                            (iptaledildi_olarak_göster && !Sorgula_İptalEdildi.Checked);
                        listele = !listele;
                    }
                    if (!listele) return;

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

                    DataGridViewRow _1_satır_dizisi_ = new DataGridViewRow();
                    dizi.Add(_1_satır_dizisi_);
                    _1_satır_dizisi_.CreateCells(Tablo, dizin);

                    if (Eşle)
                    {
                        _1_satır_dizisi_.Tag = _ödeme_;

                        string açıklama = Banka_Ortak.Yazdır_Tarih_Gün(şimdi.ToDateTime(new TimeOnly()) - son_işlem.Value.ÖdemeninYapılacağıTarih.ToDateTime(new TimeOnly()));
                        if (gecikti_olarak_göster) _1_satır_dizisi_.Cells[Tablo_ÖdemeTarihi.Index].ToolTipText = Banka_Ortak.Yazdır_Tarih_Gün(şimdi.ToDateTime(new TimeOnly()) - son_işlem.Value.ÖdemeninYapılacağıTarih.ToDateTime(new TimeOnly())) + " GECİKTİ";
                        else _1_satır_dizisi_.Cells[Tablo_ÖdemeTarihi.Index].ToolTipText = durumu.ÖdendiMi() ? null : açıklama + " daha var";
                    }
                    else
                    {
                        _1_satır_dizisi_.Cells[Tablo_ÖdemeTarihi.Index].Style.BackColor = Ortak.Renk_Mavi;
                        _1_satır_dizisi_.Cells[Tablo_ÖdemeTarihi.Index].ToolTipText = "Sadece bilgi amaçlıdır" + Environment.NewLine +
                            Banka_Ortak.Yazdır_Tarih_Gün(son_işlem.Value.ÖdemeninYapılacağıTarih.ToDateTime(new TimeOnly()) - şimdi.ToDateTime(new TimeOnly())) + " daha var";
                    }

                    if (_ödeme_.İşlemler.Count > 1)
                    {
                        _1_satır_dizisi_.Cells[Tablo_SonİşlemTarihi.Index].ToolTipText = "Sürüm : " + _ödeme_.İşlemler.Count;
                        _1_satır_dizisi_.Cells[Tablo_SonİşlemTarihi.Index].Style.BackColor = Ortak.Renk_Mavi;
                    }
                    _1_satır_dizisi_.Cells[Tablo_Miktar.Index].Style.BackColor = gelir_olarak_göster ? Ortak.Renk_Gelir : Ortak.Renk_Gider;
                    _1_satır_dizisi_.Cells[Tablo_Miktar.Index].ToolTipText = gelir_olarak_göster ? "Gelir" : "Gider";
                    if (gecikti_olarak_göster) _1_satır_dizisi_.Cells[Tablo_ÖdemeTarihi.Index].Style.BackColor = Ortak.Renk_Kırmızı;
                    if (üyelik_olarak_göster) _1_satır_dizisi_.Cells[Tablo_Üyelik.Index].ToolTipText = _ödeme_.ÜyelikKayıtTarihi.Value.Yazıya();
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
        private void Tablo_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e != null && e.RowIndex >= 0 && Tablo.Rows[e.RowIndex].Tag != null && Tablo.Rows[e.RowIndex].Tag is Banka1.İşyeri_Ödeme_)
            {
                Banka1.İşyeri_Ödeme_ ödeme = Tablo.Rows[e.RowIndex].Tag as Banka1.İşyeri_Ödeme_;

                if (Öde.Visible)
                {
                    Öde.Enabled = !ödeme.Durumu.ÖdendiMi();
                    Düzenle.Enabled = true;
                }

                İlişkiliÖdemeleriListele.Enabled = true;
                SürümleriListele.Enabled = ödeme.İşlemler.Count > 1;
            }
            else
            {
                Öde.Enabled = false;
                Düzenle.Enabled = false;
                İlişkiliÖdemeleriListele.Enabled = false;
                SürümleriListele.Enabled = false;
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

            if (ödeme.ParaBirimi != Seçilen_parabirimi)
            {
                //farklı para birimi
                Döviz.KurlarıAl(_GeriBildirimİşlemi_DövizKurları_);

                void _GeriBildirimİşlemi_DövizKurları_(double Dolar, double Avro)
                {
                    Ödeme_Ekranı.Invoke(new Action(() =>
                    {
                        double KısmiÖdemeMiktarı_TürkLirasıOlarak = 0;
                        switch (Seçilen_parabirimi)
                        {
                            case Banka1.İşyeri_Ödeme_.ParaBirimi_.TürkLirası:
                                KısmiÖdemeMiktarı_TürkLirasıOlarak = (double)Öde_KısmiÖdeme_Miktar.Value;
                                break;
                            case Banka1.İşyeri_Ödeme_.ParaBirimi_.Avro:
                                KısmiÖdemeMiktarı_TürkLirasıOlarak = (double)Öde_KısmiÖdeme_Miktar.Value * Avro;
                                break;
                            case Banka1.İşyeri_Ödeme_.ParaBirimi_.Dolar:
                                KısmiÖdemeMiktarı_TürkLirasıOlarak = (double)Öde_KısmiÖdeme_Miktar.Value * Dolar;
                                break;
                        }

                        double TamÖdemeMiktarı_TürkLirasıOlarak = 0;
                        switch (ödeme.ParaBirimi)
                        {
                            case Banka1.İşyeri_Ödeme_.ParaBirimi_.TürkLirası:
                                TamÖdemeMiktarı_TürkLirasıOlarak = ödeme.Miktarı;
                                break;
                            case Banka1.İşyeri_Ödeme_.ParaBirimi_.Avro:
                                TamÖdemeMiktarı_TürkLirasıOlarak = ödeme.Miktarı * Avro;
                                break;
                            case Banka1.İşyeri_Ödeme_.ParaBirimi_.Dolar:
                                TamÖdemeMiktarı_TürkLirasıOlarak = ödeme.Miktarı * Dolar;
                                break;
                        }

                        double KalanÖdemeMiktarı_TürkLirasıOlarak = TamÖdemeMiktarı_TürkLirasıOlarak - KısmiÖdemeMiktarı_TürkLirasıOlarak;

                        double KalanÖdemeMiktarı_ÖdemeParaBiriminde = 0;
                        switch (ödeme.ParaBirimi)
                        {
                            case Banka1.İşyeri_Ödeme_.ParaBirimi_.TürkLirası:
                                KalanÖdemeMiktarı_ÖdemeParaBiriminde = KalanÖdemeMiktarı_TürkLirasıOlarak;
                                break;
                            case Banka1.İşyeri_Ödeme_.ParaBirimi_.Avro:
                                KalanÖdemeMiktarı_ÖdemeParaBiriminde = KalanÖdemeMiktarı_TürkLirasıOlarak / Avro;

                                break;
                            case Banka1.İşyeri_Ödeme_.ParaBirimi_.Dolar:
                                KalanÖdemeMiktarı_ÖdemeParaBiriminde = KalanÖdemeMiktarı_TürkLirasıOlarak / Dolar;
                                break;
                        }

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
                    }));
                }
            }
            else
            {
                //aynı para birimi
                double KalanÖdemeMiktarı = ödeme.Miktarı - (double)Öde_KısmiÖdeme_Miktar.Value;
                if (KalanÖdemeMiktarı > 0)
                {
                    Öde_KısmiÖdeme_Miktar.BackColor = Color.White;
                    Öde_KısmiÖdeme_Miktar.Tag = KalanÖdemeMiktarı;
                    Öde_Kaydet.Enabled = true;
                }
                else
                {
                    Öde_KısmiÖdeme_Miktar.BackColor = Ortak.Renk_Kırmızı;
                }
                Öde_KısmiÖdeme.Text = "Kısmi Ödeme ( Kalan : " + Banka_Ortak.Yazdır_Ücret(KalanÖdemeMiktarı, ödeme.ParaBirimi) + " )";
            }
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

                Banka1.Muhatap_ muhatap = Ortak.Banka.Seçilenİşyeri.Muhatap_Aç(ödeme.MuhatapGrubuAdı, ödeme.MuhatapAdı);
                muhatap.GelirGider_Ekle(muhatap.GelirGider_Oluştur_KısmiÖdeme(ödeme.Tipi, ödeme.İlkKayıtTarihi, YapılanÖdeme_ÖdemeParaBiriminde, ödeme.ParaBirimi, Öde_KalanÖdemeTarihi.Value, açıklama, ödeme.Taksit, ödeme.ÜyelikKayıtTarihi));
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
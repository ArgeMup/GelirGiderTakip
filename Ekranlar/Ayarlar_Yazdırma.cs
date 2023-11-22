using System.Drawing.Printing;
using System.Drawing;
using System.Windows.Forms;
using System;
using ArgeMup.HazirKod;
using System.Drawing.Text;
using System.Collections.Generic;
using System.IO;
using ArgeMup.HazirKod.Ekİşlemler;
using System.Linq;

namespace Gelir_Gider_Takip.Ekranlar
{
    public partial class Ayarlar_Yazdırma : Form
    {
        public Ayarlar_Yazdırma(bool ÖnyüzüGöster = false)
        {
            InitializeComponent();

            Yazcılar.Items.Clear();
            for (int i = 0; i < PrinterSettings.InstalledPrinters.Count; i++)
            {
                Yazcılar.Items.Add(PrinterSettings.InstalledPrinters[i]);
            }
            if (Yazcılar.Items.Count < 1)
            {
                Yazcılar.Items.Add("Yazıcı bulunamadı");
                Yazcılar.SelectedIndex = 0;
                Enabled = false;
                return;
            }

            string bulunan = "";
            if (Ortak.Banka.Ayarlar.Yazdırma.YazıcıAdı.BoşMu())
            {
                foreach (string y in Yazcılar.Items)
                {
                    if (y.ToLower().Contains("pdf"))
                    {
                        DosyayaYazdır.Checked = true;
                        bulunan = y;
                        break;
                    }
                }
            }
            if (string.IsNullOrEmpty(bulunan)) bulunan = (string)Yazcılar.Items[0];
            Yazcılar.Text = Ortak.Banka.Ayarlar.Yazdırma.YazıcıAdı ?? bulunan;

            KarakterKümeleri.Items.Clear();
            foreach (var kk in new InstalledFontCollection().Families)
            {
                KarakterKümeleri.Items.Add(kk.Name);
            }
            KarakterKümeleri.Text = Ortak.Banka.Ayarlar.Yazdırma.KarakterKümesi;

            KenarBoşluğu.Value = (decimal)Ortak.Banka.Ayarlar.Yazdırma.KenarBoşluğu_mm;
            DosyayaYazdır.Checked = Ortak.Banka.Ayarlar.Yazdırma.DosyayaYazdır;
            RenkliHücreler.Checked = Ortak.Banka.Ayarlar.Yazdırma.RenkliHücreler;
            YatayGörünüm.Checked = Ortak.Banka.Ayarlar.Yazdırma.YatayGörünüm;
            KarakterBüyüklüğü.Value = (decimal)Ortak.Banka.Ayarlar.Yazdırma.KarakterBüyüklüğü;
            FirmaLogo_Genişlik.Value = (decimal)Ortak.Banka.Ayarlar.Yazdırma.FirmaLogo_Genişlik;
            FirmaLogo_Yükseklik.Value = (decimal)Ortak.Banka.Ayarlar.Yazdırma.FirmaLogo_Yükseklik;

            if (ÖnyüzüGöster)
            {
                TabloŞablonu.Items.AddRange(Ortak.Banka.Ayarlar.CariDökümŞablonlar.Keys.ToArray());
                if (TabloŞablonu.Items.Count > 0) TabloŞablonu.SelectedIndex = 0;

                KarakterKümeleri.SelectedIndexChanged += Ayar_Değişti;
                DosyayaYazdır.CheckedChanged += Ayar_Değişti;
                RenkliHücreler.CheckedChanged += Ayar_Değişti;
                YatayGörünüm.CheckedChanged += Ayar_Değişti;
                KenarBoşluğu.ValueChanged += Ayar_Değişti;
                KarakterBüyüklüğü.ValueChanged += Ayar_Değişti;
                FirmaLogo_Genişlik.ValueChanged += Ayar_Değişti;
                FirmaLogo_Yükseklik.ValueChanged += Ayar_Değişti;
                TabloŞablonu.SelectedIndexChanged += Ayar_Değişti;
                Ayar_Değişti(null, null);
            }
            else Hide();

            ÖnYüzler_Kaydet.Enabled = false;
        }

        private void ÖnYüzler_Kaydet_Click(object sender, EventArgs e)
        {
            Ortak.Banka.Ayarlar.Yazdırma.YazıcıAdı = Yazcılar.Text;
            Ortak.Banka.Ayarlar.Yazdırma.KenarBoşluğu_mm = (float)KenarBoşluğu.Value;
            Ortak.Banka.Ayarlar.Yazdırma.DosyayaYazdır = DosyayaYazdır.Checked;
            Ortak.Banka.Ayarlar.Yazdırma.RenkliHücreler = RenkliHücreler.Checked;
            Ortak.Banka.Ayarlar.Yazdırma.YatayGörünüm = YatayGörünüm.Checked;
            Ortak.Banka.Ayarlar.Yazdırma.KarakterKümesi = KarakterKümeleri.Text;
            Ortak.Banka.Ayarlar.Yazdırma.KarakterBüyüklüğü = (float)KarakterBüyüklüğü.Value;
            Ortak.Banka.Ayarlar.Yazdırma.FirmaLogo_Genişlik = (float)FirmaLogo_Genişlik.Value;
            Ortak.Banka.Ayarlar.Yazdırma.FirmaLogo_Yükseklik = (float)FirmaLogo_Yükseklik.Value;

            Ortak.Banka.Ayarlar.DeğişiklikYapıldı = true;
            Banka_Ortak.DeğişiklikleriKaydet();
            ÖnYüzler_Kaydet.Enabled = false;
        }
        private void Ayar_Değişti(object sender, EventArgs e)
        {
            ÖnYüzler_Kaydet.Enabled = true;

            try
            {
                Yazdır(ÖrnekTabloOluştur());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Text);
            }
        }
        DataGridView ÖrnekTabloOluştur()
        {
            DataGridViewCellStyle ortalanmış = new DataGridViewCellStyle() { Alignment = DataGridViewContentAlignment.MiddleCenter };
            DataGridView Tablo = new DataGridView();
            DataGridViewTextBoxColumn Tablo_MuhatapGrubu = new DataGridViewTextBoxColumn() { HeaderText = "Grup" };
            DataGridViewTextBoxColumn Tablo_Muhatap = new DataGridViewTextBoxColumn() { HeaderText = "Muhatap" };
            DataGridViewTextBoxColumn Tablo_ÖdemeTarihi = new DataGridViewTextBoxColumn() { HeaderText = "Ödeme Günü", DefaultCellStyle = ortalanmış };
            DataGridViewTextBoxColumn Tablo_Tip = new DataGridViewTextBoxColumn() { HeaderText = "Tip" };
            DataGridViewTextBoxColumn Tablo_Durum = new DataGridViewTextBoxColumn() { HeaderText = "Durum" };
            DataGridViewTextBoxColumn Tablo_Miktar = new DataGridViewTextBoxColumn() { HeaderText = "Miktar", DefaultCellStyle = ortalanmış };
            DataGridViewTextBoxColumn Tablo_Notlar = new DataGridViewTextBoxColumn() { HeaderText = "Notlar" };
            DataGridViewTextBoxColumn Tablo_Taksit = new DataGridViewTextBoxColumn() { HeaderText = "Taksit", DefaultCellStyle = ortalanmış };
            DataGridViewCheckBoxColumn Tablo_Üyelik = new DataGridViewCheckBoxColumn() { HeaderText = "Üyelik", DefaultCellStyle = ortalanmış };
            DataGridViewTextBoxColumn Tablo_SonİşlemTarihi = new DataGridViewTextBoxColumn() { HeaderText = "Son İşlem Tarihi", DefaultCellStyle = ortalanmış };
            DataGridViewTextBoxColumn Tablo_KullanıcıAdı = new DataGridViewTextBoxColumn() { HeaderText = "Kullanıcı Adı" };
            Tablo.Columns.AddRange(new DataGridViewColumn[] { Tablo_MuhatapGrubu, Tablo_Muhatap, Tablo_ÖdemeTarihi, Tablo_Tip, Tablo_Durum, Tablo_Miktar, Tablo_Notlar, Tablo_Taksit, Tablo_Üyelik, Tablo_SonİşlemTarihi, Tablo_KullanıcıAdı });

            Ortak.Banka.Ayarlar.CariDökümŞablonlar.TryGetValue(TabloŞablonu.Text, out Banka1.Ayarlar_CariDöküm_Şablon_ şablon);
            if (şablon != null)
            {
                Tablo_Taksit.Visible = şablon.Taksitli;
                Tablo_Üyelik.Visible = şablon.Üyelik;
                Tablo_SonİşlemTarihi.Visible = şablon.TarihTürü >= Banka1.Ayarlar_CariDöküm_Şablon_.TarihTürü_.SonİşlemTarihi;
            }

            int sutun_sayısı = Tablo.ColumnCount;
            int SatırSayısı = Rastgele.Sayı(25, 75);
            DateTime ÖdemeninYapılacağıTarih = DateTime.Now.AddDays(-15), şimdi = DateTime.Now;
            string KullanıcıAdı = "Örnek kullanıcı adı";
            List<DataGridViewRow> dizi = new List<DataGridViewRow>();

            for (int i = 0; i < SatırSayısı; i++)
            {
                _TabloyaEkle_();
            }

            Tablo.Rows.Clear();
            Tablo.Rows.AddRange(dizi.ToArray());
            return Tablo;

            void _TabloyaEkle_()
            {
                Banka1.İşyeri_Ödeme_İşlem_.Tipi_ tipi = (Banka1.İşyeri_Ödeme_İşlem_.Tipi_)Rastgele.Sayı(1, 6);
                Banka1.İşyeri_Ödeme_İşlem_.Durum_ durumu = (Banka1.İşyeri_Ödeme_İşlem_.Durum_)Rastgele.Sayı(1, 6);
                Banka1.İşyeri_Ödeme_.ParaBirimi_ parabirimi = (Banka1.İşyeri_Ödeme_.ParaBirimi_)Rastgele.Sayı(1, 3);
                DateTime? ÜyelikKayıtTarihi = Rastgele.Sayı(1, 10) > 5 ? ÖdemeninYapılacağıTarih : null;
                double miktar = Rastgele.Sayı(0, 999999999);
                string notlar = "Örnek not";
                if ((int)tipi > 2)
                {
                    notlar += Environment.NewLine + Banka_Ortak.Yazdır_GelirGider(Ortak.Banka.Seçilenİşyeri.ToplamGelir, Ortak.Banka.Seçilenİşyeri.ToplamGider);
                }
                if ((int)tipi > 4)
                {
                    notlar += Environment.NewLine + "qwerty asdfghjk zxcvbnmöç 1234567890*-asdfghjklşi," +
                        Environment.NewLine + "qwerty asdfghjk zxcvbnmöç 1234567890*-asdfghjklşi,";
                }

                object[] dizin = new object[sutun_sayısı];
                dizin[Tablo_Tip.Index] = tipi.Yazdır();
                dizin[Tablo_ÖdemeTarihi.Index] = DateOnly.FromDateTime(ÖdemeninYapılacağıTarih);
                dizin[Tablo_Notlar.Index] = notlar;
                dizin[Tablo_SonİşlemTarihi.Index] = ÖdemeninYapılacağıTarih;
                dizin[Tablo_KullanıcıAdı.Index] = KullanıcıAdı;

                if (tipi == Banka1.İşyeri_Ödeme_İşlem_.Tipi_.KontrolNoktası)
                {
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
                    bool gelir_olarak_göster = tipi.GelirMi();
                    bool üyelik_olarak_göster = ÜyelikKayıtTarihi != null;
                    bool gecikti_olarak_göster = !durumu.ÖdendiMi() && ÖdemeninYapılacağıTarih <= şimdi;

                    dizin[Tablo_MuhatapGrubu.Index] = Path.GetRandomFileName();
                    dizin[Tablo_Muhatap.Index] = Path.GetRandomFileName();
                    dizin[Tablo_Durum.Index] = durumu.Yazdır();
                    dizin[Tablo_Miktar.Index] = Banka_Ortak.Yazdır_Ücret(miktar, parabirimi);
                    dizin[Tablo_Taksit.Index] = (int)tipi > 2 ? Rastgele.Sayı(1, 99) + " / 99" : null;
                    dizin[Tablo_Üyelik.Index] = üyelik_olarak_göster;

                    DataGridViewRow _1_satır_dizisi_ = new DataGridViewRow();
                    dizi.Add(_1_satır_dizisi_);
                    _1_satır_dizisi_.CreateCells(Tablo, dizin);
                    _1_satır_dizisi_.Cells[Tablo_ÖdemeTarihi.Index].Style.BackColor = Rastgele.Sayı(1, 10) > 5 ? Ortak.Renk_Mavi : Color.White;
                    _1_satır_dizisi_.Cells[Tablo_SonİşlemTarihi.Index].Style.BackColor = Rastgele.Sayı(1, 10) > 5 ? Ortak.Renk_Mavi : Color.White;

                    _1_satır_dizisi_.Cells[Tablo_Miktar.Index].Style.BackColor = gelir_olarak_göster ? Ortak.Renk_Gelir : Ortak.Renk_Gider;
                    if (gecikti_olarak_göster) _1_satır_dizisi_.Cells[Tablo_ÖdemeTarihi.Index].Style.BackColor = Ortak.Renk_Kırmızı;
                    _1_satır_dizisi_.Cells[Tablo_Durum.Index].Style.BackColor = Cari_Döküm.DurumRengi(durumu);
                }

                ÖdemeninYapılacağıTarih = ÖdemeninYapılacağıTarih.AddDays(1);
            }
        }

        #region Doğrudan Yazdırma Kodu
        class Bir_Yazı_Yazdırma_Detayları_
        {
            public Graphics Grafik;
            public Pen ÇerçeveKalemi;
            public Font KarakterKümesi;
            public Bir_Yazı_ Yazı;
            public StringFormat Şekil = new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };

            public float Sol, Üst, Genişlik, Yükseklik;
            public bool Çerçeve = true;

            public void Yazdır(Brush YazıRengi = null)
            {
                RectangleF r = new RectangleF(Sol, Üst, Genişlik, Yükseklik);

                if (Yazı != null)
                {
                    if (Yazı.ArkaPlanRengi != null) Grafik.FillRectangle(new SolidBrush(Yazı.ArkaPlanRengi.Value), r);

                    Grafik.DrawString(Yazı.Yazı, KarakterKümesi, YazıRengi ?? Brushes.Black, r, Şekil);
                }

                if (Çerçeve) Grafik.DrawRectangle(ÇerçeveKalemi, Sol, Üst, Genişlik, Yükseklik);
            }
        }
        class Bir_Yazı_
        {
            public Color? ArkaPlanRengi;
            public SizeF Boyut;
            public string Yazı;
        }

        class İşlemler_Bir_Satır_Bilgi_
        {
            public Bir_Yazı_[] Yazılar;
            public float EnYüksek_Yükseklik;
        }
        class İşlemler_Bir_Sayfa_
        {
            public float Sol, Üst, Genişlik, Yükseklik, Yükseklik_YazılarİçinKullanılabilir, Başlık_Yüksekliği;
            public Font KaKü, Kakü_Kalın;

            public İşlemler_Bir_Sayfa_Sutun_[] Sutunlar;
            public List<İşlemler_Bir_Satır_Bilgi_> Yazılar = new List<İşlemler_Bir_Satır_Bilgi_>();

            public Bir_Yazı_ SonrakiSayfaYazısı = new Bir_Yazı_();
            public float YazılarİçinToplamYükseklik = 0;
            public int ŞimdikiSayfaSayısı = 1, ToplamSayfaSayısı = 0;

            #region İşlemler
            public float Sutunlar_ToplamGenişlik
            {
                get
                {
                    float Toplam = 0;

                    foreach (İşlemler_Bir_Sayfa_Sutun_ sutun in Sutunlar)
                    {
                        if (sutun == null) continue;
                        Toplam += sutun.Genişlik;
                    }

                    return Toplam;
                }
            }
            #endregion
        }
        class İşlemler_Bir_Sayfa_Sutun_
        {
            public string Adı, EnUzunYazı = "";
            public float Sol, Genişlik;
            public StringFormat Şekli;
        }
        public void Yazdır(DataGridView Tablo, string DosyaAdı = null, short KopyaSayısı = 1)
        {
            İşlemler_Bir_Sayfa_ Sayfa = null;
            PrintDocument pd = new PrintDocument();
            pd.PrintController = new StandardPrintController(); //Yazdırılıyor yazısının gizlenmesi
            pd.OriginAtMargins = true;

            if (DosyaAdı != null)
            {
                Klasör.Oluştur(Path.GetDirectoryName(DosyaAdı));
                pd.PrinterSettings.PrintFileName = DosyaAdı;
            }
            else
            {
                pd.DefaultPageSettings.Landscape = YatayGörünüm.Checked;
                pd.EndPrint += Pd_EndPrint;
                Önizleme.Document = pd;
            }

            pd.PrinterSettings.PrintToFile = DosyayaYazdır.Checked;
            pd.PrinterSettings.PrinterName = Yazcılar.Text;
            pd.PrintPage += İşler_Yazdır_pd;
            if (!pd.PrinterSettings.IsValid) throw new Exception("Yazıcı kullanılamıyor " + pd.PrinterSettings.PrinterName);
            pd.PrinterSettings.Copies = KopyaSayısı;

            if (DosyaAdı != null)
            {
                pd.Print();
                pd.Dispose();

                if (Dosya.BaşkaBirYerdeAçıkMı(DosyaAdı)) throw new Exception("Pdf dosyası oluşturulamadı" + Environment.NewLine + DosyaAdı);
            }

            void İşler_Yazdır_pd(object senderr, PrintPageEventArgs ev)
            {
                //MarginBounds 25,4 25,4 159,258 246,126
                //PageBounds 0 0 210,058 296,926
                //tümü mm olarak
                ev.Graphics.PageUnit = GraphicsUnit.Millimeter;
                ev.Graphics.ResetTransform();
                ev.Graphics.Clear(Color.White);

                if (Sayfa == null)
                {
                    Sayfa = new İşlemler_Bir_Sayfa_();
                    Sayfa.Sol = (ev.PageBounds.X * (float)0.254) + (ev.PageSettings.HardMarginX * (float)0.254) + (float)KenarBoşluğu.Value;
                    Sayfa.Üst = (ev.PageBounds.Y * (float)0.254) + (ev.PageSettings.HardMarginY * (float)0.254) + (float)KenarBoşluğu.Value;
                    Sayfa.Genişlik = (ev.PageBounds.Width * (float)0.254) - (2 * Sayfa.Sol);
                    Sayfa.Yükseklik = (ev.PageBounds.Height * (float)0.254) - (2 * Sayfa.Üst);

                    Sayfa.KaKü = new Font(KarakterKümeleri.Text, (int)KarakterBüyüklüğü.Value, FontStyle.Regular);
                    Sayfa.Kakü_Kalın = new Font(KarakterKümeleri.Text, (int)KarakterBüyüklüğü.Value, FontStyle.Bold);

                    Sayfa.Yükseklik_YazılarİçinKullanılabilir = Sayfa.Yükseklik - (float)FirmaLogo_Yükseklik.Value - ev.Graphics.MeasureString("ŞÇÖĞ", Sayfa.Kakü_Kalın).Height /*Başlık*/;
                    if (Sayfa.Yükseklik_YazılarİçinKullanılabilir <= 0) throw new Exception("Yazıcının kullanılabilir sayfa yüksekliği uygun değil, farklı bir yazıcı seçiniz veya ayarları kontrol ediniz").Günlük();

                    İşler_Yazdır_Hesaplat(Sayfa, Tablo, ev.Graphics);
                }

                float YazdırmaKonumu_Üst = Sayfa.Üst, YazdırmaKonumu_Yükseklik = Sayfa.Yükseklik;
                Bir_Yazı_Yazdırma_Detayları_ y = new Bir_Yazı_Yazdırma_Detayları_();
                y.Grafik = ev.Graphics;
                y.ÇerçeveKalemi = new Pen(Color.Black, 0.1F) { DashStyle = System.Drawing.Drawing2D.DashStyle.Solid };
                y.Yazı = new Bir_Yazı_();

                //logo
                ev.Graphics.DrawImage(Ortak.Firma_Logo, Sayfa.Sol, Sayfa.Üst, (float)FirmaLogo_Genişlik.Value, (float)FirmaLogo_Yükseklik.Value);

                //Yazdırma zamanı
                SizeF s1 = new SizeF(Sayfa.Genişlik - (float)FirmaLogo_Genişlik.Value, (float)FirmaLogo_Yükseklik.Value);
                y.KarakterKümesi = Sayfa.Kakü_Kalın;
                y.Yazı.Yazı = Banka_Ortak.Yazdır_Tarih(DateTime.Now.Yazıya());
                y.Yazı.Boyut = ev.Graphics.MeasureString(y.Yazı.Yazı, y.KarakterKümesi, s1);
                y.Sol = Sayfa.Sol + (Sayfa.Genişlik - y.Yazı.Boyut.Width);
                y.Üst = Sayfa.Üst;
                y.Genişlik = y.Yazı.Boyut.Width;
                y.Yükseklik = y.Yazı.Boyut.Height;
                y.Çerçeve = false;
                y.Yazdır();

                #region FirmaAdı
                s1 = new SizeF(Sayfa.Genişlik - (float)FirmaLogo_Genişlik.Value - y.Yazı.Boyut.Width, (float)FirmaLogo_Yükseklik.Value);
                y.Yazı.Yazı = Ortak.Banka.Seçilenİşyeri.İşyeriAdı;
                y.KarakterKümesi = KarakterKümesi_BoyutunuAyarla(y.Grafik, y.Yazı.Yazı, s1, Sayfa.Kakü_Kalın);
                y.Yazı.Boyut = ev.Graphics.MeasureString(y.Yazı.Yazı, y.KarakterKümesi, s1);
                y.Sol = Sayfa.Sol + (float)FirmaLogo_Genişlik.Value;
                y.Üst = Sayfa.Üst;
                y.Genişlik = s1.Width;
                y.Yükseklik = s1.Height;
                y.Yazdır();
                YazdırmaKonumu_Üst += y.Yükseklik;
                YazdırmaKonumu_Yükseklik -= y.Yükseklik;
                #endregion

                #region Çerçeveler
                //Pen k = new Pen(Color.Red, 0.1f);
                ev.Graphics.DrawRectangle(y.ÇerçeveKalemi, Sayfa.Sol, Sayfa.Üst, Sayfa.Genişlik, Sayfa.Yükseklik); //dış çerçeve
                #endregion

                #region Başlıklar
                y.KarakterKümesi = Sayfa.Kakü_Kalın;
                y.Üst = YazdırmaKonumu_Üst;
                y.Yükseklik = Sayfa.Başlık_Yüksekliği;
                y.Şekil.Alignment = StringAlignment.Center;
                y.Çerçeve = true;

                for (int x = 0; x < Sayfa.Sutunlar.Length; x++)
                {
                    if (Sayfa.Sutunlar[x] == null) continue;

                    y.Yazı.Yazı = Sayfa.Sutunlar[x].Adı;
                    y.Sol = Sayfa.Sutunlar[x].Sol;
                    y.Genişlik = Sayfa.Sutunlar[x].Genişlik;
                    y.Yazdır();
                }

                YazdırmaKonumu_Üst += y.Yükseklik;
                YazdırmaKonumu_Yükseklik -= y.Yükseklik;
                #endregion

                #region işlemler
                y.KarakterKümesi = Sayfa.KaKü;
                y.Çerçeve = true;

                while (Sayfa.Yazılar.Count > 0)
                {
                    if (Sayfa.Yazılar[0].EnYüksek_Yükseklik + Sayfa.SonrakiSayfaYazısı.Boyut.Height > YazdırmaKonumu_Yükseklik)
                    {
                        //daha fazla yazdırılacak iş var, sonraki sayfaya geç
                        SonrakiSayfaYazısınıYazdır();
                        ev.HasMorePages = true;
                        return;
                    }

                    y.Üst = YazdırmaKonumu_Üst;
                    y.Yükseklik = Sayfa.Yazılar[0].EnYüksek_Yükseklik;

                    for (int x = 0; x < Sayfa.Sutunlar.Length; x++)
                    {
                        if (Sayfa.Sutunlar[x] == null) continue;

                        y.Şekil = Sayfa.Sutunlar[x].Şekli;
                        y.Yazı = Sayfa.Yazılar[0].Yazılar[x];
                        y.Sol = Sayfa.Sutunlar[x].Sol;
                        y.Genişlik = Sayfa.Sutunlar[x].Genişlik;
                        y.Yazdır();
                    }

                    YazdırmaKonumu_Üst += y.Yükseklik;
                    YazdırmaKonumu_Yükseklik -= y.Yükseklik;
                    Sayfa.Yazılar.RemoveAt(0);
                }

                y.KarakterKümesi = Sayfa.KaKü;
                SonrakiSayfaYazısınıYazdır();
                #endregion

                void SonrakiSayfaYazısınıYazdır()
                {
                    y.Çerçeve = false;

                    if (y.Yazı == null) y.Yazı = new Bir_Yazı_();
                    y.Yazı.Yazı = Sayfa.SonrakiSayfaYazısı.Yazı.Replace("_ArGeMuP_", (Sayfa.ŞimdikiSayfaSayısı++).ToString());
                    y.Yazı.Boyut = ev.Graphics.MeasureString(y.Yazı.Yazı, y.KarakterKümesi, new SizeF(Sayfa.Genişlik, Sayfa.SonrakiSayfaYazısı.Boyut.Height));

                    y.Sol = Sayfa.Sol;
                    y.Üst = Sayfa.Üst + Sayfa.Yükseklik - y.Yazı.Boyut.Height;
                    y.Genişlik = Sayfa.Genişlik;
                    y.Yükseklik = y.Yazı.Boyut.Height;
                    y.Yazdır();
                }
            }
            void Pd_EndPrint(object senderr, PrintEventArgs ee)
            {
                Önizleme.Rows = Sayfa.ToplamSayfaSayısı;
            }
        }
        Font KarakterKümesi_BoyutunuAyarla(Graphics g, string Yazı, SizeF Alan, Font KarakterKümesi)
        {
            SizeF GerçekBoyutu = g.MeasureString(Yazı, KarakterKümesi);
            float Oran_Yükseklik = Alan.Height / GerçekBoyutu.Height;
            float Oran_Genişlik = Alan.Width / GerçekBoyutu.Width;
            float Ölçek = (Oran_Yükseklik < Oran_Genişlik) ? Oran_Yükseklik : Oran_Genişlik;
            float YeniBoyut = KarakterKümesi.Size * Ölçek;
            return new Font(KarakterKümesi.FontFamily, YeniBoyut);
        }
        void İşler_Yazdır_Hesaplat(İşlemler_Bir_Sayfa_ Sayfa, DataGridView Tablo, Graphics Grafik)
        {
            Sayfa.Başlık_Yüksekliği = Grafik.MeasureString("ÖÇŞĞ", Sayfa.Kakü_Kalın).Height;

            //Sutunlarınn bölüştürülmesi
            int SutunSayısı = Tablo.ColumnCount;
            int SutunNo_Notlar = -1, SutunNo_Durum = -1;
            Sayfa.Sutunlar = new İşlemler_Bir_Sayfa_Sutun_[SutunSayısı];
            for (int x = 0; x < Tablo.Columns.Count; x++)
            {
                DataGridViewColumn sutun = Tablo.Columns[x];
                if (!sutun.Visible) continue;

                if (sutun.HeaderText == "Notlar") SutunNo_Notlar = x;
                if (sutun.HeaderText == "Durum") SutunNo_Durum = x;

                İşlemler_Bir_Sayfa_Sutun_ BirSutun = new İşlemler_Bir_Sayfa_Sutun_();
                Sayfa.Sutunlar[x] = BirSutun;
                BirSutun.Adı = sutun.HeaderText;
                BirSutun.Şekli = new StringFormat() { LineAlignment = StringAlignment.Center };
                if (sutun.DefaultCellStyle != null && sutun.DefaultCellStyle.Alignment == DataGridViewContentAlignment.MiddleCenter)
                {
                    BirSutun.Şekli.Alignment = StringAlignment.Center;
                }
                else BirSutun.Şekli.Alignment = StringAlignment.Near;

                float ToplamGenişlik = Grafik.MeasureString(BirSutun.Adı, Sayfa.Kakü_Kalın).Width;
                Sayfa.Sutunlar[x].EnUzunYazı = BirSutun.Adı;
                float EnUzunYazıGenişliği = 0;
                for (int y = 0; y < Tablo.RowCount; y++)
                {
                    if (Tablo[sutun.Index, y].Value == null) continue;

                    string Yazı;
                    object içerik = Tablo[sutun.Index, y].Value;

                    if (içerik is DateOnly) Yazı = ((DateOnly)içerik).Yazıya();
                    else if (içerik is DateTime) Yazı = ((DateTime)içerik).Yazıya();
                    else if (içerik is bool) Yazı = (bool)içerik ? "✓" : "";
                    else Yazı = (string)içerik;

                    float genişlik = Grafik.MeasureString(Yazı, Sayfa.KaKü).Width;
                    if (genişlik > ToplamGenişlik) ToplamGenişlik = genişlik;

                    if (genişlik > EnUzunYazıGenişliği)
                    {
                        EnUzunYazıGenişliği = genişlik;
                        BirSutun.EnUzunYazı = Yazı;
                    }
                }

                Sayfa.Sutunlar[x].Genişlik = ToplamGenişlik;
            }

            //büyük veya küçük ise orantılı olarak genişletme veya daraltma
            float fark_katsayısı = Sayfa.Genişlik / Sayfa.Sutunlar_ToplamGenişlik;
            float sol_nokta = Sayfa.Sol;
            for (int x = 0; x < Sayfa.Sutunlar.Length; x++)
            {
                if (Sayfa.Sutunlar[x] == null) continue;

                Sayfa.Sutunlar[x].Genişlik = Sayfa.Sutunlar[x].Genişlik * fark_katsayısı;

                Sayfa.Sutunlar[x].Sol = sol_nokta;
                sol_nokta += Sayfa.Sutunlar[x].Genişlik;
            }

            //bölüştürülen sutun genişliklerine göre tekrar yazdırıp yeni ölçüleri bulmak 
            SizeF s = new SizeF(1000, 1000);//a4 ten büyük
            for (int x = 0; x < Sayfa.Sutunlar.Length; x++)
            {
                if (Sayfa.Sutunlar[x] == null) continue;

                s.Width = Sayfa.Sutunlar[x].Genişlik;
                Sayfa.Sutunlar[x].Genişlik = Grafik.MeasureString(Sayfa.Sutunlar[x].EnUzunYazı, Sayfa.KaKü, s).Width;
            }

            //2. sefer - büyük veya küçük ise orantılı olarak genişletme veya daraltma
            fark_katsayısı = Sayfa.Genişlik / Sayfa.Sutunlar_ToplamGenişlik;
            sol_nokta = Sayfa.Sol;
            for (int x = 0; x < Sayfa.Sutunlar.Length; x++)
            {
                if (Sayfa.Sutunlar[x] == null) continue;

                Sayfa.Sutunlar[x].Genişlik = Sayfa.Sutunlar[x].Genişlik * fark_katsayısı;

                Sayfa.Sutunlar[x].Sol = sol_nokta;
                sol_nokta += Sayfa.Sutunlar[x].Genişlik;
            }

            //işlemlerin eklenmesi
            s = new SizeF(1000, 1000);//a4 ten büyük
            for (int y = 0; y < Tablo.RowCount; y++)
            {
                İşlemler_Bir_Satır_Bilgi_ İşlemler_Bir_Satır_Bilgi = new İşlemler_Bir_Satır_Bilgi_();
                Sayfa.Yazılar.Add(İşlemler_Bir_Satır_Bilgi);
                İşlemler_Bir_Satır_Bilgi.Yazılar = new Bir_Yazı_[SutunSayısı];

                for (int x = 0; x < Sayfa.Sutunlar.Length; x++)
                {
                    if (Sayfa.Sutunlar[x] == null || Tablo[x, y].Value == null) continue;

                    İşlemler_Bir_Satır_Bilgi.Yazılar[x] = new Bir_Yazı_();
                    object içerik = Tablo[x, y].Value;
                    if (içerik is DateOnly) İşlemler_Bir_Satır_Bilgi.Yazılar[x].Yazı = ((DateOnly)içerik).Yazıya();
                    else if (içerik is DateTime) İşlemler_Bir_Satır_Bilgi.Yazılar[x].Yazı = ((DateTime)içerik).Yazıya();
                    else if (içerik is bool) İşlemler_Bir_Satır_Bilgi.Yazılar[x].Yazı = (bool)içerik ? "✓" : "";
                    else İşlemler_Bir_Satır_Bilgi.Yazılar[x].Yazı = (string)içerik;
                    s.Width = Sayfa.Sutunlar[x].Genişlik;
                    İşlemler_Bir_Satır_Bilgi.Yazılar[x].Boyut = Grafik.MeasureString(İşlemler_Bir_Satır_Bilgi.Yazılar[x].Yazı, Sayfa.KaKü, s);

                    if (İşlemler_Bir_Satır_Bilgi.EnYüksek_Yükseklik < İşlemler_Bir_Satır_Bilgi.Yazılar[x].Boyut.Height)
                    {
                        İşlemler_Bir_Satır_Bilgi.EnYüksek_Yükseklik = İşlemler_Bir_Satır_Bilgi.Yazılar[x].Boyut.Height;
                    }
                    Sayfa.YazılarİçinToplamYükseklik += İşlemler_Bir_Satır_Bilgi.EnYüksek_Yükseklik;

                    if (RenkliHücreler.Checked &&
                        Tablo[x, y].Style != null &&
                        !Tablo[x, y].Style.BackColor.IsEmpty)
                    {
                        İşlemler_Bir_Satır_Bilgi.Yazılar[x].ArkaPlanRengi = Tablo[x, y].Style.BackColor;
                    }
                }
            }

            //özetin eklenmesi
            do
            {
                //ayraç sadece -
                İşlemler_Bir_Satır_Bilgi_ İşlemler_Bir_Satır_Bilgi = new İşlemler_Bir_Satır_Bilgi_();
                Sayfa.Yazılar.Add(İşlemler_Bir_Satır_Bilgi);
                İşlemler_Bir_Satır_Bilgi.Yazılar = new Bir_Yazı_[SutunSayısı];
                for (int x = 0; x < Sayfa.Sutunlar.Length; x++)
                {
                    if (Sayfa.Sutunlar[x] == null) continue;

                    İşlemler_Bir_Satır_Bilgi.Yazılar[x] = new Bir_Yazı_();
                    İşlemler_Bir_Satır_Bilgi.Yazılar[x].Yazı = x == SutunNo_Durum ? "Özet" : Sayfa.Sutunlar[x].Şekli.Alignment == StringAlignment.Center ? "-" : "";
                    s.Width = Sayfa.Sutunlar[x].Genişlik;
                    İşlemler_Bir_Satır_Bilgi.Yazılar[x].Boyut = Grafik.MeasureString(İşlemler_Bir_Satır_Bilgi.Yazılar[x].Yazı, Sayfa.KaKü, s);
                }
                İşlemler_Bir_Satır_Bilgi.EnYüksek_Yükseklik = İşlemler_Bir_Satır_Bilgi.Yazılar[SutunNo_Durum].Boyut.Height;
                Sayfa.YazılarİçinToplamYükseklik += İşlemler_Bir_Satır_Bilgi.EnYüksek_Yükseklik;

                //özet tümü
                İşlemler_Bir_Satır_Bilgi = new İşlemler_Bir_Satır_Bilgi_();
                Sayfa.Yazılar.Add(İşlemler_Bir_Satır_Bilgi);
                İşlemler_Bir_Satır_Bilgi.Yazılar = new Bir_Yazı_[SutunSayısı];
                İşlemler_Bir_Satır_Bilgi.Yazılar[SutunNo_Durum] = new Bir_Yazı_();
                İşlemler_Bir_Satır_Bilgi.Yazılar[SutunNo_Durum].Yazı = "Tümü";
                s.Width = Sayfa.Sutunlar[SutunNo_Durum].Genişlik;
                İşlemler_Bir_Satır_Bilgi.Yazılar[SutunNo_Durum].Boyut = Grafik.MeasureString(İşlemler_Bir_Satır_Bilgi.Yazılar[SutunNo_Durum].Yazı, Sayfa.KaKü, s);
                İşlemler_Bir_Satır_Bilgi.Yazılar[SutunNo_Notlar] = new Bir_Yazı_();
                İşlemler_Bir_Satır_Bilgi.Yazılar[SutunNo_Notlar].Yazı = Banka_Ortak.Yazdır_GelirGider(Ortak.Banka.Seçilenİşyeri.ToplamGelir, Ortak.Banka.Seçilenİşyeri.ToplamGider);
                s.Width = Sayfa.Sutunlar[SutunNo_Notlar].Genişlik;
                İşlemler_Bir_Satır_Bilgi.Yazılar[SutunNo_Notlar].Boyut = Grafik.MeasureString(İşlemler_Bir_Satır_Bilgi.Yazılar[SutunNo_Notlar].Yazı, Sayfa.KaKü, s);
                İşlemler_Bir_Satır_Bilgi.EnYüksek_Yükseklik = İşlemler_Bir_Satır_Bilgi.Yazılar[SutunNo_Notlar].Boyut.Height;
                Sayfa.YazılarİçinToplamYükseklik += İşlemler_Bir_Satır_Bilgi.EnYüksek_Yükseklik;

                //özet tümü
                İşlemler_Bir_Satır_Bilgi = new İşlemler_Bir_Satır_Bilgi_();
                Sayfa.Yazılar.Add(İşlemler_Bir_Satır_Bilgi);
                İşlemler_Bir_Satır_Bilgi.Yazılar = new Bir_Yazı_[SutunSayısı];
                İşlemler_Bir_Satır_Bilgi.Yazılar[SutunNo_Durum] = new Bir_Yazı_();
                İşlemler_Bir_Satır_Bilgi.Yazılar[SutunNo_Durum].Yazı = "Ödenen";
                s.Width = Sayfa.Sutunlar[SutunNo_Durum].Genişlik;
                İşlemler_Bir_Satır_Bilgi.Yazılar[SutunNo_Durum].Boyut = Grafik.MeasureString(İşlemler_Bir_Satır_Bilgi.Yazılar[SutunNo_Durum].Yazı, Sayfa.KaKü, s);
                İşlemler_Bir_Satır_Bilgi.Yazılar[SutunNo_Notlar] = new Bir_Yazı_();
                İşlemler_Bir_Satır_Bilgi.Yazılar[SutunNo_Notlar].Yazı = Banka_Ortak.Yazdır_GelirGider(Ortak.Banka.Seçilenİşyeri.ÖdenmişToplamGelir, Ortak.Banka.Seçilenİşyeri.ÖdenmişToplamGider);
                s.Width = Sayfa.Sutunlar[SutunNo_Notlar].Genişlik;
                İşlemler_Bir_Satır_Bilgi.Yazılar[SutunNo_Notlar].Boyut = Grafik.MeasureString(İşlemler_Bir_Satır_Bilgi.Yazılar[SutunNo_Notlar].Yazı, Sayfa.KaKü, s);
                İşlemler_Bir_Satır_Bilgi.EnYüksek_Yükseklik = İşlemler_Bir_Satır_Bilgi.Yazılar[SutunNo_Notlar].Boyut.Height;
                Sayfa.YazılarİçinToplamYükseklik += İşlemler_Bir_Satır_Bilgi.EnYüksek_Yükseklik;
            } while (false);

            //toplam sayfa sayısı hesabı
            List<İşlemler_Bir_Satır_Bilgi_> Yazılar2 = new List<İşlemler_Bir_Satır_Bilgi_>(Sayfa.Yazılar);
            float Kullanılabilir_Yükseklik = Sayfa.Yükseklik_YazılarİçinKullanılabilir - Grafik.MeasureString("ŞÇÖĞ", Sayfa.KaKü).Height; //sonraki sayfa yazısı
            Sayfa.ToplamSayfaSayısı = 1; float konum = 0;
            while (Yazılar2.Count > 0)
            {
                if (konum + Yazılar2[0].EnYüksek_Yükseklik > Kullanılabilir_Yükseklik)
                {
                    Sayfa.ToplamSayfaSayısı++;
                    konum = 0;
                }
                else
                {
                    konum += Yazılar2[0].EnYüksek_Yükseklik;
                    Yazılar2.RemoveAt(0);
                }
            }

            //Sonraki sayfa yazısının ölçülmesi
            Sayfa.SonrakiSayfaYazısı.Yazı = "Toplam " + (Sayfa.Yazılar.Count - 3/*özet*/) + " işlem, sayfa _ArGeMuP_ / " + Sayfa.ToplamSayfaSayısı + ", #" + System.IO.Path.GetRandomFileName().Replace(".", "");
            Sayfa.SonrakiSayfaYazısı.Boyut = Grafik.MeasureString(Sayfa.SonrakiSayfaYazısı.Yazı, Sayfa.KaKü);
        }
        #endregion
    }
}

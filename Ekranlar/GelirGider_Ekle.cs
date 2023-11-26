using ArgeMup.HazirKod.Ekİşlemler;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Gelir_Gider_Takip.Ekranlar
{
    public partial class GelirGider_Ekle : Form
    {
        Banka1.Muhatap_ Muhatap;
        DateTime? ÜyelikKayıtTarihi;
        bool İlkAçılış = true;
        bool ÜyelikMi
        {
            get
            {
                return Avans_peşinat_taksit_ve_üyelik_ekleyebilir && Üyelik.Enabled && Üyelik.Checked;
            }
        }
        bool Avans_peşinat_taksit_ve_üyelik_ekleyebilir = Ortak.Banka.İzinliMi(Banka1.Ayarlar_Kullanıcılar_İzin.Avans_peşinat_taksit_ve_üyelik_ekleyebilir);

        public GelirGider_Ekle(Banka1.Muhatap_ Muhatap = null, DateTime? ÜyelikKayıtTarihi = null)
        {
            InitializeComponent();

            Taksit_Dönem.SelectedIndex = 2;
            Üyelik_Dönem.SelectedIndex = 2;
            ParaBirimi.SelectedIndex = 0;

            this.Muhatap = Muhatap;
            this.ÜyelikKayıtTarihi = ÜyelikKayıtTarihi;

            Gelir.FlatAppearance.CheckedBackColor = Ortak.Renk_Gelir;
            Avans.FlatAppearance.CheckedBackColor = Ortak.Renk_Sarı;
            Gider.FlatAppearance.CheckedBackColor = Ortak.Renk_Gider;

            ÖdemeTarihi_Değeri.Visible = Avans_peşinat_taksit_ve_üyelik_ekleyebilir;
            ÖdemeTarihi_Yazısı.Visible = Avans_peşinat_taksit_ve_üyelik_ekleyebilir;
            Peşinat.Visible = Avans_peşinat_taksit_ve_üyelik_ekleyebilir;
            Durum.Visible = Avans_peşinat_taksit_ve_üyelik_ekleyebilir;
            Üyelik.Visible = Avans_peşinat_taksit_ve_üyelik_ekleyebilir;
            Üyelik_Grubu.Visible = Avans_peşinat_taksit_ve_üyelik_ekleyebilir;
            Ayraç_Detaylar_Taksit.Panel2Collapsed = !Avans_peşinat_taksit_ve_üyelik_ekleyebilir;
        }
        private void GelirGider_Ekle_Shown(object sender, EventArgs e)
        {
            İlkAçılış = false;

            if (ÜyelikKayıtTarihi == null)
            {
                İşyeri_Grup_Muhatap_Click(null, null);

                Durum.SelectedIndex = (int)Banka1.İşyeri_Ödeme_İşlem_.Durum_.TamÖdendi - 1;
            }
            else
            {
                Durum.SelectedIndex = (int)Banka1.İşyeri_Ödeme_İşlem_.Durum_.Ödenmedi - 1;
                Durum.Enabled = false;

                İşyeri_Grup_Muhatap.Text = Ortak.Banka.Seçilenİşyeri.İşyeriAdı + "          " + Muhatap.GrupAdı + "          " + Muhatap.MuhatapAdı;
                İşyeri_Grup_Muhatap.Enabled = false;

                Banka1.Muhatap_Üyelik_ ÜyelikDetayları = Muhatap.Üyelikler[ÜyelikKayıtTarihi.Value];

                ÖdemeTarihi_Değeri.Value = ÜyelikDetayları.İlkÖdemeninYapılacağıTarih.ToDateTime(new TimeOnly());
                Miktar.Value = (decimal)ÜyelikDetayları.Miktarı;
                ParaBirimi.SelectedIndex = (int)ÜyelikDetayları.ParaBirimi - 1;
                Gelir.Checked = ÜyelikDetayları.Tipi == Banka1.İşyeri_Ödeme_İşlem_.Tipi_.Gelir;
                Notlar.Text = ÜyelikDetayları.Notlar;

                Üyelik.Enabled = false;
                Üyelik.Checked = true;
                Üyelik_Dönem.SelectedIndex = (int)ÜyelikDetayları.Dönemi - 1;
                Üyelik_Dönem_Adet.Value = ÜyelikDetayları.Dönem_Adet;
                Üyelik_BitişTarihi.Checked = ÜyelikDetayları.BitişTarihi != null;
                if (Üyelik_BitişTarihi.Checked) Üyelik_BitişTarihi.Value = ÜyelikDetayları.BitişTarihi.Value.ToDateTime(new TimeOnly());

                if (ÜyelikDetayları.Taksit != null)
                {
                    Taksit_Dönem.SelectedIndex = (int)ÜyelikDetayları.Taksit.Dönemi - 1;
                    Taksit_Dönem_Adet.Value = ÜyelikDetayları.Taksit.Dönem_Adet;
                    Taksit_Adet.Value = ÜyelikDetayları.Taksit.Taksit_Sayısı;
                }

                ÖnYüzler_Kaydet.Enabled = false;
            }
        }
        private void GelirGider_Ekle_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape) Close();
        }

        private void İşyeri_Grup_Muhatap_Click(object sender, EventArgs e)
        {
            Muhatap = null;

            Ortak.Seçtirt(SeçimVeDüzenleme_Ekranı.Türü_.MuhatapGrubu, Seçim_GeriBildirimİşlemi);
        }
        void Seçim_GeriBildirimİşlemi(string GrupAdı, string MuhatapAdı)
        {
            Muhatap = Ortak.Banka.Seçilenİşyeri.Muhatap_Aç(GrupAdı, MuhatapAdı, true);
            if (Muhatap == null && !ÖnYüzler_Kaydet.Enabled)
            {
                Close();
                return;
            }

            İşyeri_Grup_Muhatap.Text = Ortak.Banka.Seçilenİşyeri.İşyeriAdı + "          " + GrupAdı + "          " + MuhatapAdı;

            DateTime tt;
            if (GrupAdı == Banka1.Çalışan_Yazısı)
            {
                Avans.Visible = Ortak.Banka.İzinliMi(Banka1.Ayarlar_Kullanıcılar_İzin.Avans_peşinat_taksit_ve_üyelik_ekleyebilir);
                if (Avans.Visible) Avans.Checked = true;

                tt = Ortak.Banka.Seçilenİşyeri.EnYakınMaaşGünü().ToDateTime(new TimeOnly());
            }
            else
            {
                tt = DateTime.Now;
                tt = new DateTime(tt.Year, tt.Month, tt.Day);
            }
            ÖdemeTarihi_Değeri.Value = tt;

            İşyeri_Grup_Muhatap.Enabled = true;
            ÖnYüzler_Kaydet.Enabled = false;
        }

        private void AyarDeğişti(object sender, EventArgs e)
        {
            if (İlkAçılış) return;

            Üyelik.Enabled = !Avans.Checked;

            ÖdemeTarihi_Değeri.Value = new DateTime(ÖdemeTarihi_Değeri.Value.Year, ÖdemeTarihi_Değeri.Value.Month, ÖdemeTarihi_Değeri.Value.Day);
            İşyeri_Grup_Muhatap.Enabled = false;
            ÖnYüzler_Kaydet.Enabled = true;

            Peşinat.Enabled = Avans_peşinat_taksit_ve_üyelik_ekleyebilir && !ÜyelikMi && !Avans.Checked && Taksit_Adet.Value > 1;
            Durum.Enabled = !ÜyelikMi;
            if (ÜyelikMi || (!Avans.Checked && (int)Taksit_Adet.Value > 1)) Durum.SelectedIndex = (int)Banka1.İşyeri_Ödeme_İşlem_.Durum_.Ödenmedi - 1;

            Banka1.İşyeri_Ödeme_.ParaBirimi_ parabirimi = (Banka1.İşyeri_Ödeme_.ParaBirimi_)(ParaBirimi.SelectedIndex + 1);
            Banka1.Muhatap_Üyelik_.Dönem_ dönem_taksit = (Banka1.Muhatap_Üyelik_.Dönem_)(Taksit_Dönem.SelectedIndex + 1);
            Banka1.Muhatap_Üyelik_.Dönem_ dönem_üyelik = (Banka1.Muhatap_Üyelik_.Dönem_)(Üyelik_Dönem.SelectedIndex + 1);
            Tablo.Rows.Clear();
            Miktar.BackColor = Color.White;

            if (ÜyelikMi)
            {
                Üyelik_Grubu.Enabled = true;
                //Tablo.ReadOnly = true;

                int dönem_no = 1;
                DateTime ödeme_zamanı = ÖdemeTarihi_Değeri.Value;
                DateTime bitiş_tarihi = Üyelik_BitişTarihi.Checked ? Üyelik_BitişTarihi.Value : DateTime.MaxValue;
                while (dönem_no < 6 && ödeme_zamanı < bitiş_tarihi)
                {
                    _Yazdır_(ödeme_zamanı, "Dönem " + dönem_no + " - ");

                    dönem_no++;
                    ödeme_zamanı = Banka_Ortak.SonrakiTarihiHesapla(ödeme_zamanı, dönem_üyelik, (int)Üyelik_Dönem_Adet.Value);
                }
            }
            else
            {
                Üyelik_Grubu.Enabled = false;
                //Tablo.ReadOnly = false;

                _Yazdır_(ÖdemeTarihi_Değeri.Value);
            }

            void _Yazdır_(DateTime _başlangıç_, string Açıklama = null)
            {
                if (Taksit_Adet.Value == 1)
                {
                    Tablo.Rows.Add(new object[] { Açıklama + "1", Banka_Ortak.Yazdır_Tarih(_başlangıç_.Yazıya()), Banka_Ortak.Yazdır_Ücret((double)Miktar.Value, parabirimi) });
                }
                else
                {
                    double taksit_1_dönem_için = (double)Miktar.Value;
                    if (Peşinat.Enabled) taksit_1_dönem_için -= (double)PeşinatMiktarı.Value;

                    taksit_1_dönem_için = taksit_1_dönem_için / (double)Taksit_Adet.Value;
                    DateTime tt = _başlangıç_;
                    for (int i = 0; i < Taksit_Adet.Value; i++)
                    {
                        Tablo.Rows.Add(new object[] { Açıklama + (i + 1), Banka_Ortak.Yazdır_Tarih(tt.Yazıya()), Banka_Ortak.Yazdır_Ücret(taksit_1_dönem_için, parabirimi) });
                        tt = Banka_Ortak.SonrakiTarihiHesapla(tt, dönem_taksit, (int)Taksit_Dönem_Adet.Value);
                    }
                }
            }
        }
        private void Tablo_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0 || e.ColumnIndex <= Tablo_Taksit.Index) return;

                double toplam = 0;
                for (int i = 0; i < Tablo.Rows.Count; i++)
                {
                    Tablo[Tablo_ÖdemeTarihi.Index, i].Style.BackColor = Color.White;

                    toplam += ((string)Tablo[Tablo_Miktarı.Index, i].Value).NoktalıSayıya();
                }
                Miktar.BackColor = toplam == (double)Miktar.Value ? Color.White : Color.Salmon;

                for (int i = 1; i < Tablo.Rows.Count; i++)
                {
                    string a = (Tablo[Tablo_ÖdemeTarihi.Index, i].Value as string);
                    string b = (Tablo[Tablo_ÖdemeTarihi.Index, i - 1].Value as string);

                    bool hatalı = false;
                    if (a.Length != 10 || b.Length != 10) hatalı = true;
                    else hatalı = a.TarihSaate("dd.MM.yyyy") <= b.TarihSaate("dd.MM.yyyy");
                    if (hatalı)
                    {
                        Tablo[Tablo_ÖdemeTarihi.Index, i].Style.BackColor = Color.Salmon;
                        Tablo[Tablo_ÖdemeTarihi.Index, i - 1].Style.BackColor = Color.Salmon;
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Girdiğiniz içerik uygun değil, kontrol ediniz.", Text);
                return;
            }
        }

        private void ÖnYüzler_Kaydet_Click(object sender, EventArgs e)
        {
            Notlar.Text = Notlar.Text.Trim();
            if (Notlar.Text.BoşMu())
            {
                Notlar.Text = null;
                MessageBox.Show("Notlar kısmını doldurunuz", "Ödeme Ekranı");
                Notlar.Focus();
                return;
            }

            if (Peşinat.Enabled)
            {
                if (PeşinatMiktarı.Value >= Miktar.Value)
                {
                    PeşinatMiktarı.BackColor = Color.Salmon;
                    MessageBox.Show("Peşinat ödeme miktarından az olmalı", "Ödeme Ekranı");
                    PeşinatMiktarı.Focus();
                    return;
                }
                else PeşinatMiktarı.BackColor = Color.White;
            }
            else PeşinatMiktarı.Value = 0;

            Banka1.İşyeri_Ödeme_.ParaBirimi_ parabirimi = (Banka1.İşyeri_Ödeme_.ParaBirimi_)(ParaBirimi.SelectedIndex + 1);
            Banka1.Muhatap_Üyelik_.Dönem_ dönem_taksit = (Banka1.Muhatap_Üyelik_.Dönem_)(Taksit_Dönem.SelectedIndex + 1);
            Banka1.Muhatap_Üyelik_.Dönem_ dönem_üyelik = (Banka1.Muhatap_Üyelik_.Dönem_)(Üyelik_Dönem.SelectedIndex + 1);
            Banka1.İşyeri_Ödeme_İşlem_.Durum_ durum = Durum.Enabled ? (Banka1.İşyeri_Ödeme_İşlem_.Durum_)(Durum.SelectedIndex + 1) : Banka1.İşyeri_Ödeme_İşlem_.Durum_.Ödenmedi;

            double miktar = (double)Miktar.Value;
            Banka1.İşyeri_Ödeme_İşlem_.Tipi_ Tipi = Gelir.Checked ? Banka1.İşyeri_Ödeme_İşlem_.Tipi_.Gelir : Gider.Checked ? Banka1.İşyeri_Ödeme_İşlem_.Tipi_.Gider : Banka1.İşyeri_Ödeme_İşlem_.Tipi_.AvansVerilmesi;

            if (ÜyelikKayıtTarihi == null)
            {
                //Yeni kayıt işi
                DialogResult Dr = MessageBox.Show("Yeni bir kayıt oluşturulacak. İşleme devam etmek istiyor musunuz?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (Dr == DialogResult.No) return;

                if (ÜyelikMi)
                {
                    Muhatap.Üyelik_Ekle(Tipi, miktar, parabirimi, ÖdemeTarihi_Değeri.Value, Notlar.Text,
                        (int)Taksit_Adet.Value, dönem_taksit, (int)Taksit_Dönem_Adet.Value,
                        dönem_üyelik, (int)Üyelik_Dönem_Adet.Value, Üyelik_BitişTarihi.Checked ? Üyelik_BitişTarihi.Value : null);
                }
                else
                {
                    List<Banka1.İşyeri_Ödeme_> ödemeler;
                    DateTime? KayıtTarihi = DateTime.Now;

                    if (Avans.Checked)
                    {
                        //1 kerede çıkan paranın ödemesi
                        ödemeler = Muhatap.GelirGider_Oluştur(Tipi, durum, miktar, parabirimi, KayıtTarihi.Value, Notlar.Text,
                            1, dönem_taksit, 0,
                            null, null, KayıtTarihi);

                        //taksitle girecek olan paranın ödemeleri
                        ödemeler.AddRange(
                            Muhatap.GelirGider_Oluştur(Banka1.İşyeri_Ödeme_İşlem_.Tipi_.AvansÖdemesi, Banka1.İşyeri_Ödeme_İşlem_.Durum_.Ödenmedi, miktar, parabirimi, ÖdemeTarihi_Değeri.Value, Notlar.Text,
                                (int)Taksit_Adet.Value, dönem_taksit, (int)Taksit_Dönem_Adet.Value,
                                null, null, KayıtTarihi));
                    }
                    else
                    {
                        if (PeşinatMiktarı.Value > 0)
                        {
                            ödemeler = Muhatap.GelirGider_Oluştur(Tipi, Banka1.İşyeri_Ödeme_İşlem_.Durum_.PeşinatÖdendi,
                                (double)PeşinatMiktarı.Value, parabirimi, ÖdemeTarihi_Değeri.Value, Notlar.Text,
                                0, Banka1.Muhatap_Üyelik_.Dönem_.Boşta, 0,
                                null, null, KayıtTarihi);

                            miktar -= (double)PeşinatMiktarı.Value;
                            ÖdemeTarihi_Değeri.Value = Banka_Ortak.SonrakiTarihiHesapla(ÖdemeTarihi_Değeri.Value, dönem_taksit, (int)Taksit_Dönem_Adet.Value);
                        }
                        else ödemeler = new List<Banka1.İşyeri_Ödeme_>();

                        ödemeler.AddRange(Muhatap.GelirGider_Oluştur(Tipi, durum, miktar, parabirimi, ÖdemeTarihi_Değeri.Value, Notlar.Text,
                            (int)Taksit_Adet.Value, dönem_taksit, (int)Taksit_Dönem_Adet.Value,
                            null, null, KayıtTarihi));
                    }

                    Muhatap.GelirGider_Ekle(ödemeler);
                }
            }
            else
            {
                //Üyeliği düzenleme işi
                DialogResult Dr = MessageBox.Show("Mevcut üyelik değiştirilecek. İşleme devam etmek istiyor musunuz?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (Dr == DialogResult.No) return;

                Muhatap.Üyelik_Ekle(Tipi, miktar, parabirimi, ÖdemeTarihi_Değeri.Value, Notlar.Text,
                       (int)Taksit_Adet.Value, dönem_taksit, (int)Taksit_Dönem_Adet.Value,
                       dönem_üyelik, (int)Üyelik_Dönem_Adet.Value, Üyelik_BitişTarihi.Checked ? Üyelik_BitişTarihi.Value : null);
                Muhatap.Üyelikler.Remove(ÜyelikKayıtTarihi.Value);

                Önyüz.Dürt();
            }

            Banka_Ortak.DeğişiklikleriKaydet();
            ÖnYüzler_Kaydet.Enabled = false;
            Close();
        }
    }
}

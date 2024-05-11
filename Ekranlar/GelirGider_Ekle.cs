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
        bool AvansMı
        {
            get
            {
                return Avans_peşinat_taksit_ve_üyelik_ekleyebilir && Avans.Enabled && Avans.Checked;
            }
        }
        bool PeşinatMı
        {
            get
            {
                return Avans_peşinat_taksit_ve_üyelik_ekleyebilir && Peşinat.Enabled && PeşinatMiktarı.Value > 0;
            }
        }
        bool Avans_peşinat_taksit_ve_üyelik_ekleyebilir = AnaKontrolcü.İzinliMi(AnaKontrolcü.Kullanıcılar_İzin.Avans_peşinat_taksit_ve_üyelik_ekleyebilir);

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
            Durum_Ödendi.FlatAppearance.CheckedBackColor = Ortak.Renk_Gelir;
            Durum_Ödenmedi.FlatAppearance.CheckedBackColor = Ortak.Renk_Gider;

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

                Durum_Ödendi.Checked = true;

                ParaBirimi.BackColor = Ortak.Renk_Kırmızı;
                Notlar.BackColor = Ortak.Renk_Kırmızı;

                if (AnaKontrolcü.YanUygulamaOlarakÇalışıyor && AnaKontrolcü.Şube_Talep.Kullanıcı_Komut_EkTanım != null && AnaKontrolcü.Şube_Talep.Kullanıcı_Komut_EkTanım.Length == 1)
                {
                    if (AnaKontrolcü.Şube_Talep.Kullanıcı_Komut_EkTanım[0] == "Gelir") Gelir.Checked = true;
                }
            }
            else
            {
                İşyeri_Grup_Muhatap.Text = Ortak.Banka.Seçilenİşyeri.İşyeriAdı + "          " + Muhatap.MuhatapGrubuAdı + "          " + Muhatap.MuhatapAdı;
                İşyeri_Grup_Muhatap.Enabled = false;

                Banka1.Muhatap_Üyelik_ ÜyelikDetayları = Muhatap.Üyelikler[ÜyelikKayıtTarihi.Value];

                ÖdemeTarihi_Değeri.Value = ÜyelikDetayları.İlkÖdemeninYapılacağıTarih.ToDateTime(new TimeOnly());
                Miktar.Value = (decimal)ÜyelikDetayları.Miktarı;
                ParaBirimi.SelectedIndex = (int)ÜyelikDetayları.ParaBirimi - 1;
                Gelir.Checked = ÜyelikDetayları.Tipi == Banka1.İşyeri_Ödeme_İşlem_.Tipi_.Gelir;
                Notlar.Text = ÜyelikDetayları.Notlar;

                Durum.Visible = false;
                Peşinat.Visible = false;
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
            Ayraç_Detaylar_Taksit.Enabled = false;

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
            Ayraç_Detaylar_Taksit.Enabled = true;

            DateTime tt;
            if (GrupAdı == Banka1.Çalışan_Yazısı)
            {
                Avans.Visible = Avans_peşinat_taksit_ve_üyelik_ekleyebilir;

                tt = Ortak.Banka.Seçilenİşyeri.EnYakınMaaşGünü().ToDateTime(new TimeOnly());
            }
            else
            {
                Avans.Visible = false;

                tt = DateTime.Now;
                tt = new DateTime(tt.Year, tt.Month, tt.Day);
            }
            ÖdemeTarihi_Değeri.Value = tt;
            Avans.Checked = Avans.Visible;

            İşyeri_Grup_Muhatap.Enabled = true;
            ÖnYüzler_Kaydet.Enabled = false;
        }

        private void AyarDeğişti_ParaBirimi_Notlar(object sender, EventArgs e)
        {
            (sender as Control).BackColor = SystemColors.Window;

            AyarDeğişti(sender, e);
        }
        private void AyarDeğişti(object sender, EventArgs e)
        {
            if (İlkAçılış) return;

            Üyelik.Enabled = ÜyelikKayıtTarihi == null && !AvansMı;

            ÖdemeTarihi_Değeri.Value = new DateTime(ÖdemeTarihi_Değeri.Value.Year, ÖdemeTarihi_Değeri.Value.Month, ÖdemeTarihi_Değeri.Value.Day);
            İşyeri_Grup_Muhatap.Enabled = false;
            ÖnYüzler_Kaydet.Enabled = Muhatap != null;

            bool ÜyelikVeAvansDeğil = !ÜyelikMi && !AvansMı;
            Peşinat.Enabled = ÜyelikVeAvansDeğil;
            bool TaksitliVePeşinatDadeDeğil = ÜyelikVeAvansDeğil && Taksit_Adet.Value < 2 && (double)PeşinatMiktarı.Value == 0;
            Durum.Enabled = TaksitliVePeşinatDadeDeğil;
            if (!TaksitliVePeşinatDadeDeğil) Durum_Ödenmedi.Checked = true;

            Banka1.İşyeri_Ödeme_.ParaBirimi_ parabirimi = (Banka1.İşyeri_Ödeme_.ParaBirimi_)(ParaBirimi.SelectedIndex + 1);
            Banka1.Muhatap_Üyelik_.Dönem_ dönem_taksit = (Banka1.Muhatap_Üyelik_.Dönem_)(Taksit_Dönem.SelectedIndex + 1);
            Banka1.Muhatap_Üyelik_.Dönem_ dönem_üyelik = (Banka1.Muhatap_Üyelik_.Dönem_)(Üyelik_Dönem.SelectedIndex + 1);
            Tablo.Rows.Clear();
            Miktar.BackColor = Color.White;

            if (ÜyelikMi || ÜyelikKayıtTarihi != null)
            {
                Üyelik_Grubu.Enabled = true;

                int dönem_no = 1;
                DateTime ödeme_zamanı = ÖdemeTarihi_Değeri.Value;
                DateTime bitiş_tarihi = Üyelik_BitişTarihi.Checked ? Üyelik_BitişTarihi.Value : DateTime.MaxValue;
                while (dönem_no < 26 && ödeme_zamanı < bitiş_tarihi)
                {
                    _Yazdır_(ödeme_zamanı, "Dönem " + dönem_no + " - ");

                    dönem_no++;
                    ödeme_zamanı = Banka_Ortak.SonrakiTarihiHesapla(ödeme_zamanı, dönem_üyelik, (int)Üyelik_Dönem_Adet.Value);
                }
            }
            else
            {
                Üyelik_Grubu.Enabled = false;

                _Yazdır_(ÖdemeTarihi_Değeri.Value);
            }

            void _Yazdır_(DateTime _başlangıç_, string Açıklama = null)
            {
                double Miktarı = (double)Miktar.Value;
                DateTime tt = _başlangıç_;

                if (PeşinatMı)
                {
                    Tablo.Rows.Add(new object[] { "Peşinat", Banka_Ortak.Yazdır_Tarih(tt.Yazıya()), Banka_Ortak.Yazdır_Ücret((double)PeşinatMiktarı.Value, parabirimi) });
                    tt = Banka_Ortak.SonrakiTarihiHesapla(tt, dönem_taksit, (int)Taksit_Dönem_Adet.Value);

                    Miktarı -= (double)PeşinatMiktarı.Value;
                }

                Miktarı = Miktarı / (double)Taksit_Adet.Value;
                for (int i = 0; i < Taksit_Adet.Value; i++)
                {
                    Tablo.Rows.Add(new object[] { Açıklama + (i + 1), Banka_Ortak.Yazdır_Tarih(tt.Yazıya()), Banka_Ortak.Yazdır_Ücret(Miktarı, parabirimi) });
                    tt = Banka_Ortak.SonrakiTarihiHesapla(tt, dönem_taksit, (int)Taksit_Dönem_Adet.Value);
                }
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

            if (PeşinatMı)
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

            Banka1.İşyeri_Ödeme_İşlem_.Tipi_ Tipi = Gelir.Checked ? Banka1.İşyeri_Ödeme_İşlem_.Tipi_.Gelir : Gider.Checked ? Banka1.İşyeri_Ödeme_İşlem_.Tipi_.Gider : AvansMı ? Banka1.İşyeri_Ödeme_İşlem_.Tipi_.AvansVerilmesi : Banka1.İşyeri_Ödeme_İşlem_.Tipi_.Boşta;
            if (Tipi == Banka1.İşyeri_Ödeme_İşlem_.Tipi_.Boşta)
            {
                MessageBox.Show("Gelir veya Gider seçeneklerinden birisini seçiniz.", Text);
                return;
            }

            Banka1.İşyeri_Ödeme_.ParaBirimi_ parabirimi = (Banka1.İşyeri_Ödeme_.ParaBirimi_)(ParaBirimi.SelectedIndex + 1);
            Banka1.Muhatap_Üyelik_.Dönem_ dönem_taksit = (Banka1.Muhatap_Üyelik_.Dönem_)(Taksit_Dönem.SelectedIndex + 1);
            Banka1.Muhatap_Üyelik_.Dönem_ dönem_üyelik = (Banka1.Muhatap_Üyelik_.Dönem_)(Üyelik_Dönem.SelectedIndex + 1);
            double miktar = (double)Miktar.Value;

            if (ÜyelikKayıtTarihi == null)
            {
                //Yeni kayıt işi
                DialogResult Dr = MessageBox.Show("Yeni bir kayıt oluşturulacak. İşleme devam etmek istiyor musunuz?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if (Dr == DialogResult.No) return;

                if (ÜyelikMi)
                {
                    Muhatap.Üyelik_Ekle(Tipi, miktar, parabirimi, ÖdemeTarihi_Değeri.Value, Notlar.Text,
                        (int)Taksit_Adet.Value, dönem_taksit, (int)Taksit_Dönem_Adet.Value,
                        dönem_üyelik, (int)Üyelik_Dönem_Adet.Value, Üyelik_BitişTarihi.Checked ? Üyelik_BitişTarihi.Value : null, true);
                }
                else
                {
                    List<Banka1.İşyeri_Ödeme_> ödemeler;
                    DateTime? KayıtTarihi = DateTime.Now;

                    if (AvansMı)
                    {
                        //1 kerede çıkan paranın ödemesi
                        ödemeler = Muhatap.GelirGider_Oluştur(Tipi, Banka1.İşyeri_Ödeme_İşlem_.Durum_.TamÖdendi, miktar, parabirimi, KayıtTarihi.Value, Notlar.Text,
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

                        ödemeler.AddRange(Muhatap.GelirGider_Oluştur(Tipi, Durum_Ödendi.Checked ? Banka1.İşyeri_Ödeme_İşlem_.Durum_.TamÖdendi : Banka1.İşyeri_Ödeme_İşlem_.Durum_.Ödenmedi, miktar, parabirimi, ÖdemeTarihi_Değeri.Value, Notlar.Text,
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

                Muhatap.Üyelik_Düzenle(ÜyelikKayıtTarihi.Value,
                    Tipi, miktar, parabirimi, ÖdemeTarihi_Değeri.Value, Notlar.Text,
                    (int)Taksit_Adet.Value, dönem_taksit, (int)Taksit_Dönem_Adet.Value,
                    dönem_üyelik, (int)Üyelik_Dönem_Adet.Value, Üyelik_BitişTarihi.Checked ? Üyelik_BitişTarihi.Value : null);
            }

            Banka_Ortak.DeğişiklikleriKaydet();
            ÖnYüzler_Kaydet.Enabled = false;
            Close();

            Önyüz.Dürt();
        }
    }
}

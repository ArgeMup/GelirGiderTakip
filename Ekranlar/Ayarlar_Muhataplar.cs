using ArgeMup.HazirKod.Ekİşlemler;
using System;
using System.Threading;
using System.Windows.Forms;

namespace Gelir_Gider_Takip.Ekranlar
{
    public partial class Ayarlar_Muhataplar : Form, IEkran_Dürtü
    {
        Banka1.Muhatap_ Muhatap;
        public void Güncelle()
        {
            Notlar.Text = Muhatap.Notlar;
            Tablo_Üyelik.Rows.Clear();
            if (Muhatap.Üyelikler != null)
            {
                foreach (var üüü in Muhatap.Üyelikler)
                {
                    Banka1.Muhatap_Üyelik_ üyelik = üüü.Value;

                    int satır_no = Tablo_Üyelik.Rows.Add(new object[] {
                                üyelik.İlkÖdemeninYapılacağıTarih,
                                üyelik.Tipi.Yazdır() + " " + Banka_Ortak.Yazdır_Ücret(üyelik.Miktarı, üyelik.ParaBirimi),
                                üyelik.Yazdır_Dönem(),
                                üyelik.BitişTarihi,
                                üyelik.Notlar,
                                üyelik.GerçekleştirenKullanıcıAdı});
                    Tablo_Üyelik[Tablo_Üyelik_İlkÖdemeninYapılacağıTarih.Index, satır_no].ToolTipText = "Kayıt:" + üüü.Key.Yazıya();
                    Tablo_Üyelik.Rows[satır_no].Tag = üüü.Key;
                }
            }

            if (Muhatap.GrupAdı == Banka1.Çalışan_Yazısı)
            {
                //çalışan
                Çalışan_P1.Visible = true;
                Çalışan_P2.Visible = true;
                Ayraç_Diğer_Çalışan.Panel2Collapsed = false;

                Tablo_ÖzlükHakkı.Rows.Clear();
                if (Muhatap.Çalışan != null)
                {
                    İşeGirişTarihi.Value = Muhatap.Çalışan.İşeGirişTarihi.ToDateTime(new TimeOnly());
                    İştenAyrılışTarihi.Checked = Muhatap.Çalışan.İştenAyrılışTarihi != null;
                    if (İştenAyrılışTarihi.Checked) İştenAyrılışTarihi.Value = Muhatap.Çalışan.İştenAyrılışTarihi.Value.ToDateTime(new TimeOnly()); ;

                    Ücret.Value = (decimal)Muhatap.Çalışan.AylıkNetÜcreti;
                    İzin.Value = (decimal)Muhatap.Çalışan.MevcutİzinGünü;

                    foreach (var işl in Muhatap.Çalışan.Geçmişİşlemler)
                    {
                        Tablo_ÖzlükHakkı.Rows.Add(new object[] {
                                işl.Key.Yazıya(),
                                işl.Value.Yazdır_Açıklama(),
                                işl.Value.Notlar,
                                işl.Value.GerçekleştirenKullanıcıAdı});
                    }
                }
                else
                {
                    İşeGirişTarihi.Value = DateTime.Now;
                    İştenAyrılışTarihi.Value = İşeGirişTarihi.Value;
                    İştenAyrılışTarihi.Checked = false;

                    Ücret.Value = 0;
                    Ücret_Notlar = null;
                    İzin.Value = 0;
                    İzin_Notlar = null;
                }
            }
            else
            {
                Çalışan_P1.Visible = false;
                Çalışan_P2.Visible = false;
                Ayraç_Diğer_Çalışan.Panel2Collapsed = true;
            }

            İşyeri_Grup_Muhatap.Enabled = true;
            ÖnYüzler_Kaydet.Enabled = false;
        }

        public Ayarlar_Muhataplar()
        {
            InitializeComponent();

            Ayraç_Diğer_Çalışan.SplitterDistance = Ayraç_Diğer_Çalışan.Height * 40 / 100;
            Ayraç_Çalışan.SplitterDistance = Ayraç_Çalışan.Height * 30 / 100;
        }
        private void Muhatap_Detayları_Shown(object sender, EventArgs e)
        {
            İşyeri_Grup_Muhatap_Click(null, null);
        }
        private void Muhatap_Detayları_KeyPress(object sender, KeyPressEventArgs e)
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

            Güncelle();
        }

        private void SağTuşMenü_Üyelik_Düzenle_Click(object sender, EventArgs e)
        {
            if (Tablo_Üyelik.SelectedRows.Count != 1) return;
            DateTime KayıtTarihi = (DateTime)Tablo_Üyelik.Rows[Tablo_Üyelik.SelectedRows[0].Index].Tag;

            GelirGider_Ekle gge = new GelirGider_Ekle(Muhatap, KayıtTarihi);
            Önyüz.Aç(gge);
        }
        private void SağTuşMenü_Üyelik_Sil_Click(object sender, EventArgs e)
        {
            if (Tablo_Üyelik.SelectedRows.Count != 1) return;
            int SatırNo = Tablo_Üyelik.SelectedRows[0].Index;
            DateTime KayıtTarihi = (DateTime)Tablo_Üyelik.Rows[SatırNo].Tag;

            DialogResult Dr = MessageBox.Show("Seçilen üyelik silinecek. İşleme devam etmek istiyor musunuz?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            if (Dr == DialogResult.No) return;

            Muhatap.Üyelikler.Remove(KayıtTarihi);
            Muhatap.DeğişiklikYapıldı = true;
            Banka_Ortak.DeğişiklikleriKaydet();
            Tablo_Üyelik.Rows.RemoveAt(SatırNo);
        }

        private void AyarDeğişti(object sender, EventArgs e)
        {
            if (Muhatap == null) return;
            İşyeri_Grup_Muhatap.Enabled = false;
            ÖnYüzler_Kaydet.Enabled = true;
        }
        private void Kaydet_Click(object sender, EventArgs e)
        {
            DialogResult Dr = MessageBox.Show("Değişiklikler kaydedilecek. İşleme devam etmek istiyor musunuz?", "Ayarlar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            if (Dr == DialogResult.No) return;

            Muhatap.Notlar = Notlar.Text;

            if (Muhatap.GrupAdı == Banka1.Çalışan_Yazısı)
            {
                if (Muhatap.Çalışan == null) Muhatap.Çalışan = new Banka1.Muhatap_Çalışan_();

                Muhatap.Çalışan.İşeGirişTarihi = DateOnly.FromDateTime(İşeGirişTarihi.Value);
                Muhatap.Çalışan.İştenAyrılışTarihi = İştenAyrılışTarihi.Checked ? DateOnly.FromDateTime(İştenAyrılışTarihi.Value) : null;
            }

            Muhatap.DeğişiklikYapıldı = true;
            Banka_Ortak.DeğişiklikleriKaydet();

            İşyeri_Grup_Muhatap.Enabled = true;
            ÖnYüzler_Kaydet.Enabled = false;
        }

        private void Ücret_Güncelle_Click(object sender, EventArgs e)
        {
            if (Ücret_Notlar.Text.BoşMu(true)) Ücret_Notlar.Text = null;
            if (Ücret_Notlar.Text == "")
            {
                MessageBox.Show("Açıklama giriniz.", "Detayların kaydedilmesi");
                Ücret_Notlar.Focus();
                return;
            }

            DialogResult Dr = MessageBox.Show("Girdiler kaydedilecek. İşleme devam etmek istiyor musunuz?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            if (Dr == DialogResult.No) return;

            if (Muhatap.Çalışan == null) Muhatap.Çalışan = new Banka1.Muhatap_Çalışan_();

            Banka1.Muhatap_Çalışan_ÖzlükHakkı_ öh = new Banka1.Muhatap_Çalışan_ÖzlükHakkı_();
            öh.Türü = Banka1.Muhatap_Çalışan_ÖzlükHakkı_.Türü_.MaaşGüncelleme;
            öh.Mevcut = Muhatap.Çalışan.AylıkNetÜcreti;
            öh.GüncelVeyaKullanım = (double)Ücret.Value;
            öh.Notlar = Ücret_Notlar.Text;
            öh.GerçekleştirenKullanıcıAdı = Ortak.Banka.KullancıAdı;
            DateTime dt = DateTime.Now;
            Muhatap.Çalışan.ÖzlükHakkı_Ekle(öh, dt);

            Muhatap.Çalışan.AylıkNetÜcreti = öh.GüncelVeyaKullanım;
            Muhatap.DeğişiklikYapıldı = true;
            Banka_Ortak.DeğişiklikleriKaydet();

            Ücret_Notlar.Text = null;
            Tablo_ÖzlükHakkı.Rows.Add(new object[] {
                                    dt.Yazıya(),
                                    öh.Yazdır_Açıklama(),
                                    öh.Notlar,
                                    öh.GerçekleştirenKullanıcıAdı});
        }

        private void İzin_Güncelle_Click(object sender, EventArgs e)
        {
            if (İzin_Notlar.Text.BoşMu(true)) İzin_Notlar.Text = null;
            if (İzin_Notlar.Text == "")
            {
                MessageBox.Show("Açıklama giriniz.", "Detayların kaydedilmesi");
                İzin_Notlar.Focus();
                return;
            }

            DialogResult Dr = MessageBox.Show("Girdiler kaydedilecek. İşleme devam etmek istiyor musunuz?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            if (Dr == DialogResult.No) return;

            if (Muhatap.Çalışan == null) Muhatap.Çalışan = new Banka1.Muhatap_Çalışan_();

            Banka1.Muhatap_Çalışan_ÖzlükHakkı_ öh = new Banka1.Muhatap_Çalışan_ÖzlükHakkı_();
            öh.Türü = Banka1.Muhatap_Çalışan_ÖzlükHakkı_.Türü_.İzinGüncelleme;
            öh.Mevcut = Muhatap.Çalışan.MevcutİzinGünü;
            öh.GüncelVeyaKullanım = (double)İzin.Value;
            öh.Notlar = İzin_Notlar.Text;
            öh.GerçekleştirenKullanıcıAdı = Ortak.Banka.KullancıAdı;
            DateTime dt = DateTime.Now;
            Muhatap.Çalışan.ÖzlükHakkı_Ekle(öh, dt);

            Muhatap.Çalışan.MevcutİzinGünü = öh.GüncelVeyaKullanım;
            Muhatap.DeğişiklikYapıldı = true;
            Banka_Ortak.DeğişiklikleriKaydet();

            İzin_Notlar.Text = null;
            Tablo_ÖzlükHakkı.Rows.Add(new object[] {
                                    dt.Yazıya(),
                                    öh.Yazdır_Açıklama(),
                                    öh.Notlar,
                                    öh.GerçekleştirenKullanıcıAdı});
        }
    }
}

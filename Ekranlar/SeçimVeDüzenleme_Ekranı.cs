using ArgeMup.HazirKod.Ekİşlemler;
using ArgeMup.HazirKod.Ekranlar;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Gelir_Gider_Takip.Ekranlar
{
    public partial class SeçimVeDüzenleme_Ekranı : Form
    {
        public enum Türü_ { Boşta, İşyeri, MuhatapGrubu, Muhatap };
        Türü_ Türü;
        Action<string, string> GeriBildirimİşlemi;
        string MuhatapGrubuAdı;
        ListeKutusu.Ayarlar_ ListeKutusu_Ayarlar_İşyeri, ListeKutusu_Ayarlar_MuhatapGrupVeAdı;

        public SeçimVeDüzenleme_Ekranı(Türü_ Türü, Action<string, string> GeriBildirimİşlemi)
        {
            InitializeComponent();

            this.Türü = Türü;
            this.GeriBildirimİşlemi = GeriBildirimİşlemi;

            bool Ayarları_değiştirebilir = AnaKontrolcü.İzinliMi(AnaKontrolcü.Kullanıcılar_İzin.Ayarları_değiştirebilir);
            ListeKutusu_Ayarlar_İşyeri = new ListeKutusu.Ayarlar_(
                            Eklenebilir: Ayarları_değiştirebilir,
                            Silinebilir: Ayarları_değiştirebilir,
                            AdıDeğiştirilebilir: Ayarları_değiştirebilir,
                            Gizlenebilir: Ayarları_değiştirebilir,
                            GizliOlanlarıGöster: Ayarları_değiştirebilir,
                            ElemanKonumu: ListeKutusu.Ayarlar_.ElemanKonumu_.AdanZyeSıralanmış);
            ListeKutusu_Ayarlar_MuhatapGrupVeAdı = new ListeKutusu.Ayarlar_(
                            Eklenebilir: Ayarları_değiştirebilir,
                            Silinebilir: Ayarları_değiştirebilir,
                            AdıDeğiştirilebilir: Ayarları_değiştirebilir,
                            Gizlenebilir: Ayarları_değiştirebilir,
                            GizliOlanlarıGöster: Ayarları_değiştirebilir,
                            ElemanKonumu: Ayarları_değiştirebilir ? ListeKutusu.Ayarlar_.ElemanKonumu_.Değiştirilebilir : ListeKutusu.Ayarlar_.ElemanKonumu_.OlduğuGibi);

            switch (Türü)
            {
                case Türü_.İşyeri:
                    Ayraç.Panel2Collapsed = true;
                    Açıklama.Text = "İşyerini seçiniz";
                    İşyerleriVeMuhatapGrupları.Başlat(null, Ortak.Banka.İşyeri_Listele(),
                        "İşyerleri",
                        ListeKutusu_Ayarlar_İşyeri);
                    break;

                case Türü_.MuhatapGrubu:
                    Açıklama.Text = Ortak.Banka.Seçilenİşyeri.İşyeriAdı + " için" + Environment.NewLine + "Muhatap grubunu seçiniz";
                    List<string> l_sabit = Ortak.Banka.Seçilenİşyeri.MuhatapGrubu_Listele(true);
                    List<string> l_normal = Ortak.Banka.Seçilenİşyeri.MuhatapGrubu_Listele();
                    if (!AnaKontrolcü.İzinliMi(AnaKontrolcü.Kullanıcılar_İzin.Avans_peşinat_taksit_ve_üyelik_ekleyebilir))
                    {
                        l_sabit.Remove(Banka1.Çalışan_Yazısı);
                        l_normal.Remove(Banka1.Çalışan_Yazısı);
                    }
                    İşyerleriVeMuhatapGrupları.Başlat(l_sabit, l_normal,
                        "Muhatap Grupları" + Environment.NewLine + "Maaş ve izin denetimlerini kullanabilmek için Çalışan grubunu oluşturabilirsiniz",
                        ListeKutusu_Ayarlar_MuhatapGrupVeAdı);
                    break;

                default:
                    throw new System.Exception(Text + " Büyük Hata");
            }
        }

        private bool İşyerleriVeMuhatapGrupları_GeriBildirim_İşlemi(string Adı, ArgeMup.HazirKod.Ekranlar.ListeKutusu.İşlemTürü İşlemTürü, string YeniAdı)
        {
            switch (Türü)
            {
                case Türü_.İşyeri:
                    switch (İşlemTürü)
                    {
                        case ListeKutusu.İşlemTürü.YeniEklendi:
                            Ortak.Banka.İşyeri_Ekle(Adı);
                            Banka_Ortak.DeğişiklikleriKaydet();
                            break;

                        case ListeKutusu.İşlemTürü.AdıDeğiştirildi:
                        case ListeKutusu.İşlemTürü.Gizlendi:
                        case ListeKutusu.İşlemTürü.GörünürDurumaGetirildi:
                            Ortak.Banka.İşyeri_AdınıDeğiştir(Adı, YeniAdı);
                            Banka_Ortak.DeğişiklikleriKaydet();
                            break;

                        case ListeKutusu.İşlemTürü.Silindi:
                            DialogResult Dr = MessageBox.Show("Bu işyerinin silinmesiyle altında kayıtlı olan" + Environment.NewLine +
                                "tüm muhataplar ve tüm ödemeler" + Environment.NewLine +
                                "KALICI olarak SİLİNECEKTİR." + Environment.NewLine +
                                "İşleme devam etmek istiyor musunuz?", "İşyerinin Silinmesi", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                            if (Dr == DialogResult.No) return false;

                            Ortak.Banka.İşyeri_Sil(Adı);
                            break;
                    }
                    return true;

                case Türü_.MuhatapGrubu:
                    switch (İşlemTürü)
                    {
                        case ListeKutusu.İşlemTürü.YeniEklendi:
                            Ortak.Banka.Seçilenİşyeri.MuhatapGrubu_Ekle(Adı);
                            Banka_Ortak.DeğişiklikleriKaydet();
                            break;

                        case ListeKutusu.İşlemTürü.ElemanSeçildi:
                            MuhatapGrubuAdı = İşyerleriVeMuhatapGrupları.SeçilenEleman_Adı;
                            if (MuhatapGrubuAdı.BoşMu(true))
                            {
                                Açıklama.Text = Ortak.Banka.Seçilenİşyeri.İşyeriAdı + " için" + Environment.NewLine + "Muhatap grubunu seçiniz";
                                ListeKutusu.Ayarlar_ ListeKutusu_Ayarlar = new ListeKutusu.Ayarlar_();
                                ListeKutusu_Ayarlar.TümTuşlarıKapat();
                                Muhataplar.Başlat(null, null, "Muhataplar", ListeKutusu_Ayarlar);
                                return false;
                            }

                            Açıklama.Text = Ortak.Banka.Seçilenİşyeri.İşyeriAdı + " " + MuhatapGrubuAdı + " için" + Environment.NewLine + "Muhatabı seçiniz";
                            Muhataplar.Başlat(Ortak.Banka.Seçilenİşyeri.Muhatap_Listele(MuhatapGrubuAdı, true), Ortak.Banka.Seçilenİşyeri.Muhatap_Listele(MuhatapGrubuAdı),
                                "Muhataplar",
                                ListeKutusu_Ayarlar_MuhatapGrupVeAdı);
                            break;

                        case ListeKutusu.İşlemTürü.AdıDeğiştirildi:
                        case ListeKutusu.İşlemTürü.Gizlendi:
                        case ListeKutusu.İşlemTürü.GörünürDurumaGetirildi:
                            Ortak.Banka.Seçilenİşyeri.MuhatapGrubu_AdınıDeğiştir(Adı, YeniAdı);
                            Banka_Ortak.DeğişiklikleriKaydet();
                            break;

                        case ListeKutusu.İşlemTürü.KonumDeğişikliğiKaydedildi:
                            Ortak.Banka.Seçilenİşyeri.MuhatapGrubu_SıralamasınıDeğiştir(İşyerleriVeMuhatapGrupları.Tüm_Elemanlar);
                            Banka_Ortak.DeğişiklikleriKaydet();
                            break;

                        case ListeKutusu.İşlemTürü.Silindi:
                            DialogResult Dr = MessageBox.Show("Bu muhatap grubunun silinmesiyle altında kayıtlı olan" + Environment.NewLine +
                                "tüm muhataplar ve onlara ait üyelik ve ayarlar" + Environment.NewLine +
                                "KALICI olarak SİLİNECEKTİR." + Environment.NewLine +
                                "*İlgili ödemeler tutulmaya devam edilecek." + Environment.NewLine +
                                "İşleme devam etmek istiyor musunuz?", "İşyerinin Silinmesi", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                            if (Dr == DialogResult.No) return false;

                            Ortak.Banka.Seçilenİşyeri.MuhatapGrubu_Sil(Adı);
                            Banka_Ortak.DeğişiklikleriKaydet();
                            break;
                    }
                    return true;
            }

            return false;
        }
        private bool Muhataplar_GeriBildirim_İşlemi(string Adı, ArgeMup.HazirKod.Ekranlar.ListeKutusu.İşlemTürü İşlemTürü, string YeniAdı)
        {
            if (MuhatapGrubuAdı.BoşMu()) return false;

            switch (İşlemTürü)
            {
                case ListeKutusu.İşlemTürü.YeniEklendi:
                    Ortak.Banka.Seçilenİşyeri.Muhatap_Ekle(MuhatapGrubuAdı, Adı);
                    Banka_Ortak.DeğişiklikleriKaydet();
                    break;

                case ListeKutusu.İşlemTürü.AdıDeğiştirildi:
                case ListeKutusu.İşlemTürü.Gizlendi:
                case ListeKutusu.İşlemTürü.GörünürDurumaGetirildi:
                    Ortak.Banka.Seçilenİşyeri.Muhatap_AdınıDeğiştir(MuhatapGrubuAdı, Adı, YeniAdı);
                    Banka_Ortak.DeğişiklikleriKaydet();
                    break;

                case ListeKutusu.İşlemTürü.KonumDeğişikliğiKaydedildi:
                    Ortak.Banka.Seçilenİşyeri.Muhatap_SıralamasınıDeğiştir(MuhatapGrubuAdı, Muhataplar.Tüm_Elemanlar);
                    Banka_Ortak.DeğişiklikleriKaydet();
                    break;

                case ListeKutusu.İşlemTürü.Silindi:
                    DialogResult Dr = MessageBox.Show("Bu muhatabın silinmesiyle" + Environment.NewLine +
                                "ona ait olan üyelik ve ayarlar" + Environment.NewLine +
                                "KALICI olarak SİLİNECEKTİR." + Environment.NewLine +
                                "*İlgili ödemeler tutulmaya devam edilecek." + Environment.NewLine +
                                "İşleme devam etmek istiyor musunuz?", "İşyerinin Silinmesi", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                    if (Dr == DialogResult.No) return false;

                    Ortak.Banka.Seçilenİşyeri.Muhatap_Sil(MuhatapGrubuAdı, Adı);
                    Banka_Ortak.DeğişiklikleriKaydet();
                    break;
            }

            return true;
        }

        private void Geri_Click(object sender, EventArgs e)
        {
            GeriBildirimİşlemi(null, null);
            Close();
        }
        private void Seç_Click(object sender, System.EventArgs e)
        {
            switch (Türü)
            {
                case Türü_.İşyeri:
                    if (İşyerleriVeMuhatapGrupları.SeçilenEleman_Adı.BoşMu(true)) return;

                    GeriBildirimİşlemi(İşyerleriVeMuhatapGrupları.SeçilenEleman_Adı, null);
                    Close();
                    break;

                case Türü_.MuhatapGrubu:
                    if (Muhataplar.SeçilenEleman_Adı.BoşMu(true)) return;

                    GeriBildirimİşlemi(MuhatapGrubuAdı, Muhataplar.SeçilenEleman_Adı);
                    Close();
                    break;
            }
        }
    }
}

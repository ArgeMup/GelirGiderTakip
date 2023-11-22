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
        ListeKutusu.Ayarlar_ ListeKutusu_Ayarlar = 
                        new ListeKutusu.Ayarlar_(
                            Eklenebilir: Ortak.Banka.İzinliMi(Banka1.Ayarlar_Kullanıcılar_İzin.Ayarları_değiştirebilir), 
                            Silinebilir: Ortak.Banka.İzinliMi(Banka1.Ayarlar_Kullanıcılar_İzin.Ayarları_değiştirebilir), 
                            GizliOlanlarıGöster: Ortak.Banka.İzinliMi(Banka1.Ayarlar_Kullanıcılar_İzin.Ayarları_değiştirebilir),
                            AdıDeğiştirilebilir: false, ElemanKonumu:ListeKutusu.Ayarlar_.ElemanKonumu_.OlduğuGibi, Gizlenebilir: false);

        public SeçimVeDüzenleme_Ekranı(Türü_ Türü, Action<string, string> GeriBildirimİşlemi)
        {
            InitializeComponent();

            this.Türü = Türü;
            this.GeriBildirimİşlemi = GeriBildirimİşlemi;

            switch (Türü)
            {
                case Türü_.İşyeri:
                    Ayraç.Panel2Collapsed = true;
                    Açıklama.Text = "İşyerini seçiniz";
                    İşyerleriVeMuhatapGrupları.Başlat(null, Ortak.Banka.İşyeri_Listele(),
                        "İşyerleri",
                        ListeKutusu_Ayarlar);
                    break;

                case Türü_.MuhatapGrubu:
                    Açıklama.Text = Ortak.Banka.Seçilenİşyeri.İşyeriAdı + " için" + Environment.NewLine + "Muhatap grubunu seçiniz";
                    List<string> l_sabit = Ortak.Banka.Seçilenİşyeri.MuhatapGrubu_Listele(true);
                    List<string> l_normal = Ortak.Banka.Seçilenİşyeri.MuhatapGrubu_Listele();
                    if (!Ortak.Banka.İzinliMi(Banka1.Ayarlar_Kullanıcılar_İzin.Avans_peşinat_taksit_ve_üyelik_ekleyebilir))
                    {
                        l_sabit.Remove(Banka1.Çalışan_Yazısı);
                        l_normal.Remove(Banka1.Çalışan_Yazısı);
                    }
                    İşyerleriVeMuhatapGrupları.Başlat(l_sabit,l_normal,
                        "Muhatap Grupları" + Environment.NewLine + "Maaş ve izin denetimlerini kullanabilmek için Çalışan grubunu oluşturabilirsiniz",
                        ListeKutusu_Ayarlar);
                    break;

                default:
                    throw new System.Exception(Text + " Büyük Hata");
            }
        }

        //muhatap adı ve grubu adı şimdilik değiştirtme, bu isimlerin nerelerde geçtiği kesinleşince yapılacak

        private bool İşyerleriVeMuhatapGrupları_GeriBildirim_İşlemi(string Adı, ArgeMup.HazirKod.Ekranlar.ListeKutusu.İşlemTürü İşlemTürü, string YeniAdı)
        {
            switch (Türü)
            {
                case Türü_.İşyeri:
                    switch (İşlemTürü)
                    {
                        case ArgeMup.HazirKod.Ekranlar.ListeKutusu.İşlemTürü.YeniEklendi:
                            Ortak.Banka.İşyeri_Ekle(Adı);
                            Banka_Ortak.DeğişiklikleriKaydet();
                            break;

                        //case ArgeMup.HazirKod.ListeKutusu.İşlemTürü.AdıDeğiştirildi:
                        //case ArgeMup.HazirKod.ListeKutusu.İşlemTürü.Gizlendi:
                        //case ArgeMup.HazirKod.ListeKutusu.İşlemTürü.GörünürDurumaGetirildi:
                        //    break;

                        case ArgeMup.HazirKod.Ekranlar.ListeKutusu.İşlemTürü.Silindi:
                            Ortak.Banka.İşyeri_Sil(Adı);
                            break;
                    }
                    return true;

                case Türü_.MuhatapGrubu:
                    switch (İşlemTürü)
                    {
                        case ArgeMup.HazirKod.Ekranlar.ListeKutusu.İşlemTürü.YeniEklendi:
                            Ortak.Banka.Seçilenİşyeri.MuhatapGrubu_Ekle(Adı);
                            Banka_Ortak.DeğişiklikleriKaydet();
                            break;

                        case ArgeMup.HazirKod.Ekranlar.ListeKutusu.İşlemTürü.ElemanSeçildi:
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
                                ListeKutusu_Ayarlar);
                            break;

                        //case ArgeMup.HazirKod.ListeKutusu.İşlemTürü.AdıDeğiştirildi:
                        //case ArgeMup.HazirKod.ListeKutusu.İşlemTürü.Gizlendi:
                        //case ArgeMup.HazirKod.ListeKutusu.İşlemTürü.GörünürDurumaGetirildi:
                        //    break;

                        case ArgeMup.HazirKod.Ekranlar.ListeKutusu.İşlemTürü.Silindi:
                            //DialogResult Dr = MessageBox.Show("Bu grubun silinmesiyle altında katıtlı olan tüm muhataplar da aynı şekilde SİLİNECEKTİR. İşleme devam etmek istiyor musunuz?", "Muhatap Grubunun Silinmesi", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                            //if (Dr == DialogResult.No) return false;

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
                case ArgeMup.HazirKod.Ekranlar.ListeKutusu.İşlemTürü.YeniEklendi:
                    Ortak.Banka.Seçilenİşyeri.Muhatap_Ekle(MuhatapGrubuAdı, Adı);
                    Banka_Ortak.DeğişiklikleriKaydet();
                    break;

                //case ArgeMup.HazirKod.ListeKutusu.İşlemTürü.AdıDeğiştirildi:
                //case ArgeMup.HazirKod.ListeKutusu.İşlemTürü.Gizlendi:
                //case ArgeMup.HazirKod.ListeKutusu.İşlemTürü.GörünürDurumaGetirildi:
                //    break;

                case ArgeMup.HazirKod.Ekranlar.ListeKutusu.İşlemTürü.Silindi:
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

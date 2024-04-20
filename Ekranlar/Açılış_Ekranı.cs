using ArgeMup.HazirKod.Ekİşlemler;
using System;
using System.Linq;
using System.Windows.Forms;

namespace Gelir_Gider_Takip.Ekranlar
{
    public partial class Açılış_Ekranı : Form
    {
        public Açılış_Ekranı()
        {
            InitializeComponent();

            bool Ayarları_değiştirebilir = AnaKontrolcü.İzinliMi(AnaKontrolcü.Kullanıcılar_İzin.Ayarları_değiştirebilir);
            İşyeriSeçimi.Visible = Ayarları_değiştirebilir;
            Cari.Visible = AnaKontrolcü.İzinliMi(new AnaKontrolcü.Kullanıcılar_İzin[] { AnaKontrolcü.Kullanıcılar_İzin.Cari_dökümü_görebilir, AnaKontrolcü.Kullanıcılar_İzin.Cari_döküm_içinde_işlem_yapabilir });
            GelirGiderEkle.Visible = AnaKontrolcü.İzinliMi(new AnaKontrolcü.Kullanıcılar_İzin[] { AnaKontrolcü.Kullanıcılar_İzin.Gelir_gider_ekleyebilir, AnaKontrolcü.Kullanıcılar_İzin.Avans_peşinat_taksit_ve_üyelik_ekleyebilir });
            Ayarlar.Visible = Ayarları_değiştirebilir;
            ParolayıDeğiştir.Visible = !Ayarları_değiştirebilir;
        }
        private void Açılış_Ekranı_Shown(object sender, System.EventArgs e)
        {
            if (Ortak.Banka.Seçilenİşyeri != null) Seçim_GeriBildirimİşlemi(Ortak.Banka.Seçilenİşyeri.İşyeriAdı, null);
            else
            {
                var kvp = Ortak.Banka.İşyerleri.FirstOrDefault(x => !x.Key.StartsWith(Ortak.GizliElemanBaşlangıcı));
                if (kvp.Key.DoluMu()) Seçim_GeriBildirimİşlemi(kvp.Key, null);
                else İşyeriSeçimi_Click(null, null);
            }
        }

        private void İşyeriSeçimi_Click(object sender, System.EventArgs e)
        {
            Ortak.Seçtirt(SeçimVeDüzenleme_Ekranı.Türü_.İşyeri, Seçim_GeriBildirimİşlemi);
        }
        void Seçim_GeriBildirimİşlemi(string İşyeriAdı, string _)
        {
            GelirGiderEkle.Enabled = false;
            Cari.Enabled = false;
            Ayarlar.Enabled = false;

            if (İşyeriAdı.BoşMu()) return;

            İşyeriSeçimi.Text = "İşyeri Seçimi : " + İşyeriAdı;
            Ortak.Banka.Seçilenİşyeri = Ortak.Banka.İşyeri_Aç(İşyeriAdı);

            GelirGiderEkle.Enabled = true;
            Cari.Enabled = true;
            Ayarlar.Enabled = true;
        }

        private void GelirGiderEkle_Click(object sender, System.EventArgs e)
        {
            Önyüz.Aç(new GelirGider_Ekle());
        }
        private void Cari_Click(object sender, System.EventArgs e)
        {
            Önyüz.Aç(new Cari_Döküm());
        }
        private void Ayarlar_Click(object sender, System.EventArgs e)
        {
            Önyüz.Aç(new Ayarlar());
        }
        private void ParolayıDeğiştir_Click(object sender, EventArgs e)
        {
            Önyüz.Aç(ArgeMup.HazirKod.Ekranlar.Kullanıcılar.Önyüz_ParolaDeğiştir(Font.Size));
        }
    }
}

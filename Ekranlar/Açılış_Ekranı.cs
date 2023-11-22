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

            bool Ayarları_değiştirebilir = Ortak.Banka.İzinliMi(Banka1.Ayarlar_Kullanıcılar_İzin.Ayarları_değiştirebilir);
            İşyeriSeçimi.Visible = Ayarları_değiştirebilir;
            Cari.Visible = Ortak.Banka.İzinliMi(new Banka1.Ayarlar_Kullanıcılar_İzin[] { Banka1.Ayarlar_Kullanıcılar_İzin.Cari_dökümü_görebilir, Banka1.Ayarlar_Kullanıcılar_İzin.Cari_döküm_içinde_işlem_yapabilir });
            GelirGiderEkle.Visible = Ortak.Banka.İzinliMi(new Banka1.Ayarlar_Kullanıcılar_İzin[] { Banka1.Ayarlar_Kullanıcılar_İzin.Gelir_gider_ekleyebilir, Banka1.Ayarlar_Kullanıcılar_İzin.Avans_peşinat_taksit_ve_üyelik_ekleyebilir });
            Ayarlar.Visible = Ayarları_değiştirebilir;
            ParolayıDeğiştir.Visible = !Ayarları_değiştirebilir;
        }
        private void Açılış_Ekranı_Shown(object sender, System.EventArgs e)
        {
            if (Ortak.Banka.Seçilenİşyeri != null) İşyeriSeçimi.Text = "İşyeri Seçimi : " + Ortak.Banka.Seçilenİşyeri.İşyeriAdı;
            else
            {
                if (Ortak.Banka.İşyerleri.Count > 0) Seçim_GeriBildirimİşlemi(Ortak.Banka.İşyerleri.First().Key, null);
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
            Önyüz.Aç(new Ayarlar_Kullanıcılar(ArgeMup.HazirKod.Ekranlar.Kullanıcılar.İşlemTürü_.ParolaDeğiştirme));
        }
    }
}

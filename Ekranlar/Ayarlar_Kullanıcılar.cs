using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ArgeMup.HazirKod.Ekİşlemler;
using ArgeMup.HazirKod.Ekranlar;

namespace Gelir_Gider_Takip.Ekranlar
{
    public partial class Ayarlar_Kullanıcılar : Form
    {
        public ArgeMup.HazirKod.Ekranlar.Kullanıcılar2.İşlemTürü_ İşlemTürü;
        public Ayarlar_Kullanıcılar(ArgeMup.HazirKod.Ekranlar.Kullanıcılar2.İşlemTürü_ İşlemTürü)
        {
            InitializeComponent();
            this.İşlemTürü = İşlemTürü;

            switch (İşlemTürü)
            {
                default:
                case ArgeMup.HazirKod.Ekranlar.Kullanıcılar2.İşlemTürü_.Giriş:
                    Önyüz.Durdur();
                    Ekran.GeriBildirim_GirişBaşarılı += Ekran_GeriBildirim_GirişBaşarılı;
                    break;

                case ArgeMup.HazirKod.Ekranlar.Kullanıcılar2.İşlemTürü_.ParolaDeğiştirme:
                    Ekran.GeriBildirim_Değişiklikleri_Kaydet += Ekran_Değişiklikleri_Kaydet_ParolaDeğiştirme;
                    break;

                case ArgeMup.HazirKod.Ekranlar.Kullanıcılar2.İşlemTürü_.Ayarlar:
                    Ekran.GeriBildirim_Değişiklikleri_Kaydet += Ekran_Değişiklikleri_Kaydet_Ayarlar;
                    break;
            }

            List<string> İzinler = new List<string>();
            for (int i = 0; i < (int)Banka1.Ayarlar_Kullanıcılar_İzin.DiziElemanSayısı_; i++) { İzinler.Add(((Banka1.Ayarlar_Kullanıcılar_İzin)i).Yazdır()); }
            Ekran.Başlat(İşlemTürü, İzinler, Ortak.Banka.Ayarlar.Kullanıcılar);
        }
        private void Ayarlar_Kullanıcılar_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape && İşlemTürü != Kullanıcılar2.İşlemTürü_.Giriş) Close();
        }

        private void Ekran_GeriBildirim_GirişBaşarılı()
        {
            İşlemTürü = ArgeMup.HazirKod.Ekranlar.Kullanıcılar2.İşlemTürü_.Boşta;
            Close();
        }
        private void Ekran_Değişiklikleri_Kaydet_ParolaDeğiştirme()
        {
            Ortak.Banka.Ayarlar.DeğişiklikYapıldı = true;
            Banka_Ortak.DeğişiklikleriKaydet();
            Close();
        }
        private void Ekran_Değişiklikleri_Kaydet_Ayarlar()
        {
            if (Ortak.Banka.Ayarlar.Kullanıcılar.ParolaKontrolüGerekiyorMu)
            {
                int AyarlarıDeğiştirebilenKullanıcıSayısı = Ortak.Banka.Ayarlar.Kullanıcılar.Kişiler.Where(x => x.Parolası.DoluMu(true) && x.İzinliMi(Banka1.Ayarlar_Kullanıcılar_İzin.Ayarları_değiştirebilir)).Count();
                if (AyarlarıDeğiştirebilenKullanıcıSayısı < 1)
                {
                    Banka_Ortak.Değişiklikler_TamponuSıfırla();
                    MessageBox.Show("Son değişiklik ile hiçbir kullanıcı bu sayfaya ulaşamayacak." +
                        Environment.NewLine + Environment.NewLine +
                        "Öncelikle ayarları değiştirebilme hakkına sahip bir rol oluşturun" + Environment.NewLine +
                        "Sonra bu rolu parolası olan bir kulanıcıya eşleyin.",
                        "İşlem iptal edildi");
                    Ekran.Yenile(Ortak.Banka.Ayarlar.Kullanıcılar);
                    return;
                }
            }

            Ortak.Banka.Ayarlar.DeğişiklikYapıldı = true;
            Banka_Ortak.DeğişiklikleriKaydet();
        }
    }
}

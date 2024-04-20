using ArgeMup.HazirKod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Gelir_Gider_Takip.Ekranlar
{
    public interface IEkran_Dürtü
    {
        void Güncelle();
    }

    public static class Önyüz
    {
        public static Form AnaEkran;
        public static string SürümKontrolMesajı = "Yeni sürüm kontrol ediliyor";
        static List<Form> Önyüzler = new List<Form>();

        public static void Aç(Form Önyüz)
        {
            Önyüz.MdiParent = AnaEkran;
            Önyüz.TopLevel = false;
            Önyüz.FormBorderStyle = FormBorderStyle.None;
            Önyüz.Dock = DockStyle.Fill;

            Önyüz.FormClosing += Önyüz_FormClosing;
            Önyüz.FormClosed += Önyüz_FormClosed;

            Önyüz.BringToFront();
            Önyüz.Show();

            Önyüzler.Add(Önyüz);

            AnaEkran.Text = "ArGeMuP " + Kendi.Adı + " Kullanıcı : " + (AnaKontrolcü.KullanıcıAdı ?? "Giriş yapılmadı") + " " + SürümKontrolMesajı;

            Günlük.Ekle("Yan uygulama açıldı " + Önyüz.Text);
        }
        public static void Dürt()
        {
            foreach (Form ekran in Önyüzler)
            {
                IEkran_Dürtü dürtü = ekran as IEkran_Dürtü;
                if (dürtü != null) dürtü.Güncelle();
            }
        }
        public static void PencereleriKapat()
        {
            foreach (Form Önyüz in Önyüzler)
            {
                try
                {
                    Önyüz.FormClosing -= Önyüz_FormClosing;
                    Önyüz.FormClosed -= Önyüz_FormClosed;

                    Önyüz.Dispose();
                }
                catch (Exception) { }
            }

            Önyüzler.Clear();

            try
            {
                AnaEkran?.Dispose();
                AnaEkran = null;
            }
            catch (Exception) { }
        }

        private static void Önyüz_FormClosing(object? sender, FormClosingEventArgs e)
        {
            if (Önyüzler.Count > 1 && e.CloseReason == CloseReason.MdiFormClosing)
            {
                Önyüzler.Last().Close();
                e.Cancel = true;
            }
            else
            {
                Form öndeki = sender as Form;
                foreach (Control ÜstEleman in öndeki.Controls)
                {
                    if (_Bul_(ÜstEleman)) break;
                }

                bool _Bul_(Control Eleman)
                {
                    if (Eleman.Name.StartsWith("ÖnYüzler_Kaydet") && Eleman.Enabled)
                    {
                        DialogResult Dr = MessageBox.Show("Değişiklikleri kaydetmeden çıkmak istediğinize emin misiniz?", öndeki.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                        e.Cancel = Dr == DialogResult.No;
                        return true;
                    }

                    foreach (Control AltEleman in Eleman.Controls)
                    {
                        if (_Bul_(AltEleman)) return true;
                    }

                    return false;
                }
            }
        }
        private static void Önyüz_FormClosed(object? sender, FormClosedEventArgs e)
        {
            Form ekran = sender as Form;
            Önyüzler.Remove(ekran);

            if (Önyüzler.Count <= 0) Application.Exit();

            Günlük.Ekle("Yan uygulama kapatıldı " + ekran.Text);
        }
    }
}

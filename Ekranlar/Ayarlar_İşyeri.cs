using System.Windows.Forms;

namespace Gelir_Gider_Takip.Ekranlar
{
    public partial class Ayarlar_İşyeri : Form
    {
        public Ayarlar_İşyeri(ArgeMup.HazirKod.Ekranlar.Kullanıcılar.İşlemTürü_ İşlemTürü)
        {
            InitializeComponent();
        }
        private void Ayarlar_Kullanıcılar_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape) Close();
        }
    }
}

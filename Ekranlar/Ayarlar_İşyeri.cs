﻿using System.Windows.Forms;

namespace Gelir_Gider_Takip.Ekranlar
{
    public partial class Ayarlar_İşyeri : Form
    {
        public Ayarlar_İşyeri()
        {
            InitializeComponent();
        }
        private void Ayarlar_Kullanıcılar_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape) Close();
        }
    }
}

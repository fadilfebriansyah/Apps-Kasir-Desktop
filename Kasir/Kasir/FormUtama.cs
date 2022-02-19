using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kasir
{
    public partial class FormUtama : Form
    {
        public FormUtama()
        {
            InitializeComponent();
        }

        private void txtproduk_Click(object sender, EventArgs e)
        {
            FormProduk frm = new FormProduk();
            frm.Show();
            this.Dispose();
        }

        private void txttransaksi_Click(object sender, EventArgs e)
        {
            FormTransaksi frm = new FormTransaksi();
            frm.Show();
            this.Hide();
        }

        private void btnlogout_Click(object sender, EventArgs e)
        {
            FormLogin flog = new FormLogin();
            flog.Show();
            this.Close();
        }

        private void btnlaporan_Click(object sender, EventArgs e)
        {
            FormLaporan frm = new FormLaporan();
            frm.Show();
            this.Hide();
        }
    }
}

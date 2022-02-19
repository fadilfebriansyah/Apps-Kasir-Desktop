using MySql.Data.MySqlClient;
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
    public partial class FormCari : Form
    {
        MySqlDataReader dr;

        public FormCari()
        {
            InitializeComponent();
        }
        void isiProduk()
        {
            dr = koneksi.OpenDr("SELECT kode, nama, hargajual, jumlah FROM t_barang WHERE kode LIKE '%" + txtCari.Text + "%' OR nama LIKE '%" + txtCari.Text + "%' ORDER BY kode");
            dgv.Rows.Clear();
            while (dr.Read())
            {
                dgv.Rows.Add(new object[] { dr[0], dr[1], dr[2], dr[3] });
            }
        }

        private void FormCari_Load(object sender, EventArgs e)
        {
            isiProduk();
            txtCari.Focus();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (dgv.Rows.Count > 0)
            {
                koneksi.IdCari = koneksi.GetSelAt(dgv);
            }
            this.DialogResult = DialogResult.Yes;
            this.Close();
        }

        private void dgv_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            btnOK_Click(null, null);
        }

        private void txtCari_TextChanged(object sender, EventArgs e)
        {
            isiProduk();
        }

        private void txtCari_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                dgv.Focus();
            }
        }

        private void btnKeluar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}

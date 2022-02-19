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
    public partial class FormProduk : Form
    {
        MySqlDataReader dr;
        public FormProduk()
        {
            InitializeComponent();
        }
        void isiProduk()
        {
            dr = koneksi.OpenDr("SELECT * FROM t_barang WHERE kode LIKE '%" + txtCari.Text + "%' OR nama LIKE '%" + txtCari.Text + "%' ORDER BY kode");
            dgv.Rows.Clear();
            int no = 1;
            while (dr.Read())
            {
                dgv.Rows.Add(new Object[] {
                    no,
                    dr["kode"],
                    dr["nama"],
                    dr["satuan"],
                    koneksi.toStrC(dr["hargabeli"]),
                    koneksi.toStrC(dr["hargajual"]),
                    dr["jumlah"],

                });
                no++;
            }
        }

        private void txtCari_TextChanged(object sender, EventArgs e)
        {
            isiProduk();
        }

        private void txtCari_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (koneksi.isEnter(e)) btnRefresh_Click_1(null, null);
        }

        private void FormProduk_Load(object sender, EventArgs e)
        {
            isiProduk();
        }

        private void btnback_Click(object sender, EventArgs e)
        {
            FormUtama frm = new FormUtama();
            frm.Show();
            this.Hide();
        }

        private void btnTambah_Click_1(object sender, EventArgs e)
        {
            FormTambahProduk frm = new FormTambahProduk();
            if (frm.ShowDialog() == DialogResult.Yes)
            {
                isiProduk();
            }
        }

        private void btnUbah_Click_1(object sender, EventArgs e)
        {
            if (dgv.Rows.Count == 0)
            {
                koneksi.msgError("Belum ada data yang dipilih");
                return;
            }

            koneksi.IdEdit = koneksi.GetSelAt(dgv, 1);

            FormTambahProduk frm = new FormTambahProduk();
            frm.editState = true;
            if (frm.ShowDialog() == DialogResult.Yes)
            {
                isiProduk();
            }
        }

        private void btnHapus_Click_1(object sender, EventArgs e)
        {
            if (dgv.Rows.Count > 0)
            {
                String d = dgv.Rows[dgv.CurrentCell.RowIndex].Cells[1].Value.ToString();
                if (koneksi.msgAsk("Hapus data?") == DialogResult.Yes)
                {
                    if (koneksi.execute("DELETE FROM t_barang WHERE kode='" + d + "'"))
                    {
                        koneksi.msgInfo("Data dihapus");
                        isiProduk();
                    }
                }
            }
        }

        private void btnRefresh_Click_1(object sender, EventArgs e)
        {
            isiProduk();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}

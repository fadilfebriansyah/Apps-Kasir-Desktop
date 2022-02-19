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
    public partial class FormTransaksi : Form
    {
        double hargabeli = 0, laba = 0;
        MySqlDataReader dr;

        public FormTransaksi()
        {
            InitializeComponent();
        }
        string fakturBaru()
        {
            return koneksi.kodeOto("t_transaksi", "Faktur", "KT-" + DateTime.Now.ToString("yyyyMMdd") + "-", 3);
        }

        void newTrans()
        {
            txtKet.Text = fakturBaru();

            txtKode.Text = "";
            emptyBrg();

            dgv.Rows.Clear();

            txtTunai.Text = "";
            txtKembali.Text = "";
            txtKode.Focus();
        }
        double hitungTotal()
        {
            double hasil = 0;
            for (int a = 0; a < dgv.Rows.Count; a++)
            {
                hasil = hasil + koneksi.toD(dgv.Rows[a].Cells[5].Value);
            }
            return hasil;
        }
        double hitungLaba()
        {
            double hasil = 0;
            for (int a = 0; a < dgv.Rows.Count; a++)
            {
                hasil = hasil + koneksi.toD(dgv.Rows[a].Cells[6].Value);
            }
            return hasil;
        }
        void emptyBrg()
        {
            txtJumlah.Text = "";
            txtNama.Text = "";
            txtHargaJual.Text = "";
            txtSatuan.Text = "";
            txtTotal.Text = "";
            laba = 0;
        }
        void hitungSubtotal()
        {
            double a = 0;
            a = koneksi.toD(txtHargaJual.Text) * koneksi.toD(txtJumlah.Text);
            txtTotal.Text = a.ToString();
            txtKet.Text = a.ToString();
        }
        void cariData()
        {
            FormCari f = new FormCari();
            if (f.ShowDialog() == DialogResult.Yes)
            {
                txtKode.Text = koneksi.IdCari;
                txtKode_TextChanged(null, null);
            }
        }
        void simpanTransaksi()
        {
            MySqlCommand cmd;
            string faktur = fakturBaru();

            if (koneksi.msgAsk("Apakah ingin menyimpan transaksi?? ") == DialogResult.Yes)
            {
                cmd = new MySqlCommand("INSERT INTO t_transaksi (faktur, tanggal, jam, total, tunai, kembali, laba) VALUES (@faktur, @tanggal, @jam, @total, @tunai, @kembali, @laba)", koneksi.getConnection());
                cmd.Parameters.AddWithValue("faktur", faktur);
                cmd.Parameters.AddWithValue("tanggal", DateTime.Now);
                cmd.Parameters.AddWithValue("jam", DateTime.Now);
                cmd.Parameters.AddWithValue("total", hitungTotal());
                cmd.Parameters.AddWithValue("tunai", txtTunai.Text);
                cmd.Parameters.AddWithValue("kembali", txtKembali.Text);
                cmd.Parameters.AddWithValue("laba", hitungLaba());

                koneksi.executeCMD(cmd);

                for (int a = 0; a < dgv.Rows.Count; a++)
                {
                    cmd = new MySqlCommand("INSERT INTO t_detailtransaksi (faktur, tanggal, kode, nama, jumlah, satuan, hargajual, total, laba) " +
                        "VALUES (@faktur, @tanggal, @kode, @nama, @jumlah, @satuan, @hargajual, @total, @laba )", koneksi.getConnection());
                    DataGridViewRow m = dgv.Rows[a];
                    cmd.Parameters.AddWithValue("faktur", faktur);
                    cmd.Parameters.AddWithValue("tanggal", DateTime.Now);
                    cmd.Parameters.AddWithValue("kode", m.Cells[0].Value);
                    cmd.Parameters.AddWithValue("nama", m.Cells[1].Value);
                    cmd.Parameters.AddWithValue("jumlah", m.Cells[2].Value);
                    cmd.Parameters.AddWithValue("satuan", m.Cells[3].Value);
                    cmd.Parameters.AddWithValue("hargajual", m.Cells[4].Value);
                    cmd.Parameters.AddWithValue("total", m.Cells[5].Value);
                    cmd.Parameters.AddWithValue("laba", m.Cells[6].Value);
                    koneksi.executeCMD(cmd);

                    koneksi.execute("UPDATE t_barang SET jumlah=jumlah-" + koneksi.toD(m.Cells[2].Value) + " WHERE kode='" + m.Cells[0].Value + "'");
                }
                koneksi.msgInfo("Transaksi tersimpan!");
                newTrans();
            }
            else
            {
                koneksi.msgInfo("Transaksi Dibatalkan!");
                newTrans();
            }


        }
        private void FormTransaksi_Load(object sender, EventArgs e)
        {
            koneksi.DsbControls(txtNama, txtSatuan, txtHargaJual, txtTotal, txtKembali);
            newTrans();
        }
        private void txtJumlah_TextChanged(object sender, EventArgs e)
        {
            hitungSubtotal();
        }
        private void txtJumlah_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                if (koneksi.toD(txtJumlah.Text) > 0)
                {
                    dgv.Rows.Add(new object[] {
                        txtKode.Text,
                        txtNama.Text,
                        txtJumlah.Text,
                        txtSatuan.Text,
                        txtHargaJual.Text,
                        txtTotal.Text,
                        laba = (koneksi.toD(txtHargaJual.Text) - hargabeli) * koneksi.toD(txtJumlah.Text)
                    });
                    txtKet.Text = hitungTotal().ToString();
                    emptyBrg();
                    txtKode.Text = "";
                    txtKode.Focus();
                }
                else
                {
                    koneksi.msgError("Jumlah harus lebih dari 0");
                }
            }
        }
        private void btncari_Click(object sender, EventArgs e)
        {
            cariData();
        }

        private void txtTunai_TextChanged(object sender, EventArgs e)
        {
            double k = 0;
            k = koneksi.toD(txtTunai.Text) - hitungTotal();
            txtKembali.Text = k.ToString();
            txtKet.Text = txtTunai.Text;
        }

        private void txtKode_TextChanged(object sender, EventArgs e)
        {
            dr = koneksi.openTable("t_barang", "kode='" + txtKode.Text + "'");
            if (dr.Read())
            {
                txtKode.Text = dr["kode"].ToString();
                txtJumlah.Text = "1";
                txtNama.Text = dr["nama"].ToString();
                txtHargaJual.Text = dr["hargajual"].ToString();
                txtSatuan.Text = dr["satuan"].ToString();
                hargabeli = koneksi.toD(dr["hargabeli"].ToString());
                hitungSubtotal();
                txtJumlah.Focus();
            }
            else
            {
                txtKet.Text = hitungTotal().ToString();
                emptyBrg();
            }
        }

        private void btnback_Click(object sender, EventArgs e)
        {
            FormUtama frm = new FormUtama();
            frm.Show();
            this.Hide();
        }

        private void btnsimpan_Click(object sender, EventArgs e)
        {
            if (koneksi.toD(txtKembali.Text) < 0)
            {
                koneksi.msgError("Tunai kurang!");
                txtTunai.Focus();
                return;
            }

            if (dgv.Rows.Count == 0)
            {
                koneksi.msgError("Belum ada barang yang dibeli!");
                txtKode.Focus();
                return;
            }
            txtKet.Text = txtKembali.Text;
            simpanTransaksi();

        }

        private void btnbaru_Click(object sender, EventArgs e)
        {
            newTrans();
        }

        private void btnkeluar_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        private void txtKode_KeyPress(object sender, KeyPressEventArgs e)
        {
            cariData();
        }

        private void txtTunai_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                if (koneksi.toD(txtKembali.Text) < 0)
                {
                    koneksi.msgError("Tunai kurang!");
                    txtTunai.Focus();
                    return;
                }

                if (dgv.Rows.Count == 0)
                {
                    koneksi.msgError("Belum ada barang yang dibeli!");
                    txtKode.Focus();
                    return;
                }
                txtKet.Text = txtKembali.Text;
                simpanTransaksi();
            }
        }
    }
}


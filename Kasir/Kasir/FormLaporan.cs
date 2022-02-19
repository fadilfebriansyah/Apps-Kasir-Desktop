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
    public partial class FormLaporan : Form
    {
        MySqlDataReader dr;
        public FormLaporan()
        {
            InitializeComponent();
        }

        private void FormLaporan_Load(object sender, EventArgs e)
        {
            txtAwal.Value = DateTime.Now;
            txtAkhir.Value = DateTime.Now;
            koneksi.DsbControls(txtGrandtotal, txtLaba);
            isiTabel();
        }
        void isiTabel()
        {
            double grandtotal = 0, laba = 0;

            string awal = txtAwal.Value.ToString("yyyy-MM-dd");
            string akhir = txtAkhir.Value.ToString("yyyy-MM-dd");
            string sql = "SELECT * FROM t_transaksi WHERE tanggal>='" + awal + "' AND tanggal <='" + akhir + "'";
            

            dr = koneksi.OpenDr(sql);
            dgv.Rows.Clear();
            int no = 1;
            while (dr.Read())
            {
                dgv.Rows.Add(new object[] {
                    no,
                    dr["Faktur"].ToString(),
                    koneksi.toStrDate(dr["tanggal"]),
                    koneksi.toStrC(dr["total"]),
                    koneksi.toStrC(dr["laba"]),
                   
                });
                grandtotal += koneksi.toD(dr["total"]);
                laba += koneksi.toD(dr["laba"]);
                no++;
            }
            txtGrandtotal.Text = koneksi.toStrC(grandtotal);
            txtLaba.Text = koneksi.toStrC(laba);
        }

        private void txtAwal_ValueChanged(object sender, EventArgs e)
        {
            isiTabel();
        }

        private void txtAkhir_ValueChanged(object sender, EventArgs e)
        {
            isiTabel();
        }

        private void btnKeluar_Click(object sender, EventArgs e)
        {
            FormUtama frm = new FormUtama();
            frm.Show();
            this.Hide();
        }
    }
}

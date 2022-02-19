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
    public partial class FormTambahProduk : Form
    {
        public bool editState = false;
        MySqlDataReader dr;
        
        public FormTambahProduk()
        {
            InitializeComponent();
        }
        
        private void btnSimpan_Click_1(object sender, EventArgs e)
        {
            if (txtKode.Text == "" && !editState)
            {
                koneksi.msgError("Kode tidak boleh kosong!");
                txtKode.Focus();
                return;
            }

    

            if (koneksi.isKdExist(txtKode.Text) && !editState)
            {
                koneksi.msgError("Kode sudah ada!");
                txtKode.Focus();
                return;
            }
            if (koneksi.isKdExistNama(txtNama.Text) && !editState)
            {
                koneksi.msgError("Nama Produk sudah ada!");
                txtKode.Focus();
                return;
            }

            if (editState)
            {
                MySqlCommand cmd = new MySqlCommand("UPDATE t_barang SET nama=@nama,  satuan=@satuan, hargabeli=@hargabeli, hargajual=@hargajual, jumlah=@jumlah WHERE kode=@kode", koneksi.getConnection());
                cmd.Parameters.AddWithValue("kode", txtKode.Text);
                cmd.Parameters.AddWithValue("nama", txtNama.Text.ToString());
                cmd.Parameters.AddWithValue("satuan", txtSatuan.Text.ToString());
                cmd.Parameters.AddWithValue("hargabeli", koneksi.toD(txtHargaBeli.Text));
                cmd.Parameters.AddWithValue("hargajual", koneksi.toD(txtHargaJual.Text));
                cmd.Parameters.AddWithValue("jumlah", koneksi.toD(txtJumlah.Text));
                koneksi.executeCMD(cmd);
            }
            else
            {
                MySqlCommand cmd = new MySqlCommand("INSERT INTO t_barang (kode, nama, satuan, hargabeli, hargajual, jumlah) VALUES(@kode, @nama, @satuan, @hargabeli, @hargajual, @jumlah)", koneksi.getConnection());
                cmd.Parameters.AddWithValue("kode", txtKode.Text.ToString());
                cmd.Parameters.AddWithValue("nama", txtNama.Text.ToString());
                cmd.Parameters.AddWithValue("satuan", txtSatuan.Text.ToString());
                cmd.Parameters.AddWithValue("hargabeli", koneksi.toD(txtHargaBeli.Text));
                cmd.Parameters.AddWithValue("hargajual", koneksi.toD(txtHargaJual.Text));
                cmd.Parameters.AddWithValue("jumlah", koneksi.toD(txtJumlah.Text));
                koneksi.executeCMD(cmd);
            }
            koneksi.msgInfo("Data tersimpan");
            this.DialogResult = DialogResult.Yes;
            this.Close();
        }

        private void btnAuto_Click(object sender, EventArgs e)
        {
            txtKode.Text = koneksi.kodeOto("t_barang", "kode", "OBT", 5);
        }

        private void btnKeluar_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormTambahProduk_Load(object sender, EventArgs e)
        {
            if (editState)
            {
                dr = koneksi.openTable("t_barang", "kode='" + koneksi.IdEdit + "'");
                if (dr.Read())
                {
                    txtKode.Text = dr["kode"].ToString();
                    txtNama.Text = dr["nama"].ToString();
                    txtSatuan.Text = dr["satuan"].ToString();
                    txtHargaBeli.Text = dr["hargabeli"].ToString();
                    txtHargaJual.Text = dr["hargajual"].ToString();
                    txtJumlah.Text = dr["jumlah"].ToString();
                    txtNama.Focus();
                }

                koneksi.DsbControls(txtKode);
                btnAuto.Visible = false;
            }
        }

        private void txtNama_Leave(object sender, EventArgs e)
        {
            if (txtNama.Text == "")
            {
                koneksi.msgInfo("Nama tidak boleh kosong!");
                txtNama.Focus();
                return;
            }
            else
            {
                if ((txtNama.Text).All(Char.IsLetter))
                {
                    
                }
                else
                {
                    koneksi.msgInfo("Inputan Nama hanya boleh huruf");
                    txtNama.Focus();
                    return;
                }
            }
        }

        private void txtSatuan_Leave(object sender, EventArgs e)
        {
            if (txtSatuan.Text == "")
            {
                koneksi.msgInfo("Satuan tidak boleh kosong!");
                txtSatuan.Focus();
                return;
            }
            else
            {
                if ((txtSatuan.Text).All(Char.IsLetter))
                {

                }
                else
                {

                    koneksi.msgInfo("Inputan Satuan hanya boleh huruf");
                    txtSatuan.Focus();
                    return;
                }
            }
        }

        private void txtHargaBeli_Leave(object sender, EventArgs e)
        {
            if (txtHargaBeli.Text == "")
            {
                koneksi.msgInfo("Harga Beli tidak boleh kosong!");
                txtHargaBeli.Focus();
                return;
            }
            else
            {
                if ((txtHargaBeli.Text).All(Char.IsNumber))
                {

                }
                else
                {
                    koneksi.msgInfo("Inputan Nama hanya boleh angka!");
                    txtHargaBeli.Focus();
                    return;
                }
            }
        }

        private void txtHargaJual_Leave(object sender, EventArgs e)
        {
            if (txtHargaJual.Text == "")
            {
                koneksi.msgInfo("Harga Jual tidak boleh kosong!");
                txtHargaJual.Focus();
                return;
            }
            else
            {
                if ((txtHargaJual.Text).All(Char.IsNumber))
                {

                }
                else
                {
                    koneksi.msgInfo("Inputan Harga Jual harus angka!");
                    txtHargaJual.Focus();
                    return;
                }
                if (int.Parse(txtHargaBeli.Text) > int.Parse(txtHargaJual.Text))
                {
                    koneksi.msgInfo("Harga Beli Harus Lebih Kecil Dari Harga Jual !");
                    txtHargaBeli.Focus();
                    return;
                }
                else { }
            }
        }

        private void txtJumlah_Leave(object sender, EventArgs e)
        {
            if (txtJumlah.Text == "")
            {
                koneksi.msgInfo("Stok tidak boleh kosong!");
                txtJumlah.Focus();
                return;
            }
            else
            {
                if ((txtJumlah.Text).All(Char.IsNumber))
                {

                }
                else
                {
                    koneksi.msgInfo("Inputan Stok harus angka!");
                    txtJumlah.Focus();
                    return;
                }
            }
        }
    }
}

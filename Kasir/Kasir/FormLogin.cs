using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
namespace Kasir
{
    public partial class FormLogin : Form
    {
        MySqlDataReader dr;
        public FormLogin()
        {
            InitializeComponent();
        }

        private void login_Click(object sender, EventArgs e)
        {
            dr = koneksi.openTable("t_user", "user='" + user.Text + "' AND pass='" + pass.Text + "' AND role=1");
            if (dr.Read())
            {               
                FormUtama frm = new FormUtama();
                frm.Show();
                this.Hide();
            }
            else
            {
                koneksi.msgError("Salah kombinasi username dan password!");
            }
        }

        private void keluar_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void FormLogin_Load(object sender, EventArgs e)
        {
            pass.PasswordChar = '*';
            user.Text = "KASIR1";
            pass.Text = "fadil";
        }

        
    }
    }


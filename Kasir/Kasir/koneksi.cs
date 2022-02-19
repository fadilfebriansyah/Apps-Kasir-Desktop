using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kasir
{
    class koneksi
    {
        public static MySqlConnection conn = null;
        public static MySqlCommand cmd;
        private static MySqlDataReader dr = null;

        public static string IdEdit, IdCari, NmCari = "";
        static void Connect()
        {
            if (conn != null) return;

            string strConn = "server=localhost; user=root; database=kasir_db; password=";

            try
            {
                conn = new MySqlConnection(strConn);
                conn.Open();
            }
            catch (Exception ex)
            {
                msgError(ex.Message);
                Environment.Exit(0);
            }
        }

        public static MySqlDataReader OpenDr(object sql)
        {
            Connect();
            CloseDr();
            try
            {
                cmd = new MySqlCommand(sql.ToString(), conn);
                dr = cmd.ExecuteReader();
            }
            catch (Exception ex)
            {
                msgError(ex.Message + "\n" + sql.ToString());
            }
            cmd.Dispose();
            return dr;
        }
        public static MySqlDataReader openTable(string table = "tb_barang", string where = "")
        {
            string sql = "SELECT * FROM " + table + " WHERE 1=1 ";
            if (where != "")
            {
                sql = sql + " AND " + where;
            }
            return OpenDr(sql);
        }

        public static MySqlConnection getConnection()
        {
            Connect();
            return conn;
        }
        public static string kodeOto(string table, string field, string start, int length)
        {
            string hasil = "";
            int panjang;
            double a = 0;

            panjang = start.Length + length;
            dr = koneksi.OpenDr("SELECT " + field + " FROM " + table + " WHERE " + field + " LIKE '%" + start + "%' AND LENGTH(" + field + ")=" + panjang + " ORDER BY " + field + " DESC");

            if (dr.Read())
            {
                a = toD(Right(dr[0].ToString(), length));
                a++;
                hasil = a.ToString();
                hasil = start + new string('0', length - hasil.Length) + hasil;
            }
            else
            {
                hasil = start + new string('0', length - 1) + "1";
            }
            return hasil;
        }
        public static string Right(string str, int length)
        {
            return str.Substring(str.Length - length, length);
        }
        public static double toD(object o)
        {
            double hasil;
            try
            {
                hasil = Double.Parse(o.ToString());
            }
            catch (Exception)
            {
                return 0;
            }
            return hasil;
        }
        
        public static void EnbControls(params Control[] o)
        {
            for (int a = 0; a < o.Length; a++)
            {
                o[a].Enabled = true;
                o[a].BackColor = System.Drawing.Color.White;
            }
        }

        public static string getOption(string option_name)
        {
            string a = "";
            dr = openTable("tb_option", "option_name='" + option_name + "'");
            if (dr.Read())
            {
                a = dr["option_value"].ToString();
            }
            return a;
        }
        public static bool isKdExist(string kode = "", string table = "t_barang", string field = "kode", string exclude = "")
        {
            dr = OpenDr("SELECT * FROM " + table + " WHERE " + field + " = '" + kode + "' AND " + field + "<>'" + exclude + "'");
            if (dr.Read()) return true;
            return false;
        }
        public static bool isKdExistNama(string nama = "", string table = "t_barang", string field = "nama", string exclude = "")
        {
            dr = OpenDr("SELECT * FROM " + table + " WHERE " + field + " = '" + nama + "' AND " + field + "<>'" + exclude + "'");
            if (dr.Read()) return true;
            return false;
        }


        public static void updateOption(string option_name, string option_value)
        {
            execute("UPDATE tb_option SET option_value='" + option_value + "' WHERE option_name='" + option_name + "'");
        }
        public static void DsbControls(params Control[] o)
        {
            for (int a = 0; a < o.Length; a++)
            {
                o[a].Enabled = false; 
                o[a].BackColor = System.Drawing.Color.FromArgb(135, 206, 235);
            }
        }
        public static bool execute(string sql)
        {
            Connect();
            CloseDr();
            try
            {
                MySqlCommand cmd = new MySqlCommand(sql, getConnection());
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (MySqlException ex)
            {
                msgError(ex.Message);
            }
            return false;
        }
        public static bool executeCMD(MySqlCommand cmd)
        {
            Connect();
            CloseDr();
            try
            {
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (MySqlException ex)
            {
                msgError(ex.Message + "\n" + cmd.CommandText);
            }
            return false;
        }
        public static void CloseDr()
        {
            try
            {
                dr.Close();
            }
            catch (Exception)
            {

            }
        }
        

        public static void msgError(string msg)
        {
            MessageBox.Show(null, msg, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static void msgInfo(object msg)
        {
            MessageBox.Show(null, msg.ToString(), "INFORMASI", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static DialogResult msgAsk(string msg)
        {
            return MessageBox.Show(null, msg, "KONFIRMASI", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }
        public static string toStrC(Object o)
        {
            return Double.Parse(o.ToString()).ToString("c2", CultureInfo.CreateSpecificCulture("id-ID"));
        }
        public static string GetSelAt(DataGridView dgv, int col = 0)
        {
            return dgv.Rows[dgv.CurrentCell.RowIndex].Cells[col].Value.ToString();
        }
        public static bool isEnter(KeyPressEventArgs e)
        {
            return e.KeyChar == (char)Keys.Enter;
        }
        public static string toStrDate(Object o)
        {
            return DateTime.Parse(o.ToString()).ToString("dd-MMM-yyyy");
        }

    }
}

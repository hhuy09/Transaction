using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;

namespace DA_HQT_CSDL_N10
{
    public partial class Form5 : Form
    {
        public Form5()
        {
            InitializeComponent();
        }

        SqlConnection con;
        public string temp;

        private void Form5_Load(object sender, EventArgs e)
        {
            string conString = ConfigurationManager.ConnectionStrings["qlbanthuenha"].ConnectionString.ToString();
            con = new SqlConnection(conString);
            con.Open();

            label28.Text = temp;


        }

        private void button1_Click(object sender, EventArgs e)
        {
            con.Close();
            Form1 form1 = new Form1();
            form1.Show();
            this.Hide();
        }


        private void button5_Click(object sender, EventArgs e)
        {
            string mode = comboBox5.Text;
            if (mode == "No")
            {
                string sqlselect = "EXEC USP_SELECT_HOME N'Nhà thuê'";
                SqlCommand cmd = new SqlCommand(sqlselect, con);
                cmd.ExecuteNonQuery();
                SqlDataReader dr = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dr);
                dataGridView1.DataSource = dt;
            }
            else if (mode == "Yes")
            {
                string sqlselect = "EXEC USP_X_SELECT_HOME N'Nhà thuê'";
                SqlCommand cmd = new SqlCommand(sqlselect, con);
                cmd.ExecuteNonQuery();
                SqlDataReader dr = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dr);
                dataGridView1.DataSource = dt;
            }
            
        }
        private void button6_Click(object sender, EventArgs e)
        {
            string mode = comboBox5.Text;
            if (mode == "No")
            {
                string sqlselect = "EXEC USP_SELECT_HOME N'Nhà bán'";
                SqlCommand cmd = new SqlCommand(sqlselect, con);
                cmd.ExecuteNonQuery();
                SqlDataReader dr = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dr);
                dataGridView1.DataSource = dt;
            }
            else if (mode == "Yes")
            {
                string sqlselect = "EXEC USP_X_SELECT_HOME N'Nhà bán'";
                SqlCommand cmd = new SqlCommand(sqlselect, con);
                cmd.ExecuteNonQuery();
                SqlDataReader dr = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dr);
                dataGridView1.DataSource = dt;
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            string mode = comboBox5.Text;
            string sqlselect;
            SqlCommand cmd;
            SqlDataReader dr;
            DataTable dt;

            if (mode == "No")
            {
                sqlselect = "EXEC USP_SELECT_VIEWINFO '" + temp + "'";
                cmd = new SqlCommand(sqlselect, con);
                dr = cmd.ExecuteReader();
                dt = new DataTable();
                dt.Load(dr);
                dataGridView1.DataSource = dt;
            }
            else if (mode == "Yes")
            {
                sqlselect = "EXEC USP_X_SELECT_VIEWINFO '" + temp + "'";
                cmd = new SqlCommand(sqlselect, con);
                dr = cmd.ExecuteReader();
                dt = new DataTable();
                dt.Load(dr);
                dataGridView1.DataSource = dt;
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            string mode = comboBox5.Text;
            string sqlselect = "";
            SqlCommand cmd;
            SqlDataReader dr;
            DataTable dt;

            if (mode == "No")
            {
                sqlselect = "USP_SELECT_HOME_AREA";

            }
            else if (mode == "Yes")
            {
                sqlselect = "USP_X_SELECT_HOME_AREA";

            }
            else if (mode == "YES")
            {
                sqlselect = "USP_XX_SELECT_HOME_AREA";

            }

            cmd = new SqlCommand(sqlselect, con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlParameter are = new SqlParameter("AREA", SqlDbType.NVarChar);
            are.Value = comboBox1.Text;
            SqlParameter typ = new SqlParameter("TYPE", SqlDbType.NVarChar);
            typ.Value = comboBox2.Text;
            SqlParameter numre = new SqlParameter("NUMRESULT", SqlDbType.Int);
            numre.Direction = ParameterDirection.Output;
            numre.Value = 0;

            cmd.Parameters.Add(are);
            cmd.Parameters.Add(typ);
            cmd.Parameters.Add(numre);

            dr = cmd.ExecuteReader();
            dt = new DataTable();
            dt.Load(dr);
            dataGridView2.DataSource = dt;
            int tt = (int)numre.Value;
            label12.Text = tt.ToString();
        }
    }
}

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
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }

        SqlConnection con;
        public string temp;

        private void Form4_Load(object sender, EventArgs e)
        {
            string conString = ConfigurationManager.ConnectionStrings["qlbanthuenha"].ConnectionString.ToString();
            con = new SqlConnection(conString);
            con.Open();

            label28.Text = temp;

            string sqlselect1 = "SELECT HOMEID, HEMPLOYEEID, STATUSNAME, RENT, HADDRESSSTREET, HADDRESSDISTRICT, HADDRESSSCITY, ROOMNUMBER, POSTDATE FROM DBO.HOME, DBO.HOMERENT, DBO.STATUS WHERE HOMERENTID = HOMEID AND HOMESTATUSID = STATUSID AND HHOSTID = '" + temp + "'";
            SqlCommand cmd = new SqlCommand(sqlselect1, con);
            SqlDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            dataGridView3.DataSource = dt;

            string sqlselect5 = "SELECT HOMEID, HEMPLOYEEID, STATUSNAME, PRICE, HADDRESSSTREET, HADDRESSDISTRICT, HADDRESSSCITY, ROOMNUMBER, POSTDATE FROM DBO.HOME, DBO.HOMESELL, DBO.STATUS WHERE HOMESELLID = HOMEID AND HOMESTATUSID = STATUSID AND HHOSTID = '" + temp + "'";
            SqlCommand cmd5 = new SqlCommand(sqlselect5, con);
            SqlDataReader dr5 = cmd5.ExecuteReader();
            DataTable dt5 = new DataTable();
            dt5.Load(dr5);
            dataGridView9.DataSource = dt5;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            con.Close();
            Form1 form1 = new Form1();
            form1.Show();
            this.Hide();
        }

        private void button12_Click(object sender, EventArgs e)
        {

        }

        private void button18_Click(object sender, EventArgs e)
        {
            string mode = comboBox5.Text;
            string sqlselect;
            SqlCommand cmd;
            SqlDataReader dr;
            DataTable dt;
            int add = Int32.Parse(textBox22.Text);


            if (mode == "No")
            {
                sqlselect = "USP_INCREASE_RENT '" + textBox21.Text + "', " + add;
                cmd = new SqlCommand(sqlselect, con);
                cmd.ExecuteNonQuery();
            }
            else if (mode == "Yes")
            {
                try
                {
                    sqlselect = "USP_CONVER_INCREASE_RENT '" + textBox21.Text + "', " + add;
                    cmd = new SqlCommand(sqlselect, con);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception)
                {
                    MessageBox.Show("Conversion Deadlock");
                }
            }
            else if (mode == "YES")
            {
                try
                {
                    sqlselect = "USP_X_INCREASE_RENT '" + textBox21.Text + "', " + add;
                    cmd = new SqlCommand(sqlselect, con);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception)
                {

                }
            }

            string sqlselect1 = "SELECT HOMEID, HEMPLOYEEID, STATUSNAME, RENT, HADDRESSSTREET, HADDRESSDISTRICT, HADDRESSSCITY, ROOMNUMBER, POSTDATE FROM DBO.HOME, DBO.HOMERENT, DBO.STATUS WHERE HOMERENTID = HOMEID AND HOMESTATUSID = STATUSID AND HHOSTID = '" + temp + "'";
            cmd = new SqlCommand(sqlselect1, con);
            dr = cmd.ExecuteReader();
            dt = new DataTable();
            dt.Load(dr);
            dataGridView3.DataSource = dt;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string sqlselect = "EXEC USP_DELETE_HOME @ID";
            SqlCommand cmd = new SqlCommand(sqlselect, con);
            cmd.Parameters.Add(new SqlParameter("@ID", textBox7.Text));
            cmd.ExecuteNonQuery();

            string sqlselect1 = "SELECT HOMEID, HEMPLOYEEID, STATUSNAME, RENT, HADDRESSSTREET, HADDRESSDISTRICT, HADDRESSSCITY, ROOMNUMBER, POSTDATE FROM DBO.HOME, DBO.HOMERENT, DBO.STATUS WHERE HOMERENTID = HOMEID AND HOMESTATUSID = STATUSID AND HHOSTID = '" + temp + "'";
            SqlCommand cmd1 = new SqlCommand(sqlselect1, con);
            SqlDataReader dr1 = cmd1.ExecuteReader();
            DataTable dt1 = new DataTable();
            dt1.Load(dr1);
            dataGridView3.DataSource = dt1;

            string sqlselect5 = "SELECT HOMEID, HEMPLOYEEID, STATUSNAME, PRICE, HADDRESSSTREET, HADDRESSDISTRICT, HADDRESSSCITY, ROOMNUMBER, POSTDATE FROM DBO.HOME, DBO.HOMESELL, DBO.STATUS WHERE HOMESELLID = HOMEID AND HOMESTATUSID = STATUSID AND HHOSTID = '" + temp + "'";
            SqlCommand cmd5 = new SqlCommand(sqlselect5, con);
            SqlDataReader dr5 = cmd5.ExecuteReader();
            DataTable dt5 = new DataTable();
            dt5.Load(dr5);
            dataGridView9.DataSource = dt5;
        }
    }
}

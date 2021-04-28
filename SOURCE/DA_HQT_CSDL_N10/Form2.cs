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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        SqlConnection con;
        public string temp;

        private void Form2_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'qUAN_LY_BAN_THUE_NHA_TRANSACTIONDataSet2.TYPE' table. You can move, or remove it, as needed.
            this.tYPETableAdapter.Fill(this.qUAN_LY_BAN_THUE_NHA_TRANSACTIONDataSet2.TYPE);
            // TODO: This line of code loads data into the 'qUAN_LY_BAN_THUE_NHA_TRANSACTIONDataSet1.STATUS' table. You can move, or remove it, as needed.
            this.sTATUSTableAdapter.Fill(this.qUAN_LY_BAN_THUE_NHA_TRANSACTIONDataSet1.STATUS);
            // TODO: This line of code loads data into the 'qUAN_LY_BAN_THUE_NHA_TRANSACTIONDataSet.EMPLOYEE' table. You can move, or remove it, as needed.
            this.eMPLOYEETableAdapter.Fill(this.qUAN_LY_BAN_THUE_NHA_TRANSACTIONDataSet.EMPLOYEE);
            string conString = ConfigurationManager.ConnectionStrings["qlbanthuenha"].ConnectionString.ToString();
            con = new SqlConnection(conString);
            con.Open();

            label28.Text = temp;

            String sqlselect = "SELECT * FROM DBO.EMPLOYEE";
            SqlCommand cmd = new SqlCommand(sqlselect, con);
            SqlDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            dataGridView2.DataSource = dt;

            string sqlselect2 = "SELECT * FROM DBO.VIEWINFO";
            SqlCommand cmd2 = new SqlCommand(sqlselect2, con);
            SqlDataReader dr2 = cmd2.ExecuteReader();
            DataTable dt2 = new DataTable();
            dt2.Load(dr2);
            dataGridView12.DataSource = dt2;
            dataGridView5.DataSource = dt2;

            string sqlselect6 = "SELECT * FROM DBO.HOME";
            SqlCommand cmd6 = new SqlCommand(sqlselect6, con);
            SqlDataReader dr6 = cmd6.ExecuteReader();
            DataTable dt6 = new DataTable();
            dt6.Load(dr6);
            dataGridView10.DataSource = dt6;

            string sqlselect7 = "SELECT * FROM DBO.REQUEST";
            SqlCommand cmd7 = new SqlCommand(sqlselect7, con);
            SqlDataReader dr7 = cmd7.ExecuteReader();
            DataTable dt7 = new DataTable();
            dt7.Load(dr7);
            dataGridView11.DataSource = dt7;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            con.Close();
            Form1 form1 = new Form1();
            form1.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string sqlselect = "SELECT * FROM DBO.BRANCH";
            SqlCommand cmd = new SqlCommand(sqlselect, con);
            SqlDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            dataGridView1.DataSource = dt;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string sqlselect = "SELECT * FROM DBO.EMPLOYEE";
            SqlCommand cmd = new SqlCommand(sqlselect, con);
            SqlDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            dataGridView1.DataSource = dt;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string sqlselect = "SELECT * FROM DBO.HOST";
            SqlCommand cmd = new SqlCommand(sqlselect, con);
            SqlDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            dataGridView1.DataSource = dt;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            string sqlselect = "SELECT * FROM DBO.REQUEST";
            SqlCommand cmd = new SqlCommand(sqlselect, con);
            SqlDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            dataGridView1.DataSource = dt;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string sqlselect = "SELECT HOMERENTID, HEMPLOYEEID, HADDRESSSTREET, HADDRESSDISTRICT, HADDRESSSCITY, HOMESTATUSID, ROOMNUMBER, RENT, POSTDATE, EXPIRATIONDATE, HVIEW FROM DBO.HOME, DBO.HOMERENT WHERE HOMEID = HOMERENTID";
            SqlCommand cmd = new SqlCommand(sqlselect, con);
            SqlDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            dataGridView1.DataSource = dt;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string sqlselect = "SELECT HOMESELLID, HEMPLOYEEID, HADDRESSSTREET, HADDRESSDISTRICT, HADDRESSSCITY, HOMESTATUSID, ROOMNUMBER, PRICE, POSTDATE, EXPIRATIONDATE, HVIEW FROM DBO.HOME, DBO.HOMESELL WHERE HOMEID = HOMESELLID";
            SqlCommand cmd = new SqlCommand(sqlselect, con);
            SqlDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            dataGridView1.DataSource = dt;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            string sqlselect = "SELECT * FROM DBO.VIEWINFO";
            SqlCommand cmd = new SqlCommand(sqlselect, con);
            SqlDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            dataGridView1.DataSource = dt;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            string sqlselect = "SELECT CONTRACTRENTID, CCUSTOMERID, CHOMEID, CVIEWDATE, SIGNDATE, ENDDATE FROM DBO.CONTRACT, DBO.CONTRACTRENT WHERE CONTRACTID = CONTRACTRENTID";
            SqlCommand cmd = new SqlCommand(sqlselect, con);
            SqlDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            dataGridView1.DataSource = dt;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            string sqlselect = "SELECT CONTRACTSELLID, CCUSTOMERID, CHOMEID, CVIEWDATE, SIGNDATE, DEPOSIT FROM DBO.CONTRACT, DBO.CONTRACTSELL WHERE CONTRACTID = CONTRACTSELLID";
            SqlCommand cmd = new SqlCommand(sqlselect, con);
            SqlDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            dataGridView1.DataSource = dt;
        }


        private void button11_Click_1(object sender, EventArgs e)
        {
            string area = comboBox1.Text;
            string mode = comboBox7.Text;
            string sqlselect = "";

            if(mode == "No")
            {
                sqlselect = "USP_SELECT_EMP_AREA";
            }
            else if(mode == "Yes")
            {
                sqlselect = "USP_X_SELECT_EMP_AREA";
            }

            SqlCommand cmd = new SqlCommand(sqlselect, con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlParameter are = new SqlParameter("AREA", SqlDbType.NVarChar);
            are.Value = area; 
            SqlParameter numre = new SqlParameter("NUMRESULT", SqlDbType.Int);
            numre.Direction = ParameterDirection.Output;
            numre.Value = 0;

            cmd.Parameters.Add(are);
            cmd.Parameters.Add(numre);

            SqlDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            dataGridView2.DataSource = dt;
            int numresult = (int)numre.Value;
            label9.Text = numresult.ToString();

        }

        private void button19_Click(object sender, EventArgs e)
        {
            string mode = comboBox7.Text;
            string sqlselect;
            SqlCommand cmd;
            SqlDataReader dr;
            DataTable dt;

            if (mode == "No")
            {
                sqlselect = "EXEC USP_SELECT_CONTRACT";
                cmd = new SqlCommand(sqlselect, con);
                dr = cmd.ExecuteReader();
                dt = new DataTable();
                dt.Load(dr);
                dataGridView1.DataSource = dt;
            }
            else if (mode == "Yes")
            {
                sqlselect = "EXEC USP_X_SELECT_CONTRACT";
                cmd = new SqlCommand(sqlselect, con);
                dr = cmd.ExecuteReader();
                dt = new DataTable();
                dt.Load(dr);
                dataGridView1.DataSource = dt;
            }           
        }

        private void button20_Click(object sender, EventArgs e)
        {
            string mode = comboBox7.Text;
            string sqlselect;
            SqlCommand cmd;

            if (mode == "No")
            {
                sqlselect = "EXEC USP_INSERT_VIEWINFO @CUSID, @HOMEID, @DATE, @CMT";
                cmd = new SqlCommand(sqlselect, con);
                cmd.Parameters.Add(new SqlParameter("@CUSID", textBox24.Text));
                cmd.Parameters.Add(new SqlParameter("@HOMEID", textBox23.Text));
                cmd.Parameters.Add(new SqlParameter("@DATE", dateTimePicker3.Value.Date));
                cmd.Parameters.Add(new SqlParameter("@CMT", textBox3.Text));
                cmd.ExecuteNonQuery();
            }
            else if (mode == "Yes")
            {
                try
                {
                    sqlselect = "EXEC USP_CONVER_INSERT_VIEWINFO @CUSID, @HOMEID, @DATE, @CMT";
                    cmd = new SqlCommand(sqlselect, con);
                    cmd.Parameters.Add(new SqlParameter("@CUSID", textBox24.Text));
                    cmd.Parameters.Add(new SqlParameter("@HOMEID", textBox23.Text));
                    cmd.Parameters.Add(new SqlParameter("@DATE", dateTimePicker3.Value.Date));
                    cmd.Parameters.Add(new SqlParameter("@CMT", textBox3.Text));
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
                    sqlselect = "EXEC USP_X_INSERT_VIEWINFO @CUSID, @HOMEID, @DATE, @CMT";
                    cmd = new SqlCommand(sqlselect, con);
                    cmd.Parameters.Add(new SqlParameter("@CUSID", textBox24.Text));
                    cmd.Parameters.Add(new SqlParameter("@HOMEID", textBox23.Text));
                    cmd.Parameters.Add(new SqlParameter("@DATE", dateTimePicker3.Value.Date));
                    cmd.Parameters.Add(new SqlParameter("@CMT", textBox3.Text));
                    cmd.ExecuteNonQuery();
                }
                catch (Exception)
                {

                }
            }
            string sqlselect6 = "SELECT * FROM DBO.HOME";
            SqlCommand cmd6 = new SqlCommand(sqlselect6, con);
            SqlDataReader dr6 = cmd6.ExecuteReader();
            DataTable dt6 = new DataTable();
            dt6.Load(dr6);
            dataGridView10.DataSource = dt6;

            string sqlselect2 = "SELECT * FROM DBO.VIEWINFO";
            SqlCommand cmd2 = new SqlCommand(sqlselect2, con);
            SqlDataReader dr2 = cmd2.ExecuteReader();
            DataTable dt2 = new DataTable();
            dt2.Load(dr2);
            dataGridView12.DataSource = dt2;

        }

        private void button13_Click(object sender, EventArgs e)
        {
            string mode = comboBox7.Text;
            string sqlselect = "";
            SqlCommand cmd;
            SqlDataReader dr;
            DataTable dt;

            if (mode == "No")
            {
                sqlselect = "USP_COUNT_CONTRACT";
                
            }
            else if (mode == "Yes")
            {
                sqlselect = "USP_X_COUNT_CONTRACT";
                
            }

            cmd = new SqlCommand(sqlselect, con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlParameter total = new SqlParameter("TOTAL", SqlDbType.Int);
            total.Direction = ParameterDirection.Output;
            total.Value = 0;

            cmd.Parameters.Add(total);

            dr = cmd.ExecuteReader();
            dt = new DataTable();
            dt.Load(dr);
            dataGridView4.DataSource = dt;
            int tt = (int)total.Value;
            label8.Text = tt.ToString();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            string sqlselect;
            SqlCommand cmd;
            try
            {
                sqlselect = "EXEC USP_DELETE_VIEWINFO @HOMEID, @CUSID, @DATE";
                cmd = new SqlCommand(sqlselect, con);
                cmd.Parameters.Add(new SqlParameter("@CUSID", textBox1.Text));
                cmd.Parameters.Add(new SqlParameter("@HOMEID", textBox2.Text));
                cmd.Parameters.Add(new SqlParameter("@DATE", dateTimePicker2.Value.Date));
                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {

            }

            string sqlselect2 = "SELECT * FROM DBO.VIEWINFO";
            SqlCommand cmd2 = new SqlCommand(sqlselect2, con);
            SqlDataReader dr2 = cmd2.ExecuteReader();
            DataTable dt2 = new DataTable();
            dt2.Load(dr2);
            dataGridView12.DataSource = dt2;
            dataGridView5.DataSource = dt2;
        }
    }
}

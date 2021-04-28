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
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        SqlConnection con;
        public string temp;

        private void show()
        {

        }

        private void Form3_Load(object sender, EventArgs e)
        {
            string conString = ConfigurationManager.ConnectionStrings["qlbanthuenha"].ConnectionString.ToString();
            con = new SqlConnection(conString);
            con.Open();

            label28.Text = temp;

            string sqlselect1 = "SELECT HOMEID, HHOSTID, STATUSNAME, RENT, HADDRESSSTREET, HADDRESSDISTRICT, HADDRESSSCITY, ROOMNUMBER, POSTDATE FROM DBO.HOME, DBO.HOMERENT, DBO.STATUS WHERE HOMERENTID = HOMEID AND HOMESTATUSID = STATUSID AND HEMPLOYEEID = '" + temp + "'";
            SqlCommand cmd = new SqlCommand(sqlselect1, con);
            SqlDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            dataGridView3.DataSource = dt;

            string sqlselect2 = "SELECT * FROM DBO.VIEWINFO";
            SqlCommand cmd2 = new SqlCommand(sqlselect2, con);
            SqlDataReader dr2 = cmd2.ExecuteReader();
            DataTable dt2 = new DataTable();
            dt2.Load(dr2);
            dataGridView4.DataSource = dt2;

            string sqlselect3 = "SELECT * FROM DBO.CONTRACT";
            SqlCommand cmd3= new SqlCommand(sqlselect3, con);
            SqlDataReader dr3 = cmd3.ExecuteReader();
            DataTable dt3 = new DataTable();
            dt3.Load(dr3);
            dataGridView5.DataSource = dt3;

            string sqlselect4 = "SELECT VCUSTOMERID, VHOMEID, TYPENAME, VIEWDATE, CUSTOMERCOMMENT FROM DBO.VIEWINFO, DBO.HOME, DBO.TYPE WHERE VHOMEID = HOMEID AND HOMETYPEID = TYPEID AND HEMPLOYEEID = '" + temp + "'";
            SqlCommand cmd4 = new SqlCommand(sqlselect4, con);
            SqlDataReader dr4 = cmd4.ExecuteReader();
            DataTable dt4 = new DataTable();
            dt4.Load(dr4);
            dataGridView8.DataSource = dt4;

            string sqlselect5 = "SELECT HOMEID, HHOSTID, STATUSNAME, PRICE, HADDRESSSTREET, HADDRESSDISTRICT, HADDRESSSCITY, ROOMNUMBER, POSTDATE FROM DBO.HOME, DBO.HOMESELL, DBO.STATUS WHERE HOMESELLID = HOMEID AND HOMESTATUSID = STATUSID AND HEMPLOYEEID = '" + temp + "'";
            SqlCommand cmd5 = new SqlCommand(sqlselect5, con);
            SqlDataReader dr5 = cmd5.ExecuteReader();
            DataTable dt5 = new DataTable();
            dt5.Load(dr5);
            dataGridView9.DataSource = dt5;

            string sqlselect6 = "SELECT * FROM DBO.HOME WHERE HEMPLOYEEID = '" + temp + "'";
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

            string sqlselect8 = "SELECT * FROM DBO.EMPLOYEE WHERE EMPLOYEEID = '" + temp + "'";
            SqlCommand cmd8 = new SqlCommand(sqlselect8, con);
            SqlDataReader dr8 = cmd8.ExecuteReader();
            DataTable dt8 = new DataTable();
            dt8.Load(dr8);
            dataGridView7.DataSource = dt8;

            string sqlselect9 = "SELECT * FROM DBO.HOST";
            SqlCommand cmd9 = new SqlCommand(sqlselect9, con);
            SqlDataReader dr9 = cmd9.ExecuteReader();
            DataTable dt9 = new DataTable();
            dt9.Load(dr9);
            dataGridView6.DataSource = dt9;

            string sqlselect10 = "SELECT * FROM DBO.HOME";
            SqlCommand cmd10 = new SqlCommand(sqlselect10, con);
            SqlDataReader dr10 = cmd10.ExecuteReader();
            DataTable dt10 = new DataTable();
            dt10.Load(dr10);
            dataGridView12.DataSource = dt10;
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

        private void button12_Click(object sender, EventArgs e)
        {
            //string mode = comboBox5.Text;
            //if (mode == "No")
            //{
            //    string tran01_t1 = "EXEC TRAN01_T1 @HOMEID , @HOMESTT SELECT * FROM DBO.HOME";
            //    SqlCommand cmd = new SqlCommand(tran01_t1, con);
            //    cmd.Parameters.Add(new SqlParameter("@HOMEID", comboBox2.Text));
            //    cmd.Parameters.Add(new SqlParameter("@HOMESTT", comboBox3.Text));
            //    cmd.ExecuteNonQuery();
            //    SqlDataReader dr = cmd.ExecuteReader();
            //    if( dr.Read() == true)
            //    {
            //        MessageBox.Show("Cập nhật thành công");
            //        DataTable dt = new DataTable();
            //        dt.Load(dr);
            //        dataGridView3.DataSource = dt;
            //    }
            //    else
            //    {
            //        MessageBox.Show("Cập nhật không thành công");
            //    }
                
            //}
            //else if (mode == "Yes")
            //{
            //    string tran01_t1 = "EXEC XL_TRAN01_T1 @HOMEID , @HOMESTT";
            //    SqlCommand cmd = new SqlCommand(tran01_t1, con);
            //    cmd.Parameters.Add(new SqlParameter("@HOMEID", comboBox2.Text));
            //    cmd.Parameters.Add(new SqlParameter("@HOMESTT", comboBox3.Text));
            //    cmd.ExecuteNonQuery();
            //    SqlDataReader dr = cmd.ExecuteReader();
            //    DataTable dt = new DataTable();
            //    dt.Load(dr);
            //    dataGridView3.DataSource = dt;
            //}
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

        private void button7_Click(object sender, EventArgs e)
        {
            string sqlselect = "SELECT * FROM DBO.VIEWINFO";
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

        private void button9_Click(object sender, EventArgs e)
        {
            string sqlselect = "SELECT CONTRACTRENTID, CCUSTOMERID, CHOMEID, CVIEWDATE, SIGNDATE, ENDDATE FROM DBO.CONTRACT, DBO.CONTRACTRENT WHERE CONTRACTID = CONTRACTRENTID";
            SqlCommand cmd = new SqlCommand(sqlselect, con);
            SqlDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            dataGridView1.DataSource = dt;
        }

        private void button11_Click(object sender, EventArgs e)
        {

        }

        private void button14_Click(object sender, EventArgs e)
        {
            string mode = comboBox5.Text;
            if (mode == "No")
            {
                string tran01_t1 = "EXEC USP_INSERT_CONTRACTRENT @ID , @CUSID , @HOMEID , @DATE , @SIGN , @END  SELECT * FROM DBO.HOME";
                SqlCommand cmd = new SqlCommand(tran01_t1, con);
                cmd.Parameters.Add(new SqlParameter("@ID", textBox2.Text));
                cmd.Parameters.Add(new SqlParameter("@CUSID", textBox6.Text));
                cmd.Parameters.Add(new SqlParameter("@HOMEID", textBox5.Text));
                cmd.Parameters.Add(new SqlParameter("@DATE", dateTimePicker3.Value.Date));
                cmd.Parameters.Add(new SqlParameter("@SIGN", dateTimePicker4.Value.Date));
                cmd.Parameters.Add(new SqlParameter("@END", dateTimePicker5.Value.Date));
                cmd.ExecuteNonQuery();
            }
            else if (mode == "Yes")
            {
                string tran01_t1 = "EXEC USP_INSERT_CONTRACTRENT @ID , @CUSID , @HOMEID , @DATE , @SIGN , @END";
                SqlCommand cmd = new SqlCommand(tran01_t1, con);
                cmd.Parameters.Add(new SqlParameter("@ID", textBox2.Text));
                cmd.Parameters.Add(new SqlParameter("@CUSID", textBox6.Text));
                cmd.Parameters.Add(new SqlParameter("@HOMEID", textBox5.Text));
                cmd.Parameters.Add(new SqlParameter("@DATE", dateTimePicker3.Value.Date));
                cmd.Parameters.Add(new SqlParameter("@SIGN", dateTimePicker4.Value.Date));
                cmd.Parameters.Add(new SqlParameter("@END", dateTimePicker5.Value.Date));
                cmd.ExecuteNonQuery();
            }

            string sqlselect3 = "SELECT * FROM DBO.CONTRACT";
            SqlCommand cmd3 = new SqlCommand(sqlselect3, con);
            SqlDataReader dr3 = cmd3.ExecuteReader();
            DataTable dt3 = new DataTable();
            dt3.Load(dr3);
            dataGridView5.DataSource = dt3;
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

        private void button19_Click(object sender, EventArgs e)
        {
            string mode = comboBox5.Text;
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

            sqlselect = "SELECT HOMEID, HHOSTID, STATUSNAME, RENT, HADDRESSSTREET, HADDRESSDISTRICT, HADDRESSSCITY, ROOMNUMBER, POSTDATE FROM DBO.HOME, DBO.HOMERENT, DBO.STATUS WHERE HOMERENTID = HOMEID AND HOMESTATUSID = STATUSID AND HEMPLOYEEID = '" + temp + "'";
            cmd = new SqlCommand(sqlselect, con);
            dr = cmd.ExecuteReader();
            dt = new DataTable();
            dt.Load(dr);
            dataGridView3.DataSource = dt;
        }

        private void button16_Click(object sender, EventArgs e)
        {
            string sqlupdate = "EXEC USP_UPDATE_EMP_AREA '" + temp + "', N'" + textBox12.Text + "' SELECT * FROM DBO.EMPLOYEE WHERE EMPLOYEEID = '" + temp + "'";
            SqlCommand cmd7 = new SqlCommand(sqlupdate, con);
            SqlDataReader dr7 = cmd7.ExecuteReader();
            DataTable dt7 = new DataTable();
            dt7.Load(dr7);
            dataGridView7.DataSource = dt7;
        }

        private void button13_Click(object sender, EventArgs e)
        {
            string mode = comboBox5.Text;
            string sqlselect;
            SqlCommand cmd;

            if (mode == "No")
            {
                sqlselect = "EXEC USP_INSERT_VIEWINFO @CUSID, @HOMEID, @DATE, @CMT";
                cmd = new SqlCommand(sqlselect, con);
                cmd.Parameters.Add(new SqlParameter("@CUSID", textBox24.Text));
                cmd.Parameters.Add(new SqlParameter("@HOMEID", textBox23.Text));
                cmd.Parameters.Add(new SqlParameter("@DATE", dateTimePicker2.Value.Date));
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
                    cmd.Parameters.Add(new SqlParameter("@DATE", dateTimePicker2.Value.Date));
                    cmd.Parameters.Add(new SqlParameter("@CMT", textBox3.Text));
                    cmd.ExecuteNonQuery();
                }
                catch(Exception)
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
                    cmd.Parameters.Add(new SqlParameter("@DATE", dateTimePicker2.Value.Date));
                    cmd.Parameters.Add(new SqlParameter("@CMT", textBox3.Text));
                    cmd.ExecuteNonQuery();
                }
                catch (Exception)
                {

                }
            }
            string sqlselect6 = "SELECT * FROM DBO.HOME WHERE HEMPLOYEEID = '" + temp + "'";
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
            dataGridView4.DataSource = dt2;

            
        }

        private void button17_Click(object sender, EventArgs e)
        {
            string mode = comboBox5.Text;
            int depo = Int32.Parse(textBox17.Text);

            string tran01_t1 = "EXEC USP_INSERT_CONTRACTSELL @ID , @CUSID , @HOMEID , @DATE , @SIGN , @DEPO";
            SqlCommand cmd = new SqlCommand(tran01_t1, con);
            cmd.Parameters.Add(new SqlParameter("@ID", textBox20.Text));
            cmd.Parameters.Add(new SqlParameter("@CUSID", textBox19.Text));
            cmd.Parameters.Add(new SqlParameter("@HOMEID", textBox18.Text));
            cmd.Parameters.Add(new SqlParameter("@DATE", dateTimePicker8.Value.Date));
            cmd.Parameters.Add(new SqlParameter("@SIGN", dateTimePicker7.Value.Date));
            cmd.Parameters.Add(new SqlParameter("@DEPO", textBox17.Text));
            cmd.ExecuteNonQuery();

            string sqlselect3 = "SELECT * FROM DBO.CONTRACT";
            SqlCommand cmd3 = new SqlCommand(sqlselect3, con);
            SqlDataReader dr3 = cmd3.ExecuteReader();
            DataTable dt3 = new DataTable();
            dt3.Load(dr3);
            dataGridView5.DataSource = dt3;
        }


        private void button15_Click(object sender, EventArgs e)
        {          
            string sqlselect = "EXEC USP_INSERT_HOMERENT @HOST, @EMP, @STR, @DIS, @CITY, @ROOM, @EDATE, @RENT";
            SqlCommand cmd = new SqlCommand(sqlselect, con);
            cmd.Parameters.Add(new SqlParameter("@HOST", textBox7.Text));
            cmd.Parameters.Add(new SqlParameter("@EMP", temp));
            cmd.Parameters.Add(new SqlParameter("@STR", textBox4.Text));
            cmd.Parameters.Add(new SqlParameter("@DIS", textBox9.Text));
            cmd.Parameters.Add(new SqlParameter("@CITY", textBox8.Text));
            cmd.Parameters.Add(new SqlParameter("@ROOM", Int32.Parse(textBox10.Text)));
            cmd.Parameters.Add(new SqlParameter("@EDATE", dateTimePicker6.Value.Date));
            cmd.Parameters.Add(new SqlParameter("@RENT", Int32.Parse(textBox1.Text)));
            cmd.ExecuteNonQuery();

            string sqlselect10 = "SELECT * FROM DBO.HOME";
            SqlCommand cmd10 = new SqlCommand(sqlselect10, con);
            SqlDataReader dr10 = cmd10.ExecuteReader();
            DataTable dt10 = new DataTable();
            dt10.Load(dr10);
            dataGridView12.DataSource = dt10;
        }

        private void button20_Click(object sender, EventArgs e)
        {
            string sqlselect = "EXEC USP_INSERT_HOMESELL @HOST, @EMP, @STR, @DIS, @CITY, @ROOM, @EDATE, @SELL, @RQH";
            SqlCommand cmd = new SqlCommand(sqlselect, con);
            cmd.Parameters.Add(new SqlParameter("@HOST", textBox15.Text));
            cmd.Parameters.Add(new SqlParameter("@EMP", temp));
            cmd.Parameters.Add(new SqlParameter("@STR", textBox13.Text));
            cmd.Parameters.Add(new SqlParameter("@DIS", textBox25.Text));
            cmd.Parameters.Add(new SqlParameter("@CITY", textBox16.Text));
            cmd.Parameters.Add(new SqlParameter("@ROOM", Int32.Parse(textBox14.Text)));
            cmd.Parameters.Add(new SqlParameter("@EDATE", dateTimePicker9.Value.Date));
            cmd.Parameters.Add(new SqlParameter("@SELL", Int32.Parse(textBox11.Text)));
            cmd.Parameters.Add(new SqlParameter("@RQH", textBox26.Text));
            cmd.ExecuteNonQuery();

            string sqlselect10 = "SELECT * FROM DBO.HOME";
            SqlCommand cmd10 = new SqlCommand(sqlselect10, con);
            SqlDataReader dr10 = cmd10.ExecuteReader();
            DataTable dt10 = new DataTable();
            dt10.Load(dr10);
            dataGridView12.DataSource = dt10;
        }
    }
}

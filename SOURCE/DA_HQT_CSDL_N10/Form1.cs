using System;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;

namespace DA_HQT_CSDL_N10
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        SqlConnection con;

        private void Form1_Load(object sender, EventArgs e)
        {
            string conString = ConfigurationManager.ConnectionStrings["qlbanthuenha"].ConnectionString.ToString();
            con = new SqlConnection(conString);
            con.Open();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            con.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {         
            string user_name = textBox1.Text;
            string pass_word = textBox2.Text;
            string sqlselect = "SELECT * FROM DBO.LOGIN WHERE username = '" + user_name + "' AND password = '" + pass_word + "'";
            SqlCommand cmd = new SqlCommand(sqlselect, con);
            SqlDataReader data = cmd.ExecuteReader();
            if (data.Read() == true)
            {
                MessageBox.Show("Đăng nhập thành công");
                string role = data["role"].ToString();
                string id = data["id"].ToString();


                if (role == "1")
                {
                    Form2 form2 = new Form2();
                    form2.temp = id;
                    form2.Show();
                }
                else if (role == "2")
                {
                    Form3 form3 = new Form3();
                    form3.temp = id;
                    form3.Show();
                }
                else if (role == "3")
                {
                    Form4 form4 = new Form4();
                    form4.temp = id;
                    form4.Show();
                }
                else if (role == "4")
                {
                    Form5 form5 = new Form5();
                    form5.temp = id;
                    form5.Show();
                }
                this.Hide();
            }
            else
            {
                MessageBox.Show("Đăng nhập thất bại!");
                Form1 form1 = new Form1();
                form1.Show();
                this.Hide();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}

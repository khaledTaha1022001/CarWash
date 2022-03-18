using System.Data.SqlClient;
using System.Data;

namespace CarWash
{
    public partial class Form1 : Form
    {
        
        public Form1()
        {
            InitializeComponent();
        }
     private SqlConnection conn = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Khaled Taha\source\repos\CarWash\CarWash\Database1.mdf;Integrated Security=True");
        SqlCommand cmd = null;
        string sql=null;
        SqlDataAdapter da = null;

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            int custid = 0;
            int carid=0;
            int phone=0;
            string custName=null;
            string carType=null;
            try
            {
                custName = textBox2.Text;
                phone = int.Parse(textBox3.Text);
                carid = int.Parse(textBox4.Text);
                carType = textBox5.Text;
            }
            catch (Exception ex) {
                MessageBox.Show("make sure that u entered correct data..", "Error");
                return;
            }
            sql = "insert into customer values(@custname,@phone)";
            cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@custname", custName);
            cmd.Parameters.AddWithValue("@phone", phone);
           conn.Open();
            try
            {
                cmd.ExecuteNonQuery();
                MessageBox.Show(1 + "Record is added to Customer", "Result", MessageBoxButtons.OKCancel);

            }
            catch (Exception ex)
            {

                DialogResult res = MessageBox.Show(0 + "Record isn't  added there is something wrong.", "Result", MessageBoxButtons.OKCancel);
                return;

            }
            conn.Close();
            //---------------------------------------------------
         //   sql = "select PhoneNumber from customer where name=@custname";
        //    cmd = new SqlCommand(sql, conn);
          //  cmd.Parameters.AddWithValue("@custname", custName);
        //    conn.Open();
          // custid= (int) cmd.ExecuteScalar();
         //   conn.Close ();
            //-------------------------------------------------
            sql = "insert into car values(@carid,@cartype,@phone)";
            cmd =new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@cartype", carType);
            cmd.Parameters.AddWithValue("@phone",  phone);
            cmd.Parameters.AddWithValue("@carid",  carid);
            conn.Open();
            try { cmd.ExecuteNonQuery();
                MessageBox.Show(1 + "Record is added", "Result", MessageBoxButtons.OKCancel);

            } catch (Exception ex) {
                sql = "delete from Customer where PhoneNumber=@phone";
                cmd=new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("phone", phone);
                cmd.ExecuteNonQuery();
                DialogResult res = MessageBox.Show(0 + "Record isn't  added there is something wrong. 1 Record Deleted from Customer..", "Result", MessageBoxButtons.OKCancel);
                return;

            }

                conn.Close();




        }

        private void button2_Click(object sender, EventArgs e)
        {


            int phone = 0;
            int carid = 0;
            DateTime Oil = new DateTime(2042, 12, 24);
            DateTime Filter = new DateTime(2042, 12, 24);

            try {
                phone = int.Parse(textBox7.Text);
                carid=int.Parse(textBox6.Text);
                Oil = new DateTime(dateTimePicker1.Value.Year,dateTimePicker1.Value.Month,dateTimePicker1.Value.Day);
                Filter = new DateTime(dateTimePicker2.Value.Year, dateTimePicker1.Value.Month, dateTimePicker1.Value.Day);


            }
            catch (Exception ex) {
                MessageBox.Show("make sure that u entered correct data..", "Error");
                return;

            }
            //sql = "select CustomerID,Carid from Car where CustomerID=@phone and Carid=@cid";
            sql = "select CustomerID from Car WHERE CustomerID = @phone and Carid = @cid ;";
            cmd=new SqlCommand(sql,conn);
            cmd.Parameters.AddWithValue("phone",  phone);
            cmd.Parameters.AddWithValue("cid",  carid);
            conn.Open();
            
            try {
                cmd.ExecuteScalar();
            }
            catch (Exception ex) {
                MessageBox.Show("there is not customer with the entered infromations ,please check it again", "Error");
                conn.Close();
                return;

            }
           
      
                sql = "insert into Service values (@phone,@carid,@date,@filter)";
                cmd = new SqlCommand(sql,conn);
                cmd.Parameters.AddWithValue("phone",phone);
                cmd.Parameters.AddWithValue("carid",  carid);
                cmd.Parameters.AddWithValue("date",Oil);
                cmd.Parameters.AddWithValue("filter",Filter);
 
               
                cmd.ExecuteNonQuery();
                conn.Close();
                MessageBox.Show("Added Successfuly..");
        

        }

        DataSet ds = new DataSet();
        Boolean cleared = false;
        int maxRowSize;
        private void button3_Click(object sender, EventArgs e)
        {

            loadData();
            dataGridView1.DataSource = ds.Tables["ServiceDate"];
            maxRowSize = ds.Tables[0].Rows.Count;

        }


        void loadData() {
            ds = new DataSet();


            DateTime cDate = DateTime.Now;
            DateTime temp = new DateTime(cDate.Year, cDate.Month, cDate.Day);
            conn.Open();
            sql = "SELECT * from Service where DATEADD(month, 6, ChangeOil)<@cDate";

            cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("cDate", temp);
            da = new SqlDataAdapter(cmd); 
            da.Fill(ds, "ServiceDate");

            //**************************************
           cDate = DateTime.Now;
           temp = new DateTime(cDate.Year, cDate.Month, cDate.Day);
           
            sql = "SELECT * from Service where DATEADD(month, 12, ChangeFilter)<@cDate";

            cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("cDate", temp);
            da = new SqlDataAdapter(cmd);
            da.Fill(ds, "FilterDate");
          
            //***************************************
            sql = "SELECT * from Customer";
            cmd = new SqlCommand(sql, conn);
            da = new SqlDataAdapter(cmd);
            da.Fill(ds, "Customer");
            //------------------
            sql = "SELECT * from Car ";
       
            cmd = new SqlCommand(sql, conn);
            da = new SqlDataAdapter(cmd);
            da.Fill(ds, "Car");
            //------------------
            sql = "SELECT * from Service ";
            cmd = new SqlCommand(sql, conn);
            da = new SqlDataAdapter(cmd);
            da.Fill(ds, "Service");

            conn.Close();
            DataRelation objRelation = new DataRelation("custcar", ds.Tables["Customer"].Columns[1], ds.Tables["car"].Columns[2]);
            ds.Relations.Add(objRelation);
            objRelation = new DataRelation("custservice", ds.Tables["Customer"].Columns[1], ds.Tables["Service"].Columns[0]);
            ds.Relations.Add(objRelation);
            objRelation = new DataRelation("carservice", ds.Tables["Car"].Columns[0], ds.Tables["Service"].Columns[1]);


        }
        private void Form1_Load(object sender, EventArgs e)
        {
            loadData();


        }

        private void button4_Click(object sender, EventArgs e)
        {

            loadData();
            dataGridView2.DataSource= ds.Tables["Service"];
            dataGridView3.DataSource = ds.Tables["Customer"];
            dataGridView4.DataSource = ds.Tables["Car"];



        }

        private void button5_Click(object sender, EventArgs e)
        {
            sql = "SELECT * from Service";
            cmd = new SqlCommand(sql, conn);
            da = new SqlDataAdapter(cmd);
            SqlCommandBuilder cb = new SqlCommandBuilder(da);
           


            try
            {
                cb.DataAdapter.Update(ds.Tables["Service"]);
            }
            catch
            {
                MessageBox.Show("something editied went wrong , try again");
            }

        }

        private void button7_Click(object sender, EventArgs e)
        {
            sql = "SELECT * from Customer";
            cmd = new SqlCommand(sql, conn);
            da = new SqlDataAdapter(cmd);
            SqlCommandBuilder cb = new SqlCommandBuilder(da);
            try {
                cb.DataAdapter.Update(ds.Tables["Customer"]);
            }catch {
                MessageBox.Show("something editied went wrong , try again");
            }
           
        }

        private void button6_Click(object sender, EventArgs e)
        {
            sql = "SELECT * from Car";
            cmd = new SqlCommand(sql, conn);
            da = new SqlDataAdapter(cmd);
            SqlCommandBuilder cb = new SqlCommandBuilder(da);
           

            try
            {
                cb.DataAdapter.Update(ds.Tables["Car"]);
            }
            catch
            {
                MessageBox.Show("something editied went wrong , try again");
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
           loadData();
            dataGridView5.DataSource = ds.Tables["FilterDate"];
        
        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace andmebaasid
{
    public partial class Form1 : Form
    {
        SqlConnection connect = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename =|DataDirectory|\AppData\Tooded.mdf; Integrated Security = True");
        SqlCommand command;
        SqlDataAdapter adapter;
        int Id = 0;
        public Form1()
        {
            InitializeComponent();
            DisplayData();
        }

        private void DisplayData()
        {
            connect.Open();
            DataTable table = new DataTable();
            adapter = new SqlDataAdapter("SELECT * FROM Tootetable", connect);
            adapter.Fill(table);
            dataGridView1.DataSource = table;
            connect.Close();
        }
        private void ClearData()
        {
            Toodetxt.Text = "";
            Kogustxt.Text = "";
            Hindtxt.Text = "";

        }

        
        private void Form1_Load(object sender, EventArgs e)
        {
           // TODO: This line of code loads data into the 'toodedDataSet.Tootetable' table. You can move, or remove it, as needed.
           this.tootetableTableAdapter.Fill(this.toodedDataSet.Tootetable);

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void btn_Insert_Click(object sender, EventArgs e)
        {
            if (Toodetxt.Text!=""&& Kogustxt.Text!=""&&Hindtxt.Text!="")
            {
                command = new SqlCommand("insert into Tootetable(Toodenimetus,Kogus,Hind) values(@toode,@kogus,@hind)",connect);
                connect.Open();
                command.Parameters.AddWithValue("@toode", Toodetxt.Text);
                command.Parameters.AddWithValue("@kogus", Kogustxt.Text);
                command.Parameters.AddWithValue("@hind", Hindtxt.Text);
                command.ExecuteNonQuery();
                connect.Close();
                ClearData();
                DisplayData();
                MessageBox.Show("Andmed on lisatud");

            }
            else
            {
                MessageBox.Show("Viga");
            }
        }

        private void btn_Update_Click(object sender, EventArgs e)
        {
            if (Toodetxt.Text != "" && Kogustxt.Text != "" && Hindtxt.Text != "")
            {
                command = new SqlCommand("UPDATE Tootetable SET Toodenimetus=@toode,Kogus=@kogus,Hind=@hind WHERE ID=@id", connect);
                connect.Open();
                command.Parameters.AddWithValue("@id", Id);
                command.Parameters.AddWithValue("@toode", Toodetxt.Text);
                command.Parameters.AddWithValue("@kogus", Kogustxt.Text);
                command.Parameters.AddWithValue("@hind", Hindtxt.Text);
                command.ExecuteNonQuery();
                connect.Close();
                ClearData();
                DisplayData();
                MessageBox.Show("Andmed on lisatud");

            }
            else
            {
                MessageBox.Show("Viga");
            }
        }

        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            Id = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
            Toodetxt.Text=dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            Kogustxt.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            Hindtxt.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
        }
    }
}

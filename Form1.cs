using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
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
            Id = 0;
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

        

        private void btn_Update_Click(object sender, EventArgs e)
        {
            if (Toodetxt.Text != "" && Kogustxt.Text != "" && Hindtxt.Text != "")
            {
                command = new SqlCommand("UPDATE Tootetable SET Toodenimetus=@toode,Kogus=@kogus,Hind=@hind,Pilt=@pilt WHERE ID=@id", connect);
                connect.Open();
                command.Parameters.AddWithValue("@id", Id);
                command.Parameters.AddWithValue("@toode", Toodetxt.Text);
                command.Parameters.AddWithValue("@kogus", Kogustxt.Text);
                command.Parameters.AddWithValue("@hind", Hindtxt.Text.Replace(",", "."));
                command.Parameters.AddWithValue("@pilt", save.FileName + save.Filter);
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
            pictureBox1.Image = Image.FromFile(@"C:\Users\morgo\source\repos\DB\images\" + dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString());
        }

        private void btn_insert_Click(object sender, EventArgs e)
        {
            if (Toodetxt.Text != "" && Kogustxt.Text != "" && Hindtxt.Text != "")
            {
                command = new SqlCommand("insert into Tootetable(Toodenimetus,Kogus,Hind,Pilt) values(@toode,@kogus,@hind,@pilt)", connect);
                connect.Open();
                command.Parameters.AddWithValue("@toode", Toodetxt.Text);
                command.Parameters.AddWithValue("@kogus", Kogustxt.Text);
                command.Parameters.AddWithValue("@hind", Hindtxt.Text);
                command.Parameters.AddWithValue("@pilt", save.FileName + save.Filter);
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

        private void btn_Delete_Click(object sender, EventArgs e)
        {
            if (Id != 0)
            {
                command = new SqlCommand("DELETE Tootetable WHERE Id=@id", connect);
                connect.Open();
                command.Parameters.AddWithValue("@id", Id);

                command.ExecuteNonQuery();
                connect.Close();
                DisplayData();
                ClearData();

                MessageBox.Show("Andmed on lisatud");

            }
            else
            {
                MessageBox.Show("Viga kustutamisega");
            }
        }
        SaveFileDialog save;

        private void btn_Lisapilt_Click(object sender, EventArgs e)
        {
            if (Id != 0)
            {
                OpenFileDialog open = new OpenFileDialog();
                open.Filter = "Image Files (*.jpeg;*.bmp;*.png;*.jpg)|*.jpeg;*.bmp;*.png;*.jpg";
                if (open.ShowDialog() == DialogResult.OK)
                {
                    SaveFileDialog save = new SaveFileDialog();
                    save.FileName = Toodetxt.Text + "_" + Id;
                    save.Filter = "Image Files (*.jpeg;*.bmp;*.png;*.jpg)|*.jpeg;*.bmp;*.png;*.jpg";
                    save.InitialDirectory = Path.GetFullPath(@"C:\Users\morgo\source\repos\DB\images\");
                    save.ShowDialog();
                }
            }
            else
            {
                MessageBox.Show("Viga");
            }

        }
        
    }
}

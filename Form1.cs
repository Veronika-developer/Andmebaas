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
		SqlDataAdapter adapter, adapter2;
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
            pictureBox1.Image = Image.FromFile("../../images/Allalaaditudfail.jpg");

            adapter2 = new SqlDataAdapter("SELECT Kategooria_nimetus FROM Kategooria", connect);
            DataTable kat_table = new DataTable();
            adapter2.Fill(kat_table);
            foreach(DataRow row in kat_table.Rows)
            {
                comboBox1.Items.Add(row["Kategooria_nimetus"]);
            }
            connect.Close();
        }
        private void ClearData()
        {
            Id = 0;
            Toodetxt.Text = "";
            Kogustxt.Text = "";
            Hindtxt.Text = "";
            pictureBox1.Image = Image.FromFile("../../images/Allaladitudfail.jpg");

        }

        
        private void Form1_Load(object sender, EventArgs e)
        {
           // TODO: This line of code loads data into the 'toodedDataSet.Tootetable' table. You can move, or remove it, as needed.
           this.tootetableTableAdapter.Fill(this.toodedDataSet.Tootetable);

        }

        

        private void btn_Update_Click(object sender, EventArgs e)
        {
            if (Toodetxt.Text != "" && Kogustxt.Text != "" && Hindtxt.Text != "")
            {
                command = new SqlCommand("UPDATE Tootetable SET Toodenimetus=@toode,Kogus=@kogus,Hind=@hind,Pilt=@pilt, Kategooria_id=@kat WHERE ID=@id", connect);
                connect.Open();
                command.Parameters.AddWithValue("@id", Id);
                command.Parameters.AddWithValue("@toode", Toodetxt.Text);
                command.Parameters.AddWithValue("@kogus", Kogustxt.Text);
                command.Parameters.AddWithValue("@hind", Hindtxt.Text.Replace(",", "."));
                string file_pilt = Toodetxt.Text + "jpg";
                command.Parameters.AddWithValue("@pilt", file_pilt);
                command.Parameters.AddWithValue("@kat", (comboBox1.SelectedIndex + 1));
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
            pictureBox1.Image = Image.FromFile(@"C:\Users\opilane\source\repos\Jefimova\DB\images\" + dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString());
            string v = dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString();
            comboBox1.SelectedIndex = Int32.Parse(v)-1;
        }

        private void btn_insert_Click(object sender, EventArgs e)
        {
            if (Toodetxt.Text != "" && Kogustxt.Text != "" && Hindtxt.Text != "" && comboBox1.SelectedItem != null)
            {
                command = new SqlCommand("insert into Tootetable(Toodenimetus,Kogus,Hind,Pilt,Kategooria_id) values(@toode,@kogus,@hind,@pilt,@kat)", connect);
                connect.Open();
                command.Parameters.AddWithValue("@toode", Toodetxt.Text);
                command.Parameters.AddWithValue("@kogus", Kogustxt.Text);
                command.Parameters.AddWithValue("@hind", Hindtxt.Text);
                string file_pilt = Toodetxt.Text + "jpg";
                command.Parameters.AddWithValue("@pilt", file_pilt);
                command.Parameters.AddWithValue("@kat", (comboBox1.SelectedIndex + 1));
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
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Image Files (*.jpeg;*.bmp;*.png;*.jpg)|*.jpeg;*.bmp;*.png;*.jpg";
            open.InitialDirectory = Path.GetFullPath(@"C:\Users\opilane\source\repos\Jefimova\DB\images");
            if (open.ShowDialog() == DialogResult.OK)
            {
                SaveFileDialog save = new SaveFileDialog();
                save.FileName = Toodetxt.Text + ".jpg";
                save.Filter = "Image Files (*.jpeg;*.bmp;*.png;*.jpg)|*.jpeg;*.bmp;*.png;*.jpg";
                save.InitialDirectory = Path.GetFullPath(@"C:\Users\opilane\source\repos\Jefimova\DB\images");
                if (open.ShowDialog() == DialogResult.OK)
                {
                    File.Copy(open.FileName, save.FileName);
                    save.RestoreDirectory = true;
                    pictureBox1.Image = Image.FromFile(save.FileName);
                }
            }
            else
            {
                MessageBox.Show("Viga");
            }

        }
        
    }
}

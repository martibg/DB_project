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
using MySql.Data.MySqlClient;

namespace DB_project
{
    public partial class Form1 : Form
    {

        string connectionString = "server=localhost;user=root;password=Qwe1209poi!;database=CourseEnrollment;";
        int selectedId = -1;

        public Form1()
        {
            InitializeComponent();
            LoadData();
            comboLevel.Items.AddRange(new string[] { "начално", "средно", "напреднало" });
        }
        private void LoadData()
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string query = "SELECT * FROM Enrollments";
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dataGridView1.DataSource = dt;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string query = "INSERT INTO Enrollments (FullName, CourseName, Level, StartDate) VALUES (@name, @course, @level, @date)";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@name", txtFullName.Text);
                cmd.Parameters.AddWithValue("@course", txtCourse.Text);
                cmd.Parameters.AddWithValue("@level", comboLevel.Text);
                cmd.Parameters.AddWithValue("@date", dateStart.Value.Date);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
                LoadData();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedId == -1)
            {
                MessageBox.Show("Моля, изберете запис за редактиране.");
                return;
            }

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string query = "UPDATE Enrollments SET FullName=@name, CourseName=@course, Level=@level, StartDate=@date WHERE Id=@id";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@name", txtFullName.Text);
                cmd.Parameters.AddWithValue("@course", txtCourse.Text);
                cmd.Parameters.AddWithValue("@level", comboLevel.Text);
                cmd.Parameters.AddWithValue("@date", dateStart.Value.Date);
                cmd.Parameters.AddWithValue("@id", selectedId);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
                LoadData();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedId == -1)
            {
                MessageBox.Show("Моля, изберете запис за изтриване.");
                return;
            }

            DialogResult result = MessageBox.Show("Сигурни ли сте, че искате да изтриете този запис?", "Потвърждение", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    string query = "DELETE FROM Enrollments WHERE Id=@id";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", selectedId);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    LoadData();
                }
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                selectedId = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["Id"].Value);
                txtFullName.Text = dataGridView1.Rows[e.RowIndex].Cells["FullName"].Value.ToString();
                txtCourse.Text = dataGridView1.Rows[e.RowIndex].Cells["CourseName"].Value.ToString();
                comboLevel.Text = dataGridView1.Rows[e.RowIndex].Cells["Level"].Value.ToString();
                dateStart.Value = Convert.ToDateTime(dataGridView1.Rows[e.RowIndex].Cells["StartDate"].Value);
            }
        }
    }
    
}


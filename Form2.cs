using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Form2 : Form
    {
        public static Form2 Instance;

        public Form2()
        {
            InitializeComponent();
            Instance = this; 
        }
        
        public static void PrintTruthTable(int[,] truthTable, int numRows, int numVar, char[] var)
        {
            Instance.dataGridView1.Columns.Clear();
            Instance.dataGridView1.Rows.Clear();

            for (int j = 0; j < numVar + 1; ++j)
            {
                Instance.dataGridView1.Columns.Add(var[j].ToString(), var[j].ToString());
            }

            for (int i = 0; i < numRows; ++i)
            {
                object[] rowValues = new object[numVar + 1];
                for (int j = 0; j < numVar + 1; ++j)
                {
                    rowValues[j] = truthTable[i, j];
                }
                Instance.dataGridView1.Rows.Add(rowValues);
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}

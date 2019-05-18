using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using Excel = Microsoft.Office.Interop.Excel;

namespace PiersonProject
{
    public partial class TableGrid : Form
    {
        public static Excel.Application MyApp = null;

        public TableGrid()
        {
            InitializeComponent();

            TableGrid.MyApp = new Excel.Application();

            this.openFileDialog1.FileName = @"C:\Users\misha\University\C#\PiersonProject\PiersonProject\DataTable.txt";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.startProgress();

            List<item> pair_of_values = null;

            try
            {
                if (this.checkRead.Checked)
                {
                    pair_of_values = ExtensionClass.generateFromFile(this.openFileDialog1.FileName);
                }
                else
                {
                    pair_of_values = ExtensionClass.generateNumbers();
                }

                item.N = pair_of_values.Sum(delegate (item val) { return val.Ni; });
                item.lamb = pair_of_values.Sum(delegate (item val) { return val.XiNi; }) / item.N;

                ExtensionClass.mergeCells(pair_of_values);

                if (pair_of_values.Count <= 2)
                {
                    throw new Exception();
                }

                this.ShowResult(pair_of_values);

                this.SumP.Text = pair_of_values.Sum(delegate (item val) { return val.Pi; }).ToString();
            }
            catch (Exception)
            {
                MessageBox.Show("Smth went wrong!!! Problem can be detected in given file..", "PiersonProjectError", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
          
      
        }

        public  void startProgress()
        {
          this.progress.Value = 0;

          for (int i = 0; i < 4; ++i)
          {
            this.progress.Value += 5;
          }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.labelX1.Text = "value";
            this.labelX2.Text = "value";
            this.tableData.DataSource = null;
            this.progress.Value = 0;
        }

        private void ShowResult(List<item> pair_of_values)
        {
            this.tableData.DataSource = pair_of_values;

            double X1 = pair_of_values.Sum(delegate (item val) { return val.value; });
            double X2 = Math.Round(MyApp.WorksheetFunction.ChiInv(double.Parse((this.alphaval.Text)), pair_of_values.Count - 2), 5);

            this.labelX1.Text = X1.ToString();
            this.labelX2.Text = X2.ToString();

            if (X1 < X2)
            {
                this.answ.Text = "Hypothesis is accepted!!!!!";
            }
            else
            {
                this.answ.Text = "Hypothesis is NOT accepted!!!!!";
            }
        }

        private void searchFile_Click(object sender, EventArgs e)
        {
            this.openFileDialog1.ShowDialog();
        }
    }
}

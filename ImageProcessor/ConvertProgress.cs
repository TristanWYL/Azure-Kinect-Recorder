using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageProcessor
{
    public partial class ConvertProgress : Form
    {
        public ConvertProgress()
        {
            InitializeComponent();
            listView1.Items.Clear();
        }

        private void ConvertProgress_Load(object sender, EventArgs e)
        {

        }

        private void AddListViewItem(String name, int progress) {
            
            ListViewItem lvi = new ListViewItem();
            //lvi.SubItems[0].Text = name+" Of Subitem";
            lvi.SubItems[0].Text = name;
            //ProgressBar cbox = new ProgressBar();
            //cbox.Value = 50;
            listView1.Items.Add(lvi);
            //listView1.Controls.Add(cbox);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddListViewItem("Hello", 1);
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}

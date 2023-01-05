using EVerse.Navisworks.Plugin.ViewpointByLevel.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EVerse.Navisworks.Plugin.ViewpointByLevel
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

        }

        //public void modelsNames_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    //Tools.SelectedSystem = modelsNames.SelectedIndex;
        //}

        //private void button1_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        //Tools.CutOffset = Convert.ToDouble(textBox1.Text);

        //    }

        //    catch
        //    {
        //        MessageBox.Show("Error with offset" + Environment.NewLine + "Using 1");
        //        Tools.CutOffset = 1;
        //    }


        //    this.DialogResult = DialogResult.OK;
        //}

        //private void button2_Click(object sender, EventArgs e)
        //{
        //    this.DialogResult = DialogResult.Cancel;
        //}
        public void FillModels(Tools.GridSystems gs)
        {
            //modelsNames.Items.AddRange(gs.Models.ToArray());
        }
    }
}

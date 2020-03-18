using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bubble_Leak_Detection
{
    public partial class CaptureSingleImage : Form
    {
        public string SaveDirectory;

        public CaptureSingleImage(string dir)
        {
            InitializeComponent();
            SaveDirectory = dir;
            tbSaveDirectory.Text = SaveDirectory;
            tbSector1.Text = "Sector1";
            tbSector2.Text = "Sector2";
            tbSector3.Text = "Sector3";
            tbSector4.Text = "Sector4";
            tbSector5.Text = "Sector5";
            tbSector6.Text = "Sector6";
            tbSector7.Text = "Sector7";
            tbSector8.Text = "Sector8";
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            
        }

        private void rbSector_CheckedChanged(object sender, EventArgs e)
        {
            if (rbSector.Checked)
            {
                tbSector1.Text = "Sector1";
                tbSector2.Text = "Sector2";
                tbSector3.Text = "Sector3";
                tbSector4.Text = "Sector4";
                tbSector5.Text = "Sector5";
                tbSector6.Text = "Sector6";
                tbSector7.Text = "Sector7";
                tbSector8.Text = "Sector8";
            }
            else
            {
                tbSector1.Text = "ROI1";
                tbSector2.Text = "ROI2";
                tbSector3.Text = "ROI3";
                tbSector4.Text = "ROI4";
                tbSector5.Text = "ROI5";
                tbSector6.Text = "ROI6";
                tbSector7.Text = "ROI7";
                tbSector8.Text = "ROI8";
            }
        }
    }
}

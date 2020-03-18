using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Bubble_Leak_Detection
{
    public partial class WriteAvi : Form
    {
        public WriteAvi(TIS.Imaging.ICImagingControl IC )
        {
            InitializeComponent();
            ICControl = IC;
        }

        private TIS.Imaging.ICImagingControl ICControl; 

        /// <summary>
        /// writeavi_Load
        ///
        /// Display all available video codecs in a combo box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		        private void WriteAvi_Load(object sender, EventArgs e)
        {
            cboVideoCodec.DataSource = ICControl.AviCompressors;

            // Show the first codec in the combobox.
            cboVideoCodec.SelectedIndex = 0;
            cmdStopCapture.Enabled = true;
            cmdStartCapture.Enabled = true;

        }
		
        /// <summary>
        /// cboVideoCodec_SelectedValueChanged
        ///
        /// Handle the change of the current selection in the cvbVideoCodec combo box. If
        /// the selection has changed, it is checked whether the codec as a properties
        /// dialog.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		        private void cboVideoCodec_SelectedIndexChanged(object sender, EventArgs e)
        {
            TIS.Imaging.AviCompressor Codec;
            // Retrieve the codec from the cboVideoCodec combobox.
            Codec = (TIS.Imaging.AviCompressor)cboVideoCodec.SelectedItem;

            //Check for the configuration dialog.
            if (Codec.PropertyPageAvailable)
            {
                cmdShowPropertyPage.Enabled = true;
            }
            else
            {
                cmdShowPropertyPage.Enabled = false;
            }
        }
		
        /// <summary>
        /// cmdShowPropertyPage_Click
        ///
        /// Show the property dialog of the currently selected codec.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		        private void cmdShowPropertyPage_Click(object sender, EventArgs e)
        {
            TIS.Imaging.AviCompressor Codec;
            // Retrieve the codec from the cboVideoCodec combobox.
            Codec = (TIS.Imaging.AviCompressor)cboVideoCodec.SelectedItem;
            Codec.ShowPropertyPage();
        }
		
        /// <summary>
        /// cmdFilename_Click
        ///
        /// Select a filename for the AVI file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdFilename_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "avi files (*.avi)|*.avi|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtFilename.Text = saveFileDialog1.FileName;
            }
        }

        /// <summary>
        /// cmdStartCapture
        ///
        /// Start avi capture with the selected filename and codec.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		        private void cmdStartCapture_Click(object sender, EventArgs e)
        {
            if (txtFilename.Text == "")
            {
                MessageBox.Show("Please select an AVI filename first.");
            }
            else
            {
                ICControl.AviStartCapture(txtFilename.Text, cboVideoCodec.SelectedItem.ToString());
                cmdStopCapture.Enabled = true;
                cmdStartCapture.Enabled = true;
            }
        }
		
        /// <summary>
        /// cmdStopCapture_Click
        ///
        /// Stop video capture.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdStopCapture_Click(object sender, EventArgs e)
        {
            ICControl.AviStopCapture();
            cmdStopCapture.Enabled = false;
            cmdStartCapture.Enabled = true;
        }

        /// <summary>
        /// chkPause_Click
        ///
        /// Pause or restart the avi capture according to the value in chkPause.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		        private void chkPause_CheckedChanged(object sender, EventArgs e)
        {
            ICControl.LiveCapturePause = chkPause.Checked;
        }
		
    }
}
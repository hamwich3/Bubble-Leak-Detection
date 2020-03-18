namespace Bubble_Leak_Detection
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.icImagingControl1 = new TIS.Imaging.ICImagingControl();
            this.icImagingControl2 = new TIS.Imaging.ICImagingControl();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.camera1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.device1SettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.properties1TToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.camera2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.device2SettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.properties2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stopLiveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.startLiveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showCaptureControlsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gbCaptureImages = new System.Windows.Forms.GroupBox();
            this.label19 = new System.Windows.Forms.Label();
            this.btnCaptureMultiple = new System.Windows.Forms.Button();
            this.btnCaptureSingle = new System.Windows.Forms.Button();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.rbSector = new System.Windows.Forms.RadioButton();
            this.rbROI = new System.Windows.Forms.RadioButton();
            this.label15 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.tbSector8 = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.tbSector7 = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.tbSector6 = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.tbSector5 = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.tbSector4 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.tbSector3 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tbSector2 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tbSector1 = new System.Windows.Forms.TextBox();
            this.timerCapture = new System.Windows.Forms.Timer(this.components);
            this.button1 = new System.Windows.Forms.Button();
            this.tbRadius = new System.Windows.Forms.TextBox();
            this.label21 = new System.Windows.Forms.Label();
            this.btnFail1 = new System.Windows.Forms.Button();
            this.btnFail2 = new System.Windows.Forms.Button();
            this.btnFail3 = new System.Windows.Forms.Button();
            this.btnFail4 = new System.Windows.Forms.Button();
            this.btnFail5 = new System.Windows.Forms.Button();
            this.btnFail8 = new System.Windows.Forms.Button();
            this.btnFail7 = new System.Windows.Forms.Button();
            this.btnFail6 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.icImagingControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.icImagingControl2)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.gbCaptureImages.SuspendLayout();
            this.SuspendLayout();
            // 
            // icImagingControl1
            // 
            this.icImagingControl1.BackColor = System.Drawing.Color.White;
            this.icImagingControl1.DeviceListChangedExecutionMode = TIS.Imaging.EventExecutionMode.Invoke;
            this.icImagingControl1.DeviceLostExecutionMode = TIS.Imaging.EventExecutionMode.AsyncInvoke;
            this.icImagingControl1.DeviceState = resources.GetString("icImagingControl1.DeviceState");
            this.icImagingControl1.ImageAvailableExecutionMode = TIS.Imaging.EventExecutionMode.MultiThreaded;
            this.icImagingControl1.LiveDisplayPosition = new System.Drawing.Point(0, 0);
            this.icImagingControl1.Location = new System.Drawing.Point(1, 46);
            this.icImagingControl1.Name = "icImagingControl1";
            this.icImagingControl1.Size = new System.Drawing.Size(948, 712);
            this.icImagingControl1.TabIndex = 1;
            this.icImagingControl1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.icImagingControl1_MouseDown);
            this.icImagingControl1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.icImagingControl_MouseMove);
            this.icImagingControl1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.icImagingControl_MouseUp);
            // 
            // icImagingControl2
            // 
            this.icImagingControl2.BackColor = System.Drawing.Color.White;
            this.icImagingControl2.DeviceListChangedExecutionMode = TIS.Imaging.EventExecutionMode.Invoke;
            this.icImagingControl2.DeviceLostExecutionMode = TIS.Imaging.EventExecutionMode.AsyncInvoke;
            this.icImagingControl2.ImageAvailableExecutionMode = TIS.Imaging.EventExecutionMode.MultiThreaded;
            this.icImagingControl2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.icImagingControl2.LiveDisplayPosition = new System.Drawing.Point(0, 0);
            this.icImagingControl2.Location = new System.Drawing.Point(956, 46);
            this.icImagingControl2.Name = "icImagingControl2";
            this.icImagingControl2.Size = new System.Drawing.Size(948, 712);
            this.icImagingControl2.TabIndex = 1;
            this.icImagingControl2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.icImagingControl2_MouseDown);
            this.icImagingControl2.MouseMove += new System.Windows.Forms.MouseEventHandler(this.icImagingControl_MouseMove);
            this.icImagingControl2.MouseUp += new System.Windows.Forms.MouseEventHandler(this.icImagingControl_MouseUp);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.settingsToolStripMenuItem,
            this.toolsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1904, 24);
            this.menuStrip1.TabIndex = 5;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.camera1ToolStripMenuItem,
            this.camera2ToolStripMenuItem,
            this.stopLiveToolStripMenuItem,
            this.startLiveToolStripMenuItem});
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(59, 20);
            this.settingsToolStripMenuItem.Text = "Devices";
            // 
            // camera1ToolStripMenuItem
            // 
            this.camera1ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.device1SettingsToolStripMenuItem,
            this.properties1TToolStripMenuItem});
            this.camera1ToolStripMenuItem.Name = "camera1ToolStripMenuItem";
            this.camera1ToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.camera1ToolStripMenuItem.Text = "Camera1 (left)";
            // 
            // device1SettingsToolStripMenuItem
            // 
            this.device1SettingsToolStripMenuItem.Name = "device1SettingsToolStripMenuItem";
            this.device1SettingsToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.device1SettingsToolStripMenuItem.Text = "Device Settings";
            this.device1SettingsToolStripMenuItem.Click += new System.EventHandler(this.device1SettingsToolStripMenuItem_Click);
            // 
            // properties1TToolStripMenuItem
            // 
            this.properties1TToolStripMenuItem.Name = "properties1TToolStripMenuItem";
            this.properties1TToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.properties1TToolStripMenuItem.Text = "Properties";
            this.properties1TToolStripMenuItem.Click += new System.EventHandler(this.properties1TToolStripMenuItem_Click);
            // 
            // camera2ToolStripMenuItem
            // 
            this.camera2ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.device2SettingsToolStripMenuItem,
            this.properties2ToolStripMenuItem});
            this.camera2ToolStripMenuItem.Name = "camera2ToolStripMenuItem";
            this.camera2ToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.camera2ToolStripMenuItem.Text = "Camera2 (right)";
            // 
            // device2SettingsToolStripMenuItem
            // 
            this.device2SettingsToolStripMenuItem.Name = "device2SettingsToolStripMenuItem";
            this.device2SettingsToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.device2SettingsToolStripMenuItem.Text = "Device Settings";
            this.device2SettingsToolStripMenuItem.Click += new System.EventHandler(this.device2SettingsToolStripMenuItem_Click);
            // 
            // properties2ToolStripMenuItem
            // 
            this.properties2ToolStripMenuItem.Name = "properties2ToolStripMenuItem";
            this.properties2ToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.properties2ToolStripMenuItem.Text = "Properties";
            this.properties2ToolStripMenuItem.Click += new System.EventHandler(this.properties2ToolStripMenuItem_Click);
            // 
            // stopLiveToolStripMenuItem
            // 
            this.stopLiveToolStripMenuItem.Name = "stopLiveToolStripMenuItem";
            this.stopLiveToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.stopLiveToolStripMenuItem.Text = "Stop Live";
            this.stopLiveToolStripMenuItem.Click += new System.EventHandler(this.stopLiveToolStripMenuItem_Click);
            // 
            // startLiveToolStripMenuItem
            // 
            this.startLiveToolStripMenuItem.Name = "startLiveToolStripMenuItem";
            this.startLiveToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.startLiveToolStripMenuItem.Text = "Start Live";
            this.startLiveToolStripMenuItem.Click += new System.EventHandler(this.startLiveToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showCaptureControlsToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(46, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // showCaptureControlsToolStripMenuItem
            // 
            this.showCaptureControlsToolStripMenuItem.Name = "showCaptureControlsToolStripMenuItem";
            this.showCaptureControlsToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.showCaptureControlsToolStripMenuItem.Text = "Show Capture Controls";
            this.showCaptureControlsToolStripMenuItem.Click += new System.EventHandler(this.showCaptureControlsToolStripMenuItem_Click);
            // 
            // gbCaptureImages
            // 
            this.gbCaptureImages.Controls.Add(this.label19);
            this.gbCaptureImages.Controls.Add(this.btnCaptureMultiple);
            this.gbCaptureImages.Controls.Add(this.btnCaptureSingle);
            this.gbCaptureImages.Controls.Add(this.textBox3);
            this.gbCaptureImages.Controls.Add(this.rbSector);
            this.gbCaptureImages.Controls.Add(this.rbROI);
            this.gbCaptureImages.Controls.Add(this.label15);
            this.gbCaptureImages.Controls.Add(this.label16);
            this.gbCaptureImages.Controls.Add(this.tbSector8);
            this.gbCaptureImages.Controls.Add(this.label13);
            this.gbCaptureImages.Controls.Add(this.button1);
            this.gbCaptureImages.Controls.Add(this.label14);
            this.gbCaptureImages.Controls.Add(this.tbSector7);
            this.gbCaptureImages.Controls.Add(this.label11);
            this.gbCaptureImages.Controls.Add(this.label12);
            this.gbCaptureImages.Controls.Add(this.tbSector6);
            this.gbCaptureImages.Controls.Add(this.label9);
            this.gbCaptureImages.Controls.Add(this.label10);
            this.gbCaptureImages.Controls.Add(this.tbSector5);
            this.gbCaptureImages.Controls.Add(this.label7);
            this.gbCaptureImages.Controls.Add(this.label8);
            this.gbCaptureImages.Controls.Add(this.tbSector4);
            this.gbCaptureImages.Controls.Add(this.label5);
            this.gbCaptureImages.Controls.Add(this.label6);
            this.gbCaptureImages.Controls.Add(this.tbSector3);
            this.gbCaptureImages.Controls.Add(this.label3);
            this.gbCaptureImages.Controls.Add(this.label4);
            this.gbCaptureImages.Controls.Add(this.tbSector2);
            this.gbCaptureImages.Controls.Add(this.label2);
            this.gbCaptureImages.Controls.Add(this.label1);
            this.gbCaptureImages.Controls.Add(this.tbSector1);
            this.gbCaptureImages.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.gbCaptureImages.Location = new System.Drawing.Point(0, 931);
            this.gbCaptureImages.Name = "gbCaptureImages";
            this.gbCaptureImages.Size = new System.Drawing.Size(1904, 110);
            this.gbCaptureImages.TabIndex = 6;
            this.gbCaptureImages.TabStop = false;
            this.gbCaptureImages.Text = "Capture Images";
            this.gbCaptureImages.Visible = false;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(27, -17);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(194, 13);
            this.label19.TabIndex = 12;
            this.label19.Text = "Object Detection Threshold (0.00-1.00):";
            // 
            // btnCaptureMultiple
            // 
            this.btnCaptureMultiple.Location = new System.Drawing.Point(1198, 48);
            this.btnCaptureMultiple.Name = "btnCaptureMultiple";
            this.btnCaptureMultiple.Size = new System.Drawing.Size(176, 37);
            this.btnCaptureMultiple.TabIndex = 51;
            this.btnCaptureMultiple.Text = "Start Multiple Capture";
            this.btnCaptureMultiple.UseVisualStyleBackColor = true;
            this.btnCaptureMultiple.Click += new System.EventHandler(this.btnCaptureMultiple_Click);
            // 
            // btnCaptureSingle
            // 
            this.btnCaptureSingle.Location = new System.Drawing.Point(1071, 48);
            this.btnCaptureSingle.Name = "btnCaptureSingle";
            this.btnCaptureSingle.Size = new System.Drawing.Size(121, 37);
            this.btnCaptureSingle.TabIndex = 51;
            this.btnCaptureSingle.Text = "Capture Single";
            this.btnCaptureSingle.UseVisualStyleBackColor = true;
            this.btnCaptureSingle.Click += new System.EventHandler(this.btnCaptureSingle_Click);
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(227, -20);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(100, 20);
            this.textBox3.TabIndex = 11;
            // 
            // rbSector
            // 
            this.rbSector.AutoSize = true;
            this.rbSector.Checked = true;
            this.rbSector.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbSector.Location = new System.Drawing.Point(993, 31);
            this.rbSector.Name = "rbSector";
            this.rbSector.Size = new System.Drawing.Size(72, 20);
            this.rbSector.TabIndex = 49;
            this.rbSector.TabStop = true;
            this.rbSector.Text = "Sectors";
            this.rbSector.UseVisualStyleBackColor = true;
            this.rbSector.CheckedChanged += new System.EventHandler(this.rbSector_CheckedChanged);
            // 
            // rbROI
            // 
            this.rbROI.AutoSize = true;
            this.rbROI.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbROI.Location = new System.Drawing.Point(993, 67);
            this.rbROI.Name = "rbROI";
            this.rbROI.Size = new System.Drawing.Size(56, 20);
            this.rbROI.TabIndex = 50;
            this.rbROI.Text = "ROIs";
            this.rbROI.UseVisualStyleBackColor = true;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(931, 69);
            this.label15.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(38, 16);
            this.label15.TabIndex = 48;
            this.label15.Text = ".bmp";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.Location = new System.Drawing.Point(758, 69);
            this.label16.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(57, 16);
            this.label16.TabIndex = 47;
            this.label16.Text = "Sector8:";
            // 
            // tbSector8
            // 
            this.tbSector8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbSector8.Location = new System.Drawing.Point(823, 69);
            this.tbSector8.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tbSector8.Name = "tbSector8";
            this.tbSector8.Size = new System.Drawing.Size(100, 22);
            this.tbSector8.TabIndex = 46;
            this.tbSector8.Text = "Sector8";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(683, 69);
            this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(38, 16);
            this.label13.TabIndex = 45;
            this.label13.Text = ".bmp";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(510, 69);
            this.label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(57, 16);
            this.label14.TabIndex = 44;
            this.label14.Text = "Sector7:";
            // 
            // tbSector7
            // 
            this.tbSector7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbSector7.Location = new System.Drawing.Point(575, 69);
            this.tbSector7.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tbSector7.Name = "tbSector7";
            this.tbSector7.Size = new System.Drawing.Size(100, 22);
            this.tbSector7.TabIndex = 43;
            this.tbSector7.Text = "Sector7";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(437, 69);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(38, 16);
            this.label11.TabIndex = 42;
            this.label11.Text = ".bmp";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(264, 69);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(57, 16);
            this.label12.TabIndex = 41;
            this.label12.Text = "Sector6:";
            // 
            // tbSector6
            // 
            this.tbSector6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbSector6.Location = new System.Drawing.Point(329, 69);
            this.tbSector6.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tbSector6.Name = "tbSector6";
            this.tbSector6.Size = new System.Drawing.Size(100, 22);
            this.tbSector6.TabIndex = 40;
            this.tbSector6.Text = "Sector6";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(200, 69);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(38, 16);
            this.label9.TabIndex = 39;
            this.label9.Text = ".bmp";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(27, 69);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(57, 16);
            this.label10.TabIndex = 38;
            this.label10.Text = "Sector5:";
            // 
            // tbSector5
            // 
            this.tbSector5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbSector5.Location = new System.Drawing.Point(92, 69);
            this.tbSector5.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tbSector5.Name = "tbSector5";
            this.tbSector5.Size = new System.Drawing.Size(100, 22);
            this.tbSector5.TabIndex = 37;
            this.tbSector5.Text = "Sector5";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(931, 33);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(38, 16);
            this.label7.TabIndex = 36;
            this.label7.Text = ".bmp";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(758, 33);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(57, 16);
            this.label8.TabIndex = 35;
            this.label8.Text = "Sector4:";
            // 
            // tbSector4
            // 
            this.tbSector4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbSector4.Location = new System.Drawing.Point(823, 33);
            this.tbSector4.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tbSector4.Name = "tbSector4";
            this.tbSector4.Size = new System.Drawing.Size(100, 22);
            this.tbSector4.TabIndex = 34;
            this.tbSector4.Text = "Sector4";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(683, 33);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(38, 16);
            this.label5.TabIndex = 33;
            this.label5.Text = ".bmp";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(510, 33);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(57, 16);
            this.label6.TabIndex = 32;
            this.label6.Text = "Sector3:";
            // 
            // tbSector3
            // 
            this.tbSector3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbSector3.Location = new System.Drawing.Point(575, 33);
            this.tbSector3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tbSector3.Name = "tbSector3";
            this.tbSector3.Size = new System.Drawing.Size(100, 22);
            this.tbSector3.TabIndex = 31;
            this.tbSector3.Text = "Sector3";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(437, 33);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 16);
            this.label3.TabIndex = 30;
            this.label3.Text = ".bmp";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(264, 33);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 16);
            this.label4.TabIndex = 29;
            this.label4.Text = "Sector2:";
            // 
            // tbSector2
            // 
            this.tbSector2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbSector2.Location = new System.Drawing.Point(329, 33);
            this.tbSector2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tbSector2.Name = "tbSector2";
            this.tbSector2.Size = new System.Drawing.Size(100, 22);
            this.tbSector2.TabIndex = 28;
            this.tbSector2.Text = "Sector2";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(200, 33);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 16);
            this.label2.TabIndex = 27;
            this.label2.Text = ".bmp";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(27, 33);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 16);
            this.label1.TabIndex = 26;
            this.label1.Text = "Sector1:";
            // 
            // tbSector1
            // 
            this.tbSector1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbSector1.Location = new System.Drawing.Point(92, 33);
            this.tbSector1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tbSector1.Name = "tbSector1";
            this.tbSector1.Size = new System.Drawing.Size(100, 22);
            this.tbSector1.TabIndex = 6;
            this.tbSector1.Text = "Sector1";
            // 
            // timerCapture
            // 
            this.timerCapture.Interval = 600;
            this.timerCapture.Tick += new System.EventHandler(this.timerCapture_Tick);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(1501, 48);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(101, 23);
            this.button1.TabIndex = 10;
            this.button1.Text = "Find Cylinders";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_3);
            // 
            // tbRadius
            // 
            this.tbRadius.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbRadius.Location = new System.Drawing.Point(174, 789);
            this.tbRadius.Name = "tbRadius";
            this.tbRadius.Size = new System.Drawing.Size(52, 29);
            this.tbRadius.TabIndex = 1;
            this.tbRadius.Text = "150";
            this.tbRadius.Leave += new System.EventHandler(this.tbRadius_Leave);
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label21.Location = new System.Drawing.Point(12, 789);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(156, 24);
            this.label21.TabIndex = 12;
            this.label21.Text = "Radius (30 - 160):";
            // 
            // btnFail1
            // 
            this.btnFail1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFail1.Location = new System.Drawing.Point(552, 789);
            this.btnFail1.Name = "btnFail1";
            this.btnFail1.Size = new System.Drawing.Size(148, 40);
            this.btnFail1.TabIndex = 13;
            this.btnFail1.Text = "Fail Position 1";
            this.btnFail1.UseVisualStyleBackColor = true;
            this.btnFail1.Click += new System.EventHandler(this.btnFail1_Click);
            // 
            // btnFail2
            // 
            this.btnFail2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFail2.Location = new System.Drawing.Point(769, 789);
            this.btnFail2.Name = "btnFail2";
            this.btnFail2.Size = new System.Drawing.Size(148, 40);
            this.btnFail2.TabIndex = 13;
            this.btnFail2.Text = "Fail Position 2";
            this.btnFail2.UseVisualStyleBackColor = true;
            this.btnFail2.Click += new System.EventHandler(this.btnFail2_Click);
            // 
            // btnFail3
            // 
            this.btnFail3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFail3.Location = new System.Drawing.Point(986, 789);
            this.btnFail3.Name = "btnFail3";
            this.btnFail3.Size = new System.Drawing.Size(148, 40);
            this.btnFail3.TabIndex = 13;
            this.btnFail3.Text = "Fail Position 3";
            this.btnFail3.UseVisualStyleBackColor = true;
            this.btnFail3.Click += new System.EventHandler(this.btnFail3_Click);
            // 
            // btnFail4
            // 
            this.btnFail4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFail4.Location = new System.Drawing.Point(1203, 789);
            this.btnFail4.Name = "btnFail4";
            this.btnFail4.Size = new System.Drawing.Size(148, 40);
            this.btnFail4.TabIndex = 13;
            this.btnFail4.Text = "Fail Position 4";
            this.btnFail4.UseVisualStyleBackColor = true;
            this.btnFail4.Click += new System.EventHandler(this.btnFail4_Click);
            // 
            // btnFail5
            // 
            this.btnFail5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFail5.Location = new System.Drawing.Point(552, 852);
            this.btnFail5.Name = "btnFail5";
            this.btnFail5.Size = new System.Drawing.Size(148, 40);
            this.btnFail5.TabIndex = 13;
            this.btnFail5.Text = "Fail Position 5";
            this.btnFail5.UseVisualStyleBackColor = true;
            this.btnFail5.Click += new System.EventHandler(this.btnFail5_Click);
            // 
            // btnFail8
            // 
            this.btnFail8.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFail8.Location = new System.Drawing.Point(1203, 852);
            this.btnFail8.Name = "btnFail8";
            this.btnFail8.Size = new System.Drawing.Size(148, 40);
            this.btnFail8.TabIndex = 13;
            this.btnFail8.Text = "Fail Position 8";
            this.btnFail8.UseVisualStyleBackColor = true;
            this.btnFail8.Click += new System.EventHandler(this.btnFail8_Click);
            // 
            // btnFail7
            // 
            this.btnFail7.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFail7.Location = new System.Drawing.Point(986, 852);
            this.btnFail7.Name = "btnFail7";
            this.btnFail7.Size = new System.Drawing.Size(148, 40);
            this.btnFail7.TabIndex = 13;
            this.btnFail7.Text = "Fail Position 7";
            this.btnFail7.UseVisualStyleBackColor = true;
            this.btnFail7.Click += new System.EventHandler(this.btnFail7_Click);
            // 
            // btnFail6
            // 
            this.btnFail6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFail6.Location = new System.Drawing.Point(769, 852);
            this.btnFail6.Name = "btnFail6";
            this.btnFail6.Size = new System.Drawing.Size(148, 40);
            this.btnFail6.TabIndex = 13;
            this.btnFail6.Text = "Fail Position 6";
            this.btnFail6.UseVisualStyleBackColor = true;
            this.btnFail6.Click += new System.EventHandler(this.btnFail6_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1904, 1041);
            this.Controls.Add(this.btnFail8);
            this.Controls.Add(this.btnFail4);
            this.Controls.Add(this.btnFail7);
            this.Controls.Add(this.btnFail3);
            this.Controls.Add(this.btnFail5);
            this.Controls.Add(this.btnFail6);
            this.Controls.Add(this.btnFail2);
            this.Controls.Add(this.btnFail1);
            this.Controls.Add(this.label21);
            this.Controls.Add(this.tbRadius);
            this.Controls.Add(this.icImagingControl2);
            this.Controls.Add(this.icImagingControl1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.gbCaptureImages);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Form1";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.icImagingControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.icImagingControl2)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.gbCaptureImages.ResumeLayout(false);
            this.gbCaptureImages.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TIS.Imaging.ICImagingControl icImagingControl1;
        private TIS.Imaging.ICImagingControl icImagingControl2;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.GroupBox gbCaptureImages;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox tbSector8;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox tbSector7;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox tbSector6;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox tbSector5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox tbSector4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tbSector3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbSector2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbSector1;
        private System.Windows.Forms.Button btnCaptureMultiple;
        private System.Windows.Forms.Button btnCaptureSingle;
        private System.Windows.Forms.RadioButton rbSector;
        private System.Windows.Forms.RadioButton rbROI;
        private System.Windows.Forms.Timer timerCapture;
        private System.Windows.Forms.ToolStripMenuItem camera1ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem camera2ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showCaptureControlsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem device1SettingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem properties1TToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem device2SettingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem properties2ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stopLiveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem startLiveToolStripMenuItem;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox tbRadius;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Button btnFail1;
        private System.Windows.Forms.Button btnFail2;
        private System.Windows.Forms.Button btnFail3;
        private System.Windows.Forms.Button btnFail4;
        private System.Windows.Forms.Button btnFail5;
        private System.Windows.Forms.Button btnFail8;
        private System.Windows.Forms.Button btnFail7;
        private System.Windows.Forms.Button btnFail6;
    }
}


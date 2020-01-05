namespace DripTea.ViewForm
{
    partial class SplashScreen
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SplashScreen));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.progressbar1 = new Bunifu.Framework.UI.BunifuCircleProgressbar();
            this.gunaControlBox1 = new Guna.UI.WinForms.GunaControlBox();
            this.lblloading = new System.Windows.Forms.Label();
            this.bunifuColorTransition1 = new BunifuColorTransition.BunifuColorTransition(this.components);
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.timer3 = new System.Windows.Forms.Timer(this.components);
            this.bunifuElipse1 = new Bunifu.Framework.UI.BunifuElipse(this.components);
            this.bunifuDragControl1 = new Bunifu.Framework.UI.BunifuDragControl(this.components);
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(127, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(109, 161);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // progressbar1
            // 
            this.progressbar1.animated = false;
            this.progressbar1.animationIterval = 5;
            this.progressbar1.animationSpeed = 300;
            this.progressbar1.BackColor = System.Drawing.Color.Transparent;
            this.progressbar1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("progressbar1.BackgroundImage")));
            this.progressbar1.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F);
            this.progressbar1.ForeColor = System.Drawing.Color.SeaGreen;
            this.progressbar1.LabelVisible = false;
            this.progressbar1.LineProgressThickness = 5;
            this.progressbar1.LineThickness = 6;
            this.progressbar1.Location = new System.Drawing.Point(96, 8);
            this.progressbar1.Margin = new System.Windows.Forms.Padding(10, 9, 10, 9);
            this.progressbar1.MaxValue = 110;
            this.progressbar1.Name = "progressbar1";
            this.progressbar1.ProgressBackColor = System.Drawing.Color.Black;
            this.progressbar1.ProgressColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(177)))), ((int)(((byte)(195)))));
            this.progressbar1.Size = new System.Drawing.Size(170, 170);
            this.progressbar1.TabIndex = 17;
            this.progressbar1.Value = 0;
            // 
            // gunaControlBox1
            // 
            this.gunaControlBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.gunaControlBox1.AnimationHoverSpeed = 0.07F;
            this.gunaControlBox1.AnimationSpeed = 0.03F;
            this.gunaControlBox1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.gunaControlBox1.IconColor = System.Drawing.Color.White;
            this.gunaControlBox1.IconSize = 15F;
            this.gunaControlBox1.Location = new System.Drawing.Point(327, 1);
            this.gunaControlBox1.Name = "gunaControlBox1";
            this.gunaControlBox1.OnHoverBackColor = System.Drawing.Color.Transparent;
            this.gunaControlBox1.OnHoverIconColor = System.Drawing.Color.Black;
            this.gunaControlBox1.OnPressedColor = System.Drawing.Color.Transparent;
            this.gunaControlBox1.Size = new System.Drawing.Size(45, 23);
            this.gunaControlBox1.TabIndex = 20;
            // 
            // lblloading
            // 
            this.lblloading.BackColor = System.Drawing.Color.Transparent;
            this.lblloading.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblloading.Font = new System.Drawing.Font("Verdana", 11.75F, System.Drawing.FontStyle.Bold);
            this.lblloading.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblloading.Location = new System.Drawing.Point(0, 176);
            this.lblloading.Name = "lblloading";
            this.lblloading.Size = new System.Drawing.Size(372, 35);
            this.lblloading.TabIndex = 21;
            this.lblloading.Text = "Loading...";
            this.lblloading.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // bunifuColorTransition1
            // 
            this.bunifuColorTransition1.AutoTransition = true;
            this.bunifuColorTransition1.ColorArray = new System.Drawing.Color[] {
        System.Drawing.Color.Plum,
        System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(177)))), ((int)(((byte)(195))))),
        System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(175)))), ((int)(((byte)(227))))),
        System.Drawing.Color.FromArgb(((int)(((byte)(95)))), ((int)(((byte)(136)))), ((int)(((byte)(176))))),
        System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(122)))), ((int)(((byte)(187))))),
        System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(48)))), ((int)(((byte)(160))))),
        System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(128)))), ((int)(((byte)(255))))),
        System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(83)))), ((int)(((byte)(104))))),
        System.Drawing.Color.FromArgb(((int)(((byte)(219)))), ((int)(((byte)(104)))), ((int)(((byte)(48))))),
        System.Drawing.Color.FromArgb(((int)(((byte)(217)))), ((int)(((byte)(102)))), ((int)(((byte)(41))))),
        System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(155)))), ((int)(((byte)(40)))))};
            this.bunifuColorTransition1.EndColor = System.Drawing.Color.White;
            this.bunifuColorTransition1.Interval = 20;
            this.bunifuColorTransition1.ProgessValue = 0;
            this.bunifuColorTransition1.StartColor = System.Drawing.Color.White;
            this.bunifuColorTransition1.TransitionControl = this;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // timer2
            // 
            this.timer2.Interval = 7000;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // timer3
            // 
            this.timer3.Interval = 2000;
            this.timer3.Tick += new System.EventHandler(this.timer3_Tick);
            // 
            // bunifuElipse1
            // 
            this.bunifuElipse1.ElipseRadius = 5;
            this.bunifuElipse1.TargetControl = this;
            // 
            // bunifuDragControl1
            // 
            this.bunifuDragControl1.Fixed = true;
            this.bunifuDragControl1.Horizontal = true;
            this.bunifuDragControl1.TargetControl = this;
            this.bunifuDragControl1.Vertical = true;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::DripTea.Properties.Resources.DripTeaLogo1;
            this.pictureBox2.Location = new System.Drawing.Point(-5, 1);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(45, 23);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 22;
            this.pictureBox2.TabStop = false;
            // 
            // SplashScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(89)))), ((int)(((byte)(88)))));
            this.ClientSize = new System.Drawing.Size(372, 211);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.lblloading);
            this.Controls.Add(this.gunaControlBox1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.progressbar1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SplashScreen";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SplashScreen";
            this.Load += new System.EventHandler(this.SplashScreen_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private Bunifu.Framework.UI.BunifuCircleProgressbar progressbar1;
        private Guna.UI.WinForms.GunaControlBox gunaControlBox1;
        private System.Windows.Forms.Label lblloading;
        private BunifuColorTransition.BunifuColorTransition bunifuColorTransition1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.Timer timer3;
        private Bunifu.Framework.UI.BunifuElipse bunifuElipse1;
        private Bunifu.Framework.UI.BunifuDragControl bunifuDragControl1;
        private System.Windows.Forms.PictureBox pictureBox2;
    }
}
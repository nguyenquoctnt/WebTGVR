namespace AutoSendMailMuti
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
            this.btnketthuc = new System.Windows.Forms.Button();
            this.btnbatdat = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txtthoigianquetlaplai = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtnote = new System.Windows.Forms.RichTextBox();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtmailcamom = new System.Windows.Forms.TextBox();
            this.txtsomaildagui = new System.Windows.Forms.TextBox();
            this.txtmailhuydagui = new System.Windows.Forms.TextBox();
            this.backgroundWorkerFail = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorkerSapBay = new System.ComponentModel.BackgroundWorker();
            this.camonw = new System.ComponentModel.BackgroundWorker();
            this.TimeAuto = new System.Windows.Forms.Timer(this.components);
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnketthuc
            // 
            this.btnketthuc.BackColor = System.Drawing.Color.Red;
            this.btnketthuc.Enabled = false;
            this.btnketthuc.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnketthuc.ForeColor = System.Drawing.SystemColors.Control;
            this.btnketthuc.Location = new System.Drawing.Point(707, 14);
            this.btnketthuc.Name = "btnketthuc";
            this.btnketthuc.Size = new System.Drawing.Size(112, 44);
            this.btnketthuc.TabIndex = 13;
            this.btnketthuc.Text = "Kết thúc";
            this.btnketthuc.UseVisualStyleBackColor = false;
            this.btnketthuc.Click += new System.EventHandler(this.btnketthuc_Click);
            // 
            // btnbatdat
            // 
            this.btnbatdat.BackColor = System.Drawing.Color.Green;
            this.btnbatdat.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnbatdat.ForeColor = System.Drawing.SystemColors.Control;
            this.btnbatdat.Location = new System.Drawing.Point(577, 14);
            this.btnbatdat.Name = "btnbatdat";
            this.btnbatdat.Size = new System.Drawing.Size(112, 44);
            this.btnbatdat.TabIndex = 14;
            this.btnbatdat.Text = "Bắt đầu";
            this.btnbatdat.UseVisualStyleBackColor = false;
            this.btnbatdat.Click += new System.EventHandler(this.btnbatdat_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(23, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(105, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Thời gian quét lặp lại";
            // 
            // txtthoigianquetlaplai
            // 
            this.txtthoigianquetlaplai.Location = new System.Drawing.Point(154, 14);
            this.txtthoigianquetlaplai.Name = "txtthoigianquetlaplai";
            this.txtthoigianquetlaplai.Size = new System.Drawing.Size(404, 20);
            this.txtthoigianquetlaplai.TabIndex = 6;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtnote);
            this.groupBox1.Location = new System.Drawing.Point(13, 118);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(875, 325);
            this.groupBox1.TabIndex = 15;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Gửi thất bại";
            // 
            // txtnote
            // 
            this.txtnote.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtnote.Location = new System.Drawing.Point(3, 16);
            this.txtnote.Name = "txtnote";
            this.txtnote.ReadOnly = true;
            this.txtnote.Size = new System.Drawing.Size(869, 306);
            this.txtnote.TabIndex = 0;
            this.txtnote.Text = "";
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "notifyIcon1";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(23, 95);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(112, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "Số mail cám ơn đã gửi";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(23, 69);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(120, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Số mail nhắc lịch đã gửi";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(23, 43);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(94, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Số mail hủy đã gửi";
            // 
            // txtmailcamom
            // 
            this.txtmailcamom.Enabled = false;
            this.txtmailcamom.Location = new System.Drawing.Point(154, 92);
            this.txtmailcamom.Name = "txtmailcamom";
            this.txtmailcamom.Size = new System.Drawing.Size(404, 20);
            this.txtmailcamom.TabIndex = 2;
            // 
            // txtsomaildagui
            // 
            this.txtsomaildagui.Location = new System.Drawing.Point(154, 66);
            this.txtsomaildagui.Name = "txtsomaildagui";
            this.txtsomaildagui.Size = new System.Drawing.Size(404, 20);
            this.txtsomaildagui.TabIndex = 3;
            // 
            // txtmailhuydagui
            // 
            this.txtmailhuydagui.Location = new System.Drawing.Point(154, 40);
            this.txtmailhuydagui.Name = "txtmailhuydagui";
            this.txtmailhuydagui.Size = new System.Drawing.Size(404, 20);
            this.txtmailhuydagui.TabIndex = 4;
            // 
            // backgroundWorkerFail
            // 
            this.backgroundWorkerFail.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorkerFail_DoWork);
            // 
            // backgroundWorkerSapBay
            // 
            this.backgroundWorkerSapBay.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorkerSapBay_DoWork);
            // 
            // camonw
            // 
            this.camonw.DoWork += new System.ComponentModel.DoWorkEventHandler(this.camonw_DoWork);
            // 
            // TimeAuto
            // 
            this.TimeAuto.Tick += new System.EventHandler(this.TimeAuto_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 450);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnketthuc);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnbatdat);
            this.Controls.Add(this.txtmailcamom);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtsomaildagui);
            this.Controls.Add(this.txtmailhuydagui);
            this.Controls.Add(this.txtthoigianquetlaplai);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(916, 489);
            this.MinimumSize = new System.Drawing.Size(916, 489);
            this.Name = "Form1";
            this.Text = " Auto Gửi Mail : TGVR MUTI";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnketthuc;
        private System.Windows.Forms.Button btnbatdat;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtthoigianquetlaplai;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RichTextBox txtnote;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtmailcamom;
        private System.Windows.Forms.TextBox txtsomaildagui;
        private System.Windows.Forms.TextBox txtmailhuydagui;
        private System.ComponentModel.BackgroundWorker backgroundWorkerFail;
        private System.ComponentModel.BackgroundWorker backgroundWorkerSapBay;
        private System.ComponentModel.BackgroundWorker camonw;
        private System.Windows.Forms.Timer TimeAuto;
    }
}


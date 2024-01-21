
namespace analyze.win
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.carrier_code = new System.Windows.Forms.TextBox();
            this.tracking_number = new System.Windows.Forms.TextBox();
            this.status = new System.Windows.Forms.TextBox();
            this.delivered_to = new System.Windows.Forms.TextBox();
            this.signed_fo_by = new System.Windows.Forms.TextBox();
            this.delivery_location = new System.Windows.Forms.TextBox();
            this.service_type = new System.Windows.Forms.TextBox();
            this.ship_date = new System.Windows.Forms.TextBox();
            this.delivery_date = new System.Windows.Forms.TextBox();
            this.weight = new System.Windows.Forms.TextBox();
            this.recipient = new System.Windows.Forms.TextBox();
            this.shipper = new System.Windows.Forms.TextBox();
            this.reference = new System.Windows.Forms.TextBox();
            this.shipment_id = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox17 = new System.Windows.Forms.TextBox();
            this.textBox16 = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.textBox18 = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(867, 599);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.panel2);
            this.tabPage1.Controls.Add(this.panel1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(859, 573);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tp1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(628, 305);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.textBox1.Location = new System.Drawing.Point(0, 501);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(867, 98);
            this.textBox1.TabIndex = 0;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(50, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(137, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "承运商(Carrier code)：";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(32, 31);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(155, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "运单号(Tracking number)：";
            this.label2.Click += new System.EventHandler(this.label1_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(74, 56);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(113, 12);
            this.label4.TabIndex = 0;
            this.label4.Text = "包裹状态(Status)：";
            this.label4.Click += new System.EventHandler(this.label1_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(26, 80);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(161, 12);
            this.label5.TabIndex = 0;
            this.label5.Text = "收件人名称(Delivered To)：";
            this.label5.Click += new System.EventHandler(this.label1_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(44, 105);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(143, 12);
            this.label6.TabIndex = 0;
            this.label6.Text = "签字人(Signed for by)：";
            this.label6.Click += new System.EventHandler(this.label1_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(8, 130);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(179, 12);
            this.label7.TabIndex = 0;
            this.label7.Text = "收货地址(Delivery Location)：";
            this.label7.Click += new System.EventHandler(this.label1_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(38, 155);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(149, 12);
            this.label8.TabIndex = 0;
            this.label8.Text = "服务类型(Service type)：";
            this.label8.Click += new System.EventHandler(this.label1_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(56, 180);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(131, 12);
            this.label9.TabIndex = 1;
            this.label9.Text = "发货时间(Ship date)：";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(32, 205);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(155, 12);
            this.label10.TabIndex = 1;
            this.label10.Text = "妥投时间(Delivery date)：";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(98, 230);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(89, 12);
            this.label11.TabIndex = 2;
            this.label11.Text = "重量(Weight)：";
            this.label11.Click += new System.EventHandler(this.label11_Click);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(68, 255);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(119, 12);
            this.label12.TabIndex = 3;
            this.label12.Text = "签收地(Recipient)：";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(80, 280);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(107, 12);
            this.label13.TabIndex = 4;
            this.label13.Text = "发货地(Shipper)：";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(56, 305);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(131, 12);
            this.label14.TabIndex = 5;
            this.label14.Text = "证明编码(Reference)：";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(44, 330);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(143, 12);
            this.label15.TabIndex = 6;
            this.label15.Text = "发货号码(Shipment ID)：";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label18);
            this.panel1.Controls.Add(this.textBox18);
            this.panel1.Controls.Add(this.shipment_id);
            this.panel1.Controls.Add(this.label17);
            this.panel1.Controls.Add(this.reference);
            this.panel1.Controls.Add(this.textBox16);
            this.panel1.Controls.Add(this.shipper);
            this.panel1.Controls.Add(this.label16);
            this.panel1.Controls.Add(this.recipient);
            this.panel1.Controls.Add(this.textBox17);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.weight);
            this.panel1.Controls.Add(this.delivery_date);
            this.panel1.Controls.Add(this.ship_date);
            this.panel1.Controls.Add(this.service_type);
            this.panel1.Controls.Add(this.delivery_location);
            this.panel1.Controls.Add(this.signed_fo_by);
            this.panel1.Controls.Add(this.delivered_to);
            this.panel1.Controls.Add(this.status);
            this.panel1.Controls.Add(this.tracking_number);
            this.panel1.Controls.Add(this.carrier_code);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label15);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label14);
            this.panel1.Controls.Add(this.label13);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label12);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label11);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.label10);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(373, 435);
            this.panel1.TabIndex = 7;
            // 
            // carrier_code
            // 
            this.carrier_code.Location = new System.Drawing.Point(193, 4);
            this.carrier_code.Name = "carrier_code";
            this.carrier_code.Size = new System.Drawing.Size(171, 21);
            this.carrier_code.TabIndex = 7;
            // 
            // tracking_number
            // 
            this.tracking_number.Location = new System.Drawing.Point(193, 28);
            this.tracking_number.Name = "tracking_number";
            this.tracking_number.Size = new System.Drawing.Size(171, 21);
            this.tracking_number.TabIndex = 8;
            // 
            // status
            // 
            this.status.Location = new System.Drawing.Point(193, 53);
            this.status.Name = "status";
            this.status.Size = new System.Drawing.Size(171, 21);
            this.status.TabIndex = 9;
            this.status.Text = "Delivered";
            // 
            // delivered_to
            // 
            this.delivered_to.Location = new System.Drawing.Point(193, 77);
            this.delivered_to.Name = "delivered_to";
            this.delivered_to.Size = new System.Drawing.Size(171, 21);
            this.delivered_to.TabIndex = 10;
            // 
            // signed_fo_by
            // 
            this.signed_fo_by.Location = new System.Drawing.Point(193, 102);
            this.signed_fo_by.Name = "signed_fo_by";
            this.signed_fo_by.Size = new System.Drawing.Size(171, 21);
            this.signed_fo_by.TabIndex = 11;
            this.signed_fo_by.Text = "Signature not required";
            // 
            // delivery_location
            // 
            this.delivery_location.Location = new System.Drawing.Point(193, 127);
            this.delivery_location.Name = "delivery_location";
            this.delivery_location.Size = new System.Drawing.Size(171, 21);
            this.delivery_location.TabIndex = 12;
            // 
            // service_type
            // 
            this.service_type.Location = new System.Drawing.Point(193, 152);
            this.service_type.Name = "service_type";
            this.service_type.Size = new System.Drawing.Size(171, 21);
            this.service_type.TabIndex = 13;
            this.service_type.Text = "Home Delivery";
            // 
            // ship_date
            // 
            this.ship_date.Location = new System.Drawing.Point(193, 177);
            this.ship_date.Name = "ship_date";
            this.ship_date.Size = new System.Drawing.Size(171, 21);
            this.ship_date.TabIndex = 14;
            // 
            // delivery_date
            // 
            this.delivery_date.Location = new System.Drawing.Point(193, 202);
            this.delivery_date.Name = "delivery_date";
            this.delivery_date.Size = new System.Drawing.Size(171, 21);
            this.delivery_date.TabIndex = 15;
            // 
            // weight
            // 
            this.weight.Location = new System.Drawing.Point(193, 227);
            this.weight.Name = "weight";
            this.weight.Size = new System.Drawing.Size(148, 21);
            this.weight.TabIndex = 16;
            // 
            // recipient
            // 
            this.recipient.Location = new System.Drawing.Point(193, 252);
            this.recipient.Name = "recipient";
            this.recipient.Size = new System.Drawing.Size(171, 21);
            this.recipient.TabIndex = 17;
            // 
            // shipper
            // 
            this.shipper.Location = new System.Drawing.Point(193, 277);
            this.shipper.Name = "shipper";
            this.shipper.Size = new System.Drawing.Size(171, 21);
            this.shipper.TabIndex = 18;
            // 
            // reference
            // 
            this.reference.Location = new System.Drawing.Point(193, 302);
            this.reference.Name = "reference";
            this.reference.Size = new System.Drawing.Size(171, 21);
            this.reference.TabIndex = 19;
            this.reference.Text = "648363282030726";
            // 
            // shipment_id
            // 
            this.shipment_id.Location = new System.Drawing.Point(193, 327);
            this.shipment_id.Name = "shipment_id";
            this.shipment_id.Size = new System.Drawing.Size(171, 21);
            this.shipment_id.TabIndex = 20;
            // 
            // panel2
            // 
            this.panel2.Location = new System.Drawing.Point(411, 6);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(440, 423);
            this.panel2.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(32, 355);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(155, 12);
            this.label3.TabIndex = 8;
            this.label3.Text = "运单号(Tracking number)：";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // textBox17
            // 
            this.textBox17.Location = new System.Drawing.Point(193, 352);
            this.textBox17.Name = "textBox17";
            this.textBox17.Size = new System.Drawing.Size(171, 21);
            this.textBox17.TabIndex = 10;
            // 
            // textBox16
            // 
            this.textBox16.Location = new System.Drawing.Point(193, 404);
            this.textBox16.Name = "textBox16";
            this.textBox16.Size = new System.Drawing.Size(171, 21);
            this.textBox16.TabIndex = 12;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(86, 407);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(101, 12);
            this.label16.TabIndex = 11;
            this.label16.Text = "币种(Currency)：";
            // 
            // textBox18
            // 
            this.textBox18.Location = new System.Drawing.Point(193, 377);
            this.textBox18.Name = "textBox18";
            this.textBox18.Size = new System.Drawing.Size(171, 21);
            this.textBox18.TabIndex = 14;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(62, 380);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(125, 12);
            this.label17.TabIndex = 13;
            this.label17.Text = "系统单号(Order No)：";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(347, 230);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(17, 12);
            this.label18.TabIndex = 21;
            this.label18.Text = "KG";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(867, 599);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.Text = "Form1";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox delivery_date;
        private System.Windows.Forms.TextBox ship_date;
        private System.Windows.Forms.TextBox service_type;
        private System.Windows.Forms.TextBox delivery_location;
        private System.Windows.Forms.TextBox signed_fo_by;
        private System.Windows.Forms.TextBox delivered_to;
        private System.Windows.Forms.TextBox status;
        private System.Windows.Forms.TextBox tracking_number;
        private System.Windows.Forms.TextBox carrier_code;
        private System.Windows.Forms.TextBox reference;
        private System.Windows.Forms.TextBox shipper;
        private System.Windows.Forms.TextBox recipient;
        private System.Windows.Forms.TextBox weight;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox textBox17;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox shipment_id;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox textBox18;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox textBox16;
        private System.Windows.Forms.Label label16;
    }
}


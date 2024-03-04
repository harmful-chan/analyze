
namespace analyze.core.win
{
    partial class MainForm
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
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            panelPage4 = new Panel();
            btnShowRefundDetail = new Button();
            brnShowRefund = new Button();
            btnCreateRefund = new Button();
            button9 = new Button();
            txtNewestResourcesFileName = new TextBox();
            label32 = new Label();
            btnProfitClear = new Button();
            btnShowLendDetail = new Button();
            txtNick = new TextBox();
            txtCompanyNumber = new TextBox();
            cbCN = new ComboBox();
            button8 = new Button();
            button7 = new Button();
            button6 = new Button();
            button5 = new Button();
            button4 = new Button();
            button3 = new Button();
            button2 = new Button();
            cbCompany = new ComboBox();
            btnShowProfit = new Button();
            dateProfitEnd = new DateTimePicker();
            dateProfitStart = new DateTimePicker();
            btnCreateProfitXLSX = new Button();
            txtProfit = new TextBox();
            txtNewestOrderDirectory = new TextBox();
            txtNewestReparationDirectory = new TextBox();
            txtNewestDailyDirectory = new TextBox();
            txtNewestDeductionDirectory = new TextBox();
            txtNewestTotalDirectory = new TextBox();
            txtNewestProfitDirectory = new TextBox();
            label31 = new Label();
            label30 = new Label();
            label29 = new Label();
            label28 = new Label();
            label27 = new Label();
            label26 = new Label();
            txtRoot = new TextBox();
            label25 = new Label();
            tabPage2 = new TabPage();
            panel4 = new Panel();
            txtOrderLog = new TextBox();
            btnDeduction = new Button();
            txtOrder = new TextBox();
            tabPage3 = new TabPage();
            panel5 = new Panel();
            progressBar1 = new ProgressBar();
            tabPage12 = new TabPage();
            panel2 = new Panel();
            panel1 = new Panel();
            label18 = new Label();
            textBox18 = new TextBox();
            shipment_id = new TextBox();
            label17 = new Label();
            reference = new TextBox();
            textBox16 = new TextBox();
            shipper = new TextBox();
            label16 = new Label();
            recipient = new TextBox();
            textBox17 = new TextBox();
            label3 = new Label();
            weight = new TextBox();
            delivery_date = new TextBox();
            ship_date = new TextBox();
            service_type = new TextBox();
            delivery_location = new TextBox();
            signed_fo_by = new TextBox();
            delivered_to = new TextBox();
            status = new TextBox();
            tracking_number = new TextBox();
            carrier_code = new TextBox();
            label1 = new Label();
            label15 = new Label();
            label2 = new Label();
            label14 = new Label();
            label13 = new Label();
            label4 = new Label();
            label12 = new Label();
            label5 = new Label();
            label11 = new Label();
            label6 = new Label();
            label10 = new Label();
            label7 = new Label();
            label9 = new Label();
            label8 = new Label();
            tabPage22 = new TabPage();
            panel3 = new Panel();
            label24 = new Label();
            tbDate = new TextBox();
            txt = new TextBox();
            txtThree = new TextBox();
            txtTwo = new TextBox();
            label23 = new Label();
            tbInquire = new TextBox();
            button1 = new Button();
            tbLastLegCarrier = new TextBox();
            label22 = new Label();
            tbLastLeg = new TextBox();
            tbFirstLeg = new TextBox();
            flowLayoutPanel1 = new FlowLayoutPanel();
            rbOne = new RadioButton();
            rbTwo = new RadioButton();
            rbThree = new RadioButton();
            tbFirstLegCarrier = new TextBox();
            txtOne = new TextBox();
            label21 = new Label();
            label19 = new Label();
            label20 = new Label();
            tabPage53 = new TabPage();
            openFileDialog = new OpenFileDialog();
            backgroundWorker = new System.ComponentModel.BackgroundWorker();
            btnShowPurchase = new Button();
            label33 = new Label();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            panelPage4.SuspendLayout();
            tabPage2.SuspendLayout();
            panel4.SuspendLayout();
            tabPage3.SuspendLayout();
            panel5.SuspendLayout();
            tabPage12.SuspendLayout();
            panel1.SuspendLayout();
            tabPage22.SuspendLayout();
            panel3.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Controls.Add(tabPage3);
            tabControl1.Controls.Add(tabPage12);
            tabControl1.Controls.Add(tabPage22);
            tabControl1.Controls.Add(tabPage53);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Location = new Point(0, 0);
            tabControl1.Margin = new Padding(4);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(1119, 532);
            tabControl1.TabIndex = 1;
            tabControl1.SelectedIndexChanged += tabControl1_SelectedIndexChanged;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(panelPage4);
            tabPage1.Location = new Point(4, 26);
            tabPage1.Margin = new Padding(4);
            tabPage1.Name = "tabPage1";
            tabPage1.Size = new Size(1111, 502);
            tabPage1.TabIndex = 3;
            tabPage1.Text = "利润统计";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // panelPage4
            // 
            panelPage4.Controls.Add(label33);
            panelPage4.Controls.Add(btnShowPurchase);
            panelPage4.Controls.Add(btnShowRefundDetail);
            panelPage4.Controls.Add(brnShowRefund);
            panelPage4.Controls.Add(btnCreateRefund);
            panelPage4.Controls.Add(button9);
            panelPage4.Controls.Add(txtNewestResourcesFileName);
            panelPage4.Controls.Add(label32);
            panelPage4.Controls.Add(btnProfitClear);
            panelPage4.Controls.Add(btnShowLendDetail);
            panelPage4.Controls.Add(txtNick);
            panelPage4.Controls.Add(txtCompanyNumber);
            panelPage4.Controls.Add(cbCN);
            panelPage4.Controls.Add(button8);
            panelPage4.Controls.Add(button7);
            panelPage4.Controls.Add(button6);
            panelPage4.Controls.Add(button5);
            panelPage4.Controls.Add(button4);
            panelPage4.Controls.Add(button3);
            panelPage4.Controls.Add(button2);
            panelPage4.Controls.Add(cbCompany);
            panelPage4.Controls.Add(btnShowProfit);
            panelPage4.Controls.Add(dateProfitEnd);
            panelPage4.Controls.Add(dateProfitStart);
            panelPage4.Controls.Add(btnCreateProfitXLSX);
            panelPage4.Controls.Add(txtProfit);
            panelPage4.Controls.Add(txtNewestOrderDirectory);
            panelPage4.Controls.Add(txtNewestReparationDirectory);
            panelPage4.Controls.Add(txtNewestDailyDirectory);
            panelPage4.Controls.Add(txtNewestDeductionDirectory);
            panelPage4.Controls.Add(txtNewestTotalDirectory);
            panelPage4.Controls.Add(txtNewestProfitDirectory);
            panelPage4.Controls.Add(label31);
            panelPage4.Controls.Add(label30);
            panelPage4.Controls.Add(label29);
            panelPage4.Controls.Add(label28);
            panelPage4.Controls.Add(label27);
            panelPage4.Controls.Add(label26);
            panelPage4.Controls.Add(txtRoot);
            panelPage4.Controls.Add(label25);
            panelPage4.Dock = DockStyle.Fill;
            panelPage4.Location = new Point(0, 0);
            panelPage4.Name = "panelPage4";
            panelPage4.Size = new Size(1111, 502);
            panelPage4.TabIndex = 0;
            // 
            // btnShowRefundDetail
            // 
            btnShowRefundDetail.Location = new Point(852, 308);
            btnShowRefundDetail.Name = "btnShowRefundDetail";
            btnShowRefundDetail.Size = new Size(90, 23);
            btnShowRefundDetail.TabIndex = 39;
            btnShowRefundDetail.Text = "显示退款详情";
            btnShowRefundDetail.UseVisualStyleBackColor = true;
            // 
            // brnShowRefund
            // 
            brnShowRefund.Location = new Point(759, 308);
            brnShowRefund.Name = "brnShowRefund";
            brnShowRefund.Size = new Size(90, 23);
            brnShowRefund.TabIndex = 38;
            brnShowRefund.Text = "显示退款";
            brnShowRefund.UseVisualStyleBackColor = true;
            brnShowRefund.Click += brnShowRefund_Click;
            // 
            // btnCreateRefund
            // 
            btnCreateRefund.Location = new Point(948, 308);
            btnCreateRefund.Name = "btnCreateRefund";
            btnCreateRefund.Size = new Size(154, 23);
            btnCreateRefund.TabIndex = 37;
            btnCreateRefund.Text = "生成退款报表";
            btnCreateRefund.UseVisualStyleBackColor = true;
            btnCreateRefund.Click += btnCreateRefund_Click;
            // 
            // button9
            // 
            button9.Location = new Point(1078, 172);
            button9.Name = "button9";
            button9.Size = new Size(25, 23);
            button9.TabIndex = 36;
            button9.Text = "...";
            button9.UseVisualStyleBackColor = true;
            // 
            // txtNewestResourcesFileName
            // 
            txtNewestResourcesFileName.Location = new Point(821, 173);
            txtNewestResourcesFileName.Name = "txtNewestResourcesFileName";
            txtNewestResourcesFileName.Size = new Size(251, 23);
            txtNewestResourcesFileName.TabIndex = 35;
            // 
            // label32
            // 
            label32.AutoSize = true;
            label32.Location = new Point(759, 178);
            label32.Name = "label32";
            label32.Size = new Size(56, 17);
            label32.TabIndex = 34;
            label32.Text = "资源路径";
            // 
            // btnProfitClear
            // 
            btnProfitClear.Location = new Point(948, 255);
            btnProfitClear.Name = "btnProfitClear";
            btnProfitClear.Size = new Size(154, 23);
            btnProfitClear.TabIndex = 33;
            btnProfitClear.Text = "清空";
            btnProfitClear.UseVisualStyleBackColor = true;
            btnProfitClear.Click += btnProfitClear_Click;
            // 
            // btnShowLendDetail
            // 
            btnShowLendDetail.Location = new Point(852, 282);
            btnShowLendDetail.Name = "btnShowLendDetail";
            btnShowLendDetail.Size = new Size(90, 23);
            btnShowLendDetail.TabIndex = 32;
            btnShowLendDetail.Text = "显示利润详情";
            btnShowLendDetail.UseVisualStyleBackColor = true;
            btnShowLendDetail.Click += btnListLend_Click;
            // 
            // txtNick
            // 
            txtNick.Location = new Point(759, 227);
            txtNick.Name = "txtNick";
            txtNick.Size = new Size(56, 23);
            txtNick.TabIndex = 31;
            // 
            // txtCompanyNumber
            // 
            txtCompanyNumber.Location = new Point(759, 199);
            txtCompanyNumber.Name = "txtCompanyNumber";
            txtCompanyNumber.Size = new Size(56, 23);
            txtCompanyNumber.TabIndex = 30;
            // 
            // cbCN
            // 
            cbCN.FormattingEnabled = true;
            cbCN.Location = new Point(821, 226);
            cbCN.Name = "cbCN";
            cbCN.Size = new Size(281, 25);
            cbCN.TabIndex = 29;
            cbCN.SelectedIndexChanged += cbCN_SelectedIndexChanged;
            // 
            // button8
            // 
            button8.Location = new Point(1078, 148);
            button8.Name = "button8";
            button8.Size = new Size(25, 23);
            button8.TabIndex = 28;
            button8.Text = "...";
            button8.UseVisualStyleBackColor = true;
            // 
            // button7
            // 
            button7.Location = new Point(1078, 124);
            button7.Name = "button7";
            button7.Size = new Size(25, 23);
            button7.TabIndex = 27;
            button7.Text = "...";
            button7.UseVisualStyleBackColor = true;
            // 
            // button6
            // 
            button6.Location = new Point(1078, 100);
            button6.Name = "button6";
            button6.Size = new Size(25, 23);
            button6.TabIndex = 26;
            button6.Text = "...";
            button6.UseVisualStyleBackColor = true;
            // 
            // button5
            // 
            button5.Location = new Point(1078, 76);
            button5.Name = "button5";
            button5.Size = new Size(25, 23);
            button5.TabIndex = 25;
            button5.Text = "...";
            button5.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            button4.Location = new Point(1078, 52);
            button4.Name = "button4";
            button4.Size = new Size(25, 23);
            button4.TabIndex = 24;
            button4.Text = "...";
            button4.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            button3.Location = new Point(1078, 28);
            button3.Name = "button3";
            button3.Size = new Size(25, 23);
            button3.TabIndex = 23;
            button3.Text = "...";
            button3.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            button2.Location = new Point(1078, 4);
            button2.Name = "button2";
            button2.Size = new Size(25, 23);
            button2.TabIndex = 22;
            button2.Text = "...";
            button2.UseVisualStyleBackColor = true;
            // 
            // cbCompany
            // 
            cbCompany.FormattingEnabled = true;
            cbCompany.Location = new Point(821, 198);
            cbCompany.Name = "cbCompany";
            cbCompany.Size = new Size(281, 25);
            cbCompany.TabIndex = 21;
            cbCompany.SelectedIndexChanged += cbCompany_SelectedIndexChanged;
            // 
            // btnShowProfit
            // 
            btnShowProfit.Location = new Point(759, 282);
            btnShowProfit.Name = "btnShowProfit";
            btnShowProfit.Size = new Size(90, 23);
            btnShowProfit.TabIndex = 20;
            btnShowProfit.Text = "显示利润";
            btnShowProfit.UseVisualStyleBackColor = true;
            btnShowProfit.Click += btnShowProfit_Click;
            // 
            // dateProfitEnd
            // 
            dateProfitEnd.CustomFormat = "yyyy-MM";
            dateProfitEnd.Format = DateTimePickerFormat.Custom;
            dateProfitEnd.Location = new Point(854, 255);
            dateProfitEnd.Name = "dateProfitEnd";
            dateProfitEnd.Size = new Size(90, 23);
            dateProfitEnd.TabIndex = 19;
            // 
            // dateProfitStart
            // 
            dateProfitStart.CustomFormat = "yyyy-MM";
            dateProfitStart.Format = DateTimePickerFormat.Custom;
            dateProfitStart.Location = new Point(759, 255);
            dateProfitStart.Name = "dateProfitStart";
            dateProfitStart.Size = new Size(90, 23);
            dateProfitStart.TabIndex = 18;
            dateProfitStart.Value = new DateTime(2024, 1, 1, 0, 0, 0, 0);
            // 
            // btnCreateProfitXLSX
            // 
            btnCreateProfitXLSX.Location = new Point(948, 282);
            btnCreateProfitXLSX.Name = "btnCreateProfitXLSX";
            btnCreateProfitXLSX.Size = new Size(154, 23);
            btnCreateProfitXLSX.TabIndex = 17;
            btnCreateProfitXLSX.Text = "生成利润报表";
            btnCreateProfitXLSX.UseVisualStyleBackColor = true;
            btnCreateProfitXLSX.Click += btnCreateProfitXLSX_Click;
            // 
            // txtProfit
            // 
            txtProfit.Font = new Font("微软雅黑", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtProfit.Location = new Point(3, 3);
            txtProfit.Multiline = true;
            txtProfit.Name = "txtProfit";
            txtProfit.ReadOnly = true;
            txtProfit.ScrollBars = ScrollBars.Both;
            txtProfit.Size = new Size(751, 494);
            txtProfit.TabIndex = 16;
            txtProfit.WordWrap = false;
            // 
            // txtNewestOrderDirectory
            // 
            txtNewestOrderDirectory.Location = new Point(821, 148);
            txtNewestOrderDirectory.Name = "txtNewestOrderDirectory";
            txtNewestOrderDirectory.Size = new Size(251, 23);
            txtNewestOrderDirectory.TabIndex = 15;
            // 
            // txtNewestReparationDirectory
            // 
            txtNewestReparationDirectory.Location = new Point(821, 124);
            txtNewestReparationDirectory.Name = "txtNewestReparationDirectory";
            txtNewestReparationDirectory.Size = new Size(251, 23);
            txtNewestReparationDirectory.TabIndex = 14;
            // 
            // txtNewestDailyDirectory
            // 
            txtNewestDailyDirectory.Location = new Point(821, 100);
            txtNewestDailyDirectory.Name = "txtNewestDailyDirectory";
            txtNewestDailyDirectory.Size = new Size(251, 23);
            txtNewestDailyDirectory.TabIndex = 13;
            // 
            // txtNewestDeductionDirectory
            // 
            txtNewestDeductionDirectory.Location = new Point(821, 76);
            txtNewestDeductionDirectory.Name = "txtNewestDeductionDirectory";
            txtNewestDeductionDirectory.Size = new Size(251, 23);
            txtNewestDeductionDirectory.TabIndex = 12;
            // 
            // txtNewestTotalDirectory
            // 
            txtNewestTotalDirectory.Location = new Point(821, 52);
            txtNewestTotalDirectory.Name = "txtNewestTotalDirectory";
            txtNewestTotalDirectory.Size = new Size(251, 23);
            txtNewestTotalDirectory.TabIndex = 11;
            // 
            // txtNewestProfitDirectory
            // 
            txtNewestProfitDirectory.Location = new Point(821, 28);
            txtNewestProfitDirectory.Name = "txtNewestProfitDirectory";
            txtNewestProfitDirectory.Size = new Size(251, 23);
            txtNewestProfitDirectory.TabIndex = 10;
            // 
            // label31
            // 
            label31.AutoSize = true;
            label31.Location = new Point(759, 154);
            label31.Name = "label31";
            label31.Size = new Size(56, 17);
            label31.TabIndex = 9;
            label31.Text = "订单数据";
            // 
            // label30
            // 
            label30.AutoSize = true;
            label30.Location = new Point(759, 130);
            label30.Name = "label30";
            label30.Size = new Size(56, 17);
            label30.TabIndex = 8;
            label30.Text = "索赔记录";
            // 
            // label29
            // 
            label29.AutoSize = true;
            label29.Location = new Point(760, 106);
            label29.Name = "label29";
            label29.Size = new Size(56, 17);
            label29.TabIndex = 7;
            label29.Text = "每日数据";
            // 
            // label28
            // 
            label28.AutoSize = true;
            label28.Location = new Point(759, 82);
            label28.Name = "label28";
            label28.Size = new Size(56, 17);
            label28.TabIndex = 6;
            label28.Text = "扣款记录";
            // 
            // label27
            // 
            label27.AutoSize = true;
            label27.Location = new Point(759, 58);
            label27.Name = "label27";
            label27.Size = new Size(56, 17);
            label27.TabIndex = 5;
            label27.Text = "总表数据";
            // 
            // label26
            // 
            label26.AutoSize = true;
            label26.Location = new Point(760, 34);
            label26.Name = "label26";
            label26.Size = new Size(56, 17);
            label26.TabIndex = 4;
            label26.Text = "利润统计";
            // 
            // txtRoot
            // 
            txtRoot.Location = new Point(821, 4);
            txtRoot.Name = "txtRoot";
            txtRoot.Size = new Size(251, 23);
            txtRoot.TabIndex = 3;
            txtRoot.Text = "D:\\我的坚果云\\数据采集";
            // 
            // label25
            // 
            label25.AutoSize = true;
            label25.Location = new Point(760, 10);
            label25.Name = "label25";
            label25.Size = new Size(44, 17);
            label25.TabIndex = 2;
            label25.Text = "根目录";
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(panel4);
            tabPage2.Location = new Point(4, 26);
            tabPage2.Margin = new Padding(4);
            tabPage2.Name = "tabPage2";
            tabPage2.Size = new Size(1111, 502);
            tabPage2.TabIndex = 2;
            tabPage2.Text = "扣款";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // panel4
            // 
            panel4.Controls.Add(txtOrderLog);
            panel4.Controls.Add(btnDeduction);
            panel4.Controls.Add(txtOrder);
            panel4.Dock = DockStyle.Fill;
            panel4.Location = new Point(0, 0);
            panel4.Margin = new Padding(4);
            panel4.Name = "panel4";
            panel4.Size = new Size(1111, 502);
            panel4.TabIndex = 0;
            // 
            // txtOrderLog
            // 
            txtOrderLog.Location = new Point(4, 4);
            txtOrderLog.Margin = new Padding(4);
            txtOrderLog.Multiline = true;
            txtOrderLog.Name = "txtOrderLog";
            txtOrderLog.ReadOnly = true;
            txtOrderLog.ScrollBars = ScrollBars.Vertical;
            txtOrderLog.Size = new Size(815, 494);
            txtOrderLog.TabIndex = 2;
            // 
            // btnDeduction
            // 
            btnDeduction.Location = new Point(823, 465);
            btnDeduction.Margin = new Padding(4);
            btnDeduction.Name = "btnDeduction";
            btnDeduction.Size = new Size(284, 33);
            btnDeduction.TabIndex = 1;
            btnDeduction.Text = "自动扣款";
            btnDeduction.UseVisualStyleBackColor = true;
            btnDeduction.Click += btnDeduction_Click;
            // 
            // txtOrder
            // 
            txtOrder.Location = new Point(823, 0);
            txtOrder.Margin = new Padding(4);
            txtOrder.Multiline = true;
            txtOrder.Name = "txtOrder";
            txtOrder.ScrollBars = ScrollBars.Vertical;
            txtOrder.Size = new Size(288, 457);
            txtOrder.TabIndex = 0;
            // 
            // tabPage3
            // 
            tabPage3.Controls.Add(panel5);
            tabPage3.Location = new Point(4, 26);
            tabPage3.Name = "tabPage3";
            tabPage3.Size = new Size(1111, 502);
            tabPage3.TabIndex = 5;
            tabPage3.Text = "采购进度";
            tabPage3.UseVisualStyleBackColor = true;
            // 
            // panel5
            // 
            panel5.Controls.Add(progressBar1);
            panel5.Dock = DockStyle.Fill;
            panel5.Location = new Point(0, 0);
            panel5.Name = "panel5";
            panel5.Size = new Size(1111, 502);
            panel5.TabIndex = 0;
            // 
            // progressBar1
            // 
            progressBar1.Location = new Point(256, 134);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(100, 23);
            progressBar1.TabIndex = 0;
            // 
            // tabPage12
            // 
            tabPage12.Controls.Add(panel2);
            tabPage12.Controls.Add(panel1);
            tabPage12.Location = new Point(4, 26);
            tabPage12.Margin = new Padding(4);
            tabPage12.Name = "tabPage12";
            tabPage12.Padding = new Padding(4);
            tabPage12.Size = new Size(1111, 502);
            tabPage12.TabIndex = 0;
            tabPage12.Text = "生成面单";
            tabPage12.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            panel2.Location = new Point(442, 4);
            panel2.Margin = new Padding(4);
            panel2.Name = "panel2";
            panel2.Size = new Size(557, 616);
            panel2.TabIndex = 8;
            // 
            // panel1
            // 
            panel1.Controls.Add(label18);
            panel1.Controls.Add(textBox18);
            panel1.Controls.Add(shipment_id);
            panel1.Controls.Add(label17);
            panel1.Controls.Add(reference);
            panel1.Controls.Add(textBox16);
            panel1.Controls.Add(shipper);
            panel1.Controls.Add(label16);
            panel1.Controls.Add(recipient);
            panel1.Controls.Add(textBox17);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(weight);
            panel1.Controls.Add(delivery_date);
            panel1.Controls.Add(ship_date);
            panel1.Controls.Add(service_type);
            panel1.Controls.Add(delivery_location);
            panel1.Controls.Add(signed_fo_by);
            panel1.Controls.Add(delivered_to);
            panel1.Controls.Add(status);
            panel1.Controls.Add(tracking_number);
            panel1.Controls.Add(carrier_code);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(label15);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(label14);
            panel1.Controls.Add(label13);
            panel1.Controls.Add(label4);
            panel1.Controls.Add(label12);
            panel1.Controls.Add(label5);
            panel1.Controls.Add(label11);
            panel1.Controls.Add(label6);
            panel1.Controls.Add(label10);
            panel1.Controls.Add(label7);
            panel1.Controls.Add(label9);
            panel1.Controls.Add(label8);
            panel1.Location = new Point(4, 4);
            panel1.Margin = new Padding(4);
            panel1.Name = "panel1";
            panel1.Size = new Size(435, 616);
            panel1.TabIndex = 7;
            // 
            // label18
            // 
            label18.AutoSize = true;
            label18.Location = new Point(405, 326);
            label18.Margin = new Padding(4, 0, 4, 0);
            label18.Name = "label18";
            label18.Size = new Size(25, 17);
            label18.TabIndex = 21;
            label18.Text = "KG";
            // 
            // textBox18
            // 
            textBox18.Location = new Point(225, 534);
            textBox18.Margin = new Padding(4);
            textBox18.Name = "textBox18";
            textBox18.Size = new Size(199, 23);
            textBox18.TabIndex = 14;
            // 
            // shipment_id
            // 
            shipment_id.Location = new Point(225, 463);
            shipment_id.Margin = new Padding(4);
            shipment_id.Name = "shipment_id";
            shipment_id.Size = new Size(199, 23);
            shipment_id.TabIndex = 20;
            // 
            // label17
            // 
            label17.AutoSize = true;
            label17.Location = new Point(72, 538);
            label17.Margin = new Padding(4, 0, 4, 0);
            label17.Name = "label17";
            label17.Size = new Size(133, 17);
            label17.TabIndex = 13;
            label17.Text = "系统单号(Order No)：";
            // 
            // reference
            // 
            reference.Location = new Point(225, 428);
            reference.Margin = new Padding(4);
            reference.Name = "reference";
            reference.Size = new Size(199, 23);
            reference.TabIndex = 19;
            reference.Text = "648363282030726";
            // 
            // textBox16
            // 
            textBox16.Location = new Point(225, 572);
            textBox16.Margin = new Padding(4);
            textBox16.Name = "textBox16";
            textBox16.Size = new Size(199, 23);
            textBox16.TabIndex = 12;
            // 
            // shipper
            // 
            shipper.Location = new Point(225, 392);
            shipper.Margin = new Padding(4);
            shipper.Name = "shipper";
            shipper.Size = new Size(199, 23);
            shipper.TabIndex = 18;
            // 
            // label16
            // 
            label16.AutoSize = true;
            label16.Location = new Point(100, 577);
            label16.Margin = new Padding(4, 0, 4, 0);
            label16.Name = "label16";
            label16.Size = new Size(103, 17);
            label16.TabIndex = 11;
            label16.Text = "币种(Currency)：";
            // 
            // recipient
            // 
            recipient.Location = new Point(225, 357);
            recipient.Margin = new Padding(4);
            recipient.Name = "recipient";
            recipient.Size = new Size(199, 23);
            recipient.TabIndex = 17;
            // 
            // textBox17
            // 
            textBox17.Location = new Point(225, 499);
            textBox17.Margin = new Padding(4);
            textBox17.Name = "textBox17";
            textBox17.Size = new Size(199, 23);
            textBox17.TabIndex = 10;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(37, 503);
            label3.Margin = new Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new Size(163, 17);
            label3.TabIndex = 8;
            label3.Text = "运单号(Tracking number)：";
            // 
            // weight
            // 
            weight.Location = new Point(225, 322);
            weight.Margin = new Padding(4);
            weight.Name = "weight";
            weight.Size = new Size(172, 23);
            weight.TabIndex = 16;
            // 
            // delivery_date
            // 
            delivery_date.Location = new Point(225, 286);
            delivery_date.Margin = new Padding(4);
            delivery_date.Name = "delivery_date";
            delivery_date.Size = new Size(199, 23);
            delivery_date.TabIndex = 15;
            // 
            // ship_date
            // 
            ship_date.Location = new Point(225, 251);
            ship_date.Margin = new Padding(4);
            ship_date.Name = "ship_date";
            ship_date.Size = new Size(199, 23);
            ship_date.TabIndex = 14;
            // 
            // service_type
            // 
            service_type.Location = new Point(225, 215);
            service_type.Margin = new Padding(4);
            service_type.Name = "service_type";
            service_type.Size = new Size(199, 23);
            service_type.TabIndex = 13;
            service_type.Text = "Home Delivery";
            // 
            // delivery_location
            // 
            delivery_location.Location = new Point(225, 180);
            delivery_location.Margin = new Padding(4);
            delivery_location.Name = "delivery_location";
            delivery_location.Size = new Size(199, 23);
            delivery_location.TabIndex = 12;
            // 
            // signed_fo_by
            // 
            signed_fo_by.Location = new Point(225, 144);
            signed_fo_by.Margin = new Padding(4);
            signed_fo_by.Name = "signed_fo_by";
            signed_fo_by.Size = new Size(199, 23);
            signed_fo_by.TabIndex = 11;
            signed_fo_by.Text = "Signature not required";
            // 
            // delivered_to
            // 
            delivered_to.Location = new Point(225, 109);
            delivered_to.Margin = new Padding(4);
            delivered_to.Name = "delivered_to";
            delivered_to.Size = new Size(199, 23);
            delivered_to.TabIndex = 10;
            // 
            // status
            // 
            status.Location = new Point(225, 75);
            status.Margin = new Padding(4);
            status.Name = "status";
            status.Size = new Size(199, 23);
            status.TabIndex = 9;
            status.Text = "Delivered";
            // 
            // tracking_number
            // 
            tracking_number.Location = new Point(225, 40);
            tracking_number.Margin = new Padding(4);
            tracking_number.Name = "tracking_number";
            tracking_number.Size = new Size(199, 23);
            tracking_number.TabIndex = 8;
            // 
            // carrier_code
            // 
            carrier_code.Location = new Point(225, 6);
            carrier_code.Margin = new Padding(4);
            carrier_code.Name = "carrier_code";
            carrier_code.Size = new Size(199, 23);
            carrier_code.TabIndex = 7;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(58, 10);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(137, 17);
            label1.TabIndex = 0;
            label1.Text = "承运商(Carrier code)：";
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.Location = new Point(51, 468);
            label15.Margin = new Padding(4, 0, 4, 0);
            label15.Name = "label15";
            label15.Size = new Size(147, 17);
            label15.TabIndex = 6;
            label15.Text = "发货号码(Shipment ID)：";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(37, 44);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(163, 17);
            label2.TabIndex = 0;
            label2.Text = "运单号(Tracking number)：";
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Location = new Point(65, 432);
            label14.Margin = new Padding(4, 0, 4, 0);
            label14.Name = "label14";
            label14.Size = new Size(134, 17);
            label14.TabIndex = 5;
            label14.Text = "证明编码(Reference)：";
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Location = new Point(93, 397);
            label13.Margin = new Padding(4, 0, 4, 0);
            label13.Name = "label13";
            label13.Size = new Size(109, 17);
            label13.TabIndex = 4;
            label13.Text = "发货地(Shipper)：";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(86, 79);
            label4.Margin = new Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new Size(111, 17);
            label4.TabIndex = 0;
            label4.Text = "包裹状态(Status)：";
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new Point(79, 361);
            label12.Margin = new Padding(4, 0, 4, 0);
            label12.Name = "label12";
            label12.Size = new Size(117, 17);
            label12.TabIndex = 3;
            label12.Text = "签收地(Recipient)：";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(30, 113);
            label5.Margin = new Padding(4, 0, 4, 0);
            label5.Name = "label5";
            label5.Size = new Size(162, 17);
            label5.TabIndex = 0;
            label5.Text = "收件人名称(Delivered To)：";
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(114, 326);
            label11.Margin = new Padding(4, 0, 4, 0);
            label11.Name = "label11";
            label11.Size = new Size(93, 17);
            label11.TabIndex = 2;
            label11.Text = "重量(Weight)：";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(51, 149);
            label6.Margin = new Padding(4, 0, 4, 0);
            label6.Name = "label6";
            label6.Size = new Size(143, 17);
            label6.TabIndex = 0;
            label6.Text = "签字人(Signed for by)：";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(37, 290);
            label10.Margin = new Padding(4, 0, 4, 0);
            label10.Name = "label10";
            label10.Size = new Size(152, 17);
            label10.TabIndex = 1;
            label10.Text = "妥投时间(Delivery date)：";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(9, 184);
            label7.Margin = new Padding(4, 0, 4, 0);
            label7.Name = "label7";
            label7.Size = new Size(175, 17);
            label7.TabIndex = 0;
            label7.Text = "收货地址(Delivery Location)：";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(65, 255);
            label9.Margin = new Padding(4, 0, 4, 0);
            label9.Name = "label9";
            label9.Size = new Size(131, 17);
            label9.TabIndex = 1;
            label9.Text = "发货时间(Ship date)：";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(44, 220);
            label8.Margin = new Padding(4, 0, 4, 0);
            label8.Name = "label8";
            label8.Size = new Size(146, 17);
            label8.TabIndex = 0;
            label8.Text = "服务类型(Service type)：";
            // 
            // tabPage22
            // 
            tabPage22.Controls.Add(panel3);
            tabPage22.Location = new Point(4, 26);
            tabPage22.Margin = new Padding(4);
            tabPage22.Name = "tabPage22";
            tabPage22.Padding = new Padding(4);
            tabPage22.Size = new Size(1111, 502);
            tabPage22.TabIndex = 1;
            tabPage22.Text = "自动回复";
            tabPage22.UseVisualStyleBackColor = true;
            // 
            // panel3
            // 
            panel3.Controls.Add(label24);
            panel3.Controls.Add(tbDate);
            panel3.Controls.Add(txt);
            panel3.Controls.Add(txtThree);
            panel3.Controls.Add(txtTwo);
            panel3.Controls.Add(label23);
            panel3.Controls.Add(tbInquire);
            panel3.Controls.Add(button1);
            panel3.Controls.Add(tbLastLegCarrier);
            panel3.Controls.Add(label22);
            panel3.Controls.Add(tbLastLeg);
            panel3.Controls.Add(tbFirstLeg);
            panel3.Controls.Add(flowLayoutPanel1);
            panel3.Controls.Add(tbFirstLegCarrier);
            panel3.Controls.Add(txtOne);
            panel3.Controls.Add(label21);
            panel3.Controls.Add(label19);
            panel3.Controls.Add(label20);
            panel3.Location = new Point(4, 4);
            panel3.Margin = new Padding(4);
            panel3.Name = "panel3";
            panel3.Size = new Size(995, 803);
            panel3.TabIndex = 15;
            // 
            // label24
            // 
            label24.AutoSize = true;
            label24.Location = new Point(721, 208);
            label24.Margin = new Padding(4, 0, 4, 0);
            label24.Name = "label24";
            label24.Size = new Size(56, 17);
            label24.TabIndex = 25;
            label24.Text = "预计送达";
            // 
            // tbDate
            // 
            tbDate.Location = new Point(791, 204);
            tbDate.Margin = new Padding(4);
            tbDate.Name = "tbDate";
            tbDate.Size = new Size(198, 23);
            tbDate.TabIndex = 24;
            // 
            // txt
            // 
            txt.Location = new Point(8, 437);
            txt.Margin = new Padding(4);
            txt.Multiline = true;
            txt.Name = "txt";
            txt.Size = new Size(982, 176);
            txt.TabIndex = 23;
            // 
            // txtThree
            // 
            txtThree.Location = new Point(6, 232);
            txtThree.Margin = new Padding(4);
            txtThree.Multiline = true;
            txtThree.Name = "txtThree";
            txtThree.ScrollBars = ScrollBars.Vertical;
            txtThree.Size = new Size(695, 100);
            txtThree.TabIndex = 21;
            // 
            // txtTwo
            // 
            txtTwo.Location = new Point(6, 122);
            txtTwo.Margin = new Padding(4);
            txtTwo.Multiline = true;
            txtTwo.Name = "txtTwo";
            txtTwo.ScrollBars = ScrollBars.Vertical;
            txtTwo.Size = new Size(695, 100);
            txtTwo.TabIndex = 20;
            // 
            // label23
            // 
            label23.AutoSize = true;
            label23.Location = new Point(735, 170);
            label23.Margin = new Padding(4, 0, 4, 0);
            label23.Name = "label23";
            label23.Size = new Size(40, 17);
            label23.TabIndex = 19;
            label23.Text = "查  询";
            // 
            // tbInquire
            // 
            tbInquire.Location = new Point(790, 166);
            tbInquire.Margin = new Padding(4);
            tbInquire.Name = "tbInquire";
            tbInquire.Size = new Size(198, 23);
            tbInquire.TabIndex = 18;
            // 
            // button1
            // 
            button1.Location = new Point(790, 340);
            button1.Margin = new Padding(4);
            button1.Name = "button1";
            button1.Size = new Size(200, 33);
            button1.TabIndex = 17;
            button1.Text = "生成";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // tbLastLegCarrier
            // 
            tbLastLegCarrier.Location = new Point(791, 89);
            tbLastLegCarrier.Margin = new Padding(4);
            tbLastLegCarrier.Name = "tbLastLegCarrier";
            tbLastLegCarrier.Size = new Size(198, 23);
            tbLastLegCarrier.TabIndex = 15;
            // 
            // label22
            // 
            label22.AutoSize = true;
            label22.Location = new Point(708, 96);
            label22.Margin = new Padding(4, 0, 4, 0);
            label22.Name = "label22";
            label22.Size = new Size(68, 17);
            label22.TabIndex = 16;
            label22.Text = "尾程承运商";
            // 
            // tbLastLeg
            // 
            tbLastLeg.Location = new Point(791, 128);
            tbLastLeg.Margin = new Padding(4);
            tbLastLeg.Name = "tbLastLeg";
            tbLastLeg.Size = new Size(198, 23);
            tbLastLeg.TabIndex = 14;
            // 
            // tbFirstLeg
            // 
            tbFirstLeg.Location = new Point(791, 51);
            tbFirstLeg.Margin = new Padding(4);
            tbFirstLeg.Name = "tbFirstLeg";
            tbFirstLeg.Size = new Size(198, 23);
            tbFirstLeg.TabIndex = 13;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Controls.Add(rbOne);
            flowLayoutPanel1.Controls.Add(rbTwo);
            flowLayoutPanel1.Controls.Add(rbThree);
            flowLayoutPanel1.FlowDirection = FlowDirection.TopDown;
            flowLayoutPanel1.Location = new Point(790, 235);
            flowLayoutPanel1.Margin = new Padding(4);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(198, 97);
            flowLayoutPanel1.TabIndex = 12;
            // 
            // rbOne
            // 
            rbOne.AutoSize = true;
            rbOne.Location = new Point(4, 4);
            rbOne.Margin = new Padding(4);
            rbOne.Name = "rbOne";
            rbOne.Size = new Size(74, 21);
            rbOne.TabIndex = 11;
            rbOne.TabStop = true;
            rbOne.Text = "头程可查";
            rbOne.UseVisualStyleBackColor = true;
            // 
            // rbTwo
            // 
            rbTwo.AutoSize = true;
            rbTwo.Location = new Point(4, 33);
            rbTwo.Margin = new Padding(4);
            rbTwo.Name = "rbTwo";
            rbTwo.Size = new Size(98, 21);
            rbTwo.TabIndex = 13;
            rbTwo.TabStop = true;
            rbTwo.Text = "尾程可查未收";
            rbTwo.UseVisualStyleBackColor = true;
            // 
            // rbThree
            // 
            rbThree.AutoSize = true;
            rbThree.Location = new Point(4, 62);
            rbThree.Margin = new Padding(4);
            rbThree.Name = "rbThree";
            rbThree.Size = new Size(98, 21);
            rbThree.TabIndex = 15;
            rbThree.TabStop = true;
            rbThree.Text = "尾程可查已收";
            rbThree.UseVisualStyleBackColor = true;
            // 
            // tbFirstLegCarrier
            // 
            tbFirstLegCarrier.Location = new Point(791, 13);
            tbFirstLegCarrier.Margin = new Padding(4);
            tbFirstLegCarrier.Name = "tbFirstLegCarrier";
            tbFirstLegCarrier.Size = new Size(198, 23);
            tbFirstLegCarrier.TabIndex = 7;
            // 
            // txtOne
            // 
            txtOne.Location = new Point(6, 11);
            txtOne.Margin = new Padding(4);
            txtOne.Multiline = true;
            txtOne.Name = "txtOne";
            txtOne.ScrollBars = ScrollBars.Vertical;
            txtOne.Size = new Size(695, 100);
            txtOne.TabIndex = 6;
            // 
            // label21
            // 
            label21.AutoSize = true;
            label21.Location = new Point(708, 17);
            label21.Margin = new Padding(4, 0, 4, 0);
            label21.Name = "label21";
            label21.Size = new Size(68, 17);
            label21.TabIndex = 10;
            label21.Text = "头程承运商";
            // 
            // label19
            // 
            label19.AutoSize = true;
            label19.Location = new Point(736, 57);
            label19.Margin = new Padding(4, 0, 4, 0);
            label19.Name = "label19";
            label19.Size = new Size(40, 17);
            label19.TabIndex = 8;
            label19.Text = "头  程";
            // 
            // label20
            // 
            label20.AutoSize = true;
            label20.Location = new Point(736, 133);
            label20.Margin = new Padding(4, 0, 4, 0);
            label20.Name = "label20";
            label20.Size = new Size(40, 17);
            label20.TabIndex = 9;
            label20.Text = "尾  程";
            // 
            // tabPage53
            // 
            tabPage53.Location = new Point(4, 26);
            tabPage53.Margin = new Padding(4);
            tabPage53.Name = "tabPage53";
            tabPage53.Size = new Size(1111, 502);
            tabPage53.TabIndex = 4;
            tabPage53.Text = "tabPage5";
            tabPage53.UseVisualStyleBackColor = true;
            // 
            // openFileDialog
            // 
            openFileDialog.FileName = "openFileDialog";
            // 
            // btnShowPurchase
            // 
            btnShowPurchase.Location = new Point(760, 354);
            btnShowPurchase.Name = "btnShowPurchase";
            btnShowPurchase.Size = new Size(90, 23);
            btnShowPurchase.TabIndex = 40;
            btnShowPurchase.Text = "显采购进度";
            btnShowPurchase.UseVisualStyleBackColor = true;
            btnShowPurchase.Click += btnShowPurchase_Click;
            // 
            // label33
            // 
            label33.AutoSize = true;
            label33.Location = new Point(760, 334);
            label33.Name = "label33";
            label33.Size = new Size(328, 17);
            label33.TabIndex = 41;
            label33.Text = "----------------------------------------------------------------";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1119, 532);
            Controls.Add(tabControl1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Margin = new Padding(4);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "MainForm";
            Text = "Form1";
            Load += Form_Load;
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            panelPage4.ResumeLayout(false);
            panelPage4.PerformLayout();
            tabPage2.ResumeLayout(false);
            panel4.ResumeLayout(false);
            panel4.PerformLayout();
            tabPage3.ResumeLayout(false);
            panel5.ResumeLayout(false);
            tabPage12.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            tabPage22.ResumeLayout(false);
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            flowLayoutPanel1.ResumeLayout(false);
            flowLayoutPanel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage12;
        private System.Windows.Forms.TabPage tabPage22;
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
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.TextBox tbFirstLegCarrier;
        private System.Windows.Forms.TextBox txtOne;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.RadioButton rbTwo;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.TextBox tbLastLeg;
        private System.Windows.Forms.TextBox tbFirstLeg;
        private System.Windows.Forms.TextBox txt;
        private System.Windows.Forms.TextBox txtThree;
        private System.Windows.Forms.TextBox txtTwo;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.TextBox tbInquire;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox tbLastLegCarrier;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.RadioButton rbOne;
        private System.Windows.Forms.RadioButton rbThree;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.TextBox tbDate;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage53;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button btnDeduction;
        private System.Windows.Forms.TextBox txtOrder;
        private TextBox txtOrderLog;
        private Panel panelPage4;
        private Label label25;
        private TextBox txtProfit;
        private TextBox txtNewestOrderDirectory;
        private TextBox txtNewestReparationDirectory;
        private TextBox txtNewestDailyDirectory;
        private TextBox txtNewestDeductionDirectory;
        private TextBox txtNewestTotalDirectory;
        private TextBox txtNewestProfitDirectory;
        private Label label31;
        private Label label30;
        private Label label29;
        private Label label28;
        private Label label27;
        private Label label26;
        private TextBox txtRoot;
        private DateTimePicker dateProfitStart;
        private Button btnCreateProfitXLSX;
        private ComboBox cbCompany;
        private Button btnShowProfit;
        private DateTimePicker dateProfitEnd;
        private Button button8;
        private Button button7;
        private Button button6;
        private Button button5;
        private Button button4;
        private Button button3;
        private Button button2;
        private OpenFileDialog openFileDialog;
        private System.ComponentModel.BackgroundWorker backgroundWorker;
        private ComboBox cbCN;
        private TextBox txtNick;
        private TextBox txtCompanyNumber;
        private Button btnShowLendDetail;
        private Button btnProfitClear;
        private Button button9;
        private TextBox txtNewestResourcesFileName;
        private Label label32;
        private Button btnShowRefundDetail;
        private Button brnShowRefund;
        private Button btnCreateRefund;
        private TabPage tabPage3;
        private Panel panel5;
        private ProgressBar progressBar1;
        private Label label33;
        private Button btnShowPurchase;
    }
}


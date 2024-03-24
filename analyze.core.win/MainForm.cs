using analyze.core.Clients;
using analyze.core.Models.Daily;
using analyze.core.Models.Rola;
using analyze.core.Models.Sheet;
using analyze.core.Options;
using NPOI.HPSF;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace analyze.core.win
{
    public partial class MainForm : System.Windows.Forms.Form
    {

        public bool AllowSelect { get; set; } = true;

        public string TempDir 
        { 
            get 
            {
                string path = Path.Combine(Path.GetTempPath(), "analyze.core.win");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                return path;
            } 
        
        }
        Analyzer _analyzer = new Analyzer();

        public MainForm()
        {
            InitializeComponent();
        }


        private void Form_Load(object sender, EventArgs e)
        {
            InitializeParameter();

            // 禁止切换 tab
            tabctl.Selecting += (sender, e) => { e.Cancel = !AllowSelect; };
            InitializePage1();

        }
        private void InitializeParameter()
        {
            // 版本号
            var assemblyLocation = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
           // var c = System.Diagnostics.FileVersionInfo.GetVersionInfo(assemblyLocation).FileVersion;
            this.Text = $"{this.Text} [{assemblyLocation}]";


            #region page5
            this.txtOne.Text = "Prezado cliente, lamentamos muito o problema causado a você. Confirmamos seu pedido e temos rastreamento completo.\r\n"
    + "O primeiro segmento de logística foi enviado com o número de rastreamento [número do conhecimento de embarque]. Acompanhe as informações de envio por meio deste site oficial [URL de consulta].\r\n"
    + "Atualmente, pode levar de 3 a 7 dias para atualizar o cronograma logístico da segunda etapa da logística. Aguarde pacientemente e garantiremos que seu pedido seja entregue sem problemas.\r\n"
    + "Obrigado pela sua compreensão e apoio. Se você tiver alguma outra dúvida, não hesite em nos contatar.";

            this.txtTwo.Text = "Prezado cliente, lamentamos profundamente o transtorno causado a você. A segunda etapa foi enviada para o endereço que você forneceu e deverá chegar em [Data estimada de entrega].\r\n"
                + "Transportadora:[Transportadora]\r\n"
                + "Número de rastreamento logístico: [número do conhecimento de embarque]\r\n"
                + "Informações de contato do fornecedor de logística: [URL de consulta]\r\n"
                + "Estamos rastreando todo o pedido para garantir uma entrega segura. Assim que seu pedido for entregue com sucesso, você poderá desfrutar da alegria de fazer compras.Para entrar em contato conosco, adicione";

            this.txtThree.Text = "Prezado cliente, entendemos suas preocupações sobre o status logístico do seu pedido. Notamos que você afirmou que não recebeu a mercadoria. Para resolver o problema, verifique primeiro a veracidade do endereço de entrega, principalmente o número da casa, andar e outros detalhes. Posteriormente, entre em contato com o correio local para obter detalhes. Às vezes, os pacotes podem ser deixados na casa de um vizinho ou em outro local sem notificação imediata. Por favor, aguarde um certo tempo de espera, pois às vezes há um atraso nas informações de rastreamento logístico. Número de rastreamento logístico: [número do conhecimento de embarque]. Informações de contato do fornecedor de logística: [URL de consulta]\r\n"
                + "Se você ainda não resolveu o problema após as etapas acima, cooperaremos ativamente com a empresa de logística para ajudá - lo a resolver o problema durante todo o processo. Obrigado pela sua compreensão e apoio.";

            #endregion

            // 读取所有资源文件并另存为到 C:\Users\Administrator\AppData\Local\Temp\analyze.core.win
            System.Reflection.Assembly assembly = this.GetType().Assembly;
            string[] names = assembly.GetManifestResourceNames();
            names = names.Where(x => !x.EndsWith("resources")).ToArray();
            foreach (var name in names)
            {
                Stream? stream = assembly.GetManifestResourceStream(name);
                string[] strings = name.Split('.');
                string filename = strings[strings.Length - 2] + '.' + strings[strings.Length - 1];
                FileStream fileStream = File.Create(Path.Combine(TempDir, filename));
                stream.Seek(0, SeekOrigin.Begin);
                stream.CopyTo(fileStream);
                fileStream.Close();
            }

            // 修订记录
            if (File.Exists(Path.Combine(TempDir, "Revision.txt")))
            {
                this.txtRevision.Text = File.ReadAllText(Path.Combine(TempDir, "Revision.txt"));
            }

        }

        private void SwitchPage(int pageIndex)
        {
            switch (pageIndex)
            {
                case 0: InitializePage1(); break;
                case 1: InitializePage2(); break;
                default: break;
            }
        }


        #region page1

        DailyDetail[] daily;

        bool IsInitializePage1 = false;
        private void InitializePage1()
        {
            // 切换显示 textbox
            _analyzer.Output = new FormOutput(this, this.txtProfit);


            if (Directory.Exists(this.txtRoot.Text) && !IsInitializePage1 && !_analyzer.IsRunning)
            {
                BackgroundWorker bk = new BackgroundWorker();
                bk.DoWork += (sender, e) =>
                {
                    // 禁止切换 不能点击按钮
                    this.BeginInvoke(new Action(() =>
                    {
                        this.AllowSelect = false;
                        this.panelHone.Enabled = false;
                    }));

                    // 设置根目录，采集所有表格数据
                    _analyzer.SetRootDirectorie(this.txtRoot.Text);
                    _analyzer.StartCollect(CollectTypes.Shop | CollectTypes.Daily);
                    _analyzer.Output.WriteLine();
                    _analyzer.ListMissingStores(DateTime.Now);
                    _analyzer.Output.WriteLine("读取完成");

                    ShopRecord[] shopRecords = _analyzer.ShopRecords.ToArray();
                    var sGroup = shopRecords.GroupBy(x => x.Shop.CompanyName);


                    this.BeginInvoke(new Action(() =>
                    {
                        this.cbCompany.Items.Clear();
                        string[] companys = sGroup.Select(y => y.Key).ToArray();
                        System.Array.Sort(companys);
                        this.cbCompany.Items.AddRange(companys);
                        this.cbCompany.SelectedIndex = 0;
                        this.txtNewestTotalDirectory.Text = _analyzer.NewestTotalDirectory;
                        this.txtNewestDailyDirectory.Text = _analyzer.NewestDailyDirectory;
                        this.txtNewestDeductionDirectory.Text = _analyzer.NewestDeductionDirectory;
                        this.txtNewestOrderDirectory.Text = _analyzer.NewestOrderDirectory;
                        this.txtNewestProfitDirectory.Text = _analyzer.NewestProfitDirectory;
                        this.txtNewestReparationDirectory.Text = _analyzer.NewestReparationDirectory;
                        this.txtNewestResourcesFileName.Text = _analyzer.NewestResourcesDirectory;

                        this.AllowSelect = true;
                        this.panelHone.Enabled = true;
                    }));

                    IsInitializePage1 = true;
                };
                bk.RunWorkerAsync();
            }
        }


        private Dictionary<string, ShopRecord> SureShopRecordDic()
        {
            var dic = new Dictionary<string, ShopRecord>();
            DateTime start = DateTime.Parse(dateProfitStart.Value.ToString("yyyy-MM"));
            DateTime end = DateTime.Parse(dateProfitEnd.Value.ToString("yyyy-MM"));
            string company = this.txtCompanyNumber.Text + this.cbCN.Text + this.cbCompany.Text;
            for (DateTime i = start; i < end; i = i.AddMonths(1))
            {
                ShopRecord shopRecord = _analyzer.ShopRecords.FirstOrDefault(sr => sr.Shop.CN.Equals(this.cbCN.Text));
                dic[i.ToString("yyyy-MM")] = shopRecord;
            }
            return dic;
        }
        private void btnShowProfit_Click(object sender, EventArgs e)
        {
            foreach (var item in SureShopRecordDic())
            {
                DateTime i = DateTime.Parse(item.Key);
                Shop s = item.Value.Shop;
                _analyzer.FillProfitForOneMonth(i.Year, i.Month, item.Value);
                var p = _analyzer.StatisticalProfit(i.Year, i.Month, item.Value);
                _analyzer.Output.WriteLine($"{s.CompanyName + s.Nick} {i.Year} {i.Month.ToString("D2")} Lend:{Math.Round(p.Lend, 2)} Cost:{Math.Round(p.Cost, 2)} Profit:{Math.Round(p.Value, 2)} Rate:{Math.Round(p.Rate, 2)}");

            }
        }


        private void cbCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.cbCN.Items.Clear();
            ShopRecord[] shopRecords = _analyzer.ShopRecords.ToArray();
            // 设置公司序号
            string text = (sender as ComboBox).Text;
            this.txtCompanyNumber.Text = shopRecords.Where(s => s.Shop.CompanyName.Equals(text)).First().Shop.CompanyNumber;

            // 设置cn号下拉框
            var sGroup = shopRecords.GroupBy(x => x.Shop.CompanyName).Where(y => y.Key.Equals(this.cbCompany.Text));
            foreach (var group in sGroup)
            {
                foreach (var item in group)
                {
                    this.cbCN.Items.Add(item.Shop.CN);
                }
            }
            this.cbCN.SelectedIndex = 0;
        }

        private void cbCN_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 设置昵称
            ShopRecord[] shopRecords = _analyzer.ShopRecords.ToArray();
            string text = ((ComboBox)sender).Text;
            this.txtNick.Text = shopRecords.Where(s => s.Shop.CN.Equals(text)).First().Shop.Nick;
        }

        private void btnListLend_Click(object sender, EventArgs e)
        {

            foreach (var item in SureShopRecordDic())
            {
                DateTime i = DateTime.Parse(item.Key);
                var p = _analyzer.StatisticalProfit(i.Year, i.Month, item.Value);
                _analyzer.ShowOneMonthLend(i.Year, i.Month, item.Value, p);
            }
        }
        private void btnProfitClear_Click(object sender, EventArgs e)
        {
            this.txtProfit.Clear();
        }

        private void btnCreateProfitXLSX_Click(object sender, EventArgs e)
        {
            foreach (var item in SureShopRecordDic())   // 多个月
            {
                DateTime i = DateTime.Parse(item.Key);
                _analyzer.FillProfitForOneMonth(i.Year, i.Month, item.Value);
                ShopLend[] shopLends = _analyzer.GetOneMonthLend(i.Year, i.Month, item.Value);

                string dir = Path.Combine(_analyzer.NewestProfitDirectory, $"{item.Value.Shop.CompanyNumber}{item.Value.Shop.CompanyName}");
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                string filename = Path.Combine(dir, $"{item.Value.Shop.CompanyName}{item.Value.Shop.Nick}{i.Year}{i.Month.ToString("D2")}.xlsx");
                _analyzer.SaveProfit(filename, shopLends);
            }
        }


        private void brnShowRefund_Click(object sender, EventArgs e)
        {
            foreach (var item in SureShopRecordDic())
            {
                DateTime i = DateTime.Parse(item.Key);
                _analyzer.FillRefundForOneMonth(i.Year, i.Month, item.Value);
                _analyzer.ShowOneMonthRefund();
            }
        }
        private void btnCreateRefund_Click(object sender, EventArgs e)
        {
            foreach (var item in SureShopRecordDic())   // 多个月
            {
                DateTime i = DateTime.Parse(item.Key);
                _analyzer.FillRefundForOneMonth(i.Year, i.Month, item.Value);
                ShopRefund[] shopRefunds = _analyzer.GetOneMonthRefund(i.Year, i.Month, item.Value);

                string dir = Path.Combine(_analyzer.NewestReparationDirectory, $"{item.Value.Shop.CompanyNumber}{item.Value.Shop.CompanyName}");
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                string filename = Path.Combine(dir, $"{item.Value.Shop.CompanyName}{item.Value.Shop.Nick}{i.Year}{i.Month.ToString("D2")}.xlsx");
                _analyzer.SaveRefund(filename, shopRefunds);
            }
        }

        private void btnShowPurchase_Click(object sender, EventArgs e)
        {
            //_analyzer.ShowPurchase();
        }

        private void btnCreateOrderTxt_Click(object sender, EventArgs e)
        {
            string filename = Path.Combine(this.txtNewestDailyDirectory.Text, _analyzer.NewestDailyDate.ToString("yyyy年MM月dd日") + "订单.txt");
            _analyzer.BuildOrders(_analyzer.NewestDailyDate, filename);
            _analyzer.Output.WriteLine($"创建 {filename}");
        }

        private void btnCreateStoreTxt_Click(object sender, EventArgs e)
        {
            string filename = Path.Combine(this.txtNewestDailyDirectory.Text, _analyzer.NewestDailyDate.ToString("yyyy年MM月dd日") + "店铺数据.txt");
            _analyzer.BuildCompanyProfits(_analyzer.NewestDailyDate, filename);
            _analyzer.Output.WriteLine($"创建 {filename}");
        }

        #endregion



        #region page3
        string[][] list;
        RolaClient rolaClient = new RolaClient();
        private void btnCheck_Click(object sender, EventArgs e)
        {
            this.txtIpShow.Clear();
            foreach (var item in GetList(this.txtUserList.Text))
            {
                Position i = rolaClient.RolaCheck(item.Country, item.Region, item.City);
                this.txtIpShow.AppendText($"{item.UserName} {i.Country} {i.Region} {i.City}\r\n");
            }
        }

        private Position[] GetList(string str)
        {
            string[] strings = str.Split("\r\n");
            Position[] positions = new Position[strings.Length];
            for (int i = 0; i < strings.Length; i++)
            {
                positions[i] = new Position();
                string[] split = strings[i].Split(' ');
                if (split.Length > 0)
                {
                    positions[i].UserName = strings[i].Split(' ')[0];
                }
                if (split.Length > 1)
                {
                    positions[i].Country = strings[i].Split(' ')[1];
                }
                if (split.Length > 2)
                {
                    positions[i].Region = strings[i].Split(' ')[2];
                }
                if (split.Length > 3)
                {
                    positions[i].City = strings[i].Split(' ')[3];
                }
            }
            return positions;
        }

        private async void btnGetIP_Click(object sender, EventArgs e)
        {
            this.txtIpShow.Clear();
            foreach (var item in GetList(this.txtUserList.Text))
            {
                Position i = rolaClient.RolaCheck(item.Country, item.Region, item.City);
                Position j = await rolaClient.GetPosition("gate8.rola.vip", 2024, item.UserName, "123");
                this.txtIpShow.AppendText($"{item.UserName} {i.Country} {i.Region} {i.City} {j.IP} {j.Country} {j.Region} {j.City} \r\n");
            }
        }
        #endregion



        #region page2
        private void InitializePage2()
        {
            _analyzer.Output = new FormOutput(this, this.txtOrderLog);
        }

        private void btnDeduction_Click(object sender, EventArgs e)
        {

            if (!_analyzer.IsRunning)
            {
                BackgroundWorker bk = new BackgroundWorker();
                bk.DoWork += (sender, e) => 
                {
                    string raw = this.txtOrder.Text;
                    var list = _analyzer.ReadNeedShip(raw);
                    _analyzer.Deduction(list);
                };
                bk.RunWorkerAsync();
            }
        }

        private void btnShip_Click(object sender, EventArgs e)
        {
            if (!_analyzer.IsRunning)
            {
                BackgroundWorker bk = new BackgroundWorker();
                bk.DoWork += (sender, e) =>
                {
                    string raw = this.txtOrder.Text;
                    var list = _analyzer.ReadNeedShip(raw);
                    _analyzer.MarkShipment(list);
                };
                bk.RunWorkerAsync();
            }
        }

        private void btnDeductionShip_Click(object sender, EventArgs e)
        {
            if (!_analyzer.IsRunning)
            {
                BackgroundWorker bk = new BackgroundWorker();
                bk.DoWork += (sender, e) =>
                {
                    string raw = this.txtOrder.Text;
                    var list = _analyzer.ReadNeedShip(raw);
                    _analyzer.Deduction(list);
                    _analyzer.MarkShipment(list);
                };
                bk.RunWorkerAsync();
            }
        }

        #endregion


        #region page5
        private void btnCreateAnswer_Click(object sender, EventArgs e)
        {
            if (this.rbOne.Checked)
            {
                this.txt.Text = this.txtOne.Text.Replace("[número do conhecimento de embarque]", this.tbFirstLeg.Text).Replace("[URL de consulta]", this.tbFirstLegCarrier.Text);
            }
            else if (this.rbTwo.Checked)
            {
                this.txt.Text = this.txtTwo.Text.Replace("[número do conhecimento de embarque]", this.tbLastLeg.Text).Replace("[URL de consulta]", this.tbInquire.Text).Replace("[Transportadora]", this.tbLastLegCarrier.Text).Replace("[Data estimada de entrega]", this.tbDate.Text);
            }
            else if (this.rbThree.Checked)
            {
                this.txt.Text = this.txtThree.Text.Replace("[número do conhecimento de embarque]", this.tbLastLeg.Text).Replace("[URL de consulta]", this.tbInquire.Text);
            }
        }
        #endregion

        #region page6



        public Image PressImg()
        {
            Bitmap bmp = new Bitmap(104, 30); //这里给104是为了左边和右边空出2个像素，剩余的100就是百分比的值
            Graphics g = Graphics.FromImage(bmp);
            g.Clear(Color.White); //背景填白色
                                  //g.FillRectangle(Brushes.Red, 2, 2, this.Press, 26);  //普通效果
                                  //填充渐变效果
            //g.FillRectangle(new LinearGradientBrush(new Point(30, 2), new Point(30, 30), Color.Black, Color.Gray), 2, 2, this.Press, 26);
            return bmp;
        }


        #endregion



        private async void btnRefush_Click(object sender, EventArgs e)
        {
            this.txtIpShow.Clear();
            foreach (var item in GetList(this.txtFous.Text))
            {
                Position i = rolaClient.RolaCheck(item.Country, item.Region, item.City);
                string result = await rolaClient.RolaRefresh(item.UserName, i.Country, i.Region, i.City);
                Position j = await rolaClient.GetPosition("gate8.rola.vip", 2024, item.UserName, "123");
                this.txtIpShow.AppendText($"{item.UserName} {i.Country} {i.Region} {i.City} {j.IP} {j.Country} {j.Region} {j.City} {result} \r\n");
            }
        }

        private async void btnGetIpPosition_Click(object sender, EventArgs e)
        {
            this.txtIpShow.Clear();
            foreach (var item in GetList(this.txtFous.Text))
            {
                Position i = rolaClient.RolaCheck(item.Country, item.Region, item.City);
                Position j = await rolaClient.GetPosition("gate8.rola.vip", 2024, item.UserName, "123");
                Position p = await rolaClient.CheckIpPosition(j.IP);
                this.txtIpShow.AppendText($"{item.UserName} {i.Country} {i.Region} {i.City} {j.IP} {j.Country} {j.Region} {j.City}  {p.Country} {p.Region} {p.City} \r\n");
            }

        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            InitializePage1();
        }

        private void label18_Click(object sender, EventArgs e)
        {

        }

        private void label23_Click(object sender, EventArgs e)
        {

        }

        private void tabctl_SelectedIndexChanged(object sender, EventArgs e) => SwitchPage(tabctl.SelectedIndex);

        private void btnDeductionClean_Click(object sender, EventArgs e)
        {
            this.txtOrderLog.Clear();
        }

 
    }
}

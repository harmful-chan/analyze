using analyze.core.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace analyze.core.win
{
    public partial class MainForm : System.Windows.Forms.Form
    {
        public MainForm()
        {
            InitializeComponent();
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


        }
        private void button1_Click(object sender, EventArgs e)
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

        string old = "";
        delegate void SetTextCallback(string text);
        private void SetText(string text)
        {
            BeginInvoke(new MethodInvoker(delegate 
            {
                this.txtOrderLog.AppendText($"{text}");
                this.txtOrderLog.ScrollToCaret();
            }));
        }

        private void btnDeduction_Click(object sender, EventArgs e)
        {
        }

        private void Form_Load(object sender, EventArgs e)
        {

        }
    }
}

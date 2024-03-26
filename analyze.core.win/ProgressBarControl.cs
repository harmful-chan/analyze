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
    public partial class ProgressBarControl : UserControl
    {

        private int val;//进度值
        private Color PBackgroundColor = Color.FromArgb(217, 218, 219);//初始化颜色
        private Color PForegroundColor = Color.Green;

        public ProgressBarControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 背景色
        /// </summary>
        public Color pBackgroundColor
        {
            get => PBackgroundColor;
            set
            {
                PBackgroundColor = value;
                this.BackColor = PBackgroundColor;
            }
        }

        /// <summary>
        /// 前景色
        /// </summary>
        public Color pForegroundColor
        {
            get => PForegroundColor;
            set => PForegroundColor = value;
        }

        /// <summary>
        /// 当前值
        /// </summary>
        public int Val
        {
            get => val;
            set
            {
                val = value;
                this.Invalidate();
            }
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            SolidBrush brush = new SolidBrush(PForegroundColor);
            float percent = val ;
            Rectangle rect = this.ClientRectangle;
            rect.Width = (int)(rect.Width * percent);
            rect.Height = this.Height;
            g.FillRectangle(brush, rect);
            brush.Dispose();
            g.Dispose();
        }

        private void ProgressBarControl_Load(object sender, EventArgs e)
        {

        }
    }
}

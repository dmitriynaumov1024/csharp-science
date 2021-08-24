using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using RelationMatrix;

namespace RelationMatrix_Winforms
{
    class XForm: Form
    {
        protected Button closeButton, minimizeButton, dummyButton;
        public XForm()
        {
            this.Name = "Relation matrix checker";
            this.ClientSize = new Size(560, 400);
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = Color.FromArgb(0x30, 0x30, 0x30);

            closeButton = new Button
            {
                Location = new Point(this.ClientSize.Width - 23, 1),
                Size = new Size(22, 18),
                FlatStyle = FlatStyle.Flat,
                Text = "X",
                ForeColor = Color.White
            };
            closeButton.FlatAppearance.BorderSize = 0;
            closeButton.FlatAppearance.MouseOverBackColor = Color.DarkGray;
            closeButton.Click += (sender, args) => this.Close();

            minimizeButton = new Button
            {
                Location = new Point(this.ClientSize.Width - 47, 1),
                Size = new Size(22, 18),
                FlatStyle = FlatStyle.Flat,
                Text = "--",
                ForeColor = Color.White
            };
            minimizeButton.FlatAppearance.BorderSize = 0;
            minimizeButton.FlatAppearance.MouseOverBackColor = Color.DarkGray;
            minimizeButton.Click += (sender, args) => { 
                this.Focus();
                this.WindowState = FormWindowState.Minimized; 
            };

            this.Controls.Add 
            (
                new MainView<Relation>(new Relation(4))
                { 
                    Location = new Point(1, 20),
                    Size = new Size(this.ClientSize.Width - 2, this.ClientSize.Height - 21)
                }
            );

            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.minimizeButton);
        }

        protected override void OnPaint(PaintEventArgs args)
        {
            var gr = args.Graphics;
            gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            gr.DrawString(this.Name, DefaultFont, Brushes.White, new PointF(4, 2));
        }

        protected override void OnPaintBackground(PaintEventArgs args)
        {
            base.OnPaintBackground(args);
            var gr = args.Graphics;
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == WM_NCHITTEST)
                m.Result = (IntPtr)(HT_CAPTION);
        }

        private const int WM_NCHITTEST = 0x84;
        private const int HT_CLIENT = 0x1;
        private const int HT_CAPTION = 0x2;
    }
}

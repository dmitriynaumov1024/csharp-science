using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;

namespace RelationMatrix_Winforms
{
    class XButton: Button
    {
        public XButton(string text): base()
        {
            this.Text = text;
            this.FlatStyle = FlatStyle.Flat;
            this.FlatAppearance.BorderSize = 0;
            this.FlatAppearance.MouseOverBackColor = Color.FromArgb(10, 10, 10, 10);
            this.FlatAppearance.MouseDownBackColor = Color.FromArgb(30, 30, 30, 30);
        }

        public XButton(): this(null) { }
    }
}

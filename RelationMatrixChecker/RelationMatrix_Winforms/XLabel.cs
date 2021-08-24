using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;

namespace RelationMatrix_Winforms
{
    class XLabel: Label
    {
        public XLabel(string text, ContentAlignment alignment = ContentAlignment.MiddleCenter): base()
        {
            this.Text = text;
            this.TextAlign = alignment;
        }
    }
}

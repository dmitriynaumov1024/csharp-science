using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace RelationMatrix_Winforms
{
    class XCheckBoxUnclickable: CheckBox
    {
        public XCheckBoxUnclickable(): base() { }

        // Make it unclickable
        protected override void OnClick(EventArgs e) { }
        protected override void OnMouseDown(MouseEventArgs e) { }
        protected override void OnMouseUp(MouseEventArgs e) { }
        protected override void OnMouseClick(MouseEventArgs e) { }
        protected override void OnMouseDoubleClick(MouseEventArgs e) { }
    }
}

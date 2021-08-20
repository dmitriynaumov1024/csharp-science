using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace RelationMatrix_Winforms
{
    /// <summary>
    /// Main window (form) of Relation Matrix Checker Application.
    /// </summary>
    class MainForm: Form
    {
        public MainForm()
        {
            this.ClientSize = new Size(560, 400);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.Controls.Add(new MainView());
        }
    }
}

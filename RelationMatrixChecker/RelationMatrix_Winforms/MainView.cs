using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using RelationMatrix;

namespace RelationMatrix_Winforms
{
    class MainView<TViewModel>: Control where TViewModel: IRelation, IInteractiveGrid
    {
        protected TViewModel Model;
        protected Button[,] buttonGrid;
        protected CheckBox 
            reflectiveLabel, 
            antireflectiveLabel,
            symmetricLabel,
            antisymmetricLabel,
            asymmetricLabel,
            transitiveLabel,
            fullLabel,
            orderedLabel,
            equivalentLabel;

        const int mainPanelUpperRowSizePercent = 90,
                  mainPanelBottomRowSizePercent = 10,
                  mainPanelLeftColumnSizePercent = 60,
                  mainPanelRightColumnSizePercent = 40;

        protected TableLayoutPanel mainPanel;
        protected FlowLayoutPanel buttonGridPanel;
        protected FlowLayoutPanel infoPanel;

        public MainView(TViewModel model)
        {
            this.Model = model;
            this.InitializeView();
            this.Model.ObjectChanged += this.OnModelChanged;
        }

        protected virtual void InitializeView()
        {
            this.BackColor = Color.FromArgb(0xFA, 0xFA, 0xFA);

            // Setup main layout panel
            this.mainPanel = new TableLayoutPanel(){ 
                Dock = DockStyle.Fill, 
                BackColor = Color.AliceBlue,
                ColumnCount = 2,
                RowCount = 2 
            };
            this.mainPanel.RowStyles.Clear();
            this.mainPanel.ColumnStyles.Clear();
            this.mainPanel.ColumnStyles.Add (new ColumnStyle (SizeType.Percent, mainPanelLeftColumnSizePercent));
            this.mainPanel.ColumnStyles.Add (new ColumnStyle (SizeType.Percent, mainPanelRightColumnSizePercent));
            this.mainPanel.RowStyles.Add (new RowStyle (SizeType.Percent, mainPanelUpperRowSizePercent));
            this.mainPanel.RowStyles.Add (new RowStyle (SizeType.Percent, mainPanelBottomRowSizePercent));

            this.InitializeButtons();
            this.InitializeLabels();
            this.Controls.Add(mainPanel);
        }

        protected virtual void InitializeLabels()
        {
            CheckBox MakeCheckBox(string text)
            { 
                return new CheckBox { 
                    Text = text, 
                    Checked = false,
                    Enabled = false, 
                    CheckAlign = ContentAlignment.MiddleLeft 
                }; 
            };
            this.reflectiveLabel = MakeCheckBox("Reflective");
            this.antireflectiveLabel = MakeCheckBox("Antireflective");
            this.symmetricLabel = MakeCheckBox("Symmetric");
            this.antisymmetricLabel = MakeCheckBox("Antisymmetric");
            this.asymmetricLabel = MakeCheckBox("Asymmetric");
            this.transitiveLabel = MakeCheckBox("Transitive");
            this.fullLabel = MakeCheckBox("Full");
            this.orderedLabel = MakeCheckBox("Ordered");
            this.equivalentLabel = MakeCheckBox("Equivalent");

            this.infoPanel = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown };
            // Debug crutch
            this.infoPanel.BackColor = Color.PaleGreen;

            this.infoPanel.Controls.AddRange
            (
                new Control[] 
                { 
                    this.reflectiveLabel, 
                    this.antireflectiveLabel, 
                    this.symmetricLabel,
                    this.antisymmetricLabel,
                    this.asymmetricLabel,
                    this.transitiveLabel,
                    this.fullLabel,
                    this.orderedLabel,
                    this.equivalentLabel
                }
            );
            this.mainPanel.Controls.Add (this.infoPanel, row: 0, column: 1);
            this.mainPanel.Controls.Add (new Button { Dock = DockStyle.Fill, Text = "DEBUG CRUTCH 1"}, row: 1, column: 0);
            this.mainPanel.Controls.Add (new Button { Dock = DockStyle.Fill, Text = "DEBUG CRUTCH 2"}, row: 1, column: 1);
        }

        protected virtual void UpdateLabels()
        {
            this.reflectiveLabel.Checked = this.Model.IsReflective;
            this.antireflectiveLabel.Checked = this.Model.IsAntireflective;
            this.symmetricLabel.Checked = this.Model.IsSymmetric;
            this.antisymmetricLabel.Checked = this.Model.IsAntisymmetric;
            this.asymmetricLabel.Checked = this.Model.IsAsymmetric;
            this.transitiveLabel.Checked = this.Model.IsTransitive;
            this.fullLabel.Checked = this.Model.IsFull;
            this.orderedLabel.Checked = this.Model.IsOrdered;
            this.equivalentLabel.Checked = this.Model.IsEquivalent;
        }

        protected virtual void InitializeButtons()
        {
            Button MakeFlatButton()
            {
                var result = new Button();
                result.FlatStyle = FlatStyle.Flat;
                result.FlatAppearance.BorderSize = 0;
                return result;
            }

            Button[,] oldButtons = this.buttonGrid;
            int sizeRow = Model.Matrix.GetLength(0), 
                sizeCol = Model.Matrix.GetLength(1),
                oldSizeRow = oldButtons is null ? 0 : oldButtons.GetLength(0),
                oldSizeCol = oldButtons is null ? 0 : oldButtons.GetLength(1),
                keepSizeRow = Math.Min(sizeRow, oldSizeRow),
                keepSizeCol = Math.Min(sizeCol, oldSizeCol);

            this.buttonGrid = new Button[sizeRow, sizeCol];
            for(int row=0; row<sizeRow; row++)
            {
                for(int col=0; col<sizeCol; col++)
                {
                    // Means the button will be used further
                    if (row < keepSizeRow && col < keepSizeCol)
                    {
                        this.buttonGrid[row, col] = oldButtons[row, col];
                    } 
                    // Means the button must be created
                    else
                    {
                        this.buttonGrid[row, col] = MakeFlatButton();
                    }

                    this.buttonGrid[row, col].Text = this.Model.Matrix[row, col] ? "1" : "0";
                }
            }

        }

        protected virtual void OnModelChanged(object sender, GridChangeEventArgs args)
        {
            if (args.WhatChanged == GridChangeEventArgs.Change.Resize)
            {
                InitializeButtons();
                UpdateLabels();
            }
            else if (args.WhatChanged == GridChangeEventArgs.Change.CellToggled)
            {
                try
                {
                    int row = (int)args.Row, col = (int)args.Col;
                    this.buttonGrid[row, col].Text = this.Model.Matrix[row, col] ? "1" : "0";
                }
                finally
                {
                    UpdateLabels();
                }
            }
        }
    }
}

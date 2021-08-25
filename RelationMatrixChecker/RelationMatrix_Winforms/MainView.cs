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
        const string aboutString = 
@"Relation   Matrix   Checker
Copyright (C) 2021 Dmitriy Naumov.
Code from previous own projects 
reused. This Application is free and
comes with no license or warranty.";

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
        protected Panel buttonGridPanel;
        protected GroupBox infoPanel;
        protected FlowLayoutPanel sizeAdjustmentPanel;

        protected Label sizeLabel;

        protected Label aboutLabel;

        protected ToolTip toolTip;

        public MainView(TViewModel model)
        {
            this.Model = model;
            this.InitializeView();
            this.Model.ObjectChanged += this.OnModelChanged;
        }

        protected virtual void InitializeView()
        {
            //this.BackColor = Color.FromArgb(0xFA, 0xFA, 0xFA);
            this.Margin = new Padding(top: 20, left: 0, right: 0, bottom: 0);

            // Setup main layout panel
            this.mainPanel = new TableLayoutPanel(){ 
                Dock = DockStyle.Fill, 
                ColumnCount = 2,
                RowCount = 2 
            };
            this.mainPanel.RowStyles.Clear();
            this.mainPanel.ColumnStyles.Clear();
            this.mainPanel.ColumnStyles.Add (new ColumnStyle (SizeType.Percent, mainPanelLeftColumnSizePercent));
            this.mainPanel.ColumnStyles.Add (new ColumnStyle (SizeType.Percent, mainPanelRightColumnSizePercent));
            this.mainPanel.RowStyles.Add (new RowStyle (SizeType.Percent, mainPanelUpperRowSizePercent));
            this.mainPanel.RowStyles.Add (new RowStyle (SizeType.Percent, mainPanelBottomRowSizePercent));

            this.aboutLabel = new Label { Text = "?", Width = 32, Dock = DockStyle.Right, TextAlign = ContentAlignment.MiddleCenter };
            this.toolTip = new ToolTip();
            this.toolTip.BackColor = Color.White;
            this.toolTip.SetToolTip(aboutLabel, aboutString);

            this.InitializeButtons();
            this.InitializeLabels();
            this.InitializeSizeAdjustment();
            this.mainPanel.Controls.Add (this.buttonGridPanel, row: 0, column: 0);
            this.mainPanel.Controls.Add (this.infoPanel, row: 0, column: 1);
            this.mainPanel.Controls.Add (this.sizeAdjustmentPanel, row: 1, column: 0);
            this.mainPanel.Controls.Add (this.aboutLabel, row: 1, column: 1);
            this.Controls.Add(mainPanel);
            this.UpdateLabels();
        }

        protected virtual void InitializeLabels()
        {
            CheckBox MakeCheckBox(string text)
            { 
                return new XCheckBoxUnclickable { 
                    Text = text, 
                    Checked = false,
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

            this.infoPanel = new GroupBox { Dock = DockStyle.Fill, Text = "Relation properties", Padding = new Padding(10) };

            var infoPanelFlowPanel = new FlowLayoutPanel { 
                Dock = DockStyle.Fill, 
                FlowDirection = FlowDirection.TopDown
            };
            infoPanelFlowPanel.Controls.AddRange (
                new Control[] { 
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
            
            this.infoPanel.Controls.Add(infoPanelFlowPanel);
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
            Button MakeGridButton(int row, int col)
            {
                return new XButtonCoordinated(null, row, col);
            }

            Button[,] oldButtons = this.buttonGrid;
            int rowCount = Model.Matrix.GetLength(0), 
                colCount = Model.Matrix.GetLength(1),
                oldRowCount = oldButtons is null ? 0 : oldButtons.GetLength(0),
                oldColCount = oldButtons is null ? 0 : oldButtons.GetLength(1),
                keepRowCount = Math.Min(rowCount, oldRowCount),
                keepColCount = Math.Min(colCount, oldColCount);

            int buttonPanelHeight = this.ClientSize.Height * mainPanelUpperRowSizePercent / 100,
                buttonPanelWidth = this.ClientSize.Width * mainPanelLeftColumnSizePercent / 100,
                buttonSize = Math.Min (buttonPanelWidth / (colCount + 2), buttonPanelHeight / (rowCount + 2)),
                topLeftX = (int)((buttonPanelWidth - (float)(buttonSize * colCount)) / 2),
                topLeftY = (int)((buttonPanelHeight - (float)(buttonSize * rowCount)) / 2);

            this.buttonGrid = new Button[rowCount, colCount];

            if (this.buttonGridPanel == null) this.buttonGridPanel = new Panel { Dock = DockStyle.Fill };

            // Delete everything unused
            for(int row=0; row<oldRowCount; row++)
            {
                for(int col=0; col<oldColCount; col++)
                {
                    if(row >= rowCount || col >= colCount) 
                    { 
                        this.buttonGridPanel.Controls.Remove(oldButtons[row, col]);
                        oldButtons[row, col].Dispose(); 
                    }
                }
            }

            // Make new button array, reusing some buttons
            for(int row=0; row<rowCount; row++)
            {
                for(int col=0; col<colCount; col++)
                {
                    // Means the button will be used further
                    if (row < keepRowCount && col < keepColCount)
                    {
                        this.buttonGrid[row, col] = oldButtons[row, col];
                    } 
                    // Means the button must be created
                    else
                    {
                        this.buttonGrid[row, col] = MakeGridButton(row, col);
                        this.buttonGrid[row, col].Click += OnGridButtonClick;
                        this.buttonGridPanel.Controls.Add(buttonGrid[row, col]);
                    }
                    var button = this.buttonGrid[row, col];
                    button.Text = this.Model.Matrix[row, col] ? "1" : "0";
                    button.Location = new Point(topLeftX + buttonSize * col, topLeftY + buttonSize * row);
                    button.Size = new Size(buttonSize, buttonSize);
                }
            }
        }

        protected virtual void InitializeSizeAdjustment()
        {
            this.sizeAdjustmentPanel = new FlowLayoutPanel { 
                Dock = DockStyle.Fill, 
                FlowDirection = FlowDirection.LeftToRight 
            };

            var MonospaceFont = new Font(FontFamily.GenericMonospace, 9);

            var clearButton = new Button { Text = "Clear cells" };
            clearButton.Click += (sender, args) => this.Model.Clear();
            
            var minusButton = new Button { Text = "—", Width = 25, Font = MonospaceFont };
            minusButton.Click += (sender, args) => this.Model.TryDecrementSize(keepValues: true);
            
            var plusButton = new Button { Text = "+", Width = 25, Font = MonospaceFont };
            plusButton.Click += (sender, args) => this.Model.TryIncrementSize(keepValues: true);

            this.sizeLabel = new XLabel($"Size: {(this.Model as IRelation).Size}"){ Height = clearButton.Height, Margin = clearButton.Margin };

            this.sizeAdjustmentPanel.Controls.Add (clearButton);
            this.sizeAdjustmentPanel.Controls.Add (this.sizeLabel);
            this.sizeAdjustmentPanel.Controls.Add (minusButton);
            this.sizeAdjustmentPanel.Controls.Add (plusButton);
        }

        protected virtual void UpdateSizeLabel()
        {
            this.sizeLabel.Text = $"Size: {(this.Model as IRelation).Size}";
        }

        protected virtual void OnModelChanged(object sender, GridChangeEventArgs args)
        {
            if (args.WhatChanged == GridChangeEventArgs.Change.Resize)
            {
                InitializeButtons();
                UpdateLabels();
                UpdateSizeLabel();
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

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            InitializeButtons();
        }

        protected virtual void OnGridButtonClick(object sender, EventArgs args)
        {
            var btn = sender as XButtonCoordinated;
            this.Model.ToggleCell(btn.Row, btn.Col);
        }
    }
}

namespace Konditer
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.справочникиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.видыТортовToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuStuffing = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuDecoration = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuCake = new System.Windows.Forms.ToolStripMenuItem();
            this.заказыToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuOrders = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.tsbCategory = new System.Windows.Forms.ToolStripButton();
            this.tsbStuffing = new System.Windows.Forms.ToolStripButton();
            this.tsbDecor = new System.Windows.Forms.ToolStripButton();
            this.tsbCake = new System.Windows.Forms.ToolStripButton();
            this.tsbOrder = new System.Windows.Forms.ToolStripButton();
            this.menuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 371);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(597, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.справочникиToolStripMenuItem,
            this.заказыToolStripMenuItem,
            this.mnuExit});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(597, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // справочникиToolStripMenuItem
            // 
            this.справочникиToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.видыТортовToolStripMenuItem,
            this.mnuStuffing,
            this.mnuDecoration,
            this.mnuCake});
            this.справочникиToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("справочникиToolStripMenuItem.Image")));
            this.справочникиToolStripMenuItem.Name = "справочникиToolStripMenuItem";
            this.справочникиToolStripMenuItem.Size = new System.Drawing.Size(110, 20);
            this.справочникиToolStripMenuItem.Text = "Справочники";
            // 
            // видыТортовToolStripMenuItem
            // 
            this.видыТортовToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("видыТортовToolStripMenuItem.Image")));
            this.видыТортовToolStripMenuItem.Name = "видыТортовToolStripMenuItem";
            this.видыТортовToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.видыТортовToolStripMenuItem.Text = "Виды тортов";
            this.видыТортовToolStripMenuItem.Click += new System.EventHandler(this.видыТортовToolStripMenuItem_Click);
            // 
            // mnuStuffing
            // 
            this.mnuStuffing.Image = ((System.Drawing.Image)(resources.GetObject("mnuStuffing.Image")));
            this.mnuStuffing.Name = "mnuStuffing";
            this.mnuStuffing.Size = new System.Drawing.Size(152, 22);
            this.mnuStuffing.Text = "Начинки";
            this.mnuStuffing.Click += new System.EventHandler(this.mnuStuffing_Click);
            // 
            // mnuDecoration
            // 
            this.mnuDecoration.Image = ((System.Drawing.Image)(resources.GetObject("mnuDecoration.Image")));
            this.mnuDecoration.Name = "mnuDecoration";
            this.mnuDecoration.Size = new System.Drawing.Size(152, 22);
            this.mnuDecoration.Text = "Оформление";
            this.mnuDecoration.Click += new System.EventHandler(this.mnuDecoration_Click);
            // 
            // mnuCake
            // 
            this.mnuCake.Image = ((System.Drawing.Image)(resources.GetObject("mnuCake.Image")));
            this.mnuCake.Name = "mnuCake";
            this.mnuCake.Size = new System.Drawing.Size(152, 22);
            this.mnuCake.Text = "Торты";
            this.mnuCake.Click += new System.EventHandler(this.mnuCake_Click);
            // 
            // заказыToolStripMenuItem
            // 
            this.заказыToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuOrders});
            this.заказыToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("заказыToolStripMenuItem.Image")));
            this.заказыToolStripMenuItem.Name = "заказыToolStripMenuItem";
            this.заказыToolStripMenuItem.Size = new System.Drawing.Size(74, 20);
            this.заказыToolStripMenuItem.Text = "Заказы";
            // 
            // mnuOrders
            // 
            this.mnuOrders.Image = ((System.Drawing.Image)(resources.GetObject("mnuOrders.Image")));
            this.mnuOrders.Name = "mnuOrders";
            this.mnuOrders.Size = new System.Drawing.Size(113, 22);
            this.mnuOrders.Text = "Заказы";
            this.mnuOrders.Click += new System.EventHandler(this.mnuOrders_Click);
            // 
            // mnuExit
            // 
            this.mnuExit.Name = "mnuExit";
            this.mnuExit.Size = new System.Drawing.Size(53, 20);
            this.mnuExit.Text = "Выход";
            this.mnuExit.Click += new System.EventHandler(this.mnuExit_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbCategory,
            this.tsbStuffing,
            this.tsbDecor,
            this.tsbCake,
            this.tsbOrder,
            this.toolStripButton1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(597, 25);
            this.toolStrip1.TabIndex = 3;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton1.Text = "toolStripButton1";
            this.toolStripButton1.ToolTipText = "Выход";
            this.toolStripButton1.Click += new System.EventHandler(this.mnuExit_Click);
            // 
            // tsbCategory
            // 
            this.tsbCategory.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbCategory.Image = ((System.Drawing.Image)(resources.GetObject("tsbCategory.Image")));
            this.tsbCategory.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbCategory.Name = "tsbCategory";
            this.tsbCategory.Size = new System.Drawing.Size(23, 22);
            this.tsbCategory.Text = "toolStripButton2";
            this.tsbCategory.ToolTipText = "Виды тортов";
            this.tsbCategory.Click += new System.EventHandler(this.видыТортовToolStripMenuItem_Click);
            // 
            // tsbStuffing
            // 
            this.tsbStuffing.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbStuffing.Image = ((System.Drawing.Image)(resources.GetObject("tsbStuffing.Image")));
            this.tsbStuffing.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbStuffing.Name = "tsbStuffing";
            this.tsbStuffing.Size = new System.Drawing.Size(23, 22);
            this.tsbStuffing.Text = "toolStripButton3";
            this.tsbStuffing.ToolTipText = "Начинки";
            this.tsbStuffing.Click += new System.EventHandler(this.mnuStuffing_Click);
            // 
            // tsbDecor
            // 
            this.tsbDecor.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbDecor.Image = ((System.Drawing.Image)(resources.GetObject("tsbDecor.Image")));
            this.tsbDecor.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbDecor.Name = "tsbDecor";
            this.tsbDecor.Size = new System.Drawing.Size(23, 22);
            this.tsbDecor.Text = "toolStripButton4";
            this.tsbDecor.ToolTipText = "Оформление";
            this.tsbDecor.Click += new System.EventHandler(this.mnuDecoration_Click);
            // 
            // tsbCake
            // 
            this.tsbCake.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbCake.Image = ((System.Drawing.Image)(resources.GetObject("tsbCake.Image")));
            this.tsbCake.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbCake.Name = "tsbCake";
            this.tsbCake.Size = new System.Drawing.Size(23, 22);
            this.tsbCake.Text = "toolStripButton5";
            this.tsbCake.ToolTipText = "Торты";
            this.tsbCake.Click += new System.EventHandler(this.mnuCake_Click);
            // 
            // tsbOrder
            // 
            this.tsbOrder.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbOrder.Image = ((System.Drawing.Image)(resources.GetObject("tsbOrder.Image")));
            this.tsbOrder.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbOrder.Name = "tsbOrder";
            this.tsbOrder.Size = new System.Drawing.Size(23, 22);
            this.tsbOrder.Text = "toolStripButton6";
            this.tsbOrder.ToolTipText = "Заказы";
            this.tsbOrder.Click += new System.EventHandler(this.mnuOrders_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(597, 393);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Кондитерское ателье";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem справочникиToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem заказыToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mnuExit;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripMenuItem видыТортовToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mnuStuffing;
        private System.Windows.Forms.ToolStripMenuItem mnuDecoration;
        private System.Windows.Forms.ToolStripMenuItem mnuCake;
        private System.Windows.Forms.ToolStripMenuItem mnuOrders;
        private System.Windows.Forms.ToolStripButton tsbCategory;
        private System.Windows.Forms.ToolStripButton tsbStuffing;
        private System.Windows.Forms.ToolStripButton tsbDecor;
        private System.Windows.Forms.ToolStripButton tsbCake;
        private System.Windows.Forms.ToolStripButton tsbOrder;
    }
}
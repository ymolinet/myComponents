namespace myServices
{
    partial class ConfigForm
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.cbLoadOnStart = new System.Windows.Forms.CheckBox();
            this.lbServicesInstalled = new System.Windows.Forms.ListBox();
            this.lbServicesSelected = new System.Windows.Forms.ListBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnDel = new System.Windows.Forms.Button();
            this.btnApply = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cbLoadOnStart
            // 
            this.cbLoadOnStart.AutoSize = true;
            this.cbLoadOnStart.Location = new System.Drawing.Point(12, 12);
            this.cbLoadOnStart.Name = "cbLoadOnStart";
            this.cbLoadOnStart.Size = new System.Drawing.Size(251, 17);
            this.cbLoadOnStart.TabIndex = 0;
            this.cbLoadOnStart.Text = "Charger l\'application au démarrage de Windows";
            this.cbLoadOnStart.UseVisualStyleBackColor = true;
            this.cbLoadOnStart.CheckedChanged += new System.EventHandler(this.cbLoadOnStart_CheckedChanged);
            // 
            // lbServicesInstalled
            // 
            this.lbServicesInstalled.FormattingEnabled = true;
            this.lbServicesInstalled.HorizontalScrollbar = true;
            this.lbServicesInstalled.Location = new System.Drawing.Point(12, 52);
            this.lbServicesInstalled.Name = "lbServicesInstalled";
            this.lbServicesInstalled.Size = new System.Drawing.Size(303, 355);
            this.lbServicesInstalled.TabIndex = 1;
            // 
            // lbServicesSelected
            // 
            this.lbServicesSelected.FormattingEnabled = true;
            this.lbServicesSelected.HorizontalScrollbar = true;
            this.lbServicesSelected.Location = new System.Drawing.Point(402, 52);
            this.lbServicesSelected.Name = "lbServicesSelected";
            this.lbServicesSelected.Size = new System.Drawing.Size(303, 355);
            this.lbServicesSelected.TabIndex = 2;
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(321, 150);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 3;
            this.btnAdd.Text = ">>";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnDel
            // 
            this.btnDel.Location = new System.Drawing.Point(321, 200);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(75, 23);
            this.btnDel.TabIndex = 4;
            this.btnDel.Text = "<<";
            this.btnDel.UseVisualStyleBackColor = true;
            this.btnDel.Click += new System.EventHandler(this.btnDel_Click);
            // 
            // btnApply
            // 
            this.btnApply.Enabled = false;
            this.btnApply.Location = new System.Drawing.Point(283, 414);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(75, 23);
            this.btnApply.TabIndex = 5;
            this.btnApply.Text = "Appliquer";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(365, 414);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 6;
            this.btnClose.Text = "Fermer";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(670, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 13);
            this.label1.TabIndex = 7;
            // 
            // ConfigForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(723, 457);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.btnDel);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.lbServicesSelected);
            this.Controls.Add(this.lbServicesInstalled);
            this.Controls.Add(this.cbLoadOnStart);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConfigForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "myServices - Configuration";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox cbLoadOnStart;
        private System.Windows.Forms.ListBox lbServicesInstalled;
        private System.Windows.Forms.ListBox lbServicesSelected;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnDel;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label label1;
    }
}


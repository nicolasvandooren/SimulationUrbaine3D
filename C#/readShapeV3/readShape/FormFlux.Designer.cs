namespace readShape
{
    partial class FormFlux
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
            this.button1 = new System.Windows.Forms.Button();
            this.textBoxCoordXHaut = new System.Windows.Forms.TextBox();
            this.textBoxCoordYGauche = new System.Windows.Forms.TextBox();
            this.textBoxCoordXBas = new System.Windows.Forms.TextBox();
            this.textBoxCoordYDroite = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(100, 176);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(80, 32);
            this.button1.TabIndex = 0;
            this.button1.Text = "Confirmer";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBoxCoordXHaut
            // 
            this.textBoxCoordXHaut.Location = new System.Drawing.Point(137, 31);
            this.textBoxCoordXHaut.Name = "textBoxCoordXHaut";
            this.textBoxCoordXHaut.Size = new System.Drawing.Size(100, 22);
            this.textBoxCoordXHaut.TabIndex = 1;
            this.textBoxCoordXHaut.Text = "2500226";
            // 
            // textBoxCoordYGauche
            // 
            this.textBoxCoordYGauche.Location = new System.Drawing.Point(137, 62);
            this.textBoxCoordYGauche.Name = "textBoxCoordYGauche";
            this.textBoxCoordYGauche.Size = new System.Drawing.Size(100, 22);
            this.textBoxCoordYGauche.TabIndex = 2;
            this.textBoxCoordYGauche.Text = "1117008";
            // 
            // textBoxCoordXBas
            // 
            this.textBoxCoordXBas.Location = new System.Drawing.Point(137, 90);
            this.textBoxCoordXBas.Name = "textBoxCoordXBas";
            this.textBoxCoordXBas.Size = new System.Drawing.Size(100, 22);
            this.textBoxCoordXBas.TabIndex = 3;
            this.textBoxCoordXBas.Text = "2501120";
            // 
            // textBoxCoordYDroite
            // 
            this.textBoxCoordYDroite.Location = new System.Drawing.Point(137, 119);
            this.textBoxCoordYDroite.Name = "textBoxCoordYDroite";
            this.textBoxCoordYDroite.Size = new System.Drawing.Size(100, 22);
            this.textBoxCoordYDroite.TabIndex = 4;
            this.textBoxCoordYDroite.Text = "1116110";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 17);
            this.label1.TabIndex = 5;
            this.label1.Text = "Coord X haut";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(110, 17);
            this.label2.TabIndex = 6;
            this.label2.Text = "Coord Y gauche";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 95);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(86, 17);
            this.label3.TabIndex = 7;
            this.label3.Text = "Coord X bas";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(20, 124);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(99, 17);
            this.label4.TabIndex = 8;
            this.label4.Text = "Coord Y droite";
            // 
            // FormFlux
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(282, 253);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxCoordYDroite);
            this.Controls.Add(this.textBoxCoordXBas);
            this.Controls.Add(this.textBoxCoordYGauche);
            this.Controls.Add(this.textBoxCoordXHaut);
            this.Controls.Add(this.button1);
            this.Name = "FormFlux";
            this.Text = "FormFlux";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBoxCoordXHaut;
        private System.Windows.Forms.TextBox textBoxCoordYGauche;
        private System.Windows.Forms.TextBox textBoxCoordXBas;
        private System.Windows.Forms.TextBox textBoxCoordYDroite;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
    }
}
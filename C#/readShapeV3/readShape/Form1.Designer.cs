namespace readShape
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbSignalisation = new System.Windows.Forms.RadioButton();
            this.rbFluxEntree = new System.Windows.Forms.RadioButton();
            this.rbVitesse = new System.Windows.Forms.RadioButton();
            this.rbGrapheRoutier = new System.Windows.Forms.RadioButton();
            this.button3 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            resources.ApplyResources(this.button1, "button1");
            this.button1.Name = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            resources.ApplyResources(this.button2, "button2");
            this.button2.Name = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // progressBar1
            // 
            resources.ApplyResources(this.progressBar1, "progressBar1");
            this.progressBar1.Name = "progressBar1";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbSignalisation);
            this.groupBox1.Controls.Add(this.rbFluxEntree);
            this.groupBox1.Controls.Add(this.rbVitesse);
            this.groupBox1.Controls.Add(this.rbGrapheRoutier);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // rbSignalisation
            // 
            resources.ApplyResources(this.rbSignalisation, "rbSignalisation");
            this.rbSignalisation.Name = "rbSignalisation";
            this.rbSignalisation.UseVisualStyleBackColor = true;
            // 
            // rbFluxEntree
            // 
            resources.ApplyResources(this.rbFluxEntree, "rbFluxEntree");
            this.rbFluxEntree.Name = "rbFluxEntree";
            this.rbFluxEntree.UseVisualStyleBackColor = true;
            this.rbFluxEntree.CheckedChanged += new System.EventHandler(this.radioButton3_CheckedChanged);
            // 
            // rbVitesse
            // 
            resources.ApplyResources(this.rbVitesse, "rbVitesse");
            this.rbVitesse.Name = "rbVitesse";
            this.rbVitesse.UseVisualStyleBackColor = true;
            this.rbVitesse.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            // 
            // rbGrapheRoutier
            // 
            resources.ApplyResources(this.rbGrapheRoutier, "rbGrapheRoutier");
            this.rbGrapheRoutier.Checked = true;
            this.rbGrapheRoutier.Name = "rbGrapheRoutier";
            this.rbGrapheRoutier.TabStop = true;
            this.rbGrapheRoutier.UseVisualStyleBackColor = true;
            this.rbGrapheRoutier.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // button3
            // 
            resources.ApplyResources(this.button3, "button3");
            this.button3.Name = "button3";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // Form1
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.button3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbGrapheRoutier;
        private System.Windows.Forms.RadioButton rbVitesse;
        private System.Windows.Forms.RadioButton rbFluxEntree;
        private System.Windows.Forms.RadioButton rbSignalisation;
        private System.Windows.Forms.Button button3;
    }
}


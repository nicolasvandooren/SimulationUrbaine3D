using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;


namespace readShape
{
    public partial class Form1 : Form
    {
        
        public Form1()
        {
            InitializeComponent();
            this.label1.Text = "";
            Bdd myBdd = new Bdd();
            if (!myBdd.emptyBdd()) { 
                DialogResult result = MessageBox.Show("Voulez-vous supprimez les données déjà existante ?","Important Question", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                    myBdd.deleteAll();
            }
            myBdd.closeConnection();
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
        }

        //Charger fichier
        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog();
        }

        private void openFileDialog()
        {
            
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            string path = "";
            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "Fichier SHP (*.shp)|*.shp";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    path = openFileDialog1.FileName;
                    if (path != null)
                    {
                        var buttons = groupBox1.Controls.OfType<RadioButton>().FirstOrDefault(n => n.Checked);
                        int typeFile = buttons.TabIndex + 1;
                        int error = 0;

                        Console.WriteLine("Open BDD");
                        MyShapeFile sf = new MyShapeFile(path, this.progressBar1);
                        switch (typeFile)
                        {
                            case 1:
                                if (!sf.bdd.emptyRoadGraph())
                                {
                                    DialogResult result = MessageBox.Show("Voulez-vous supprimez les données du graphe routier déjà existante ?", "Question importante", MessageBoxButtons.YesNo);
                                    if (result == DialogResult.Yes)
                                        sf.bdd.deleteRoadGraph();
                                }

                                if ((error = sf.insertRoadGraph()) == 0)
                                    this.label1.Text = "Fichier chargé : \n" + Path.GetFileName(path);

                                break;
                            case 2:
                                if (!sf.bdd.emptySpeed())
                                {
                                    DialogResult result = MessageBox.Show("Voulez-vous supprimez les données de la vitesse déjà existante ?", "Question importante", MessageBoxButtons.YesNo);
                                    if (result == DialogResult.Yes)
                                        sf.bdd.deleteSpeed();
                                }

                                if ((error = sf.insertSpeed()) == 0)
                                    this.label1.Text = "Fichier chargé : \n" + Path.GetFileName(path);

                                break;
                            case 3:
                                if (!sf.bdd.emptyFlux())
                                {
                                    DialogResult result = MessageBox.Show("Voulez-vous supprimez les données des flux déjà existante ?", "Question importante", MessageBoxButtons.YesNo);
                                    if (result == DialogResult.Yes)
                                        sf.bdd.deleteFlux();
                                }
                                var formFlux = new FormFlux();
                                formFlux.ShowDialog();
                                double xHaut = formFlux.coordXHaut;
                                double xBas = formFlux.coordXBas;
                                double yLeft = formFlux.coordYGauche;
                                double yRight = formFlux.coordYDroite;
                                Console.WriteLine(xHaut + " " + xBas + " " + yLeft + " " + yRight);
                                if (!sf.bdd.emptyRoadGraph())
                                {
                                    if ((error = sf.insertFlux(xHaut, xBas, yLeft, yRight)) == 0)
                                        this.label1.Text = "Fichier chargé : \n" + Path.GetFileName(path);
                                } else
                                {
                                    MessageBox.Show("Veuillez charger un graphe routier avant l'insertion des flux");
                                }
                                
                                break;
                            case 4:
                                if (!sf.bdd.emptyRoadSign())
                                {
                                    DialogResult result = MessageBox.Show("Voulez-vous supprimez les données des signalisations déjà existante ?", "Question importante", MessageBoxButtons.YesNo);
                                    if (result == DialogResult.Yes)
                                        sf.bdd.deleteRoadSign();
                                }

                                if ((error = sf.insertRoadSign()) == 0)
                                    this.label1.Text = "Fichier chargé : \n" + Path.GetFileName(path);
                                break;
                            default:
                                break;
                        }

                        if (error < 0)
                            this.label1.Text = "Impossible de charger le fichier.";
                        Console.WriteLine("Close BDD");
                        sf.closeBddShape();
                        
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        //Quitter application
        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            var myForm = new FormEdition();
            myForm.Show();
            this.Hide();
        }
    }
}


    


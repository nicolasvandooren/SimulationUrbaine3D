using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace readShape
{
    public partial class FormEdition : Form
    {
        Bdd myBdd = new Bdd();
        public FormEdition()
        {
            InitializeComponent();
            initComboBox();
            initPanel();
        }

        private void initPanel()
        {
            this.panel1.Hide();
            this.labelPanel1.Text = "";
            this.labelPanel2.Text = "";
            this.labelPanel3.Text = "";
            this.labelPanel4.Text = "";
            this.labelPanel5.Text = "";
            this.textBoxPanel1.Hide();
            this.textBoxPanel2.Hide();
            this.textBoxPanel3.Hide();
            this.textBoxPanel4.Hide();
            this.textBoxPanel5.Hide();
        }

        private void initComboBox()
        {
            this.comboBox1.Items.Add("Graphe Routier - Arêtes");
            this.comboBox1.Items.Add("Flux d'entrée");
            this.comboBox1.Items.Add("Signalisation");
            this.comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            this.comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.comboBox2.Items.Clear();
            this.label2.Text = "";
            if (this.comboBox1.SelectedIndex == this.comboBox1.FindStringExact("Graphe Routier - Arêtes")) {
                setSelectedEdge();
            }
            else if (this.comboBox1.SelectedIndex == this.comboBox1.FindStringExact("Flux d'entrée")) {
                setSelectedFlux();
            } else if (this.comboBox1.SelectedIndex == this.comboBox1.FindStringExact("Signalisation")) {
                setSelectedRoadSign();
            }


        }

        private void setSelectedRoadSign()
        {
            this.label1.Text = "Champs à modifier \n id - type de feux";
            List<RoadSign> roadSigns = new List<RoadSign>();
            roadSigns = myBdd.getAllRoadSign();
            foreach (RoadSign r in roadSigns)
            {
                this.comboBox2.Items.Add(r.id + " - " + r.type);
            }
        }

        private void setSelectedFlux()
        {
            this.label1.Text = "Champs à modifier \n id - nom rue";
            List<Vertex> vertex = new List<Vertex>();
            vertex = myBdd.getAllVertexOut();
            List<Edge> edges = new List<Edge>();
            edges = myBdd.getAllEdgesOut();
            foreach (Edge e in edges)
            {
                this.comboBox2.Items.Add(e.id + " - " + e.nomRue);
            }
            initTextBox();
            this.panel1.Show();
            this.labelPanel1.Text = "Angle";
            this.labelPanel2.Text = "Time";
            this.labelPanel3.Text = "Count";
            this.labelPanel4.Text = "";
            this.labelPanel5.Text = "";
            this.textBoxPanel1.Show();
            this.textBoxPanel2.Show();
            this.textBoxPanel3.Show();
            this.textBoxPanel4.Hide();
            this.textBoxPanel5.Hide();
        }
        

        private void setSelectedEdge()
        {
            this.label1.Text = "Champs à modifier \n id - nom de la rue";
            List<Edge> edges = new List<Edge>();
            edges = myBdd.getAllEdges();
            foreach (Edge e in edges)
            {
                this.comboBox2.Items.Add(e.id + " - " + e.nomRue);
            }
            initTextBox();
            this.panel1.Show();
            this.labelPanel1.Text = "Nombre de voie FT";
            this.labelPanel2.Text = "Nombre de voie TF";
            this.labelPanel3.Text = "Vitesse";
            this.labelPanel4.Text = "Hierarchie";
            this.labelPanel5.Text = "Nom de la rue";
            this.textBoxPanel1.Show();
            this.textBoxPanel2.Show();
            this.textBoxPanel3.Show();
            this.textBoxPanel4.Show();
            this.textBoxPanel5.Show();
        }
        private void initTextBox()
        {
            this.textBoxPanel1.Text = "";
            this.textBoxPanel2.Text = "";
            this.textBoxPanel3.Text = "";
            this.textBoxPanel4.Text = "";
            this.textBoxPanel5.Text = "";
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.label2.Text = this.comboBox2.SelectedItem.ToString();
            int id = Convert.ToInt32(this.comboBox2.SelectedItem.ToString().Split(' ')[0]);
            if (this.comboBox1.SelectedIndex == this.comboBox1.FindStringExact("Graphe Routier - Arêtes"))
            {
                initPanelEdge(id);
            }
            else if (this.comboBox1.SelectedIndex == this.comboBox1.FindStringExact("Flux d'entrée"))
            {
                initPanelFlux(id);
            }
            else if (this.comboBox1.SelectedIndex == this.comboBox1.FindStringExact("Signalisation"))
            {
            }
        }
        private void initPanelEdge(int id)
        {
            Edge edge = new Edge();
            edge = myBdd.getEdgeById(id);
            this.textBoxPanel1.Text = Convert.ToString(edge.nbVoieFT);
            this.textBoxPanel2.Text = Convert.ToString(edge.nbVoieTF);
            this.textBoxPanel3.Text = Convert.ToString(edge.vitesse);
            this.textBoxPanel4.Text = Convert.ToString(edge.hierarchie);
            this.textBoxPanel5.Text = Convert.ToString(edge.nomRue);
        }
        private void initPanelFlux(int id)
        {
            Flux flux = new Flux();
            flux = myBdd.getFluxByEdge(id);
            this.textBoxPanel1.Text = Convert.ToString(flux.angle);
            this.textBoxPanel2.Text = Convert.ToString(flux.time);
            this.textBoxPanel3.Text = Convert.ToString(flux.count);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var formMain = new Form1();
            this.Close();
            formMain.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(this.comboBox2.SelectedItem.ToString().Split(' ')[0]);
            if (this.comboBox1.SelectedIndex == this.comboBox1.FindStringExact("Graphe Routier - Arêtes"))
            {

            }
            else if (this.comboBox1.SelectedIndex == this.comboBox1.FindStringExact("Flux d'entrée"))
            {
                myBdd.insertUpdateFluxEntry(Convert.ToDouble(textBoxPanel1.Text), Convert.ToInt32(textBoxPanel2.Text), Convert.ToInt32(textBoxPanel3.Text), id);
                MessageBox.Show("Votre flux a bien été ajouté");
            }
            else if (this.comboBox1.SelectedIndex == this.comboBox1.FindStringExact("Signalisation"))
            {

            }
        }
    }
}

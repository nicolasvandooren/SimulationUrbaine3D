using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;

namespace readShape
{
    public partial class FormFlux : Form
    {
        public FormFlux()
        {
            InitializeComponent();
        }

        public double coordXHaut
        {
            get { return Convert.ToDouble(textBoxCoordXHaut.Text); }
        }

        public double coordXBas
        {
            get { return Convert.ToDouble(textBoxCoordXBas.Text); }
        }

        public double coordYGauche
        {
            get { return Convert.ToDouble(textBoxCoordYGauche.Text); }
        }

        public double coordYDroite
        {
            get { return Convert.ToDouble(textBoxCoordYDroite.Text); }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            double number;
            /*
            NumberStyles style = NumberStyles.AllowDecimalPoint; ;
            CultureInfo culture = CultureInfo.CreateSpecificCulture("fr-FR");
            */
            if (textBoxCoordXHaut.Text == "" || textBoxCoordXBas.Text == "" || textBoxCoordYDroite.Text == "" || textBoxCoordYGauche.Text == "")
            {
                MessageBox.Show("Veuillez remplir tous les champs");
            } else if (!Double.TryParse(textBoxCoordXHaut.Text, out number) || !Double.TryParse(textBoxCoordXBas.Text, out number) || !Double.TryParse(textBoxCoordYDroite.Text, out number) || !Double.TryParse(textBoxCoordYGauche.Text, out number))
            {
                MessageBox.Show("Impossible de convertire en double \n Format : x.y");
            } else { 
                this.Close();
            }
        }
    }
}

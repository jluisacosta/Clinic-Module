using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ModuloClinica
{
    public partial class Dosis : Form
    {
        public Dosis(String compuesto)
        {
            InitializeComponent();
            label.Text += " " + compuesto + " :";
        }

        private void cancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public String dameDosis()
        {
            return dosis_r.Text;
        }
    }
}

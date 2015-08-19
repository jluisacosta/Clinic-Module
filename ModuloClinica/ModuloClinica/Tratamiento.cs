using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar.SuperGrid;

namespace ModuloClinica
{
    public partial class Tratamiento : Form
    {
        public String codigo;

        public Tratamiento(DataSet tratamientos)
        {
            InitializeComponent();
            this.sgrid_tr.PrimaryGrid.DataSource = tratamientos;
        }

        private void aceptar_Click(object sender, EventArgs e)
        {
            GridRow tupla = (GridRow)sgrid_tr.ActiveRow;
            GridCellCollection celdas = tupla.Cells;

            if (tupla != null)
                codigo = celdas[0].Value.ToString();
        }

        private void cancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

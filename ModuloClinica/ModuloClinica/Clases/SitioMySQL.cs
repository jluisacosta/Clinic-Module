using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace ModuloClinica.Clases
{
    class SitioMySQL : Sitio
    {
        private MySqlConnection conexion;
        private MySqlDataAdapter adaptador;

        public SitioMySQL(String id, String cc) : base(id, cc)
        {
            conexion = new MySqlConnection(cadena_conexion);
        }

        public override void dame_dataset_de(String consulta, DataSet data_set)
        {
            try
            {
                adaptador = new MySqlDataAdapter();
                adaptador.SelectCommand = new MySqlCommand(consulta, conexion);
                adaptador.Fill(data_set);
            }
            catch (MySqlException excepcion)
            {
                MessageBox.Show("Manejador: MySql Sitio: " + ID.ToString() + "\n" + excepcion.Message);
            }
        }

        public override void ejecuta_comando(string comando)
        {
            try
            {
                MySqlCommand instruccion = new MySqlCommand(comando, conexion);
                conexion.Open();
                instruccion.ExecuteNonQuery();
                conexion.Close();
            }
            catch (MySqlException excepcion)
            {
                MessageBox.Show("Manejador: MySql Sitio: "+ID.ToString()+"\n"+excepcion.Message);
            }
        }

        public override void dame_columnas_de(String fragmento, List<String> columnas)
        {
            String consulta = "SELECT COLUMN_NAME FROM information_schema.COLUMNS WHERE TABLE_NAME = '"+ fragmento +"'";
            enlista_columnas(consulta, columnas);
        }
    }
}

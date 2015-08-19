using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DevComponents.DotNetBar.SuperGrid;
using DevComponents.DotNetBar.Controls;

namespace ModuloClinica.Clases
{
    class Sitio
    {
        public String ID;
        public String cadena_conexion;

        public Sitio(String identificador, String cadena_manejador)
        {
            ID = identificador;
            cadena_conexion = cadena_manejador;
        }

        public virtual void dame_dataset_de(String consulta,DataSet data_set)
        { }

        public virtual void ejecuta_comando(String comando)
        { }

        public virtual void dame_clave_secuencia(String secuencia, ref String clave)
        { }

        public virtual void dame_columnas_de(String fragmento, List<String> columnas)
        { }



        public void inserta(String tabla_fragmento,String columnas,String clave_primaria,String valores)
        {
            String instruccion = "INSERT INTO " + tabla_fragmento + " " + columnas + " VALUES (" + clave_primaria + valores + ")";
            ejecuta_comando(instruccion);
        }

        public void elimina(String tabla_frgamento,String columna_clave,String clave_primaria)
        {
            String instruccion = "DELETE FROM " + tabla_frgamento + " WHERE " + columna_clave + "=" + clave_primaria;
            ejecuta_comando(instruccion);
        }

        public void elimina_detalle(String tabla_frgamento, String columna1,String columna2, String clave1,String clave2)
        {
            String instruccion = "DELETE FROM " + tabla_frgamento + " WHERE " + columna1 + "=" + clave1+" AND "+columna2+"="+clave2;
            ejecuta_comando(instruccion);
        }

        public void modifica(String tabla_fragmento, String columna_clave, String clave_primaria, List<String> valores)
        {
            String instruccion = "UPDATE " + tabla_fragmento + " SET ";

            foreach (String valor in valores)
            {
                if (valores.IndexOf(valor) != valores.Count - 1)
                    instruccion += valor + ",";
                else
                    instruccion += valor;
            }

            instruccion += " WHERE " + columna_clave + "=" + clave_primaria;

            ejecuta_comando(instruccion);
        }

        public void llena_superGrid_con(String consulta, SuperGridControl super_grid)
        {
            DataSet data_set = new DataSet();
            dame_dataset_de(consulta, data_set);
            super_grid.PrimaryGrid.DataSource = data_set;
        }

        public void llena_combo_estatico(String consulta, ComboBoxEx combo)
        {
            DataSet data_set = new DataSet();
            DataTable data_table;

            dame_dataset_de(consulta, data_set);
            data_table = data_set.Tables[0];
            combo.DataSource = data_table;
            combo.ValueMember = "VALUEMEMBER";
            combo.DisplayMember = "DISPLAYMEMBER";
        }

        public void enlista_columnas(String consulta, List<String> columnas)
        {
            DataSet data_set = new DataSet();
            DataTable tabla_columnas;

            dame_dataset_de(consulta, data_set);
            tabla_columnas = data_set.Tables[0];

            foreach (DataRow tupla in tabla_columnas.Rows)
                columnas.Add(tupla["COLUMN_NAME"].ToString().ToUpper());
        }
    }
}

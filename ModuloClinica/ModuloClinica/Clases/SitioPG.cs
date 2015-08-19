using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Forms;
using Npgsql;
using CrystalDecisions.Windows.Forms;

namespace ModuloClinica.Clases
{
    class SitioPG : Sitio
    {
        private NpgsqlConnection conexion;
        private NpgsqlDataAdapter adaptador;

        public SitioPG(String id, String cc) : base(id, cc)
        {
            conexion = new NpgsqlConnection(cadena_conexion);
        }

        public override void dame_dataset_de(String consulta, DataSet data_set)
        {
            try
            {
                adaptador = new NpgsqlDataAdapter();
                adaptador.SelectCommand = new NpgsqlCommand(consulta, conexion);
                adaptador.Fill(data_set);
            }
            catch (NpgsqlException excepcion)
            {
                MessageBox.Show("Manejador: Postgresql Sitio: " + ID.ToString() + "\n" + excepcion.Message);
            }
        }

        public override void ejecuta_comando(string comando)
        {
            try
            {
                NpgsqlCommand instruccion = new NpgsqlCommand(comando, conexion);
                conexion.Open();
                instruccion.ExecuteNonQuery();
                conexion.Close();
            }
            catch (NpgsqlException excepcion)
            {
                MessageBox.Show("Manejador: Postgresql Sitio: "+ID.ToString()+"\n"+excepcion.Message);
            }
        }

        public override void dame_clave_secuencia(String secuencia, ref String clave)
        {
            String consulta = "SELECT LAST_VALUE+1 FROM "+secuencia+";";
            DataSet data_set = new DataSet();

            dame_dataset_de(consulta, data_set);
            clave = data_set.Tables[0].Rows[0][0].ToString();
        }

        public override void dame_columnas_de(String fragmento, List<String> columnas)
        {
            String consulta = "SELECT COLUMN_NAME FROM information_schema.COLUMNS WHERE TABLE_NAME = '"+fragmento.ToLower()+"'";
            enlista_columnas(consulta, columnas);
        }

        public void generaReporteCitas(VentanaReporte vr, String codigo_tratamiento)
        {
            String consulta = "SELECT CODIGO AS \"Código\",NOMBRE AS \"Tratamiento\" FROM TRATAMIENTOC WHERE CODIGO = "+codigo_tratamiento;

            String consulta2 = "SELECT ID,NUMERO_CONSULTORIO,NOMBRE_PILA,APELLIDO_PATERNO,APELLIDO_MATERNO,FECHA_NACIMIENTO,EDAD,SEXO "
                               +"FROM PACIENTEC WHERE CODIGO_TRATAMIENTO="+codigo_tratamiento;

            String consulta3 = "SELECT (NOMBRE_PILA||' '||APELLIDO_PATERNO||' '||APELLIDO_MATERNO) AS \"Paciente\","
                               +"FECHA AS \"Fecha\",TO_CHAR(HORA,'HH24:MI:SS') AS \"Hora\",ASISTENCIA AS \"Asistencia\" "
                               +"FROM CITAC C INNER JOIN PACIENTEC P ON C.ID_PACIENTE=P.ID AND ID_PACIENTE IN "
                               +"(SELECT ID FROM PACIENTEC WHERE CODIGO_TRATAMIENTO="+codigo_tratamiento+")";

            dsReportes ds = new dsReportes();
            crCitas cr = new crCitas();
            crSubCitas scr = new crSubCitas();
            CrystalReportViewer crv;
            NpgsqlDataAdapter adapter = new NpgsqlDataAdapter();
            NpgsqlDataAdapter adapter2 = new NpgsqlDataAdapter();
            NpgsqlDataAdapter adapter3 = new NpgsqlDataAdapter();

            vr = new VentanaReporte();
            crv = vr.dame_viewer();
            adapter.SelectCommand = new NpgsqlCommand(consulta, conexion);
            adapter.Fill(ds, "Tratamiento");
            adapter2.SelectCommand = new NpgsqlCommand(consulta2, conexion);
            adapter2.Fill(ds, "Paciente");
            adapter3.SelectCommand = new NpgsqlCommand(consulta3, conexion);
            adapter3.Fill(ds, "Consulta");
            cr.SetDataSource(ds);
            scr.SetDataSource(ds);
            crv.ReportSource = cr;
            crv.Show();
            vr.Show();
        }
    }
}

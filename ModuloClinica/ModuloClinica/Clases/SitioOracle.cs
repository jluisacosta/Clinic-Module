using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Forms;
using Oracle.DataAccess.Client;
using CrystalDecisions.Windows.Forms;

namespace ModuloClinica.Clases
{
    class SitioOracle : Sitio
    {
        private OracleConnection conexion;
        private OracleDataAdapter adaptador;

        public SitioOracle(String id, String cc) : base(id, cc)
        {
            conexion = new OracleConnection(cadena_conexion);
        }

        public override void dame_dataset_de(String consulta, DataSet data_set)
        {
            try
            {
                adaptador = new OracleDataAdapter();
                adaptador.SelectCommand = new OracleCommand(consulta, conexion);
                adaptador.Fill(data_set);
            }
            catch (OracleException excepcion)
            {
                MessageBox.Show("Manejador: Oracle Sitio: " + ID.ToString() + "\n" + excepcion.Message);
            }
        }

        public override void ejecuta_comando(String comando)
        {
            try
            {
                OracleCommand instruccion = new OracleCommand(comando, conexion);
                conexion.Open();
                instruccion.ExecuteNonQuery();
                conexion.Close();
            }
            catch (OracleException excepcion)
            {
                MessageBox.Show("Manejador: Oracle Sitio: "+ID.ToString()+"\n"+excepcion.Message);
            }
        }

        public override void dame_clave_secuencia(String secuencia, ref String clave)
        {
            String consulta = "SELECT LAST_NUMBER FROM USER_SEQUENCES WHERE SEQUENCE_NAME = '"+secuencia+"'";
            DataSet data_set = new DataSet();

            dame_dataset_de(consulta, data_set);
            clave = data_set.Tables[0].Rows[0][0].ToString();
        }

        public override void dame_columnas_de(String fragmento, List<String> columnas)
        {
            String consulta = "SELECT COLUMN_NAME FROM ALL_TAB_COLUMNS WHERE TABLE_NAME = '"+fragmento+"'";
            enlista_columnas(consulta, columnas);
        }

        public void generaReporteReceta(VentanaReporte vr,String numero_receta)
        {
            String consulta = "SELECT R.NUMERO AS \"Número\",P.ID AS \"IDP\",(P.NOMBRE_PILA||' '||P.APELLIDO_PATERNO||' '||P.APELLIDO_MATERNO) AS \"Paciente\","
                              +"M.ID AS \"IDM\",(M.NOMBRE_PILA||' '||M.APELLIDO_PATERNO||' '||M.APELLIDO_MATERNO) AS \"Médico\",TO_CHAR(R.FECHA,'DD-MON-YYYY HH24:MI:SS') AS \"Fecha\","
                              + "R.INDICACIONES AS \"Indicaciones\" FROM RECETAF R, NOMBRE_PACIENTE P,NOMBRE_MEDICO M WHERE P.ID = R.ID_PACIENTE AND M.ID=R.ID_MEDICO AND R.NUMERO="+numero_receta;

            String consulta2 = "SELECT M.ID,T.NOMBRE AS \"Tipo\",M.NOMBRE_COMPUESTO AS \"Compuesto\",M.PRESENTACION AS \"Presentación\","
                               + "M.CONTENIDO AS \"Contenido\",M.VIA_ADMINISTRACION AS \"Vía de administración\",M.CBP AS \"c.b.p\",MX.DOSIS AS \"Dosis\" "
                               + "FROM MEDICAMENTOF M,TIPO_MEDICAMENTOF T, MXRF MX WHERE M.ID_TIPO_MEDICAMENTO = T.ID "
                               +"AND M.ID = MX.ID_MEDICAMENTO AND MX.NUMERO_RECETA =" + numero_receta;

            dsReportes ds = new dsReportes();
            crReceta cr = new crReceta();
            CrystalReportViewer crv = vr.dame_viewer();
            OracleDataAdapter adapter = new OracleDataAdapter();
            OracleDataAdapter adapter2 = new OracleDataAdapter();

            adapter.SelectCommand = new OracleCommand(consulta, conexion);
            adapter.Fill(ds, "Encabezado_receta");
            adapter2.SelectCommand = new OracleCommand(consulta2, conexion);
            adapter2.Fill(ds, "MXRF");
            cr.SetDataSource(ds);
            crv.ReportSource = cr;
            crv.Show();
            vr.Show();
        }

        public void generaReporteMedicamentos(VentanaReporte vr)
        {
            String consulta = "SELECT M.ID,M.NOMBRE_COMPUESTO,M.PRESENTACION,M.CONTENIDO,SC.DEMANDA "
                              + "FROM (SELECT MX.ID_MEDICAMENTO, COUNT(MX.ID_MEDICAMENTO) AS DEMANDA "
                              + "FROM MXRF MX GROUP BY MX.ID_MEDICAMENTO ORDER BY COUNT(ID_MEDICAMENTO) DESC) SC,MEDICAMENTOF M "
                              + "WHERE SC.ID_MEDICAMENTO = M.ID AND ROWNUM <=10";

            dsReportes ds = new dsReportes();
            crMedicamento cr = new crMedicamento();
            CrystalReportViewer crv;
            OracleDataAdapter adapter = new OracleDataAdapter();

            vr = new VentanaReporte();
            crv = vr.dame_viewer();
            adapter.SelectCommand = new OracleCommand(consulta, conexion);
            adapter.Fill(ds, "Medicamentos");
            cr.SetDataSource(ds);
            crv.ReportSource = cr;
            crv.Show();
            vr.Show();
        }
    }
}

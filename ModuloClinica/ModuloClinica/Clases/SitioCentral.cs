using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Forms;
using DevComponents.DotNetBar.Controls;
using DevComponents.DotNetBar.SuperGrid;
using IBM.Data.DB2;

namespace ModuloClinica.Clases
{
    class SitioCentral : Sitio
    {
        private DB2Connection conexion;
        private DB2DataAdapter adaptador;
        private SitioOracle SO;
        private SitioMySQL SM1, SM2, SM3;
        private SitioPG SPG;
        private List<SuperGridControl> grids;
        private List<String> consultas;
        private int ID = 0, CC = 1, SEC = 2, NOM = 3, TABLA = 4, TIPO = 5, SITIO = 6, COND = 7, DEP = 8;
        private int G_MTOS = 0,G_PAC = 1,G_MED = 2,G_PC = 3,G_CIT=4,G_TMTOS=5,G_TRA=6,G_PR=7,G_SUS=8;

        public SitioCentral(String id, String cc,List<SuperGridControl> sgc) : base(id, cc)
        {
            conexion = new DB2Connection(cadena_conexion);

            SPG = new SitioPG("CONTROL_INTERNO", dame_cc_de("CONTROL_INTERNO"));
            SO = new SitioOracle("FARMACIA", dame_cc_de("FARMACIA"));
            SM1 = new SitioMySQL("CONSULTORIO1", dame_cc_de("CONSULTORIO1"));
            SM2 = new SitioMySQL("CONSULTORIO2", dame_cc_de("CONSULTORIO2"));
            SM3 = new SitioMySQL("CONSULTORIO3", dame_cc_de("CONSULTORIO3"));
            grids = sgc;
        }

        public override void dame_dataset_de(String consulta,DataSet data_set)
        {
            try
            {
                adaptador = new DB2DataAdapter();
                adaptador.SelectCommand = new DB2Command(consulta, conexion);
                adaptador.Fill(data_set);
            }
            catch (DB2Exception excepcion)
            {
                MessageBox.Show(excepcion.Message);
            }
        }


        //Métodos exclusivos del Sitio Central

        private String dame_cc_de(String nombre_sitio)
        {
            int numero_sitio = dame_numero_sitio(nombre_sitio);
            String consulta = "SELECT CADENA FROM CADENA_CONEXION WHERE ID=" + numero_sitio.ToString() + ";";
            DataSet data_set = new DataSet();
            String cc;

            dame_dataset_de(consulta, data_set);
            cc = data_set.Tables[0].Rows[0][0].ToString();

            return cc;
        } //

        private DataTable dame_fragmentos_de(String tabla)
        {
            String consulta = "SELECT F.ID,C.CADENA,F.SECUENCIA,F.NOMBRE,F.TABLA,F.TIPO,F.SITIO,F.CONDICION,F.DEPENDENCIA "
                              +"FROM FRAGMENTO F INNER JOIN CADENA_CONEXION C ON F.ID_CC=C.ID "
                              +"WHERE TABLA='"+tabla+"';";
            DataSet data_set = new DataSet();
            dame_dataset_de(consulta, data_set);
            return (data_set.Tables[0]);
        } //

        private Sitio dame_sitio_del_fragmento(DataRow fragmento)
        {
            String nombre_fragmento = (String)fragmento[SITIO];
            Sitio buscado = null;

            if (nombre_fragmento == SO.ID)
                buscado = SO;
            else if (nombre_fragmento == SM1.ID)
                buscado = SM1;
            else if (nombre_fragmento == SM2.ID)
                buscado = SM2;
            else if (nombre_fragmento == SM3.ID)
                buscado = SM3;
            else if (nombre_fragmento == SPG.ID)
                buscado = SPG;

            return buscado;
        } //

        private int dame_indice_fragmento_principal(DataTable tabla)
        {
            int indice = -1;
            bool band = true;

            foreach (DataRow tupla in tabla.Rows)
            {
                indice++;
                if (tupla[SEC].ToString() != "")
                {
                    band = false;
                    break;
                }
            }

            if (band)
                indice = -1;

            return indice;
        } //

        private int dame_numero_sitio(String nombre)
        {
            int numero_sitio = 0;

            switch (nombre)
            {
                case "FARMACIA": numero_sitio = 1;
                break;
                case "CONSULTORIO1": numero_sitio = 2;
                break;
                case "CONSULTORIO2": numero_sitio = 3;
                break;
                case "CONSULTORIO3": numero_sitio = 4;
                break;
                case "CONTROL_INTERNO": numero_sitio = 5;
                break;
            }

            return numero_sitio;
        } //

        private void inicializa_consultas()
        {
            consultas = new List<string>();
            consultas.Add("SELECT M.ID,T.NOMBRE AS \"Tipo\",M.NOMBRE_COMPUESTO AS \"Compuesto\",M.PRESENTACION AS \"Presentación\"," +
                          "M.CONTENIDO AS \"Contenido\",M.VIA_ADMINISTRACION AS \"Vía de administración\",M.CBP AS \"c.b.p\",M.EXISTENCIAS " +
                          "FROM MEDICAMENTOF M INNER JOIN TIPO_MEDICAMENTOF T ON M.ID_TIPO_MEDICAMENTO = T.ID");
            consultas.Add("SELECT ID AS \"ID\",CODIGO_TRATAMIENTO AS \"Tratamiento\",NUMERO_CONSULTORIO AS \"Consultorio\","
                          + "NOMBRE_PILA AS \"Nombre\",APELLIDO_PATERNO AS \"A. Paterno\",APELLIDO_MATERNO AS \"A. Materno\","
                          + "FECHA_NACIMIENTO AS \"Fecha nacimiento\",EDAD AS \"Edad\",SEXO AS \"Sexo\" FROM PACIENTEC");
            consultas.Add("SELECT ID AS \"ID\",NUMERO_CONSULTORIO AS \"Consultorio\",NOMBRE_PILA AS \"Nombre\",APELLIDO_PATERNO AS \"A. Paterno\","
                          + "APELLIDO_MATERNO AS \"A. Materno\",TURNO AS \"Turno\",ESPECIALIDAD AS \"Especialidad\" FROM MEDICOC");
            consultas.Add("SELECT ID AS \"ID\",NUMERO_CONSULTORIO AS \"Consultorio\",NOMBRE_PILA AS \"Nombre\",APELLIDO_PATERNO AS \"Apellido paterno\","
                          +"APELLIDO_MATERNO AS \"Apellido materno\" FROM PACIENTEC");
            consultas.Add("SELECT ID_PACIENTE AS \"ID\",(NOMBRE_PILA||' '||APELLIDO_PATERNO||' '||APELLIDO_MATERNO) AS \"Paciente\",FECHA AS \"Fecha\","
                          + "TO_CHAR(HORA,'HH24:MI') AS \"Hora\",ASISTENCIA AS \"Asistencia\" FROM CITAC C INNER JOIN PACIENTEC P ON C.ID_PACIENTE=P.ID");
            consultas.Add("SELECT M.ID,T.NOMBRE AS \"Tipo\",M.NOMBRE_COMPUESTO AS \"Compuesto\",M.PRESENTACION AS \"Presentación\"," +
                          "M.CONTENIDO AS \"Contenido\",M.VIA_ADMINISTRACION AS \"Vía de administración\",M.CBP AS \"c.b.p\" " +
                          "FROM MEDICAMENTOF M INNER JOIN TIPO_MEDICAMENTOF T ON M.ID_TIPO_MEDICAMENTO = T.ID");
            consultas.Add("SELECT CODIGO AS \"Código\",NOMBRE AS \"Nombre\",DESCRIPCION AS \"Descripción\" FROM TRATAMIENTOC");
            consultas.Add("SELECT ID AS \"ID\",NUMERO_CONSULTORIO AS \"Consultorio\",CODIGO_TRATAMIENTO AS \"Tratamiento\",NOMBRE_PILA AS \"Nombre\","
                          +"APELLIDO_PATERNO AS \"Apellido paterno\",APELLIDO_MATERNO AS \"Apellido materno\" FROM PACIENTEC");
            consultas.Add("SELECT R.NUMERO AS \"Num\",P.ID,(P.NOMBRE_PILA||' '||P.APELLIDO_PATERNO||' '||P.APELLIDO_MATERNO) AS \"Paciente\","
                          +"M.ID,(M.NOMBRE_PILA||' '||M.APELLIDO_PATERNO||' '||M.APELLIDO_MATERNO) AS \"Médico que suscribe\","
                          +"TO_CHAR(R.FECHA,'DD-MON-YYYY HH24:MI:SS') AS \"Fecha\", R.INDICACIONES AS \"Indicaciones\" "
                          +"FROM RECETAF R, NOMBRE_PACIENTE P,NOMBRE_MEDICO M WHERE P.ID = R.ID_PACIENTE AND M.ID=R.ID_MEDICO");

        } //agregar consultas de las demas grids

        private void llena_superGrid(Sitio sitio,int grid_indx)
        {
            sitio.llena_superGrid_con(consultas[grid_indx], grids[grid_indx]);
        } //

        private void llena_superGrid_de(String tabla)
        {
            switch (tabla)
            { 
                case "MEDICAMENTO":
                    llena_superGrid(SO, G_MTOS);
                    llena_superGrid(SO, G_TMTOS);
                break;
                case "PACIENTE":
                    llena_superGrid(SPG, G_PAC);
                    llena_superGrid(SPG, G_PC);
                    llena_superGrid(SPG, G_PR);
                break;
                case "MEDICO":
                    llena_superGrid(SPG, G_MED);
                break;
                case "CITA":
                    llena_superGrid(SPG, G_CIT);
                break;
                case "TRATAMIENTO":
                    llena_superGrid(SPG, G_TRA);
                break;
                case "RECETA":
                    llena_superGrid(SO, G_SUS);
                break;

            }
        } //agregar las demas grids

        private void inserta_dependientes(DataTable fragmentos, String columnas, String clave, String valores)
        {
            foreach (DataRow fragmento in fragmentos.Rows)
            {
                Sitio sitio = dame_sitio_del_fragmento(fragmento);
                switch (fragmento[TIPO].ToString())
                {
                    case "N":
                    case "R":
                        sitio.inserta(fragmento[NOM].ToString(), "", clave, valores);
                    break;
                    case "H":
                        inserta_fragmento_H(sitio, fragmento, clave, valores);
                    break;
                    case "V":
                        inserta_fragmento_V(sitio, fragmento, columnas, clave, valores);
                    break;
                }
            }
        }

        private List<String> dame_elementos_condicion(String condicion)
        {
            List<String> elementos = new List<String>();
            String[] operadores = new String[] { "=", ">", "<", ">=", "<=", "!=", "<>" };
            String operador_usado = "";
            String[] items;

            items = condicion.Split(operadores, 2, StringSplitOptions.RemoveEmptyEntries);

            foreach (String op in operadores)
            {
                if (condicion.Contains(op))
                {
                    operador_usado = op;
                    break;
                }
            }

            elementos.Add(items[0]);
            elementos.Add(operador_usado);
            elementos.Add(items[1]);

            return elementos;
        }

        private List<String> enlista_valores(String valores, Boolean FV)
        {
            List<String> lista = new List<string>();
            String[] separadores;
            String[] cv;

            if(FV)
                separadores = new String[] { "(", ")", "," };
            else
                separadores = new String[] { "(", ")", ",", "'" };

            valores = valores.Trim();
            cv = valores.Split(separadores, StringSplitOptions.RemoveEmptyEntries);

            foreach (String c in cv)
                lista.Add(c);

            return lista;
        }

        private Boolean evalua_condicion(String operador, String entrada, String valor_condicion)
        {
            Boolean cumple = false;

            switch (operador)
            {
                case "=":
                    if (entrada == valor_condicion)
                        cumple = true;
                    break;
                case ">":
                    if (Convert.ToInt32(entrada) > Convert.ToInt32(valor_condicion))
                        cumple = true;
                    break;
                case "<":
                    if (Convert.ToInt32(entrada) < Convert.ToInt32(valor_condicion))
                        cumple = true;
                    break;
                case ">=":
                    if (Convert.ToInt32(entrada) >= Convert.ToInt32(valor_condicion))
                        cumple = true;
                    break;
                case "<=":
                    if (Convert.ToInt32(entrada) <= Convert.ToInt32(valor_condicion))
                        cumple = true;
                    break;
                case "!=":
                case "<>":
                    if (entrada != valor_condicion)
                        cumple = true;
                    break;
            }

            return cumple;
        }

        private String dame_valor_entrada(String PIZQ, List<String> columnas, List<String> valores)
        {
            int indice_col = -1;

            foreach (String c in columnas)
            {
                if (PIZQ.Contains(c))
                {
                    indice_col = columnas.IndexOf(c);
                    break;
                }
            }

            if (columnas.Count > valores.Count && indice_col == 0)
                MessageBox.Show("Error al emparejar columnas con valores!");

            if (columnas.Count > valores.Count)
                indice_col -= 1;

            return (valores[indice_col]);
        }

        private void inserta_fragmento_H(Sitio sitio, DataRow fragmento, String clave, String valores)
        {
            String condicion = fragmento[COND].ToString();
            String dependencia = fragmento[DEP].ToString();
            List<String> items = dame_elementos_condicion(condicion);
            int PIZQ = 0, OPERADOR = 1, PDER = 2;
            Boolean cumple_condicion = false;
            String valor_entrada;
            List<String> columnas = new List<string>();

            sitio.dame_columnas_de(fragmento[NOM].ToString(), columnas);
            valor_entrada = dame_valor_entrada(items[PIZQ], columnas, enlista_valores(valores,false));

            if (dependencia == "")
                cumple_condicion = evalua_condicion(items[OPERADOR], valor_entrada, items[PDER]);
            else
            {
                List<String> col_dep = new List<string>();
                sitio.dame_columnas_de(dependencia, col_dep);
                cumple_condicion = valida_dependencia(sitio, dependencia, col_dep[0], valor_entrada);
            }

            if (cumple_condicion)
                sitio.inserta(fragmento[NOM].ToString(), "", clave, valores);
            else
            {
                //MessageBox.Show("No se realizó la inserción en " + fragmento[NOM].ToString());
            }
        }

        private Boolean valida_dependencia(Sitio sitio, String dependencia, String columna_pk, String valor_entrada)
        {
            DataSet data_set = new DataSet();
            String consulta = "SELECT * FROM " + dependencia + " WHERE " + columna_pk + "=" + valor_entrada;
            sitio.dame_dataset_de(consulta, data_set);

            if (data_set.Tables[0].Rows.Count > 0)
                return true;

            return false;
        }

        private void inserta_fragmento_V(Sitio sitio, DataRow fragmento, String columnas, String clave, String valores)
        {
            List<String> columnas_FV = enlista_columnas_FV(fragmento[NOM].ToString());
            List<String> valores_entrada = enlista_valores(valores,true);
            List<String> columnas_completas = enlista_valores(columnas,true);
            String cad_col = formatea_cadena_columnas(columnas_FV);
            String cad_val = "";

            clave = clave.Remove(clave.Length - 1, 1);
            valores_entrada.Insert(0, clave);

            for (int i = columnas_FV.Count-1; i >= 0; i--)
                if(!columnas_completas.Contains(columnas_FV[i]))
                    columnas_completas.Insert(0,columnas_FV[i]);

            cad_val = empareja_datos_FV(columnas_completas, columnas_FV, valores_entrada);

            if (columnas_FV.Count == enlista_valores(cad_val,true).Count)
                sitio.inserta(fragmento[NOM].ToString(), cad_col, "", cad_val);
            else
                MessageBox.Show("Error al emparejar columnas con valores!");
        }

        private String formatea_cadena_columnas(List<String> columnas_FV)
        {
            String cadena = "(";

            foreach (String nombre_columna in columnas_FV)
            {
                cadena += nombre_columna;
                if(columnas_FV.IndexOf(nombre_columna) < columnas_FV.Count-1)
                     cadena += ",";
            }

            cadena += ")";
            return cadena;
        }

        private List<String> enlista_columnas_FV(String fragmento)
        {
            DataSet data_set = new DataSet();
            List<String> columnas = new List<String>();
            DataTable tabla_columnas;
            String consulta = "SELECT A.NOMBRE AS COLUMNA FROM FRAGMENTO F INNER JOIN ATRIBUTO_FV A "
                              +"ON F.ID=A.ID_FRAGMENTO WHERE F.NOMBRE = '"+fragmento+"'";
            dame_dataset_de(consulta, data_set);
            tabla_columnas = data_set.Tables[0];

            foreach (DataRow tupla in tabla_columnas.Rows)
                columnas.Add(tupla[0].ToString());

            return columnas;
        }

        private String empareja_datos_FV(List<String> col_tab, List<String> col_fv, List<String> valores)
        {
            String values = "";
            int indx;

            foreach (String columna in col_fv)
            {
                indx = col_tab.IndexOf(columna);
                values += valores[indx].ToString();
                if (col_fv.IndexOf(columna) != col_fv.Count - 1)
                    values += ",";
            }

            return values;
        }

        private void insercion_detalleR(String tabla_detalle, String clave, List<String[]> val_detalle)
        {
            DataTable fragmentos = dame_fragmentos_de(tabla_detalle);

            foreach (DataRow f in fragmentos.Rows)
            {
                Sitio sitio = dame_sitio_del_fragmento(f);
                foreach (String[] valor in val_detalle)
                    sitio.inserta(f[NOM].ToString(), "", clave, valor[0]+","+valor[1]);
            }
        }
        
        private void insercion_detalle(String tabla_detalle, String clave, List<String> val_detalle)
        {
            DataTable fragmentos = dame_fragmentos_de(tabla_detalle);

            foreach (DataRow f in fragmentos.Rows)
            {
                Sitio sitio = dame_sitio_del_fragmento(f);
                foreach (String valor in val_detalle)
                    sitio.inserta(f[NOM].ToString(), "", clave, valor);
            }
        }

        //Publicos

        public void llena_combos_mt(List<ComboBoxEx> combos)
        {
            List<String> consultas = new List<string>();
            consultas.Add("SELECT DISTINCT NOMBRE_COMPUESTO AS VALUEMEMBER, NOMBRE_COMPUESTO AS DISPLAYMEMBER FROM MEDICAMENTOF");
            consultas.Add("SELECT ID AS VALUEMEMBER, NOMBRE AS DISPLAYMEMBER FROM TIPO_MEDICAMENTOF");
            consultas.Add("SELECT DISTINCT PRESENTACION AS VALUEMEMBER, PRESENTACION AS DISPLAYMEMBER FROM MEDICAMENTOF");
            consultas.Add("SELECT DISTINCT CONTENIDO AS VALUEMEMBER, CONTENIDO AS DISPLAYMEMBER FROM MEDICAMENTOF");
            consultas.Add("SELECT DISTINCT VIA_ADMINISTRACION AS VALUEMEMBER, VIA_ADMINISTRACION AS DISPLAYMEMBER FROM MEDICAMENTOF");
            consultas.Add("SELECT DISTINCT CBP AS VALUEMEMBER, CBP AS DISPLAYMEMBER FROM MEDICAMENTOF");

            for (int i = 0; i < combos.Count; i++)
            {
                SO.llena_combo_estatico(consultas[i], combos[i]);
                combos[i].SelectedIndex = -1;
            }
        } //

        public void inicializa_superGrids()
        {
            inicializa_consultas();
            llena_superGrid(SO, G_MTOS);
            llena_superGrid(SPG, G_PAC);
            llena_superGrid(SPG, G_MED);
            llena_superGrid(SPG, G_PC);
            llena_superGrid(SPG, G_CIT);
            llena_superGrid(SO, G_TMTOS);
            llena_superGrid(SPG, G_TRA);
            llena_superGrid(SPG, G_PR);
            llena_superGrid(SO, G_SUS);
        } //agregar grids

        public void insercion(String tabla, String columnas, String valores,String tabla_detalle,String clave_detalle,Boolean detalle,List<String> val_detalle)
        {
            DataTable fragmentos = dame_fragmentos_de(tabla);
            int indx_fsec = dame_indice_fragmento_principal(fragmentos);
            String clave = "";

            if (indx_fsec > -1) //Si hay secuencia
            {
                DataRow fragmento_ppal = fragmentos.Rows[indx_fsec];
                Sitio sitio = dame_sitio_del_fragmento(fragmento_ppal);
                DataSet data_set = new DataSet();

                sitio.dame_clave_secuencia(fragmento_ppal[SEC].ToString(), ref clave);
                sitio.inserta(fragmento_ppal[NOM].ToString(),columnas,"",valores);
                fragmentos.Rows.RemoveAt(indx_fsec);
            }

            if (clave != "")
                clave += ",";

            if (clave_detalle != "")
                clave = clave_detalle + ",";

            if (detalle)
                insercion_detalle(tabla_detalle, clave, val_detalle);

            inserta_dependientes(fragmentos,columnas,clave,valores);
            llena_superGrid_de(tabla);
        }

        public void insercionR(String tabla, String columnas, String valores, String tabla_detalle, String clave_detalle, Boolean detalle, List<String[]> val_detalle,VentanaReporte vr)
        {
            DataTable fragmentos = dame_fragmentos_de(tabla);
            int indx_fsec = dame_indice_fragmento_principal(fragmentos);
            String clave = "";
            String clave_reporte = "";

            if (indx_fsec > -1) //Si hay secuencia
            {
                DataRow fragmento_ppal = fragmentos.Rows[indx_fsec];
                Sitio sitio = dame_sitio_del_fragmento(fragmento_ppal);
                DataSet data_set = new DataSet();

                sitio.dame_clave_secuencia(fragmento_ppal[SEC].ToString(), ref clave);
                sitio.inserta(fragmento_ppal[NOM].ToString(), columnas, "", valores);
                fragmentos.Rows.RemoveAt(indx_fsec);
            }

            if (clave != "")
            {
                clave_reporte = clave;
                clave += ",";
            }

            if (clave_detalle != "")
                clave = clave_detalle + ",";

            if (detalle)
                insercion_detalleR(tabla_detalle, clave, val_detalle);

            inserta_dependientes(fragmentos, columnas, clave, valores);
            llena_superGrid_de(tabla);

            if (clave_reporte != "")
            {
                vr = new VentanaReporte();
                SO.generaReporteReceta(vr, clave_reporte);
            }
        }

        public void llena_detalle(String detalle,String clave,SuperGridControl sgrid)
        {
            String consulta = "";
            if (detalle == "MXTC")
            {
                consulta = "SELECT CODIGO_TRATAMIENTO AS \"Tratamiento\",ID AS \"ID\",NOMBRE_COMPUESTO AS \"Compuesto\","
                           + "PRESENTACION AS \"Presentación\",CONTENIDO AS \"Contenido\",VIA_ADMINISTRACION AS \"Vía de administración\","
                           + "CBP AS \"c.b.p\" FROM MXTC INNER JOIN MEDICAMENTOC ON MXTC.ID_MEDICAMENTO=MEDICAMENTOC.ID WHERE CODIGO_TRATAMIENTO=" + clave;
                SPG.llena_superGrid_con(consulta, sgrid);
            }
            else if (detalle == "MXRF")
                 {
                     consulta = "SELECT ID AS \"ID\",NOMBRE_COMPUESTO AS \"Compuesto\",PRESENTACION AS \"Presentación\",CONTENIDO AS \"Contenido\","
                                + "VIA_ADMINISTRACION AS \"Vía de administración\",CBP AS \"c.b.p\", DOSIS AS \"Dosis\" FROM MXRF INNER JOIN MEDICAMENTOF "
                                + "ON MXRF.ID_MEDICAMENTO=MEDICAMENTOF.ID WHERE NUMERO_RECETA=" + clave;
                     SO.llena_superGrid_con(consulta, sgrid);
                 }
        }

        public void llena_detalle_tratamiento(String codigo, String consultorio, SuperGridControl grid_medicos, SuperGridControl grid_mtos)
        {
            String consulta_m = consultas[G_MED]+" WHERE NUMERO_CONSULTORIO="+consultorio;
            SPG.llena_superGrid_con(consulta_m, grid_medicos);
            llena_detalle("MXTC", codigo, grid_mtos);
        }

        public void eliminacion(String tabla, String clave)
        {
            DataTable fragmentos = dame_fragmentos_de(tabla);

            foreach (DataRow fragmento in fragmentos.Rows)
            {
                Sitio sitio = dame_sitio_del_fragmento(fragmento);
                List<String> columnas = new List<string>();
                String columna_clave = "";

                sitio.dame_columnas_de(fragmento[NOM].ToString(), columnas);
                columna_clave = columnas[0];
                sitio.elimina(fragmento[NOM].ToString(), columna_clave, clave);
            }

            llena_superGrid_de(tabla);
        }

        public void eliminacion_detalle(String tabla, String clave1, String clave2)
        {
            DataTable fragmentos = dame_fragmentos_de(tabla);

            foreach (DataRow fragmento in fragmentos.Rows)
            {
                Sitio sitio = dame_sitio_del_fragmento(fragmento);
                List<String> columnas_clave = new List<string>();
                sitio.dame_columnas_de(fragmento[NOM].ToString(), columnas_clave);
                sitio.elimina_detalle(fragmento[NOM].ToString(),columnas_clave[0],columnas_clave[1],clave1,clave2);
            }

            llena_superGrid_de(tabla);
        }

        public void modificacion(String tabla,String columna_clave,String clave,DataRow valores)
        {
            DataTable fragmentos = dame_fragmentos_de(tabla);

            foreach (DataRow fragmento in fragmentos.Rows)
            {
                Sitio sitio = dame_sitio_del_fragmento(fragmento);
                List<String> columnas_val = new List<string>();

                if (fragmento[TIPO].ToString() == "V")
                {
                    List<String> columnas_FV = enlista_columnas_FV(fragmento[NOM].ToString());
                    foreach (String atributo in columnas_FV)
                    {
                        if(atributo!=columna_clave)
                            columnas_val.Add(atributo + "=" + valores[atributo].ToString());
                    }
                }
                else
                {
                    int n_columnas = valores.Table.Columns.Count;
                    for (int i = 0; i < n_columnas; i++)
                    {
                        String nom_col = valores.Table.Columns[i].ColumnName;
                        columnas_val.Add(nom_col + "=" + valores[i].ToString());
                    }
                }

                if (columnas_val.Count != 0)
                    sitio.modifica(fragmento[NOM].ToString(),columna_clave,clave,columnas_val);
                else
                    MessageBox.Show("columnas_val Count = 0");
            }

            llena_superGrid_de(tabla);
        }

        public void modificacion_campo(String tabla,String campo,String valor,String campo_clave,String clave,String campo_clave2,String clave2)
        {
            DataTable fragmentos = dame_fragmentos_de(tabla);
            foreach (DataRow fragmento in fragmentos.Rows)
            {
                String consulta = "UPDATE " + fragmento[NOM].ToString() + " SET " + campo + "=" + valor+" WHERE "+campo_clave+"="+clave;
                if (campo_clave2 != "" && clave2 != "")
                    consulta += " AND " + campo_clave2 + "=" + clave2;
                Sitio sitio = dame_sitio_del_fragmento(fragmento);
                sitio.ejecuta_comando(consulta);
            }

            llena_superGrid_de(tabla);
        }

        public void modificacion_tratamiento(String tabla, String campo, String valor, String campo_clave, String clave)
        {
            DataTable fragmentos = dame_fragmentos_de(tabla);
            foreach (DataRow fragmento in fragmentos.Rows)
            {
                if (fragmento[TIPO].ToString() != "V")
                {
                    String consulta = "UPDATE " + fragmento[NOM].ToString() + " SET " + campo + "=" + valor + " WHERE " + campo_clave + "=" + clave;
                    Sitio sitio = dame_sitio_del_fragmento(fragmento);
                    sitio.ejecuta_comando(consulta);
                }
            }

            llena_superGrid_de(tabla);
        }

        public Sitio dame_sitio(String sitio)
        {
            Sitio s = null;

            switch (sitio)
            { 
                case "CONTROL_INTERNO":
                    s = SPG;
                break;
                case "FARMACIA":
                    s = SO;
                break;
            }

            return s;
        }

        public void generaReporteDemanda(VentanaReporte vr)
        {
            SO.generaReporteMedicamentos(vr);
        }

        public void generaReporteTratamiento(VentanaReporte vr, String codigo)
        {
            SPG.generaReporteCitas(vr, codigo);
        }

    }
}

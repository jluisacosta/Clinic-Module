using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using DevComponents.DotNetBar.Controls;
using DevComponents.DotNetBar.SuperGrid;
using System.Windows.Forms;
using ModuloClinica.Clases;

namespace ModuloClinica
{
    public partial class modulo_clinica : Form
    {
        private SitioCentral SC;
        private String cc_sitiocentral = "Server=localhost:50000;Database=ESQUEMAF;UID=db2admin;PWD=db210.5bdd;";
        private Boolean modifica_tm = false;
        private String codigo_tm = "";
        private List<String> med_aux;
        private Tratamiento dialogo_t;
        public VentanaReporte vr;

        public modulo_clinica()
        {
            InitializeComponent();
            SC = new SitioCentral("DB2", cc_sitiocentral,enlista_superGrids());
            inicializa_controles();
        }

        public void inicializa_controles()
        {
            DateTime hoy = DateTime.Today;
            SC.inicializa_superGrids();
            SC.llena_combos_mt(enlista_ComboBoxEx_mt());
            calendario_citas.MinDate = hoy;
            calendario_citas.DisplayMonth = hoy;
            fecha_recetas.Text = "Fecha : " + hoy.Day.ToString() + "/" + hoy.Month.ToString() + "/" + hoy.Year;
            boton_si.Enabled = false;
            boton_no.Enabled = false;
        }

        public List<ComboBoxEx> enlista_ComboBoxEx_mt()
        {
            List<ComboBoxEx> combos = new List<ComboBoxEx>();
            combos.Add(this.compuesto_mt);
            combos.Add(this.tipo_mt);
            combos.Add(this.presentacion_mt);
            combos.Add(this.contenido_mt);
            combos.Add(this.via_admon_mt);
            combos.Add(this.cbp_mt);
            return combos;
        }

        public List<SuperGridControl> enlista_superGrids()
        {
            List<SuperGridControl> lista_grids = new List<SuperGridControl>();
            lista_grids.Add(this.sgrid_mt);
            lista_grids.Add(this.sgrid_pacientes);
            lista_grids.Add(this.sgrid_medicos);
            lista_grids.Add(this.sgrid_pc);
            lista_grids.Add(this.sgrid_citas);
            lista_grids.Add(this.sgrid_tmtos);
            lista_grids.Add(this.sgrid_tratamientos);
            lista_grids.Add(this.sgrid_pr);
            lista_grids.Add(this.sgrid_sus);

            return lista_grids;
        } //ENLISTAR DEMAS GRIDS

        private void mensaje_error(String mensaje)
        {
            MessageBox.Show(mensaje, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        //Interfaz medicamentos

        private void sgrid_mt_RowClick(object sender, GridRowClickEventArgs e)
        {
            GridRow tupla = (GridRow)e.GridRow;
            GridCellCollection celdas = tupla.Cells;

            this.compuesto_mt.Text = celdas[2].Value.ToString();
            this.tipo_mt.Text = celdas[1].Value.ToString();
            this.presentacion_mt.Text = celdas[3].Value.ToString();
            this.contenido_mt.Text = celdas[4].Value.ToString();
            this.via_admon_mt.Text = celdas[5].Value.ToString();
            this.cbp_mt.Text = celdas[6].Value.ToString();
            this.existencias_mt.Value = Convert.ToInt32(celdas[7].Value.ToString());
        }

        private void alta_mt_Click(object sender, EventArgs e)
        {
            if (compuesto_mt.Text != "" && tipo_mt.Text != "" && presentacion_mt.Text != ""
               && contenido_mt.Text != "" && via_admon_mt.Text != "" && cbp_mt.Text != "" && existencias_mt.Value.ToString()!="")
            {
                String valores = tipo_mt.SelectedValue.ToString() + ",'" + compuesto_mt.Text + "','" + presentacion_mt.Text + "','"
                                 + contenido_mt.Text + "','" + via_admon_mt.Text + "','" + cbp_mt.Text + "'," + existencias_mt.Value.ToString();

                SC.insercion("MEDICAMENTO", "(ID_TIPO_MEDICAMENTO,NOMBRE_COMPUESTO,PRESENTACION,CONTENIDO,VIA_ADMINISTRACION,CBP,EXISTENCIAS)",
                            valores,"","",false,null);
                limpia_entradas_mt();
            }
            else
                mensaje_error("No se deben dejar campos vacíos.");
        }

        private void baja_mt_Click(object sender, EventArgs e)
        {
            GridRow tupla_seleccionada = (GridRow)sgrid_mt.ActiveRow;
            String clave = tupla_seleccionada.Cells[0].Value.ToString();

            if (clave != "")
            {
                SC.eliminacion("MEDICAMENTO", clave);
                limpia_entradas_mt();
            }
        }

        private void mod_mt_Click(object sender, EventArgs e)
        {
            if (compuesto_mt.Text != "" && tipo_mt.Text != "" && presentacion_mt.Text != ""
               && contenido_mt.Text != "" && via_admon_mt.Text != "" && cbp_mt.Text != "")
            {
                DataTable tabla = new DataTable();
                GridRow tupla_seleccionada = (GridRow)sgrid_mt.ActiveRow;
                String clave = tupla_seleccionada.Cells[0].Value.ToString();
                String[] columnas = new String[] { "ID_TIPO_MEDICAMENTO","NOMBRE_COMPUESTO",
                                                   "PRESENTACION","CONTENIDO","VIA_ADMINISTRACION",
                                                   "CBP","EXISTENCIAS" };

                foreach (String columna in columnas)
                    tabla.Columns.Add(new DataColumn(columna));

                tabla.Rows.Add(tipo_mt.SelectedValue.ToString(),"'"+compuesto_mt.Text+"'","'"+presentacion_mt.Text+"'","'"+contenido_mt.Text+"'",
                               "'"+via_admon_mt.Text+"'","'"+cbp_mt.Text+"'",tupla_seleccionada.Cells[7].Value.ToString());

                SC.modificacion("MEDICAMENTO","ID",clave,tabla.Rows[0]);
                limpia_entradas_mt();
            }
            else
                mensaje_error("No se deben dejar campos vacíos.");
            
        }

        private void limpia_entradas_mt()
        {
            this.compuesto_mt.SelectedIndex = -1;
            this.tipo_mt.SelectedIndex = -1;
            this.presentacion_mt.SelectedIndex = -1;
            this.contenido_mt.SelectedIndex = -1;
            this.via_admon_mt.SelectedIndex = -1;
            this.cbp_mt.SelectedIndex = -1;
            this.compuesto_mt.Text = "";
            this.tipo_mt.Text = "";
            this.presentacion_mt.Text = "";
            this.contenido_mt.Text = "";
            this.via_admon_mt.Text = "";
            this.cbp_mt.Text = "";
            this.existencias_mt.Value = 10;
        }

        private void limpia_mt_Click(object sender, EventArgs e)
        {
            limpia_entradas_mt();
        }

        private void med_reporte_Click(object sender, EventArgs e)
        {
            SC.generaReporteDemanda(vr);
        }

        //Interfaz pacientes

        private void sgrid_pacientes_RowClick(object sender, GridRowClickEventArgs e)
        {
            GridRow tupla = (GridRow)e.GridRow;
            GridCellCollection celdas = tupla.Cells;

            this.consultorio_p.Text = celdas[2].Value.ToString();
            this.nombre_p.Text = celdas[3].Value.ToString();
            this.app_p.Text = celdas[4].Value.ToString();
            this.apm_p.Text = celdas[5].Value.ToString();
            this.fn_p.Text = celdas[6].Value.ToString();
            this.edad_p.Text = celdas[7].Value.ToString();
            this.sexo_p.Text = celdas[8].Value.ToString();

        }

        private void fn_p_MonthCalendar_DateSelected(object sender, DateRangeEventArgs e)
        {
            DateTime fecha = e.Start;
            DateTime hoy = DateTime.Today;
            int years = hoy.Year - fecha.Year;

            if (fecha.Month > hoy.Month || (fecha.Month==hoy.Month && fecha.Day > hoy.Day))
                years -= 1;

            edad_p.Text = years.ToString();
        }

        private void alta_p_Click(object sender, EventArgs e)
        {
            if (nombre_p.Text != "" && app_p.Text != "" && apm_p.Text != "" && fn_p.IsEmpty != true
                && edad_p.Text != "" && consultorio_p.SelectedItem != null && sexo_p.SelectedItem != null)
            {
                String valores = "NULL," + consultorio_p.SelectedItem.ToString() + ",'" + nombre_p.Text + "','" + app_p.Text + "','" + apm_p.Text + "','" 
                                 + fn_p.Value.Year.ToString()+"/"+fn_p.Value.Month.ToString()+"/"+fn_p.Value.Day.ToString()+ "',"+ edad_p.Text + ",'"
                                 + sexo_p.SelectedItem.ToString() + "'";

                SC.insercion("PACIENTE", "(CODIGO_TRATAMIENTO,NUMERO_CONSULTORIO,NOMBRE_PILA,APELLIDO_PATERNO,APELLIDO_MATERNO,FECHA_NACIMIENTO,EDAD,SEXO)",
                            valores,"","",false,null);
                limpia_entradas_pacientes();
            }
            else
                mensaje_error("No se deben dejar campos vacíos.");
        }

        private void baja_p_Click(object sender, EventArgs e)
        {
            GridRow tupla_seleccionada = (GridRow)sgrid_pacientes.ActiveRow;
            String clave = tupla_seleccionada.Cells[0].Value.ToString();

            if (clave != "")
            {
                SC.eliminacion("PACIENTE", clave);
                limpia_entradas_pacientes();
            }
        }

        private void mod_p_Click(object sender, EventArgs e)
        {
            if (nombre_p.Text != "" && app_p.Text != "" && apm_p.Text != "" && fn_p.IsEmpty != true
                && edad_p.Text != "" && consultorio_p.SelectedItem != null && sexo_p.SelectedItem != null)
            {
                DataTable tabla = new DataTable();
                GridRow tupla_seleccionada = (GridRow)sgrid_pacientes.ActiveRow;
                String clave = tupla_seleccionada.Cells[0].Value.ToString();
                String tratamiento = tupla_seleccionada[1].Value.ToString();
                String[] columnas = new String[] { "CODIGO_TRATAMIENTO","NUMERO_CONSULTORIO","NOMBRE_PILA","APELLIDO_PATERNO",
                                                   "APELLIDO_MATERNO","FECHA_NACIMIENTO","EDAD","SEXO" };

                foreach (String columna in columnas)
                    tabla.Columns.Add(new DataColumn(columna));

                if(tratamiento == "")
                    tratamiento = "NULL";

                tabla.Rows.Add(tratamiento,consultorio_p.SelectedItem.ToString(), "'" + nombre_p.Text + "'", "'" + app_p.Text + "'",
                               "'" + apm_p.Text + "'","'"+fn_p.Value.Year.ToString()+"/"+fn_p.Value.Month.ToString()+"/"+fn_p.Value.Day.ToString()+ "'",
                               edad_p.Text,"'" + sexo_p.SelectedItem.ToString() + "'");

                SC.modificacion("PACIENTE", "ID", clave, tabla.Rows[0]);
                limpia_entradas_pacientes();
            }
            else
                mensaje_error("No se deben dejar campos vacíos.");
        }

        private void limpia_entradas_pacientes()
        {
            this.consultorio_p.SelectedIndex = -1;
            this.nombre_p.Text = "";
            this.app_p.Text = "";
            this.apm_p.Text = "";
            this.fn_p.Text = "";
            this.edad_p.Text = "";
            this.sexo_p.SelectedIndex = -1;
        }

        private void limpia_p_Click(object sender, EventArgs e)
        {
            limpia_entradas_pacientes();
        }

        private void mod_p_MouseMove(object sender, MouseEventArgs e)
        {
            if (consultorio_p.SelectedIndex != -1)
            {
                GridRow tupla = (GridRow)sgrid_pacientes.ActiveRow;
                GridCellCollection celdas = tupla.Cells;

                this.consultorio_p.Text = celdas[2].Value.ToString();
                consultorio_p.Enabled = false;
            }
        }

        private void mod_p_MouseLeave(object sender, EventArgs e)
        {
            if (consultorio_p.SelectedIndex != -1)
                consultorio_p.Enabled = true;
        }

        //Interfaz medicos

        private void sgrid_medicos_RowClick(object sender, GridRowClickEventArgs e)
        {
            GridRow tupla = (GridRow)e.GridRow;
            GridCellCollection celdas = tupla.Cells;

            this.nombre_m.Text = celdas[2].Value.ToString();
            this.app_m.Text = celdas[3].Value.ToString();
            this.apm_m.Text = celdas[4].Value.ToString();
            this.consultorio_m.Text = celdas[1].Value.ToString();
            this.especialidad_m.Text = celdas[6].Value.ToString();

            if (celdas[5].Value.ToString() == "Matutino")
                this.turnom_m.Checked = true;
            else
                this.turnov_m.Checked = true;
        }

        private void alta_m_Click(object sender, EventArgs e)
        {
            if (nombre_m.Text != "" && app_m.Text != "" && apm_m.Text != "" && especialidad_m.SelectedItem != null
                && consultorio_m.SelectedItem != null)
            {
                String turno = "";

                if (turnom_m.Checked == true)
                    turno = "'Matutino'";
                else
                    turno = "'Vespertino'";

                String valores = consultorio_m.SelectedItem.ToString() + ",'" + nombre_m.Text + "','" + app_m.Text + "','" + apm_m.Text + "',"
                                 + turno + ",'" + especialidad_m.SelectedItem.ToString() + "'";
                SC.insercion("MEDICO", "(NUMERO_CONSULTORIO,NOMBRE_PILA,APELLIDO_PATERNO,APELLIDO_MATERNO,TURNO,ESPECIALIDAD)",
                            valores,"","",false,null);
                limpia_entradas_medicos();
            }
            else
                mensaje_error("No se deben dejar campos vacíos.");
        }

        private void baja_m_Click(object sender, EventArgs e)
        {
            GridRow tupla_seleccionada = (GridRow)sgrid_medicos.ActiveRow;
            String clave = tupla_seleccionada.Cells[0].Value.ToString();

            if (clave != "")
            {
                SC.eliminacion("MEDICO", clave);
                limpia_entradas_medicos();
            }
        }

        private void mod_m_Click(object sender, EventArgs e)
        {
            if (nombre_m.Text != "" && app_m.Text != "" && apm_m.Text != "" && especialidad_m.SelectedItem != null
                && consultorio_m.SelectedItem != null)
            {
                DataTable tabla = new DataTable();
                GridRow tupla_seleccionada = (GridRow)sgrid_pacientes.ActiveRow;
                String clave = tupla_seleccionada.Cells[0].Value.ToString();
                String tratamiento = tupla_seleccionada[1].Value.ToString();
                String[] columnas = new String[] { "NUMERO_CONSULTORIO","NOMBRE_PILA","APELLIDO_PATERNO",
                                                   "APELLIDO_MATERNO","TURNO","ESPECIALIDAD" };
                String turno = "";

                if (turnom_m.Checked == true)
                    turno = "'Matutino'";
                else
                    turno = "'Vespertino'";

                foreach (String columna in columnas)
                    tabla.Columns.Add(new DataColumn(columna));

                tabla.Rows.Add(consultorio_m.SelectedItem.ToString(), "'" + nombre_m.Text + "'", "'" + app_m.Text + "'",
                               "'" + apm_m.Text + "'",turno,"'" + especialidad_m.SelectedItem.ToString() + "'");

                SC.modificacion("MEDICO", "ID", clave, tabla.Rows[0]);
                limpia_entradas_medicos();
            }
            else
                mensaje_error("No se deben dejar campos vacíos.");
        }

        private void limpia_entradas_medicos()
        {
            this.nombre_m.Text = "";
            this.app_m.Text = "";
            this.apm_m.Text = "";
            this.consultorio_m.SelectedIndex = -1;
            this.especialidad_m.SelectedIndex = -1;
            this.turnom_m.Checked = true;
        }

        private void limpia_m_Click(object sender, EventArgs e)
        {
            limpia_entradas_medicos();
        }

        private void mod_m_MouseMove(object sender, MouseEventArgs e)
        {
            if (consultorio_m.SelectedIndex != -1)
            {
                GridRow tupla = (GridRow)sgrid_medicos.ActiveRow;
                GridCellCollection celdas = tupla.Cells;

                this.consultorio_m.Text = celdas[1].Value.ToString();
                consultorio_m.Enabled = false;
            }
        }

        private void mod_m_MouseLeave(object sender, EventArgs e)
        {
            if (consultorio_m.SelectedIndex != -1)
                consultorio_m.Enabled = true;
        }

        //Interfaz citas

        private void sgrid_pc_RowClick(object sender, GridRowClickEventArgs e)
        {
            boton_si.Enabled = boton_no.Enabled = false;
        }

        private void sgrid_citas_RowClick(object sender, GridRowClickEventArgs e)
        {
            boton_si.Enabled = boton_no.Enabled = true;
        }

        private void limpia_c_Click(object sender, EventArgs e)
        {
            limpia_entradas_cita();
        }

        private void prog_c_Click(object sender, EventArgs e)
        {
            if (!hora_cita.IsEmpty)
            {
                if (calendario_citas.SelectedDate.CompareTo(DateTime.Today) > 0)
                {
                    GridRow tupla_seleccionada = (GridRow)sgrid_pc.ActiveRow;
                    if (tupla_seleccionada != null)
                    {
                        DateTime fecha = calendario_citas.SelectedDate;
                        DateTime hora = hora_cita.Value;
                        String clave = tupla_seleccionada.Cells[0].Value.ToString();
                        String valores = clave + ",'" + fecha.Year.ToString() + "/" + fecha.Month + "/" + fecha.Day + "','"
                                         + hora.Hour.ToString()+":"+hora.Minute.ToString()+":"+ hora.Second.ToString()+"',NULL";

                        SC.insercion("CITA", "(ID_PACIENTE,FECHA,HORA,ASISTENCIA)", valores,"","",false,null);
                        limpia_entradas_cita();
                    }
                    else
                        mensaje_error("No se ha seleccionado ningun paciente.");
                }
                else
                    mensaje_error("No se pueden programar citas para hoy!.");
            }
            else
                mensaje_error("No se deben dejar campos vacíos.");
        }

        private void cancela_c_Click(object sender, EventArgs e)
        {
            GridRow tupla = (GridRow)sgrid_citas.ActiveRow;
            
            if (tupla != null)
            {
                GridCellCollection celdas = tupla.Cells;
                String[] fecha = celdas[2].Value.ToString().Split(' ');
                SC.eliminacion_detalle("CITA", celdas[0].Value.ToString(), "'"+fecha[0].ToString()+"'");
            }
        }

        private void boton_si_Click(object sender, EventArgs e)
        {
            GridRow tupla = (GridRow)sgrid_citas.ActiveRow;

            if (tupla != null)
            {
                String id_paciente = tupla.Cells[0].Value.ToString();
                String[] fecha = tupla.Cells[2].Value.ToString().Split(' ');
                SC.modificacion_campo("CITA", "ASISTENCIA", "TRUE","ID_PACIENTE",id_paciente,"FECHA","'"+fecha[0].ToString()+"'");
            }
        }

        private void boton_no_Click(object sender, EventArgs e)
        {
            GridRow tupla = (GridRow)sgrid_citas.ActiveRow;

            if (tupla != null)
            {
                String id_paciente = tupla.Cells[0].Value.ToString();
                String[] fecha = tupla.Cells[2].Value.ToString().Split(' ');
                SC.modificacion_campo("CITA", "ASISTENCIA", "FALSE", "ID_PACIENTE", id_paciente, "FECHA", "'" + fecha[0].ToString() + "'");
            }
        }

        public void limpia_entradas_cita()
        {
            calendario_citas.SelectedDate = DateTime.Today;
            hora_cita.Text = "";
        }

        //Interfaz tratamientos

        private void alta_t_Click(object sender, EventArgs e)
        {
            if (nombre_t.Text != "" && descripcion_t.Text != "" && sgrid_tmtos2.PrimaryGrid.Rows.Count > 0)
            {
                List<String> medicamentos = new List<string>();
                foreach (GridRow tupla in sgrid_tmtos2.PrimaryGrid.Rows)
                    medicamentos.Add(tupla[0].Value.ToString());

                if (!modifica_tm)
                {
                    String valores = "'" + nombre_t.Text + "','" + descripcion_t.Text + "'";
                    SC.insercion("TRATAMIENTO", "(NOMBRE,DESCRIPCION)", valores, "MEDICAMENTO POR TRATAMIENTO","", true, medicamentos);
                }
                else
                {
                    DataTable tabla = new DataTable();
                    String[] columnas = new String[] { "NOMBRE","DESCRIPCION" };

                    foreach (String columna in columnas)
                        tabla.Columns.Add(new DataColumn(columna));

                    tabla.Rows.Add("'" + nombre_t.Text + "'","'" + descripcion_t.Text + "'");
                    SC.modificacion("TRATAMIENTO", "CODIGO", codigo_tm, tabla.Rows[0]);

                    foreach (String med in med_aux)
                        SC.eliminacion_detalle("MEDICAMENTO POR TRATAMIENTO", codigo_tm, med);

                    SC.insercion("", "", "", "MEDICAMENTO POR TRATAMIENTO",codigo_tm, true, medicamentos);
                    alta_t.Text = "Alta";
                    stc_tratamiento.SelectedTab = sti_catalogo_t;
                    modifica_tm = false;
                }
                nombre_t.Text = "";
                descripcion_t.Text = "";
                sgrid_tmtos2.PrimaryGrid.Rows.Clear();
            }
            else
                mensaje_error("No se deben dejar campos vacíos.");
        }

        private void elimina_t_Click(object sender, EventArgs e)
        {
            GridRow tupla = (GridRow)sgrid_tratamientos.ActiveRow;

            if (tupla != null)
            {
                String codigo = tupla.Cells[0].Value.ToString();
                foreach (GridRow medicamento in sgrid_det_med.PrimaryGrid.Rows)
                    SC.eliminacion_detalle("MEDICAMENTO POR TRATAMIENTO", codigo, medicamento.Cells[1].Value.ToString());

                SC.eliminacion("TRATAMIENTO", codigo);
                desc_esp_t.Text = "";
                sgrid_det_med.PrimaryGrid.Rows.Clear();
            }
        }

        private void limpia_tratamiento_Click(object sender, EventArgs e)
        {
            nombre_t.Text = "";
            descripcion_t.Text = "";
        }

        private void anadir_tmtos_Click(object sender, EventArgs e)
        {
            GridRow tupla = (GridRow)sgrid_tmtos.ActiveRow;

            if (tupla != null)
            {
                if (sgrid_tmtos2.PrimaryGrid.Columns.Count == 0)
                {
                    GridColumnCollection columnas = sgrid_tmtos.PrimaryGrid.Columns;
                    foreach (GridColumn columna in columnas)
                        sgrid_tmtos2.PrimaryGrid.Columns.Add(new GridColumn(columna.Name));
                }

                GridCellCollection c = tupla.Cells;

                if(!esta_agregado(c[0].ToString()))
                {
                    GridRow nueva = new GridRow(c[0].Value.ToString(),c[1].Value.ToString(),c[2].Value.ToString(),c[3].Value.ToString(),
                                                c[4].Value.ToString(),c[5].Value.ToString(),c[6].Value.ToString());

                    sgrid_tmtos2.PrimaryGrid.Rows.Add((GridElement)nueva);
                }
                else
                    mensaje_error("Ya se ha agregado el mismo medicamento.");
            }
        }

        private void descartar_tmtos_Click(object sender, EventArgs e)
        {
            GridRow tupla = (GridRow)sgrid_tmtos2.ActiveRow;

            if (tupla != null)
                sgrid_tmtos2.PrimaryGrid.Rows.Remove(tupla);
        }

        private Boolean esta_agregado(String clave)
        {
            foreach (GridRow tupla in sgrid_tmtos2.PrimaryGrid.Rows)
            {
                if (tupla.Cells[0].ToString() == clave)
                    return true;
            }

            return false;
        }

        private void rest_t_Click(object sender, EventArgs e)
        {
            nombre_t.Text = "";
            descripcion_t.Text = "";
            alta_t.Text = "Alta";
            modifica_tm = false;
            sgrid_tmtos2.PrimaryGrid.Rows.Clear();
        }

        private void modifica_t_Click_1(object sender, EventArgs e)
        {
            GridRow tupla = (GridRow)sgrid_tratamientos.ActiveRow;

            if (tupla != null)
            {
                GridCellCollection tratamiento = tupla.Cells;
                List<String> ids_med = new List<string>();

                codigo_tm = tratamiento[0].Value.ToString();
                nombre_t.Text = tratamiento[1].Value.ToString();
                descripcion_t.Text = tratamiento[2].Value.ToString();
                sgrid_tmtos2.PrimaryGrid.Rows.Clear();

                if (sgrid_tmtos2.PrimaryGrid.Columns.Count == 0)
                {
                    GridColumnCollection columnas = sgrid_tmtos.PrimaryGrid.Columns;
                    foreach (GridColumn columna in columnas)
                        sgrid_tmtos2.PrimaryGrid.Columns.Add(new GridColumn(columna.Name));
                }

                med_aux = new List<string>();

                foreach (GridRow medicamento in sgrid_det_med.PrimaryGrid.Rows)
                {
                    GridCellCollection m = medicamento.Cells;
                    med_aux.Add(m[1].Value.ToString());
                    GridRow nueva = new GridRow(m[1].Value.ToString(),"", m[2].Value.ToString(), m[3].Value.ToString(), m[4].Value.ToString(),
                                                m[5].Value.ToString(), m[6].Value.ToString());

                    sgrid_tmtos2.PrimaryGrid.Rows.Add((GridElement)nueva);
                }

                sgrid_det_med.PrimaryGrid.Rows.Clear();
                alta_t.Text = "Confirmar";
                stc_tratamiento.SelectedTab = sti_control_t;
                modifica_tm = true;
            }
        }

        private void sgrid_tratamientos_RowClick(object sender, GridRowClickEventArgs e)
        {
            GridRow tupla = (GridRow)sgrid_tratamientos.ActiveRow;

            if (tupla != null)
            {
                desc_esp_t.Text = tupla.Cells[2].Value.ToString();
                SC.llena_detalle("MXTC", tupla[0].Value.ToString(), sgrid_det_med);
            }
        }

        private void genera_reporte_t_Click(object sender, EventArgs e)
        {
            GridRow tupla = (GridRow)sgrid_tratamientos.ActiveRow;

            if (tupla != null)
            {
                String codigo = tupla.Cells[0].Value.ToString();
                SC.generaReporteTratamiento(vr, codigo);
            }
        }

        //Interfaz receta

        private void asignar_t_Click(object sender, EventArgs e)
        {
            GridRow tupla = (GridRow)sgrid_pr.ActiveRow;

            if (tupla != null)
            {
                Sitio sitio = SC.dame_sitio("CONTROL_INTERNO");
                DataSet tratamientos = new DataSet();
                String consulta = "SELECT CODIGO AS \"Código\",NOMBRE AS \"Nombre\" FROM TRATAMIENTOC";

                sitio.dame_dataset_de(consulta, tratamientos);
                dialogo_t = new Tratamiento(tratamientos);

                if (dialogo_t.ShowDialog() == DialogResult.OK)
                {
                    String id_paciente = tupla.Cells[0].Value.ToString();
                    SC.modificacion_tratamiento("PACIENTE", "CODIGO_TRATAMIENTO", dialogo_t.codigo, "ID", id_paciente);
                }
            }
        }

        private void sgrid_pr_RowClick(object sender, GridRowClickEventArgs e)
        {
            GridRow tupla = (GridRow)e.GridRow;

            if (tupla != null)
            {
                GridCellCollection celdas = tupla.Cells;
                String codigo = celdas[2].Value.ToString();
                String consultorio = celdas[1].Value.ToString();
                //sgrid_pr.PrimaryGrid.Rows.Clear();
                if (codigo != "")
                    SC.llena_detalle_tratamiento(codigo, consultorio, sgrid_mr, sgrid_rmtos);
                else
                    mensaje_error("Antes de editar una receta debe\nasignar un tratamiento al paciente.");
            }
        }

        private void anadir_rmtos_Click(object sender, EventArgs e)
        {
            GridRow tupla = (GridRow)sgrid_rmtos.ActiveRow;

            if (tupla != null)
            {
                if (sgrid_rmtos2.PrimaryGrid.Columns.Count == 0)
                {
                    GridColumnCollection columnas = sgrid_rmtos.PrimaryGrid.Columns;
                    foreach (GridColumn columna in columnas)
                        sgrid_rmtos2.PrimaryGrid.Columns.Add(new GridColumn(columna.Name));

                    sgrid_rmtos2.PrimaryGrid.Columns.Add(new GridColumn("Dosis"));
                }

                GridCellCollection c = tupla.Cells;

                if (!esta_agregadoR(c[1].ToString()))
                {
                    Dosis d = new Dosis(c[2].Value.ToString());
                    if (d.ShowDialog() == DialogResult.OK)
                    {
                        GridRow nueva = new GridRow(c[0].Value.ToString(), c[1].Value.ToString(), c[2].Value.ToString(), c[3].Value.ToString(),
                                                    c[4].Value.ToString(), c[5].Value.ToString(), c[6].Value.ToString(), d.dameDosis());

                        sgrid_rmtos2.PrimaryGrid.Rows.Add((GridElement)nueva);
                    }
                }
                else
                    mensaje_error("Ya se ha agregado el mismo medicamento.");
            }
        }

        private Boolean esta_agregadoR(String clave)
        {
            foreach (GridRow tupla in sgrid_rmtos2.PrimaryGrid.Rows)
            {
                if (tupla.Cells[1].ToString() == clave)
                    return true;
            }

            return false;
        }

        private void descartar_rmtos_Click(object sender, EventArgs e)
        {
            GridRow tupla = (GridRow)sgrid_rmtos2.ActiveRow;

            if (tupla != null)
                sgrid_rmtos2.PrimaryGrid.Rows.Remove(tupla);
        }

        private void rest_r_Click(object sender, EventArgs e)
        {
            indicaciones_r.Text = "";
            sgrid_mr.PrimaryGrid.Rows.Clear();
            sgrid_rmtos.PrimaryGrid.Rows.Clear();
            sgrid_rmtos2.PrimaryGrid.Rows.Clear();
        }

        private void expedir_r_Click(object sender, EventArgs e)
        {
            if (indicaciones_r.Text != "" && sgrid_rmtos2.PrimaryGrid.Rows.Count > 0)
            {
                List<String[]> medicamentos = new List<string[]>();
                foreach (GridRow tupla in sgrid_rmtos2.PrimaryGrid.Rows)
                {
                    String[] dupla = new string[2];
                    dupla[0]=tupla[1].Value.ToString();
                    dupla[1]="'"+tupla[7].Value.ToString()+"'";
                    medicamentos.Add(dupla);
                }

                GridRow paciente = (GridRow)sgrid_pr.ActiveRow;
                GridRow medico = (GridRow)sgrid_mr.ActiveRow;
                DateTime hoy = DateTime.Today;
                String valores = paciente.Cells[0].Value.ToString() + "," + medico.Cells[0].Value.ToString() + ",'"
                                    + indicaciones_r.Text + "',CURRENT_TIMESTAMP";

                SC.insercionR("RECETA", "(ID_PACIENTE,ID_MEDICO,INDICACIONES,FECHA)", valores, "MEDICAMENTO POR RECETA", "", true, medicamentos,this.vr);
                
                indicaciones_r.Text = "";
                sgrid_mr.PrimaryGrid.Rows.Clear();
                sgrid_rmtos.PrimaryGrid.Rows.Clear();
                sgrid_rmtos2.PrimaryGrid.Rows.Clear();
            }
            else
                mensaje_error("No se deben dejar campos vacíos.");
        }

        private void sgrid_sus_RowClick(object sender, GridRowClickEventArgs e)
        {
            GridRow tupla = (GridRow)sgrid_sus.ActiveRow;

            if (tupla != null)
            {
                indicaciones_hist.Text = tupla.Cells[6].Value.ToString();
                SC.llena_detalle("MXRF", tupla[0].Value.ToString(), sgrid_det_medrec);
            }
        }

        private void eliminar_rec_Click(object sender, EventArgs e)
        {
            GridRow tupla = (GridRow)sgrid_sus.ActiveRow;

            if (tupla != null)
            {
                String numero = tupla.Cells[0].Value.ToString();
                foreach (GridRow medicamento in sgrid_det_medrec.PrimaryGrid.Rows)
                    SC.eliminacion_detalle("MEDICAMENTO POR RECETA", numero, medicamento.Cells[0].Value.ToString());

                SC.eliminacion("RECETA", numero);
                indicaciones_hist.Text = "";
                sgrid_det_medrec.PrimaryGrid.Rows.Clear();
            }
        }

    }
}

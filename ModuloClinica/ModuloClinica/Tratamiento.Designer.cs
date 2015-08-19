namespace ModuloClinica
{
    partial class Tratamiento
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.sgrid_tr = new DevComponents.DotNetBar.SuperGrid.SuperGridControl();
            this.aceptar = new DevComponents.DotNetBar.ButtonX();
            this.cancelar = new DevComponents.DotNetBar.ButtonX();
            this.reflectionLabel23 = new DevComponents.DotNetBar.Controls.ReflectionLabel();
            this.SuspendLayout();
            // 
            // sgrid_tr
            // 
            this.sgrid_tr.FilterExprColors.SysFunction = System.Drawing.Color.DarkRed;
            this.sgrid_tr.Location = new System.Drawing.Point(42, 64);
            this.sgrid_tr.Name = "sgrid_tr";
            this.sgrid_tr.PrimaryGrid.ActiveRowIndicatorStyle = DevComponents.DotNetBar.SuperGrid.ActiveRowIndicatorStyle.Highlight;
            this.sgrid_tr.PrimaryGrid.AllowEdit = false;
            this.sgrid_tr.PrimaryGrid.ColumnAutoSizeMode = DevComponents.DotNetBar.SuperGrid.ColumnAutoSizeMode.Fill;
            this.sgrid_tr.PrimaryGrid.EnableColumnFiltering = true;
            this.sgrid_tr.PrimaryGrid.EnableFiltering = true;
            this.sgrid_tr.PrimaryGrid.Filter.Visible = true;
            this.sgrid_tr.PrimaryGrid.SelectionGranularity = DevComponents.DotNetBar.SuperGrid.SelectionGranularity.Row;
            this.sgrid_tr.PrimaryGrid.UseAlternateRowStyle = true;
            this.sgrid_tr.Size = new System.Drawing.Size(350, 165);
            this.sgrid_tr.TabIndex = 2;
            // 
            // aceptar
            // 
            this.aceptar.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.aceptar.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.aceptar.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.aceptar.Font = new System.Drawing.Font("Berlin Sans FB", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.aceptar.ImageFixedSize = new System.Drawing.Size(30, 30);
            this.aceptar.Location = new System.Drawing.Point(282, 242);
            this.aceptar.Name = "aceptar";
            this.aceptar.Shape = new DevComponents.DotNetBar.RoundRectangleShapeDescriptor(15);
            this.aceptar.Size = new System.Drawing.Size(110, 35);
            this.aceptar.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.aceptar.TabIndex = 6;
            this.aceptar.Text = "Aceptar";
            this.aceptar.TextColor = System.Drawing.SystemColors.HotTrack;
            this.aceptar.Click += new System.EventHandler(this.aceptar_Click);
            // 
            // cancelar
            // 
            this.cancelar.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.cancelar.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.cancelar.Font = new System.Drawing.Font("Berlin Sans FB", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cancelar.ImageFixedSize = new System.Drawing.Size(30, 30);
            this.cancelar.Location = new System.Drawing.Point(42, 242);
            this.cancelar.Name = "cancelar";
            this.cancelar.Shape = new DevComponents.DotNetBar.RoundRectangleShapeDescriptor(15);
            this.cancelar.Size = new System.Drawing.Size(110, 35);
            this.cancelar.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cancelar.TabIndex = 7;
            this.cancelar.Text = "Cancelar";
            this.cancelar.TextColor = System.Drawing.SystemColors.HotTrack;
            this.cancelar.Click += new System.EventHandler(this.cancelar_Click);
            // 
            // reflectionLabel23
            // 
            this.reflectionLabel23.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.reflectionLabel23.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.reflectionLabel23.Font = new System.Drawing.Font("Berlin Sans FB", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.reflectionLabel23.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.reflectionLabel23.Location = new System.Drawing.Point(40, 22);
            this.reflectionLabel23.Name = "reflectionLabel23";
            this.reflectionLabel23.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.reflectionLabel23.Size = new System.Drawing.Size(256, 40);
            this.reflectionLabel23.TabIndex = 8;
            this.reflectionLabel23.Text = "Seleccionar nuevo tratamiento :";
            // 
            // Tratamiento
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightSkyBlue;
            this.ClientSize = new System.Drawing.Size(434, 297);
            this.Controls.Add(this.cancelar);
            this.Controls.Add(this.aceptar);
            this.Controls.Add(this.sgrid_tr);
            this.Controls.Add(this.reflectionLabel23);
            this.Name = "Tratamiento";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Tratamientos";
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.SuperGrid.SuperGridControl sgrid_tr;
        private DevComponents.DotNetBar.ButtonX aceptar;
        private DevComponents.DotNetBar.ButtonX cancelar;
        private DevComponents.DotNetBar.Controls.ReflectionLabel reflectionLabel23;
    }
}
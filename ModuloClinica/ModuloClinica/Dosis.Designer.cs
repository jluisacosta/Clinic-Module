namespace ModuloClinica
{
    partial class Dosis
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
            this.dosis_r = new DevComponents.DotNetBar.Controls.TextBoxX();
            this.label = new DevComponents.DotNetBar.Controls.ReflectionLabel();
            this.cancelar = new DevComponents.DotNetBar.ButtonX();
            this.aceptar = new DevComponents.DotNetBar.ButtonX();
            this.SuspendLayout();
            // 
            // dosis_r
            // 
            // 
            // 
            // 
            this.dosis_r.Border.Class = "TextBoxBorder";
            this.dosis_r.Border.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.dosis_r.Font = new System.Drawing.Font("Berlin Sans FB", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dosis_r.ForeColor = System.Drawing.Color.SteelBlue;
            this.dosis_r.Location = new System.Drawing.Point(42, 66);
            this.dosis_r.MaxLength = 100;
            this.dosis_r.Multiline = true;
            this.dosis_r.Name = "dosis_r";
            this.dosis_r.PreventEnterBeep = true;
            this.dosis_r.Size = new System.Drawing.Size(300, 80);
            this.dosis_r.TabIndex = 5;
            this.dosis_r.WatermarkColor = System.Drawing.Color.LightSkyBlue;
            this.dosis_r.WatermarkText = "Dosis";
            // 
            // label
            // 
            this.label.BackColor = System.Drawing.Color.Transparent;
            // 
            // 
            // 
            this.label.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.label.Font = new System.Drawing.Font("Berlin Sans FB", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.label.Location = new System.Drawing.Point(40, 22);
            this.label.Name = "label";
            this.label.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label.Size = new System.Drawing.Size(317, 40);
            this.label.TabIndex = 9;
            this.label.Text = "Indicar dosis de";
            // 
            // cancelar
            // 
            this.cancelar.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.cancelar.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.cancelar.Font = new System.Drawing.Font("Berlin Sans FB", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cancelar.ImageFixedSize = new System.Drawing.Size(30, 30);
            this.cancelar.Location = new System.Drawing.Point(42, 160);
            this.cancelar.Name = "cancelar";
            this.cancelar.Shape = new DevComponents.DotNetBar.RoundRectangleShapeDescriptor(15);
            this.cancelar.Size = new System.Drawing.Size(110, 35);
            this.cancelar.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.cancelar.TabIndex = 11;
            this.cancelar.Text = "Cancelar";
            this.cancelar.TextColor = System.Drawing.SystemColors.HotTrack;
            this.cancelar.Click += new System.EventHandler(this.cancelar_Click);
            // 
            // aceptar
            // 
            this.aceptar.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.aceptar.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.aceptar.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.aceptar.Font = new System.Drawing.Font("Berlin Sans FB", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.aceptar.ImageFixedSize = new System.Drawing.Size(30, 30);
            this.aceptar.Location = new System.Drawing.Point(232, 160);
            this.aceptar.Name = "aceptar";
            this.aceptar.Shape = new DevComponents.DotNetBar.RoundRectangleShapeDescriptor(15);
            this.aceptar.Size = new System.Drawing.Size(110, 35);
            this.aceptar.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.aceptar.TabIndex = 10;
            this.aceptar.Text = "Aceptar";
            this.aceptar.TextColor = System.Drawing.SystemColors.HotTrack;
            // 
            // Dosis
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightSkyBlue;
            this.ClientSize = new System.Drawing.Size(384, 212);
            this.Controls.Add(this.cancelar);
            this.Controls.Add(this.aceptar);
            this.Controls.Add(this.label);
            this.Controls.Add(this.dosis_r);
            this.Name = "Dosis";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Agregar medicamento a receta";
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.Controls.TextBoxX dosis_r;
        private DevComponents.DotNetBar.Controls.ReflectionLabel label;
        private DevComponents.DotNetBar.ButtonX cancelar;
        private DevComponents.DotNetBar.ButtonX aceptar;
    }
}
namespace MyPOS
{
    partial class Frm_Login
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
            this.metroLabel1 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel2 = new MetroFramework.Controls.MetroLabel();
            this.txt_usuario = new MetroFramework.Controls.MetroTextBox();
            this.txt_contraseña = new MetroFramework.Controls.MetroTextBox();
            this.chec_recordar_datos = new MetroFramework.Controls.MetroToggle();
            this.metroLabel3 = new MetroFramework.Controls.MetroLabel();
            this.btn_login = new MetroFramework.Controls.MetroButton();
            this.btn_cerrar = new MetroFramework.Controls.MetroButton();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // metroLabel1
            // 
            this.metroLabel1.AutoSize = true;
            this.metroLabel1.Location = new System.Drawing.Point(13, 133);
            this.metroLabel1.Name = "metroLabel1";
            this.metroLabel1.Size = new System.Drawing.Size(53, 19);
            this.metroLabel1.TabIndex = 0;
            this.metroLabel1.Text = "Cuenta:";
            // 
            // metroLabel2
            // 
            this.metroLabel2.AutoSize = true;
            this.metroLabel2.Location = new System.Drawing.Point(13, 170);
            this.metroLabel2.Name = "metroLabel2";
            this.metroLabel2.Size = new System.Drawing.Size(78, 19);
            this.metroLabel2.TabIndex = 1;
            this.metroLabel2.Text = "Contraseña:";
            // 
            // txt_usuario
            // 
            // 
            // 
            // 
            this.txt_usuario.CustomButton.Image = null;
            this.txt_usuario.CustomButton.Location = new System.Drawing.Point(122, 1);
            this.txt_usuario.CustomButton.Name = "";
            this.txt_usuario.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.txt_usuario.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.txt_usuario.CustomButton.TabIndex = 1;
            this.txt_usuario.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.txt_usuario.CustomButton.UseSelectable = true;
            this.txt_usuario.CustomButton.Visible = false;
            this.txt_usuario.Lines = new string[0];
            this.txt_usuario.Location = new System.Drawing.Point(154, 133);
            this.txt_usuario.MaxLength = 32767;
            this.txt_usuario.Name = "txt_usuario";
            this.txt_usuario.PasswordChar = '\0';
            this.txt_usuario.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txt_usuario.SelectedText = "";
            this.txt_usuario.SelectionLength = 0;
            this.txt_usuario.SelectionStart = 0;
            this.txt_usuario.ShortcutsEnabled = true;
            this.txt_usuario.Size = new System.Drawing.Size(144, 23);
            this.txt_usuario.TabIndex = 2;
            this.txt_usuario.UseSelectable = true;
            this.txt_usuario.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.txt_usuario.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // txt_contraseña
            // 
            // 
            // 
            // 
            this.txt_contraseña.CustomButton.Image = null;
            this.txt_contraseña.CustomButton.Location = new System.Drawing.Point(122, 1);
            this.txt_contraseña.CustomButton.Name = "";
            this.txt_contraseña.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.txt_contraseña.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.txt_contraseña.CustomButton.TabIndex = 1;
            this.txt_contraseña.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.txt_contraseña.CustomButton.UseSelectable = true;
            this.txt_contraseña.CustomButton.Visible = false;
            this.txt_contraseña.Lines = new string[0];
            this.txt_contraseña.Location = new System.Drawing.Point(154, 165);
            this.txt_contraseña.MaxLength = 32767;
            this.txt_contraseña.Name = "txt_contraseña";
            this.txt_contraseña.PasswordChar = '*';
            this.txt_contraseña.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txt_contraseña.SelectedText = "";
            this.txt_contraseña.SelectionLength = 0;
            this.txt_contraseña.SelectionStart = 0;
            this.txt_contraseña.ShortcutsEnabled = true;
            this.txt_contraseña.Size = new System.Drawing.Size(144, 23);
            this.txt_contraseña.TabIndex = 3;
            this.txt_contraseña.UseSelectable = true;
            this.txt_contraseña.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.txt_contraseña.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // chec_recordar_datos
            // 
            this.chec_recordar_datos.AutoSize = true;
            this.chec_recordar_datos.Location = new System.Drawing.Point(218, 194);
            this.chec_recordar_datos.Name = "chec_recordar_datos";
            this.chec_recordar_datos.Size = new System.Drawing.Size(80, 17);
            this.chec_recordar_datos.TabIndex = 4;
            this.chec_recordar_datos.Text = "Off";
            this.chec_recordar_datos.UseSelectable = true;
            // 
            // metroLabel3
            // 
            this.metroLabel3.AutoSize = true;
            this.metroLabel3.Location = new System.Drawing.Point(51, 194);
            this.metroLabel3.Name = "metroLabel3";
            this.metroLabel3.Size = new System.Drawing.Size(161, 19);
            this.metroLabel3.TabIndex = 5;
            this.metroLabel3.Text = "Guardar datos de acceso?";
            // 
            // btn_login
            // 
            this.btn_login.Location = new System.Drawing.Point(128, 238);
            this.btn_login.Name = "btn_login";
            this.btn_login.Size = new System.Drawing.Size(75, 23);
            this.btn_login.TabIndex = 6;
            this.btn_login.Text = "Entrar";
            this.btn_login.UseSelectable = true;
            this.btn_login.Click += new System.EventHandler(this.btn_login_Click);
            // 
            // btn_cerrar
            // 
            this.btn_cerrar.Location = new System.Drawing.Point(223, 238);
            this.btn_cerrar.Name = "btn_cerrar";
            this.btn_cerrar.Size = new System.Drawing.Size(75, 23);
            this.btn_cerrar.TabIndex = 7;
            this.btn_cerrar.Text = "Cerrar";
            this.btn_cerrar.UseSelectable = true;
            this.btn_cerrar.Click += new System.EventHandler(this.btn_cerrar_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::MyPOS.Properties.Resources.Login;
            this.pictureBox1.Location = new System.Drawing.Point(117, 54);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(202, 73);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 8;
            this.pictureBox1.TabStop = false;
            // 
            // Frm_Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackImage = global::MyPOS.Properties.Resources.Login;
            this.ClientSize = new System.Drawing.Size(360, 264);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.btn_cerrar);
            this.Controls.Add(this.btn_login);
            this.Controls.Add(this.metroLabel3);
            this.Controls.Add(this.chec_recordar_datos);
            this.Controls.Add(this.txt_contraseña);
            this.Controls.Add(this.txt_usuario);
            this.Controls.Add(this.metroLabel2);
            this.Controls.Add(this.metroLabel1);
            this.Name = "Frm_Login";
            this.Resizable = false;
            this.Text = "Acceso al sistema";
            this.Load += new System.EventHandler(this.Frm_Login_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MetroFramework.Controls.MetroLabel metroLabel1;
        private MetroFramework.Controls.MetroLabel metroLabel2;
        private MetroFramework.Controls.MetroTextBox txt_usuario;
        private MetroFramework.Controls.MetroTextBox txt_contraseña;
        private MetroFramework.Controls.MetroToggle chec_recordar_datos;
        private MetroFramework.Controls.MetroLabel metroLabel3;
        private MetroFramework.Controls.MetroButton btn_login;
        private MetroFramework.Controls.MetroButton btn_cerrar;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}
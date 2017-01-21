using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
//using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Windows.Forms;

using System.Windows.Forms;
using MetroFramework.Controls;
using MetroFramework;
using System.Xml;
using Utilerias;
using System.IO;
using MetroFramework.Forms;
using System.Runtime.InteropServices;

namespace MyPOS
{
    public partial class Frm_Login : MetroForm
    {
        FrmTiempoAirePagoServicios frm = null;
        public Frm_Login()
        {
            InitializeComponent();
        }

        private void btn_login_Click(object sender, EventArgs e)
        {
            if(txt_usuario.Text.Length <1)
            {
                MetroMessageBox.Show(this, "Debes escribir la cuenta de usuario asignada por tu distribuidor autorizado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txt_usuario.Focus();
                return;
            }

            if (txt_contraseña.Text.Length < 1)
            {
                MetroMessageBox.Show(this, "Debes escribir la contraseña asignada por tu distribuidor autorizado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txt_usuario.Focus();
                return;
            }

            String Parametros = "";
            String Respuesta = "";
            XmlDocument Xml_Respuesta = new XmlDocument();
            XmlNode Nodo_Folio;
            XmlNode Nodo_Error;

            try
            {


                TiempoAire.ApplicationServices ws = new TiempoAire.ApplicationServices();
                /* usuario del demo 18/mzo/2016
                Parametros += "<Recarga>";
                Parametros += "<Usuario>demo123456</Usuario>";
                Parametros += "<Passwd>Demo123456</Passwd>";
                Parametros += "</Recarga>";
                */
                /*Mi usuario real funciona de perlas
                no consume saldo la consulta prodiamos consultar antes de recargas y
                o mandar un email de que se esta terminando el saldo
                */
                Parametros += "<Recarga>";
                Parametros += "<Usuario>" + txt_usuario.Text + "</Usuario>";
                Parametros += "<Passwd>" + txt_contraseña.Text  + "</Passwd>";
                Parametros += "</Recarga>";
                Respuesta = ws.ObtenSaldo(Parametros);

                // ws.ObtenSaldo

                Xml_Respuesta.LoadXml(Respuesta);
                Nodo_Error = Xml_Respuesta.SelectSingleNode("//Error");
                if (Nodo_Error != null)
                {
                    MetroMessageBox.Show(this, Nodo_Error.InnerText, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    Nodo_Folio = Xml_Respuesta.SelectSingleNode("//Saldo");
                    if (Nodo_Folio != null)
                    {
                        Program.usuario = txt_usuario.Text;
                        Program.contraseña = txt_contraseña.Text;

                        if(chec_recordar_datos.Checked)
                        {
                            Properties.Settings.Default["Tiempo_Aire_Usuario"] = txt_usuario.Text;
                            Properties.Settings.Default["Tiempo_Aire_Password"] = txt_contraseña.Text;
                            Properties.Settings.Default.Save();
                        }
                        else
                        {
                            Properties.Settings.Default["Tiempo_Aire_Usuario"] = "";
                            Properties.Settings.Default["Tiempo_Aire_Password"] = "";
                            Properties.Settings.Default.Save();
                        }

                        MetroMessageBox.Show(this, "Bienvenido al sistema de recargas. \n Su saldo actual es de: " + Nodo_Folio.InnerText, "Confirmación de Acceso al Sistema", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Hide();
                         frm= new FrmTiempoAirePagoServicios();
                        frm.ShowInTaskbar = true;
                        frm.ShowDialog();
                    }
                    else
                    {
                        MetroMessageBox.Show(this, "Error desconocido, no se recibió respuesta del servicio de recargas.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception Ex_Respuesta)
            {
                MessageBox.Show( Ex_Respuesta.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_cerrar_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Frm_Login_Load(object sender, EventArgs e)
        {
            txt_usuario.Text = Properties.Settings.Default.Tiempo_Aire_Usuario.ToString(); ;
            txt_contraseña.Text = Properties.Settings.Default.Tiempo_Aire_Password.ToString(); ;

            if(txt_usuario.Text.Length>0)
            {
                chec_recordar_datos.Checked = true;
            }
        }
    }
}

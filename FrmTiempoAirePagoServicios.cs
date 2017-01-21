using MetroFramework.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
//using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework.Controls;
using MetroFramework;
using System.Xml;
using Utilerias;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text.RegularExpressions;

namespace MyPOS
{
    public partial class FrmTiempoAirePagoServicios : MetroForm
    {
                private string Compañia;
       
       //         private string usuario_proveedor = "demo123456";
       //         private string contraseña_proveedor = "Demo123456";
        
/*
        private string usuario_proveedor = "rubernardo";
        private string contraseña_proveedor = "$Animal83";
//*/
        public FrmTiempoAirePagoServicios()
        {
            InitializeComponent();
        }

        private void llenarComboTipoDeposito()
        {
            cmb_tipo_compra.Items.Add(new MiListaCGato("Recargas", "0"));
            cmb_tipo_compra.Items.Add(new MiListaCGato("Pago de servicios", "1"));
        }

        private void FrmTiempoAirePagoServicios_Load(object sender, EventArgs e)
        {
            metroTabControl1.SelectedIndex = 0;
            Selecciona_Imagenes_Compañia("");
            //los valores del tipo de compra de saldo
            llenarComboTipoDeposito();
            this.WindowState = FormWindowState.Maximized;
        }

        private void Btn_Asigana_monto_Click(object sender, EventArgs e)
        {
            MetroButton btn = (MetroButton)sender;
            Txt_Recarga_Monto.Text = btn.Tag.ToString();
            Txt_Recarga_Numero_Celular.Focus();
        }

        
        

        private void Txt_Recarga_Numero_Celular_Click(object sender, EventArgs e)
        {
            if (Txt_Recarga_Numero_Celular.Text.Trim() == "")
            {
                Txt_Recarga_Numero_Celular.Icon = Properties.Resources.celular;
            }
        }

        private void Txt_Recarga_Confirmar_Numero_Telefono_Celular_Click(object sender, EventArgs e)
        {
            if (Txt_Recarga_Confirmar_Numero_Telefono_Celular.Text.Trim() == "")
            {
                Txt_Recarga_Confirmar_Numero_Telefono_Celular.Icon = Properties.Resources.celular;
            }
        }

        private void Txt_Recarga_Numero_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsNumber(e.KeyChar)) && (e.KeyChar != (char)Keys.Back))
            {
                e.Handled = true;
                return;
            }
        }

        private void Txt_Recarga_Confirmar_Numero_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsNumber(e.KeyChar)) && (e.KeyChar != (char)Keys.Back))
            {
                e.Handled = true;
                return;
            }
        }

        private void Txt_Recarga_Numero_Celular_Changed(object sender, EventArgs e)
        {
            if (Txt_Recarga_Numero_Celular.Text.Trim() == "")
            {
                Txt_Recarga_Numero_Celular.Icon = Properties.Resources.celular;
            }
            else
            {
                if (Txt_Recarga_Confirmar_Numero_Telefono_Celular.Text.Trim() != "")
                {
                    if (Txt_Recarga_Numero_Celular.Text.Trim() == Txt_Recarga_Confirmar_Numero_Telefono_Celular.Text.Trim())
                    {
                        Txt_Recarga_Numero_Celular.Icon = Properties.Resources.celular_ok;
                        Txt_Recarga_Confirmar_Numero_Telefono_Celular.Icon = Properties.Resources.celular_ok;
                    }
                    else
                    {
                        Txt_Recarga_Numero_Celular.Icon = Properties.Resources.celular_no;
                        Txt_Recarga_Confirmar_Numero_Telefono_Celular.Icon = Properties.Resources.celular_no;
                    }
                }
            }
        }

        private void Txt_Recarga_Confirmar_Numero_Telefono_Celular_Changed(object sender, EventArgs e)
        {
            if (Txt_Recarga_Confirmar_Numero_Telefono_Celular.Text.Trim() == "")
            {
                Txt_Recarga_Confirmar_Numero_Telefono_Celular.Icon = Properties.Resources.celular;
            }
            else
            {
                if (Txt_Recarga_Numero_Celular.Text.Trim() != "")
                {
                    if (Txt_Recarga_Numero_Celular.Text.Trim() == Txt_Recarga_Confirmar_Numero_Telefono_Celular.Text.Trim())
                    {
                        Txt_Recarga_Numero_Celular.Icon = Properties.Resources.celular_ok;
                        Txt_Recarga_Confirmar_Numero_Telefono_Celular.Icon = Properties.Resources.celular_ok;
                    }
                    else
                    {
                        Txt_Recarga_Numero_Celular.Icon = Properties.Resources.celular_no;
                        Txt_Recarga_Confirmar_Numero_Telefono_Celular.Icon = Properties.Resources.celular_no;
                    }
                }
            }
        }

        private void Lnk_Aceptar_Click(object sender, EventArgs e)
        {
            String Parametros = "";
            String Respuesta = "";
            XmlDocument Xml_Respuesta = new XmlDocument();
            XmlNode Nodo_Folio;
            XmlNode Nodo_Error;
            int Monto;

            try
            {
                if (Compañia == "")
                {
                    MetroMessageBox.Show(this, "Debe de seleccionar una compañia a recargar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                int.TryParse(Txt_Recarga_Monto.Text.Replace("$", "").Trim(), out Monto);
                if (Monto <= 0)
                {
                    MetroMessageBox.Show(this, "Debe de seleccionar un monto a recargar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (Txt_Recarga_Numero_Celular.Text.Trim() == "")
                {
                    MetroMessageBox.Show(this, "Debe de proporcionar el número de teléfono celular a recargar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Txt_Recarga_Numero_Celular.Focus();
                    return;
                }
                if (Txt_Recarga_Confirmar_Numero_Telefono_Celular.Text.Trim() == "")
                {
                    
                    MetroMessageBox.Show(this, "Debe de proporcionar la confirmación del número de teléfono celular a recargar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Txt_Recarga_Confirmar_Numero_Telefono_Celular.Focus();
                    return;
                }
                if (Txt_Recarga_Numero_Celular.Text.Trim() != Txt_Recarga_Confirmar_Numero_Telefono_Celular.Text.Trim())
                {
                    MetroMessageBox.Show(this, "Los números de celular no pueden ser diferentes, favor de verificarlos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                MyPOS.TiempoAire.ApplicationServices ws = new MyPOS.TiempoAire.ApplicationServices();

                Parametros += "<Recarga>";
                Parametros += "<Usuario>"+Program.usuario+"</Usuario>";
                Parametros += "<Passwd>"+ Program.contraseña+ "</Passwd>";
                Parametros += "<Telefono>" + Txt_Recarga_Numero_Celular.Text.Trim() + "</Telefono>";
                Parametros += "<Carrier>" + Compañia + "</Carrier>";
                Parametros += "<Monto>" + Txt_Recarga_Monto.Text.Replace("$", "").Trim() + "</Monto>";
                Parametros += "</Recarga>";

                Respuesta = ws.RecargaEWS(Parametros);

               

                Xml_Respuesta.LoadXml(Respuesta);
                Nodo_Error = Xml_Respuesta.SelectSingleNode("//Error");
                if (Nodo_Error != null)
                {
                    MetroMessageBox.Show(this, Nodo_Error.InnerText, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    Nodo_Folio = Xml_Respuesta.SelectSingleNode("//Folio");
                    if (Nodo_Folio != null)
                    {
                        
                        MetroMessageBox.Show(this, "Recarga realizada exitosamente, su Folio de Recarga es: " + Nodo_Folio.InnerText, "Confirmación de Recarga", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        if(togl_imprimir_ticket.Checked)
                        {
                            Impresion.Impresor_Tickets Impresor_Ticket = new Impresion.Impresor_Tickets("Courier New", "*", Impresion.Impresor_Tickets.Impresor_Ticket_Tamaño_Papel.CHICO);
                            // EL ENCABEZADO EL NOMBRE D ELA EMPRESA
                            /*
                            Impresor_Ticket.AddHeaderLine(Program.datos_empres.Nombre);
                            Impresor_Ticket.AddHeaderLine(Program.datos_empres.RFC);
                            Impresor_Ticket.AddHeaderLine(Program.datos_empres.Calle + " C.P. " + Program.datos_empres.Codigo_Postal);
                            Impresor_Ticket.AddHeaderLine(Program.datos_empres.Ciudad + ", " + Program.datos_empres.Estado);
                            Impresor_Ticket.AddHeaderLine(Program.datos_empres.Telefono);
                            */
                            // Impresor_Ticket.AddSubHeaderLine(/*"Caja #" + Program.Datos_Caja_Activa.Caja_ID +*/ " - Ticket #" + no_venta);
                            //Impresor_Ticket.AddSubHeaderLine("Cajero:" + Program.usuario_activo.Nombre);
                            //Impresor_Ticket.AddSubHeaderLine("*****************************");
                            //Impresor_Ticket.AddSubHeaderLine("Cliente:" + cmb_Cliente.Text );
                            //Impresor_Ticket.AddSubHeaderLine("*****************************");
                            Impresor_Ticket.AddHeaderLine("Comprobante de recarga");
                            Impresor_Ticket.Imprimir_Detalles = false;
                            Impresor_Ticket.AddSubHeaderLine("Fecha:" + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString());
                            /*
                            //los productos
                            foreach (DAO_detalles_venta f in lista_detalles_venta)
                            {
                                Impresor_Ticket.AddItem(
                                                        f.Cantidad.ToString(),
                                                         "",
                                                         f.Nombre_producto, String.Format("{0:0.00}", Math.Round(f.Precio, 2)),
                                                        "",

                                                        String.Format("{0:0.00}", Math.Round(f.Subtotal, 2))
                                                   );
                            }
                            */
                            /*
                            Impresor_Ticket.AddFooterLine(new FooterLine("Este ticket no es un comprobante fiscal", true));
                            if (Program.datos_empres.Saludo_Ticket.Length <= 0 || Program.datos_empres.Saludo_Ticket == null)
                                Impresor_Ticket.AddFooterLine(new FooterLine("GRACIAS POR TU VISITA", false));
                            else
                                Impresor_Ticket.AddFooterLine(new FooterLine(Program.datos_empres.Saludo_Ticket, false));
                            */
                            //totales
                            Impresor_Ticket.AddFooterLine(new Impresion.FooterLine( "Compañia:" + Compañia,false));
                            Impresor_Ticket.AddTotal("Folio autor", String.Format("{0:000000}", AyudanteCGato.convierteADoble(Nodo_Folio.InnerText)));
                            Impresor_Ticket.AddTotal("Monto recargado", String.Format("{0:0.00}", AyudanteCGato.convierteADoble(Txt_Recarga_Monto.Text)));
                            /*
                            Impresor_Ticket.AddTotal("RECIBIDO",
                                   String.Format("{0:0.00}", Math.Round(AyudanteCGato.convierteADoble(txt_efectivo.Text), 2)));

                            Impresor_Ticket.AddTotal("CAMBIO", String.Format("{0:0.00}", Math.Round(AyudanteCGato.convierteADoble(txt_cambio.Text), 2)));
                            */
                            Impresor_Ticket.AddTotal("", "");//Ponemos un total en blanco que sirve de espacio




                            //El metodo AddFooterLine funciona igual que la cabecera

                            //Y por ultimo llamamos al metodo PrintTicket para imprimir el ticket, este metodo necesita un
                            //parametro de tipo string que debe de ser el nombre de la impresora.
                            string la_impresora = ""; //lo jala de la configuracion grlobal

                            System.Drawing.Printing.PrinterSettings prop = new System.Drawing.Printing.PrinterSettings();
                            la_impresora=prop.PrinterName;
                            if (prop.IsValid)
                                Impresor_Ticket.PrintTicket(la_impresora);
                            else
                            {
                                DialogResult respuesta = MessageBox.Show("No se podra imprimir el ticket debido a que la impresora no es válida \n Clic en Si para asignar otra impresora de una lista", "Aviso", MessageBoxButtons.YesNo);
                                if (respuesta == DialogResult.Yes)
                                {
                                    System.Collections.ArrayList lista_impresoras = new System.Collections.ArrayList();
                                    foreach (string printer in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
                                    {

                                        lista_impresoras.Add(printer);

                                    }
                                    InputBoxResult iResult = InputBox.ShowConComboBox("Seleccione la impresora que desee utilizar", lista_impresoras);

                                }
                            }
                        }
                        //al final li,piamos todos los controles
                        Txt_Recarga_Monto.Text = "$0";
                        Txt_Recarga_Numero_Celular.Text = "";
                        Txt_Recarga_Confirmar_Numero_Telefono_Celular.Text = "";
                        Selecciona_Imagenes_Compañia("");
                        MostrarPanelMontos(""); //vacio para que desaparezcan los montos disponibles
                        Compañia = "";
                    }
                    else
                    {
                        MetroMessageBox.Show(this, "Error desconocido, no se recibio respuesta del servicio de recargas.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception Ex_Respuesta)
            {
                MetroMessageBox.Show(this, Ex_Respuesta.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Lnk_Salir_Click(object sender, EventArgs e)
        {
            //DialogResult resultado = MetroMessageBox.Show(this, "Seguro que deseas salir?");
            //if (resultado ==DialogResult.Yes)
            //{
            //    this.Close();
            //    //Application.Exit();
            //}

            Selecciona_Imagenes_Compañia("");
            MostrarPanelMontos("");
            Txt_Recarga_Monto.Text = "";
            Txt_Recarga_Numero_Celular.Text = "";
            Txt_Recarga_Confirmar_Numero_Telefono_Celular.Text = "";
            togl_imprimir_ticket.Checked = false;
        }

        private void MTL_Telcel_Click(object sender, EventArgs e)
        {
            Compañia = "Telcel";
            Selecciona_Imagenes_Compañia(Compañia);
            MostrarPanelMontos(Compañia);
        }

        private void MTL_Movistar_Click(object sender, EventArgs e)
        {
            Compañia = "Movistar";
            Selecciona_Imagenes_Compañia(Compañia);
            MostrarPanelMontos(Compañia);
        }

        private void MTL_Iusacell_Click(object sender, EventArgs e)
        {
            Compañia = "Iusacell";
            Selecciona_Imagenes_Compañia(Compañia);
            MostrarPanelMontos(Compañia);
            MostrarPanelMontos(Compañia);
        }

        private void MTL_Unefon_Click(object sender, EventArgs e)
        {
            Compañia = "Unefon";
            Selecciona_Imagenes_Compañia(Compañia);
            MostrarPanelMontos(Compañia);
        }

        private void MTL_Nextel_Click(object sender, EventArgs e)
        {
            Compañia = "Nextel";
            Selecciona_Imagenes_Compañia(Compañia);
            MostrarPanelMontos(Compañia);
        }

        private void Selecciona_Imagenes_Compañia(string Compañia)
        {
            
            MTL_Telcel.TileImage = Properties.Resources.telcel_bn;
            MTL_Movistar.TileImage = Properties.Resources.movistar_bn;
            MTL_Iusacell.TileImage = Properties.Resources.Iusacell_bn;
            MTL_Unefon.TileImage = Properties.Resources.unefon_bn;
            MTL_Nextel.TileImage = Properties.Resources.nextel_bn;

            if (Compañia == "Telcel")
            {
                MTL_Telcel.TileImage = Properties.Resources.telcel;
            } else if (Compañia == "Movistar") {
                MTL_Movistar.TileImage = Properties.Resources.movistar;
            } else if (Compañia == "Iusacell") {
                MTL_Iusacell.TileImage = Properties.Resources.Iusacell;
            } else if (Compañia == "Unefon") {
                MTL_Unefon.TileImage = Properties.Resources.unefon;
            } else if (Compañia == "Nextel") {
                MTL_Nextel.TileImage = Properties.Resources.nextel;
            }

            this.Refresh();
        }

        /// <summary>
        /// Muestra el panel de montos o los oculta sino coincide con la compaía
        /// </summary>
        /// <param name="Compañia"></param>
        private void MostrarPanelMontos(string Compañia)
        {

            switch (Compañia)
            {
                case "Telcel":
                case "Unefon":
                case "Iusacell":
                    {
                        panel_montos_telcel_iusacell_unefon.Visible = true;
                        panel_montos_movistar.Visible = false;
                        panel_montos_nextel.Visible = false;
                    }
                    break;
                case "Movistar":
                    {
                        panel_montos_telcel_iusacell_unefon.Visible = false;
                        panel_montos_movistar.Visible = true;
                        panel_montos_nextel.Visible = false;
                    }
                    break;
                case "Nextel":
                    {
                        panel_montos_nextel.Visible = true;
                        panel_montos_telcel_iusacell_unefon.Visible = false;
                        panel_montos_movistar.Visible = false;
                        
                    }
                    break;
                default:
                    {
                        panel_montos_nextel.Visible = false;
                        panel_montos_telcel_iusacell_unefon.Visible = false;
                        panel_montos_movistar.Visible = false;
                    }
                    break;
            }
            txt_monto_deposito.Text = "";

            this.Refresh();
        }

        private void link_consulta_saldo_Click(object sender, EventArgs e)
        {
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
                Parametros += "<Usuario>"+Program.usuario+ "</Usuario>";
                Parametros += "<Passwd>"+Program.contraseña+"</Passwd>";
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
                        Txt_Recarga_Monto.Text = "$0";
                       
                        //MetroMessageBox.Show(this, "Recarga realizada exitosamente, su Folio de Recarga es: " + Nodo_Folio.InnerText, "Confirmación de Recarga", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txt_saldo_actual.Text = Nodo_Folio.InnerText;
                    }
                    else
                    {
                        MetroMessageBox.Show(this, "Error desconocido, no se recibió respuesta del servicio de recargas.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception Ex_Respuesta)
            {
                MetroMessageBox.Show(this, Ex_Respuesta.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        public void stringToLista(string resultado)
        {

            string[] words = resultado.Split(',');

            foreach (string i in words)
            {
                if (i != "")
                {
                    MiListaCGato item = new MiListaCGato();
                    string[] id = i.Split('|');
                    string banco = id[0].Trim();
                    item.ItemData = id[1].Trim();
                    item.Name = id[0].Trim() + " ["+id[2].Trim()+"]";
                    if (cmb_banco.Items.Contains(banco) == false)
                    {
                        cmb_banco.Items.Add(item);
                    }
                  
                }

            }
            cmb_banco.SelectedIndex = 0;
           

        }
        private void link_consulta_cuentas_Click(object sender, EventArgs e)
        {
            
        }

        private void btn_selecciona_ficha_deposito_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog1.InitialDirectory = "C:/";
                OpenFileDialog1.Filter = "JPG|*.jpg;*.jpeg|BMP|*.bmp|GIF|*.gif|PNG|*.png|TIFF|*.tif;*.tiff|"
       + "All Graphics Types|*.bmp;*.jpg;*.jpeg;*.png;*.tif;*.tiff";
                OpenFileDialog1.FilterIndex = 1;

                
                if (OpenFileDialog1.ShowDialog() == DialogResult.OK)
                {
                   

                }
                else
                {
                    MetroMessageBox.Show(this,"No se seleccionó una ficha de depósito válida");
                }
            }
            catch (Exception ex)
            {

                MetroMessageBox.Show(this,"Error al seleccionar la ficha \n"+ ex.Message);
            }
        }

        public static string convertfile_to_base64(string file)
        {
            Byte[] bytes = File.ReadAllBytes(file);
            // Convert byte[] to Base64 String
            string base64String = Convert.ToBase64String(bytes);
            return base64String;


        }

        private void btn_reportar_deposito_Click(object sender, EventArgs e)
        {
            try
            {
                MiListaCGato ml_cuenta=null, ml_tipo_depo=null, ml_rfc=null;

                if(cmb_banco.SelectedIndex <0)
                {
                    MetroMessageBox.Show(this, "Primero debe seleccionar el banco/cuenta a la que depósito","Validación",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    cmb_banco.Focus();
                    return;
                }
                else
                {
                    ml_cuenta = (MiListaCGato)cmb_banco.Items[cmb_banco.SelectedIndex];
                }

                if (cmb_tipo_compra.SelectedIndex < 0)
                {
                    MetroMessageBox.Show(this, "Primero debe seleccionar el tipo de compra que realizó", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cmb_tipo_compra.Focus();
                    return;
                }
                else
                {
                    ml_tipo_depo = (MiListaCGato)cmb_tipo_compra.Items[cmb_tipo_compra.SelectedIndex];
                }

                if (AyudanteCGato.convierteADoble(txt_monto_deposito.Text)<=0)
                {
                    MetroMessageBox.Show(this, "Debe escribir el monto que depositó", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txt_monto_deposito.Text = "";
                    txt_monto_deposito.Focus();
                    return;
                }

                if (txt_folio_autorizacion.Text.Length <= 0)
                {
                    MetroMessageBox.Show(this, "Debe escribir el folio de autorización que viene en su ficha/voucher del depósito", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txt_folio_autorizacion.Text = "";
                    txt_folio_autorizacion.Focus();
                    return;
                }

                if(togl_facturar.Checked)
                {
                    if(cmb_rfcs.SelectedIndex>-1)
                    {
                        ml_rfc = (MiListaCGato)cmb_rfcs.Items[cmb_rfcs.SelectedIndex];
                    }
                    else
                    {
                        MetroMessageBox.Show(this, "Primero debe seleccionar el RFC al cual desea se emita la factura", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        cmb_rfcs.Focus();
                        return;
                    }
                }

                if(!AyudanteIO.ExisteArchivo(OpenFileDialog1.FileName))
                {
                    MetroMessageBox.Show(this, "Primero debe seleccionar la ficha escaneada del depósito", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cmb_rfcs.Focus();
                    return;
                }
                
                //lo pasamo a string base64 

                if(OpenFileDialog1.FileName.Length >0 )
                {
                    string strbase64 = convertfile_to_base64(OpenFileDialog1.FileName);
                    string extension = System.IO.Path.GetExtension(OpenFileDialog1.FileName).Substring(1); //sin el punto;
                    string nombre_archivo = System.IO.Path.GetFileName(OpenFileDialog1.FileName);
                    string arrFilename = nombre_archivo + "." + extension;
                    string RFC = "";
                    if(ml_rfc !=null)
                    {
                        RFC = ml_rfc.ItemData;
                    }

                    //fromamos el xml para subir la ficha
                    string info = "<Deposito><Usuario>" + Program.usuario + "</Usuario ><Passwd>" + Program.contraseña + "</Passwd><Banco>" + ml_cuenta.ItemData + "</Banco><Folio>" + txt_folio_autorizacion.Text + "</Folio><Fecha>"+ dtp_fecha_deposito.Value.ToString("yyyy-MM-dd") + "</Fecha><Tipo>" + ml_tipo_depo.ItemData + "</Tipo><Cantidad>" + txt_monto_deposito.Text + "</Cantidad>" +
                    "<Hora>" + nu_hora_deposito.Value + "</Hora><Minuto>" + nu_minuto_deposito.Value + "</Minuto>";
                    info += "<RFC>"+RFC+"</RFC>";
                    info += "<FichaNombre>" + nombre_archivo + "</FichaNombre><FichaData>" + 
                        strbase64 +
                        "</FichaData><FichaMIME>image/" + extension + "</FichaMIME></Deposito>";

                    //intentamos enviar el desposito
                    com.misaldotelcel.ApplicationServices ws = new com.misaldotelcel.ApplicationServices();


                    string resultado = "";

                    try //exitosamente
                    {
                        resultado = ws.ReportarDep(info);
                        if(resultado.IndexOf("exitosamente")>-1)
                        {
                            MetroMessageBox.Show(this, resultado, "Depósito enviado satisfactoriamente", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            //li,piamos los controles
                            cmb_banco.SelectedIndex = -1;
                            cmb_tipo_compra.SelectedIndex = -1;
                            cmb_rfcs.SelectedIndex = -1;
                            txt_monto_deposito.Text = "";
                            txt_folio_autorizacion.Text = "";
                            togl_facturar.Checked = false;
                            OpenFileDialog1.FileName = ""; 
                        }
                        else //quiza esta duplicado
                        {
                            MetroMessageBox.Show(this, resultado, "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    catch (Exception ex)
                    {

                        MetroMessageBox.Show(this, "No se pudo reportar el depósito, intente más tarde." +
                            "Descripción del problema:"+ ex.Message
                            , "Error", MessageBoxButtons.OK, MessageBoxIcon.Information); ;
                    }
                    


                }
                else
                {
                    MetroMessageBox.Show(this, "Debe seleccionar su ficha/voucher del depósito", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    btn_selecciona_ficha_deposito.Focus();
                }
                

            }
            catch (Exception ex)
            {

                MetroMessageBox.Show(this, "Error al reportar el pago \n" + ex.Message+"\n Intente más tarde") ; ;
            }
        }

        private void listarCuentasBancos()
        {

        }

        private void btn_consultar_cuentas_bancarias_Click(object sender, EventArgs e)
        {

            try
            {


                com.misaldotelcel.ApplicationServices ws = new com.misaldotelcel.ApplicationServices();
                string Parametros = "";
                Parametros += "<Recarga>";
                Parametros += "<Usuario>"+Program.usuario+ "</Usuario>";
                Parametros += "<Passwd>"+Program.contraseña+"</Passwd>";
                Parametros += "</Recarga>";
                string resultado = ws.ObtenBanco(Parametros);

                

                if (resultado == "Error en mostrar bancos/cuentas : bancos/cuentas no disponibles")
                {
                    cmb_banco.Enabled = false;
                    //Cuentas.IsEnabled = false;
                }
                else
                {
                    cmb_banco.Items.Clear();
                    stringToLista(resultado);
                    cmb_banco.SelectedIndex = 0;
                    //Cuentas.SelectedIndex = 0;
                }

            }
            catch (Exception Ex_Respuesta)
            {
                MetroMessageBox.Show(this, Ex_Respuesta.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Txt_Recarga_Monto_KeyPress(object sender, KeyPressEventArgs e)
        {
            AyudanteCGato.solo_Numeros(sender, e, true);
        }

        private void txt_monto_deposito_KeyPress(object sender, KeyPressEventArgs e)
        {
            AyudanteCGato.solo_Numeros(sender, e, true);
        }

        private void togl_facturar_CheckedChanged(object sender, EventArgs e)
        {
            if(togl_facturar.Checked)
            {
                try
                {


                    com.misaldotelcel.ApplicationServices ws = new com.misaldotelcel.ApplicationServices();
                    string Parametros = "";
                    String Respuesta = "";
                    XmlDocument Xml_Respuesta = new XmlDocument();
                    XmlNode Nodo_Folio;
                    XmlNodeList nodos_rfc = null;
                    XmlNode Nodo_Error;

                    Parametros += "<Recarga>";
                    Parametros += "<Usuario>" + Program.usuario + "</Usuario>";
                    Parametros += "<Passwd>" + Program.contraseña + "</Passwd>";
                    Parametros += "</Recarga>";
                    string resultado = ws.ConsultaRFC(Parametros);
                    //20abr2016--funciona pero vamos aver si de una vez cargamos los RFCs en el combo
                    /*

                                        if (resultado.IndexOf("No tiene RFCS")>-1)
                                        {
                                            pan_rfcs.Visible = false;
                                            MetroMessageBox.Show(this, "No tiene ningun RFC registrado para solocitar facturas");
                                        }
                                        else
                                        {
                                            pan_rfcs.Visible = true;                        
                                        }
                    */
                    Xml_Respuesta.LoadXml(resultado);
                    Nodo_Error = Xml_Respuesta.SelectSingleNode("//Error");
                    if (Nodo_Error != null)
                    {
                        MetroMessageBox.Show(this, Nodo_Error.InnerText, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        nodos_rfc = Xml_Respuesta.SelectNodes("//Rfc");
                        if (nodos_rfc != null)
                        {
                           foreach (XmlNode nodo in  nodos_rfc)
                            {
                                string data = nodo.InnerText; //0-No tiene RFCS
                                if(data.IndexOf("No tiene RFCS")>-1)
                                {
                                    cmb_rfcs.Items.Clear();
                                    pan_rfcs.Visible = false;
                                    pan_rfcs.Hide();
                                    togl_facturar.Checked = false;
                                    MetroMessageBox.Show(this, "No tiene ningun RFC registrado para solocitar facturas. \nContacte a su distribuidor si desea que le asigne RFCs");
                                    break;
                                }
                                else
                                {
                                    string[] datos = data.Split('|');
                                    string id = datos[0].Substring(0, datos[0].IndexOf("-"));
                                    string info = datos[0].Substring(datos[0].IndexOf("-") + 1);

                                    if (cmb_banco.Items.Contains(id) == false)  //ejemplo:998-GAHR830622DX4
                                    {
                                        cmb_rfcs.Items.Add(new MiListaCGato(info, id));
                                    }
                                    pan_rfcs.Visible = true;
                                }
                                
                            }
                            
                        }
                        else
                        {
                            pan_rfcs.Visible = false;
                            MetroMessageBox.Show(this, "No se pudieron cargar los RFCs disponibles.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }

                }
                catch (Exception Ex_Respuesta)
                {
                    MetroMessageBox.Show(this, Ex_Respuesta.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            else
            {
                pan_rfcs.Visible = false;
                cmb_rfcs.Items.Clear();
            }
        }

        private void metroPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void metroTabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(metroTabControl1.SelectedIndex==1)
            {
                btn_consultar_cuentas_bancarias_Click(null, null);
            }
        }

        private void metroLabel18_Click(object sender, EventArgs e)
        {

        }

        private void FrmTiempoAirePagoServicios_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void togl_imprimir_ticket_CheckedChanged(object sender, EventArgs e)
        {
            //this.ShowInTaskbar = true;
        }
    }
}

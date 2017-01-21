//
using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Collections;

namespace Impresion
{

    /*
        forma de usarla  http://www.foromsn.com/Version_Imprimible.php?Id=178209   
     * Ticket ticket = new Ticket();

        ticket.HeaderImage = "C:\imagen.jpg"; //esta propiedad no es obligatoria

        ticket.AddHeaderLine("STARBUCKS COFFEE TAMAULIPAS" );
        ticket.AddHeaderLine("EXPEDIDO EN:" );
        ticket.AddHeaderLine("AV. TAMAULIPAS NO. 5 LOC. 101" );
        ticket.AddHeaderLine("MEXICO, DISTRITO FEDERAL" );
        ticket.AddHeaderLine("RFC: CSI-020226-MV4" );

        //El metodo AddSubHeaderLine es lo mismo al de AddHeaderLine con la diferencia
        //de que al final de cada linea agrega una linea punteada "=========="
        ticket.AddSubHeaderLine("Caja # 1 - Ticket # 1" );
        ticket.AddSubHeaderLine("Le atendió: Prueba" );
        ticket.AddSubHeaderLine(DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString());

        //El metodo AddItem requeire 3 parametros, el primero es cantidad, el segundo es la descripcion
        //del producto y el tercero es el precio
        ticket.AddItem("1", "Articulo Prueba", "15.00" );
        ticket.AddItem("2", "Articulo Prueba", "25.00" );

        //El metodo AddTotal requiere 2 parametros, la descripcion del total, y el precio
        ticket.AddTotal("SUBTOTAL", "29.75" );
        ticket.AddTotal("IVA", "5.25" );
        ticket.AddTotal("TOTAL", "35.00" );
        ticket.AddTotal("", "" ); //Ponemos un total en blanco que sirve de espacio
        ticket.AddTotal("RECIBIDO", "50.00" );
        ticket.AddTotal("CAMBIO", "15.00" );
        ticket.AddTotal("", "" );/Ponemos un total en blanco que sirve de espacio
        ticket.AddTotal("USTED AHORRO", "0.00" );

        //El metodo AddFooterLine funciona igual que la cabecera
        ticket.AddFooterLine("EL CAFE ES NUESTRA PASION..." );
        ticket.AddFooterLine("VIVE LA EXPERIENCIA EN STARBUCKS" );
        ticket.AddFooterLine("GRACIAS POR TU VISITA" ); 
      */
     public class FooterLine
     {
     	public string line="";
	    public bool bold=false;
	     	
	     public FooterLine(string pline, bool pbold)
	     {
	     	line=pline;
	     	bold=pbold;
	     }
     }
     
    public class Impresor_Tickets
    {
        ArrayList headerLines = new ArrayList();
        ArrayList subHeaderLines = new ArrayList();
        ArrayList items = new ArrayList();
        ArrayList totales = new ArrayList();
        ArrayList footerLines = new ArrayList();
        ArrayList notasLines = new ArrayList();
        private Image _Imagen_Encabezado = null;

        int _Cant_Lineas = 0; //lineas impresas

        int _Longitud_MaximRenglon = 32; 
        //35  para impresoras grandes
        //22 para la chikas Ec 58mm
        int _Longitud_Maxima_Descripcion = 17;//20; //TAmaño maximo de la decripcion del articulo

        int _Alto_Imagen = 0;

        float _Margen_Izquierdo = 0;
        float _Margen_Superior = 3; //Margen Superior

        string _Nombre_Fuente = "Lucida Console";//Lucida Console version previa 28DIc09
        int _Tamaño_Fuente = 9;//9

        private string _Caracter_Doted_Line = "=";

        public string Caracter_Doted_Line
        {
            get { return _Caracter_Doted_Line; }
            set { _Caracter_Doted_Line = value; }
        }
        
        private string _Formato_Items="";
        /// <summary>
        /// Indica el titulo que se mostrara en el ticket para
        /// describir las columnas vistas 
        /// </summary>
        private string Formato_Items
        {
			get { return _Formato_Items; }
			set { _Formato_Items = value; }
		}
		
        private  bool _Mostrar_Precio=true;
        /// <summary>
        /// propiedad para indicar el impresor si el precio se va imprimir
        /// </summary>
		public bool Mostrar_Precio {
			get { return _Mostrar_Precio; }
			set { _Mostrar_Precio = value; }
		}
        
        private bool _Mostrar_Subtotal=true;
        /// <summary>
        /// propiedad para indicar el impresor si el Subtotal se va imprimir
        /// </summary>
		public bool Mostrar_Subtotal {
			get { return _Mostrar_Subtotal; }
			set { _Mostrar_Subtotal = value; }
		}

        private bool _Cortar_Unimed = true;

        public bool Cortar_Unimed
        {
            get { return _Cortar_Unimed; }
            set { _Cortar_Unimed = value; }
        }

        private bool _Imprimir_Detalles = true;
        /// <summary>
        /// Indica si s emuestran detalle sen el ticket o no
        /// </summary>
        public bool Imprimir_Detalles
        {
            get { return _Imprimir_Detalles; }
            set { _Imprimir_Detalles = value; }
        }

        Font _Fuente_Usada = null;
        SolidBrush _Brocha = new SolidBrush(Color.Black);

        Graphics _Objeto_Dibujo = null;
        int LineasPorOja = 91;//87; //91		
        string _Renglon = null;
        //para comprobar en donde vamos
        bool _Impreso_Encabezado = false;
        bool _Impreso_Detalles=false;
        bool _Impreso_Totales=false;
		bool _Impreso_Pie_Pagina=false;
        //17jul2012----evitar ke se reimprima la imagen
        bool _Impreso_Imagen = false;

        int _Detalle_Activo = 0;
        int _Lineas_Corridas = 0;
        PrintDocument PrnDocument = new PrintDocument();

        /// <summary>
        /// Estructuraq ue nos idica el tañamo del papel del ticket
        /// </summary>
        public enum Impresor_Ticket_Tamaño_Papel
        {
            GRANDE=1,
            CHICO=2
        }

        private Impresor_Ticket_Tamaño_Papel _tamañoPapel;

        public Impresion.Impresor_Tickets.Impresor_Ticket_Tamaño_Papel TamañoPapel
        {
            get { return _tamañoPapel; }
            set { _tamañoPapel = value; }
        }

        private string _Encabezado_Lista_Productos = "";
        /// <summary>
        /// Establece el encabezado CANT UNIMED  PU SUBT
        /// </summary>
        public string Encabezado_Lista_Productos
        {
            get { return _Encabezado_Lista_Productos; }
            set { _Encabezado_Lista_Productos = value; }
        }
        public Impresor_Tickets(String Nombre_Fuente, 
            string Caracter_Linea,
            Impresor_Ticket_Tamaño_Papel tamaño_papel
            )
        {

            _Nombre_Fuente = Nombre_Fuente;
            Caracter_Doted_Line = Caracter_Linea;
            this.TamañoPapel = tamaño_papel;

            switch (TamañoPapel)
            {
                case Impresor_Ticket_Tamaño_Papel.GRANDE:
                    {
                        Longitud_MaximRenglon = 32;
                        Encabezado_Lista_Productos = "CANT    UMED     PU         SUBT";
                        this._Formato_Items = "{0,3} {1,-3} {2,6}  {3,5} {4,9}";
                    }
                    break;
                case Impresor_Ticket_Tamaño_Papel.CHICO :
                    {
                        Longitud_MaximRenglon = 22;
                        Encabezado_Lista_Productos = "CANT  UMED   PU   SUBT";
                        this._Formato_Items = "{0,3} {1,2} {2,6} {4,7}";
                    }
                    break;
            }
        }
        /// <summary>
        /// El logo del ticket
        /// </summary>
        public Image Imagen_Encabezado
        {
            get { return _Imagen_Encabezado; }
            set { if (_Imagen_Encabezado != value) _Imagen_Encabezado = value; }
        }
        /// <summary>
        /// Cantidad total de caracteres a imprimir
        /// </summary>
        public int Longitud_MaximRenglon
        {
            get { return _Longitud_MaximRenglon; }
            set { if (value != _Longitud_MaximRenglon) _Longitud_MaximRenglon = value; }
        }
        /// <summary>
        /// Tamaño maximo de los caracteres de la Descripcion del articulo
        /// </summary>
        public int Longitud_Maxima_Descripcion
        {
            get { return _Longitud_Maxima_Descripcion; }
            set { if (value != _Longitud_Maxima_Descripcion) _Longitud_Maxima_Descripcion = value; }
        }
        /// <summary>
        /// Tamaño de la fuente que que se imprime el ticket
        /// </summary>
        public int Tamaño_Fuente
        {
            get { return _Tamaño_Fuente; }
            set { if (value != _Tamaño_Fuente) _Tamaño_Fuente = value; }
        }
        /// <summary>
        /// Nombre de la fuente con que s eimprime el ticket
        /// </summary>
        public string Nombre_Fuente
        {
            get { return _Nombre_Fuente; }
            set { if (value != _Nombre_Fuente) _Nombre_Fuente = value; }
        }

        public void AddHeaderLine(string line)
        {
            headerLines.Add(line);
        }

        public void AddNotesLine(string note)
        {
            notasLines.Add(note);
        }

        public void AddSubHeaderLine(string line)
        {
            subHeaderLines.Add(line);
        }
        /// <summary>
        /// Agrega un renglon a la seccion de articulosde la venta
        /// </summary>
        /// <param name="cantidad"></param>
        /// <param name="item"></param>
        /// <param name="price"></param>
        public void AddItem(string cantidad,string unimed, string item, string price, string priceXcaja, string subtotal )
        {
            ListItem newItem = new ListItem('?');
            items.Add(newItem.GenerateItem(cantidad, unimed, item, price, priceXcaja,subtotal));
        }

        public void AddTotal(string name,string price)
        {
            OrderTotal newTotal = new OrderTotal('?');
            totales.Add(newTotal.GenerateTotal(name, price));
        }

        public void AddFooterLine(FooterLine line)
        {
            footerLines.Add(line);
        }   
        /// <summary>
        /// Alinea el texto a la derecha
        /// </summary>
        /// <param name="lenght">Cantidad de espacios disponibles</param>
        /// <returns></returns>
        private string AlignRightText(int lenght)
        {
            string espacios = "";
            int spaces = _Longitud_MaximRenglon - lenght;
            for (int x = 0; x < spaces; x++)
                espacios += " ";
            return espacios;
        }

        /// <summary>
        /// Alinea el texto a la derecha
        /// </summary>
        /// <param name="lenght">Cantidad de espacios disponibles</param>
        /// <returns></returns>
        private string AlignCenterText(int lenght)
        {
            string espacios = "";
            int spaces = (int)(_Longitud_MaximRenglon - lenght)/2;
            for (int x = 0; x < spaces; x++)
                espacios += " ";
            return espacios;
        }

        private string DottedLine()
        {
            string dotted = "";
            for (int x = 0; x < _Longitud_MaximRenglon; x++)
                dotted += Caracter_Doted_Line ; //se  elige que carater s eusa como doted line
            return dotted;
        }
        /// <summary>
        /// Reviza si una impresora existe instalada en la PC utilizada
        /// </summary>
        /// <param name="impresora"></param>
        /// <returns></returns>
        public bool PrinterExists(string impresora)
        {
            foreach (String strPrinter in PrinterSettings.InstalledPrinters)
            {
                if (impresora == strPrinter)
                    return true;
            }
            return false;
        }
        /// <summary>
        /// Imprime el ticket en la Impresora Indicada
        /// </summary>
        /// <param name="impresora"></param>
        public void PrintTicket(string impresora)
        {
            _Fuente_Usada = new Font(_Nombre_Fuente, _Tamaño_Fuente, FontStyle.Regular);
            
            PrnDocument.PrinterSettings.PrinterName = impresora;
            
            PrnDocument.PrintPage += new PrintPageEventHandler(pr_PrintPage);
            PrnDocument.Print();
        }

        private void pr_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            
            e.Graphics.PageUnit = GraphicsUnit.Millimeter;
            
            _Objeto_Dibujo = e.Graphics;

            if (_Impreso_Imagen == false)
            {
                DrawImage();
            }
            else
            {
                //_Lineas_Corridas = 0;
                _Alto_Imagen = 0;
            }

            //metemos todo el codigo en el evento para 
            //ver si cambiamos de pagina
            if (_Impreso_Encabezado == false)
            {
                DibujarEncabezado();
                DrawSubHeader();
               
                //this.Titulos_Items="CANT    UMED     PU  CAJAS  SUBT";
                //12jun2012 no ocupa cant en cajas
                if (this.Imprimir_Detalles)
                {
                    _Objeto_Dibujo.DrawString(this.Encabezado_Lista_Productos, _Fuente_Usada, _Brocha, _Margen_Izquierdo,
                                              YPosition(), new StringFormat());
                    _Cant_Lineas++;
                    _Lineas_Corridas++;
                }
                _Impreso_Encabezado = true; 
            }
            //los detalles aki mismo para poner otra hoja
            #region DETALLES
            if (this.Imprimir_Detalles)
            {
                if (_Impreso_Detalles == false)
                {
                    ListItem ordIt = new ListItem('?');


                    DrawEspacio();

                    for (int i = _Detalle_Activo; i < items.Count; i++)
                    {
                        bool nueva_pagina = false;
                        //Descripcion
                        string item = items[i].ToString();
                        string name = ordIt.GetItemName(item);
                        _Margen_Izquierdo = 0;
                        if (name.Length > _Longitud_MaximRenglon)
                        {
                            int currentChar = 0;
                            int itemLenght = name.Length;

                            while (itemLenght > _Longitud_MaximRenglon)
                            {
                                _Renglon = ordIt.GetItemName(item);
                                _Objeto_Dibujo.DrawString(_Renglon.Substring(currentChar, _Longitud_MaximRenglon),
                                                          _Fuente_Usada, _Brocha, _Margen_Izquierdo, YPosition(),
                                                          new StringFormat());

                                _Cant_Lineas++;
                                _Lineas_Corridas++;
                                currentChar += _Longitud_MaximRenglon;
                                itemLenght -= _Longitud_MaximRenglon;
                            }

                            _Renglon = ordIt.GetItemName(item);

                            _Objeto_Dibujo.DrawString(
                                "" + _Renglon.Substring(currentChar, _Renglon.Length - currentChar), _Fuente_Usada,
                                _Brocha, _Margen_Izquierdo, YPosition(), new StringFormat());
                            _Cant_Lineas++;
                            _Lineas_Corridas++;
                        }
                        else
                        {
                            _Objeto_Dibujo.DrawString(ordIt.GetItemName(item), _Fuente_Usada, _Brocha, _Margen_Izquierdo,
                                                      YPosition(), new StringFormat());

                            _Cant_Lineas++;
                            _Lineas_Corridas++;
                        }
                        //Ahora los detalles
                        //CANT  UNIMED PU                 SUBTOTAL
                        //http://www.csharp-examples.net/align-string-with-spaces/
                        object[] datos = new object[5];
                        datos[0] = ordIt.GetItemCantidad(item);
                        datos[1] = ordIt.GetItemUnimed(item);
                        if (datos[1].ToString().Length > 3)
                        {
                            if (Cortar_Unimed) //tipico paa imprimir el cliente
                                datos[1] = datos[1].ToString().Substring(0, 3);
                        }
                        else
                        {
                            int largo = datos[1].ToString().Length;
                            for (int z = 0; z < largo - 3; z++)
                            {
                                datos[1] = datos[1].ToString() + " ";
                            }
                        }

                        datos[2] = String.Format("{0:0.00}", Math.Round(Convert.ToDouble(ordIt.GetItemPrice(item)), 2));
                        datos[2] = datos[2].ToString().Trim();
                        string datos2 = datos[2].ToString();
                        //cehcar lo delpunto decimal
                        if (datos2.Length < 6)
                        {


                            int largo = datos[2].ToString().Length;
                            for (int z = 0; z < 6 - largo; z++)
                            {
                                datos[2] = " " + datos[2].ToString();
                            }
                        }

                        if (this.Mostrar_Precio == false)
                        {
                            datos[2] = "";
                        }

                        string pzcaja = "";
                        pzcaja = ordIt.GetItemPriceXCaja(item);
                        if (pzcaja.Length < 1)
                            pzcaja = "0";

                        datos[3] = String.Format("{0:0.0}", Math.Round(Convert.ToDouble(pzcaja), 1));
                            // ordIt.GetItemPriceXCaja(item);
                        if (datos[3].ToString().Length < 5)
                        {
                            //datos[2] = datos[2].ToString().Substring(0, 6);

                            int largo = datos[3].ToString().Length;
                            for (int z = 0; z < 5 - largo; z++)
                            {
                                datos[3] = " " + datos[3].ToString();
                            }
                        }
                        //2012-06-12no ocupa cant en cajas
                        datos[3] = " ";

                        if (this.Mostrar_Subtotal)
                            datos[4] = String.Format("{0:0.00}",
                                                     Math.Round(Convert.ToDouble(ordIt.GetItemSubTotal(item)), 2));
                                //ordIt.GetItemSubTotal(item);//realmente se puede mandar lo ke sea
                        else
                            datos[4] = "";
                        _Renglon = String.Format(

                            //	"{0,3} {1,-3} {2,6}  {3,5} {4,9}" //22jul2014--se cambia segun el tmaño de la printer
                            this.Formato_Items
                            , datos);
                        _Objeto_Dibujo.DrawString(_Renglon, _Fuente_Usada, _Brocha, _Margen_Izquierdo, YPosition());
                        _Cant_Lineas++;
                        _Lineas_Corridas++;
                        _Detalle_Activo = i + 1;

                        if (_Lineas_Corridas <= LineasPorOja)
                        {
                            e.HasMorePages = false;
                        }
                        else
                        {
                            _Lineas_Corridas = 0;


                            e.HasMorePages = true; //avisa que hay mas paginas
                            nueva_pagina = true;
                            _Cant_Lineas = 0;
                            break;
                        }

                    } //fin del ciclo de los detalles
                    if (e.HasMorePages == false)
                    {
                        _Detalle_Activo = 0;
                        _Impreso_Detalles = true;
                    }
                } //DETALLES NO IMPRESOS
            }

            #endregion DETALLES
            if (!e.HasMorePages)
            {
                _Margen_Izquierdo = 0;
                if (this.Imprimir_Detalles)
                {
                    _Renglon = DottedLine();
                    _Objeto_Dibujo.DrawString(_Renglon, _Fuente_Usada, _Brocha, _Margen_Izquierdo, YPosition(),
                                              new StringFormat());
                    _Cant_Lineas++;
                    _Lineas_Corridas++;
                    //                DrawEspacio();
                }
                // DrawTotales();
               if(_Impreso_Totales==false)
               {
	               OrderTotal ordTot = new OrderTotal('?');
		            for (int i=_Detalle_Activo; i<totales.Count ;i++)
		            {
		            	string total=totales[i].ToString();
		                _Renglon =ordTot.GetTotalCantidad(total);
		                _Renglon = AlignRightText(_Renglon.Length)  +_Renglon;
		
		                _Objeto_Dibujo.DrawString(_Renglon, _Fuente_Usada, _Brocha, _Margen_Izquierdo, YPosition(), new StringFormat());
		                _Margen_Izquierdo = 0;
		
		                _Renglon = ""+ordTot.GetTotalName(total);
		                _Objeto_Dibujo.DrawString(_Renglon, _Fuente_Usada, _Brocha, _Margen_Izquierdo, YPosition(), new StringFormat());
		                _Cant_Lineas++;
		                _Lineas_Corridas++;
		             	_Detalle_Activo = i + 1;   
		                if (_Lineas_Corridas <= LineasPorOja) 
		                {									
		                    e.HasMorePages = false;
		                }
		                else
		                {
		                    _Lineas_Corridas = 0;
		                    //_Detalle_Activo = i + 1;
		
		                    e.HasMorePages = true;//avisa que hay mas paginas
		                    //nueva_pagina = true;
		                    _Cant_Lineas = 0;
		                    break;
		                }
		            }//for de totales
		            if(e.HasMorePages==false )
		            {
			            _Impreso_Totales=true;
			            _Margen_Izquierdo = 0;
			            _Detalle_Activo=0;
//			            DrawEspacio();
//			            DrawEspacio();
		            }
	               
               }//sino esta impreso TOTALES
	           //ver impresion de pie de pagina  
		       if (!e.HasMorePages)
            {  
	           	if(_Impreso_Pie_Pagina==false)
		           {
		             // foreach (string footer in footerLines)
		             for (int i=_Detalle_Activo; i<footerLines.Count ;i++)
			            {
		             		FooterLine linea=(FooterLine) footerLines[i];
		             		
		             		//string footer=footerLines[i].ToString();
		             		string footer=linea.line;
		             		//vemos si el bold ponemos la letra negrita sino pos normal
		             		
		             		if(linea.bold)
		             		{
		             			//Tamaño_Fuente=10;
		             			_Fuente_Usada= new Font(_Fuente_Usada.FontFamily,Tamaño_Fuente, FontStyle.Bold | FontStyle.Underline);
		             		}
		             		else
		             		{
		             			//Tamaño_Fuente=9;
		             				_Fuente_Usada= new Font(_Fuente_Usada.FontFamily,Tamaño_Fuente, FontStyle.Regular);
		             		}
		             		//--------------------------------------------------------
		             		
		             		if (footer.Length > _Longitud_MaximRenglon)
			                {
			                    int currentChar = 0;
			                    int footerLenght = footer.Length;
			
			                    while (footerLenght > _Longitud_MaximRenglon)
			                    {
			                        _Renglon = footer;
			                        _Objeto_Dibujo.DrawString(_Renglon.Substring(currentChar, _Longitud_MaximRenglon), _Fuente_Usada, _Brocha, _Margen_Izquierdo, YPosition(), new StringFormat());
			
			                        _Cant_Lineas++;
			                        _Lineas_Corridas++;
			                        currentChar += _Longitud_MaximRenglon;
			                        footerLenght -= _Longitud_MaximRenglon;
			                    }
			                    _Renglon = footer;
			                    _Objeto_Dibujo.DrawString(_Renglon.Substring(currentChar, _Renglon.Length - currentChar), _Fuente_Usada, _Brocha, _Margen_Izquierdo, YPosition(), new StringFormat());
			                    _Cant_Lineas++;
			                    _Lineas_Corridas++;
			                    
			                }
			                else
			                {
			                    _Renglon = footer;
			                    _Objeto_Dibujo.DrawString(_Renglon, _Fuente_Usada, _Brocha, _Margen_Izquierdo, YPosition(), new StringFormat());
			
			                    _Cant_Lineas++;
			                }
			                
			                _Detalle_Activo = i + 1;   
			                
			                if (_Lineas_Corridas <= LineasPorOja)
			                {									
			                    e.HasMorePages = false;
			                }
			                else
			                {
			                    _Lineas_Corridas = 0;
			                    //_Detalle_Activo = i + 1;
			
			                    e.HasMorePages = true;//avisa que hay mas paginas
			                    //nueva_pagina = true;
			                    _Cant_Lineas = 0;
			                    break;
			                }
			            } 
	            	}
		       }
            try
            {
	            if (_Imagen_Encabezado != null)
	            {
	                Imagen_Encabezado.Dispose();
	                _Imagen_Encabezado.Dispose();
	            }
            }
            catch {}
        }
        }
        /// <summary>
        /// Deuelve la posicion Y sumando margenSUperior + (lineas * Espacio_Entre_Lineas) + AlturaImagen
        /// </summary>
        /// <returns></returns>
        private float YPosition()
        {
            return _Margen_Superior + (_Cant_Lineas * _Fuente_Usada.GetHeight(_Objeto_Dibujo));//+ _Alto_Imagen);
        }

        public string CentrarCadena(string dato, int longitud)
        {

            try
            {

                int Bandera = longitud - dato.Length;

                for (int i = 0; i <= Bandera - 1; i++)
                {

                    if (i % 2 != 0)

                        dato = " " + dato;

                    else

                        dato = dato + " ";

                }

                return dato;

            }

            catch (Exception ex)
            {

                throw new Exception(ex.Message);

            }

        }
        private void DrawImage()
        {
            if (_Imagen_Encabezado != null)
            {
                try
                {
                    _Objeto_Dibujo.DrawImage(_Imagen_Encabezado, new Point( 3, (int)YPosition()));//antes15
                    double height = ((double)_Imagen_Encabezado.Height / 58) * 15;
                    _Alto_Imagen = (int)Math.Round(height) + 3;
                }
                catch (Exception)
                {
                }
            }
            _Impreso_Imagen = true;
        }

        private void DibujarEncabezado()
        {
            //aki ponemso el margen izquierdo a mas de cero al final se pone cero
            _Margen_Izquierdo = 9;

            foreach(string header in headerLines)
            {
                if (header != null)
                {
                    if (header.Length > _Longitud_MaximRenglon)
                    {
                        int currentChar = 0;
                        int headerLenght = header.Length;

                        while (headerLenght > _Longitud_MaximRenglon)
                        {
                            _Renglon = header.Substring(currentChar, _Longitud_MaximRenglon);
                            _Objeto_Dibujo.DrawString(CentrarCadena(_Renglon, _Longitud_MaximRenglon), _Fuente_Usada, _Brocha, _Margen_Izquierdo, YPosition() , new StringFormat());

                            _Cant_Lineas++;
                            _Lineas_Corridas++;
                            currentChar += _Longitud_MaximRenglon;
                            headerLenght -= _Longitud_MaximRenglon;
                        }
                        _Renglon = header;
                        _Objeto_Dibujo.DrawString(CentrarCadena(_Renglon.Substring(currentChar, _Renglon.Length - currentChar), _Longitud_MaximRenglon), _Fuente_Usada, _Brocha, _Margen_Izquierdo, YPosition(), new StringFormat());
                        _Cant_Lineas++;
                        _Lineas_Corridas++;
                    }
                    else
                    {
                        _Renglon = header;
                        _Objeto_Dibujo.DrawString(CentrarCadena(_Renglon, _Longitud_MaximRenglon), _Fuente_Usada, _Brocha, _Margen_Izquierdo, YPosition(), new StringFormat());

                        _Cant_Lineas++;
                        _Lineas_Corridas++;
                    }
                }
                else
                {
                    
                }
            }
            _Margen_Izquierdo = 0;
            DrawEspacio();
        }   

        private void DrawSubHeader()
        {
            foreach (string subHeader in subHeaderLines)
            {
                if (subHeader.Length > _Longitud_MaximRenglon)
                {
                    int currentChar = 0;
                    int subHeaderLenght = subHeader.Length;

                    while (subHeaderLenght > _Longitud_MaximRenglon)
                    {
                        _Renglon = subHeader;
                        _Objeto_Dibujo.DrawString(_Renglon.Substring(currentChar, _Longitud_MaximRenglon), _Fuente_Usada, _Brocha, _Margen_Izquierdo,YPosition(), new StringFormat());

                        _Cant_Lineas++;
                        _Lineas_Corridas++;
                        currentChar += _Longitud_MaximRenglon;
                        subHeaderLenght -= _Longitud_MaximRenglon;
                    }
                    _Renglon = subHeader;
                    _Objeto_Dibujo.DrawString(_Renglon.Substring(currentChar, _Renglon.Length - currentChar), _Fuente_Usada, _Brocha, _Margen_Izquierdo, YPosition(), new StringFormat());
                    _Cant_Lineas++;
                    _Lineas_Corridas++;
                }
                else
                {
                    _Renglon = subHeader;

                    _Objeto_Dibujo.DrawString(_Renglon, _Fuente_Usada, _Brocha, _Margen_Izquierdo,YPosition(), new StringFormat());

                    _Cant_Lineas++;
                    _Lineas_Corridas++;
                    _Renglon = DottedLine();

                    _Objeto_Dibujo.DrawString(_Renglon, _Fuente_Usada, _Brocha, _Margen_Izquierdo, YPosition(), new StringFormat());

                    _Cant_Lineas++;
                    _Lineas_Corridas++;
                }
            }
            DrawEspacio();
        }
        /// <summary>
        /// Imprime el los detalles de los articulos vendidos
        /// en el formato CANT UMED DESCRIPCION       IMPORTE
        /// </summary>
        private void ImprimeItemsComun()
        {
            ListItem  ordIt = new ListItem('?');

            _Objeto_Dibujo.DrawString("CANT UMED DESCRIPCION       IMPORTE", _Fuente_Usada, _Brocha, _Margen_Izquierdo, YPosition(), new StringFormat());

            _Cant_Lineas++;
            DrawEspacio();

            foreach (string item in items)
            {
                //cantidad
                _Renglon = ordIt.GetItemCantidad(item);
                _Objeto_Dibujo.DrawString(_Renglon, _Fuente_Usada, _Brocha, _Margen_Izquierdo, YPosition(), new StringFormat());
                //Unimed
                _Renglon = ordIt.GetItemUnimed(item);
                _Objeto_Dibujo.DrawString("     " +_Renglon.Substring(0, 3), _Fuente_Usada, _Brocha, _Margen_Izquierdo, YPosition(), new StringFormat());
                //Precio
                _Renglon = ordIt.GetItemPrice(item);
                _Renglon = AlignRightText(_Renglon.Length) + _Renglon;
                _Objeto_Dibujo.DrawString(_Renglon, _Fuente_Usada, _Brocha, _Margen_Izquierdo, YPosition(), new StringFormat());
                //Descripcion
                string name = ordIt.GetItemName(item);

                _Margen_Izquierdo = 0;
                if (name.Length > _Longitud_Maxima_Descripcion)
                {
                    int currentChar = 0;
                    int itemLenght = name.Length;

                    while (itemLenght > _Longitud_Maxima_Descripcion)
                    {
                        _Renglon = ordIt.GetItemName(item);
                        _Objeto_Dibujo.DrawString("         " + _Renglon.Substring(currentChar, _Longitud_Maxima_Descripcion), _Fuente_Usada, _Brocha, _Margen_Izquierdo, YPosition(), new StringFormat());

                        _Cant_Lineas++;
                        currentChar += _Longitud_Maxima_Descripcion;
                        itemLenght -= _Longitud_Maxima_Descripcion;
                    }

                    _Renglon = ordIt.GetItemName(item);
                    _Objeto_Dibujo.DrawString("         " + _Renglon.Substring(currentChar, _Renglon.Length - currentChar), _Fuente_Usada, _Brocha, _Margen_Izquierdo, YPosition(), new StringFormat());
                    _Cant_Lineas++;
                }
                else
                {
                    _Objeto_Dibujo.DrawString("         " + ordIt.GetItemName(item), _Fuente_Usada, _Brocha, _Margen_Izquierdo, YPosition(), new StringFormat());

                    _Cant_Lineas++;
                }
            }

            _Margen_Izquierdo = 0;
            DrawEspacio();
            _Renglon = DottedLine();

            _Objeto_Dibujo.DrawString(_Renglon, _Fuente_Usada, _Brocha, _Margen_Izquierdo, YPosition(), new StringFormat());

            _Cant_Lineas++;
            DrawEspacio();
        }

        /// <summary>
        /// Imprime los detalles del Ticke en el formato:
        /// AAAAARRRRRTTTTTTTTIIIIICCCCCCUUUUULLLLLOOOO
        /// cantidad      PU              Subtotal
        /// </summary>
        private void ImprimeItemsCantidadesDebajoArticulo(ref System.Drawing.Printing.PrintPageEventArgs  gr )
        {

            ListItem ordIt = new ListItem('?');

            _Objeto_Dibujo.DrawString("CANT    UMED     PU      SUBTOTAL", _Fuente_Usada, _Brocha, _Margen_Izquierdo, YPosition(), new StringFormat());

            _Cant_Lineas++;
            int LineasCorridas = 0;
            int LineasPorOja = 73;
            DrawEspacio();

            foreach (string item in items)
            {
                
                //Descripcion
                string name = ordIt.GetItemName(item);
                
                _Margen_Izquierdo = 0;
                if (name.Length > _Longitud_MaximRenglon) 
                {
                    int currentChar = 0;
                    int itemLenght = name.Length;

                    while (itemLenght > _Longitud_MaximRenglon)
                    {
                        _Renglon = ordIt.GetItemName(item);
                        _Objeto_Dibujo.DrawString( _Renglon.Substring(currentChar, _Longitud_MaximRenglon), _Fuente_Usada, _Brocha, _Margen_Izquierdo, YPosition(), new StringFormat());

                        _Cant_Lineas++;
                        LineasCorridas++;
                        currentChar += _Longitud_MaximRenglon;
                        itemLenght -= _Longitud_MaximRenglon;
                    }

                    _Renglon = ordIt.GetItemName(item);
                    
                    _Objeto_Dibujo.DrawString("" + _Renglon.Substring(currentChar, _Renglon.Length - currentChar), _Fuente_Usada, _Brocha, _Margen_Izquierdo, YPosition(), new StringFormat());
                    _Cant_Lineas++;
                    LineasCorridas++;
                }
                else
                {
                    _Objeto_Dibujo.DrawString( ordIt.GetItemName(item), _Fuente_Usada, _Brocha, _Margen_Izquierdo, YPosition(), new StringFormat());

                    _Cant_Lineas++;
                    LineasCorridas++;
                }
                //Ahora los detalles
                
                //CANT  UNIMED PU                 SUBTOTAL
                //cantidad
                _Renglon = ordIt.GetItemCantidad(item);
                _Objeto_Dibujo.DrawString(_Renglon, _Fuente_Usada, _Brocha, _Margen_Izquierdo, YPosition(), new StringFormat());
                //Unimed
                _Renglon = ordIt.GetItemUnimed(item);
                _Objeto_Dibujo.DrawString("      " + _Renglon, _Fuente_Usada, _Brocha, _Margen_Izquierdo, YPosition(), new StringFormat());
                //PRECIO
                _Renglon = ordIt.GetItemPrice(item);
                _Renglon = AlignRightText(18) + _Renglon;
                _Objeto_Dibujo.DrawString(_Renglon, _Fuente_Usada, _Brocha, _Margen_Izquierdo, YPosition(), new StringFormat());

//                _Renglon = ordIt.GetItemPriceXCaja(item);
//                _Renglon = AlignRightText(25) + _Renglon;
//                _Objeto_Dibujo.DrawString(_Renglon, _Fuente_Usada, _Brocha, _Margen_Izquierdo, YPosition(), new StringFormat());
                
                
                //subtotal -- Final de renglon
                _Renglon = ordIt.GetItemSubTotal(item);
                _Renglon = AlignRightText(_Renglon.Length)  + _Renglon;
                _Objeto_Dibujo.DrawString(_Renglon, _Fuente_Usada, _Brocha, _Margen_Izquierdo, YPosition(), new StringFormat());
                
                
                _Cant_Lineas++;
                LineasCorridas++;
                if (LineasCorridas <= LineasPorOja)
                {
                    gr.HasMorePages = false;
                }
                else
                {
                    LineasCorridas=0;
                    gr.HasMorePages = true;
                }
            }
            
            _Margen_Izquierdo = 0;
            DrawEspacio();
            _Renglon = DottedLine();

            _Objeto_Dibujo.DrawString(_Renglon, _Fuente_Usada, _Brocha, _Margen_Izquierdo, YPosition(), new StringFormat());

            _Cant_Lineas++;
            DrawEspacio();
        }

        private void DrawTotales()
        {
            OrderTotal ordTot = new OrderTotal('?');

            foreach (string total in totales)
            {
                _Renglon =ordTot.GetTotalCantidad(total);
                _Renglon = AlignRightText(_Renglon.Length)  +_Renglon;

                _Objeto_Dibujo.DrawString(_Renglon, _Fuente_Usada, _Brocha, _Margen_Izquierdo, YPosition(), new StringFormat());
                _Margen_Izquierdo = 0;

                _Renglon = ""+ordTot.GetTotalName(total);
                _Objeto_Dibujo.DrawString(_Renglon, _Fuente_Usada, _Brocha, _Margen_Izquierdo, YPosition(), new StringFormat());
                _Cant_Lineas++;
                _Lineas_Corridas++;
            }
            _Margen_Izquierdo = 0;
            DrawEspacio();
            DrawEspacio();
        }

        private void DrawFooter()
        {
            foreach (string footer in footerLines)
            {
                if (footer.Length > _Longitud_MaximRenglon)
                {
                    int currentChar = 0;
                    int footerLenght = footer.Length;

                    while (footerLenght > _Longitud_MaximRenglon)
                    {
                        _Renglon = footer;
                        _Objeto_Dibujo.DrawString(_Renglon.Substring(currentChar, _Longitud_MaximRenglon), _Fuente_Usada, _Brocha, _Margen_Izquierdo, YPosition(), new StringFormat());

                        _Cant_Lineas++;
                        _Lineas_Corridas++;
                        currentChar += _Longitud_MaximRenglon;
                        footerLenght -= _Longitud_MaximRenglon;
                    }
                    _Renglon = footer;
                    _Objeto_Dibujo.DrawString(_Renglon.Substring(currentChar, _Renglon.Length - currentChar), _Fuente_Usada, _Brocha, _Margen_Izquierdo, YPosition(), new StringFormat());
                    _Cant_Lineas++;
                    _Lineas_Corridas++;
                }
                else
                {
                    _Renglon = footer;
                    _Objeto_Dibujo.DrawString(_Renglon, _Fuente_Usada, _Brocha, _Margen_Izquierdo, YPosition(), new StringFormat());

                    _Cant_Lineas++;
                }
            }
            _Margen_Izquierdo = 0;
            DrawEspacio();
        }

        private void DrawNotas()
        {
            foreach (string nota in notasLines)
            {
                //TODO:Dibujar un rectangulo
                _Objeto_Dibujo.DrawRectangle(new Pen(_Brocha), 0, YPosition(), _Longitud_MaximRenglon, 20); 

                if (nota.Length > _Longitud_MaximRenglon)
                {
                    int currentChar = 0;
                    int footerLenght = nota.Length;

                    while (footerLenght > _Longitud_MaximRenglon)
                    {
                        _Renglon = nota;
                        _Objeto_Dibujo.DrawString(_Renglon.Substring(currentChar, _Longitud_MaximRenglon), _Fuente_Usada, _Brocha, _Margen_Izquierdo, YPosition(), new StringFormat());

                        _Cant_Lineas++;
                        _Lineas_Corridas++;
                        currentChar += _Longitud_MaximRenglon;
                        footerLenght -= _Longitud_MaximRenglon;
                    }
                    _Renglon = nota;
                    _Objeto_Dibujo.DrawString(_Renglon.Substring(currentChar, _Renglon.Length - currentChar), _Fuente_Usada, _Brocha, _Margen_Izquierdo, YPosition(), new StringFormat());
                    _Cant_Lineas++;
                    _Lineas_Corridas++;
                }
                else
                {
                    _Renglon = nota;
                    _Objeto_Dibujo.DrawString(_Renglon, _Fuente_Usada, _Brocha, _Margen_Izquierdo, YPosition(), new StringFormat());

                    _Cant_Lineas++;
                }
            }
            _Margen_Izquierdo = 0;
            DrawEspacio();
        }

        public void DrawEspacio()
        {
            _Renglon = "";

            _Objeto_Dibujo.DrawString(_Renglon, _Fuente_Usada, _Brocha, _Margen_Izquierdo, YPosition(), new StringFormat());

            _Cant_Lineas++;
            _Lineas_Corridas++;
        }
    }

    //private class ListItem
    //{
    //    char[] delimitador = new char[] { '?' };

    //    public ListItem(char delimit)
    //    {
    //        delimitador= new char[] {delimit };
    //    }

    //    public string GetItemCantidad(string orderItem)
    //    {
    //        string [] delimitado = orderItem.Split(delimitador);
    //        return delimitado[0];
    //    }

    //    public string GetItemUnimed(string orderItem)
    //    {
    //        string[] delimitado = orderItem.Split(delimitador);
    //        return delimitado[1];
    //    }

    //    public string GetItemName(string orderItem)
    //    {
    //        string[] delimitado = orderItem.Split(delimitador);
    //        return delimitado[2];
    //    }

    //    public string GetItemPrice(string orderItem)
    //    {
    //        string[] delimitado = orderItem.Split(delimitador);
    //        return delimitado[3];
    //    }

    //    public string GetItemPriceXCaja(string orderItem)
    //    {
    //        string[] delimitado = orderItem.Split(delimitador);
    //        return delimitado[4];
    //    }

    //    public string GetItemSubTotal(string orderItem)
    //    {
    //        string[] delimitado = orderItem.Split(delimitador);
    //        return delimitado[5];
    //    }

    //    public string GenerateItem(string cantidad,string Unimed,  string itemName, string price,string precioXcaja, string subtotal)
    //    {
    //        return cantidad + delimitador[0] + Unimed + delimitador[0] + itemName + delimitador[0] + price + delimitador[0] + precioXcaja + delimitador[0] + subtotal;
    //    }
    //}

    //private class OrderTotal
    //{
    //    char[] delimitador = new char[] { '?' };

    //    public OrderTotal(char delimit)
    //    {
    //        delimitador = new char[] { delimit };
    //    }

    //    public string GetTotalName(string totalItem)
    //    {
    //        string[] delimitado = totalItem.Split(delimitador);
    //        return delimitado[0];
    //    }

    //    public string GetTotalCantidad(string totalItem)
    //    {
    //        string[] delimitado = totalItem.Split(delimitador);
    //        return delimitado[1];
    //    }

    //    public string GenerateTotal(string totalName, string price)
    //    {
    //        return totalName + delimitador[0] + price;
    //    }
    //}

    public class ListItem
    {
        char[] delimitador = new char[] { '?' };

        public ListItem(char delimit)
        {
            delimitador = new char[] { delimit };
        }

        public string GetItemCantidad(string orderItem)
        {
            string[] delimitado = orderItem.Split(delimitador);
            return delimitado[0];
        }

        public string GetItemUnimed(string orderItem)
        {
            string[] delimitado = orderItem.Split(delimitador);
            return delimitado[1];
        }

        public string GetItemName(string orderItem)
        {
            string[] delimitado = orderItem.Split(delimitador);
            return delimitado[2];
        }

        public string GetItemPrice(string orderItem)
        {
            string[] delimitado = orderItem.Split(delimitador);
            return delimitado[3];
        }

        public string GetItemPriceXCaja(string orderItem)
        {
            string[] delimitado = orderItem.Split(delimitador);
            return delimitado[4];
        }

        public string GetItemSubTotal(string orderItem)
        {
            string[] delimitado = orderItem.Split(delimitador);
            return delimitado[5];
        }

        public string GenerateItem(string cantidad, string Unimed, string itemName, string price, string precioXcaja, string subtotal)
        {
            return cantidad + delimitador[0] + Unimed + delimitador[0] + itemName + delimitador[0] + price + delimitador[0] + precioXcaja + delimitador[0] + subtotal;
        }
    }

    public class OrderTotal
    {
        char[] delimitador = new char[] { '?' };

        public OrderTotal(char delimit)
        {
            delimitador = new char[] { delimit };
        }

        public string GetTotalName(string totalItem)
        {
            string[] delimitado = totalItem.Split(delimitador);
            return delimitado[0];
        }

        public string GetTotalCantidad(string totalItem)
        {
            string[] delimitado = totalItem.Split(delimitador);
            return delimitado[1];
        }

        public string GenerateTotal(string totalName, string price)
        {
            return totalName + delimitador[0] + price;
        }
    }
}
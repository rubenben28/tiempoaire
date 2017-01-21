using System;
using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyPOS
{
    static class Program
    {
        static Frm_Login frm_login;
        public static string usuario = "";
        public static string contraseña = "";
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            frm_login = new Frm_Login();
            Application.Run(frm_login);
        }
    }
}

using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;

namespace myServices
{
    class SysTrayApp
    {
        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.Run(new Systray());
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace myComponents.SoftwareLocker.Interfaces.frm
{

    public partial class frmCompany : Form
    {
        public frmCompany()
        {
            InitializeComponent();
        }

        [ReadOnly(true)]
        public String Company
        {
            get { return tbCompany.Text; }
        }

    }
}

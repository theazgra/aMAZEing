using System;
using System.Windows.Forms;

namespace aMaze_ingSolver
{
    public partial class ExportTypeForm : Form
    {
        public ExportTypeForm()
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
    }
}

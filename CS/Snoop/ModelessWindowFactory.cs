using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;

namespace RevitLookup.Snoop
{
    static class ModelessWindowFactory
    {
        public static void Show(Form newForm, Form parentForm = null)
        {
            if (parentForm == null)
            {
                newForm.StartPosition = FormStartPosition.CenterScreen;
            }
            else
            {
                parentForm.AddOwnedForm(newForm);
                newForm.StartPosition = FormStartPosition.Manual;
                newForm.Location = new Point(parentForm.Location.X + (parentForm.Width - newForm.Width) / 2, parentForm.Location.Y + (parentForm.Height - newForm.Height) / 2);  
            }
            newForm.Show(new ModelessWindowHandle(parentForm));
            newForm.FormClosed += NewForm_FormClosed;
        }

        private static void NewForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            ModelessWindowHandle.BringRevitToFront();
        }
    }
}

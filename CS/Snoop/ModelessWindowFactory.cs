using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Form = System.Windows.Forms.Form;
using Point = System.Drawing.Point;

namespace RevitLookup.Snoop
{
    public class ModelessWindowFactory
    {
        private readonly Form parentForm;
        private readonly Document targetDocument;

        public ModelessWindowFactory(Form parentForm, Document targetDocument)
        {
            this.parentForm = parentForm;
            this.targetDocument = targetDocument;
        }


        public void Show(Form newForm)
        {
            Show(newForm, targetDocument, parentForm);
        }


        public static void Show(Form newForm, Document targetDocument = null, Form parentForm = null)
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
            if ((targetDocument != null) && (newForm is IHaveCollector formWithCollector))
            {
                formWithCollector.SetDocument(targetDocument);
            }
            newForm.Show(new ModelessWindowHandle(parentForm));
            newForm.FormClosed += (s, e) => ModelessWindowHandle.BringRevitToFront();
        }
    }
}
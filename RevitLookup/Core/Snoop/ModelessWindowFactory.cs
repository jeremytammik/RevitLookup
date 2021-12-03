using System.Windows.Forms;
using Autodesk.Revit.DB;
using Form = System.Windows.Forms.Form;
using Point = System.Drawing.Point;

namespace RevitLookup.Core.Snoop
{
    public class ModelessWindowFactory
    {
        private readonly Form _parentForm;
        private readonly Document _targetDocument;

        public ModelessWindowFactory(Form parentForm, Document targetDocument)
        {
            _parentForm = parentForm;
            _targetDocument = targetDocument;
        }

        public void Show(Form newForm)
        {
            Show(newForm, _targetDocument, _parentForm);
        }


        public static void Show(Form newForm, Document targetDocument = null, Form parentForm = null)
        {
            AddKeyEvents(newForm);
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

            if (targetDocument != null && newForm is IHaveCollector formWithCollector) formWithCollector.SetDocument(targetDocument);
            newForm.Show(new ModelessWindowHandle());
            newForm.FormClosed += (s, e) =>
            {
                ModelessWindowHandle.BringRevitToFront();
                FocusOwner((Form) s);
            };
        }

        private static void AddKeyEvents(Form parentForm)
        {
            parentForm.KeyPreview = true;
            parentForm.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Escape) ((Form) s).Close();
            };
        }

        private static void FocusOwner(Form form)
        {
            if (form.Owner is { } owner) owner.Focus();
        }
    }
}
using Autodesk.Revit.DB;
using Form = System.Windows.Forms.Form;
using Point = System.Drawing.Point;

namespace RevitLookup.Core;

public class ModelessWindowFactory
{
    private readonly Form _parentForm;
    private readonly Document _targetDocument;

    public ModelessWindowFactory(Form parentForm, Document targetDocument)
    {
        _parentForm = parentForm;
        _targetDocument = targetDocument;
    }

    public void ShowForm(Form form)
    {
        Show(form, _targetDocument, _parentForm);
    }

    public static void Show(Form form, Document targetDocument = null, Form parentForm = null)
    {
        AddKeyEvents(form);
        if (parentForm is null)
        {
            form.StartPosition = FormStartPosition.CenterScreen;
        }
        else
        {
            parentForm.AddOwnedForm(form);
            form.StartPosition = FormStartPosition.Manual;
            form.Location = new Point(parentForm.Location.X + (parentForm.Width - form.Width) / 2, parentForm.Location.Y + (parentForm.Height - form.Height) / 2);
        }

        if (targetDocument is not null && form is IHaveCollector formWithCollector) formWithCollector.Document = targetDocument;
        form.Show(new ModelessWindowHandle());
        form.FormClosed += (s, e) =>
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
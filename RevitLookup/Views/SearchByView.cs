using Autodesk.Revit.DB;
using RevitLookup.Core;
using Form = System.Windows.Forms.Form;

namespace RevitLookup.Views;

public partial class SearchByView : Form
{
    private readonly Document _doc;

    public SearchByView(Document doc)
    {
        InitializeComponent();
        m_cbSearchByVariant.SelectedIndex = 0;
        _doc = doc;
    }

    private void m_bnFindAndSnoop_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(m_tbSearchValue.Text))
            MessageBox.Show(@"You did not enter a value to search for", @"Attention!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
        switch (m_cbSearchByVariant.SelectedItem as string)
        {
            case "ElementId":
                SearchAndSnoopByElementId();
                break;
            case "UniqId":
                SearchAndSnoopByUniqId();
                break;
        }
    }

    private void SearchAndSnoopByElementId()
    {
        if (int.TryParse(m_tbSearchValue.Text, out var id))
        {
            var element = _doc.GetElement(new ElementId(id));
            if (element is not null)
            {
                var form = new ObjectsView(element);
                ModelessWindowFactory.Show(form, _doc, this);
            }
            else
            {
                MessageBox.Show($@"No items with ID {id} found");
            }
        }
        else
        {
            MessageBox.Show(@"The ID value must represent an integer value");
        }
    }

    private void SearchAndSnoopByUniqId()
    {
        var element = _doc.GetElement(m_tbSearchValue.Text);
        if (element is not null)
        {
            var form = new ObjectsView(element);
            ModelessWindowFactory.Show(form, _doc, this);
        }
        else
        {
            MessageBox.Show($@"No items with ID {m_tbSearchValue.Text} found");
        }
    }
}
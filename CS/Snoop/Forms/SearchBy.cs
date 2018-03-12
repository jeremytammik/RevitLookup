using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Autodesk.Revit.DB;
using Form = System.Windows.Forms.Form;

namespace RevitLookup.Snoop.Forms
{
  public partial class SearchBy : Form
  {
    private readonly Document _doc;

    public SearchBy( Document doc )
    {
      InitializeComponent();
      m_cbSearchByVariant.SelectedIndex = 0;
      _doc = doc;
    }

    private void m_bnFindAndSnoop_Click( object sender, System.EventArgs e )
    {
      if( string.IsNullOrEmpty( m_tbSearchValue.Text ) )
      {
        MessageBox.Show( @"You did not enter a value to search for", @"Attention!", MessageBoxButtons.OK,
            MessageBoxIcon.Stop );
      }
      switch( m_cbSearchByVariant.SelectedItem as string )
      {
        case "ElementId": // by ElementId
          SearcAndSnoopByElementId();
          break;
        case "UniqId": // by UniqId
          SearchAndSnoopByUniqId();
          break;
      }
    }

    private void SearcAndSnoopByElementId()
    {
      int id;
      if( int.TryParse( m_tbSearchValue.Text, out id ) )
      {
        FilteredElementCollector elemTypeCtor = ( new FilteredElementCollector( _doc ) ).WhereElementIsElementType();
        FilteredElementCollector notElemTypeCtor = ( new FilteredElementCollector( _doc ) ).WhereElementIsNotElementType();
        FilteredElementCollector allElementCtor = elemTypeCtor.UnionWith( notElemTypeCtor );
        ICollection<ElementId> ids = allElementCtor
            .Where( el => el.Id.IntegerValue == id ).Select( el => el.Id ).ToList();
        if( ids.Count != 0 )
        {
          Snoop.Forms.Objects form = new Snoop.Forms.Objects( _doc, ids );
          form.ShowDialog();
        }
        else
          MessageBox.Show( $@"No items with ID {id} found" );
      }
      else
        MessageBox.Show( @"The ID value must represent an integer value" );
    }

    private void SearchAndSnoopByUniqId()
    {
      FilteredElementCollector elemTypeCtor = ( new FilteredElementCollector( _doc ) ).WhereElementIsElementType();
      FilteredElementCollector notElemTypeCtor = ( new FilteredElementCollector( _doc ) ).WhereElementIsNotElementType();
      FilteredElementCollector allElementCtor = elemTypeCtor.UnionWith( notElemTypeCtor );
      ICollection<ElementId> ids = allElementCtor
          .Where( el => el.UniqueId == m_tbSearchValue.Text ).Select( el => el.Id ).ToList();
      if( ids.Count != 0 )
      {
        Snoop.Forms.Objects form = new Snoop.Forms.Objects( _doc, ids );
        form.ShowDialog();
      }
      else
        MessageBox.Show( $@"No items with ID {m_tbSearchValue.Text} found" );
    }
  }
}

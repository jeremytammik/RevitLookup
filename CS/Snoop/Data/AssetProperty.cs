using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Visual;
using System.Collections;

namespace RevitLookup.Snoop.Data
{
    public class AssetProperty : Data
    {
        protected Autodesk.Revit.DB.Visual.AssetProperty m_val;
        protected Element m_elem;
        protected AssetProperties m_assetProperties;

        public AssetProperty(string label, 
            AssetProperties assetProperties,
            Autodesk.Revit.DB.Visual.AssetProperty val) : base(label)
        {
            m_val = val;
            m_assetProperties = assetProperties;
        }


        public override bool 
            HasDrillDown
        {
            get
            {
                return m_assetProperties!=null && m_assetProperties.Size > 0;
            }
        }


        public override void DrillDown()
        {
            if (m_assetProperties != null)
            {
                ArrayList objs = new ArrayList();
                for (int i = 0; i < m_assetProperties.Size; i++)
                {
                    objs.Add(m_assetProperties.Get(i));
                }
                

                Snoop.Forms.Objects form = new Snoop.Forms.Objects(objs);
                form.ShowDialog();
            }
        }

       
        public override string StrValue()
        {
            return "<AssetProperty>";
        }
       
    }
}
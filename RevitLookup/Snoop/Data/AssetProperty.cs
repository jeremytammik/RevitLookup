using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Visual;
using System.Collections;
using System.Windows.Forms;

namespace RevitLookup.Snoop.Data
{
    public class AssetProperty : Data
    {
        protected Autodesk.Revit.DB.Visual.AssetProperty MVal;
        protected Element MElem;
        protected AssetProperties MAssetProperties;

        public AssetProperty(string label, 
            AssetProperties assetProperties,
            Autodesk.Revit.DB.Visual.AssetProperty val) : base(label)
        {
            MVal = val;
            MAssetProperties = assetProperties;
        }


        public override bool 
            HasDrillDown =>
            MAssetProperties is {Size: > 0};


        public override System.Windows.Forms.Form DrillDown()
        {
            if (MAssetProperties != null)
            {
                var objs = new ArrayList();
                for (var i = 0; i < MAssetProperties.Size; i++)
                {
                    objs.Add(MAssetProperties.Get(i));
                }
                

                var form = new Forms.Objects(objs);
                return form;
            }
            return null;
        }

       
        public override string StrValue()
        {
            return "<AssetProperty>";
        }
       
    }
}
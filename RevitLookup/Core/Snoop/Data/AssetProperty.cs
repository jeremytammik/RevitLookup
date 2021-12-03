using System.Collections;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Visual;
using RevitLookup.Views;
using Form = System.Windows.Forms.Form;

namespace RevitLookup.Core.Snoop.Data
{
    public class AssetProperty : Data
    {
        protected AssetProperties MAssetProperties;
        protected Element MElem;
        protected Autodesk.Revit.DB.Visual.AssetProperty MVal;

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


        public override Form DrillDown()
        {
            if (MAssetProperties != null)
            {
                var objs = new ArrayList();
                for (var i = 0; i < MAssetProperties.Size; i++) objs.Add(MAssetProperties.Get(i));


                var form = new Objects(objs);
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
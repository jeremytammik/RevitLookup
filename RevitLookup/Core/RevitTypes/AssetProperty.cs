using System.Collections;
using System.Windows.Forms;
using Autodesk.Revit.DB.Visual;
using RevitLookup.Views;

namespace RevitLookup.Core.RevitTypes
{
    public class AssetProperty : Data
    {
        private readonly AssetProperties _assetProperties;

        public AssetProperty(string label, AssetProperties assetProperties) : base(label)
        {
            _assetProperties = assetProperties;
        }

        public override bool HasDrillDown => _assetProperties is {Size: > 0};

        public override Form DrillDown()
        {
            if (_assetProperties is null) return null;
            var objects = new ArrayList();
            for (var i = 0; i < _assetProperties.Size; i++) objects.Add(_assetProperties.Get(i));
            var form = new Objects(objects);
            return form;
        }

        public override string StrValue()
        {
            return "<AssetProperty>";
        }
    }
}
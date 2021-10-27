using Autodesk.Revit.DB;
using System;
using System.Collections;
using System.Linq;

namespace RevitLookup.Snoop.CollectorExts
{
    public class PartUtilsStream : IElementStream
    {
        private readonly object _elem;
        private readonly ArrayList _data;

        public PartUtilsStream(ArrayList data, object elem)
        {
            this._elem = elem;
            this._data = data;
        }

        public void Stream(Type type)
        {
            var element = _elem as Element;
            var part = _elem as Part;

            if (type == typeof(Element) && element != null)
            {
                _data.Add(new Snoop.Data.MemberSeparatorWithOffset(nameof(PartUtils)));
                _data.Add(new Snoop.Data.Bool(nameof(PartUtils.AreElementsValidForCreateParts), PartUtils.AreElementsValidForCreateParts(element.Document, new[] { element.Id, })));
                _data.Add(new Snoop.Data.Object(nameof(PartUtils.GetAssociatedPartMaker), PartUtils.GetAssociatedPartMaker(element.Document, element.Id)));
                _data.Add(new Snoop.Data.Bool(nameof(PartUtils.HasAssociatedParts), PartUtils.HasAssociatedParts(element.Document, element.Id)));
                _data.Add(new Snoop.Data.Bool(nameof(PartUtils.IsValidForCreateParts), PartUtils.IsValidForCreateParts(element.Document, new LinkElementId(element.Id))));
            }

            if (type == typeof(Part) && part != null)
            {
                _data.Add(new Snoop.Data.MemberSeparatorWithOffset(nameof(PartUtils)));
                _data.Add(new Snoop.Data.Bool(nameof(PartUtils.ArePartsValidForDivide), PartUtils.ArePartsValidForDivide(part.Document, new[] { part.Id, })));
                _data.Add(new Snoop.Data.Bool(nameof(PartUtils.ArePartsValidForMerge), PartUtils.ArePartsValidForMerge(part.Document, new[] { part.Id, })));
                _data.Add(new Snoop.Data.Int(nameof(PartUtils.GetChainLengthToOriginal), PartUtils.GetChainLengthToOriginal(part)));
                var isMergedPart = PartUtils.IsMergedPart(part);
                _data.Add(new Snoop.Data.Enumerable(nameof(PartUtils.GetMergedParts), isMergedPart ? PartUtils.GetMergedParts(part) : Array.Empty<ElementId>(), part.Document));
                _data.Add(new Snoop.Data.Bool(nameof(PartUtils.IsMergedPart), isMergedPart));
                _data.Add(new Snoop.Data.Bool(nameof(PartUtils.IsPartDerivedFromLink), PartUtils.IsPartDerivedFromLink(part)));

                _data.Add(new Snoop.Data.MemberSeparatorWithOffset(nameof(Part)));
                _data.Add(new Snoop.Data.String(nameof(Part.OriginalCategoryId), ((BuiltInCategory)part.OriginalCategoryId.IntegerValue).ToString()));

                var sourceElementIds = part.GetSourceElementIds().Where(e => e.HostElementId != ElementId.InvalidElementId).Select(e => e.HostElementId).ToList();
                _data.Add(new Snoop.Data.Enumerable(nameof(Part.GetSourceElementIds), sourceElementIds, part.Document));

                var sourceCategoryIds = part.GetSourceElementOriginalCategoryIds().Select(e => (BuiltInCategory)e.IntegerValue).ToList();
                _data.Add(new Snoop.Data.EnumerableAsString(nameof(Part.GetSourceElementOriginalCategoryIds), sourceCategoryIds));
            }
        }
    }
}

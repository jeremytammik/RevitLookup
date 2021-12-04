using System;
using System.Collections;
using System.Linq;
using Autodesk.Revit.DB;
using RevitLookup.Core.RevitTypes;
using ElementId = Autodesk.Revit.DB.ElementId;
using Enumerable = RevitLookup.Core.RevitTypes.Enumerable;
using Object = RevitLookup.Core.RevitTypes.Object;
using String = RevitLookup.Core.RevitTypes.String;

namespace RevitLookup.Core.Streams
{
    public class PartUtilsStream : IElementStream
    {
        private readonly ArrayList _data;
        private readonly object _elem;

        public PartUtilsStream(ArrayList data, object elem)
        {
            _elem = elem;
            _data = data;
        }

        public void Stream(Type type)
        {
            var part = _elem as Part;

            if (type == typeof(Element) && _elem is Element element)
            {
                _data.Add(new MemberSeparatorWithOffset(nameof(PartUtils)));
                _data.Add(new Bool(nameof(PartUtils.AreElementsValidForCreateParts), PartUtils.AreElementsValidForCreateParts(element.Document, new[] {element.Id})));
                _data.Add(new Object(nameof(PartUtils.GetAssociatedPartMaker), PartUtils.GetAssociatedPartMaker(element.Document, element.Id)));
                _data.Add(new Bool(nameof(PartUtils.HasAssociatedParts), PartUtils.HasAssociatedParts(element.Document, element.Id)));
                _data.Add(new Bool(nameof(PartUtils.IsValidForCreateParts), PartUtils.IsValidForCreateParts(element.Document, new LinkElementId(element.Id))));
            }

            if (type == typeof(Part) && part != null)
            {
                _data.Add(new MemberSeparatorWithOffset(nameof(PartUtils)));
                _data.Add(new Bool(nameof(PartUtils.ArePartsValidForDivide), PartUtils.ArePartsValidForDivide(part.Document, new[] {part.Id})));
                _data.Add(new Bool(nameof(PartUtils.ArePartsValidForMerge), PartUtils.ArePartsValidForMerge(part.Document, new[] {part.Id})));
                _data.Add(new Int(nameof(PartUtils.GetChainLengthToOriginal), PartUtils.GetChainLengthToOriginal(part)));
                var isMergedPart = PartUtils.IsMergedPart(part);
                _data.Add(new Enumerable(nameof(PartUtils.GetMergedParts), isMergedPart ? PartUtils.GetMergedParts(part) : Array.Empty<ElementId>(), part.Document));
                _data.Add(new Bool(nameof(PartUtils.IsMergedPart), isMergedPart));
                _data.Add(new Bool(nameof(PartUtils.IsPartDerivedFromLink), PartUtils.IsPartDerivedFromLink(part)));

                _data.Add(new MemberSeparatorWithOffset(nameof(Part)));
                _data.Add(new String(nameof(Part.OriginalCategoryId), ((BuiltInCategory) part.OriginalCategoryId.IntegerValue).ToString()));

                var sourceElementIds = part.GetSourceElementIds().Where(e => e.HostElementId != ElementId.InvalidElementId).Select(e => e.HostElementId).ToList();
                _data.Add(new Enumerable(nameof(Part.GetSourceElementIds), sourceElementIds, part.Document));

                var sourceCategoryIds = part.GetSourceElementOriginalCategoryIds().Select(e => (BuiltInCategory) e.IntegerValue).ToList();
                _data.Add(new EnumerableAsString(nameof(Part.GetSourceElementOriginalCategoryIds), sourceCategoryIds));
            }
        }
    }
}
using System.Collections;
using Autodesk.Revit.DB;
using RevitLookup.Core.RevitTypes;
using RevitLookup.RevitUtils;

namespace RevitLookup.Core.Streams;

public class PartUtilsStream : IElementStream
{
    private readonly ArrayList _data;
    private readonly object _element;

    public PartUtilsStream(ArrayList data, object element)
    {
        _element = element;
        _data = data;
    }

    public void Stream(Type type)
    {
        var part = _element as Part;

        if (type == typeof(Element) && _element is Element element)
        {
            _data.Add(new MemberSeparatorWithOffsetData(nameof(PartUtils)));
            _data.Add(new BoolData(nameof(PartUtils.AreElementsValidForCreateParts), PartUtils.AreElementsValidForCreateParts(element.Document, new[] {element.Id})));
            _data.Add(new ObjectData(nameof(PartUtils.GetAssociatedPartMaker), PartUtils.GetAssociatedPartMaker(element.Document, element.Id)));
            _data.Add(new BoolData(nameof(PartUtils.HasAssociatedParts), PartUtils.HasAssociatedParts(element.Document, element.Id)));
            _data.Add(new BoolData(nameof(PartUtils.IsValidForCreateParts), PartUtils.IsValidForCreateParts(element.Document, new LinkElementId(element.Id))));
        }

        if (type == typeof(Part) && part is not null)
        {
            _data.Add(new MemberSeparatorWithOffsetData(nameof(PartUtils)));
            _data.Add(new BoolData(nameof(PartUtils.ArePartsValidForDivide), PartUtils.ArePartsValidForDivide(part.Document, new[] {part.Id})));
            _data.Add(new BoolData(nameof(PartUtils.ArePartsValidForMerge), PartUtils.ArePartsValidForMerge(part.Document, new[] {part.Id})));
            _data.Add(new IntData(nameof(PartUtils.GetChainLengthToOriginal), PartUtils.GetChainLengthToOriginal(part)));
            var isMergedPart = PartUtils.IsMergedPart(part);
            _data.Add(new EnumerableData(nameof(PartUtils.GetMergedParts), isMergedPart ? PartUtils.GetMergedParts(part) : Array.Empty<ElementId>(), part.Document));
            _data.Add(new BoolData(nameof(PartUtils.IsMergedPart), isMergedPart));
            _data.Add(new BoolData(nameof(PartUtils.IsPartDerivedFromLink), PartUtils.IsPartDerivedFromLink(part)));

            _data.Add(new MemberSeparatorWithOffsetData(nameof(Part)));
            _data.Add(new StringData(nameof(Part.OriginalCategoryId), ((BuiltInCategory) part.OriginalCategoryId.GetValue()).ToString()));

            var sourceElementIds = part.GetSourceElementIds().Where(id => id.HostElementId != ElementId.InvalidElementId).Select(e => e.HostElementId).ToList();
            _data.Add(new EnumerableData(nameof(Part.GetSourceElementIds), sourceElementIds, part.Document));

            var sourceCategoryIds = part.GetSourceElementOriginalCategoryIds().Select(id => (BuiltInCategory) id.GetValue()).ToList();
            _data.Add(new EnumerableAsString(nameof(Part.GetSourceElementOriginalCategoryIds), sourceCategoryIds));
        }
    }
}
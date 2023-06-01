using System.Reflection;
using RevitLookup.Core.Utils;

namespace RevitLookup.Core.Metadata;

public partial class DescriptorBuilder
{
    private void AddEvents(BindingFlags bindingFlags)
    {
        if (!_settings.IsEventsAllowed) return;

        var members = _type.GetEvents(bindingFlags);
        foreach (var member in members)
        {
            WriteDescriptor(member, DescriptorUtils.MakeGenericTypeName(member.EventHandlerType), null);
        }
    }
}
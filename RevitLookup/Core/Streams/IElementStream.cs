using System;

namespace RevitLookup.Core.Streams;

public interface IElementStream
{
    void Stream(Type type);
}
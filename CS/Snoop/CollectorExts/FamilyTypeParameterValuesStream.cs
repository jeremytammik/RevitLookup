using System;
using System.Collections;
using System.Linq;
using Autodesk.Revit.DB;

namespace RevitLookup.Snoop.CollectorExts
{
    public class FamilyTypeParameterValuesStream : IElementStream
    {
        private readonly ArrayList data;
        private readonly object elem;

        public FamilyTypeParameterValuesStream(ArrayList data, object elem)
        {
            this.data = data;
            this.elem = elem;
        }

        public void Stream(Type type)
        {
            if (type != typeof(Parameter))
                return;

            var parameter = (Parameter)elem;

            var family = (parameter.Element as FamilyInstance)?.Symbol.Family ?? (parameter.Element as FamilySymbol)?.Family;

            if (parameter.Definition.ParameterType != ParameterType.FamilyType || family == null)
                return;

            var familyTypeParameterValues = family
                .GetFamilyTypeParameterValues(parameter.Id)
                .Select(family.Document.GetElement)
                .ToList();

            data.Add(new Data.Enumerable($"{nameof(Family)}.{nameof(Family.GetFamilyTypeParameterValues)}()", familyTypeParameterValues));
        }
    }
}
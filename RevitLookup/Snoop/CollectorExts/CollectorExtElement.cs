#region Header

//
// Copyright 2003-2021 by Autodesk, Inc. 
//
// Permission to use, copy, modify, and distribute this software in
// object code form for any purpose and without fee is hereby granted, 
// provided that the above copyright notice appears in all copies and 
// that both that copyright notice and the limited warranty and
// restricted rights notice below appear in all supporting 
// documentation.
//
// AUTODESK PROVIDES THIS PROGRAM "AS IS" AND WITH ALL FAULTS. 
// AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE.  AUTODESK, INC. 
// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.
//
// Use, duplication, or disclosure by the U.S. Government is subject to 
// restrictions set forth in FAR 52.227-19 (Commercial Computer
// Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
// (Rights in Technical Data and Computer Software), as applicable.
//

#endregion // Header

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using RevitLookup.Snoop.Collectors;
using RevitLookup.Snoop.Data;
using Enumerable = RevitLookup.Snoop.Data.Enumerable;
using Exception = System.Exception;
using Object = RevitLookup.Snoop.Data.Object;
using String = RevitLookup.Snoop.Data.String;

namespace RevitLookup.Snoop.CollectorExts
{
    /// <summary>
    ///     Provide Snoop.Data for any classes related to an Element.
    /// </summary>
    public class CollectorExtElement : CollectorExt
    {
        private static readonly Type[] Types;

        static CollectorExtElement()
        {
            var baseDirectory = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);

            Types = AppDomain.CurrentDomain.GetAssemblies()
                .Where(x => !x.IsDynamic && !string.IsNullOrWhiteSpace(x.Location))
                .Where(x => Path.GetDirectoryName(x.Location) == baseDirectory)
                .Where(x => x.GetName().Name.ToLower().Contains("revit"))
                .SelectMany(x => x.GetTypes())
                .Union(new[] {typeof(KeyValuePair<,>)})
                .ToArray();
        }

        public CollectorExtElement(Document doc) : base(doc)
        {
        }

        public override void Collect(Collector snoopCollector, CollectorEventArgs e)
        {
            if (e.ObjToSnoop is IEnumerable snoop)
                snoopCollector.Data().Add(new Enumerable(snoop.GetType().Name, snoop));
            else
                Stream(snoopCollector.Data(), e.ObjToSnoop);
        }

        private void Stream(ArrayList data, object elem)
        {
            var thisElementTypes = Types.Where(x => IsSnoopableType(x, elem)).ToList();

            var streams = new IElementStream[]
            {
                new ElementPropertiesStream(MDoc, data, elem),
                new ElementMethodsStream(MDoc, data, elem),
                new SpatialElementStream(data, elem),
                new FamilyTypeParameterValuesStream(data, elem),
                new ExtensibleStorageEntityContentStream(MDoc, data, elem),
                new PartUtilsStream(data, elem)
            };

            foreach (var type in thisElementTypes)
            {
                data.Add(new ClassSeparator(type));

                foreach (var elementStream in streams)
                    elementStream.Stream(type);
            }

            StreamElementExtensibleStorages(data, elem as Element);

            StreamSimpleType(data, elem);
        }

        private static bool IsSnoopableType(Type type, object element)
        {
            var elementType = element.GetType();

            if (type == elementType || elementType.IsSubclassOf(type) || type.IsAssignableFrom(elementType))
                return true;

            return type.IsGenericType && elementType.IsGenericType && IsSubclassOfRawGeneric(type, elementType);
        }

        private static bool IsSubclassOfRawGeneric(Type generic, Type toCheck)
        {
            while (toCheck != null && toCheck != typeof(object))
            {
                var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
                if (generic == cur) return true;
                toCheck = toCheck.BaseType;
            }

            return false;
        }

        private static void StreamElementExtensibleStorages(ArrayList data, Element elem)
        {
            var schemas = Schema.ListSchemas();

            if (elem == null || !schemas.Any())
                return;

            data.Add(new ExtensibleStorageSeparator());

            foreach (var schema in schemas)
            {
                var objectName = "Entity with Schema [" + schema.SchemaName + "]";
                try
                {
                    var entity = elem.GetEntity(schema);

                    if (!entity.IsValid())
                        continue;

                    data.Add(new Object(objectName, entity));
                }
                catch (Exception ex)
                {
                    data.Add(new Data.Exception(objectName, ex));
                }
            }
        }

        private void StreamSimpleType(ArrayList data, object elem)
        {
            var elemType = elem.GetType();

            if (elemType.IsEnum || elemType.IsPrimitive || elemType.IsValueType)
                data.Add(new String($"{elemType.Name} value", elem.ToString()));
        }
    }
}
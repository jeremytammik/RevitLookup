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
using System.Linq;
using System.Collections;
using System.Diagnostics;
using System.IO;
using RevitLookup.Snoop.Collectors;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using RevitLookup.Snoop.Data;
using String = System.String;

namespace RevitLookup.Snoop.CollectorExts
{
    /// <summary>
    /// Provide Snoop.Data for any classes related to an Element.
    /// </summary>

    public class CollectorExtElement : CollectorExt
    {
        readonly Type[] types;

        public CollectorExtElement()
        {
            var baseDirectory = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);

            types = AppDomain.CurrentDomain.GetAssemblies()
                .Where(x => !x.IsDynamic && !string.IsNullOrWhiteSpace(x.Location))
                .Where(x => Path.GetDirectoryName(x.Location) == baseDirectory)
                .Where(x => x.GetName().Name.ToLower().Contains("revit"))
                .SelectMany(x => x.GetTypes())
                .ToArray();
        }

        protected override void CollectEvent(object sender, CollectorEventArgs e)
        {
            Collector snoopCollector = sender as Collector;
            if (snoopCollector == null)
            {
                Debug.Assert(false); // why did someone else send us the message?
                return;
            }

            if (e.ObjToSnoop is IEnumerable)
                snoopCollector.Data().Add(new Snoop.Data.Enumerable(e.ObjToSnoop.GetType().Name, e.ObjToSnoop as IEnumerable));
            else
                Stream(snoopCollector.Data(), e.ObjToSnoop);
        }

        private void Stream(ArrayList data, object elem)
        {
            var thisElementTypes = types.Where(x => elem.GetType().IsSubclassOf(x) || elem.GetType() == x || x.IsAssignableFrom(elem.GetType())).ToList();

            var streams = new IElementStream[]
                {
                    new ElementPropertiesStream(m_app, data, elem),
                    new ElementMethodsStream(m_app, data, elem),
                    new SpatialElementStream(data, elem),
                    new FamilyTypeParameterValuesStream(data, elem),
                    new ExtensibleStorageEntityContentStream(m_app.ActiveUIDocument.Document, data, elem)
                };

            foreach (Type type in thisElementTypes)
            {
                data.Add(new Snoop.Data.ClassSeparator(type));

                foreach (var elementStream in streams)
                    elementStream.Stream(type);
            }
            
            StreamElementExtensibleStorages(data, elem as Element);

            StreamSimpleType(data, elem);
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

                    data.Add(new Data.Object(objectName, entity));
                }
                catch (System.Exception ex)
                {
                    data.Add(new Data.Exception(objectName, ex));
                }
            }
        }

        private void StreamSimpleType(ArrayList data, object elem)
        {
            var elemType = elem.GetType();

            if (elemType.IsEnum || elemType.IsPrimitive || elemType.IsValueType)
                data.Add(new Data.String($"{elemType.Name} value", elem.ToString()));
        }
    }
}

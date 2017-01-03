#region Header
//
// Copyright 2003-2017 by Autodesk, Inc. 
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
using System.Diagnostics;

using Autodesk.Revit.DB;


using RevitLookup.Snoop.Collectors;

namespace RevitLookup.Snoop.CollectorExts
{
	/// <summary>
	/// Provide Snoop.Data for any classes related to Parameters.
	/// </summary>
	
	public class CollectorExtParams : CollectorExt
	{
		public CollectorExtParams()
		{
		}

        protected override void
        CollectEvent(object sender, CollectorEventArgs e)
        {
                // cast the sender object to the SnoopCollector we are expecting
            Collector snoopCollector = sender as Collector;
            if (snoopCollector == null) {
                Debug.Assert(false);    // why did someone else send us the message?
                return;
            }

                // see if it is a type we are responsible for
			Parameter param = e.ObjToSnoop as Parameter;
			if (param != null) {
				Stream(snoopCollector.Data(), param);
				return;
			}

			Definition paramDef = e.ObjToSnoop as Definition;
			if (paramDef != null) {
				Stream(snoopCollector.Data(), paramDef);
				return;
			}
			
			DefinitionGroup defGroup = e.ObjToSnoop as DefinitionGroup;
			if (defGroup != null) {
				Stream(snoopCollector.Data(), defGroup);
				return;
			}

			DefinitionFile defFile = e.ObjToSnoop as DefinitionFile;
			if (defFile != null) {
				Stream(snoopCollector.Data(), defFile);
				return;
			}

			Binding binding = e.ObjToSnoop as Binding;
			if (binding != null) {
				Stream(snoopCollector.Data(), binding);
				return;
			}

			ElementBinding elemBind = e.ObjToSnoop as ElementBinding;
			if (elemBind != null) {
				Stream(snoopCollector.Data(), elemBind);
				return;
			}

            // no more in 2011?
            //ParameterListItem paramListItem = e.ObjToSnoop as ParameterListItem;
            //if (paramListItem != null) {
            //    Stream(snoopCollector.Data(), paramListItem);
            //    return;
            //}
        }
        
		private void
		Stream(ArrayList data, Parameter param)
		{
		    data.Add(new Snoop.Data.ClassSeparator(typeof(Parameter)));

            data.Add(new Snoop.Data.Object("Definition", param.Definition));

            try {   // this only works for certain types of Parameters
                data.Add(new Snoop.Data.String("Display unit type", param.DisplayUnitType.ToString()));
            }
            catch (System.Exception) {
                data.Add(new Snoop.Data.String("Display unit type", "N/A"));
            }
            try
            {   // this only works for certain types of Parameters
              data.Add(new Snoop.Data.Object("Element", param.Element));
            }
            catch (System.Exception ex)
            {
              data.Add(new Snoop.Data.Exception("Element", ex));
            }
            try
            {   // this only works for certain types of Parameters
              data.Add(new Snoop.Data.String("GUID", param.GUID.ToString()));
            }
            catch (System.Exception ex)
            {
              data.Add(new Snoop.Data.Exception("GUID", ex));
            }
            try
            {   // this only works for certain types of Parameters
              data.Add(new Snoop.Data.Bool("HasValue", param.HasValue));
            }
            catch (System.Exception ex)
            {
              data.Add(new Snoop.Data.Exception("HasValue", ex));
            }
            try
            {   // this only works for certain types of Parameters
              data.Add(new Snoop.Data.ElementId("ID", param.Id,m_activeDoc));
            }
            catch (System.Exception ex)
            {
              data.Add(new Snoop.Data.Exception("ID", ex));
            }
            try
            {   // this only works for certain types of Parameters
              data.Add(new Snoop.Data.Bool("IsShared", param.IsShared));
            }
            catch (System.Exception ex)
            {
              data.Add(new Snoop.Data.Exception("IsShared", ex));
            }

            data.Add(new Snoop.Data.String("Storage type", param.StorageType.ToString()));

			if (param.StorageType == StorageType.Double)
				data.Add(new Snoop.Data.Double("Value", param.AsDouble()));
			else if (param.StorageType == StorageType.ElementId)
				data.Add(new Snoop.Data.ElementId("Value", param.AsElementId(), m_app.ActiveUIDocument.Document));
			else if (param.StorageType == StorageType.Integer)
				data.Add(new Snoop.Data.Int("Value", param.AsInteger()));
			else if (param.StorageType == StorageType.String)
				data.Add(new Snoop.Data.String("Value", param.AsString()));

            data.Add(new Snoop.Data.String("As value string", param.AsValueString()));
        }

		private void
		Stream(ArrayList data, Definition paramDef)
		{
		    data.Add(new Snoop.Data.ClassSeparator(typeof(Definition)));

            data.Add(new Snoop.Data.String("Name", paramDef.Name));
            data.Add(new Snoop.Data.String("Parameter type", paramDef.ParameterType.ToString()));
            data.Add(new Snoop.Data.String("Parameter group", paramDef.ParameterGroup.ToString()));

			ExternalDefinition extDef = paramDef as ExternalDefinition;
			if (extDef != null) {
				Stream(data, extDef);
				return;
			}

			InternalDefinition intrnalDef = paramDef as InternalDefinition;
			if (intrnalDef != null) {
				Stream(data, intrnalDef);
				return;
			}
        }

		private void
		Stream(ArrayList data, ExternalDefinition extDef)
		{
		    data.Add(new Snoop.Data.ClassSeparator(typeof(ExternalDefinition)));

            data.Add(new Snoop.Data.String("GUID", extDef.GUID.ToString()));
            data.Add(new Snoop.Data.Object("Owner group", extDef.OwnerGroup));           
        }

		private void
		Stream(ArrayList data, InternalDefinition internalDef)
		{
		    data.Add(new Snoop.Data.ClassSeparator(typeof(InternalDefinition)));

            var values = (BuiltInParameter[])Enum.GetValues(typeof (BuiltInParameter));
		    string[] allNames = Enum.GetNames(typeof (BuiltInParameter));
		    var names = new System.Collections.Generic.List<string>();
		    for (int i = 0; i < values.Length; i++)
		    {
		        BuiltInParameter value = values[i];
		        if ((int) value != (int) internalDef.BuiltInParameter) continue;
                names.Add(allNames[i]);
		    }

		    data.Add(new Snoop.Data.String("Built in param", string.Join(", ", names)));				
        }

		private void
		Stream(ArrayList data, DefinitionFile defFile)
		{
		    data.Add(new Snoop.Data.ClassSeparator(typeof(DefinitionFile)));

            data.Add(new Snoop.Data.String("Filename", defFile.Filename));
            data.Add(new Snoop.Data.Object("Groups", defFile.Groups));
        }

		private void
		Stream(ArrayList data, DefinitionGroup defGroup)
		{
		    data.Add(new Snoop.Data.ClassSeparator(typeof(DefinitionGroup)));

            data.Add(new Snoop.Data.String("Name", defGroup.Name));
            data.Add(new Snoop.Data.Enumerable("Definitions", defGroup.Definitions));
        }

		private void
		Stream(ArrayList data, Binding binding)
		{
		    data.Add(new Snoop.Data.ClassSeparator(typeof(Binding)));

				// Nothing at this level yet!
        }

		private void
		Stream(ArrayList data, ElementBinding elemBind)
		{
		    data.Add(new Snoop.Data.ClassSeparator(typeof(ElementBinding)));

            data.Add(new Snoop.Data.Enumerable("Categories", elemBind.Categories));

			InstanceBinding instBind = elemBind as InstanceBinding;
			if (instBind != null) {
				Stream(data, instBind);
				return;
			}

			TypeBinding typeBind = elemBind as TypeBinding;
			if (typeBind != null) {
				Stream(data, typeBind);
				return;
			}
        }

		private void
		Stream(ArrayList data, InstanceBinding instBind)
		{
		    data.Add(new Snoop.Data.ClassSeparator(typeof(InstanceBinding)));

				// Nothing at this level yet!
        }

		private void
		Stream(ArrayList data, TypeBinding typeBind)
		{
		    data.Add(new Snoop.Data.ClassSeparator(typeof(TypeBinding)));

				// Nothing at this level yet!
        }

        // no more in 2011? MM
        //private void
        //Stream(ArrayList data, ParameterListItem paramListItem)
        //{
        //    data.Add(new Snoop.Data.ClassSeparator(typeof(ParameterListItem)));

        //    data.Add(new Snoop.Data.String("String", paramListItem.String));
        //    data.Add(new Snoop.Data.String("Value", paramListItem.Value));
        //}

	}
}

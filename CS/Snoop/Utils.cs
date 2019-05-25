#region Header
//
// Copyright 2003-2019 by Autodesk, Inc. 
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
using System.Diagnostics;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Printing;
using System.Reflection;
using Autodesk.Revit.DB;

namespace RevitLookup.Snoop
{

   public class Utils
   {
      public Utils()
      {

      }

      public static void
      Display(ListView lvCur, Snoop.Collectors.Collector snoopCollector)
      {
         lvCur.BeginUpdate();
         lvCur.Items.Clear();

         Font oldFont = lvCur.Font;
         FontStyle newStyle = lvCur.Font.Style ^ FontStyle.Bold;
         Font boldFont = new Font(oldFont, newStyle);

         for (int i = 0; i < snoopCollector.Data().Count; i++)
         {
            Snoop.Data.Data tmpSnoopData = (Snoop.Data.Data)snoopCollector.Data()[i];

            // if it is a class separator, then color the background differently
            // and don't add a SubItem for the "Field" value
            if (tmpSnoopData.IsSeparator)
            {
               ListViewItem lvItem = new ListViewItem(tmpSnoopData.StrValue());

               if (tmpSnoopData is Snoop.Data.ClassSeparator)
                  lvItem.BackColor = System.Drawing.Color.LightBlue;
               else
                  lvItem.BackColor = System.Drawing.Color.WhiteSmoke;

               lvItem.Tag = tmpSnoopData;
               lvCur.Items.Add(lvItem);
            }
            else
            {
               ListViewItem lvItem = new ListViewItem(tmpSnoopData.Label);
               lvItem.SubItems.Add(tmpSnoopData.StrValue());

               if (tmpSnoopData.IsError)
               {
                  ListViewItem.ListViewSubItem sItem = (ListViewItem.ListViewSubItem)lvItem.SubItems[0];
                  sItem.ForeColor = System.Drawing.Color.Red;
               }

               if (tmpSnoopData.HasDrillDown)
               {
                  ListViewItem.ListViewSubItem sItem = (ListViewItem.ListViewSubItem)lvItem.SubItems[0];
                  sItem.Font = boldFont;
               }

               lvItem.Tag = tmpSnoopData;
               lvCur.Items.Add(lvItem);
            }
         }

         lvCur.EndUpdate();
      }

      public static void
      DataItemSelected(ListView lvCur)
      {
         Debug.Assert((lvCur.SelectedItems.Count > 1) == false);

         if (lvCur.SelectedItems.Count != 0)
         {
            Snoop.Data.Data tmpSnoopData = (Snoop.Data.Data)lvCur.SelectedItems[0].Tag;
            tmpSnoopData.DrillDown();
         }
      }

      public static void
      BrowseReflection(Object obj)
      {
         if (obj == null)
         {
            MessageBox.Show("Object == null");
            return;
         }

         RevitLookup.Snoop.Forms.GenericPropGrid pgForm = new RevitLookup.Snoop.Forms.GenericPropGrid(obj);
         pgForm.Text = string.Format("Object Data (System.Type = {0})", obj.GetType().FullName);
         pgForm.ShowDialog();
      }

       private static string GetNamedObjectLabel(object obj)
       {
           var nameProperty = obj
               .GetType()
               .GetProperty("Name", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

           if (nameProperty != null)
           {
               var propertyValue = nameProperty.GetValue(obj) as string;

               return string.Format("< {0}  {1} >", obj.GetType().Name,
                   string.IsNullOrEmpty(propertyValue) ? "???" : propertyValue);
           }

           return null;
       }

       public static string GetParameterObjectLabel(object obj)
       {
           var parameter = obj as Parameter;

           return parameter?.Definition.Name;
       }

       public static string ObjToLabelStr(object obj)
       {
           if (obj == null)
               return "< null >";

           var elem = obj as Element;

           if (elem != null)
           {
               // TBD: Exceptions are thrown in certain cases when accessing the Name property. 
               // Eg. for the RoomTag object. So we will catch the exception and display the exception message
               // arj - 1/23/07
               try
               {
                   var nameStr = (elem.Name == string.Empty) ? "???" : elem.Name; // use "???" if no name is set
                   return $"< {nameStr}  {elem.Id.IntegerValue} >";
               }
               catch (InvalidOperationException ex)
               {
                   return $"< {null}  {ex.Message} >";
               }
           }

           return GetNamedObjectLabel(obj) ?? GetParameterObjectLabel(obj) ?? $"< {obj.GetType().Name} >";
       }

       public static void
      CopyToClipboard(ListView lv)
      {
         if (lv.Items.Count == 0)
         {
            Clipboard.Clear();
            return;
         }

         //First find the longest piece of text in the Field column
         //
         Int32 maxField = 0;
         Int32 maxValue = 0;

         foreach (ListViewItem item in lv.Items)
         {
            if (item.Text.Length > maxField)
            {
               maxField = item.Text.Length;
            }
            if ((item.SubItems.Count > 1) && (item.SubItems[1].Text.Length > maxValue))
            {
               maxValue = item.SubItems[1].Text.Length;
            }
         }

         String headerFormat = String.Format("{{0,{0}}}----{1}\r\n", maxField, new String('-', maxValue));
         String tabbedFormat = String.Format("{{0,{0}}}    {{1}}\r\n", maxField);

         System.Text.StringBuilder bldr = new System.Text.StringBuilder();

         foreach (ListViewItem item in lv.Items)
         {
            if (item.SubItems.Count == 1)
            {
               String tmp = item.Text;
               if (item.Text.Length < maxField)
               {
                  tmp = item.Text.PadLeft(item.Text.Length + (maxField - item.Text.Length), '-');
               }

               bldr.AppendFormat(headerFormat, tmp);
            }
            else if (item.SubItems.Count > 1)
            {
               bldr.AppendFormat(tabbedFormat, item.Text, item.SubItems[1].Text);
            }
         }

         String txt = bldr.ToString();
         if (String.IsNullOrEmpty(txt) == false)
         {
            Clipboard.SetDataObject(txt);
         }
      }

      public static void
      CopyToClipboard(ListViewItem lvItem, Boolean multipleItems)
      {
         if (lvItem.SubItems.Count > 1)
         {
            if (!multipleItems)
            {
               Clipboard.SetDataObject(lvItem.SubItems[1].Text);
            }
            else
            {
               Clipboard.SetDataObject(lvItem.SubItems[0].Text + " => " + lvItem.SubItems[1].Text);
            }
         }
         else
         {
            Clipboard.Clear();
         }

      }

      public static Int32
      Print(String title, ListView lv, System.Drawing.Printing.PrintPageEventArgs e, Int32 maxFieldWidth, Int32 maxValueWidth, Int32 currentItem)
      {
         Single linesPerPage = 0;
         Single yPos = 0;
         Single leftMargin = e.MarginBounds.Left + ((e.MarginBounds.Width - (maxFieldWidth + maxValueWidth)) / 2);
         Single topMargin = e.MarginBounds.Top;
         Single fontHeight = lv.Font.GetHeight(e.Graphics);
         Int32 count = 0;
         String line = null;
         ListViewItem item;
         SolidBrush backgroundBrush;
         SolidBrush subbackgroundBrush;
         SolidBrush textBrush;
         RectangleF rect;
         StringFormat centerFormat = new StringFormat();
         StringFormat fieldFormat = new StringFormat();
         StringFormat valueFormat = new StringFormat();

         centerFormat.Alignment = StringAlignment.Center;
         fieldFormat.Alignment = HorizontalAlignmentToStringAligment(lv.Columns[0].TextAlign);
         valueFormat.Alignment = HorizontalAlignmentToStringAligment(lv.Columns[1].TextAlign);

         //Draw the title of the list.
         //
         rect = new RectangleF(leftMargin, topMargin, maxFieldWidth + maxValueWidth, fontHeight);
         e.Graphics.DrawString(title, lv.Font, Brushes.Black, rect, centerFormat);

         //Update the count so that we are giving ourselves a line between the title and the list.
         //
         count = 2;

         //Calculate the number of lines per page
         //
         linesPerPage = e.MarginBounds.Height / fontHeight;

         while ((count < linesPerPage) && (currentItem < lv.Items.Count))
         {
            item = lv.Items[currentItem];
            line = item.Text;
            yPos = topMargin + (count * fontHeight);

            backgroundBrush = new SolidBrush(item.BackColor);
            textBrush = new SolidBrush(item.ForeColor);

            rect = new RectangleF(leftMargin, yPos, maxFieldWidth, fontHeight);

            e.Graphics.FillRectangle(backgroundBrush, rect);
            e.Graphics.DrawRectangle(Pens.Black, rect.X, rect.Y, rect.Width, rect.Height);

            //Draw the field portion of the list view item
            //
            e.Graphics.DrawString(" " + item.Text, item.Font, textBrush, rect, fieldFormat);

            //Draw the value portion of the list view item
            //
            rect = new RectangleF(leftMargin + maxFieldWidth, yPos, maxValueWidth, fontHeight);
            if (item.SubItems.Count > 1)
            {
               subbackgroundBrush = new SolidBrush(item.SubItems[1].BackColor);

               e.Graphics.FillRectangle(subbackgroundBrush, rect);
               e.Graphics.DrawString(" " + item.SubItems[1].Text, item.Font, textBrush, rect, valueFormat);
            }
            else
            {
               e.Graphics.FillRectangle(backgroundBrush, rect);
            }
            e.Graphics.DrawRectangle(Pens.Black, rect.X, rect.Y, rect.Width, rect.Height);

            count++;
            currentItem++;
         }

         if (currentItem < lv.Items.Count)
         {
            e.HasMorePages = true;
         }
         else
         {
            e.HasMorePages = false;
            currentItem = 0;
         }

         return currentItem;
      }

      public static StringAlignment
      HorizontalAlignmentToStringAligment(HorizontalAlignment ha)
      {
         switch (ha)
         {
            case HorizontalAlignment.Center:
               return StringAlignment.Center;
            case HorizontalAlignment.Left:
               return StringAlignment.Near;
            case HorizontalAlignment.Right:
               return StringAlignment.Far;
            default:
               return StringAlignment.Near;
         }
      }

      public static Int32[]
      GetMaximumColumnWidths(ListView lv)
      {
         Int32 index = 0;
         Int32[] widthArray = new Int32[lv.Columns.Count];

         foreach (ColumnHeader col in lv.Columns)
         {
            widthArray[index] = col.Width;
            index++;
         }

         System.Drawing.Graphics g = lv.CreateGraphics();
         Int32 offset = Convert.ToInt32(Math.Ceiling(g.MeasureString(" ", lv.Font).Width));
         Int32 width = 0;

         foreach (ListViewItem item in lv.Items)
         {
            index = 0;

            foreach (ListViewItem.ListViewSubItem subItem in item.SubItems)
            {
               width = Convert.ToInt32(Math.Ceiling(g.MeasureString(subItem.Text, item.Font).Width)) + offset;
               if (width > widthArray[index])
               {
                  widthArray[index] = width;
               }
               index++;
            }
         }

         g.Dispose();

         return widthArray;
      }

      public static TreeNode
      GetRootNode(TreeNode node)
      {
         if (node.Parent == null)
         {
            return node;
         }
         return GetRootNode(node.Parent);
      }

      public static String
      GetPrintDocumentName(TreeNode node)
      {
         TreeNode root = GetRootNode(node);
         String str = root.Tag as String;

         if (str != null)
         {
            return System.IO.Path.GetFileNameWithoutExtension((String)root.Tag);
         }
         return String.Empty;
      }

      public static void
      UpdatePrintSettings(PrintDocument doc, TreeView tv, ListView lv, ref Int32[] widthArray)
      {
         if (tv.SelectedNode == null)
         {
            return;
         }
         doc.DocumentName = Utils.GetPrintDocumentName(tv.SelectedNode);
         widthArray = Utils.GetMaximumColumnWidths(lv);
      }

      public static void
      UpdatePrintSettings(ListView lv, ref Int32[] widthArray)
      {
         widthArray = Utils.GetMaximumColumnWidths(lv);
      }

      public static void
      PrintMenuItemClick(PrintDialog dlg, TreeView tv)
      {
         if (tv.SelectedNode == null)
         {
            MessageBox.Show(tv.Parent, "Please select a node in the tree to print.", "RevitLookup", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
         }

         if (dlg.ShowDialog(tv.Parent) == DialogResult.OK)
         {
            dlg.Document.Print();
         }
      }

      public static void
      PrintMenuItemClick(PrintDialog dlg)
      {
         dlg.Document.Print();
      }

      public static void
      PrintPreviewMenuItemClick(PrintPreviewDialog dlg, TreeView tv)
      {
         if (tv.SelectedNode == null)
         {
            MessageBox.Show(tv.Parent, "Please select a node in the tree to print.", "RevitLookup", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
         }

         dlg.ShowDialog(tv.Parent);
      }

      public static void
      PrintPreviewMenuItemClick(PrintPreviewDialog dlg, ListView lv)
      {
         dlg.ShowDialog(lv.Parent);
      }

   }
}


// Copyright 2003-2022 by Autodesk, Inc.
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

using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using RevitLookup.Core.Objects;

namespace RevitLookup.Views.Utils;

public static class VisualUtils
{
    //https://learn.microsoft.com/ru-ru/dotnet/desktop/wpf/controls/how-to-find-a-treeviewitem-in-a-treeview?view=netframeworkdesktop-4.8
    public static TreeViewItem GetTreeViewItem(ItemsControl container, SnoopableObject item)
    {
        if (container == null) return null;
        if (item is null) return null;
        if (container.DataContext == item) return container as TreeViewItem;
        if (container.Items.Count == 0) return null;

        if (container is TreeViewItem {IsExpanded: false} viewItem)
        {
            viewItem.SetValue(TreeViewItem.IsExpandedProperty, true);
        }

        container.ApplyTemplate();
        var itemsPresenter = (ItemsPresenter) container.Template.FindName("ItemsHost", container);
        if (itemsPresenter != null)
        {
            itemsPresenter.ApplyTemplate();
        }
        else
        {
            itemsPresenter = FindVisualChild<ItemsPresenter>(container);
            if (itemsPresenter == null)
            {
                container.UpdateLayout();
                itemsPresenter = FindVisualChild<ItemsPresenter>(container);
            }
        }

        var itemsHostPanel = (Panel) VisualTreeHelper.GetChild(itemsPresenter, 0);
        var virtualizingPanel = (VirtualizingStackPanel) itemsHostPanel;

        for (int i = 0, count = container.Items.Count; i < count; i++)
        {
            virtualizingPanel.BringIndexIntoViewPublic(i);
            var subContainer = (TreeViewItem) container.ItemContainerGenerator.ContainerFromIndex(i);
            if (subContainer == null) continue;
            if (subContainer.DataContext is CollectionViewGroup viewGroup)
                if ((string) viewGroup.Name != item.Descriptor.Type)
                    continue;

            var resultContainer = GetTreeViewItem(subContainer, item);
            if (resultContainer != null)
            {
                return resultContainer;
            }

            subContainer.IsExpanded = false;
        }

        return null;
    }

    public static T FindVisualChild<T>(Visual visual) where T : Visual
    {
        for (var i = 0; i < VisualTreeHelper.GetChildrenCount(visual); i++)
        {
            var child = (Visual) VisualTreeHelper.GetChild(visual, i);
            if (child is T correctlyTyped)
            {
                return correctlyTyped;
            }

            var descendent = FindVisualChild<T>(child);
            if (descendent != null)
            {
                return descendent;
            }
        }

        return null;
    }
}
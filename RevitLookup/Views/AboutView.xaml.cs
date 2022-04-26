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

using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Navigation;

namespace RevitLookup.Views;

public partial class AboutView
{
    public AboutView()
    {
        InitializeComponent();
        UpdateContent();
        EventManager.RegisterClassHandler(typeof(Hyperlink), Hyperlink.RequestNavigateEvent, new RequestNavigateEventHandler(Hyperlink_OnClick));
    }

    private void UpdateContent()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var assemblyName = assembly.GetName();
        VersionRun.Text = assemblyName.Version.ToString();
    }

    private void AboutView_OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs eventArgs)
    {
        if (eventArgs.Source is Run) return;
        Close();
    }

    private static void Hyperlink_OnClick(object sender, RequestNavigateEventArgs eventArgs)
    {
        Process.Start(eventArgs.Uri.AbsoluteUri);
        eventArgs.Handled = true;
    }
}
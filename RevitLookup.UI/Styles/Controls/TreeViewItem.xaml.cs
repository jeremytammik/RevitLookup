// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows;

namespace Wpf.Ui.Styles.Controls
{
    partial class TreeViewItem
    {
        public void OnRequestBringIntoView(object sender, RequestBringIntoViewEventArgs args)
        {
            args.Handled = true;
        }
    }
}
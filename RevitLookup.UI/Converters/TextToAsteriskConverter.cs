// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Globalization;
using System.Windows.Data;

namespace RevitLookup.UI.Converters;

internal class TextToAsteriskConverter : IValueConverter
{
    /// <summary>
    ///     Converts <see langword="string" /> to <see langword="*" />.
    /// </summary>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return new string('*', value?.ToString()?.Length ?? 0);
    }

    /// <summary>
    ///     Not Implemented.
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
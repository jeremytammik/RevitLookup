// Copyright 2003-2024 by Autodesk, Inc.
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

using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using RevitLookup.ViewModels.ObservableObjects;

namespace RevitLookup.Core.Modules.Configuration;

[SuppressMessage("ReSharper", "ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator")]
public sealed class RevitConfigurator
{
    private const char CommentChar = ';';
    private const string SessionOptionsCategory = "[Jrn.SessionOptions]";
    private const string RevitAttributeRecord = " Rvt.Attr.";

    private readonly Encoding _encoding;

    public RevitConfigurator()
    {
#if NETCOREAPP
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
#endif
        _encoding = Encoding.GetEncoding(1251);
    }

    public List<ObservableRevitSettingsEntry> ParseSources()
    {
        var userIniPath = Context.Application.CurrentUsersDataFolderPath.AppendPath("Revit.ini");
        var localConfigurations = ParseLocalSource(userIniPath);

        if (localConfigurations.Count == 0)
        {
            var defaultIniPath = Environment
                .GetFolderPath(Environment.SpecialFolder.CommonApplicationData)
                .AppendPath("Autodesk", $"RVT {Context.Application.VersionNumber}", "UserDataCache", "Revit.ini");

            localConfigurations = ParseLocalSource(defaultIniPath);
        }

        var validConfigurations = ParseJournalSource();
        return MergeSources(validConfigurations, localConfigurations);
    }

    private List<ObservableRevitSettingsEntry> ParseJournalSource()
    {
        var journalsPath = Directory.GetParent(Context.Application.RecordingJournalFilename)!;

        foreach (var journal in Directory.EnumerateFiles(journalsPath.FullName).Reverse().Skip(2)) //We can skip a log already in use
        {
            var lines = File.ReadLines(journal, _encoding);
            foreach (var sessionOptions in lines.Reverse())
            {
                if (sessionOptions.Contains(SessionOptionsCategory))
                {
                    var startIndex = sessionOptions.IndexOf(SessionOptionsCategory, StringComparison.Ordinal) + SessionOptionsCategory.Length;
                    var optionsPart = sessionOptions[startIndex..];

                    var parts = optionsPart.Split([RevitAttributeRecord], StringSplitOptions.RemoveEmptyEntries)
                        .Select(line => line.Trim())
                        .ToArray();

                    var sections = new List<ObservableRevitSettingsEntry>();
                    foreach (var part in parts)
                    {
                        var keyValue = part.Split([':'], 2);
                        var keyParts = keyValue[0].Split('.');

                        var section = keyParts[0];
                        var entry = keyParts[1];
                        var value = keyValue[1].Trim();

                        sections.Add(new ObservableRevitSettingsEntry
                        {
                            Category = section,
                            Property = entry,
                            Value = value
                        });
                    }

                    return sections;
                }
            }
        }

        return [];
    }

    private List<ObservableRevitSettingsEntry> ParseLocalSource(string path)
    {
        var entries = new List<ObservableRevitSettingsEntry>();
        if (!File.Exists(path)) return entries;

        var lines = File.ReadLines(path, _encoding);
        var currentCategory = string.Empty;

        foreach (var line in lines)
        {
            var trimmedLine = line.Trim();
            if (string.IsNullOrWhiteSpace(trimmedLine)) continue;

            if (trimmedLine.StartsWith("[") && trimmedLine.EndsWith("]"))
            {
                currentCategory = trimmedLine.Trim('[', ']');
                continue;
            }

            var isActive = !trimmedLine.StartsWith(CommentChar.ToString());
            if (!isActive)
            {
                trimmedLine = trimmedLine.TrimStart(CommentChar).Trim();
            }

            var keyValue = trimmedLine.Split(['='], 2);
            if (keyValue.Length != 2) continue;

            var property = keyValue[0].Trim();
            var value = keyValue[1].Trim();

            entries.Add(new ObservableRevitSettingsEntry
            {
                Category = currentCategory,
                Property = property,
                Value = value,
                DefaultValue = value,
                IsActive = isActive
            });
        }

        return entries;
    }

    private List<ObservableRevitSettingsEntry> MergeSources(List<ObservableRevitSettingsEntry> source, List<ObservableRevitSettingsEntry> extraSource)
    {
        if (source.Count == 0 && extraSource.Count == 0) return [];

        foreach (var extraEntry in extraSource)
        {
            ObservableRevitSettingsEntry existingEntry = null;
            foreach (var entry in source)
            {
                if (entry.Category == extraEntry.Category && entry.Property == extraEntry.Property)
                {
                    existingEntry = entry;
                    break;
                }
            }

            if (existingEntry != null)
            {
                existingEntry.DefaultValue = extraEntry.Value;
                existingEntry.IsActive = extraEntry.IsActive;
            }
            else
            {
                source.Add(extraEntry);
            }
        }

        return source;
    }
}
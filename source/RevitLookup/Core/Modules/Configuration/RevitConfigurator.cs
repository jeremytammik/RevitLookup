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
        var defaultIniPath = Environment
            .GetFolderPath(Environment.SpecialFolder.CommonApplicationData)
            .AppendPath("Autodesk", $"RVT {Context.Application.VersionNumber}", "UserDataCache", "Revit.ini");

        var journalConfigurations = ParseJournalSource();
        var userConfigurations = ParseIniFile(userIniPath, false);
        var defaultConfigurations = ParseIniFile(defaultIniPath, true);

        return MergeSources(journalConfigurations, userConfigurations, defaultConfigurations);
    }

    private List<ObservableRevitSettingsEntry> ParseJournalSource()
    {
        var currentJournal = Context.Application.RecordingJournalFilename;
        var journalsPath = Directory.GetParent(currentJournal)!;
        var journals = Directory.EnumerateFiles(journalsPath.FullName, "journal*txt").Reverse();
        
        foreach (var journal in journals)
        {
            if (journal == currentJournal) continue;
            
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

                    var entries = new List<ObservableRevitSettingsEntry>();
                    foreach (var part in parts)
                    {
                        var keyValue = part.Split([':'], 2);
                        var keyParts = keyValue[0].Split('.');

                        var section = keyParts[0];
                        var entry = keyParts[1];
                        var value = keyValue[1].Trim();

                        entries.Add(new ObservableRevitSettingsEntry
                        {
                            Category = section,
                            Property = entry,
                            Value = value
                        });
                    }

                    return entries;
                }
            }
        }

        return [];
    }

    private List<ObservableRevitSettingsEntry> ParseIniFile(string path, bool isDefault)
    {
        if (!File.Exists(path)) return [];
        
        var entries = new List<ObservableRevitSettingsEntry>();
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

            var entry = new ObservableRevitSettingsEntry
            {
                Category = currentCategory,
                Property = property,
                Value = value
            };

            if (isDefault)
            {
                entry.DefaultValue = value;
            }
            else
            {
                entry.IsActive = isActive;
            }

            entries.Add(entry);
        }

        return entries;
    }

    private List<ObservableRevitSettingsEntry> MergeSources(
        List<ObservableRevitSettingsEntry> journalEntries, 
        List<ObservableRevitSettingsEntry> userEntries, 
        List<ObservableRevitSettingsEntry> defaultEntries)
    {
        foreach (var userEntry in userEntries)
        {
            var existingEntry = journalEntries.FirstOrDefault(entry => entry.Category == userEntry.Category && entry.Property == userEntry.Property);
            if (existingEntry != null)
            {
                existingEntry.IsActive = userEntry.IsActive;
            }
            else
            {
                journalEntries.Add(userEntry);
            }
        }

        foreach (var defaultEntry in defaultEntries)
        {
            var existingEntry = journalEntries.FirstOrDefault(e => e.Category == defaultEntry.Category && e.Property == defaultEntry.Property);
            if (existingEntry != null)
            {
                existingEntry.DefaultValue = defaultEntry.DefaultValue;
            }
            else
            {
                journalEntries.Add(defaultEntry);
            }
        }

        return journalEntries;
    }
}
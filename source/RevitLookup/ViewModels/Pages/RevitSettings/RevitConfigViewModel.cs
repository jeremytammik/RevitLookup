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

using System.IO;
using System.Text;
using Microsoft.Extensions.Logging;

namespace RevitLookup.ViewModels.Pages.RevitSettings;

public sealed partial class RevitConfigViewModel : ObservableObject
{
    private readonly ILogger<RevitConfigViewModel> _logger;
    public List<ObservableRevitConfigEntry> Entries { get; private set; }

    public RevitConfigViewModel(ILogger<RevitConfigViewModel> logger)
    {
        _logger = logger;
        InitializeAsync();
    }

    private async void InitializeAsync()
    {
        Entries = await Task.Run(() =>
        {
            var encoding = Encoding.GetEncoding("windows-1251");
            var journalsPath = Directory.GetParent(Context.Application.RecordingJournalFilename)!;
            
            foreach (var journal in Directory.EnumerateFiles(journalsPath.FullName).Reverse().Skip(1)) //We can skip a log already in use
            {
                try
                {
                    var lines = File.ReadLines(journal, encoding);
                    foreach (var sessionOptions in lines)
                    {
                        if (sessionOptions.Contains("[Jrn.SessionOptions]"))
                        {
                            var startIndex = sessionOptions.IndexOf("[Jrn.SessionOptions]", StringComparison.Ordinal) + "[Jrn.SessionOptions]".Length;
                            var optionsPart = sessionOptions[startIndex..];

                            var parts = optionsPart.Split([" Rvt.Attr."], StringSplitOptions.RemoveEmptyEntries)
                                .Select(line => line.Trim())
                                .ToArray();

                            var sections = new List<ObservableRevitConfigEntry>();
                            foreach (var part in parts)
                            {
                                var keyValue = part.Split([':'], 2);
                                var keyParts = keyValue[0].Split('.');

                                var section = keyParts[0];
                                var entry = keyParts[1];
                                var value = keyValue[1].Trim();

                                sections.Add(new ObservableRevitConfigEntry
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
                catch (Exception exception)
                {
                    _logger.LogError(exception, "Unavailable to read log file");
                }
            }

            return [];
        });
    }

    [RelayCommand]
    private void DeleteEntry(ObservableRevitConfigEntry entry)
    {
        Entries.Remove(entry);
    }
}
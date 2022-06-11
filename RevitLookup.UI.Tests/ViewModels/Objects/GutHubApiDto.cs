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

using Newtonsoft.Json;

namespace RevitLookup.UI.Tests.ViewModels.Objects;

public class GutHubApiDto
{
    [JsonProperty("html_url")] public string Url { get; set; }
    [JsonProperty("tag_name")] public string TagName { get; set; }
    [JsonProperty("published_at")] public DateTimeOffset PublishedDate { get; set; }
    [JsonProperty("assets")] public List<AssetDto> Assets { get; set; }
}

public class AssetDto
{
    [JsonProperty("browser_download_url")] public string DownloadUrl { get; set; }
}
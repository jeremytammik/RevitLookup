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

using System.Globalization;
using Color = System.Drawing.Color;

namespace RevitLookup.Utils;

/// <summary>
///     Helper class to easier work with color representation
/// </summary>
/// <remarks>
///     Implementation: https://github.com/microsoft/PowerToys/blob/main/src/modules/colorPicker/ColorPickerUI/Helpers/ColorRepresentationHelper.cs
/// </remarks>
public static class ColorRepresentationUtils
{
    private static readonly Dictionary<string, string> KnownColors = new()
    {
        {"000000", Resources.Localization.Colors._000000},
        {"000080", Resources.Localization.Colors._000080},
        {"0000FF", Resources.Localization.Colors._0000FF},
        {"0018A8", Resources.Localization.Colors._0018A8},
        {"002E63", Resources.Localization.Colors._002E63},
        {"003153", Resources.Localization.Colors._003153},
        {"003366", Resources.Localization.Colors._003366},
        {"003399", Resources.Localization.Colors._003399},
        {"003A6C", Resources.Localization.Colors._003A6C},
        {"004225", Resources.Localization.Colors._004225},
        {"0047AB", Resources.Localization.Colors._0047AB},
        {"0048BA", Resources.Localization.Colors._0048BA},
        {"00563F", Resources.Localization.Colors._00563F},
        {"006A4E", Resources.Localization.Colors._006A4E},
        {"006B3C", Resources.Localization.Colors._006B3C},
        {"007BA7", Resources.Localization.Colors._007BA7},
        {"007DFF", Resources.Localization.Colors._007DFF},
        {"007FFF", Resources.Localization.Colors._007FFF},
        {"008080", Resources.Localization.Colors._008080},
        {"0088DC", Resources.Localization.Colors._0088DC},
        {"0093AF", Resources.Localization.Colors._0093AF},
        {"0095B6", Resources.Localization.Colors._0095B6},
        {"00A86B", Resources.Localization.Colors._00A86B},
        {"00B9FB", Resources.Localization.Colors._00B9FB},
        {"00BFFF", Resources.Localization.Colors._00BFFF},
        {"00C4B0", Resources.Localization.Colors._00C4B0},
        {"00CC99", Resources.Localization.Colors._00CC99},
        {"00CCCC", Resources.Localization.Colors._00CCCC},
        {"00FF00", Resources.Localization.Colors._00FF00},
        {"00FF7F", Resources.Localization.Colors._00FF7F},
        {"00FFFF", Resources.Localization.Colors._00FFFF},
        {"013220", Resources.Localization.Colors._013220},
        {"01796F", Resources.Localization.Colors._01796F},
        {"03C03C", Resources.Localization.Colors._03C03C},
        {"064E40", Resources.Localization.Colors._064E40},
        {"082567", Resources.Localization.Colors._082567},
        {"08457E", Resources.Localization.Colors._08457E},
        {"08E8DE", Resources.Localization.Colors._08E8DE},
        {"0A1195", Resources.Localization.Colors._0A1195},
        {"0BDA51", Resources.Localization.Colors._0BDA51},
        {"0D98BA", Resources.Localization.Colors._0D98BA},
        {"116062", Resources.Localization.Colors._116062},
        {"120A8F", Resources.Localization.Colors._120A8F},
        {"126180", Resources.Localization.Colors._126180},
        {"141414", Resources.Localization.Colors._141414},
        {"1560BD", Resources.Localization.Colors._1560BD},
        {"177245", Resources.Localization.Colors._177245},
        {"1974D2", Resources.Localization.Colors._1974D2},
        {"1A4780", Resources.Localization.Colors._1A4780},
        {"1B1811", Resources.Localization.Colors._1B1811},
        {"1B4D3E", Resources.Localization.Colors._1B4D3E},
        {"1C6B72", Resources.Localization.Colors._1C6B72},
        {"1DACD6", Resources.Localization.Colors._1DACD6},
        {"1E90FF", Resources.Localization.Colors._1E90FF},
        {"1F4037", Resources.Localization.Colors._1F4037},
        {"1F75FE", Resources.Localization.Colors._1F75FE},
        {"21ABCD", Resources.Localization.Colors._21ABCD},
        {"232B2B", Resources.Localization.Colors._232B2B},
        {"246BCE", Resources.Localization.Colors._246BCE},
        {"24A0ED", Resources.Localization.Colors._24A0ED},
        {"253529", Resources.Localization.Colors._253529},
        {"2A52BE", Resources.Localization.Colors._2A52BE},
        {"2A8FBD", Resources.Localization.Colors._2A8FBD},
        {"2E2D88", Resources.Localization.Colors._2E2D88},
        {"2E5894", Resources.Localization.Colors._2E5894},
        {"2E8B57", Resources.Localization.Colors._2E8B57},
        {"2F4F4F", Resources.Localization.Colors._2F4F4F},
        {"30D5C8", Resources.Localization.Colors._30D5C8},
        {"310062", Resources.Localization.Colors._310062},
        {"318CE7", Resources.Localization.Colors._318CE7},
        {"333399", Resources.Localization.Colors._333399},
        {"3399FF", Resources.Localization.Colors._3399FF},
        {"34B334", Resources.Localization.Colors._34B334},
        {"34C924", Resources.Localization.Colors._34C924},
        {"36454F", Resources.Localization.Colors._36454F},
        {"365194", Resources.Localization.Colors._365194},
        {"391802", Resources.Localization.Colors._391802},
        {"3A75C4", Resources.Localization.Colors._3A75C4},
        {"3B2F2F", Resources.Localization.Colors._3B2F2F},
        {"3B3B6D", Resources.Localization.Colors._3B3B6D},
        {"3B3C36", Resources.Localization.Colors._3B3C36},
        {"3B444B", Resources.Localization.Colors._3B444B},
        {"3B7A57", Resources.Localization.Colors._3B7A57},
        {"3C1421", Resources.Localization.Colors._3C1421},
        {"3C3024", Resources.Localization.Colors._3C3024},
        {"3C8D0D", Resources.Localization.Colors._3C8D0D},
        {"3CAA3C", Resources.Localization.Colors._3CAA3C},
        {"3D0C02", Resources.Localization.Colors._3D0C02},
        {"3D2B1F", Resources.Localization.Colors._3D2B1F},
        {"3F000F", Resources.Localization.Colors._3F000F},
        {"40826D", Resources.Localization.Colors._40826D},
        {"4169E1", Resources.Localization.Colors._4169E1},
        {"423189", Resources.Localization.Colors._423189},
        {"442D25", Resources.Localization.Colors._442D25},
        {"45161C", Resources.Localization.Colors._45161C},
        {"464451", Resources.Localization.Colors._464451},
        {"465945", Resources.Localization.Colors._465945},
        {"4682B4", Resources.Localization.Colors._4682B4},
        {"480607", Resources.Localization.Colors._480607},
        {"4A2C2A", Resources.Localization.Colors._4A2C2A},
        {"4AFF00", Resources.Localization.Colors._4AFF00},
        {"4B0082", Resources.Localization.Colors._4B0082},
        {"4B3621", Resources.Localization.Colors._4B3621},
        {"4B5320", Resources.Localization.Colors._4B5320},
        {"4C2F27", Resources.Localization.Colors._4C2F27},
        {"4C5866", Resources.Localization.Colors._4C5866},
        {"4D1A7F", Resources.Localization.Colors._4D1A7F},
        {"4E1609", Resources.Localization.Colors._4E1609},
        {"4F7942", Resources.Localization.Colors._4F7942},
        {"4F86F7", Resources.Localization.Colors._4F86F7},
        {"5072A7", Resources.Localization.Colors._5072A7},
        {"50C878", Resources.Localization.Colors._50C878},
        {"54626F", Resources.Localization.Colors._54626F},
        {"551B8C", Resources.Localization.Colors._551B8C},
        {"553592", Resources.Localization.Colors._553592},
        {"556832", Resources.Localization.Colors._556832},
        {"560319", Resources.Localization.Colors._560319},
        {"568203", Resources.Localization.Colors._568203},
        {"56A0D3", Resources.Localization.Colors._56A0D3},
        {"58111A", Resources.Localization.Colors._58111A},
        {"592720", Resources.Localization.Colors._592720},
        {"5D2B2C", Resources.Localization.Colors._5D2B2C},
        {"5D8AA8", Resources.Localization.Colors._5D8AA8},
        {"5DA130", Resources.Localization.Colors._5DA130},
        {"5DADEC", Resources.Localization.Colors._5DADEC},
        {"5F1933", Resources.Localization.Colors._5F1933},
        {"630F0F", Resources.Localization.Colors._630F0F},
        {"63775B", Resources.Localization.Colors._63775B},
        {"6495ED", Resources.Localization.Colors._6495ED},
        {"654321", Resources.Localization.Colors._654321},
        {"660000", Resources.Localization.Colors._660000},
        {"660066", Resources.Localization.Colors._660066},
        {"660099", Resources.Localization.Colors._660099},
        {"6600FF", Resources.Localization.Colors._6600FF},
        {"663398", Resources.Localization.Colors._663398},
        {"665D1E", Resources.Localization.Colors._665D1E},
        {"6699CC", Resources.Localization.Colors._6699CC},
        {"66B447", Resources.Localization.Colors._66B447},
        {"66FF00", Resources.Localization.Colors._66FF00},
        {"6B4423", Resources.Localization.Colors._6B4423},
        {"6B8E23", Resources.Localization.Colors._6B8E23},
        {"6E7F80", Resources.Localization.Colors._6E7F80},
        {"702963", Resources.Localization.Colors._702963},
        {"703642", Resources.Localization.Colors._703642},
        {"704214", Resources.Localization.Colors._704214},
        {"708090", Resources.Localization.Colors._708090},
        {"720B98", Resources.Localization.Colors._720B98},
        {"72A0C1", Resources.Localization.Colors._72A0C1},
        {"734A12", Resources.Localization.Colors._734A12},
        {"735184", Resources.Localization.Colors._735184},
        {"7366BD", Resources.Localization.Colors._7366BD},
        {"737000", Resources.Localization.Colors._737000},
        {"755A57", Resources.Localization.Colors._755A57},
        {"77DD77", Resources.Localization.Colors._77DD77},
        {"78866B", Resources.Localization.Colors._78866B},
        {"79443B", Resources.Localization.Colors._79443B},
        {"79A0C1", Resources.Localization.Colors._79A0C1},
        {"7B3F00", Resources.Localization.Colors._7B3F00},
        {"7B917B", Resources.Localization.Colors._7B917B},
        {"7BA05B", Resources.Localization.Colors._7BA05B},
        {"7BB661", Resources.Localization.Colors._7BB661},
        {"7C0A02", Resources.Localization.Colors._7C0A02},
        {"7CB9E8", Resources.Localization.Colors._7CB9E8},
        {"7DF9FF", Resources.Localization.Colors._7DF9FF},
        {"7F1734", Resources.Localization.Colors._7F1734},
        {"7F3E98", Resources.Localization.Colors._7F3E98},
        {"7FC7FF", Resources.Localization.Colors._7FC7FF},
        {"7FFF00", Resources.Localization.Colors._7FFF00},
        {"7FFFD4", Resources.Localization.Colors._7FFFD4},
        {"800000", Resources.Localization.Colors._800000},
        {"800020", Resources.Localization.Colors._800020},
        {"804040", Resources.Localization.Colors._804040},
        {"808000", Resources.Localization.Colors._808000},
        {"808080", Resources.Localization.Colors._808080},
        {"81613C", Resources.Localization.Colors._81613C},
        {"834D18", Resources.Localization.Colors._834D18},
        {"841B2D", Resources.Localization.Colors._841B2D},
        {"848482", Resources.Localization.Colors._848482},
        {"84DE02", Resources.Localization.Colors._84DE02},
        {"856088", Resources.Localization.Colors._856088},
        {"87413F", Resources.Localization.Colors._87413F},
        {"87A96B", Resources.Localization.Colors._87A96B},
        {"884535", Resources.Localization.Colors._884535},
        {"893F45", Resources.Localization.Colors._893F45},
        {"89CFF0", Resources.Localization.Colors._89CFF0},
        {"8A0303", Resources.Localization.Colors._8A0303},
        {"8A2BE2", Resources.Localization.Colors._8A2BE2},
        {"8A3324", Resources.Localization.Colors._8A3324},
        {"8B00FF", Resources.Localization.Colors._8B00FF},
        {"8C92AC", Resources.Localization.Colors._8C92AC},
        {"8DB600", Resources.Localization.Colors._8DB600},
        {"8F5973", Resources.Localization.Colors._8F5973},
        {"8F9779", Resources.Localization.Colors._8F9779},
        {"900020", Resources.Localization.Colors._900020},
        {"904D30", Resources.Localization.Colors._904D30},
        {"911E42", Resources.Localization.Colors._911E42},
        {"915C83", Resources.Localization.Colors._915C83},
        {"918151", Resources.Localization.Colors._918151},
        {"92000A", Resources.Localization.Colors._92000A},
        {"954535", Resources.Localization.Colors._954535},
        {"960018", Resources.Localization.Colors._960018},
        {"964B00", Resources.Localization.Colors._964B00},
        {"965A3E", Resources.Localization.Colors._965A3E},
        {"967117", Resources.Localization.Colors._967117},
        {"986960", Resources.Localization.Colors._986960},
        {"987654", Resources.Localization.Colors._987654},
        {"98777B", Resources.Localization.Colors._98777B},
        {"98817B", Resources.Localization.Colors._98817B},
        {"98FF98", Resources.Localization.Colors._98FF98},
        {"990066", Resources.Localization.Colors._990066},
        {"991199", Resources.Localization.Colors._991199},
        {"993366", Resources.Localization.Colors._993366},
        {"996666", Resources.Localization.Colors._996666},
        {"9966CC", Resources.Localization.Colors._9966CC},
        {"997A8D", Resources.Localization.Colors._997A8D},
        {"99958C", Resources.Localization.Colors._99958C},
        {"9B2D30", Resources.Localization.Colors._9B2D30},
        {"9C2542", Resources.Localization.Colors._9C2542},
        {"9DB1CC", Resources.Localization.Colors._9DB1CC},
        {"9F2B68", Resources.Localization.Colors._9F2B68},
        {"9F8170", Resources.Localization.Colors._9F8170},
        {"9FA91F", Resources.Localization.Colors._9FA91F},
        {"A08040", Resources.Localization.Colors.A08040},
        {"A17A74", Resources.Localization.Colors.A17A74},
        {"A1CAF1", Resources.Localization.Colors.A1CAF1},
        {"A25F2A", Resources.Localization.Colors.A25F2A},
        {"A2A2D0", Resources.Localization.Colors.A2A2D0},
        {"A41313", Resources.Localization.Colors.A41313},
        {"A4C639", Resources.Localization.Colors.A4C639},
        {"A5260A", Resources.Localization.Colors.A5260A},
        {"A52A2A", Resources.Localization.Colors.A52A2A},
        {"A57164", Resources.Localization.Colors.A57164},
        {"A67B5B", Resources.Localization.Colors.A67B5B},
        {"A8516E", Resources.Localization.Colors.A8516E},
        {"AA381E", Resources.Localization.Colors.AA381E},
        {"AB274F", Resources.Localization.Colors.AB274F},
        {"AB381F", Resources.Localization.Colors.AB381F},
        {"ABCDEF", Resources.Localization.Colors.ABCDEF},
        {"ACB78E", Resources.Localization.Colors.ACB78E},
        {"ACE1AF", Resources.Localization.Colors.ACE1AF},
        {"ACE5EE", Resources.Localization.Colors.ACE5EE},
        {"AD6F69", Resources.Localization.Colors.AD6F69},
        {"ADDFAD", Resources.Localization.Colors.ADDFAD},
        {"ADFF2F", Resources.Localization.Colors.ADFF2F},
        {"AF002A", Resources.Localization.Colors.AF002A},
        {"AF4035", Resources.Localization.Colors.AF4035},
        {"AF6E4D", Resources.Localization.Colors.AF6E4D},
        {"AFEEEE", Resources.Localization.Colors.AFEEEE},
        {"B01B2E", Resources.Localization.Colors.B01B2E},
        {"B08D57", Resources.Localization.Colors.B08D57},
        {"B0BF1A", Resources.Localization.Colors.B0BF1A},
        {"B284BE", Resources.Localization.Colors.B284BE},
        {"B2BEB5", Resources.Localization.Colors.B2BEB5},
        {"B31B1B", Resources.Localization.Colors.B31B1B},
        {"B32134", Resources.Localization.Colors.B32134},
        {"B5A642", Resources.Localization.Colors.B5A642},
        {"B60C26", Resources.Localization.Colors.B60C26},
        {"B7410E", Resources.Localization.Colors.B7410E},
        {"B87333", Resources.Localization.Colors.B87333},
        {"B8860B", Resources.Localization.Colors.B8860B},
        {"BADBAD", Resources.Localization.Colors.BADBAD},
        {"BBBBBB", Resources.Localization.Colors.BBBBBB},
        {"BCD4E6", Resources.Localization.Colors.BCD4E6},
        {"BD33A4", Resources.Localization.Colors.BD33A4},
        {"BDB76B", Resources.Localization.Colors.BDB76B},
        {"BEF574", Resources.Localization.Colors.BEF574},
        {"BF4F51", Resources.Localization.Colors.BF4F51},
        {"BF94E4", Resources.Localization.Colors.BF94E4},
        {"BFAFB2", Resources.Localization.Colors.BFAFB2},
        {"BFFF00", Resources.Localization.Colors.BFFF00},
        {"C0C0C0", Resources.Localization.Colors.C0C0C0},
        {"C19A6B", Resources.Localization.Colors.C19A6B},
        {"C32148", Resources.Localization.Colors.C32148},
        {"C39953", Resources.Localization.Colors.C39953},
        {"C3B091", Resources.Localization.Colors.C3B091},
        {"C41E3A", Resources.Localization.Colors.C41E3A},
        {"C46210", Resources.Localization.Colors.C46210},
        {"C4D8E2", Resources.Localization.Colors.C4D8E2},
        {"C71585", Resources.Localization.Colors.C71585},
        {"C7D0CC", Resources.Localization.Colors.C7D0CC},
        {"C7FCEC", Resources.Localization.Colors.C7FCEC},
        {"C8A2C8", Resources.Localization.Colors.C8A2C8},
        {"C95A49", Resources.Localization.Colors.C95A49},
        {"C9A0DC", Resources.Localization.Colors.C9A0DC},
        {"C9FFE5", Resources.Localization.Colors.C9FFE5},
        {"CAA906", Resources.Localization.Colors.CAA906},
        {"CADABA", Resources.Localization.Colors.CADABA},
        {"CAE00D", Resources.Localization.Colors.CAE00D},
        {"CB4154", Resources.Localization.Colors.CB4154},
        {"CC0000", Resources.Localization.Colors.CC0000},
        {"CC5500", Resources.Localization.Colors.CC5500},
        {"CC7722", Resources.Localization.Colors.CC7722},
        {"CC8899", Resources.Localization.Colors.CC8899},
        {"CC9900", Resources.Localization.Colors.CC9900},
        {"CC9966", Resources.Localization.Colors.CC9966},
        {"CCCCCC", Resources.Localization.Colors.CCCCCC},
        {"CCCCFF", Resources.Localization.Colors.CCCCFF},
        {"CCFF00", Resources.Localization.Colors.CCFF00},
        {"CD00CD", Resources.Localization.Colors.CD00CD},
        {"CD5700", Resources.Localization.Colors.CD5700},
        {"CD5B45", Resources.Localization.Colors.CD5B45},
        {"CD5C5C", Resources.Localization.Colors.CD5C5C},
        {"CD607E", Resources.Localization.Colors.CD607E},
        {"CD7F32", Resources.Localization.Colors.CD7F32},
        {"CD8032", Resources.Localization.Colors.CD8032},
        {"CD853F", Resources.Localization.Colors.CD853F},
        {"CD9575", Resources.Localization.Colors.CD9575},
        {"CFB53B", Resources.Localization.Colors.CFB53B},
        {"CFCFCF", Resources.Localization.Colors.CFCFCF},
        {"D0DB61", Resources.Localization.Colors.D0DB61},
        {"D0F0C0", Resources.Localization.Colors.D0F0C0},
        {"D0FF14", Resources.Localization.Colors.D0FF14},
        {"D1001C", Resources.Localization.Colors.D1001C},
        {"D19FE8", Resources.Localization.Colors.D19FE8},
        {"D1E231", Resources.Localization.Colors.D1E231},
        {"D2691E", Resources.Localization.Colors.D2691E},
        {"D2B48C", Resources.Localization.Colors.D2B48C},
        {"D3212D", Resources.Localization.Colors.D3212D},
        {"D3AF37", Resources.Localization.Colors.D3AF37},
        {"D53E07", Resources.Localization.Colors.D53E07},
        {"D5713F", Resources.Localization.Colors.D5713F},
        {"D5D5D5", Resources.Localization.Colors.D5D5D5},
        {"D77D31", Resources.Localization.Colors.D77D31},
        {"D891EF", Resources.Localization.Colors.D891EF},
        {"D8A903", Resources.Localization.Colors.D8A903},
        {"D8BFD8", Resources.Localization.Colors.D8BFD8},
        {"DA70D6", Resources.Localization.Colors.DA70D6},
        {"DAA520", Resources.Localization.Colors.DAA520},
        {"DABDAB", Resources.Localization.Colors.DABDAB},
        {"DAD871", Resources.Localization.Colors.DAD871},
        {"DB7093", Resources.Localization.Colors.DB7093},
        {"DBE9F4", Resources.Localization.Colors.DBE9F4},
        {"DC143C", Resources.Localization.Colors.DC143C},
        {"DDADAF", Resources.Localization.Colors.DDADAF},
        {"DDE26A", Resources.Localization.Colors.DDE26A},
        {"DE3163", Resources.Localization.Colors.DE3163},
        {"DE5D83", Resources.Localization.Colors.DE5D83},
        {"DE6FA1", Resources.Localization.Colors.DE6FA1},
        {"DF73FF", Resources.Localization.Colors.DF73FF},
        {"E0218A", Resources.Localization.Colors.E0218A},
        {"E1DFE0", Resources.Localization.Colors.E1DFE0},
        {"E28B00", Resources.Localization.Colors.E28B00},
        {"E2E5DE", Resources.Localization.Colors.E2E5DE},
        {"E30022", Resources.Localization.Colors.E30022},
        {"E32636", Resources.Localization.Colors.E32636},
        {"E34234", Resources.Localization.Colors.E34234},
        {"E3DAC9", Resources.Localization.Colors.E3DAC9},
        {"E4717A", Resources.Localization.Colors.E4717A},
        {"E49B0F", Resources.Localization.Colors.E49B0F},
        {"E4D00A", Resources.Localization.Colors.E4D00A},
        {"E52B50", Resources.Localization.Colors.E52B50},
        {"E6E6FA", Resources.Localization.Colors.E6E6FA},
        {"E75480", Resources.Localization.Colors.E75480},
        {"E7FEFF", Resources.Localization.Colors.E7FEFF},
        {"E88E5A", Resources.Localization.Colors.E88E5A},
        {"E97451", Resources.Localization.Colors.E97451},
        {"E9967A", Resources.Localization.Colors.E9967A},
        {"E9D66B", Resources.Localization.Colors.E9D66B},
        {"EA8DF7", Resources.Localization.Colors.EA8DF7},
        {"EB4C42", Resources.Localization.Colors.EB4C42},
        {"EBC2AF", Resources.Localization.Colors.EBC2AF},
        {"EBECF0", Resources.Localization.Colors.EBECF0},
        {"ED872D", Resources.Localization.Colors.ED872D},
        {"ED9121", Resources.Localization.Colors.ED9121},
        {"EEDC82", Resources.Localization.Colors.EEDC82},
        {"EEE0B1", Resources.Localization.Colors.EEE0B1},
        {"EEE6A3", Resources.Localization.Colors.EEE6A3},
        {"EFAF8C", Resources.Localization.Colors.EFAF8C},
        {"EFBBCC", Resources.Localization.Colors.EFBBCC},
        {"EFDECD", Resources.Localization.Colors.EFDECD},
        {"F0DC82", Resources.Localization.Colors.F0DC82},
        {"F0F8FF", Resources.Localization.Colors.F0F8FF},
        {"F19CBB", Resources.Localization.Colors.F19CBB},
        {"F1DDCF", Resources.Localization.Colors.F1DDCF},
        {"F28E1C", Resources.Localization.Colors.F28E1C},
        {"F2B400", Resources.Localization.Colors.F2B400},
        {"F2E8C9", Resources.Localization.Colors.F2E8C9},
        {"F2F0E6", Resources.Localization.Colors.F2F0E6},
        {"F2F3F4", Resources.Localization.Colors.F2F3F4},
        {"F37042", Resources.Localization.Colors.F37042},
        {"F4A460", Resources.Localization.Colors.F4A460},
        {"F4BBFF", Resources.Localization.Colors.F4BBFF},
        {"F4C2C2", Resources.Localization.Colors.F4C2C2},
        {"F4C430", Resources.Localization.Colors.F4C430},
        {"F5DEB3", Resources.Localization.Colors.F5DEB3},
        {"F5F5DC", Resources.Localization.Colors.F5F5DC},
        {"F7E7CE", Resources.Localization.Colors.F7E7CE},
        {"F7F21A", Resources.Localization.Colors.F7F21A},
        {"F88379", Resources.Localization.Colors.F88379},
        {"F984E5", Resources.Localization.Colors.F984E5},
        {"FA6E79", Resources.Localization.Colors.FA6E79},
        {"FADADD", Resources.Localization.Colors.FADADD},
        {"FADFAD", Resources.Localization.Colors.FADFAD},
        {"FAE7B5", Resources.Localization.Colors.FAE7B5},
        {"FAEBD7", Resources.Localization.Colors.FAEBD7},
        {"FAEEDD", Resources.Localization.Colors.FAEEDD},
        {"FAF0E6", Resources.Localization.Colors.FAF0E6},
        {"FB607F", Resources.Localization.Colors.FB607F},
        {"FBCCE7", Resources.Localization.Colors.FBCCE7},
        {"FBCEB1", Resources.Localization.Colors.FBCEB1},
        {"FBEC5D", Resources.Localization.Colors.FBEC5D},
        {"FC0FC0", Resources.Localization.Colors.FC0FC0},
        {"FD7C6E", Resources.Localization.Colors.FD7C6E},
        {"FDE910", Resources.Localization.Colors.FDE910},
        {"FDEE00", Resources.Localization.Colors.FDEE00},
        {"FE6F5E", Resources.Localization.Colors.FE6F5E},
        {"FEF200", Resources.Localization.Colors.FEF200},
        {"FEFEFA", Resources.Localization.Colors.FEFEFA},
        {"FF0000", Resources.Localization.Colors.FF0000},
        {"FF0038", Resources.Localization.Colors.FF0038},
        {"FF007F", Resources.Localization.Colors.FF007F},
        {"FF00FF", Resources.Localization.Colors.FF00FF},
        {"FF033E", Resources.Localization.Colors.FF033E},
        {"FF0800", Resources.Localization.Colors.FF0800},
        {"FF2052", Resources.Localization.Colors.FF2052},
        {"FF2400", Resources.Localization.Colors.FF2400},
        {"FF47CA", Resources.Localization.Colors.FF47CA},
        {"FF4D00", Resources.Localization.Colors.FF4D00},
        {"FF4F00", Resources.Localization.Colors.FF4F00},
        {"FF55A3", Resources.Localization.Colors.FF55A3},
        {"FF6600", Resources.Localization.Colors.FF6600},
        {"FF7518", Resources.Localization.Colors.FF7518},
        {"FF7E00", Resources.Localization.Colors.FF7E00},
        {"FF7F50", Resources.Localization.Colors.FF7F50},
        {"FF8B00", Resources.Localization.Colors.FF8B00},
        {"FF8C69", Resources.Localization.Colors.FF8C69},
        {"FF91AF", Resources.Localization.Colors.FF91AF},
        {"FF9218", Resources.Localization.Colors.FF9218},
        {"FF9900", Resources.Localization.Colors.FF9900},
        {"FF9966", Resources.Localization.Colors.FF9966},
        {"FFA500", Resources.Localization.Colors.FFA500},
        {"FFA600", Resources.Localization.Colors.FFA600},
        {"FFA6C9", Resources.Localization.Colors.FFA6C9},
        {"FFA812", Resources.Localization.Colors.FFA812},
        {"FFAA1D", Resources.Localization.Colors.FFAA1D},
        {"FFBA00", Resources.Localization.Colors.FFBA00},
        {"FFBCD9", Resources.Localization.Colors.FFBCD9},
        {"FFBF00", Resources.Localization.Colors.FFBF00},
        {"FFC0CB", Resources.Localization.Colors.FFC0CB},
        {"FFC1CC", Resources.Localization.Colors.FFC1CC},
        {"FFCC00", Resources.Localization.Colors.FFCC00},
        {"FFCC99", Resources.Localization.Colors.FFCC99},
        {"FFCCCB", Resources.Localization.Colors.FFCCCB},
        {"FFD1DC", Resources.Localization.Colors.FFD1DC},
        {"FFD59A", Resources.Localization.Colors.FFD59A},
        {"FFD700", Resources.Localization.Colors.FFD700},
        {"FFD800", Resources.Localization.Colors.FFD800},
        {"FFDAB9", Resources.Localization.Colors.FFDAB9},
        {"FFDB58", Resources.Localization.Colors.FFDB58},
        {"FFDEAD", Resources.Localization.Colors.FFDEAD},
        {"FFE135", Resources.Localization.Colors.FFE135},
        {"FFE4B2", Resources.Localization.Colors.FFE4B2},
        {"FFE4C4", Resources.Localization.Colors.FFE4C4},
        {"FFE5B4", Resources.Localization.Colors.FFE5B4},
        {"FFEBCD", Resources.Localization.Colors.FFEBCD},
        {"FFEF00", Resources.Localization.Colors.FFEF00},
        {"FFEFD5", Resources.Localization.Colors.FFEFD5},
        {"FFF0F5", Resources.Localization.Colors.FFF0F5},
        {"FFF5EE", Resources.Localization.Colors.FFF5EE},
        {"FFF600", Resources.Localization.Colors.FFF600},
        {"FFF8DC", Resources.Localization.Colors.FFF8DC},
        {"FFF8E7", Resources.Localization.Colors.FFF8E7},
        {"FFFACD", Resources.Localization.Colors.FFFACD},
        {"FFFDD0", Resources.Localization.Colors.FFFDD0},
        {"FFFDDF", Resources.Localization.Colors.FFFDDF},
        {"FFFF00", Resources.Localization.Colors.FFFF00},
        {"FFFF99", Resources.Localization.Colors.FFFF99},
        {"FFFFCC", Resources.Localization.Colors.FFFFCC},
        {"FFFFFF", Resources.Localization.Colors.FFFFFF},
    };

    /// <summary>
    /// Return a <see cref="string"/> representation of a CMYK color
    /// </summary>
    /// <param name="color">The <see cref="System.Windows.Media.Color"/> for the CMYK color presentation</param>
    /// <returns>A <see cref="string"/> representation of a CMYK color</returns>
    public static string ColorToCmyk(Color color)
    {
        var (cyan, magenta, yellow, blackKey) = ColorFormatUtils.ConvertToCmykColor(color);

        cyan = Math.Round(cyan * 100);
        magenta = Math.Round(magenta * 100);
        yellow = Math.Round(yellow * 100);
        blackKey = Math.Round(blackKey * 100);

        return $"cmyk({cyan.ToString(CultureInfo.InvariantCulture)}%"
               + $", {magenta.ToString(CultureInfo.InvariantCulture)}%"
               + $", {yellow.ToString(CultureInfo.InvariantCulture)}%"
               + $", {blackKey.ToString(CultureInfo.InvariantCulture)}%)";
    }

    /// <summary>
    /// Return a hexadecimal <see cref="string"/> representation of a RGB color
    /// </summary>
    /// <param name="color">The <see cref="Color"/> for the hexadecimal presentation</param>
    /// <returns>A hexadecimal <see cref="string"/> representation of a RGB color</returns>
    public static string ColorToHex(Color color)
    {
        const string hexFormat = "x2";

        return $"{color.R.ToString(hexFormat, CultureInfo.InvariantCulture)}"
               + $"{color.G.ToString(hexFormat, CultureInfo.InvariantCulture)}"
               + $"{color.B.ToString(hexFormat, CultureInfo.InvariantCulture)}";
    }

    /// <summary>
    /// Return a <see cref="string"/> representation of a HSB color
    /// </summary>
    /// <param name="color">The <see cref="Color"/> for the HSB color presentation</param>
    /// <returns>A <see cref="string"/> representation of a HSB color</returns>
    public static string ColorToHsb(Color color)
    {
        var (hue, saturation, brightness) = ColorFormatUtils.ConvertToHsbColor(color);

        hue = Math.Round(hue);
        saturation = Math.Round(saturation * 100);
        brightness = Math.Round(brightness * 100);

        return $"hsb({hue.ToString(CultureInfo.InvariantCulture)}"
               + $", {saturation.ToString(CultureInfo.InvariantCulture)}%"
               + $", {brightness.ToString(CultureInfo.InvariantCulture)}%)";
    }

    /// <summary>
    /// Return a <see cref="string"/> representation float color styling(0.1f, 0.1f, 0.1f)
    /// </summary>
    /// <param name="color">The <see cref="Color"/> to convert</param>
    /// <returns>a string value (0.1f, 0.1f, 0.1f)</returns>
    public static string ColorToFloat(Color color)
    {
        var (red, green, blue) = (color.R / 255d, color.G / 255d, color.B / 255d);
        const int precision = 2;
        const string floatFormat = "0.##";

        return $"({Math.Round(red, precision).ToString(floatFormat, CultureInfo.InvariantCulture)}f"
               + $", {Math.Round(green, precision).ToString(floatFormat, CultureInfo.InvariantCulture)}f"
               + $", {Math.Round(blue, precision).ToString(floatFormat, CultureInfo.InvariantCulture)}f, 1f)";
    }

    /// <summary>
    /// Return a <see cref="string"/> representation decimal color value
    /// </summary>
    /// <param name="color">The <see cref="Color"/> to convert</param>
    /// <returns>a string value number</returns>
    public static string ColorToDecimal(Color color)
    {
        return $"{(color.R * 65536) + (color.G * 256) + color.B}";
    }

    /// <summary>
    /// Return a <see cref="string"/> representation of a HSI color
    /// </summary>
    /// <param name="color">The <see cref="Color"/> for the HSI color presentation</param>
    /// <returns>A <see cref="string"/> representation of a HSI color</returns>
    public static string ColorToHsi(Color color)
    {
        var (hue, saturation, intensity) = ColorFormatUtils.ConvertToHsiColor(color);

        hue = Math.Round(hue);
        saturation = Math.Round(saturation * 100);
        intensity = Math.Round(intensity * 100);

        return $"hsi({hue.ToString(CultureInfo.InvariantCulture)}"
               + $", {saturation.ToString(CultureInfo.InvariantCulture)}%"
               + $", {intensity.ToString(CultureInfo.InvariantCulture)}%)";
    }

    /// <summary>
    /// Return a <see cref="string"/> representation of a HSL color
    /// </summary>
    /// <param name="color">The <see cref="Color"/> for the HSL color presentation</param>
    /// <returns>A <see cref="string"/> representation of a HSL color</returns>
    public static string ColorToHsl(Color color)
    {
        var (hue, saturation, lightness) = ColorFormatUtils.ConvertToHslColor(color);

        hue = Math.Round(hue);
        saturation = Math.Round(saturation * 100);
        lightness = Math.Round(lightness * 100);

        // Using InvariantCulture since this is used for color representation
        return $"hsl({hue.ToString(CultureInfo.InvariantCulture)}"
               + $", {saturation.ToString(CultureInfo.InvariantCulture)}%"
               + $", {lightness.ToString(CultureInfo.InvariantCulture)}%)";
    }

    /// <summary>
    /// Return a <see cref="string"/> representation of a HSV color
    /// </summary>
    /// <param name="color">The <see cref="Color"/> for the HSV color presentation</param>
    /// <returns>A <see cref="string"/> representation of a HSV color</returns>
    public static string ColorToHsv(Color color)
    {
        var (hue, saturation, value) = ColorFormatUtils.ConvertToHsvColor(color);

        hue = Math.Round(hue);
        saturation = Math.Round(saturation * 100);
        value = Math.Round(value * 100);

        // Using InvariantCulture since this is used for color representation
        return $"hsv({hue.ToString(CultureInfo.InvariantCulture)}"
               + $", {saturation.ToString(CultureInfo.InvariantCulture)}%"
               + $", {value.ToString(CultureInfo.InvariantCulture)}%)";
    }

    /// <summary>
    /// Return a <see cref="string"/> representation of a HWB color
    /// </summary>
    /// <param name="color">The <see cref="Color"/> for the HWB color presentation</param>
    /// <returns>A <see cref="string"/> representation of a HWB color</returns>
    public static string ColorToHwb(Color color)
    {
        var (hue, whiteness, blackness) = ColorFormatUtils.ConvertToHwbColor(color);

        hue = Math.Round(hue);
        whiteness = Math.Round(whiteness * 100);
        blackness = Math.Round(blackness * 100);

        return $"hwb({hue.ToString(CultureInfo.InvariantCulture)}"
               + $", {whiteness.ToString(CultureInfo.InvariantCulture)}%"
               + $", {blackness.ToString(CultureInfo.InvariantCulture)}%)";
    }

    /// <summary>
    /// Return a <see cref="string"/> representation of a natural color
    /// </summary>
    /// <param name="color">The <see cref="Color"/> for the natural color presentation</param>
    /// <returns>A <see cref="string"/> representation of a natural color</returns>
    public static string ColorToNCol(Color color)
    {
        var (hue, whiteness, blackness) = ColorFormatUtils.ConvertToNaturalColor(color);

        whiteness = Math.Round(whiteness * 100);
        blackness = Math.Round(blackness * 100);

        return $"{hue}"
               + $", {whiteness.ToString(CultureInfo.InvariantCulture)}%"
               + $", {blackness.ToString(CultureInfo.InvariantCulture)}%";
    }

    /// <summary>
    /// Return a <see cref="string"/> representation of a RGB color
    /// </summary>
    /// <param name="color">The <see cref="Color"/> for the RGB color presentation</param>
    /// <returns>A <see cref="string"/> representation of a RGB color</returns>
    public static string ColorToRgb(Color color)
        => $"rgb({color.R.ToString(CultureInfo.InvariantCulture)}"
           + $", {color.G.ToString(CultureInfo.InvariantCulture)}"
           + $", {color.B.ToString(CultureInfo.InvariantCulture)})";

    /// <summary>
    /// Returns a <see cref="string"/> representation of a CIE LAB color
    /// </summary>
    /// <param name="color">The <see cref="Color"/> for the CIE LAB color presentation</param>
    /// <returns>A <see cref="string"/> representation of a CIE LAB color</returns>
    public static string ColorToCielab(Color color)
    {
        var (lightness, chromaticityA, chromaticityB) = ColorFormatUtils.ConvertToCielabColor(color);
        lightness = Math.Round(lightness, 2);
        chromaticityA = Math.Round(chromaticityA, 2);
        chromaticityB = Math.Round(chromaticityB, 2);

        return $"CIELab({lightness.ToString(CultureInfo.InvariantCulture)}" +
               $", {chromaticityA.ToString(CultureInfo.InvariantCulture)}" +
               $", {chromaticityB.ToString(CultureInfo.InvariantCulture)})";
    }

    /// <summary>
    /// Returns a <see cref="string"/> representation of a CIE XYZ color
    /// </summary>
    /// <param name="color">The <see cref="Color"/> for the CIE XYZ color presentation</param>
    /// <returns>A <see cref="string"/> representation of a CIE XYZ color</returns>
    public static string ColorToCieXyz(Color color)
    {
        var (x, y, z) = ColorFormatUtils.ConvertToCiexyzColor(color);

        x = Math.Round(x * 100, 4);
        y = Math.Round(y * 100, 4);
        z = Math.Round(z * 100, 4);

        return $"XYZ({x.ToString(CultureInfo.InvariantCulture)}" +
               $", {y.ToString(CultureInfo.InvariantCulture)}" +
               $", {z.ToString(CultureInfo.InvariantCulture)})";
    }

    /// <summary>
    /// Return a hexadecimal integer <see cref="string"/> representation of a RGB color
    /// </summary>
    /// <param name="color">The <see cref="Color"/> for the hexadecimal integer presentation</param>
    /// <returns>A hexadecimal integer <see cref="string"/> representation of a RGB color</returns>
    public static string ColorToHexInteger(Color color)
    {
        const string hexFormat = "X2";

        return "0xFF"
               + $"{color.R.ToString(hexFormat, CultureInfo.InvariantCulture)}"
               + $"{color.G.ToString(hexFormat, CultureInfo.InvariantCulture)}"
               + $"{color.B.ToString(hexFormat, CultureInfo.InvariantCulture)}";
    }

    /// <summary>
    /// Return a name of a RGB color
    /// </summary>
    /// <param name="color">The <see cref="Color"/> for presentation</param>
    /// <returns>Approximate name of a color based on RGB representation</returns>
    public static string GetColorName(Color color)
    {
        var colorName = string.Empty;
        var closestDistance = double.MaxValue;
        foreach (var entry in KnownColors)
        {
            var knownColor = ConvertHexStringToColor(entry.Key);
            var distance = CalculateColorDistance(color, knownColor);

            if (distance < closestDistance)
            {
                colorName = entry.Value;
                closestDistance = distance;
            }
        }

        return colorName;
    }

    [Pure]
    private static Color ConvertHexStringToColor(string hex)
    {
        var red = byte.Parse(hex.Substring(0, 2), NumberStyles.HexNumber);
        var green = byte.Parse(hex.Substring(2, 2), NumberStyles.HexNumber);
        var blue = byte.Parse(hex.Substring(4, 2), NumberStyles.HexNumber);

        return Color.FromArgb(red, green, blue);
    }

    [Pure]
    private static double CalculateColorDistance(Color color1, Color color2)
    {
        var deltaR = color1.R - color2.R;
        var deltaG = color1.G - color2.G;
        var deltaB = color1.B - color2.B;

        return Math.Sqrt(deltaR * deltaR + deltaG * deltaG + deltaB * deltaB);
    }
}
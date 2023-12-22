// Copyright 2003-2023 by Autodesk, Inc.
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
using System.Windows.Media;

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
        {"CAE00D", "Bitter lemon"},
        {"0095B6", "Bondi blue"},
        {"56A0D3", "Carolina blue"},
        {"703642", "Catawba"},
        {"FBCEB1", "Apricot"},
        {"0048BA", "Absolute zero"},
        {"568203", "Avocado"},
        {"4C2F27", "Acajou"},
        {"7FFFD4", "Aquamarine"},
        {"63775B", "Axolotl"},
        {"AF002A", "Alabama crimson"},
        {"F2F0E6", "Alabaster"},
        {"E32636", "Alizarin crimson"},
        {"F0F8FF", "Alice blue"},
        {"FF2400", "Scarlet"},
        {"00C4B0", "Amazonite"},
        {"3B7A57", "Amazon"},
        {"E52B50", "Amaranth"},
        {"D3212D", "Amaranth red"},
        {"9F2B68", "Amaranth deep purple"},
        {"AB274F", "Amaranth purple"},
        {"391802", "American bronze"},
        {"FF033E", "American rose"},
        {"F2B400", "American yellow"},
        {"34B334", "American green"},
        {"804040", "American brown"},
        {"B32134", "American red"},
        {"FF8B00", "American orange"},
        {"3B3B6D", "American blue"},
        {"551B8C", "American violet"},
        {"D3AF37", "American gold"},
        {"CFCFCF", "American silver"},
        {"9966CC", "Amethyst"},
        {"F2F3F4", "Anti-flash white"},
        {"665D1E", "Antique bronze"},
        {"CD9575", "Antique brass"},
        {"915C83", "Antique fuchsia"},
        {"FAEBD7", "Antique white"},
        {"841B2D", "Antique ruby"},
        {"464451", "Anthracite"},
        {"E9D66B", "Arylide yellow"},
        {"D0FF14", "Arctic lime"},
        {"4B5320", "Army green"},
        {"8F9779", "Artichoke"},
        {"003A6C", "Ateneo blue"},
        {"6E7F80", "Auro metal saurus"},
        {"B284BE", "African violet"},
        {"C39953", "Aztec Gold"},
        {"7CB9E8", "Aero"},
        {"C9FFE5", "Aero blue"},
        {"990066", "Eggplant"},
        {"FAE7B5", "Banana Mania"},
        {"FFE135", "Banana yellow"},
        {"CCCCFF", "Periwinkle"},
        {"FA6E79", "Begonia"},
        {"FAEEDD", "Scared nymph"},
        {"F5F5DC", "Beige"},
        {"FF91AF", "Baker-Miller pink"},
        {"FFFFFF", "White"},
        {"FFDEAD", "Navajo white"},
        {"003153", "Prussian blue"},
        {"30D5C8", "Turquoise"},
        {"A5260A", "Bismarck-furious"},
        {"3D2B1F", "Bistre"},
        {"967117", "Bistre brown"},
        {"FFEBCD", "Blanched almond"},
        {"ABCDEF", "Pale cornflower blue"},
        {"AF4035", "Pale carmine"},
        {"DDADAF", "Pale chestnut"},
        {"987654", "Pale brown"},
        {"DABDAB", "Pale Sandy Brown"},
        {"F984E5", "Pale magenta"},
        {"FADADD", "Pale pink"},
        {"AFEEEE", "Pale Blue"},
        {"F4BBFF", "Brilliant lavender"},
        {"3399FF", "Brilliant azure"},
        {"FF55A3", "Brilliant rose"},
        {"4E1609", "Flea belly"},
        {"9F8170", "Beaver"},
        {"480607", "Bulgarian rose"},
        {"ACB78E", "Swamp green"},
        {"E88E5A", "Big Foot Feet"},
        {"9C2542", "Big dip o’ruby"},
        {"7F1734", "Claret"},
        {"9B2D30", "Wine red"},
        {"D5D5D5", "Abdel Kerim's beard"},
        {"CC0000", "Boston University Red"},
        {"1B4D3E", "Brunswick green"},
        {"87413F", "Brandy"},
        {"004225", "British racing green"},
        {"CD7F32", "Bronze"},
        {"B08D57", "Bronze (Metallic)"},
        {"737000", "Bronze Yellow"},
        {"800020", "Burgundy"},
        {"900020", "Burgundy"},
        {"45161C", "Fulvous"},
        {"D5713F", "Vanilla"},
        {"C19A6B", "Camel"},
        {"DAD871", "Vert-de-pеche"},
        {"34C924", "Vert-de-pomme"},
        {"A57164", "Blast-off bronze"},
        {"BD33A4", "Byzantine"},
        {"702963", "Byzantium"},
        {"911E42", "Cherry"},
        {"3CAA3C", "Toad in love"},
        {"5D8AA8", "Air Force Navy(RAF)"},
        {"98777B", "Bazaar"},
        {"5072A7", "Blue yonder"},
        {"72A0C1", "Air superiority blue"},
        {"FFA6C9", "Carnation pink"},
        {"DF73FF", "Heliotrope"},
        {"C9A0DC", "Wisteria"},
        {"0088DC", "Blue cola"},
        {"ACE5EE", "Blue Lagoon"},
        {"00BFFF", "Deep sky blue"},
        {"00B9FB", "Blue Bolt"},
        {"BCD4E6", "Beau blue"},
        {"A2A2D0", "Blue Bell"},
        {"FFDB58", "Mustard"},
        {"BFFF00", "Bitter lime"},
        {"FE6F5E", "Bittersweet"},
        {"C7D0CC", "Gris de perle"},
        {"D1E231", "Pear"},
        {"E49B0F", "Gamboge"},
        {"A1CAF1", "Baby blue eyes"},
        {"89CFF0", "Baby blue"},
        {"F7F21A", "Child's surprise"},
        {"1560BD", "Denim"},
        {"36454F", "Charcoal"},
        {"8F5973", "Blackberry"},
        {"FFC1CC", "Bubble gum"},
        {"FFD800", "School bus yellow"},
        {"00A86B", "Jade"},
        {"ADFF2F", "Green-yellow"},
        {"FADFAD", "Peach-yellow"},
        {"FFE4B2", "Yellow Pink"},
        {"FFFF00", "Yellow"},
        {"D2B48C", "Tan"},
        {"1E90FF", "Dodger blue"},
        {"FF47CA", "Shocked star"},
        {"006A4E", "Bottle green"},
        {"00FF7F", "Spring Green"},
        {"98FF98", "Mint Green"},
        {"01796F", "Pine Green"},
        {"2E8B57", "Sea Green"},
        {"8DB600", "Apple Green"},
        {"00FF00", "Green"},
        {"A4C639", "Android green"},
        {"7BB661", "Bud green"},
        {"ADDFAD", "Moss green"},
        {"4F7942", "Fern green"},
        {"CADABA", "Gray-Tea Green"},
        {"D0F0C0", "Tea Green"},
        {"232B2B", "Charleston green"},
        {"DAA520", "Goldenrod"},
        {"FFD700", "Gold"},
        {"50C878", "Emerald"},
        {"4B0082", "Indigo"},
        {"FFF600", "Cadmium yellow"},
        {"006B3C", "Cadmium green"},
        {"E30022", "Cadmium red"},
        {"ED872D", "Cadmium orange"},
        {"B60C26", "Cadmium Purple"},
        {"0A1195", "Cadmium blue"},
        {"7F3E98", "Cadmium violet"},
        {"A25F2A", "Camelopardalis"},
        {"EFBBCC", "Cameo pink"},
        {"78866B", "Camouflage green"},
        {"FFFF99", "Canary"},
        {"FFEF00", "Canary yellow"},
        {"FFD59A", "Caramel"},
        {"C41E3A", "Cardinal"},
        {"00CC99", "Caribbean green"},
        {"960018", "Carmine"},
        {"FF0038", "Carmine red"},
        {"EB4C42", "Carmine pink"},
        {"00563F", "Castleton green"},
        {"4B3621", "Café noir"},
        {"CD5C5C", "Chestnut"},
        {"954535", "Chestnut"},
        {"99958C", "Quartz"},
        {"C95A49", "Cedar Chest"},
        {"246BCE", "Celtic Blue"},
        {"FF4D00", "Vermilion"},
        {"E34234", "Cinnabar"},
        {"CB4154", "Brick red"},
        {"884535", "Brick"},
        {"B0BF1A", "Acid green"},
        {"CD8032", "Chinese bronze"},
        {"A8516E", "China rose"},
        {"856088", "Chinese violet"},
        {"E2E5DE", "Chinese white"},
        {"D0DB61", "Chinese green"},
        {"AB381F", "Chinese brown"},
        {"AA381E", "Chinese red"},
        {"F37042", "Chinese orange"},
        {"720B98", "Chinese purple"},
        {"DE6FA1", "China pink"},
        {"365194", "Chinese blue"},
        {"141414", "Chinese black"},
        {"CC9900", "Chinese gold"},
        {"CCCCCC", "Chinese silver"},
        {"FBCCE7", "Classic rose"},
        {"0047AB", "Cobalt"},
        {"F0DC82", "Buff"},
        {"965A3E", "Coconut"},
        {"3C3024", "Cola"},
        {"C4D8E2", "Columbia Blue"},
        {"F88379", "Congo pink"},
        {"FFFFCC", "Conditioner"},
        {"E4717A", "Candy pink"},
        {"FF0800", "Candy apple red"},
        {"FF7F50", "Coral"},
        {"FD7C6E", "Coral Reef"},
        {"893F45", "Cordovan"},
        {"800000", "Maroon"},
        {"7B3F00", "Cinnamon"},
        {"964B00", "Brown"},
        {"CD607E", "Cinnamon Satin"},
        {"CC9966", "Brown Yellow"},
        {"81613C", "Coyote brown"},
        {"6B4423", "Brown-nose"},
        {"AF6E4D", "Brown Sugar"},
        {"5F1933", "Brown Chocolate"},
        {"4169E1", "Royal Blue"},
        {"2E2D88", "Cosmic Cobalt"},
        {"FFF8E7", "Cosmic latte"},
        {"E3DAC9", "Bone"},
        {"A67B5B", "Café au lait"},
        {"442D25", "Coffee"},
        {"4A2C2A", "Brown Coffee"},
        {"CD5700", "Tenne"},
        {"755A57", "Russet"},
        {"CC8899", "Puce"},
        {"1F4037", "Red Sea"},
        {"FF0000", "Red"},
        {"7C0A02", "Barn red"},
        {"FFFDD0", "Cream"},
        {"660000", "Blood red"},
        {"D1001C", "Blood orange"},
        {"8A0303", "Blood"},
        {"A41313", "Blood (Animal)"},
        {"630F0F", "Blood (Organ)"},
        {"FFF8DC", "Cornsilk"},
        {"FBEC5D", "Corn"},
        {"E6E6FA", "Lavender"},
        {"2A52BE", "Cerulean blue"},
        {"007BA7", "Cerulean"},
        {"007FFF", "Azure"},
        {"DBE9F4", "Azureish white"},
        {"CCFF00", "Lime"},
        {"B5A642", "Brass"},
        {"DB7093", "Pale red-violet"},
        {"FFFACD", "Lemon Cream"},
        {"FDE910", "Lemon"},
        {"FF8C69", "Salmon"},
        {"EEDC82", "Flax"},
        {"FAF0E6", "Linen"},
        {"7B917B", "Fainted frog"},
        {"0BDA51", "Malachite"},
        {"DC143C", "Crimson"},
        {"FFCC00", "Tangerine"},
        {"4C5866", "Marengo"},
        {"834D18", "Byron"},
        {"B87333", "Copper"},
        {"AD6F69", "Copper penny"},
        {"996666", "Copper rose"},
        {"FF4F00", "Safety orange"},
        {"EFDECD", "Almond"},
        {"ED9121", "Carrot"},
        {"00FFFF", "Aqua"},
        {"FFF5EE", "Seashell"},
        {"1C6B72", "Moray"},
        {"3B444B", "Arsenic"},
        {"7FC7FF", "Sky"},
        {"6B8E23", "Olive Drab"},
        {"DDE26A", "Booger Buster"},
        {"9DB1CC", "Niagara"},
        {"808000", "Olive"},
        {"FFCC99", "Peach-orange"},
        {"FF9966", "Pink-orange"},
        {"FFA500", "Orange"},
        {"C46210", "Alloy orange"},
        {"FDEE00", "Aureolin"},
        {"DA70D6", "Orchid"},
        {"2E5894", "B'dazzled blue"},
        {"FFBA00", "Selective yellow"},
        {"A17A74", "Burnished Brown"},
        {"CC7722", "Ochre"},
        {"C7FCEC", "Pang"},
        {"77DD77", "Pastel green"},
        {"FFD1DC", "Pastel pink"},
        {"98817B", "Cinereous"},
        {"EEE6A3", "Perhydor"},
        {"6600FF", "Persian blue"},
        {"FFE5B4", "Peach"},
        {"F4A460", "Sandy brown"},
        {"EEE0B1", "Cookies and cream"},
        {"F28E1C", "Beer"},
        {"FFEFD5", "Papaya whip"},
        {"84DE02", "Alien Armpit"},
        {"003366", "Midnight Blue"},
        {"003399", "Powder blue"},
        {"592720", "Caput mortuum"},
        {"FF9218", "Jaco"},
        {"FF2052", "Awesome"},
        {"FEFEFA", "Baby powder"},
        {"002E63", "Cool Black"},
        {"E7FEFF", "Bubbles"},
        {"660099", "Purple"},
        {"F5DEB3", "Wheat"},
        {"B7410E", "Rust"},
        {"FEF200", "Christmas yellow"},
        {"3C8D0D", "Christmas green"},
        {"5D2B2C", "Christmas brown"},
        {"B01B2E", "Christmas red"},
        {"FF6600", "Christmas orange"},
        {"FFCCCB", "Christmas pink"},
        {"2A8FBD", "Christmas blue"},
        {"663398", "Christmas purple"},
        {"CAA906", "Christmas gold"},
        {"E1DFE0", "Christmas silver"},
        {"993366", "Mauve"},
        {"E0218A", "Barbie pink"},
        {"FFF0F5", "Lavender Blush"},
        {"FFC0CB", "Pink"},
        {"F19CBB", "Amaranth pink"},
        {"FB607F", "Brink pink"},
        {"997A8D", "Mountbatten pink"},
        {"F4C2C2", "Baby pink"},
        {"DE5D83", "Blush"},
        {"A52A2A", "Auburn"},
        {"D77D31", "Reddish-brown"},
        {"7FFF00", "Chartreuse"},
        {"92000A", "Sangria"},
        {"082567", "Sapphire"},
        {"FFBCD9", "Cotton candy"},
        {"DE3163", "Cerise"},
        {"CD853F", "Light brown"},
        {"BBBBBB", "Light Grey"},
        {"E97451", "Burnt sienna"},
        {"704214", "Sepia"},
        {"465945", "Gray-asparagus"},
        {"B31B1B", "Carnelian"},
        {"C0C0C0", "Silver"},
        {"735184", "Seroburomalinovyj"},
        {"ACE1AF", "Celadon"},
        {"6699CC", "Blue-gray"},
        {"808080", "Gray"},
        {"848482", "Battleship grey"},
        {"708090", "Slate gray"},
        {"FF9900", "Blaze Orange"},
        {"E28B00", "Siena"},
        {"79A0C1", "Bluish"},
        {"00308F", "Air Force blue (USAF)"},
        {"008080", "Teal"},
        {"064E40", "Blue-green (color wheel)"},
        {"553592", "Blue-magenta violet"},
        {"8A2BE2", "Blue-violet"},
        {"7366BD", "Blue-violet (Crayola)"},
        {"4D1A7F", "Blue-violet (color wheel)"},
        {"5DADEC", "Blue Jeans"},
        {"0000FF", "Blue"},
        {"1F75FE", "Blue (Crayola)"},
        {"0087BD", "Blue (NCS)"},
        {"0093AF", "Blue (Munsell)"},
        {"0018A8", "Blue (Pantone)"},
        {"333399", "Blue (pigment)"},
        {"007DFF", "Gradus Blue"},
        {"3A75C4", "Klein Blue"},
        {"126180", "Blue sapphire"},
        {"318CE7", "Bleu de France"},
        {"24A0ED", "Button Blue"},
        {"4682B4", "Steel blue"},
        {"C8A2C8", "Lilac"},
        {"BF4F51", "Bittersweet shimmer"},
        {"660066", "Plum"},
        {"F2E8C9", "Light cream"},
        {"FFFDDF", "Ivory"},
        {"CC5500", "Burnt orange"},
        {"EFAF8C", "Saumon"},
        {"7BA05B", "Asparagus"},
        {"87A96B", "Asparagus"},
        {"CFB53B", "Old Gold"},
        {"79443B", "Bole"},
        {"FFE4C4", "Bisque"},
        {"FFA600", "Cheese"},
        {"E9967A", "Dark salmon"},
        {"560319", "Dark Scarlet"},
        {"2F4F4F", "Dark slate gray"},
        {"116062", "Dark turquoise"},
        {"D8A903", "Dark pear"},
        {"013220", "Dark green"},
        {"B8860B", "Dark goldenrod"},
        {"986960", "Dark chestnut"},
        {"CD5B45", "Dark coral"},
        {"654321", "Dark brown"},
        {"08457E", "Dark cerulean"},
        {"FFA812", "Dark tangerine"},
        {"556832", "Dark Olive"},
        {"FFDAB9", "Dark Peach"},
        {"E75480", "Dark pink"},
        {"000080", "Navy blue"},
        {"423189", "Dark violet"},
        {"177245", "Dark spring green"},
        {"918151", "Dark tan"},
        {"BADBAD", "Dark Tea Green"},
        {"310062", "Dark Indigo"},
        {"03C03C", "Dark pastel green"},
        {"BDB76B", "Dark Khaki"},
        {"904D30", "Terracotta"},
        {"D53E07", "Titian"},
        {"5DA130", "Grass"},
        {"FF7518", "Pumpkin"},
        {"120A8F", "Ultramarine"},
        {"734A12", "Raw umber"},
        {"8A3324", "Burnt umber"},
        {"EA8DF7", "Violaceous"},
        {"991199", "Violet-eggplant"},
        {"C71585", "Red-violet"},
        {"8B00FF", "Violet"},
        {"BEF574", "Pistachio"},
        {"FF00FF", "Magenta (Fuchsia)"},
        {"C3B091", "Khaki"},
        {"4AFF00", "Chlorophyll green"},
        {"8C92AC", "Cool grey"},
        {"0D98BA", "Blue-green"},
        {"EBC2AF", "Zinnwaldite"},
        {"E4D00A", "Citrine"},
        {"9FA91F", "Citron"},
        {"4F86F7", "Blueberry"},
        {"1A4780", "Black Sea"},
        {"BFAFB2", "Black Shadows"},
        {"000000", "Black"},
        {"3D0C02", "Black bean"},
        {"253529", "Black leather jacket"},
        {"54626F", "Black Coral"},
        {"3B2F2F", "Black coffee"},
        {"3B3C36", "Black olive"},
        {"1B1811", "Black chocolate"},
        {"D8BFD8", "Thistle"},
        {"F1DDCF", "Champagne pink"},
        {"F7E7CE", "Champagne"},
        {"A08040", "Chamois"},
        {"21ABCD", "Ball blue"},
        {"F4C430", "Saffron"},
        {"D2691E", "Chocolate"},
        {"3F000F", "Chocolate Brown"},
        {"58111A", "Chocolate Cosmos"},
        {"3C1421", "Chocolate Kisses"},
        {"7DF9FF", "Electric"},
        {"66B447", "Apple"},
        {"40826D", "Viridian"},
        {"00CCCC", "Robin egg blue"},
        {"FFBF00", "Amber"},
        {"FF7E00", "Amber (SAE/ECE)"},
        {"BF94E4", "Bright lavender"},
        {"C32148", "Bright maroon"},
        {"D19FE8", "Bright ube"},
        {"FFAA1D", "Bright Yellow (Crayola)"},
        {"D891EF", "Bright lilac"},
        {"1974D2", "Bright navy blue"},
        {"1DACD6", "Bright cerulean"},
        {"08E8DE", "Bright turquoise"},
        {"66FF00", "Bright green"},
        {"FC0FC0", "Hot pink"},
        {"FF007F", "Bright pink"},
        {"EBECF0", "Bright gray"},
        {"CD00CD", "Bright violet"},
        {"B2BEB5", "Ash gray"},
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

        return Color.FromRgb(red, green, blue);
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
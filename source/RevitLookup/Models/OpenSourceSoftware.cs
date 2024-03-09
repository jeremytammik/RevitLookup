namespace RevitLookup.Models;

public sealed class OpenSourceSoftware
{
    public string SoftwareName { get; set; }
    public string SoftwareUri { get; set; }
    public string LicenseName { get; set; }
    public string LicenseUri { get; set; }

    public OpenSourceSoftware AddSoftware(string name, string uri)
    {
        SoftwareName = name;
        SoftwareUri = uri;
        return this;
    }

    public OpenSourceSoftware AddLicense(string name, string uri)
    {
        LicenseName = name;
        LicenseUri = uri;
        return this;
    }
}
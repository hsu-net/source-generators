using System.ComponentModel;

namespace HsuSgProxyTests.Samples.Restful.Models.Rfid.Antennas;

/// <summary>
/// An enum representing the type of antenna polarization.
/// </summary>
[DefaultValue(COMBINED)]
public enum Polarization
{
    VERTICAL,
    HORIZONTAL,
    COMBINED,
    RIGHTHAND,
    LEFTHAND
}
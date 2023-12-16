using System.ComponentModel;

namespace HsuSgProxyTests.Samples.Restful.Models.Rfid.Filters;

/// <summary>
/// An enum representing if a filter is activated or not, or is inverted.
/// </summary>
[DefaultValue(OFF)]
public enum FilterActivation
{
    OFF,
    ON,
    INVERTED
}
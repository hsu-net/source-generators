using System.Collections.Generic;
using System.Runtime.Serialization;

namespace HsuSgProxyTests.Samples.Restful.Models.Common;

/// <summary>
/// An object containing information about whether or not the DNS addresses are obtained automatically, e.g. from DHCP, plus the current addresses.
/// </summary>
[DataContract]
public record NetworkInterfaceIpv4Dns
{
    /// <summary>
    /// Gets or Sets Automatic
    /// </summary>
    [DataMember(Name = "automatic", EmitDefaultValue = false)]
    public bool? Automatic { get; set; }

    /// <summary>
    /// Gets or Sets Addresses
    /// </summary>
    [DataMember(Name = "addresses", EmitDefaultValue = false)]
    public List<string> Addresses { get; set; }
}

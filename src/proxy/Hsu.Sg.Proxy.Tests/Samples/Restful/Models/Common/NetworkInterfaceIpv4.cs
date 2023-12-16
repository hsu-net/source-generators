using System.Collections.Generic;
using System.Runtime.Serialization;

namespace HsuSgProxyTests.Samples.Restful.Models.Common;

/// <summary>
/// An object containing all IPv4 settings.
/// </summary>
[DataContract]
public record NetworkInterfaceIpv4
{
    /// <summary>
    /// Gets or Sets Method
    /// </summary>
    [DataMember(Name = "method", EmitDefaultValue = false)]
    public string Method { get; set; }

    /// <summary>
    /// Gets or Sets Addresses
    /// </summary>
    [DataMember(Name = "addresses", EmitDefaultValue = false)]
    public List<NetworkInterfaceIpv4Address> Addresses { get; set; }

    /// <summary>
    /// Gets or Sets Dns
    /// </summary>
    [DataMember(Name = "dns", EmitDefaultValue = false)]
    public NetworkInterfaceIpv4Dns Dns { get; set; }
}

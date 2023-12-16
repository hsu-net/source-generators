using System.Runtime.Serialization;

namespace HsuSgProxyTests.Samples.Restful.Models.Common;

/// <summary>
/// An object containing all characteristics of an IPv4 address.
/// </summary>
[DataContract]
public record NetworkInterfaceIpv4Address
{
    /// <summary>
    /// Gets or Sets IpAddress
    /// </summary>
    [DataMember(Name = "ip_address", EmitDefaultValue = false)]
    public string IpAddress { get; set; }

    /// <summary>
    /// Gets or Sets Netmask
    /// </summary>
    [DataMember(Name = "netmask", EmitDefaultValue = false)]
    public string Netmask { get; set; }

    /// <summary>
    /// Gets or Sets Gateway
    /// </summary>
    [DataMember(Name = "gateway", EmitDefaultValue = false)]
    public string Gateway { get; set; }
}
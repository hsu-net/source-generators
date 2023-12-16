using System.Runtime.Serialization;
using HsuSgProxyTests.Samples.Restful.Models.Common;
using HsuSgProxyTests.Samples.Restful.Models.Rfid.Common;

namespace HsuSgProxyTests.Samples.Restful.Models.System;

  /// <summary>
  /// An object containing all network system configuration for one interface.
  /// </summary>
  [DataContract]
  public record NetworkInterfaceResponse {
    /// <summary>
    /// Gets or Sets Id
    /// </summary>
    [DataMember(Name="id", EmitDefaultValue=false)]
    public int? Id { get; set; }

    /// <summary>
    /// Gets or Sets Name
    /// </summary>
    [DataMember(Name="name", EmitDefaultValue=false)]
    public string Name { get; set; }

    /// <summary>
    /// Gets or Sets Mac
    /// </summary>
    [DataMember(Name="mac", EmitDefaultValue=false)]
    public string Mac { get; set; }

    /// <summary>
    /// Gets or Sets DomainName
    /// </summary>
    [DataMember(Name="domain_name", EmitDefaultValue=false)]
    public string DomainName { get; set; }

    /// <summary>
    /// Gets or Sets Ipv4
    /// </summary>
    [DataMember(Name="ipv4", EmitDefaultValue=false)]
    public NetworkInterfaceIpv4 Ipv4 { get; set; }

}
using System.Runtime.Serialization;
using HsuSgProxyTests.Samples.Restful.Models.Rfid.Common;

namespace HsuSgProxyTests.Samples.Restful.Models.Rfid.Filters;

/// <summary>
/// An object containing memory information for a mask.
/// </summary>
[DataContract]
public record FilterMemory {
    /// <summary>
    /// Gets or Sets Bank
    /// </summary>
    [DataMember(Name="bank", EmitDefaultValue=false)]
    public MemoryBank Bank { get; set; }

    /// <summary>
    /// Gets or Sets BitAddress
    /// </summary>
    [DataMember(Name="bit_address", EmitDefaultValue=false)]
    public int? BitAddress { get; set; }

}

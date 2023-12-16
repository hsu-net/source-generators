using System.Runtime.Serialization;

namespace HsuSgProxyTests.Samples.Restful.Models.Rfid.Common;

/// <summary>
/// An object containing memory information for a mask.
/// </summary>
[DataContract]
public record Memory 
{
    /// <summary>
    /// Gets or Sets Bank
    /// </summary>
    [DataMember(Name="bank", EmitDefaultValue=false)]
    public MemoryBank Bank { get; set; }

    /// <summary>
    /// Gets or Sets BitAddress
    /// </summary>
    [DataMember(Name="byte_address", EmitDefaultValue=false)]
    public int? Address { get; set; }

}

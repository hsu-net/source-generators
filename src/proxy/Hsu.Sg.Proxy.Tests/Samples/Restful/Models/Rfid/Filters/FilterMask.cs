using System.Runtime.Serialization;

namespace HsuSgProxyTests.Samples.Restful.Models.Rfid.Filters;

/// <summary>
/// An object containing memory mask information.
/// </summary>
[DataContract]
public record FilterMask {
    /// <summary>
    /// A string representing the memory mask defined in a Hex format.
    /// </summary>
    /// <value>A string representing the memory mask defined in a Hex format.</value>
    [DataMember(Name="value", EmitDefaultValue=false)]
    public string Value { get; set; }

    /// <summary>
    /// Gets or Sets BitLength
    /// </summary>
    [DataMember(Name="bit_length", EmitDefaultValue=false)]
    public int? BitLength { get; set; }
}

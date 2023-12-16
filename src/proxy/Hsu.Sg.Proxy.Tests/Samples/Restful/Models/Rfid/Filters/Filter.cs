using System.Runtime.Serialization;

namespace HsuSgProxyTests.Samples.Restful.Models.Rfid.Filters;

/// <summary>
/// Filter
/// </summary>
[DataContract]
public record Filter
{
    /// <summary>
    /// Gets or Sets Id
    /// </summary>
    [DataMember(Name = "id", EmitDefaultValue = false)]
    public int? Id { get; set; }

    /// <summary>
    /// Gets or Sets Truncate
    /// </summary>
    [DataMember(Name = "truncate", EmitDefaultValue = false)]
    public bool? Truncate { get; set; }

    /// <summary>
    /// Gets or Sets LogicalOperation
    /// </summary>
    [DataMember(Name = "logical_operation", EmitDefaultValue = false)]
    public LogicalOperation LogicalOperation { get; set; }

    /// <summary>
    /// Gets or Sets Negate
    /// </summary>
    [DataMember(Name = "negate", EmitDefaultValue = false)]
    public bool? Negate { get; set; }

    /// <summary>
    /// Gets or Sets Memory
    /// </summary>
    [DataMember(Name = "memory", EmitDefaultValue = false)]
    public FilterMemory Memory { get; set; }

    /// <summary>
    /// Gets or Sets Mask
    /// </summary>
    [DataMember(Name = "mask", EmitDefaultValue = false)]
    public FilterMask Mask { get; set; }
}

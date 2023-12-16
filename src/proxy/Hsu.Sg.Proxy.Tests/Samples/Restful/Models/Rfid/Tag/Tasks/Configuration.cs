using System.Runtime.Serialization;
using HsuSgProxyTests.Samples.Restful.Models.Rfid.Common;

namespace HsuSgProxyTests.Samples.Restful.Models.Rfid.Tag.Tasks;

[DataContract]
public record Configuration
{
    /// <summary>
    /// Gets or Sets Mode
    /// </summary>
    [DataMember(Name = "mode", EmitDefaultValue = false)]
    public Mode Mode { get; set; }

    /// <summary>
    /// Gets or Sets ExecutionType
    /// </summary>
    [DataMember(Name = "execution_type", EmitDefaultValue = false)]
    public ExecutionType ExecutionType { get; set; }

    /// <summary>
    /// Gets or Sets Memory
    /// </summary>
    [DataMember(Name = "memory", EmitDefaultValue = false)]
    public Memory Memory { get; set; }

    /// <summary>
    /// Gets or Sets ByteLength
    /// </summary>
    [DataMember(Name = "byte_length", EmitDefaultValue = false)]
    public int? ByteLength { get; set; }
    
    /// <summary>
    /// Gets or Sets Data for write
    /// </summary>
    [DataMember(Name = "data", EmitDefaultValue = false)]
    public string? Data { get; set; }
}
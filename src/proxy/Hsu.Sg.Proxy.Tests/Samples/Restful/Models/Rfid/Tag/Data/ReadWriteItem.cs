using System.Runtime.Serialization;

namespace HsuSgProxyTests.Samples.Restful.Models.Rfid.Tag.Data;

[DataContract]
public record ReadWriteItem : Item
{
    /// <summary>
    /// Gets or Sets RSSI
    /// </summary>
    [DataMember(Name = "RSSI", EmitDefaultValue = false)]
    public int? RSSI { get; set; }

    /// <summary>
    /// Gets or Sets Data
    /// </summary>
    [DataMember(Name = "data", EmitDefaultValue = false)]
    public string Data { get; set; }
}

/// <summary>
/// An object containing all the data obtained from a execution.
/// </summary>
[DataContract]
public record Item
{
    /// <summary>
    /// Gets or Sets UII
    /// </summary>
    [DataMember(Name = "UII", EmitDefaultValue = false)]
    public string UII { get; set; }

    /// <summary>
    /// Gets or Sets Active
    /// </summary>
    [DataMember(Name = "active", EmitDefaultValue = false)]
    public bool? Active { get; set; }

    /// <summary>
    /// Gets or Sets Time
    /// </summary>
    [DataMember(Name = "time", EmitDefaultValue = false)]
    public int? Time { get; set; }
}
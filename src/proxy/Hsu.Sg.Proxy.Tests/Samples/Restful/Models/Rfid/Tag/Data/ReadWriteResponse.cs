using System.Collections.Generic;
using System.Runtime.Serialization;
using HsuSgProxyTests.Samples.Restful.Models.Rfid.Tag.Data;

namespace HsuSgProxyTests.Samples.Restful.Models.Rfid.Antennas.Tag.Data;

/// <summary>
/// An array containing all antennas with continues reading/writing tag responses.
/// </summary>
[DataContract]
public record ReadWriteResponse : DataBase
{
    /// <summary>
    /// An array of multiple tag read or write responses.
    /// </summary>
    /// <value>An array of multiple tag read or write responses.</value>
    [DataMember(Name = "data", EmitDefaultValue = false)]
    public List<ReadWriteItem> Data { get; set; }
}

/// <summary>
/// An object containing all the data obtained from a execution.
/// </summary>
public abstract record DataBase
{
    /// <summary>
    /// Gets or Sets Id
    /// </summary>
    [DataMember(Name = "id", EmitDefaultValue = false)]
    public int? Id { get; set; }
}

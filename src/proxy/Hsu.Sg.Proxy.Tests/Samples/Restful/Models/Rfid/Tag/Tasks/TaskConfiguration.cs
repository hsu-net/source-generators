using System.Runtime.Serialization;

namespace HsuSgProxyTests.Samples.Restful.Models.Rfid.Tag.Tasks;

/// <summary>
/// 
/// </summary>
[DataContract]
public record TaskConfiguration
{
    /// <summary>
    /// Gets or Sets Id
    /// </summary>
    [DataMember(Name="id", EmitDefaultValue=false)]
    public int? Id { get; set; }
    
    /// <summary>
    /// Gets or Sets Configuration
    /// </summary>
    [DataMember(Name="configuration", EmitDefaultValue=false)]
    public Configuration Configuration { get; set; }
}
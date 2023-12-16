using System.Runtime.Serialization;
using HsuSgProxyTests.Samples.Restful.Models.Rfid.Antennas;
using HsuSgProxyTests.Samples.Restful.Models.Rfid.Common;
using HsuSgProxyTests.Samples.Restful.Models.Rfid.Filters;

namespace HsuSgProxyTests.Samples.Restful.Models.Rfid.Antennas;

public partial record AntennaResponse
{
    /// <summary>
    /// Gets or Sets Id
    /// </summary>
    [DataMember(Name="id", EmitDefaultValue=false)]
    public int? Id { get; set; }

    /// <summary>
    /// Gets or Sets Type
    /// </summary>
    [DataMember(Name="type", EmitDefaultValue=false)]
    public string Type { get; set; }

    /// <summary>
    /// Gets or Sets CountryIdentifier
    /// </summary>
    [DataMember(Name="country_identifier", EmitDefaultValue=false)]
    public CountryIdentifier CountryIdentifier { get; set; }

    /// <summary>
    /// Gets or Sets _Internal
    /// </summary>
    [DataMember(Name="internal", EmitDefaultValue=false)]
    public bool? Internal { get; set; }

    /// <summary>
    /// Gets or Sets Polarization
    /// </summary>
    [DataMember(Name="polarization", EmitDefaultValue=false)]
    public Polarization Polarization { get; set; }

    /// <summary>
    /// Gets or Sets QValue
    /// </summary>
    [DataMember(Name="q_value", EmitDefaultValue=false)]
    public int? QValue { get; set; }

    /// <summary>
    /// Gets or Sets MaximumTransponders
    /// </summary>
    [DataMember(Name="maximum_transponders", EmitDefaultValue=false)]
    public int? MaximumTransponders { get; set; }

    /// <summary>
    /// Gets or Sets TriesAllowed
    /// </summary>
    [DataMember(Name="tries_allowed", EmitDefaultValue=false)]
    public int? TriesAllowed { get; set; }

    /// <summary>
    /// Gets or Sets EnhancedStatus
    /// </summary>
    [DataMember(Name="enhanced_status_5", EmitDefaultValue=false)]
    public int? EnhancedStatus { get; set; }

    /// <summary>
    /// Gets or Sets MemoryBank
    /// </summary>
    [DataMember(Name="memory_bank", EmitDefaultValue=false)]
    public MemoryBank MemoryBank { get; set; }

    /// <summary>
    /// Gets or Sets FilterActivation
    /// </summary>
    [DataMember(Name="filter_activation", EmitDefaultValue=false)]
    public FilterActivation FilterActivation { get; set; }

    /// <summary>
    /// Gets or Sets Filters
    /// </summary>
    [DataMember(Name="filters", EmitDefaultValue=false)]
    public List<Filter> Filters { get; set; }
    
    /// <summary>
    /// Gets or Sets PowerTransmit
    /// </summary>
    [DataMember(Name="power_transmit", EmitDefaultValue=false)]
    public List<int> PowerTransmit { get; set; }
}

/// <summary>
/// </summary>
public partial record AntennaResponse
{
    /// <summary>
    /// A number representing the transmission channels, in accordance with EPC Gen 2 (ISO/IEC 18000-63).
    /// </summary>
    /// <remarks>
    /// Antenna1
    /// Antenna4
    /// Antenna5
    /// Antenna6
    /// Antenna8
    /// Antenna15
    /// </remarks>
    [DataMember(Name="transmission_channels", EmitDefaultValue=false)]
    public List<int> TransmissionChannels { get; set; }
}

public partial record AntennaResponse
{
    /// <summary>
    /// A number representing the number of channels used, in accordance with EPC Gen 2 (ISO/IEC 18000-63).
    /// </summary>
    /// <remarks>
    /// Antenna2
    /// Antenna3
    /// Antenna7
    /// Antenna9
    /// Antenna10
    /// Antenna11
    /// Antenna12
    /// Antenna13
    /// Antenna14
    /// Antenna16
    /// </remarks>
    [DataMember(Name="number_of_channels", EmitDefaultValue=false)]
    public int? NumberOfChannels { get; set; }
}
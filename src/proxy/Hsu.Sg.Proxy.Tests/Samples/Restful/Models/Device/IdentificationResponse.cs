using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace HsuSgProxyTests.Samples.Restful.Models.Device;

/// <summary>
/// An object containing the identification information
/// </summary>
[DataContract]
public record IdentificationResponse
{
    /// <summary>
    /// Gets or Sets Name
    /// </summary>
    [DataMember(Name = "name", EmitDefaultValue = false)]
    public string Name { get; set; }

    /// <summary>
    /// Gets or Sets EdmIndex
    /// </summary>
    [DataMember(Name = "edm_index", EmitDefaultValue = false)]
    public string EdmIndex { get; set; }

    /// <summary>
    /// Gets or Sets EdmNumber
    /// </summary>
    [DataMember(Name = "edm_number", EmitDefaultValue = false)]
    public string EdmNumber { get; set; }

    /// <summary>
    /// Gets or Sets VendorName
    /// </summary>
    [DataMember(Name = "vendor_name", EmitDefaultValue = false)]
    public string VendorName { get; set; }

    /// <summary>
    /// Gets or Sets ProductName
    /// </summary>
    [DataMember(Name = "product_name", EmitDefaultValue = false)]
    public string ProductName { get; set; }

    /// <summary>
    /// Gets or Sets ArticleNumber
    /// </summary>
    [DataMember(Name = "article_number", EmitDefaultValue = false)]
    public string ArticleNumber { get; set; }

    /// <summary>
    /// Gets or Sets SoftwareRevision
    /// </summary>
    [DataMember(Name = "software_revision", EmitDefaultValue = false)]
    public string SoftwareRevision { get; set; }

    /// <summary>
    /// Gets or Sets HardwareRevision
    /// </summary>
    [DataMember(Name = "hardware_revision", EmitDefaultValue = false)]
    public string HardwareRevision { get; set; }

    /// <summary>
    /// Gets or Sets SerialNumber
    /// </summary>
    [DataMember(Name = "serial_number", EmitDefaultValue = false)]
    public string SerialNumber { get; set; }

    /// <summary>
    /// Gets or Sets CompileDate
    /// </summary>
    [DataMember(Name = "compile_date", EmitDefaultValue = false)]
    public string CompileDate { get; set; }

    /// <summary>
    /// Gets or Sets ProductionBatch
    /// </summary>
    [DataMember(Name = "production_batch", EmitDefaultValue = false)]
    public string ProductionBatch { get; set; }

    /// <summary>
    /// Gets or Sets ProductionDate
    /// </summary>
    [DataMember(Name = "production_date", EmitDefaultValue = false)]
    public DateTime? ProductionDate { get; set; }

    /// <summary>
    /// An array containing the components identification information
    /// </summary>
    /// <value>An array containing the components identification information</value>
    [DataMember(Name = "children", EmitDefaultValue = false)]
    public List<IdentificationResponse> Children { get; set; }
}

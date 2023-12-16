using System.ComponentModel;

namespace HsuSgProxyTests.Samples.Restful.Models.Rfid.Filters;

/// <summary>
/// An enum informing the way in which the antenna filter masks are combined.
/// </summary>
[DefaultValue(OR)]
public enum LogicalOperation
{
    OR,
    AND
}

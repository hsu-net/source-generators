using System.ComponentModel;

namespace HsuSgProxyTests.Samples.Restful.Models.Rfid.Common;

/// <summary>
/// An enum representing an execution type of a command.
/// </summary>
[DefaultValue(SINGLE)]
public enum ExecutionType
{
    SINGLE,
    CONTINUOUS
}

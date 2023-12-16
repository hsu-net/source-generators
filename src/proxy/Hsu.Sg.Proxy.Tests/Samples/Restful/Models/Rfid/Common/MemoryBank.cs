using System.ComponentModel;

namespace HsuSgProxyTests.Samples.Restful.Models.Rfid.Common;

/// <summary>
/// An enum containing the memory bank which should be used.
/// </summary>
[DefaultValue(USER)]
public enum MemoryBank
{
    USER,
    UII,
    TID,
    RESERVED
}
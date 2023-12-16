// ReSharper disable CommentTypo
namespace HsuSgProxyTests.Samples.Restful.Models.Rfid.Antennas;

/// <summary>
/// A number representative of the country in which the device is used
/// </summary>
public enum CountryIdentifier
{
    /// <summary>
    /// None
    /// </summary>
    Default,
    /// <summary>
    ///`1` 865 - 868 MHz (ETSI) EU, AZ, BY, IR, MD, OM, SA, ZA, TN, AE, HK
    /// </summary>
    C01,
    /// <summary>
    ///`2` 903 - 927 MHz (FHSS) US, MX
    /// </summary>
    C02,
    /// <summary>
    ///`3` 920 - 925 MHz (FHSS) CN
    /// </summary>
    C03,
    /// <summary>
    ///`4` 865 - 867 MHz (ETSI) IN
    /// </summary>
    C04,
    /// <summary>
    ///`5` 866 - 868 MHz (ETSI) SG, VN
    /// </summary>
    C05,
    /// <summary>
    ///`6` 866 - 868 MHz (ETSI) RU
    /// </summary>
    C06,
    /// <summary>
    ///`7` 915 - 928 MHz (FHSS) BR
    /// </summary>
    C07,
    /// <summary>
    ///`8` 917 - 920 MHz (ETSI) JP
    /// </summary>
    C08,
    /// <summary>
    ///`9` 917 - 920 MHz (FHSS) KR
    /// </summary>
    C09,
    /// <summary>
    ///`10` 920 - 926 MHz (FHSS) AU
    /// </summary>
    C10,
    /// <summary>
    ///`11` 922 - 928 MHz (FHSS) NZ
    /// </summary>
    C11,
    /// <summary>
    ///`12` 920 - 925 MHz (FHSS) TH, HK
    /// </summary>
    C12,
    /// <summary>
    ///`13` 919 - 923 MHz (FHSS) MY
    /// </summary>
    C13,
    /// <summary>
    ///`14` 920 - 925 MHz (FHSS) SG, VN
    /// </summary>
    C14,
    /// <summary>
    ///`15` 868 - 868 MHz (ETSI) MA
    /// </summary>
    C15,
    /// <summary>
    ///`16` 923 - 925 MHz (FHSS) ID
    /// </summary>
    C16
}

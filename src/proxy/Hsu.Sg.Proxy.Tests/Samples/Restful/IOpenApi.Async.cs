using HsuSgProxyTests.Samples.Restful.Models.Authentication;
using HsuSgProxyTests.Samples.Restful.Models.Device;
using HsuSgProxyTests.Samples.Restful.Models.Rfid.Tag.Tasks;
using HsuSgProxyTests.Samples.Restful.Models.Rfid.Antennas.Tag.Data;
using HsuSgProxyTests.Samples.Restful.Models.System;
using Hsu.Sg.Proxy;
using Hsu.Sg.Sync;
using HsuSgProxyTests.Samples.Restful.Models.Rfid.Antennas;
using Refit;

namespace HsuSgProxyTests.Samples.Restful;

/// <summary>
/// Authentication endpoints
/// </summary>
[Headers("Authorization: Bearer")]
[Sync(Attribute = true, AttributeIncludes = new []{"Proxy"})]
[Proxy(Abstract = true,Attribute = false,Interfaces = new[] {nameof(IOpenApi)})]
public partial interface IOpenApi
{
    /// <summary>
    /// Log in to the system
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [Post("/auth/login")]
    [Headers("Authorization")]
    Task<IApiResponse<LoginResponse>> LoginAsync([Body(BodySerializationMethod.Serialized)] LoginRequest request);

    /// <summary>
    /// Log out from the system
    /// </summary>
    /// <returns></returns>
    [Post("/auth/logout")]
    Task<IApiResponse> LogoutAsync();
}

/// <summary>
/// Device identification endpoints
/// </summary>
public partial interface IOpenApi
{
    /// <summary>
    /// Returns the device identification
    /// </summary>
    /// <returns></returns>
    [Get("/identification")]
    Task<IApiResponse<IdentificationResponse>> IdentificationAsync();
}

/// <summary>
/// System information endpoints
/// </summary>
public partial interface IOpenApi
{
    /// <summary>
    /// Get all network interfaces 
    /// </summary>
    /// <returns></returns>
    [Get("/system/network/interfaces")]
    Task<IApiResponse<NetworkInterfaceResponse[]>> NetworkInterfacesAsync();
}

/// <summary>
/// RFID Antennas endpoints
/// </summary>
public partial interface IOpenApi
{
    /// <summary>
    /// Return all antenna configurations 
    /// </summary>
    [Get("/rfid/antennas")]
    Task<IApiResponse<AntennaResponse[]>> AntennasAsync();

    /// <summary>
    /// Get antenna by ID
    /// </summary>
    [Get("/rfid/antennas/{id}")]
    Task<IApiResponse<AntennaResponse>> AntennaAsync(string id);
}

/// <summary>
/// RFID Tag data endpoints
/// </summary>
public partial interface IOpenApi
{
    /// <summary>
    /// Get tag data
    /// </summary>
    [Get("/rfid/tag/data")]
    Task<IApiResponse<ReadWriteResponse[]>> DataAsync();

    /// <summary>
    /// Get tag data by antenna ID
    /// </summary>
    [Get("/rfid/tag/data/{id}")]
    Task<IApiResponse<ReadWriteResponse>> DataAsync(string id);
}

/// <summary>
/// RFID Tag tasks action endpoints
/// </summary>
public partial interface IOpenApi
{
    /// <summary>
    /// Start all tag tasks
    /// </summary>
    [Post("/rfid/tag/tasks/start")]
    Task<IApiResponse<ReadWriteResponse[]>> StartTasksAsync();

    /// <summary>
    /// Start tag task by antenna ID
    /// </summary>
    [Post("/rfid/tag/tasks/{id}/start")]
    Task<IApiResponse<ReadWriteResponse>> StartTaskAsync(string id);

    /// <summary>
    /// Stop all tag tasks
    /// </summary>
    [Post("/rfid/tag/tasks/stop")]
    Task<IApiResponse> StopTasksAsync();

    /// <summary>
    /// Stop tag task by antenna ID
    /// </summary>
    [Post("/rfid/tag/tasks/{id}/stop")]
    Task<IApiResponse> StopTaskAsync(string id);
}

/// <summary>
/// RFID Tag tasks configuration endpoints
/// </summary>
public partial interface IOpenApi
{
    /// <summary>
    /// Get the current active tag task configuration for each antenna
    /// </summary>
    [Get("/rfid/tag/active-tasks")]
    Task<IApiResponse<TaskConfiguration[]>> ActiveTasksAsync();

    /// <summary>
    /// Get the current active tag task configuration by antenna ID
    /// </summary>
    [Get("/rfid/tag/active-tasks/{id}")]
    Task<IApiResponse<TaskConfiguration>> ActiveTaskAsync(string id);

    /// <summary>
    /// Get the tag task configuration for each antenna
    /// </summary>
    [Get("/rfid/tag/tasks")]
    Task<IApiResponse<TaskConfiguration[]>> GetTasksAsync();

    /// <summary>
    /// Get tag task configuration by antenna ID
    /// </summary>
    [Get("/rfid/tag/tasks/{id}")]
    Task<IApiResponse<TaskConfiguration>> GetTaskAsync(string id);

    /// <summary>
    /// Update the tag task configuration for each antenna.
    /// </summary>
    [Put("/rfid/tag/tasks")]
    Task<IApiResponse> UpdateTasksAsync([Body(BodySerializationMethod.Serialized)] IEnumerable<TaskConfiguration> requests);

    /// <summary>
    /// Update tag task configuration by antenna ID
    /// </summary>
    [Put("/rfid/tag/tasks/{id}")]
    Task<IApiResponse> UpdateTaskAsync(string id, [Body(BodySerializationMethod.Serialized)] TaskConfiguration request);
}

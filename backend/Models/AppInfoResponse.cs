namespace backend.Models;

public sealed record AppInfoResponse(
    string Application,
    string Environment,
    string Version);

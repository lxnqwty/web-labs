using backend.Models;

namespace backend.Services;

public sealed class AppInfoService(IHostEnvironment environment) : IAppInfoService
{
    public AppInfoResponse GetInfo()
    {
        return new AppInfoResponse(
            Application: "Student Profile API",
            Environment: environment.EnvironmentName,
            Version: "ЛР7");
    }
}

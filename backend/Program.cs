using backend.Services;

var builder = WebApplication.CreateBuilder(args);

// Регистрация MVC-контроллеров и сервисов Swagger.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Сервис приложения регистрируется в DI-контейнере.
builder.Services.AddSingleton<IAppInfoService, AppInfoService>();

var app = builder.Build();

// Swagger доступен в режиме разработки.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

// Служебные маршруты каркаса ЛР7.
app.MapGet("/health", () => Results.Text("ok"))
    .WithName("Health");

app.MapGet("/info", (IAppInfoService infoService) =>
        Results.Ok(infoService.GetInfo()))
    .WithName("GetInfo");

app.Run();

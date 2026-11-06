# Отчёт по лабораторной работе №10

**Тема:** EF Core: сервер пишет в базу данных.

## Цель работы

Подключить ASP.NET Core Web API к PostgreSQL через Entity Framework Core и заменить временное хранение REST-записей в памяти на постоянное хранение в реляционной базе данных.

## Выполненная работа

1. Добавлены пакеты `Npgsql.EntityFrameworkCore.PostgreSQL` и `Microsoft.EntityFrameworkCore.Design` версии 8.0.11 для .NET 8.
2. Создан `AppDbContext` с наборами `Activities`, `Categories` и `Users`.
3. Настроено отображение сущностей на таблицы ЛР9 (`activities`, `categories`, `users`) и столбцы PostgreSQL в формате `snake_case`.
4. Настроены первичные и внешние ключи, индексы, ограничения длины текста и допустимых статусов.
5. Создана миграция `InitialEfCore`, создающая схему и базовые записи пользователей и категорий.
6. `TicketService` переведён с коллекции в памяти на операции EF Core: чтение, создание, назначение исполнителя и смена статуса выполняются через `AppDbContext`.
7. В DI зарегистрирован контекст с провайдером Npgsql и scoped-сервис заявок. Проверка `/health` теперь дополнительно проверяет доступность PostgreSQL.
8. Строка подключения и ID пользователя-автора задаются конфигурацией. Пароль не следует хранить в Git; используйте User Secrets или переменную окружения.

## Соответствие модели ЛР9

API продолжает использовать REST-маршруты `/api/tickets`, но записи физически сохраняются в таблице `activities`, как определено моделью проекта. Категории и пользователи находятся в связанных таблицах. При создании запись получает статус `New`, дату создания и автора (настраиваемый `Tickets:DefaultCreatedByUserId`, по умолчанию 1). Исполнитель может отсутствовать до назначения.

## Структура

```text
backend/
├── Data/AppDbContext.cs
├── Migrations/202611060001_InitialEfCore.cs
├── Models/
│   ├── Ticket.cs
│   ├── Category.cs
│   └── User.cs
├── Services/TicketService.cs
├── Program.cs
└── appsettings.json
docs/reports/
└── lab-10.md
```

## Настройка и запуск

Создайте отдельную базу данных для EF Core (не используйте базу, в которой уже вручную выполнена схема ЛР9: миграция создаёт таблицы сама):

```sql
CREATE DATABASE course_profile_app;
```

Задайте пароль PostgreSQL через переменную окружения, не записывая его в Git:

**PowerShell**
```powershell
$env:ConnectionStrings__DefaultConnection="Host=localhost;Port=5432;Database=course_profile_app;Username=postgres;Password=ВАШ_ПАРОЛЬ"
```

**Bash**
```bash
export ConnectionStrings__DefaultConnection="Host=localhost;Port=5432;Database=course_profile_app;Username=postgres;Password=ВАШ_ПАРОЛЬ"
```

Затем из каталога `backend`:

```bash
dotnet restore
dotnet tool install --global dotnet-ef --version 8.0.11
dotnet ef database update
dotnet run
```

Если инструмент `dotnet-ef` уже установлен, повторная установка не нужна. Для локальной разработки вместо переменной окружения можно использовать `dotnet user-secrets`.

## Проверка

1. Убедиться, что `GET /health` возвращает `ok` при доступной БД.
2. Создать запись через `POST /api/tickets` с существующим `categoryId`.
3. Получить список через `GET /api/tickets` и убедиться, что запись присутствует.
4. Перезапустить backend и повторить GET: созданная запись должна сохраниться.
5. Назначить существующего пользователя через `PATCH /api/tickets/{id}/assignee`, затем проверить смену статуса.
6. Проверить строки непосредственно в PostgreSQL: `SELECT * FROM activities ORDER BY id DESC;`.

**Ограничение проверки:** исходный код и конфигурация подготовлены, но сборка .NET, применение миграции к реальному экземпляру PostgreSQL и HTTP-сценарии в этой среде не запускались. Перед сдачей необходимо выполнить шаги проверки и приложить фактические ответы API или скриншоты pgAdmin.

## Вывод

REST API переведён на Entity Framework Core и PostgreSQL: данные создаются, читаются и изменяются через контекст EF Core, а не через временный список в памяти. Это обеспечивает сохранение записей после перезапуска сервера.

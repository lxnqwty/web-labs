# web-labs

ТРИС-1-23 Карабаев Андрей Маратович

Учебный проект: цифровой профиль студента для учёта учебной, научной и проектной активности.

**Стек:** Vue 3 + Vite + Vue Router, ASP.NET Core Web API (.NET 8), PostgreSQL, Entity Framework Core 8.

## Запуск frontend

```bash
cd frontend
npm install
npm run dev
```

Откройте `http://localhost:5173`. Для production-сборки: `npm run build`.

## Запуск backend

Требуется .NET SDK 8.0 и PostgreSQL.

```bash
cd backend
dotnet restore
dotnet run
```

Backend доступен по адресу `http://localhost:5000`. Swagger доступен в Development: `/swagger`.

## PostgreSQL и EF Core (ЛР10)

Создайте отдельную базу приложения (не ту, где выполнялись SQL-скрипты ЛР9):

```sql
CREATE DATABASE course_profile_app;
```

Задайте строку подключения в переменной окружения `ConnectionStrings__DefaultConnection`. Пример для PowerShell:

```powershell
$env:ConnectionStrings__DefaultConnection="Host=localhost;Port=5432;Database=course_profile_app;Username=postgres;Password=ВАШ_ПАРОЛЬ"
```

Примените миграции из каталога `backend`:

```bash
dotnet restore
dotnet tool install --global dotnet-ef --version 8.0.11
dotnet ef database update
dotnet run
```

При необходимости используйте `dotnet user-secrets` для локальной строки подключения. Не коммитьте пароли и другие секреты. Миграция создаёт таблицы `users`, `categories`, `activities` и начальные записи пользователей и категорий. API хранит активности в `activities`; после перезапуска сервера данные остаются в PostgreSQL.

Автор новой записи задаётся параметром `Tickets:DefaultCreatedByUserId` (по умолчанию 1), поэтому в БД должен существовать такой пользователь. Категория, передаваемая в `categoryId`, также должна существовать.

## PostgreSQL и SQL (ЛР9)

SQL-схема, учебные данные и запросы находятся в `db/schema.sql`, `db/seed.sql`, `db/queries.sql`. Для самостоятельной работы ЛР9 создайте отдельную базу и выполните:

```bash
createdb course_profile
psql -d course_profile -f db/schema.sql
psql -d course_profile -f db/seed.sql
psql -d course_profile -f db/queries.sql
```

Не используйте эту базу для первого запуска EF Core: миграции ЛР10 рассчитаны на пустую `course_profile_app`.

## REST API (ЛР8)

| Метод | Путь | Назначение |
|---|---|---|
| POST | `/api/tickets` | Создать запись |
| GET | `/api/tickets` | Получить список |
| GET | `/api/tickets/{id}` | Получить одну запись |
| PATCH | `/api/tickets/{id}/assignee` | Назначить исполнителя |
| PATCH | `/api/tickets/{id}/status` | Изменить статус |

Контракт: [docs/api-contract.md](docs/api-contract.md). Коллекция Postman: [docs/api-collection/tickets.postman_collection.json](docs/api-collection/tickets.postman_collection.json).

## Документация

- [ЛР8 — REST API заявок](docs/reports/lab-08.md)
- [ЛР9 — PostgreSQL и SQL](docs/reports/lab-09.md)
- [ЛР10 — EF Core: сервер пишет в базу](docs/reports/lab-10.md)
- [Модель данных](docs/data-model.md)
- [Контракт API](docs/api-contract.md)
- [Архитектура](docs/architecture.md)

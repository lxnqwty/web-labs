# web-labs

ТРИС-1-23 Карабаев Андрей Маратович

Учебный проект: цифровой профиль студента для учёта учебной, научной и проектной активности.

**Стек:** Vue 3 + Vite + Vue Router (клиент), ASP.NET Core Web API (.NET 8, сервер), PostgreSQL (планируемая БД).

## Запуск frontend

Требуются Node.js LTS и npm.

```bash
cd frontend
npm install
npm run dev
```

Откройте `http://localhost:5173`. Для production-сборки: `npm run build`.

## Запуск backend

Требуется .NET SDK 8.0 или новее.

```bash
cd backend
dotnet restore
dotnet run
```

Backend доступен по адресу `http://localhost:5000`.

- Swagger: [http://localhost:5000/swagger](http://localhost:5000/swagger) (Development).
- Проверка процесса: [http://localhost:5000/health](http://localhost:5000/health).
- Информация о приложении: [http://localhost:5000/info](http://localhost:5000/info).

## REST API (ЛР8)

| Метод | Путь | Назначение |
|---|---|---|
| POST | `/api/tickets` | Создать заявку |
| GET | `/api/tickets` | Получить список |
| GET | `/api/tickets/{id}` | Получить одну заявку |
| PATCH | `/api/tickets/{id}/assignee` | Назначить исполнителя |
| PATCH | `/api/tickets/{id}/status` | Изменить статус |

Контракт и примеры: [docs/api-contract.md](docs/api-contract.md). Коллекция Postman: импортируйте [docs/api-collection/tickets.postman_collection.json](docs/api-collection/tickets.postman_collection.json). В коллекции используется базовый URL `http://localhost:5000`.

При переходе в `InProgress` передавайте `X-User-Id`, совпадающий с `assigneeUserId`. Это учебная имитация идентификатора пользователя, не полноценная аутентификация.

Frontend по умолчанию обращается к настоящему backend. Чтобы включить mock-режим, задайте `VITE_USE_MOCK=true` в `frontend/.env`. При необходимости базовый адрес API можно переопределить через `VITE_API_BASE_URL`. CORS настроен для локального Vite-сервера на порту 5173.

## Ограничения

Заявки ЛР8 хранятся в памяти backend и сбрасываются после перезапуска процесса. PostgreSQL и постоянное хранение будут добавлены позднее. Маршрут `/health` проверяет только доступность процесса, а не готовность бизнес-операций.

## Документация

- [ЛР1 — требования](docs/reports/lab-01.md)
- [ЛР3 — каркас и три экрана](docs/reports/lab-03.md)
- [ЛР4 — компоненты и маршруты](docs/reports/lab-04.md)
- [ЛР5 — форма и список в памяти](docs/reports/lab-05.md)
- [ЛР6 — клиент API, mock и состояния UI](docs/reports/lab-06.md)
- [ЛР7 — каркас ASP.NET Core Web API](docs/reports/lab-07.md)
- [ЛР8 — REST API заявок](docs/reports/lab-08.md)
- [Модель данных](docs/data-model.md)
- [Контракт API](docs/api-contract.md)
- [Архитектура](docs/architecture.md)

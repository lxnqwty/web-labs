# web-labs

ТРИС-1-23 Карабаев Андрей Маратович

Учебный проект: цифровой профиль студента для учёта учебной, научной и проектной активности.

**Стек:** Vue 3 + Vite + Vue Router (клиент), ASP.NET Core Web API (сервер), PostgreSQL (планируемая БД).

### Запуск frontend

Требуются Node.js LTS и npm.

```bash
cd frontend
npm install
npm run dev
```

Откройте `http://localhost:5173`. Каждый маршрут можно проверить прямым вводом адреса в браузере. Для production-сборки: `npm run build`.

### Запуск backend

Требуется .NET SDK 8.0 или новее.

```bash
cd backend
dotnet restore
dotnet run
```

После запуска backend доступен по адресу `http://localhost:5000`.

- Swagger: [http://localhost:5000/swagger](http://localhost:5000/swagger) (в режиме Development).
- Проверка доступности процесса: [http://localhost:5000/health](http://localhost:5000/health).
- Информация о приложении: [http://localhost:5000/info](http://localhost:5000/info).

Маршрут `/health` возвращает `200 OK` и короткий ответ `ok`. Он проверяет только то, что процесс запущен и маршрут отвечает. Это **не** проверка создания заявки и не подтверждение готовности CRUD. Создание заявок появится в ЛР8.

### Ограничения

В клиентской демонстрационной версии записи пока хранятся в памяти приложения и сбрасываются после перезагрузки страницы. Backend в рамках ЛР7 — только каркас: он пока не реализует создание, чтение или сохранение заявок. Постоянное хранение в PostgreSQL и подключение Vue к API будут добавляться в следующих работах.

## Документация

- [ЛР1 — требования](docs/reports/lab-01.md)
- [ЛР3 — каркас и три экрана](docs/reports/lab-03.md)
- [ЛР4 — компоненты и маршруты](docs/reports/lab-04.md)
- [ЛР5 — форма и список в памяти](docs/reports/lab-05.md)
- [ЛР6 — клиент API, mock и состояния UI](docs/reports/lab-06.md)
- [ЛР7 — каркас ASP.NET Core Web API](docs/reports/lab-07.md)
- [Модель данных](docs/data-model.md)
- [Контракт API](docs/api-contract.md)
- [Архитектура](docs/architecture.md)

# Лабораторная работа №4 — Vue: компоненты и маршруты

## Реализованный результат

Клиент разделён на компоненты и страницы. Для навигации подключён `vue-router` и используется `createWebHistory`.

## Структура

```text
frontend/src/
├── main.js
├── App.vue
├── router.js
├── ticketsStore.js
├── assets/main.css
├── components/
│   ├── AppHeader.vue
│   └── TicketTable.vue
└── pages/
    ├── TicketList.vue
    ├── TicketNew.vue
    └── Login.vue
```

## Маршруты

| URL | Экран |
|---|---|
| `/` | Список активностей |
| `/new` | Создание активности |
| `/login` | Вход |

Маршруты Vue выбирают экран и не являются HTTP-запросами к серверному API.

## Компоненты и состояние

- `App.vue` содержит общую оболочку, постоянный `AppHeader` и `<router-view />`.
- `AppHeader.vue` содержит меню на `<router-link>` и отображается на всех страницах.
- `TicketTable.vue` получает массив через обязательный prop `items`, не создаёт собственный массив и не копирует props.
- `TicketList.vue` передаёт данные через `<TicketTable :items="visibleActivities" />`.
- Единственный общий реактивный массив расположен в `ticketsStore.js`; список и форма импортируют один и тот же объект.

## Согласование с ЛР2

В проекте используется сущность `Activity`, справочник `Category` и поле `categoryId`, как определено в `docs/data-model.md` и `docs/api-contract.md`. Имя компонента `TicketTable.vue` оставлено по структуре методички. Статусы: `New`, `InProgress`, `Closed`, `Cancelled`.

## Проверка

```bash
cd frontend
npm install
npm run dev
```

Вручную введите адреса:
- `http://localhost:5173/`
- `http://localhost:5173/new`
- `http://localhost:5173/login`

Обновите страницы на `/new` и `/login`: Vite должен вернуть приложение, а роутер — нужную страницу.

## Ограничения

Данные находятся в памяти приложения и сбрасываются после перезагрузки. Серверное API, PostgreSQL и настоящая авторизация в ЛР4 не реализуются.

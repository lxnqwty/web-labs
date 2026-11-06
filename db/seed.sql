-- ЛР9: демонстрационные данные. Повторный запуск очищает только данные этих трёх таблиц.
TRUNCATE TABLE activities, categories, users RESTART IDENTITY CASCADE;

INSERT INTO users (id, full_name, login, role) VALUES
    (1, 'Андрей Карабаев', 'andrey.student', 'Student'),
    (2, 'Иван Петров', 'ivan.reviewer', 'Reviewer'),
    (3, 'Мария Сидорова', 'maria.manager', 'Manager');

INSERT INTO categories (id, name) VALUES
    (1, 'Учебная деятельность'),
    (2, 'Научная деятельность'),
    (3, 'Проектная деятельность');

INSERT INTO activities
    (id, number, title, description, category_id, status, created_at, created_by_user_id, assignee_user_id)
VALUES
    (1, 1001, 'Курсовой веб-проект', 'Разработка и документирование учебного веб-приложения.', 1, 'New',        now() - interval '4 days', 1, NULL),
    (2, 1002, 'Научный доклад',      'Подготовка доклада по результатам исследования.',       2, 'InProgress', now() - interval '3 days', 1, 2),
    (3, 1003, 'Командный сервис',    'Создание прототипа цифрового сервиса для студентов.',   3, 'Closed',     now() - interval '2 days', 1, 2),
    (4, 1004, 'Анализ публикаций',   'Обзор источников для научно-исследовательской работы.', 2, 'Cancelled',  now() - interval '1 day',  1, 3);

-- Синхронизируем последовательности после явной установки учебных id и number.
SELECT setval(pg_get_serial_sequence('users', 'id'), (SELECT max(id) FROM users));
SELECT setval(pg_get_serial_sequence('categories', 'id'), (SELECT max(id) FROM categories));
SELECT setval(pg_get_serial_sequence('activities', 'id'), (SELECT max(id) FROM activities));
SELECT setval(pg_get_serial_sequence('activities', 'number'), (SELECT max(number) FROM activities));
